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

namespace ApiClient.Constants
{
    /// <summary>
    ///     Uri constants to talk to our OAuth2 server implementation.
    /// </summary>
    public static class DigiKeyUriConstants
    {
        public static Uri GetBaseAddress()
        {
            var Production = bool.Parse(Environment.GetEnvironmentVariable("DigikeyProduction"));
            return Production ? ProductionBaseAddress : SandboxBaseAddress;
        }

        public static Uri GetTokenEndpoint()
        {
            var Production = bool.Parse(Environment.GetEnvironmentVariable("DigikeyProduction"));
            return Production ? ProductionTokenEndpoint : SandboxTokenEndpoint;
        }

        public static Uri GetAuthorizationEndpoint()
        {
            var Production = bool.Parse(Environment.GetEnvironmentVariable("DigikeyProduction"));
            return Production ? ProductionAuthorizationEndpoint : SandboxAuthorizationEndpoint;
        }

        // Production Sandbox instance
        public static readonly Uri SandboxBaseAddress = new("https://sandbox-api.digikey.com");
        public static readonly Uri SandboxTokenEndpoint = new("https://sandbox-api.digikey.com/v1/oauth2/token");
        public static readonly Uri SandboxAuthorizationEndpoint = new("https://sandbox-api.digikey.com/v1/oauth2/authorize");

        // Production instance
        public static readonly Uri ProductionBaseAddress = new("https://api.digikey.com");
        public static readonly Uri ProductionTokenEndpoint = new("https://api.digikey.com/v1/oauth2/token");
        public static readonly Uri ProductionAuthorizationEndpoint = new("https://api.digikey.com/v1/oauth2/authorize");
    }
}
