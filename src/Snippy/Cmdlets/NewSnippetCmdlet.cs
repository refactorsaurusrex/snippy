﻿using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Infrastructure;
using Snippy.Models;
using Snippy.Services;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.New, "Snippet")]
    public class NewSnippetCmdlet : CmdletBase
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Title { get; set; }

        [Parameter]
        public string[] Files { get; set; }

        [Parameter]
        public string Description { get; set; }

        [Parameter]
        public string[] Tags { get; set; }

        [Parameter]
        public SwitchParameter Sync { get; set; }

        protected override void Run()
        {
            var organizer = new SnippetOrganizer(Options, FileAssociations);
            var path = organizer.CreateNewSnippet(Title, Description, Tags, Files);
            path.RunWithCode();

            var manifest = Manifest.Load(Options.WorkspacePath);
            organizer.UpdateAllWorkspaces(manifest.Definitions, manifest.OrderBy, manifest.SortDirection, resetSettings: false);

            if (Sync || Options.AutoSync)
                CommitAndPush("Add new snippet");
        }
    }
}