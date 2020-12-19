using System.Collections.Generic;
using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Models;
using Snippy.Services;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "Snippets")]
    [OutputType(typeof(ICollection<SnippetIndexEntry>))]
    public class GetSnippetsCmdlet : CmdletBase
    {
        [Parameter]
        public OrderBy OrderBy { get; set; } = OrderBy.Created;

        [Parameter]
        public SortDirection SortDirection { get; set; } = SortDirection.Descending;

        protected override void Run()
        {
            var organizer = new SnippetOrganizer(Options, FileAssociations);
            var index = organizer.CreateIndex(OrderBy, SortDirection);
            WriteObject(index, true);
        }
    }
}