using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using Snippy.Infrastructure;

namespace Snippy.Services
{
    public class WorkspaceNameCompleter : IArgumentCompleter
    {
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            var organizer = new SnippetOrganizer(SnippyOptions.Instance.Value, FileAssociations.Instance.Value);
            var files = organizer.GetAllWorkspaceFiles().Select(Path.GetFileNameWithoutExtension);
            return string.IsNullOrEmpty(wordToComplete)
                ? files.Select(x => new CompletionResult(x))
                : files.Where(x => x.StartsWith(wordToComplete)).Select(x => new CompletionResult(x));
        }
    }
}