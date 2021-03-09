using System.IO;
using System.Linq;
using Snippy.Models;

namespace Snippy.Services
{
    public class ManifestPublisher
    {
        public void Publish(Manifest manifest, string workspaceDirectory)
        {
            manifest.Definitions = manifest.Definitions.OrderBy(x => x.FileName).ToList();
            var serializer = new Serializer();
            var manifestFilePath = Path.Combine(workspaceDirectory, Manifest.FileName);
            serializer.SerializeToYaml(this, manifestFilePath);
        }
    }
}