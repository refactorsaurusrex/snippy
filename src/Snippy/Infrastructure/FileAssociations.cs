using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Snippy.Services;

namespace Snippy.Infrastructure
{
    internal class FileAssociations : SettingsBase, IFileAssociations
    {
        public static Lazy<IFileAssociations> Instance { get; private set; } = new Lazy<IFileAssociations>(Load);
        public static void Clear() => Instance = new Lazy<IFileAssociations>(Load);

        private static IFileAssociations Load() => new FileAssociations();
        private readonly Dictionary<string, string> _fileAssociations;

        private FileAssociations()
        {
            FileAssociationsPath = Path.Combine(AppDataDirectory, "fileAssociations.yaml");

            if (!File.Exists(FileAssociationsPath))
            {
                var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                const string error = "Unable to locate executing assembly's location on disk.";
                var defaultAssociations = Path.Combine(assemblyPath ?? throw new InvalidOperationException(error), "DefaultFileAssociations.yaml");
                File.Copy(defaultAssociations, FileAssociationsPath);
            }

            var serializer = new Serializer();
            _fileAssociations = serializer.DeserializeFromYaml<Dictionary<string, string>>(FileAssociationsPath);
        }

        public string FileAssociationsPath { get; }

        public string Lookup(string fileNameOrPath)
        {
            var extension = Path.GetExtension(fileNameOrPath)?.Replace(".", string.Empty) ?? string.Empty;
            return _fileAssociations.TryGetValue(extension, out var value) ? value : "unknown";
        }

        public void Save()
        {
            var serializer = new Serializer();
            serializer.SerializeToYaml(_fileAssociations, FileAssociationsPath);
        }
    }
}