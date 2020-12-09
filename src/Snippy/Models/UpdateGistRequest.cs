using System.Linq;
using Newtonsoft.Json.Linq;
using Snippy.Api;
using Snippy.Infrastructure;

namespace Snippy.Models
{
    public class UpdateGistRequest
    {
        public GistFile[] Files { get; set; }
        public string Description { get; set; }

        public JObject ToJObject()
        {
            var i = 0;
            var files = Files.Select(x => new JProperty(i++.ToString(), JToken.FromObject(x)));
            var result = new JObject(new JProperty("files", new JObject(files)));

            if (!Description.IsNullOrWhiteSpace())
                result.Add(new JProperty("description", Description));

            return result;
        }
    }
}