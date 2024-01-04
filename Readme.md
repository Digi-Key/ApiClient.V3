<div align="center">
    <h1>C# DigiKey API Client Library</h1>

![image](https://i.imgur.com/gLgV1tB.png)

</div>



### Features

* Makes structured calls to the DigiKey API from .NET projects
* Handles the OAuth2 control flow, logs users in, refreshes tokens when needed, etc.

### Basic Usage

```csharp
var settings = ApiClientSettings.CreateFromConfigFile();
var client = new ApiClientService(settings);
var postResponse = await client.ProductInformation.KeywordSearch("P5555-ND");
Console.WriteLine("response is {0}", postResponse);
```


### Project Contents

* **ApiClient** - Client Library that contains the code to manage a config file with OAuth2 settings and classes to do the OAuth2 call and  an example call to DigiKey's KeywordSearch Api. 
* **ApiClient.ConsoleApp** - Console app to test out programmatic refresh of access token when needed and also check if access token failed to work and then refresh and try again.
* **OAuth2Service.ConsoleApp** - Console app to create the initial access token and refresh token.

### Getting Started

1. Clone the repository or download and extract the zip file containing the ApiClient solution.
2. You will need to register an application on the [DigiKey Developer Portal](https://developer.digikey.com/) in order to create your unique Client ID, Client Secret as well as to set your redirection URI.
3. In the solution folder copy sample-apiclient.config as apiclient.config, and update it with the ClientId, ClientSecret, and RedirectUri values from step 2.
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <appSettings>
        <add key="ApiClient.ClientId"" value="YOUR_CLIENT_ID_HERE" />
        <add key="ApiClient.ClientSecret" value="YOUR_CLIENT_SECRET_HERE" />
        <add key="ApiClient.RedirectUri"  value="YOUR_REDIRECT_URI_HERE" />
        <add key="ApiClient.AccessToken" value="" />
        <add key="ApiClient.RefreshToken" value="" />
        <add key="ApiClient.ExpirationDateTime" value="" />
    </appSettings>
</configuration>
```
4. Run 3Legged_OAuth2Service.ConsoleApp to set the access token, refresh token and expiration date in apiclient.config. 
5. Run ApiClient.ConsoleApp to get results from keyword search.



### Advanced Usage
The library supports using external data sources to store previous requests and prevent sending duplicate requests by checking the external data source.

Checking for existing requests and saving requests can be done by implementing the generic IRequestQuerySave interface within your application. When implementing the `IRequestQuerySave<T3, T4>` interface, T3 is meant to be your DbContext, and T4 is meant to be the EF Core model for the specific table you are storing the requests in. The intended functions of the generic interface are as shown:

* **Convert**: is meant to convert a `RequestSnapshot` object to the native object your table uses.
* **RequestSnapshots**: does the opposite, and is intended to convert from your EF Core table to a `IQueryable<RequestSnapshot>` using a LINQ select statement.
* **Query**: should use `RequestSnapshots` and find any matching results for the route and routeParameter.
* **Save** should use the Convert method to create a new EF Core object and insert it into your table & save.

Here is a sample implementation of the IRequestQuerySave inteface:

```csharp
public class RequestQuerySave : IRequestQuerySave<DbContext, DigikeyAPIRequest>
{
    public void Save(RequestSnapshot requestSnapshot, DbContext database)
    {
        database.DigikeyAPIRequests.Add(Convert(requestSnapshot));
        database.SaveChanges();
    }

    public DigikeyAPIRequest Convert(RequestSnapshot requestSnapshot)
    {
        return new()
        {
            Route = requestSnapshot.Route,
            RouteParameter = requestSnapshot.RouteParameter,
            Response = requestSnapshot.Response
        };
    }

    public string? Query(string route, string routeParameter, DbContext database, DateTime? afterDate = null)
    {
        afterDate ??= DateTime.MinValue;
        var snapshot = RequestSnapshots(database.DigikeyAPIRequests)
            .Where(r =>
            r.Route == route
            && r.RouteParameter == routeParameter
            && r.DateTime > afterDate)
            .OrderByDescending(
            r => r.DateTime)
            .FirstOrDefault();
        return snapshot?.Response;
    }

    public IQueryable<RequestSnapshot> RequestSnapshots(IQueryable<DigikeyAPIRequest>? table)
    {
        return table == null ? Enumerable.Empty<RequestSnapshot>().AsQueryable() : table.Select(x => new RequestSnapshot()
        {
            RequestID = x.RequestID,
            Route = x.Route,
            RouteParameter = x.RouteParameter,
            Response = x.Response,
            DateTime = x.DateTime
        });
    }
}
```

An instance of this is then passed to the constructor of the ApiClientService:

```csharp
var requestQuerySave = new RequestQuerySave();
var clientSettings = ApiClientSettings.CreateFromConfigFile();
ApiClientService<DbContext, DigikeyAPIRequest> clientService = new(clientSettings, requestQuerySave);
```

### Previous Request Cutoffs

The age of previous requests to consider as recent enough can be globably set, or set on a request to request basis.

#### Global Time Cutoff

```csharp
DateTime cutoffDateTime = DateTime.Today.AddDays(-30);
_clientService = new(_clientSettings, _requestQuerySave, afterDate: cutoffDateTime);
```

#### Single Request Time Cutoff

```csharp
DateTime cutoffDateTime = DateTime.Today.AddDays(-5);
var postResponse = await _clientService.ProductInformation.KeywordSearch("P5555-ND", afterDate: cutoffDateTime);
Console.WriteLine("response is {0}", postResponse);
```

### Environment Variables

The library also supports custom apiclient.config file locations. Simply set the APICLIENT_CONFIG_PATH environment variable to the path of your file.

If you need to change between Production and Sandbox environments, use the DIGIKEY_PRODUCTION environement variable.
