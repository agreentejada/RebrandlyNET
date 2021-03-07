using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace Rebrandly
{
    /// <summary>
    /// These tests aren't attached to any framework, so don't attach them to something else.
    /// </summary>
    public static class Tests
    {
        static string apiKey = string.Empty;
        static string workspace = string.Empty;
        static RebrandlyClient client;

        public static void ConsoleTest()
        {
            Console.WriteLine("RebrandlyNET V" + Assembly.GetCallingAssembly().GetName().Version);
            Console.WriteLine("https://github.com/agreentejada/RebrandlyNET");
            Console.WriteLine("Please enter in your API Key.");
            Console.Write("API Key:");

            //Attempts to read the apiKey.
            apiKey = Console.ReadLine();

            Console.WriteLine("Retrieving account information...");
            client = new RebrandlyClient(apiKey);

            Models.Account account = null;

            try
            {
                account = client.Account.Get().Result;
                Console.WriteLine($"GET request successful. Welcome user.\r\n" +
                    $"ID: {account.ID}\r\n" +
                    $"Name: {account.FullName}\r\n" +
                    $"Email: {account.Email}");
            }
            catch (Exception exc)
            {
                Console.WriteLine("Account information could not be retrieved with this API Key.");
                Console.WriteLine("Please see exception: ");
                Console.WriteLine(exc.Message);

                return;
            }

            Console.WriteLine("Testing list and get functionality.");
            //WorkspacesTest();
            //DomainsTest();
            //LinksTest();
            //TagsTest();
            //ScriptsTest();

            Console.WriteLine("Testing creating and updating links.");
            CreateLinksTest();
        }

        static void WorkspacesTest()
        {
            Console.WriteLine("Getting all workspaces.");
            var allworkspaces = client.Account.GetAllWorkspaces().Result;
            Console.WriteLine(allworkspaces.Length + " workspaces were found.");
        }

        static void DomainsTest()
        {
            Console.WriteLine("Retrieving the first page of domains.");
            var pagedomains = client.Domains.List().Result;

            bool zero = pagedomains.Length == 0;

            if (zero)
            {
                Console.WriteLine("The first query retrieved 0 elements.");
            }
            else
            {
                Console.WriteLine($"The first query retrieved {pagedomains.Length}, with the first domains's ID as {pagedomains[0].ID}.");
            }


            Console.WriteLine("Getting every domain on the server.");
            var alldomains = client.Domains.ListAll().Result;
            Console.WriteLine($"The all query retrieved {alldomains.Length} domains.");

            Console.WriteLine("According to the server, there were " + client.Domains.Count().Result);

            if (!zero)
            {
                Console.WriteLine("Retrieving a single domain.");
                var script = client.Domains.Get(alldomains[0].ID).Result;
                Console.WriteLine(JsonConvert.SerializeObject(script));
            }
        }

        static void LinksTest()
        {
            Console.WriteLine("Retrieving the first page of links.");
            var pagelinks = client.Links.List().Result;
            Console.WriteLine($"The first query retrieved {pagelinks.Length}, with the first link's ID as {pagelinks[0].ID}.");

            Console.WriteLine("Getting every link on the server.");
            var alllinks = client.Links.ListAll().Result;
            Console.WriteLine($"The all query retrieved {alllinks.Length} links.");

            Console.WriteLine("Getting link count from server.");
            Console.WriteLine("According to the server, there were " + client.Links.Count().Result);
            Console.WriteLine("Writing every ID to the console.");
            var ids = alllinks.Select(X => X.ID);
            Console.WriteLine(JsonConvert.SerializeObject(ids));

            Console.WriteLine("Retrieving a single link.");
            var link = client.Links.Get(alllinks[0].ID).Result;
            Console.WriteLine(JsonConvert.SerializeObject(link));

            Console.WriteLine("Retrieving the tags of a single link.");
            var tags = client.Links.ListAllTags(link.ID, OrderDir.desc).Result;
            Console.WriteLine(JsonConvert.SerializeObject(tags));

            Console.WriteLine("Retrieving the scripts of a single link.");
            var scripts = client.Links.ListAllScripts(link.ID, OrderDir.desc).Result;
            Console.WriteLine(JsonConvert.SerializeObject(scripts));
        }

        static void ScriptsTest()
        {
            Console.WriteLine("Retrieving the first page of scripts.");
            var pagescripts = client.Scripts.List(OrderDir.asc).Result;

            bool zero = pagescripts.Length == 0;

            if (zero)
            {
                Console.WriteLine($"The first query retrieved {pagescripts.Length} elements.");
            }
            else
            {
                Console.WriteLine($"The first query retrieved {pagescripts.Length}, with the first script's ID as {pagescripts[0].ID}.");
            }
                
            Console.WriteLine("Getting every script on the server.");
            var allscripts = client.Scripts.ListAll(OrderDir.desc).Result;
            Console.WriteLine($"The all query retrieved {allscripts.Length} scripts.");

            Console.WriteLine("According to the server, there were " + client.Scripts.Count().Result);

            if (!zero)
            {
                Console.WriteLine("Retrieving a single script.");
                var script = client.Scripts.Get(allscripts[0].ID).Result;
                Console.WriteLine(JsonConvert.SerializeObject(script));
            }

        }

        static void TagsTest()
        {
            Console.WriteLine("Retrieving the first page of tags.");
            var pagetags = client.Tags.List().Result;

            bool zero = pagetags.Length == 0;

            if (zero)
            {
                Console.WriteLine($"The first query retrieved {pagetags.Length} elements.");
            }
            else
            {
                Console.WriteLine($"The first query retrieved {pagetags.Length}, with the first tags's ID as {pagetags[0].ID}.");
            }
            

            Console.WriteLine("Getting every tag on the server.");
            var alltags = client.Tags.ListAll().Result;
            Console.WriteLine($"The all query retrieved {alltags.Length} tags.");

            Console.WriteLine("According to the server, there were " + client.Tags.Count().Result);

            if (!zero)
            {
                Console.WriteLine("Retrieving a single script.");
                var tag = client.Tags.Get(alltags[0].ID).Result;
                Console.WriteLine(JsonConvert.SerializeObject(tag));
            }
        }

        static void CreateLinksTest()
        {
            string testURL = "https://www.google.com";
            string testvanity = "google10310";

            string alttestURL = "https://www.youtube.com";
            string alttestvanity = "thegreatestyoutubevideo";

            Console.WriteLine("Creating a test link using get.");
            var link = client.Links.CreateGET(testURL, testvanity, "TestURL").Result;

            Console.WriteLine("Updating test link.");
            var updated = client.Links.Update(link.ID, "TestURLV2", false, alttestURL).Result;

            Console.WriteLine("Deleting the updated link.");
            client.Links.Delete(updated.ID).Wait();

            Console.WriteLine("Creating and deleting a new link with POST");
            link = client.Links.Create(alttestURL, alttestvanity).Result;
            client.Links.Delete(link.ID).Wait();

        }
    }
}
