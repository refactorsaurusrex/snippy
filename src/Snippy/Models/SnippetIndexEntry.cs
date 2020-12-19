using System;
using System.Collections.Generic;

namespace Snippy.Models
{
    public class SnippetIndexEntry
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Directory { get; set; }
        public List<string> Tags { get; set; }
        public DateTime? Created { get; set; }
        public List<string> Files { get; set; }
    }
}