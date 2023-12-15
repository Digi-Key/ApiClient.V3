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

using ApiClient.Models;

namespace _2Legged_OAuth2Service.ConsoleApp
{
    public class Program
    {
        static async Task Main()
        {
            // read clientSettings values from apiclient.config
            ApiClientSettings? _clientSettings = ApiClientSettings.CreateFromConfigFile();
            Console.WriteLine(_clientSettings.ToString());

            var oAuth2Service = new ApiClient.OAuth2.OAuth2Service(_clientSettings);

            var result = await oAuth2Service.Get2LeggedAccessToken();

            // Check if you got an error during finishing the OAuth2 authorization
            if (result == null)
                throw new Exception("Authorize result null");
            else if (result.IsError)
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
            // This will keep the console window up until a key is press in the console window.
            Console.WriteLine("\n\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}