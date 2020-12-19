using System.Collections.Generic;
using System.Linq;
using Snippy.Infrastructure;
using Snippy.Models;

namespace Snippy.Services
{
    public class ManifestGenerator
    {
        private readonly HashSet<WorkspacePackage> _packages = new HashSet<WorkspacePackage>();

        public void Add(WorkspacePackage package)
        {
            if (!_packages.Add(package))
                throw new DuplicateWorkspacePackageException(package.FileName);
        }

        public Manifest ToManifest(OrderBy order, SortDirection direction)
        {
            var manifest = new Manifest
            {
                SortDirection = direction,
                OrderBy = order
            };

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