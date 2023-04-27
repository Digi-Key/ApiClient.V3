using System.Diagnostics;
using System.Net;
using System.Web;
using ApiClient.Extensions;
using ApiClient.Models;

namespace _2Legged_OAuth2Service.ConsoleApp
{
    public class Program
    {
        private ApiClientSettings? _clientSettings;

        static void Main()
        {
            var program = new Program();

            // Read configuration values from apiclient.config file and run OAuth2 code flow with OAuth2 Server
            program.Authorize();

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

            var oAuth2Service = new ApiClient.OAuth2.OAuth2Service(_clientSettings);

            var result = await oAuth2Service.Get2LeggedAccessToken();

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