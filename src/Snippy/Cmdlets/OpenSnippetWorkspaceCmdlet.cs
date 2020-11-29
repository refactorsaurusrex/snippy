using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Models;

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
            var path = Path.Combine(options.WorkspacePath, Name);
            path.Run();
        }
    }
}