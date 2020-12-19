using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Infrastructure;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "SnippySettings")]
    public class GetSnippySettingsCmdlet : CmdletBase
    {
        protected override void Run()
        {
            var options = SnippyOptions.Instance.Value;
            WriteObject(options);
        }
    }
}