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
    public class ApiClientService
    {
        private const string CustomHeader = "Api-StaleTokenRetry";

        private ApiClientSettings _clientSettings;
        private readonly ILogger _logger;

        public readonly ISaveRequest SaveRequest;
        public ProductInformation ProductInformation { get; private set; }
        public DateTime AfterDate = DateTime.MinValue;

        public readonly IQueryable<RequestSnapshot> ExistingRequests;

        public ApiClientSettings ClientSettings
        {
            get => _clientSettings;
            set => _clientSettings = value;
        }

        /// <summary>
        ///     The httpclient which will be used for the api calls through the this instance
        /// </summary>
        public HttpClient HttpClient { get; private set; }

        public ApiClientService(ApiClientSettings clientSettings, ILogger? logger = null, ISaveRequest? saveRequest = null, IQueryable<RequestSnapshot>? existingRequests = null, DateTime? afterDate = null)
        {
            ExistingRequests = existingRequests ?? Enumerable.Empty<RequestSnapshot>().AsQueryable();
            if (afterDate != null) AfterDate = (DateTime)afterDate;
            SaveRequest = saveRequest ?? new DefaultSaveRequest();
            _logger = logger ?? ConsoleLogger.Create();
            _clientSettings = clientSettings ?? throw new ArgumentNullException(nameof(clientSettings));

            ProductInformation = new(this);

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
                _logger?.LogInformation(_clientSettings.ToString());

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
                    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", _clientSettings.AccessToken);

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
                    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", _clientSettings.AccessToken);

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
        public string? PrevRequest(string route, string routeParameter, DateTime afterDate)
        {
            var snapshot = ExistingRequests.Where(r => r.Route == route && r.RouteParameter == routeParameter && r.DateTime > afterDate).OrderByDescending(r => r.DateTime).FirstOrDefault();
            return snapshot?.Response;
        }
    }
}
