using System.Collections.Generic;
using System.Linq;
using Snippy.Models;

namespace Snippy.Services
{
    public class ManifestGenerator
    {
        private readonly List<WorkspacePackage> _packages = new List<WorkspacePackage>();

        public void Add(params WorkspacePackage[] packages) => _packages?.AddRange(packages);

        public Manifest ToManifest()
        {
            var manifest = new Manifest();
            foreach (var package in _packages)
            {
                var definition = new WorkspaceDefinition
                {
                    FileName = package.FileName,
                    Languages = package.Languages.ToList(),
                    Tags = package.Tags.ToList()
                };
                manifest.Definitions.Add(definition);
            }

            return manifest;
        }
    }
}