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
            var root = ps.AddScript(getRoot).Invoke()[0].BaseObject as string;
            var path = root ?? Options.SnippetPath;
            var getStatus = @$"cd {path}; git status;";
            var status = ps.AddScript(getStatus).Invoke();
            WriteObject(status);
        }
    }
}