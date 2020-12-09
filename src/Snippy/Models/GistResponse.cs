﻿using System;
using Newtonsoft.Json;

namespace Snippy.Models
{
    public class GistResponse
    {
        public string Url { get; set; }

        public string Id { get; set; }

        public string Description { get; set; }

        [JsonProperty("created_at")]
        public DateTime Created { get; set; }

        [JsonProperty("updated_at")]
        public DateTime Updated { get; set; }
    }
}