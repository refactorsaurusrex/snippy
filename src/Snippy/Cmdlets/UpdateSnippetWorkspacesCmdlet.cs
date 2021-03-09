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
        public WorkspaceName[] Workspaces { get; set; }

        [Parameter]
        public OrderBy? OrderBy { get; set; }

        [Parameter]
        public SortDirection? SortDirection { get; set; }

        [Parameter]
        public SwitchParameter ResetSettings { get; set; }

        [Parameter]
        public SwitchParameter Sync { get; set; }

        protected override void Run()
        {
            var updater = new ManifestUpdater();
            var publisher = new ManifestPublisher();
            var manifest = Manifest.Load(Options.WorkspacePath);
            var organizer = new SnippetOrganizer(Options, FileAssociations);

            if (All)
            {
                organizer.UpdateAllWorkspaces(manifest.Definitions, ResetSettings, OrderBy, SortDirection);
                updater.UpdateAllDefinitions(manifest, OrderBy, SortDirection);
                publisher.Publish(manifest, Options.WorkspacePath);
            }
            else
            {
                var workspaceFileNames = Workspaces.Select(x => x.FileName).ToList();
                organizer.UpdateWorkspaces(manifest.Definitions, workspaceFileNames, ResetSettings, OrderBy, SortDirection);
                updater.UpdateSpecifiedDefinitions(manifest, workspaceFileNames);
                publisher.Publish(manifest, Options.WorkspacePath);
            }

            if (Sync || Options.AutoSync)
                CommitAndPush("Update workspaces");
        }
    }

    public class WorkspaceName
    {
        private readonly string _name;

        public WorkspaceName(string name)
        {
            _name = name;
            FileName = Path.ChangeExtension(_name, Constants.WorkspaceFileExtension);
        }

        public static implicit operator WorkspaceName(string value) => new WorkspaceName(value);

        public static implicit operator string(WorkspaceName value) => value.ToString();

        public override string ToString() => _name;

        public string FileName { get; }
    }
}