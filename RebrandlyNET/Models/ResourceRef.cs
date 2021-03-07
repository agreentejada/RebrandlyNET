using Newtonsoft.Json;

namespace Rebrandly.Models
{
    public class ResourceRef
    {
        /// <summary>
        /// Unique identifier of the original resource
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// API path to resource details
        /// </summary>
        [JsonProperty("ref")]
        public string Ref { get; set; }
    }
}
