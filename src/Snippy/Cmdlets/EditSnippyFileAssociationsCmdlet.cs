using System.Management.Automation;
using JetBrains.Annotations;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsData.Edit, "SnippyFileAssociations")]
    public class EditSnippyFileAssociationsCmdlet : CmdletBase
    {
        protected override void Run()
        {
            var associations = Snippy.FileAssociations.Instance.Value;
            associations.FileAssociationsPath.Run();
            Snippy.FileAssociations.Clear();
        }
    }
}