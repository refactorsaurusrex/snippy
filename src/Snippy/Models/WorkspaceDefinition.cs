using System.Collections.Generic;
using System.Diagnostics;

namespace Snippy.Models
{
    // ReSharper disable once UseNameofExpression
    [DebuggerDisplay("{FileName}")]
    public class WorkspaceDefinition
    {
        public string FileName { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Languages { get; set; }
    }
}