using Newtonsoft.Json;

namespace Snippy.Models
{
    public class FilesExclude
    {
        [JsonProperty("**/meta.yaml")]
        public bool MetaJson { get; set; } = true;
    }
}