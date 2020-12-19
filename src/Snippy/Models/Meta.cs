using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Snippy.Models
{
    internal class Meta
    {
        public static string FileName = "meta.yaml";
        private List<string> _tags;
        private DateTime? _createdUtc;

        public string Title { get; set; }
        public string Description { get; set; }

        [YamlMember(Alias = "Created")]
        public DateTime? CreatedUtc
        {
            get => _createdUtc;
            set => _createdUtc = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : (DateTime?)null;
        }

        public List<string> Tags
        {
            get => _tags;
            set => _tags = value ?? new List<string>();
        }

        public string GistUrl { get; set; }
    }
}