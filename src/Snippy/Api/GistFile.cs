using Newtonsoft.Json;

namespace Snippy.Api
{
    public class GistFile
    {
        [JsonProperty("filename")]
        public string FileName { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}