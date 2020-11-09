using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace RadioApp.Models
{
    public class RadioStation : INotifyPropertyChanged
    {

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("slug"), PrimaryKey]
        public string Slug { get; set; }
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

        [JsonProperty("favorite")]

        private bool _favorite;
        public bool Favorite { get => _favorite;  set { _favorite = value;  OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            //Invoke/Raise Event of the specific method name

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
