﻿using Newtonsoft.Json;

namespace Snippy.Models
{
    public class Folder
    {
        public Folder(string name, string path)
        {
            Name = name;
            Path = path;
        }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("path")]
        public string Path { get; }
    }
}