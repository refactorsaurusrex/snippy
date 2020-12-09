using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Snippy.Services;

namespace Snippy.Models
{
    internal class SnippetFiles : IEnumerable<SnippetFile>
    {
        private readonly DirectoryInfo _directory;

        public SnippetFiles(string directory)
        {
            _directory = new DirectoryInfo(directory);
            var metaPath = Path.Combine(_directory.FullName, Meta.FileName);
            if (File.Exists(metaPath))
            {
                var serializer = new Serializer();
                Meta = serializer.DeserializeFromYaml<Meta>(metaPath);
            }
        }

        public Meta Meta { get; }

        public IEnumerator<SnippetFile> GetEnumerator()
        {
            var files = _directory.GetFiles().Where(x => x.Name != Meta.FileName).Select(x => new SnippetFile(x.FullName));
            foreach (var file in files)
                yield return file;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}