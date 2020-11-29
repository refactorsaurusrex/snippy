using System.Management.Automation;
using JetBrains.Annotations;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Set, "SnippySettings")]
    public class SetSnippySettingsCmdlet : CmdletBase
    {
        [Parameter(Mandatory = true)]
        public string SnippetPath { get; set; }

        [Parameter(Mandatory = true)]
        public string WorkspacePath { get; set; }

        protected override void Run()
        {
            var options = SnippyOptions.Instance.Value;
            options.SnippetPath = SnippetPath;
            options.WorkspacePath = WorkspacePath;
            options.Save();
        }
    }
}