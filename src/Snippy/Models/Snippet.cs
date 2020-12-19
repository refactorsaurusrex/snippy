using System.Collections.Generic;

namespace Snippy.Models
{
    internal class Snippet
    {
        public string DirectoryPath { get; set; }
        public List<string> Files { get; set; }
        public Meta Meta { get; set; }
    }
}