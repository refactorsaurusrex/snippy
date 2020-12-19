using System.Management.Automation;
using JetBrains.Annotations;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "SnippetChanges")]
    public class GetSnippetChangesCmdlet : CmdletBase
    {
        protected override void Run()
        {
            using var ps = PowerShell.Create();
            var getRoot = @$"cd {Options.SnippetPath}; git rev-parse --show-toplevel;";
            var response = ps.AddScript(getRoot).Invoke();

            if (ps.HadErrors)
            {
                WriteObject(ps.Streams.Error);
                return;
            }

            var root = response[0].BaseObject as string;
            var path = root ?? Options.SnippetPath;
            var getStatus = @$"cd {path}; git status;";
            var status = ps.AddScript(getStatus).Invoke();
            WriteObject(status);
        }
    }
}