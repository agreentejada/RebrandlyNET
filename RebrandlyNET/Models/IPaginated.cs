using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebrandly.Models
{
    public interface IPaginated
    {
        [JsonProperty("id")]
        public string ID { get; set; }
    }
}
