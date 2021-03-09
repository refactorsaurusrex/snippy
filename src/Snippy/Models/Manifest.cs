using System;
using System.Collections.Generic;
using System.IO;
using Snippy.Services;

namespace Snippy.Models
{
    public class Manifest
    {
        public static Manifest Load(string workspaceDirectory)
        {
            var serializer = new Serializer();
            var path = Path.Combine(workspaceDirectory, Manifest.FileName);
            try
            {
                return serializer.DeserializeFromYaml<Manifest>(path) ?? new Manifest();
            }
            catch (Exception)
            {
                return new Manifest();
            }
        }

        public const string FileName = "manifest.yaml";
        public List<WorkspaceDefinition> Definitions { get; set; } = new List<WorkspaceDefinition>();
    }
}
