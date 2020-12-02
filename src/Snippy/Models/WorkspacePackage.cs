using System;
using System.IO;
using Newtonsoft.Json;
using Snippy.Infrastructure;

namespace Snippy.Models
{
    internal class WorkspacePackage
    {
        public WorkspacePackage(string fileName, Workspace workspace)
        {
            FileName = fileName;
            Workspace = workspace;
        }

        public string FileName { get; }
        public Workspace Workspace { get; }

        public void Publish(ISnippyOptions options, bool overwrite)
        {
            Directory.CreateDirectory(options.WorkspacePath);
            var path = Path.Combine(options.WorkspacePath, FileName);
            if (File.Exists(path) && !overwrite)
                throw new InvalidOperationException($"Workspace already exists: '{path}'");

            var serialized = JsonConvert.SerializeObject(Workspace, Formatting.Indented);
            File.WriteAllText(path, serialized);
        }
    }
}