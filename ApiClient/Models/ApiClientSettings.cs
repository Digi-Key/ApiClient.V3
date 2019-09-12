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
using System.Diagnostics.CodeAnalysis;
using System.Text;
using ApiClient.Core.Configuration;
using ApiClient.OAuth2.Models;

namespace ApiClient.Models
{
    public class ApiClientSettings
    {
        public String ClientId { get; set; }
        public String ClientSecret { get; set; }
        public String RedirectUri { get; set; }
        public String AccessToken { get; set; }
        public String RefreshToken { get; set; }
        public DateTime ExpirationDateTime { get; set; }

        public void Save()
        {
            ApiClientConfigHelper.Instance().ClientId = ClientId;
            ApiClientConfigHelper.Instance().ClientSecret = ClientSecret;
            ApiClientConfigHelper.Instance().RedirectUri = RedirectUri;
            ApiClientConfigHelper.Instance().AccessToken = AccessToken;
            ApiClientConfigHelper.Instance().RefreshToken = RefreshToken;
            ApiClientConfigHelper.Instance().ExpirationDateTime = ExpirationDateTime;
            ApiClientConfigHelper.Instance().Save();
        }

        public static ApiClientSettings CreateFromConfigFile()
        {
            return new ApiClientSettings()
            {
                ClientId = ApiClientConfigHelper.Instance().ClientId,
                ClientSecret = ApiClientConfigHelper.Instance().ClientSecret,
                RedirectUri = ApiClientConfigHelper.Instance().RedirectUri,
                AccessToken = ApiClientConfigHelper.Instance().AccessToken,
                RefreshToken = ApiClientConfigHelper.Instance().RefreshToken,
                ExpirationDateTime = ApiClientConfigHelper.Instance().ExpirationDateTime,
            };
        }

        public void UpdateAndSave(OAuth2AccessToken oAuth2AccessToken)
        {
            AccessToken = oAuth2AccessToken.AccessToken;
            RefreshToken = oAuth2AccessToken.RefreshToken;
            ExpirationDateTime = DateTime.Now.AddSeconds(oAuth2AccessToken.ExpiresIn);
            Save();
        }
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"   ------------ [ ApiClientSettings ] -------------");
            sb.AppendLine(@"     ClientId            : " + ClientId);
            sb.AppendLine(@"     ClientSecret        : " + ClientSecret);
            sb.AppendLine(@"     RedirectUri         : " + RedirectUri);
            sb.AppendLine(@"     AccessToken         : " + AccessToken);
            sb.AppendLine(@"     RefreshToken        : " + RefreshToken);
            sb.AppendLine(@"     ExpirationDateTime  : " + ExpirationDateTime);
            sb.AppendLine(@"   ---------------------------------------------");

            return sb.ToString();
        }
    }
}
