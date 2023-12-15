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

using ApiClient.Constants;
using ApiClient.Exception;
using ApiClient.Models;
using ApiClient.OAuth2;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ApiClient
{
    public class ApiClientService
    {
        private const string CustomHeader = "Api-StaleTokenRetry";

        private ApiClientSettings _clientSettings;

        public ApiClientSettings ClientSettings
        {
            get => _clientSettings;
            set => _clientSettings = value;
        }

        /// <summary>
        ///     The httpclient which will be used for the api calls through the this instance
        /// </summary>
        public HttpClient HttpClient { get; private set; }

        public ApiClientService(ApiClientSettings clientSettings)
        {
            _clientSettings = clientSettings ?? throw new ArgumentNullException(nameof(clientSettings));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpClient = new() { BaseAddress = DigiKeyUriConstants.BaseAddress };
            HttpClient.DefaultRequestHeaders.Authorization = new("Bearer", ClientSettings.AccessToken);
            HttpClient.DefaultRequestHeaders.Add("X-Digikey-Client-Id", ClientSettings.ClientId);
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task ResetExpiredAccessTokenIfNeeded()
        {
            if (_clientSettings.ExpirationDateTime < DateTime.Now)
            {
                // Let's refresh the token
                var oAuth2Service = new OAuth2Service(_clientSettings);
                var oAuth2AccessToken = await oAuth2Service.RefreshTokenAsync();
                if (oAuth2AccessToken.IsError)
                {
                    // Current Refresh token is invalid or expired 
                    Console.WriteLine("Current Refresh token is invalid or expired ");
                    return;
                }

                // Update the clientSettings
                _clientSettings.UpdateAndSave(oAuth2AccessToken);
                Console.WriteLine("ApiClientService::CheckifAccessTokenIsExpired() call to refresh");
                Console.WriteLine(_clientSettings.ToString());

                // Reset the Authorization header value with the new access token.
                var authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", _clientSettings.AccessToken);
                HttpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }
        }

        public async Task<string> KeywordSearch(string keyword)
        {
            var resourcePath = "/Search/v3/Products/Keyword";

            var request = new KeywordSearchRequest
            {
                Keywords = keyword ?? "P5555-ND",
                RecordCount = 25
            };

            await ResetExpiredAccessTokenIfNeeded();
            var postResponse = await PostAsJsonAsync(resourcePath, request);

            return GetServiceResponse(postResponse).Result;
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string resourcePath, T postRequest)
        {
            Console.WriteLine(">ApiClientService::PostAsJsonAsync()");
            try
            {
                HttpResponseMessage response = await HttpClient.PostAsJsonAsync(resourcePath, postRequest);
                Console.WriteLine("<ApiClientService::PostAsJsonAsync()");

                //Unauthorized, then there is a chance token is stale
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();

                    if (OAuth2Helpers.IsTokenStale(responseBody))
                    {
                        Console.WriteLine(
                            $"Stale access token detected ({_clientSettings.AccessToken}. Calling RefreshTokenAsync to refresh it");
                        await OAuth2Helpers.RefreshTokenAsync(_clientSettings);
                        Console.WriteLine($"New Access token is {_clientSettings.AccessToken}");

                        //Only retry the first time.
                        if (!response.RequestMessage!.Headers.Contains(CustomHeader))
                        {
                            HttpClient.DefaultRequestHeaders.Add(CustomHeader, CustomHeader);
                            HttpClient.DefaultRequestHeaders.Authorization =
                                new AuthenticationHeaderValue("Authorization", _clientSettings.AccessToken);
                            return await PostAsJsonAsync(resourcePath, postRequest);
                        }
                        else if (response.RequestMessage.Headers.Contains(CustomHeader))
                        {
                            throw new ApiException($"Inside method {nameof(PostAsJsonAsync)} we received an unexpected stale token response - during the retry for a call whose token we just refreshed {response.StatusCode}");
                        }
                    }
                }

                return response;
            }
            catch (HttpRequestException hre)
            {
                Console.WriteLine($"PostAsJsonAsync<T>: HttpRequestException is {hre.Message}");
                throw;
            }
            catch (ApiException dae)
            {
                Console.WriteLine($"PostAsJsonAsync<T>: ApiException is {dae.Message}");
                throw;
            }
        }

        protected static async Task<string> GetServiceResponse(HttpResponseMessage response)
        {
            Console.WriteLine(">ApiClientService::GetServiceResponse()");
            var postResponse = string.Empty;

            if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    postResponse = await response.Content.ReadAsStringAsync();
                }
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response");
                Console.WriteLine("  Status Code : {0}", response.StatusCode);
                Console.WriteLine("  Content     : {0}", errorMessage);
                Console.WriteLine("  Reason      : {0}", response.ReasonPhrase);
                throw new System.Exception(response.ReasonPhrase);
            }

            Console.WriteLine("<ApiClientService::GetServiceResponse()");
            return postResponse;
        }
    }
}
