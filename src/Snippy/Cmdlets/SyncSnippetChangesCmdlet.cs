using System.Management.Automation;
using JetBrains.Annotations;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsData.Sync, "SnippetChanges")]
    public class SyncSnippetChangesCmdlet : CmdletBase
    {
        [Parameter(Position = 0)]
        public string Message { get; set; }

        protected override void Run()
        {
            CommitAndPush(Message);
        }
    }
}