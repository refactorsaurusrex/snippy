using System;
using Newtonsoft.Json;

namespace Snippy.Models
{
    public class Folder : IEquatable<Folder>
    {
        public Folder(string name, string path)
        {
            Name = name;
            Path = path;
        }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("path")]
        public string Path { get; }

        public bool Equals(Folder other)
        {
            if (ReferenceEquals(null, other))
                return false;
            return ReferenceEquals(this, other) || string.Equals(Path, other.Path, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((Folder)obj);
        }

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Path);

        public static bool operator ==(Folder left, Folder right) => Equals(left, right);

        public static bool operator !=(Folder left, Folder right) => !Equals(left, right);
    }
}