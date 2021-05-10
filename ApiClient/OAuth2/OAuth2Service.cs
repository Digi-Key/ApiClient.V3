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
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ApiClient.Constants;
using ApiClient.Models;
using ApiClient.OAuth2.Models;
using Common.Logging;
using Newtonsoft.Json;

namespace ApiClient.OAuth2
{
    /// <summary>
    /// OAuth2Service accepts ApiClientSettings to use to initialize and finish an OAuth2 Authorization and 
    /// get and set the Access Token and Refresh Token for the given ClientId and Client Secret in the ApiClientSettings
    /// </summary>
    public class OAuth2Service
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(OAuth2Service));

        private ApiClientSettings _clientSettings;

        public ApiClientSettings ClientSettings
        {
            get { return _clientSettings; }
            set { _clientSettings = value; }
        }

        public OAuth2Service(ApiClientSettings clientSettings)
        {
            ClientSettings = clientSettings;
        }

        /// <summary>
        /// Generates the authentication URL based on ApiClientSettings.
        /// </summary>
        /// <param name="scopes">This is current not used and should be "".</param>
        /// <param name="state">This is not currently used.</param>
        /// <returns>String which is the oauth2 authorization url.</returns>
        public string GenerateAuthUrl(string scopes = "", string state = null)
        {
            var url = string.Format("{0}?client_id={1}&scope={2}&redirect_uri={3}&response_type={4}",
                                    DigiKeyUriConstants.AuthorizationEndpoint,
                                    ClientSettings.ClientId,
                                    scopes,
                                    ClientSettings.RedirectUri,
                                    OAuth2Constants.ResponseTypes.CodeResponse);

            if (!string.IsNullOrWhiteSpace(state))
            {
                url = string.Format("{0}&state={1}", url, state);
            }
            _log.DebugFormat($"Authorize Url is {url}");

            return url;
        }

        /// <summary>
        ///     Finishes authorization by passing the authorization code to the Token endpoint
        /// </summary>
        /// <param name="code">Code value returned by the RedirectUri callback</param>
        /// <returns>Returns OAuth2AccessToken</returns>
        public async Task<OAuth2AccessToken> FinishAuthorization(string code)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.ServerCertificateValidationCallback =
                delegate { return true; };

            // Build up the body for the token request
            var body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(OAuth2Constants.Code, code),
                new KeyValuePair<string, string>(OAuth2Constants.RedirectUri, ClientSettings.RedirectUri),
                new KeyValuePair<string, string>(OAuth2Constants.ClientId, ClientSettings.ClientId),
                new KeyValuePair<string, string>(OAuth2Constants.ClientSecret, ClientSettings.ClientSecret),
                new KeyValuePair<string, string>(OAuth2Constants.GrantType,
                                                 OAuth2Constants.GrantTypes.AuthorizationCode)
            };

            // Request the token
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, DigiKeyUriConstants.TokenEndpoint);

            var httpClient = new HttpClient {BaseAddress = DigiKeyUriConstants.BaseAddress};

            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = new FormUrlEncodedContent(body);
            Console.WriteLine("HttpRequestMessage {0}", requestMessage.RequestUri.AbsoluteUri);
            var tokenResponse = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
            var text = await tokenResponse.Content.ReadAsStringAsync();

            // Check if there was an error in the response
            if (!tokenResponse.IsSuccessStatusCode)
            {
                var status = tokenResponse.StatusCode;
                if (status == HttpStatusCode.BadRequest)
                {
                    // Deserialize and return model
                    var errorResponse = JsonConvert.DeserializeObject<OAuth2AccessToken>(text);
                    return errorResponse;
                }

                // Throw error
                tokenResponse.EnsureSuccessStatusCode();
            }

            // Deserializes the token response if successfull
            var oAuth2Token = OAuth2Helpers.ParseOAuth2AccessTokenResponse(text);

            _log.DebugFormat("FinishAuthorization: " + oAuth2Token);

            return oAuth2Token;
        }

        /// <summary>
        /// Refreshes the token asynchronous.
        /// </summary>
        /// <returns>Returns OAuth2AccessToken</returns>
        public async Task<OAuth2AccessToken> RefreshTokenAsync()
        {
            return await OAuth2Helpers.RefreshTokenAsync(ClientSettings);
        }

        
    }
}
