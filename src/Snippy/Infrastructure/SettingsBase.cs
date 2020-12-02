using System;
using System.IO;
using YamlDotNet.Serialization;

namespace Snippy.Infrastructure
{
    internal abstract class SettingsBase
    {
        protected SettingsBase()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            AppDataDirectory = Path.Combine(appData, Constants.AppName);

            if (!Directory.Exists(AppDataDirectory))
                Directory.CreateDirectory(AppDataDirectory);
        }

        [YamlIgnore]
        public string AppDataDirectory { get; }
    }
}
