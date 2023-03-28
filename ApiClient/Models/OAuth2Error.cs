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

using Newtonsoft.Json;

namespace ApiClient.Models
{
    public class OAuth2Error
    {
        [JsonProperty("StatusCode")]
        public string HttpStatusCode { get; set; }

        [JsonProperty("ErrorMessage")]
        public string HttpMessage { get; set; }

        [JsonProperty("ErrorDetails")]
        public string MoreInformation { get; set; }
    }
}
