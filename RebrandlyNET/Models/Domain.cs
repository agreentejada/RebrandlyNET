using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rebrandly.Models
{
    public class Domain : IPaginated
    {
        /// <summary>
        /// Unique identifier for the branded domain
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Full name of the branded domain
        /// </summary>
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        /// <summary>
        /// The top level domain part of the branded domain name
        /// </summary>
        [JsonProperty("topLevelDomain")]
        public string TopLevelDomain { get; set; }

        /// <summary>
        /// UTC creation date/time of the branded domain
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// UTC last update date/time of the branded domain
        /// </summary>
        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Branded domain type
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DomainType Type { get; set; }

        /// <summary>
        /// Whether the branded domain can be used or not to create branded short links
        /// </summary>
        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
