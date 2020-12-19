using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Snippy.Infrastructure;
using Snippy.Services;

namespace Snippy.Models
{
    public class WorkspacePackage : IEquatable<WorkspacePackage>
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
                throw new WorkspaceAlreadyExistsException(path);

            new Serializer().SerializeToJson(Workspace, path);
        }

        public bool Equals(WorkspacePackage other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return FileName == other.FileName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((WorkspacePackage) obj);
        }

        public override int GetHashCode() => FileName.GetHashCode();

        public static bool operator ==(WorkspacePackage left, WorkspacePackage right) => Equals(left, right);

        public static bool operator !=(WorkspacePackage left, WorkspacePackage right) => !Equals(left, right);
    }
}