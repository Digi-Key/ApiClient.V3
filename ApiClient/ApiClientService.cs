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

using ApiClient.API;
using ApiClient.Constants;
using ApiClient.Exception;
using ApiClient.Models;
using ApiClient.OAuth2;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ApiClient
{
    public class ApiClientServiceParent
    {
        internal const string CustomHeader = "Api-StaleTokenRetry";

        internal ApiClientSettings _clientSettings;
        internal readonly ILogger _logger;
        internal DateTime AfterDate = DateTime.MinValue;

        public ApiClientSettings ClientSettings
        {
            get => _clientSettings;
            set => _clientSettings = value;
        }

        /// <summary>
        ///     The httpclient which will be used for the api calls through the this instance
        /// </summary>
        public HttpClient HttpClient { get; private set; }

        public ApiClientServiceParent(ApiClientSettings clientSettings, ILogger? logger = null, DateTime? afterDate = null)
        {
            if (afterDate != null) AfterDate = (DateTime)afterDate;
            _logger = logger ?? ConsoleLogger.Create();
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
                    _logger?.LogInformation("Current Refresh token is invalid or expired ");
                    return;
                }

                // Update the clientSettings
                _clientSettings.UpdateAndSave(oAuth2AccessToken);
                _logger?.LogInformation("ApiClientService::CheckifAccessTokenIsExpired() call to refresh");
                _logger?.LogInformation("{clientsettings}", _clientSettings.ToString());

                // Reset the Authorization header value with the new access token.
                var authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", _clientSettings.AccessToken);
                HttpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string resourcePath)
        {
            _logger?.LogInformation(">ApiClientService::GetAsync()");
            var response = await HttpClient.GetAsync(resourcePath);
            _logger?.LogInformation("<ApiClientService::GetAsync()");

            //Unauthorized, then there is a chance token is stale
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                if (OAuth2Helpers.IsTokenStale(responseBody))
                {
                    _logger?.LogInformation("Stale access token detected ({accessToken}. Calling RefreshTokenAsync to refresh it", _clientSettings.AccessToken);
                    await OAuth2Helpers.RefreshTokenAsync(_clientSettings);

                    _logger?.LogInformation("New Access token is {accessToken}", _clientSettings.AccessToken);

                    //Only retry the first time.
                    if (response.RequestMessage!.Headers.Contains(CustomHeader))
                        throw new ApiException($"Inside method {nameof(GetAsync)} we received an unexpected stale token response - during the retry for a call whose token we just refreshed {response.StatusCode}");

                    HttpClient.DefaultRequestHeaders.Add(CustomHeader, CustomHeader);
                    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _clientSettings.AccessToken);

                    return await GetAsync(resourcePath);
                }
            }

            return response;
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string resourcePath, T postRequest)
        {
            _logger?.LogInformation(">ApiClientService::PostAsJsonAsync()");
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(resourcePath, postRequest);
            _logger?.LogInformation("<ApiClientService::PostAsJsonAsync()");

            //Unauthorized, then there is a chance token is stale
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                if (OAuth2Helpers.IsTokenStale(responseBody))
                {
                    _logger?.LogInformation("Stale access token detected ({accessToken}. Calling RefreshTokenAsync to refresh it", _clientSettings.AccessToken);
                    await OAuth2Helpers.RefreshTokenAsync(_clientSettings);

                    _logger?.LogInformation("New Access token is {accessToken}", _clientSettings.AccessToken);

                    //Only retry the first time.
                    if (response.RequestMessage!.Headers.Contains(CustomHeader))
                        throw new ApiException($"Inside method {nameof(PostAsJsonAsync)} we received an unexpected stale token response - during the retry for a call whose token we just refreshed {response.StatusCode}");

                    HttpClient.DefaultRequestHeaders.Add(CustomHeader, CustomHeader);
                    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _clientSettings.AccessToken);

                    return await PostAsJsonAsync(resourcePath, postRequest);
                }
            }

            return response;
        }

        public async Task<string> GetServiceResponse(HttpResponseMessage response)
        {
            _logger?.LogInformation(">ApiClientService::GetServiceResponse()");
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
                _logger?.LogInformation("Response");
                _logger?.LogInformation("  Status Code : {statusCode}", response.StatusCode);
                _logger?.LogInformation("  Content     : {errorMessage}", errorMessage);
                _logger?.LogInformation("  Reason      : {reasonPhrase}", response.ReasonPhrase);
                throw new System.Exception(response.ReasonPhrase);
            }

            _logger?.LogInformation("<ApiClientService::GetServiceResponse()");
            return postResponse;
        }
    }

    public class ApiClientService : ApiClientServiceParent
    {
        public ProductInformation ProductInformation { get; private set; }

        public ApiClientService(ApiClientSettings clientSettings, ILogger? logger = null, DateTime? afterDate = null) : base(clientSettings, logger, afterDate)
        {
            ProductInformation = new(this);
        }
    }

    public class ApiClientService<T3, T4> : ApiClientServiceParent
    {

        public readonly IRequestQuerySave<T3, T4> RequestQuerySave;
        public ProductInformation<T3, T4> ProductInformation { get; private set; }

        public ApiClientService(ApiClientSettings clientSettings, IRequestQuerySave<T3, T4> requestQuerySave, ILogger? logger = null, DateTime? afterDate = null) : base(clientSettings, logger, afterDate)
        {
            RequestQuerySave = requestQuerySave;

            ProductInformation = new(this);
        }
    }
}
