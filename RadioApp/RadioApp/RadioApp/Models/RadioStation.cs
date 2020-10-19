using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace RadioApp.Models
{
    public class RadioStation
    {

        [JsonProperty("slug"), PrimaryKey]
        public string Slug { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("subtext")]
        public object Subtext { get; set; }

        [JsonProperty("bitrate")]
        public object Bitrate { get; set; }

        [JsonProperty("stream_url")]
        public object StreamUrl { get; set; }
    }
}
