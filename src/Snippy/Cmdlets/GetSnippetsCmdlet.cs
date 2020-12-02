using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Services;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "Snippets")]
    public class GetSnippetsCmdlet : CmdletBase
    {
        protected override void Run()
        {
            var organizer = new SnippetOrganizer(Options, FileAssociations);
            var index = organizer.CreateIndex();
            WriteObject(index, true);
        }
    }
}