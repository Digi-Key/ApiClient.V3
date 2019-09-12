//-----------------------------------------------------------------------
//
// THE SOFTWARE IS PROVIDED "AS IS" WITHOUT ANY WARRANTIES OF ANY KIND, EXPRESS, IMPLIED, STATUTORY, 
// OR OTHERWISE. EXPECT TO THE EXTENT PROHIBITED BY APPLICABLE LAW, DIGI-KEY DISCLAIMS ALL WARRANTIES, 
// INCLUDING, WITHOUT LIMITATION, ANY IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, 
// SATISFACTORY QUALITY, TITLE, NON-INFRINGEMENT, QUIET ENJOYMENT, 
// AND WARRANTIES ARISING OUT OF ANY COURSE OF DEALING OR USAGE OF TRADE. 
// 
// DIGI-KEY DOES NOT WARRANT THAT THE SOFTWARE WILL FUNCTION AS DESCRIBED, 
// WILL BE UNINTERRUPTED OR ERROR-FREE, OR FREE OF HARMFUL COMPONENTS.
// 
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using ApiClient.Extensions;
using ApiClient.Models;

namespace OAuth2Service.ConsoleApp
{
    public class Program
    {
        private ApiClientSettings _clientSettings;

        static void Main()
        {
            var prog = new Program();

            // Read configuration values from apiclient.config file and run OAuth2 code flow with OAuth2 Server
            prog.Authorize();

            // This will keep the console window up until a key is press in the console window.
            Console.WriteLine("\n\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        ///     OAuth2 code flow authorization with apiclient.config values
        /// </summary>
        private async void Authorize()
        {
            // read clientSettings values from apiclient.config
            _clientSettings = ApiClientSettings.CreateFromConfigFile();
            Console.WriteLine(_clientSettings.ToString());

            // start up a HttpListener for the callback(RedirectUri) from the OAuth2 server
            var httpListener = new HttpListener();
            httpListener.Prefixes.Add(_clientSettings.RedirectUri.EnsureTrailingSlash());
            Console.WriteLine($"listening to {_clientSettings.RedirectUri}");
            httpListener.Start();

            // Initialize our OAuth2 service
            var oAuth2Service = new ApiClient.OAuth2.OAuth2Service(_clientSettings);
            var scopes = "";

            // create Authorize url and send call it thru Process.Start
            var authUrl = oAuth2Service.GenerateAuthUrl(scopes);
            Process.Start(authUrl);

            // get the URL returned from the callback(RedirectUri)
            var context = await httpListener.GetContextAsync();

            // Done with the callback, so stop the HttpListener
            httpListener.Stop();

            // exact the query parameters from the returned URL
            var queryString = context.Request.Url.Query;
            var queryColl = HttpUtility.ParseQueryString(queryString);

            // Grab the needed query parameter code from the query collection
            var code = queryColl["code"];
            Console.WriteLine($"Using code {code}");

            // Pass the returned code value to finish the OAuth2 authorization
            var result = await oAuth2Service.FinishAuthorization(code);

            // Check if you got an error during finishing the OAuth2 authorization
            if (result.IsError)
            {
                Console.WriteLine("\n\nError            : {0}", result.Error);
                Console.WriteLine("\n\nError.Description: {0}", result.ErrorDescription);
            }
            else
            {
                // Display the Access Token and Refresh Token to the Console.
                Console.WriteLine();
                Console.WriteLine("Access token : {0}", result.AccessToken);
                Console.WriteLine("Refresh token: {0}", result.RefreshToken);
                Console.WriteLine("Expires in   : {0}", result.ExpiresIn);

                _clientSettings.UpdateAndSave(result);
                Console.WriteLine("After a good refresh");
                Console.WriteLine(_clientSettings.ToString());
            }
        }
    }
}
