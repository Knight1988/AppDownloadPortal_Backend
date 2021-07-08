using System;
using Newtonsoft.Json;

namespace AppDownloadPortal_Backend.Model
{
    public class AppVersion
    {
        [JsonIgnore]
        public Version Version { get; private set; }

        [JsonProperty("version")]
        public string VersionString
        {
            get => Version.ToString();
            set => Version = Version.Parse(value);
        }
        
        [JsonProperty("displayText")]
        public string DisplayText { get; set; }
        
        [JsonProperty("env")]
        public string Environment { get; set; }
        
        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }
    }
}