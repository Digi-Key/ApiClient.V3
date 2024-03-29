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
	    [JsonProperty("ErrorResponseVersion")]
	    public string ErrorResponseVersion { get; set; }

	    [JsonProperty("StatusCode")]
	    public int StatusCode { get; set; }

	    [JsonProperty("ErrorMessage")]
	    public string ErrorMessage { get; set; }

	    [JsonProperty("ErrorDetails")]
	    public string ErrorDetails { get; set; }

	    [JsonProperty("RequestId")]
	    public string RequestId { get; set; }

	    [JsonProperty("ValidationErrors")]
	    public List<string> ValidationErrors { get; set; }
    }
}
