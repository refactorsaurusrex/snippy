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
            var script = @$"cd {Options.SnippetPath}; git status;";
            using var ps = PowerShell.Create();
            var result = ps.AddScript(script).Invoke();
            WriteObject(result);
        }
    }
}