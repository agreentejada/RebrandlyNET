using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Rebrandly
{
    public class Links
    {
        HttpClient client;

        public Links(HttpClient _client)
        {
            client = _client;
        }

        /// <summary>
        /// Gets page of list of links.
        /// </summary>
        /// <param name="domainid">Filter branded short links which refer to a specific branded domain id (or specify a comma-separated set of ids)</param>
        /// <param name="domainfullname">Filter branded short links which refer to a specific branded domain's name (FQDN)</param>
        /// <param name="slashtag">Filter branded short links according to their slashtag value. Use in conjunction with domain.id or domain.fullName parameter to get a specific link.
        /// WARNING: this will not take effect if you don't specify also a domain.id or a domain.fullName along with the request</param>
        /// <param name="creatorid">Filter branded short links which have been created by a specific teammate id (or specify a comma-separated set of ids)</param>
        /// <param name="orderBy">Sorting criteria to apply to your branded short links collection among "createdAt", "title" and "slashtag".</param>
        /// <param name="orderDir">Sorting direction to apply to your branded short links collection</param>
        /// <param name="limit">How many branded short links to load (max: 25)</param>
        /// <param name="last">The id of the last link you fetched</param>
        /// <returns></returns>
        public async Task<Models.Link[]> List(string domainid = null, string domainfullname = null, string slashtag = null, 
            string creatorid = null, OrderByLinks? orderBy = null, OrderDir? orderDir = null, uint limit = 25, string last = null)
        {
            string segment = "/links";

            segment = Template.AddQueryParams(segment, new Dictionary<string, object>()
            {
                {"domain.id", domainid},
                {"domain.fullname", domainfullname},
                {"creatorid", creatorid},
                {"slashtag", slashtag},
                {"orderBy", orderBy},
                {"orderDir", orderDir },
                {"limit", limit },
                {"last", last }
            });

            var response = await client.GetAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Link[]>(json);
        }

        /// <summary>
        /// Gets every link on the server.
        /// </summary>
        /// <param name="domainid">Filter branded short links which refer to a specific branded domain id (or specify a comma-separated set of ids)</param>
        /// <param name="domainfullname">Filter branded short links which refer to a specific branded domain's name (FQDN)</param>
        /// <param name="slashtag">Filter branded short links according to their slashtag value. Use in conjunction with domain.id or domain.fullName parameter to get a specific link.
        /// WARNING: this will not take effect if you don't specify also a domain.id or a domain.fullName along with the request</param>
        /// <param name="creatorid">Filter branded short links which have been created by a specific teammate id (or specify a comma-separated set of ids)</param>
        /// <param name="orderBy">Sorting criteria to apply to your branded short links collection among "createdAt", "title" and "slashtag".</param>
        /// <param name="orderDir">Sorting direction to apply to your branded short links collection</param>
        /// <returns></returns>
        public async Task<Models.Link[]> ListAll(string domainid = null, string domainfullname = null, string slashtag = null,
            string creatorid = null, OrderByLinks? orderBy = null, OrderDir? orderDir = null)
        {
            Func<string, Task<Models.Link[]>> getTask = ((string last) =>
            {
                return List(domainid, domainfullname, slashtag, creatorid, orderBy, orderDir, 25, last);
            });

            return await Template.GetPaginated(getTask, 25);
        }

        /// <summary>
        /// Get details about a specific link (don't know the ID? Fetch it using GET /v1/links)
        /// </summary>
        /// <param name="id">Unique identifier of the branded short link you want to get details for</param>
        /// <returns></returns>
        public async Task<Models.Link> Get(string id)
        {
            string segment = "/links/" + id;
            var response = await client.GetAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Link>(json);
        }

        /// <summary>
        /// Get how many links with given filtering conditions.
        /// </summary>
        /// <param name="favourite">Filter branded short links depnding on the favourite (loved) property</param>
        /// <param name="domainid">Filter branded short links which refer to a specific branded domain id</param>
        /// <returns></returns>
        public async Task<long> Count(bool? favourite = null, string domainid = null)
        {
            string segment = "/links/count";

            segment = Template.AddQueryParams(segment, new Dictionary<string, object>()
            {
                {"favourite", favourite},
                {"domain.id", domainid},
            });

            var response = await client.GetAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.Linq.JObject.Parse(json).Value<long>("count");
        }

        /// <summary>
        /// Creates a new link. GET is still supported, but POST is preferred.
        /// </summary>
        /// <param name="destination">The destination URL you want your branded short link to point to</param>
        /// <param name="slashtag">The keyword portion of your branded short link</param>
        /// <param name="title">A title you assign to the branded short link in order to remember what's behind it</param>
        /// <param name="domainid">The unique id of the branded domain. If not specified, rebrand.ly is used</param>
        /// <param name="domainfullName">The unique name of the branded domain, to be used in place of domain[id] in special cases.
        /// Precedence will be given to domain[id] value.</param>
        /// <returns></returns>
        public async Task<Models.Link> CreateGET(string destination, string slashtag = null, string title = null, 
            string domainid = null, string domainfullName = "rebrandly.ly")
        {
            string segment = "/links/new";

            segment = Template.AddQueryParams(segment, new Dictionary<string, object>()
            {
                {"destination", destination},
                {"slashtag", slashtag},
                {"title", title},
                {"domain[id]", domainid},
                {"domain[fullName]", domainfullName}
            });

            var response = await client.GetAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Link>(json);
        }

        /// <summary>
        /// Creates a new link.
        /// </summary>
        /// <param name="_destination">The destination URL you want your branded short link to point to. Example: https://google.com</param>
        /// <param name="_slashtag">The keyword portion of your branded short link. A random one (as short as possible according to the branded domain you use) will be auto-generated if you do not specify one.</param>
        /// <param name="_domainname">Branded domain's unique identifier</param>
        /// <param name="_domainid">Branded domain's FQDN</param>
        /// <param name="_title">A title you assign to the branded short link in order to remember what's behind it. A random title will be assigned to the link if you do not specify one.</param>
        /// <param name="_description">A description/note you associate to the branded short link, available only if your plan includes link notes.</param>
        /// <remarks>
        /// For the domain object, only the <paramref name="_domainname"/> OR the <paramref name="_domainid"/> are needed. Unless the user has a plan
        /// that offers custom domains, they will not be allowed to use any domain but rebrandly.ly. Make sure the domain is already active/verified and shared in this workspace.
        /// </remarks>
        public async Task<Models.Link> Create(string _destination, string _slashtag = null, string _domainname = "rebrandly.ly", 
            string _domainid = null, string _title = null, string _description = null)
        {
            string segment = "/links";
            string json = JsonConvert.SerializeObject(new
            {
                destination = _destination,
                slashtag = _slashtag,
                domain = new
                {
                    fullName = _domainname,
                    id = _domainid
                },
                title = _title,
                description = _description
            });

            var response = await client.PostAsync(segment,
                new StringContent(json, Encoding.Default, "application/json"));
            response.EnsureSuccessStatusCode();

            string responsejson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Link>(responsejson);
        }

        /// <summary>
        /// Creates a new link.
        /// </summary>
        /// <returns></returns>
        public async Task<Models.Link> Create(Models.LinkCreationArgs linkArgs)
        {
            string segment = "/links";
            string json = JsonConvert.SerializeObject(linkArgs);

            var response = await client.PostAsync(segment,
                new StringContent(json, Encoding.Default, "application/json"));
            response.EnsureSuccessStatusCode();

            string responsejson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Link>(responsejson);
        }

        /// <summary>
        /// Updates a specific link.
        /// </summary>
        /// <param name="id">Unique identifier of the branded short link you want to update</param>
        /// <param name="_title">New title you want to assign to a branded short link</param>
        /// <param name="_destination">New destination URL you want to assign to a branded short link.</param>
        /// <param name="_favourite">Whether a link should be marked as favourite (loved) or not</param>
        /// <param name="_description">A description/note you associate to the branded short link, available only if your plan includes link notes </param>
        /// <returns></returns>
        public async Task<Models.Link> Update(string id, string _title, bool _favourite, string _destination, string _description = null)
        {
            string segment = "/links/ " + id;
            string json = JsonConvert.SerializeObject(new
            {
                title = _title,
                favourite = _favourite,
                destination = _destination,
                description = _description
            });

            var response = await client.PostAsync(segment,
                new StringContent(json, Encoding.Default, "application/json"));
            response.EnsureSuccessStatusCode();

            string responsejson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Link>(responsejson);
        }

        /// <summary>
        /// Deletes a specific link.
        /// </summary>
        /// <param name="id">Unique identifier of the link you want to delete</param>
        /// <returns></returns>
        public async Task<Models.Link> Delete(string id)
        {
            string segment = "/links/" + id;
            var response = await client.DeleteAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Link>(json);
        }

        /// <summary>
        /// Get page of tags attached to a specific link
        /// </summary>
        /// <param name="id">Unique identifier of the Link resource</param>
        /// <param name="orderDir">Sorting direction to apply to your tags collection</param>
        /// <param name="limit">How many tags to load (max: 25)</param>
        /// <param name="last">The id of the last tag you fetched</param>
        /// <returns></returns>
        public async Task<Models.Tag[]> ListTags(string id, OrderDir? orderDir, uint limit = 25, string last = null)
        {
            string segment = "/links/" + id + "/tags";

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
            return JsonConvert.DeserializeObject<Models.Tag[]>(json);
        }

        /// <summary>
        /// Get all tags attached to a specific link
        /// </summary>
        /// <param name="id">Unique identifier of the Link resource</param>
        /// <param name="orderDir">Sorting direction to apply to your tags collection</param>
        /// <returns></returns>
        public async Task<Models.Tag[]> ListAllTags(string id, OrderDir? orderDir)
        {
            Func<string, Task<Models.Tag[]>> getTask = ((string last) =>
            {
                return ListTags(id, orderDir, 25, last);
            });

            return await Template.GetPaginated(getTask, 25);
        }

        /// <summary>
        /// Get page of scripts attached to a specific link.
        /// </summary>
        /// <param name="id">Unique identifier of the Link resource</param>
        /// <param name="orderDir">Sorting direction to apply to your scripts collection</param>
        /// <param name="limit">How many scripts to load (max: 25)</param>
        /// <param name="last">The id of the last script you fetched.</param>
        /// <returns></returns>
        public async Task<Models.Script[]> ListScripts(string id, OrderDir? orderDir, uint limit = 25, string last = null)
        {
            string segment = "/links/" + id + "/scripts";

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
        /// Get all scripts attached to a specific link
        /// </summary>
        /// <param name="id">Unique identifier of the Link resource</param>
        /// <param name="orderDir">Sorting direction to apply to your scripts collection</param>
        /// <returns></returns>
        public async Task<Models.Script[]> ListAllScripts(string id, OrderDir? orderDir)
        {
            Func<string, Task<Models.Script[]>> getTask = ((string last) =>
            {
                return ListScripts(id, orderDir, 25, last);
            });

            return await Template.GetPaginated(getTask, 25);
        }
    }
}
