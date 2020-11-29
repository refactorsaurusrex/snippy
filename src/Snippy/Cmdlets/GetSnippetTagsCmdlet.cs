using System.Management.Automation;
using JetBrains.Annotations;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "SnippetTags")]
    public class GetSnippetTagsCmdlet : CmdletBase
    {
        protected override void Run()
        {
            var organizer = new SnippetOrganizer(SnippyOptions.Instance.Value, Snippy.FileAssociations.Instance.Value);
            var tags = organizer.GetUniqueTags();
            WriteObject(tags);
        }
    }
}