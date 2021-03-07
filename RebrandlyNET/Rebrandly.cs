using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Rebrandly
{
    public class RebrandlyClient
    {
        HttpClient client;
        const string baseAddress = "https://api.rebrandly.com/v1/";
        const string oauthAddress = "https://oauth.rebrandly.com/connect/authorize/";

        //If apiKey, save key.
        public string APIKey { get; private set; }

        private string workspace;

        /// <summary>
        /// Which workspace id to be used to create scripts. If no workspace is specified, will select main workspace.
        /// The workspace will be added as a header to all API requests. Entering in a null, empty or whitespace value will remove the header.
        /// </summary>
        public string Workspace
        {
            get => workspace;
            set
            {
                workspace = value;
                if (!client.DefaultRequestHeaders.Contains("workspace"))
                {
                    if (!string.IsNullOrWhiteSpace(workspace))
                    {
                        client.DefaultRequestHeaders.Add("workspace", workspace);
                    }
                }
                else
                {
                    client.DefaultRequestHeaders.Remove("workspace");

                    if (!string.IsNullOrWhiteSpace(workspace))
                    {
                        client.DefaultRequestHeaders.Add("workspace", workspace);
                    }
                }
            }
        }

        RebrandlyClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);

            Account = new Account(client);
            Domains = new Domains(client);
            Links = new Links(client);
            Scripts = new Scripts(client);
            Tags = new Tags(client);
        }

        //TODO: Add OAuth method.
        public RebrandlyClient(string apiKey) : this()
        {
            client.DefaultRequestHeaders.Add("apiKey", apiKey);
            APIKey = apiKey;
        }

        #region Services.

        /// <summary>
        /// Gets account and workspace details.
        /// </summary>
        public Account Account { get; private set; }

        /// <summary>
        /// Gets domain details.
        /// </summary>
        public Domains Domains { get; private set; }

        /// <summary>
        /// Manages all functionality related to links.
        /// </summary>
        public Links Links { get; private set; }

        /// <summary>
        /// Manages creating, attaching, updating, getting, and deleting scripts.
        /// </summary>
        public Scripts Scripts { get; private set;}

        /// <summary>
        /// Manages creating, attaching, updating, getting, and deleting tags.
        /// </summary>
        public Tags Tags { get; private set;  }

        #endregion
    }

    internal class Template
    {
        /// <summary>
        /// Generic implementation of getting all object from a paginated API call.
        /// </summary>
        /// <param name="getTask">Delegate to the API calling method, points to the last object ID or null.</param>
        /// <param name="limit">The limit on # of elements.</param>
        /// <returns></returns>
        internal static async Task<T[]> GetPaginated<T>(Func<string, Task<T[]>> getTask, int limit)
            where T : Models.IPaginated
        {
            List<T> values = new List<T>();
            T[] page = await getTask(null);
            values.AddRange(page);

            while (page.Length == limit)
            {
                var lastobj = page[limit - 1];
                page = await getTask(lastobj.ID);
                values.AddRange(page);
            }

            return values.ToArray();
        }

        //Remarks: Not officially templated, but adding query params discovered by https://stackoverflow.com/questions/17096201/build-query-string-for-system-net-httpclient-get/17096289
        internal static string AddQueryParams(string segment, Dictionary<string, object> queryparameters)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            foreach (var param in queryparameters)
            {
                if (param.Value != null)
                {
                    if (param.Value is bool)
                    {
                        string result = ((bool)param.Value) ? "true" : "false";
                        query.Add(param.Key, result);
                    }
                    else
                    {
                        query.Add(param.Key, param.Value.ToString());
                    }
                }
            }

            string querystring = query.ToString();

            if (!string.IsNullOrWhiteSpace(querystring))
            {
                segment = segment + '?' + querystring;
            }

            return segment;
        }
    }
}
