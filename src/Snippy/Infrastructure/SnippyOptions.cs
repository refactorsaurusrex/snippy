using System;
using System.IO;
using Snippy.Services;

namespace Snippy.Infrastructure
{
    internal class SnippyOptions : SettingsBase, ISnippyOptions
    {
        public static Lazy<ISnippyOptions> Instance { get; private set; } = new Lazy<ISnippyOptions>(LoadOptions);

        public static void Clear() => Instance = new Lazy<ISnippyOptions>(LoadOptions);

        private readonly string _optionsPath;
        
        public SnippyOptions() => _optionsPath = Path.Combine(AppDataDirectory, "options");

        public string WorkspacePath { get; set; }
        public string SnippetPath { get; set; }
        public string SecretVault { get; set; }
        public string GitHubTokenSecretName { get; set; }
        public bool AutoSync { get; set; }

        public bool IsValid() => !string.IsNullOrWhiteSpace(WorkspacePath) && !string.IsNullOrWhiteSpace(SnippetPath);

        public void Save()
        {
            var serializer = new Serializer();
            serializer.SerializeToYaml(this, _optionsPath);
        }

        private static ISnippyOptions LoadOptions()
        {
            var options = new SnippyOptions();
            if (!File.Exists(options._optionsPath))
                return options;

            var serializer = new Serializer();
            return serializer.DeserializeFromYaml<SnippyOptions>(options._optionsPath);
        }
    }
}
