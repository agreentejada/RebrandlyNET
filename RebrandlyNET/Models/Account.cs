using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebrandly.Models
{
    public class Account
    {
        /// <summary>
        /// Unique identifier of the account
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Username used in login
        /// </summary>
        [JsonProperty("username")]
        public string UserName { get; set; }

        /// <summary>
        /// Contact email of the account
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Full name of the account owner
        /// </summary>
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        /// <summary>
        /// URL of the account avatar
        /// </summary>
        [JsonProperty("avatarURL")]
        public string AvatarURL { get; set; }

        /// <summary>
        /// UTC creation date/time of the account
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Set of feature/limits info related to the account and its plan
        /// </summary>
        [JsonProperty("subscription")]
        public AccountSubsription Subscription { get; set; }
    }

    public class AccountSubsription
    {
        /// <summary>
        /// Account Subscription object includes a limits dictionary indicating how many:
        /// links, domains, workspaces, teammates, tags, scripts
        /// there are in the account and how many can be created.
        /// </summary>
        public class Limit
        {
            /// <summary>
            /// How many resources of the given type used
            /// </summary>
            [JsonProperty("used")]
            public long Used { get; set; }

            /// <summary>
            /// How many resources of the given type the account is allowing
            /// </summary>
            [JsonProperty("max")]
            public long Max { get; set; }
        }

        /// <summary>
        /// UTC subscription date/time of the account's current plan
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// UTC expiration date/time of the account's current plan, when plan's category is not free
        /// </summary>
        [JsonProperty("expiredAt")]
        public DateTime ExpiredAt { get; set; }

        /// <summary>
        /// Account's resources usage and limits: how many links/domains/tags/etc created so far and which are the maximum limits
        /// </summary>
        [JsonProperty("limits")]
        public Dictionary<string, Limit> Limits { get; set; }
    }
}
