using System.Management.Automation;
using JetBrains.Annotations;
using Snippy.Infrastructure;

namespace Snippy.Cmdlets
{
    [PublicAPI]
    [Cmdlet(VerbsData.Edit, "SnippyFileAssociations")]
    public class EditSnippyFileAssociationsCmdlet : CmdletBase
    {
        protected override void Run()
        {
            var associations = Infrastructure.FileAssociations.Instance.Value;
            associations.FileAssociationsPath.Run();
            Infrastructure.FileAssociations.Clear();
        }
    }
}