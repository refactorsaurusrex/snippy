using System.IO;
using Newtonsoft.Json;

namespace Snippy.Services
{
    internal class Serializer
    {
        private readonly YamlDotNet.Serialization.Serializer _yamlSerializer = new YamlDotNet.Serialization.Serializer();
        private readonly YamlDotNet.Serialization.Deserializer _yamlDeserializer = new YamlDotNet.Serialization.Deserializer();

        public T DeserializeFromYaml<T>(string filePath)
        {
            var text = File.ReadAllText(filePath);
            return _yamlDeserializer.Deserialize<T>(text);
        }

        public void SerializeToYaml<T>(T graph, string filePath)
        {
            using var file = File.CreateText(filePath);
            _yamlSerializer.Serialize(file, graph);
        }

        public T DeserializeFromJson<T>(string filePath)
        {
            var text = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(text);
        }

        public void SerializeToJson<T>(T graph, string filePath)
        {
            var serialized = JsonConvert.SerializeObject(graph, Formatting.Indented);
            File.WriteAllText(filePath, serialized);
        }
    }
}
