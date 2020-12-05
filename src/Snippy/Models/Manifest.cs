using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;

namespace Snippy.Models
{
    public class Manifest
    {
        public const string FileName = "manifest.yaml";

        public static Manifest Load(string workspaceDirectory)
        {
            var deserializer = new Deserializer();
            var path = Path.Combine(workspaceDirectory, FileName);
            try
            {
                var text = File.ReadAllText(path);
                return deserializer.Deserialize<Manifest>(text) ?? new Manifest();
            }
            catch (Exception)
            {
                return new Manifest();
            }
        }

        public List<WorkspaceDefinition> Definitions { get; set; } = new List<WorkspaceDefinition>();

        public void Publish(string workspaceDirectory)
        {
            var oldManifest = Load(workspaceDirectory);

            foreach (var old in oldManifest.Definitions.Where(old => Definitions.SingleOrDefault(current => current.FileName == old.FileName) == null))
                Definitions.Add(old);

            Definitions = Definitions.OrderBy(x => x.FileName).ToList();
            var serializer = new Serializer();
            using var manifestFile = File.CreateText(Path.Combine(workspaceDirectory, FileName));
            serializer.Serialize(manifestFile, this);
        }
    }
}
