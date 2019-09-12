namespace AspNetExample.WebApp.ViewModels
{
        public class OAuth2ConfigurationViewModel
        {
            public string Callback { get; set; }
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public string AuthorizationEndpoint { get; set; }
            public string TokenEndpoint { get; set; }
        }
}