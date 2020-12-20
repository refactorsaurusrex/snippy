using System.IO;
using System.Linq;
using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Infrastructure;
using Snippy.Models;
using Snippy.Services;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsData.Update, "SnippetWorkspaces")]
    public class UpdateSnippetWorkspacesCmdlet : CmdletBase
    {
        [Parameter(ParameterSetName = "All")]
        public SwitchParameter All { get; set; }

        [Parameter(ParameterSetName = "Workspaces")]
        [ArgumentCompleter(typeof(WorkspaceNameCompleter))]
        public string[] Workspaces { get; set; }

        [Parameter]
        public OrderBy OrderBy { get; set; } = OrderBy.Created;

        [Parameter]
        public SortDirection SortDirection { get; set; } = SortDirection.Descending;

        [Parameter]
        public SwitchParameter ResetSettings { get; set; }

        [Parameter]
        public SwitchParameter Sync { get; set; }

        protected override void Run()
        {
            var manifest = Manifest.Load(Options.WorkspacePath);
            var organizer = new SnippetOrganizer(Options, FileAssociations);

            if (All)
            {
                organizer.UpdateAllWorkspaces(manifest.Definitions, ResetSettings, OrderBy, SortDirection);
            }
            else
            {
                var workspaceFileNames = Workspaces.Select(x => Path.ChangeExtension(x, Constants.WorkspaceFileExtension)).ToList();
                organizer.UpdateWorkspaces(manifest.Definitions, workspaceFileNames, ResetSettings, OrderBy, SortDirection);
            }

            if (Sync || Options.AutoSync)
                CommitAndPush("Update workspaces");
        }
    }
}