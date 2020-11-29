using System;
using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Models;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Set, "SnippetStandardWorkspace")]
    public class SetSnippetStandardWorkspaceCmdlet : CmdletBase
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

        protected override void Run()
        {
            var organizer = new SnippetOrganizer(Options, FileAssociations);

            switch (Type)
            {
                case WorkspaceType.Unpartitioned:
                {
                    var unpartitioned = organizer.CreateUnpartitionedWorkspace(OrderBy, SortDirection, HideMetaFiles);
                    unpartitioned.Publish(Options, Overwrite);
                    break;
                }

                case WorkspaceType.PartitionedByLanguage:
                {
                    var languagePackages = organizer.CreateWorkspacesByLanguage(OrderBy, SortDirection, HideMetaFiles);
                    foreach (var languagePackage in languagePackages)
                        languagePackage.Publish(Options, Overwrite);
                    break;
                }

                case WorkspaceType.PartitionedByTag:
                {
                    var tagPackages = organizer.CreateWorkspacesByTag(OrderBy, SortDirection, HideMetaFiles);
                    foreach (var tagPackage in tagPackages)
                        tagPackage.Publish(Options, Overwrite);
                    break;
                }

                case WorkspaceType.AllStandardTypes:
                {
                    var unpartitioned = organizer.CreateUnpartitionedWorkspace(OrderBy, SortDirection, HideMetaFiles);
                    unpartitioned.Publish(Options, Overwrite);

                    var languagePackages = organizer.CreateWorkspacesByLanguage(OrderBy, SortDirection, HideMetaFiles);
                    foreach (var languagePackage in languagePackages)
                        languagePackage.Publish(Options, Overwrite);

                    var tagPackages = organizer.CreateWorkspacesByTag(OrderBy, SortDirection, HideMetaFiles);
                    foreach (var tagPackage in tagPackages)
                        tagPackage.Publish(Options, Overwrite);
                    break;
                }

                default:
                    throw new NotSupportedException($"The workspace type '{Type}' is not currently supported.");
            }
        }
    }
}