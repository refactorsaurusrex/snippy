using System;
using System.Collections.Generic;
using System.Linq;
using Snippy.Infrastructure;
using Snippy.Models;

namespace Snippy.Services
{
    public class ManifestCleaner
    {
        public Manifest Clean(Manifest manifest, string workspaceDirectory)
        {
            foreach (var def in manifest.Definitions)
            {
#error if file doesn't exist, remove from manifest
            }
        }
    }

    public class ManifestGenerator
    {
        private readonly HashSet<WorkspacePackage> _packages = new HashSet<WorkspacePackage>();

        public void Add(WorkspacePackage package)
        {
            if (!_packages.Add(package))
                throw new DuplicateWorkspacePackageException(package.FileName);
        }

        public Manifest Generate(OrderBy order, SortDirection direction, Manifest previousManifest)
        {
            if (!_packages.Any())
                throw new InvalidOperationException("No workspace packages have been added. Can't create empty manifest.");

            var manifest = new Manifest();

            foreach (var package in _packages)
            {
                var definition = new WorkspaceDefinition
                {
                    FileName = package.FileName,
                    Languages = package.Languages.ToList(),
                    Tags = package.Tags.ToList(),
                    OrderBy = order,
                    SortDirection = direction
                };
                manifest.Definitions.Add(definition);
            }

            if (previousManifest != null && previousManifest.Definitions.Any())
            {
                foreach (var old in previousManifest.Definitions.Where(old => manifest.Definitions.SingleOrDefault(current => current.FileName == old.FileName) == null))
                    manifest.Definitions.Add(old);
            }

            return manifest;
        }
    }
}