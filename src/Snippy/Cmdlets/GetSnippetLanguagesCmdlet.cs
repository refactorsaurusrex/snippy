using System.Management.Automation;
using JetBrains.Annotations;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "SnippetLanguages")]
    public class GetSnippetLanguagesCmdlet : CmdletBase
    {
        protected override void Run()
        {
            var organizer = new SnippetOrganizer(SnippyOptions.Instance.Value, Snippy.FileAssociations.Instance.Value);
            var languages = organizer.GetUniqueLanguages();
            WriteObject(languages);
        }
    }
}