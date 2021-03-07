using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Rebrandly
{
    public class Scripts
    {
        private HttpClient client;

        public Scripts(HttpClient _client)
        {
            client = _client;
        }

        /// <summary>
        /// Get a list of scripts.
        /// </summary>
        /// <param name="orderDir">Sorting direction to apply to your scripts collection</param>
        /// <param name="limit">How many scripts to load (max: 25)</param>
        /// <param name="last">The id of the last script you fetched</param>
        public async Task<Models.Script[]> List(OrderDir? orderDir, uint limit = 25, string last = null)
        {
            string segment = "/scripts";

            segment = Template.AddQueryParams(segment, new Dictionary<string, object>()
            {
                {"orderBy", "name" },
                {"orderDir", orderDir },
                {"limit", limit },
                {"last", last }
            });

            var response = await client.GetAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Script[]>(json);
        }

        /// <summary>
        /// Get all scripts from server.
        /// </summary>
        /// <param name="orderDir">Sorting direction to apply to your scripts collection</param>
        /// <returns></returns>
        public async Task<Models.Script[]> ListAll(OrderDir? orderDir)
        {
            Func<string, Task<Models.Script[]>> getTask = ((string last) =>
            {
                return List(orderDir, 25, last);
            });

            return await Template.GetPaginated(getTask, 25);
        }

        /// <summary>
        /// Get details about a specific script.
        /// </summary>
        /// <param name="id">Unique identifier of the script you want to get details for</param>
        /// <returns></returns>
        public async Task<Models.Script> Get(string id)
        {
            string segment = "/scripts/" + id;
            var response = await client.GetAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Script>(json);
        }

        /// <summary>
        /// Update a script.
        /// </summary>
        /// <param name="id">Unique identifier of the Script resource you want to update</param>
        /// <param name="_name">The new name you want to associate to the script</param>
        /// <param name="_value">The new javascript snippet (including ) for the script</param>
        /// <returns></returns>
        public async Task<Models.Script> Update(string id, string _name, string _value)
        {
            string segment = "/scripts/" + id;
            string json = JsonConvert.SerializeObject(new
            {
                name = _name,
                value = _value
            });

            var response = await client.PostAsync(segment,
                new StringContent(json, Encoding.Default, "application/json"));
            response.EnsureSuccessStatusCode();

            string responsejson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Script>(responsejson);
        }

        /// <summary>
        /// Update a script.
        /// </summary>
        /// <param name="script">The <see cref="Models.Script"/> representation to be updated.</param>
        /// <returns></returns>
        public async Task<Models.Script> Update(Models.Script script)
        {
            return await Update(script.ID, script.Name, script.Value);
        }

        /// <summary>
        /// Create a new script.
        /// </summary>
        /// <param name="_name">The name you want to associated to the script.</param>
        /// <param name="_value">A javascript snippet (including HTML tags).</param>
        /// <returns></returns>
        public async Task<Models.Script> Create(string _name, string _value = null)
        {
            string segment = "/scripts";
            string json = JsonConvert.SerializeObject(new
            {
                name = _name,
                value = _value
            });

            var response = await client.PostAsync(segment,
                new StringContent(json, Encoding.Default, "application/json"));
            response.EnsureSuccessStatusCode();

            string responsejson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Script>(responsejson);
        }

        /// <summary>
        /// Get how many scripts with given filtering conditions.
        /// </summary>
        /// <returns></returns>
        public async Task<long> Count()
        {
            string segment = "/scripts/count";
            var response = await client.GetAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.Linq.JObject.Parse(json).Value<long>("count");
        }

        /// <summary>
        /// Deletes a specific script.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Models.Script> Delete(string id)
        {
            string segment = "/scripts/" + id;
            var response = await client.DeleteAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Script>(json);
        }

        /// <summary>
        /// Attaches a script to a link.
        /// </summary>
        /// <param name="linkID">ID of link you want to edit</param>
        /// <param name="scriptID">ID of script to attach</param>
        /// <returns></returns>
        public async Task Attach(string linkID, string scriptID)
        {
            string segment = "links/" + linkID + "/scripts/" + scriptID;
            var response = await client.PostAsync(segment, null);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Detaches an existing script from a link.
        /// </summary>
        /// <param name="linkID">ID of link you want to edit.</param>
        /// <param name="scriptID">ID of script to attach.</param>
        /// <returns></returns>
        public async Task Detach(string linkID, string scriptID)
        {
            string segment = "links/" + linkID + "/scripts/" + scriptID;
            var response = await client.DeleteAsync(segment);
            response.EnsureSuccessStatusCode();
        }
    }
}