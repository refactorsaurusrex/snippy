using System.Collections.Generic;
using Newtonsoft.Json;

namespace Snippy.Models
{
    public class Workspace
    {
        private readonly HashSet<Folder> _folders = new HashSet<Folder>();

        [JsonProperty("folders")]
        public ICollection<Folder> Folders => _folders;

        public void Add(Folder folder) => _folders.Add(folder);

        [JsonProperty("settings")]
        public Settings Settings { get; set; } = new Settings();
    }
}
