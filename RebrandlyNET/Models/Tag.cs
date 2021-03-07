using Newtonsoft.Json;

namespace Rebrandly.Models
{
    public class Tag : IPaginated
    {
        /// <summary>
        /// Unique identifier of a tag
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Unique name of a tag
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Hexadecimal representation of a color assigned to a tag
        /// </summary>
        [JsonProperty("color")]
        public string Color { get; set; }
    }
}
