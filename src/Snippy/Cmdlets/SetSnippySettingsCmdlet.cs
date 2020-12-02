using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Infrastructure;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Set, "SnippySettings")]
    public class SetSnippySettingsCmdlet : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string SnippetPath { get; set; }

        [Parameter(Mandatory = true)]
        public string WorkspacePath { get; set; }

        protected override void ProcessRecord()
        {
            var options = SnippyOptions.Instance.Value;
            options.SnippetPath = SnippetPath;
            options.WorkspacePath = WorkspacePath;
            options.Save();
        }
    }
}