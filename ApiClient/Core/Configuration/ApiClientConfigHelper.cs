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
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using ApiClient.Core.Configuration.Interfaces;
using ApiClient.Exception;

namespace ApiClient.Core.Configuration
{
    public class ApiClientConfigHelper : ConfigurationHelper, IApiClientConfigHelper
    {
        // Static members are 'eagerly initialized', that is, 
        // immediately when class is loaded for the first time.
        // .NET guarantees thread safety for static initialization
        private static readonly ApiClientConfigHelper _thisInstance = new ApiClientConfigHelper();

        private const string _ClientId = "ApiClient.ClientId";
        private const string _ClientSecret = "ApiClient.ClientSecret";
        private const string _RedirectUri = "ApiClient.RedirectUri";
        private const string _AccessToken = "ApiClient.AccessToken";
        private const string _RefreshToken = "ApiClient.RefreshToken";
        private const string _ExpirationDateTime = "ApiClient.ExpirationDateTime";

        private ApiClientConfigHelper()
        {
            try
            {
                string regexPattern = @"^(.*)(\\bin\\)(.*)$";

                // We are attempting to find the apiclient.config file in the solution folder for this project
                // Using this method we can use the same apiclient.config for all the projects in this solution.
                var baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');

                // This little hack is ugly but needed to work with Console apps and Asp.Net apps.
                var solutionDir = Regex.IsMatch(baseDir, regexPattern)
                    ? Directory.GetParent(baseDir).Parent.Parent   // Console Apps
                    : Directory.GetParent(baseDir);    // Asp.Net apps

                if (!File.Exists(Path.Combine(solutionDir.FullName, "apiclient.config")))
                {
                    throw new ApiException($"Unable to locate apiclient.config in solution folder {solutionDir.FullName}");
                }

                var map = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = Path.Combine(solutionDir.FullName, "apiclient.config"),
                };
                Console.WriteLine($"map.ExeConfigFilename {map.ExeConfigFilename}");
                _config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            }
            catch (System.Exception ex)
            {
                throw new ApiException($"Error in ApiClientConfigHelper on opening up apiclient.config {ex.Message}");
            }
        }

        public static ApiClientConfigHelper Instance()
        {
            return _thisInstance;
        }

        /// <summary>
        ///     ClientId for ApiClient usage
        /// </summary>
        public string ClientId
        {
            get { return GetAttribute(_ClientId); }
            set { Update(_ClientId, value); }
        }

        /// <summary>
        ///     ClientSecret for ApiClient usage
        /// </summary>
        public string ClientSecret
        {
            get { return GetAttribute(_ClientSecret); }
            set { Update(_ClientSecret, value); }
        }

        /// <summary>
        ///     RedirectUri for ApiClient usage
        /// </summary>
        public string RedirectUri
        {
            get { return GetAttribute(_RedirectUri); }
            set { Update(_RedirectUri, value); }
        }

        /// <summary>
        ///     AccessToken for ApiClient usage
        /// </summary>
        public string AccessToken
        {
            get { return GetAttribute(_AccessToken); }
            set { Update(_AccessToken, value); }
        }

        /// <summary>
        ///     RefreshToken for ApiClient usage
        /// </summary>
        public string RefreshToken
        {
            get { return GetAttribute(_RefreshToken); }
            set { Update(_RefreshToken, value); }
        }

        /// <summary>
        ///     Client for ApiClient usage
        /// </summary>
        public DateTime ExpirationDateTime
        {
            get
            {
                var dateTime = GetAttribute(_ExpirationDateTime);
                if (string.IsNullOrEmpty(dateTime))
                {
                    return DateTime.MinValue;
                }
                return DateTime.Parse(dateTime, null, DateTimeStyles.RoundtripKind);
            }
            set
            {
                var dateTime = value.ToString("o"); // "o" is "roundtrip"
                Update(_ExpirationDateTime, dateTime);
            }
        }
    }
}
