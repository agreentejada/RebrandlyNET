# RebrandlyNET
A .NET SDK for the Rebrandly API.

RebrandlyNET is a minimal dependency, straightforward SDK of the Rebrandly REST API that's easy to use.

To get started, just install the package through nuget!
```
PM> Install-Package RebrandlyNet
```

# Usage

Starting the client is super-easy, just provide your API key that you got from Rebrandly.
```c#
string apiKey = "MY_API_KEY";
var client = new RebrandlyClient(apiKey);
```

To add a workspace, set the Workspace property, which will automatically add the necessary headers! Alternatively, you can get all of your workspaces, and choose one. 
```c#
var workspaces = await rebrand.Account.GetWorkspaces();
client.Workspace = workspaces[0].ID;
```

## Creating a new link
To create a new link, either create an ```LinkCreationArgs``` object or use string parameters. RebrandlyNET let's you create links using either ```GET``` or ```POST```.
```c#
string testURL = "https://www.google.com";
string testvanity = "google10310";

string alttestURL = "https://www.youtube.com";
string alttestvanity = "thegreatestyoutubevideo";

var link = await client.Links.CreateGET(testURL, testvanity);
await client.Links.Delete(link.ID);

link = await client.Links.Create(alttestURL, alttestvanity);
await client.Links.Delete(link.ID);
```

## Pagination, or why are there so many ListAll() methods?
Rebrandly uses pagination on nearly all of it's ```GET``` methods. This can get annoying when trying to retrieve every element. RebrandlyNET gives you easy methods, usually called ```ListAll()``` that run through all the pages to get you an array of every element. RebrandlyNET also provides some nifty string enums that make the API a lot easier to use.
```c#
//Gets the first 25 tags on the server.
var tagpage = await client.Tags.List(OrderDir.asc, 25);

//Gets every tag on the server.
var alltags = await client.Tags.ListAll(OrderDir.asc);
```

# Ongoing
- Implementing OAUTH2 support.
- Fixing some HTTP errors that pass through ```.EnsureSuccessStatusCode()```.
- Adding better tests.

