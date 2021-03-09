using System.Collections.Generic;
using System.Linq;
using Snippy.Models;

namespace Snippy.Services
{
    public class ManifestUpdater
    {
        public Manifest UpdateAllDefinitions(Manifest manifest, OrderBy? order = null, SortDirection? direction = null)
        {
            if (!order.HasValue && !direction.HasValue)
                return manifest;

            foreach (var definition in manifest.Definitions)
            {
                if (order.HasValue)
                    definition.OrderBy = order.Value;

                if (direction.HasValue)
                    definition.SortDirection = direction.Value;
            }

            return manifest;
        }

        public Manifest UpdateSpecifiedDefinitions(Manifest manifest, ICollection<string> workspaceFileNames, OrderBy? order = null, SortDirection? direction = null)
        {
            if (!order.HasValue && !direction.HasValue)
                return manifest;

            foreach (var definition in manifest.Definitions.Where(x => workspaceFileNames.Contains(x.FileName)))
            {
                if (order.HasValue)
                    definition.OrderBy = order.Value;

                if (direction.HasValue)
                    definition.SortDirection = direction.Value;
            }

            return manifest;
        }
    }
}