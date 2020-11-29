using System;
using System.Collections.Generic;

namespace Snippy.Models
{
    internal class Snippet
    {
        public DateTime LastModified { get; set; }
        public DateTime Created { get; set; }
        public string DirectoryPath { get; set; }
        public List<string> Files { get; set; }
        public Meta Meta { get; set; }
    }
}