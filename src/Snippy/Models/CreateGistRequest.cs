using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Snippy.Api;
using Snippy.Infrastructure;

namespace Snippy.Models
{
    public class CreateGistRequest
    {
        public bool Public { get; set; }
        public List<GistFile> Files { get; set; }
        public string Description { get; set; }

        public JObject ToJObject()
        {
            var i = 0;
            var files = Files.Select(x => new JProperty(i++.ToString(), JToken.FromObject(x)));
            var result = new JObject(
                new JProperty("public", Public),
                new JProperty("files", new JObject(files)));

            if (!Description.IsNullOrWhiteSpace())
                result.Add(new JProperty("description", Description));

            return result;
        }
    }
}