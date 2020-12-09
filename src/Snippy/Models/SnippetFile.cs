using System.IO;

namespace Snippy.Models
{
    internal class SnippetFile
    {
        public SnippetFile(string fullName)
        {
            FullName = fullName;
            Name = Path.GetFileName(FullName);
        }

        public string FullName { get; }

        public string Name { get; }

        public string ReadAllText() => File.ReadAllText(FullName);
    }
}