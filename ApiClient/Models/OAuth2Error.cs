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
        [JsonProperty(nameof(ErrorResponseVersion))]
        public string ErrorResponseVersion { get; set; } = string.Empty;

        [JsonProperty(nameof(StatusCode))]
        public int StatusCode { get; set; }

        [JsonProperty(nameof(ErrorMessage))]
        public string ErrorMessage { get; set; } = string.Empty;

        [JsonProperty(nameof(ErrorDetails))]
        public string ErrorDetails { get; set; } = string.Empty;

        [JsonProperty(nameof(RequestId))]
        public string RequestId { get; set; } = string.Empty;

        [JsonProperty(nameof(ValidationErrors))]
        public List<string> ValidationErrors { get; set; } = [];
    }
}
