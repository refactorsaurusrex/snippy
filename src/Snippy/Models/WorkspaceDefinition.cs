using System.Collections.Generic;

namespace Snippy.Models
{
    public class WorkspaceDefinition
    {
        public string FileName { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Languages { get; set; }
    }
}