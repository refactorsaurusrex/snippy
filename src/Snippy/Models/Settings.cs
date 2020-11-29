using Newtonsoft.Json;

namespace Snippy.Models
{
    public class Settings
    {
        [JsonProperty("markdown.preview.fontSize")]
        public int MarkdownPreviewFontSize { get; set; } = 16;

        [JsonProperty("files.exclude")]
        public FilesExclude FilesExclude { get; set; } = new FilesExclude();
    }
}