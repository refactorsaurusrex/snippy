using System.IO;
using System.Linq;
using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Models;
using Snippy.Services;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.New, "SnippetCustomWorkspace")]
    public class NewSnippetCustomWorkspaceCmdlet : CmdletBase
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [ArgumentCompleter(typeof(TagsCompleter))]
        [Parameter]
        public string[] Tags { get; set; }

        [ArgumentCompleter(typeof(LanguageCompleter))]
        [Parameter]
        public string[] Languages { get; set; }

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
            var invalid = Path.GetInvalidFileNameChars();
            if (Name.Any(c => invalid.Contains(c)))
                throw new PSArgumentException($"The file name '{Name}' contains invalid characters");

            var organizer = new SnippetOrganizer(Options, FileAssociations);
            var package = organizer.CreateCustomWorkspace(Name, Tags, Languages, OrderBy, SortDirection, HideMetaFiles);
            package.Publish(Options, Overwrite);

            var manifestGenerator = new ManifestGenerator();
            manifestGenerator.Add(package);
            var manifest = manifestGenerator.Generate(OrderBy, SortDirection, Manifest.Load(Options.WorkspacePath));
            var publisher = new ManifestPublisher();
            publisher.Publish(manifest, Options.WorkspacePath);

            if (Sync || Options.AutoSync)
                CommitAndPush("New custom workspace");
        }
    }
}