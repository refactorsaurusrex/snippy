using System.Collections.Generic;

namespace Snippy.Models
{
    internal class Meta
    {
        public static string FileName = "meta.yaml";
        private List<string> _tags;

        public string Title { get; set; }
        public string Description { get; set; }

        public List<string> Tags
        {
            get => _tags;
            set => _tags = value ?? new List<string>();
        }

        public string GistUrl { get; set; }
    }
}