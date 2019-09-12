using ApiClient.Models;

namespace AspNetExample.WebApp.Models
{
    public class SettingsViewModel
    {
        public string RedirectUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthorizationEndpoint { get; set; }
        public string TokenEndpoint { get; set; }


        public static SettingsViewModel Create(ApiClientSettings settings)
        {
            return new SettingsViewModel()
            {
                ClientId = settings.ClientId,
                ClientSecret = settings.ClientSecret,
                RedirectUri = settings.RedirectUri,
            };
        }
    }
}
