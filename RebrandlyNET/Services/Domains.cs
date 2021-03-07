using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Generic;

namespace Rebrandly
{
    public class Domains
    {
        HttpClient client;

        public Domains(HttpClient _client)
        {
            client = _client;
        }

        /// <summary>
        /// Get the list of domains shared in the current workspace.
        /// </summary>
        /// <param name="active">Filter branded domains depending on whether they can be used to brand short links or not</param>
        /// <param name="orderBy">Sorting criteria to apply to your branded domains collection among "createdAt", "updatedAt" and "fullName".</param>
        /// <param name="orderDir">Sorting direction to apply to your branded short links collection among "desc" and "asc".</param>
        /// <param name="limit">How many branded domains to load (max: 25)</param>
        /// <param name="last">The id of the last domain you fetched.</param>
        /// <returns></returns>
        public async Task<Models.Domain[]> List(bool? active = null, OrderByDomain? orderBy = null, OrderDir? orderDir = null, uint limit = 25, string last = null)
        {
            string segment = "/domains";

            segment = Template.AddQueryParams(segment, new Dictionary<string, object>()
            {
                {"active", active },
                {"orderBy", orderBy },
                {"orderDir", orderDir },
                {"limit", limit },
                {"last", last }
            });

            var response = await client.GetAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Domain[]>(json);
        }

        /// <summary>
        /// Custom method, uses <see cref="List(bool, string, string, int, string)"/> to get all domains.
        /// </summary>
        /// <param name="active">Filter branded domains depending on whether they can be used to brand short links or not</param>
        /// <param name="orderBy">Sorting criteria to apply to your branded domains collection among "createdAt", "updatedAt" and "fullName".</param>
        /// <param name="orderDir">Sorting direction to apply to your branded short links collection among "desc" and "asc".</param>
        public async Task<Models.Domain[]> ListAll(bool? active = null, OrderByDomain? orderBy = null, OrderDir? orderDir = null)
        {
            Func<string, Task<Models.Domain[]>> getTask = ((string last) =>
            {
                return List(active, orderBy, orderDir, 25, last);
            });

            return await Template.GetPaginated(getTask, 25);
        }

        /// <summary>
        /// Get details about a specific domain
        /// </summary>
        /// <param name="id">Unique identifier of the branded domain you want to get details about</param>
        /// <returns></returns>
        public async Task<Models.Domain> Get(string id)
        {
            string segment = "/domains/" + id;
            var response = await client.GetAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Domain>(json);
        }

        /// <summary>
        /// Get how many domains with given filtering conditions
        /// </summary>
        /// <param name="active">Filter branded domains depending on whether they can be used to branded short links or not</param>
        /// <param name="type">Filter branded domains depending on their type (own by "user" or "service" domains like rebrand.ly)</param>
        /// <returns></returns>
        public async Task<long> Count(bool? active = null, DomainType? type = null)
        {
            string segment = "/domains/count";

            segment = Template.AddQueryParams(segment, new Dictionary<string, object>()
            {
                {"active", active },
                {"type", type }
            });

            var response = await client.GetAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.Linq.JObject.Parse(json).Value<long>("count");
        }
    }
}
