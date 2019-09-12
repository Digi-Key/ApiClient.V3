using System;
using ApiClient.Constants;
using ApiClient.OAuth2.Models;

namespace AspNetExample.WebApp.ViewModels
{
    public class DisplayInformationViewModel
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthorizationEndpoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string AccessToken { get; set; }
        public string Error { get; set; }
        public string ErrorDescription { get; set; }
        public string IdToken { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresIn { get; set; }

        public static DisplayInformationViewModel Create(OAuth2AccessToken oAuth2AccessToken, string clientId, string clientSecret)
        {
            var displayInformationViewModel = new DisplayInformationViewModel
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                AuthorizationEndpoint = DigiKeyUriConstants.AuthorizationEndpoint.ToString(),
                TokenEndpoint = DigiKeyUriConstants.TokenEndpoint.ToString(),
                AccessToken = oAuth2AccessToken.AccessToken,
                Error = oAuth2AccessToken.Error,
                ErrorDescription = oAuth2AccessToken.ErrorDescription,
                IdToken = oAuth2AccessToken.IdToken,
                RefreshToken = oAuth2AccessToken.RefreshToken,
                TokenType = oAuth2AccessToken.TokenType,
                ExpiresIn = DateTime.Now.AddSeconds(oAuth2AccessToken.ExpiresIn)
            };
            return displayInformationViewModel;
        }
    }
}
