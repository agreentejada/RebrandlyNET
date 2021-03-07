using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Generic;

namespace Rebrandly
{
    public class Account
    {
        HttpClient client;

        public Account(HttpClient _client)
        {
            client = _client;
        }

        /// <summary>
        /// Gets the current account's details from the server.
        /// </summary>
        /// <returns>A <see cref="Models.Account"/> description of the current account.</returns>
        public async Task<Models.Account> Get()
        {
            string segment = "account";
            var response = await client.GetAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Account>(json);
        }

        /// <summary>
        /// Get an array of <see cref="Models.Workspace"/> from the server.
        /// </summary>
        /// <param name="orderBy">Sorting criteria to apply to your workspaces collection among "name", "createdAt" and "updatedAt."</param>
        /// <param name="orderDir">Sorting direction to apply to your workspaces collection. Either "asc" or "desc"</param>
        /// <param name="limit">How many workspaces to load (max: 25)</param>
        /// <param name="last">The id of the last workspace you fetched.</param>
        /// <returns></returns>
        public async Task<Models.Workspace[]> GetWorkspaces(OrderByWorkspace? orderBy = null, OrderDir? orderDir = null, uint limit = 25, string last = null)
        {
            string segment = "account/workspaces";

            segment = Template.AddQueryParams(segment, new Dictionary<string, object>()
            {
                {"orderBy", orderBy},
                {"orderDir", orderDir },
                {"limit", limit },
                {"last", last }
            });

            var response = await client.GetAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Workspace[]>(json);
        }

        /// <summary>
        /// Custom method, uses <see cref="GetWorkspaces(string, string, int, string)"/> to get all workspaces available.
        /// </summary>
        /// <param name="orderBy">Sorting criteria to apply to your workspaces collection among "name", "createdAt" and "updatedAt."</param>
        /// <param name="orderDir">Sorting direction to apply to your workspaces collection. Either "asc" or "desc"</param>
        /// <returns>An array of all <see cref="Models.Workspace"/>.</returns>
        public async Task<Models.Workspace[]> GetAllWorkspaces(OrderByWorkspace? orderBy = null, OrderDir? orderDir = null)
        {
            Func<string, Task<Models.Workspace[]>> getTask = ((string last) =>
            {
                return GetWorkspaces(orderBy, orderDir, 25, last);
            });

            return await Template.GetPaginated(getTask, 25);
        }
    }
}
