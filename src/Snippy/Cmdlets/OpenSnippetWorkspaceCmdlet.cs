using System.IO;
using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Infrastructure;
using Snippy.Services;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Open, "SnippetWorkspace")]
    public class OpenSnippetWorkspaceCmdlet : CmdletBase
    {
        [ArgumentCompleter(typeof(WorkspaceNameCompleter))]
        [Parameter(Mandatory = true, Position = 0)]
        public string Name { get; set; }

        protected override void Run()
        {
            var options = SnippyOptions.Instance.Value;
            var name = Path.ChangeExtension(Name, Constants.WorkspaceFileExtension);
            var path = Path.Combine(options.WorkspacePath, name);
            path.Run();
        }
    }
}