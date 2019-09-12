namespace AspNetExample.WebApp.ViewModels
{
        public class RefreshTokenViewModel
        {
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public string TokenEndpoint { get; set; }
            public string RefreshToken { get; set; }
        }
}