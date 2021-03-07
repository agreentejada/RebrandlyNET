using System;
using Newtonsoft.Json;

namespace Rebrandly.Models
{
    public class Link : IPaginated
    {
        /// <summary>
        /// Unique identifier associated with the branded short link
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// A title you assign to the branded short link in order to remember what's behind it
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// The keyword section of your branded short link
        /// </summary>
        [JsonProperty("slashtag")]
        public string SlashTag { get; set; }

        /// <summary>
        /// The destination URL you want your branded short link to point to
        /// </summary>
        [JsonProperty("destination")]
        public string Destination { get; set; }

        /// <summary>
        /// The full branded short link URL, including domain
        /// </summary>
        [JsonProperty("shortURL")]
        public string ShortURL { get; set; }

        /// <summary>
        /// A reference to the branded domain's resource of a branded short link
        /// </summary>
        [JsonProperty("domain")]
        public Domain Domain { get; set; }

        /// <summary>
        /// The UTC date/time this branded short link was created
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The last UTC date/time this branded short link was updated. When created, it matches createdAt
        /// </summary>
        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        #region Optional

        /// <summary>
        /// How many clicks there are on this branded short link so far
        /// </summary>
        [JsonProperty("clicks")]
        public int? Clicks { get; set; }

        /// <summary>
        /// The UTC date/time this branded short link was last clicked on
        /// </summary>
        [JsonProperty("lastClickAt")]
        public DateTime? LastClickAt { get; set; }

        /// <summary>
        /// Whether a link is favourited (loved) or not
        /// </summary>
        [JsonProperty("favourite")]
        public bool? Favorite { get; set; }

        /// <summary>
        /// Whether query parameters in short URL will be forwarded to destination URL. 
        /// E.g. short.link/kw? p = 1 with forwardParameters = true will redirect to 
        /// longurl.com/home/path? p = 1, otherwise will redirect to 
        /// longurl.com/home/path (without query parameters)
        /// </summary>
        [JsonProperty("forwardParameters")]
        public bool? ForwardParameters { get; set; }

        #endregion
    }

    /// <summary>
    /// A POCO that helps with query params for the /v1/links POST API.
    /// </summary>
    public class LinkCreationArgs
    {
        /// <summary>
        /// The destination URL you want your branded short link to point to. Example: https://google.com</summary>
        [JsonProperty("destination")]
        public string Destination { get; set; }

        /// <summary>
        /// The keyword portion of your branded short link. A random one (as short as possible according to the branded domain you use) will be auto-generated if you do not specify one.
        /// </summary>
        [JsonProperty("slashtag")]
        public string Slashtag { get; set; }

        /// <summary>
        /// A title you assign to the branded short link in order to remember what's behind it. A random title will be assigned to the link if you do not specify one.</summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// A reference to the Branded Domain resource for this branded short link. Specify either domain id or domain fullName or both. Make sure the domain is already active/verified and shared in this workspace.
        /// </summary>
        [JsonProperty("domain")]
        public LinkDomainArgs Domain { get; set; } = new LinkDomainArgs()
        {
            FullName = "rebrand.ly"
        };

        /// <summary>
        /// A description/note you associate to the branded short link, available only if your plan includes link notes
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class LinkDomainArgs
    {
        /// <summary>
        /// Branded domain's unique identifier
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Branded domain's FQDN
        /// </summary>
        [JsonProperty("fullName")]
        public string FullName { get; set; }
    }
}
