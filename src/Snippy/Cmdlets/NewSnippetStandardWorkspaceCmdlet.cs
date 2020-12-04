using System;
using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Models;
using Snippy.Services;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.New, "SnippetStandardWorkspace")]
    public class NewSnippetStandardWorkspaceCmdlet : CmdletBase
    {
        [Parameter(Mandatory = true)]
        public WorkspaceType Type { get; set; }

        [Parameter]
        public OrderBy OrderBy { get; set; }

        [Parameter]
        public SortDirection SortDirection { get; set; }

        [Parameter]
        public SwitchParameter HideMetaFiles { get; set; }

        [Parameter]
        public SwitchParameter Overwrite { get; set; }

        [Parameter]
        public SwitchParameter Push { get; set; }

        protected override void Run()
        {
            var organizer = new SnippetOrganizer(Options, FileAssociations);
            var manifestGenerator = new ManifestGenerator();

            switch (Type)
            {
                case WorkspaceType.Unpartitioned:
                {
                    var unpartitioned = organizer.CreateUnpartitionedWorkspace(OrderBy, SortDirection, HideMetaFiles);
                    unpartitioned.Publish(Options, Overwrite);
                    manifestGenerator.Add(unpartitioned);
                    break;
                }

                case WorkspaceType.PartitionedByLanguage:
                {
                    var languagePackages = organizer.CreateWorkspacesByLanguage(OrderBy, SortDirection, HideMetaFiles);
                    foreach (var languagePackage in languagePackages)
                    {
                        languagePackage.Publish(Options, Overwrite);
                        manifestGenerator.Add(languagePackage);
                    }
                    break;
                }

                case WorkspaceType.PartitionedByTag:
                {
                    var tagPackages = organizer.CreateWorkspacesByTag(OrderBy, SortDirection, HideMetaFiles);
                    foreach (var tagPackage in tagPackages)
                    {
                        tagPackage.Publish(Options, Overwrite);
                        manifestGenerator.Add(tagPackage);
                    }
                    break;
                }

                case WorkspaceType.AllStandardTypes:
                {
                    var unpartitioned = organizer.CreateUnpartitionedWorkspace(OrderBy, SortDirection, HideMetaFiles);
                    unpartitioned.Publish(Options, Overwrite);
                    manifestGenerator.Add(unpartitioned);

                    var languagePackages = organizer.CreateWorkspacesByLanguage(OrderBy, SortDirection, HideMetaFiles);
                    foreach (var languagePackage in languagePackages)
                    {
                        languagePackage.Publish(Options, Overwrite);
                        manifestGenerator.Add(languagePackage);
                    }

                    var tagPackages = organizer.CreateWorkspacesByTag(OrderBy, SortDirection, HideMetaFiles);
                    foreach (var tagPackage in tagPackages)
                    {
                        tagPackage.Publish(Options, Overwrite);
                        manifestGenerator.Add(tagPackage);
                    }
                    break;
                }

                default:
                    throw new NotSupportedException($"The workspace type '{Type}' is not currently supported.");
            }
            
            var manifest = manifestGenerator.ToManifest();
            manifest.Publish(Options.WorkspacePath);

            if (Push)
                CommitAndPush();
        }
    }
}