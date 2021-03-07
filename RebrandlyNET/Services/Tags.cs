using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Rebrandly
{
    public class Tags
    {
        HttpClient client;

        public Tags(HttpClient _client)
        {
            client = _client;
        }

        /// <summary>
        /// Get a list of tags.
        /// </summary>
        /// <param name="orderDir">Sorting direction to apply to your tags collection</param>
        /// <param name="limit">How many tags to load (max: 25)</param>
        /// <param name="last">The id of the last tag you fetched, see Infinite Scrolling section</param>
        /// <returns></returns>
        public async Task<Models.Tag[]> List(OrderDir? orderDir, uint limit = 25, string last = null)
        {
            string segment = "/tags";

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
        /// Gets all the tags on the server.
        /// </summary>
        /// <param name="orderDir">Sorting direction to apply to your tags collection</param>
        /// <returns></returns>
        public async Task<Models.Tag[]> ListAll(OrderDir? orderDir)
        {
            Func<string, Task<Models.Tag[]>> getTask = ((string last) =>
            {
                return List(orderDir, 25, last);
            });

            return await Template.GetPaginated(getTask, 25);
        }

        /// <summary>
        /// Get details about a specific tag
        /// </summary>
        /// <param name="id">Unique identifier of the tag you want to get details for</param>
        /// <returns></returns>
        public async Task<Models.Tag> Get(string id)
        {
            string segment = "/tags/" + id;
            var response = await client.GetAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Tag>(json);
        }

        /// <summary>
        /// Create a new tag.
        /// </summary>
        /// <param name="_name">The name you want to associated to the tag</param>
        /// <param name="_color">The hexadecimal code for a color you want to assign to the tag</param>
        /// <returns></returns>
        public async Task<Models.Tag> Create(string _name, string _color = "ddeeff")
        {
            string segment = "/tags";
            string json = JsonConvert.SerializeObject(new
            {
                name = _name,
                color = _color
            });
            var response = await client.PostAsync(segment,
                new StringContent(json, Encoding.Default, "application/json"));
            response.EnsureSuccessStatusCode();

            string responsejson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Tag>(responsejson);
        }

        /// <summary>
        /// Updates a tag.
        /// </summary>
        /// <param name="id">Unique identifier of the Tag resource you want to update</param>
        /// <param name="_name">The new name you want to associated to the tag</param>
        /// <param name="_color">The hexadecimal code for a new color you want to assign to the tag</param>
        /// <returns></returns>
        public async Task<Models.Tag> Update(string id, string _name, string _color)
        {
            string segment = "/tags/" + id;
            string json = JsonConvert.SerializeObject(new
            {
                name = _name,
                color = _color
            });
            var response = await client.PostAsync(segment,
                new StringContent(json, Encoding.Default, "application/json"));
            response.EnsureSuccessStatusCode();

            string responsejson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Tag>(responsejson);
        }

        /// <summary>
        /// Updates a tag.
        /// </summary>
        /// <param name="tag">A <see cref="Models.Tag"/> of the tag you'd like to update.</param>
        /// <returns></returns>
        public async Task<Models.Tag> Update(Models.Tag tag)
        {
            return await Update(tag.ID, tag.Name, tag.Color);
        }

        /// <summary>
        /// Get how many tags with given filtering conditions.
        /// </summary>
        public async Task<long> Count()
        {
            string segment = "/tags/count";
            var response = await client.GetAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.Linq.JObject.Parse(json).Value<long>("count");
        }

        /// <summary>
        /// Deletes a specific tag.
        /// </summary>
        /// <param name="id">Unique identifier of the tag you want to delete</param>
        /// <returns></returns>
        public async Task<Models.Tag> Delete(string id)
        {
            string segment = "/tags/" + id;
            var response = await client.DeleteAsync(segment);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Models.Tag>(json);
        }

        /// <summary>
        /// Attaches a tag to a link.
        /// </summary>
        /// <param name="linkID">ID of the link you want to tag.</param>
        /// <param name="tagID">ID of the tag.</param>
        /// <returns></returns>
        public async Task Attach(string linkID, string tagID)
        {
            string segment = "links/" + linkID + "/tags/" + tagID;
            var response = await client.PostAsync(segment, null);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Detaches a tag from a link.
        /// </summary>
        /// <param name="linkID">ID of the link specified.</param>
        /// <param name="tagID">ID of the tag you want to detach.</param>
        /// <returns></returns>
        public async Task Detach(string linkID, string tagID)
        {
            string segment = "links/" + linkID + "/tags/" + tagID;
            var response = await client.DeleteAsync(segment);
            response.EnsureSuccessStatusCode();
        }
    }
}
