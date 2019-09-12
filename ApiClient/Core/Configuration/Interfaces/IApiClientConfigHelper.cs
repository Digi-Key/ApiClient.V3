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

namespace ApiClient.Core.Configuration.Interfaces
{
    public interface IApiClientConfigHelper
    {
        /// <summary>
        ///     ClientId for ApiClient Usage
        /// </summary>
        string ClientId { get; set; }

        /// <summary>
        ///     ClientSecret for ApiClient Usage
        /// </summary>
        string ClientSecret { get; set; }

        /// <summary>
        ///     RedirectUri for ApiClient Usage
        /// </summary>
        string RedirectUri { get; set; }

        /// <summary>
        ///     AccessToken for ApiClient Usage
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        ///     RefreshToken for ApiClient Usage
        /// </summary>
        string RefreshToken { get; set; }

        /// <summary>
        ///     ExpirationDateTime for ApiClient Usage
        /// </summary>
        DateTime ExpirationDateTime { get; set; }
    }
}
