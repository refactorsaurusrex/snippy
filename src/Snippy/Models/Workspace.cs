using System.Collections.Generic;
using Newtonsoft.Json;

namespace Snippy.Models
{
    public class Workspace
    {
        // This should be a hashset so no dups can be included
        private readonly List<Folder> _folders = new List<Folder>();

        [JsonProperty("folders")]
        public ICollection<Folder> Folders => _folders;

        public void Add(Folder folder) => _folders.Add(folder);

        [JsonProperty("settings")]
        public Settings Settings { get; set; } = new Settings();
    }
}
