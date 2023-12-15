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
The library supports using external data sources to store previous requests and prevent sending duplicate requests by checking the external data source. Checking for existing requests can be done by mapping the data source object to a new RequestSnapshot object. Saving requests can be done by implementing the ISaveRequest interface.


```csharp
public class DigiKeyHelperService
    {
        private readonly ApiClientSettings _clientSettings;
        private readonly ApiClientService _clientService;
        private readonly EFCoreContext _efCoreContext;
        private readonly SaveRequest _saveRequest;


        public DigiKeyHelperService(EFCoreContext efCoreContext)
        {
            _efCoreContext = efCoreContext;
            _saveRequest = new(_efCoreContext);
            _clientSettings = ApiClientSettings.CreateFromConfigFile();
            _clientService = new(_clientSettings,
                saveRequest: _saveRequest,
                existingRequests: _efCoreContext.DigikeyAPIRequests.Select(x => new RequestSnapshot()
                {
                    RequestID = x.RequestID,
                    Route = x.Route,
                    RouteParameter = x.RouteParameter,
                    Response = x.Response,
                    DateTime = x.DateTime
                }));
        }

        public class SaveRequest(EFCoreContext efCoreContext) : ISaveRequest
        {
            private readonly EFCoreContext _efCoreContext = efCoreContext;

            public void Save(RequestSnapshot requestSnapshot)
            {
                _efCoreContext.DigikeyAPIRequests.Add(new()
                {
                    Route = requestSnapshot.Route,
                    RouteParameter = requestSnapshot.RouteParameter,
                    Response = requestSnapshot.Response
                });
                _efCoreContext.SaveChanges();
            }
        }
}
```

### Previous Request Cutoffs

The age of previous requests to consider as recent enough can be globably set, or set on a request to request basis.

#### Global Time Cutoff

```csharp
DateTime cutoffDateTime = DateTime.Today.AddDays(-30);
_clientService = new(_clientSettings, afterDate: cutoffDateTime);
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
