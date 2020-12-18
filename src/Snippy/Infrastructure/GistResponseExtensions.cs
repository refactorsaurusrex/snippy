using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Snippy.Models;

namespace Snippy.Infrastructure
{
    public static class GistResponseExtensions
    {
        public static object ToMetaData(this GistResponse response, int maxDescriptionLength)
        {
            return new
            {
                response.Id,
                Description = response.Description.Substring(0, maxDescriptionLength < 1 ? response.Description.Length : maxDescriptionLength),
                response.Created,
                response.Updated
            };
        }

        public static void SaveFiles(this GistResponse response, string destination)
        {
            const string message = "Unable to parse {0} from gist response";
            Directory.CreateDirectory(destination);
            foreach (var (_, value) in response.Files)
            {
                var filename = (value.SelectToken("filename") ?? throw new InvalidOperationException(string.Format(message, "filename"))).Value<string>();
                var content = (value.SelectToken("content") ?? throw new InvalidOperationException(string.Format(message, "content"))).Value<string>();
                var path = Path.Combine(destination, filename);
                File.WriteAllText(path, content);
            }
        }
    }
}