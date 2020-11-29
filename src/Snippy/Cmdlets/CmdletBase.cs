using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Reflection;
using AsyncProgressReporter;

namespace Snippy.Cmdlets
{
    public abstract class CmdletBase : AsyncProgressPSCmdlet
    {
        private readonly Lazy<string> _assemblyName = new Lazy<string>(() => Assembly.GetExecutingAssembly().FullName);

        /// <summary>
        /// The statement should be in the form of "You are about to _______________________."
        /// Fill in the blank with the scary thing the user is about to do.
        /// </summary>
        /// <returns>True, if the user wants to continue. If false, the command should exit immediately.</returns>
        protected bool AreYouSure(string statement, bool defaultToYes = false, bool isDryRun = false)
        {
            var dryRun = isDryRun ? "DRY RUN: " : "";
            var result = Choice($"{dryRun}You are about to {statement}.", "Are you sure you want to continue?", defaultToYes ? 0 : 1, "&Yes", "&No");
            return result == 0;
        }

        /// <summary>
        /// Sends an informational message to the pipeline.
        /// </summary>
        /// <param name="message"></param>
        protected void WriteInformation(string message) => WriteInformation(new InformationRecord(message, _assemblyName.Value));

        /// <summary>
        /// Writes a non-terminating error message to the host.
        /// </summary>
        protected void WriteNonTerminatingError(string message) => WriteHost($"ERROR: {message}", ConsoleColor.Red);

        /// <summary>
        /// Prints a message to the PowerShell host.
        /// </summary>
        protected void WriteHost(string message, bool lineBreak = true)
        {
            if (lineBreak)
                Host.UI.WriteLine(message);
            else
                Host.UI.Write(message);
        }

        /// <summary>
        /// Prints a message to the PowerShell host with the specified fore- and background colors.
        /// </summary>
        protected void WriteHost(string message, ConsoleColor backgroundColor, ConsoleColor foregroundColor) =>
            Host.UI.WriteLine(foregroundColor, backgroundColor, message);

        /// <summary>
        /// Prints a message to the PowerShell host with the specified foreground color.
        /// </summary>
        protected void WriteHost(string message, ConsoleColor foregroundColor) =>
            Host.UI.WriteLine(foregroundColor, Host.UI.RawUI.BackgroundColor, message);

        /// <summary>
        /// Prints a message to the PowerShell host with the fore- and background colors inverted.
        /// </summary>
        protected void WriteHostInverted(string message) => Host.UI.WriteLine(Host.UI.RawUI.BackgroundColor, Host.UI.RawUI.ForegroundColor, message);

        /// <summary>
        /// Asks the user a yes or no question and returns true if the user selects yes, or false for no.
        /// </summary>
        /// <param name="question">A question to ask the user. Include a question mark at the end.</param>
        protected bool YesOrNo(string question)
        {
            KeyInfo key;
            do
            {
                Host.UI.Write(Host.UI.RawUI.BackgroundColor, Host.UI.RawUI.ForegroundColor, $" {question} (y/n) ");
                key = Host.UI.RawUI.ReadKey();
                Host.UI.WriteLine();
            } while (key.Character != 'y' && key.Character != 'n');

            return key.Character == 'y';
        }

        /// <summary>
        /// Presents the user with an ordered list of options to choose from and returns the index of the selection.
        /// </summary>
        /// <param name="caption">The title to display.</param>
        /// <param name="message">The message body to display. </param>
        /// <param name="defaultChoice">The index of the value in 'choices' which should be the default.</param>
        /// <param name="choices">A list of strings which represent choices. Insert an ampersand before a single letter of
        /// each item to be used as a shortcut key. Example: ("&amp;one", &amp;two", "t&amp;three"), where 'O' would be
        /// the shortcut for "one", 'T' for "two", and 'H' for "three".</param>
        protected int Choice(string caption, string message, int defaultChoice, params string[] choices)
        {
            var choiceDescriptions = choices.Select(c => new ChoiceDescription(c)).ToList();
            return Host.UI.PromptForChoice(caption, message, new Collection<ChoiceDescription>(choiceDescriptions), defaultChoice);
        }

        protected void WriteMassiveWarning(string text)
        {
            const string warning = @"
 __          __              _             _ 
 \ \        / /             (_)           | |
  \ \  /\  / /_ _ _ __ _ __  _ _ __   __ _| |
   \ \/  \/ / _` | '__| '_ \| | '_ \ / _` | |
    \  /\  / (_| | |  | | | | | | | | (_| |_|
     \/  \/ \__,_|_|  |_| |_|_|_| |_|\__, (_)
                                      __/ |  
                                     |___/   ";
            const int halfWarningWidth = 23;
            var padding = (Console.WindowWidth / 2) - halfWarningWidth;
            var block = new string('=', Console.WindowWidth);
            WriteHost(block, ConsoleColor.Yellow);

            foreach (var line in warning.Split(Environment.NewLine))
                WriteHost(line.PadLeft(line.Length + padding), ConsoleColor.Yellow);

            WriteHost(block + Environment.NewLine, ConsoleColor.Yellow);
            WriteHost(text, ConsoleColor.Yellow);
        }

        protected sealed override void ProcessRecord() => Run();

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Options = SnippyOptions.Instance.Value;
            if (!Options.IsValid())
                throw new PSInvalidOperationException("Invalid settings detected. Please run 'Set-SnippySettings' to set correct settings and then try again.");

            FileAssociations = Snippy.FileAssociations.Instance.Value;
        }

        protected abstract void Run();

        protected void WriteHeader(string title, ConsoleColor foregroundColor = ConsoleColor.Green)
        {
            var width = Host.UI.RawUI.WindowSize.Width - 3;
            var backgroundColor = Host.UI.RawUI.BackgroundColor;

            WriteHost(string.Empty);
            WriteHost(new string('=', width), backgroundColor, foregroundColor);
            WriteHost(string.Empty);
            WriteHost(title.PadLeft(3 + title.Length), backgroundColor, foregroundColor);
            WriteHost(string.Empty);
            WriteHost(new string('=', width), backgroundColor, foregroundColor);
            WriteHost(string.Empty);
        }

        private protected ISnippyOptions Options { get; private set; }

        private protected IFileAssociations FileAssociations { get; private set; }
    }
}
