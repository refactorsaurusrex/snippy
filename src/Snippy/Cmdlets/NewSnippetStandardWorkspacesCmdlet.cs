using System;
using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Infrastructure;
using Snippy.Models;
using Snippy.Services;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.New, "SnippetStandardWorkspaces")]
    public class NewSnippetStandardWorkspacesCmdlet : CmdletBase
    {
        [Parameter]
        public OrderBy OrderBy { get; set; } = OrderBy.Created;

        [Parameter]
        public SortDirection SortDirection { get; set; } = SortDirection.Descending;

        [Parameter]
        public SwitchParameter HideMetaFiles { get; set; }

        [Parameter]
        public SwitchParameter Overwrite { get; set; }

        [Parameter]
        public SwitchParameter Sync { get; set; }

        protected override void Run()
        {
            var organizer = new SnippetOrganizer(Options, FileAssociations);
            var manifestGenerator = new ManifestGenerator();

            var unpartitioned = organizer.CreateUnpartitionedWorkspace(OrderBy, SortDirection, HideMetaFiles);
            try
            {
                unpartitioned.Publish(Options, Overwrite);
                manifestGenerator.Add(unpartitioned);
            }
            catch (WorkspaceAlreadyExistsException e)
            {
                WriteWarning(e.Message);
            }

            var languagePackages = organizer.CreateWorkspacesByLanguage(OrderBy, SortDirection, HideMetaFiles);
            foreach (var languagePackage in languagePackages)
            {
                try
                {
                    languagePackage.Publish(Options, Overwrite);
                    manifestGenerator.Add(languagePackage);
                }
                catch (WorkspaceAlreadyExistsException e)
                {
                    WriteWarning(e.Message);
                }
            }

            var tagPackages = organizer.CreateWorkspacesByTag(OrderBy, SortDirection, HideMetaFiles);
            foreach (var tagPackage in tagPackages)
            {
                try
                {
                    tagPackage.Publish(Options, Overwrite);
                    manifestGenerator.Add(tagPackage);
                }
                catch (WorkspaceAlreadyExistsException e)
                {
                    WriteWarning(e.Message);
                }
            }

            var manifest = manifestGenerator.Generate(OrderBy, SortDirection, Manifest.Load(Options.WorkspacePath));
            var publisher = new ManifestPublisher();
            publisher.Publish(manifest, Options.WorkspacePath);

            if (Sync || Options.AutoSync)
                CommitAndPush("New standard workspace");
        }
    }
}