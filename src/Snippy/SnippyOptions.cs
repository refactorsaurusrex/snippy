using System;
using System.IO;
using YamlDotNet.Serialization;

namespace Snippy
{
    internal class SnippyOptions : SettingsBase, ISnippyOptions
    {
        public static Lazy<ISnippyOptions> Instance { get; private set; } = new Lazy<ISnippyOptions>(LoadOptions);

        public static void Clear() => Instance = new Lazy<ISnippyOptions>(LoadOptions);

        private readonly string _optionsPath;
        
        public SnippyOptions() => _optionsPath = Path.Combine(AppDataDirectory, "options");

        public string WorkspacePath { get; set; }
        public string SnippetPath { get; set; }

        public bool IsValid() => !string.IsNullOrWhiteSpace(WorkspacePath) && !string.IsNullOrWhiteSpace(SnippetPath);

        public void Save()
        {
            var serializer = new Serializer();
            var serialized = serializer.Serialize(this);
            File.WriteAllText(_optionsPath, serialized);
        }

        private static ISnippyOptions LoadOptions()
        {
            var options = new SnippyOptions();
            if (!File.Exists(options._optionsPath))
                return options;

            var deserializer = new Deserializer();
            var text = File.ReadAllText(options._optionsPath);
            return deserializer.Deserialize<SnippyOptions>(text);
        }
    }
}
