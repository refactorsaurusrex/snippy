using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Snippy.Infrastructure;
using Snippy.Services;

namespace Snippy.Models
{
    public class WorkspacePackage
    {
        public WorkspacePackage(string fileName, Workspace workspace, ICollection<string> tags, ICollection<string> languages)
        {
            var empty = Enumerable.Empty<string>().ToList();
            FileName = fileName;
            Workspace = workspace;
            Tags = tags ?? empty;
            Languages = languages ?? empty;
        }

        public ICollection<string> Tags { get; }
        public ICollection<string> Languages { get; }
        public string FileName { get; }
        public Workspace Workspace { get; }

        public void Publish(ISnippyOptions options, bool overwrite)
        {
            Directory.CreateDirectory(options.WorkspacePath);
            var path = Path.Combine(options.WorkspacePath, FileName);
            if (File.Exists(path) && !overwrite)
                throw new InvalidOperationException($"Workspace already exists: '{path}'");

            new Serializer().SerializeToJson(Workspace, path);
        }
    }
}