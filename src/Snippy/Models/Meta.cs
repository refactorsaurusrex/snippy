using System.Collections.Generic;

namespace Snippy.Models
{
    internal class Meta
    {
        // Change to yaml
        public static string FileName = "meta.yaml";

        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
    }
}