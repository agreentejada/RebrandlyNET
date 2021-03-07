using Newtonsoft.Json;

namespace Rebrandly.Models
{
    public class Script : IPaginated
    {
        /// <summary>
        /// Unique identifier of the script
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Unique name of the script
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Javascript snippet (enclosed into HTML tags)
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Publicly accessible URL to the script content
        /// </summary>
        [JsonProperty("uri")]
        public string URI { get; set; }
    }
}
