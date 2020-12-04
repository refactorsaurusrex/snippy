using System.Management.Automation;
using JetBrains.Annotations;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Push, "SnippetChanges")]
    public class PushSnippetChangesCmdlet : CmdletBase
    {
        [Parameter(Position = 0)]
        public string Message { get; set; }

        protected override void Run()
        {
            CommitAndPush(Message);
        }
    }
}