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
        public static bool GetProductionVariable()
        {
            try
            {
                return bool.Parse(Environment.GetEnvironmentVariable("DIGIKEY_PRODUCTION")!);
            }
            catch
            {
                throw new System.Exception("Issue getting the DIGIKEY_PRODUCTION environment variable. Make sure the variable is set to either true or false.");
            }
        }

        public static Uri BaseAddress
        {
            get { return GetProductionVariable() ? ProductionBaseAddress : SandboxBaseAddress; }
        }

        public static Uri TokenEndpoint
        {
            get { return GetProductionVariable() ? ProductionTokenEndpoint : SandboxTokenEndpoint; }
        }

        public static Uri AuthorizationEndpoint
        {
            get { return GetProductionVariable() ? ProductionAuthorizationEndpoint : SandboxAuthorizationEndpoint; }
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
