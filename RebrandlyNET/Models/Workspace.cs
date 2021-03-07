using Newtonsoft.Json;
using System;

namespace Rebrandly.Models
{
    /*
     * Workspaces aren't technically documented. This is what I guessed from the JSON obj.
     *  {
  "id": "fffa4cc5b6ee45d6g7897b06ac2d16af",
  "name": "Marketing",
  "avatarUrl": "http://avatar-url-here.com",
  "links": 153,
  "teammates": 5,
  "domains": 2,
  "createdAt": "2017-05-23T10:54:12.000Z",
  "updatedAt": "2017-05-23T10:05:22.000Z"
 },
     * 
     */
    public class Workspace : IPaginated
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("avatarUrl")]
        public string AvatarURL { get; set; }

        [JsonProperty("links")]
        public int Links { get; set; }

        [JsonProperty("teammates")]
        public int Teammates { get; set; }

        [JsonProperty("domains")]
        public int Domains { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}