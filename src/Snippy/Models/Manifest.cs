using System;
using System.Collections.Generic;
using System.IO;
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
            var text = File.ReadAllText(path);
            return deserializer.Deserialize<Manifest>(text);
        }

        public List<WorkspaceDefinition> Definitions { get; set; } = new List<WorkspaceDefinition>();

        public void Publish(string workspaceDirectory)
        {
            var serializer = new Serializer();
            using var manifestFile = File.CreateText(Path.Combine(workspaceDirectory, FileName));
            serializer.Serialize(manifestFile, this);
        }
    }
}
