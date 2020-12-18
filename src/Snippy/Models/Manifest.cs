using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Snippy.Services;

namespace Snippy.Models
{
    public class Manifest
    {
        public const string FileName = "manifest.yaml";

        public static Manifest Load(string workspaceDirectory)
        {
            var serializer = new Serializer();
            var path = Path.Combine(workspaceDirectory, FileName);
            try
            {
                return serializer.DeserializeFromYaml<Manifest>(path) ?? new Manifest();
            }
            catch (Exception)
            {
                return new Manifest();
            }
        }

        public OrderBy OrderBy { get; set; }
        public SortDirection SortDirection { get; set; }

        public List<WorkspaceDefinition> Definitions { get; set; } = new List<WorkspaceDefinition>();

        public void Publish(string workspaceDirectory)
        {
            var oldManifest = Load(workspaceDirectory);

            foreach (var old in oldManifest.Definitions.Where(old => Definitions.SingleOrDefault(current => current.FileName == old.FileName) == null))
                Definitions.Add(old);

            Definitions = Definitions.OrderBy(x => x.FileName).ToList();
            var serializer = new Serializer();
            var manifestFilePath = Path.Combine(workspaceDirectory, FileName);
            serializer.SerializeToYaml(this, manifestFilePath);
        }
    }
}
