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
var postResponse = await client.KeywordSearch("P5555-ND");
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
4. Run OAuth2Service.ConsoleApp to set the access token, refresh token and expiration date in apiclient.config. 
5. Run ApiClient.ConsoleApp to get results from keyword search.
