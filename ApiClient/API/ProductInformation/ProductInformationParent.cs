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

using ApiClient.Models;
using System.Web;

namespace ApiClient.API
{
    public class ProductInformationParent
    {
        public class ResourcePathParent(string? arg = null, string? route = null, string? resourcePath = null, string? resourcePathSuffix = null, string[]? includes = null, Dictionary<string, string>? otherParameters = null)
        {
            public string Route = route ?? string.Empty;
            public string RouteParameter = arg ?? string.Empty;
            public string ResourcePath = resourcePath ?? string.Empty;
            public string ResourcePathSuffix = resourcePathSuffix ?? string.Empty;
            public string Endpoint = arg ?? string.Empty;
            internal string[]? _includes = includes;
            internal Dictionary<string, string> _otherParameters = otherParameters ?? [];

            public string Encoded = arg == null ? string.Empty : HttpUtility.UrlEncode(arg);

            public string Path
            {
                get
                {
                    var prefix = ResourcePath == string.Empty ? string.Empty : '/' + ResourcePath;
                    var endpoint = Endpoint == string.Empty ? string.Empty : '/' + Endpoint;
                    var suffix = ResourcePathSuffix == string.Empty ? string.Empty : '/' + ResourcePathSuffix;
                    var parameters = Parameters == string.Empty ? string.Empty : '?' + Parameters;
                    return $"{prefix}{endpoint}{suffix}{parameters}";
                }
            }

            public string Parameters
            {
                get
                {
                    var parameters = string.Empty;

                    foreach (KeyValuePair<string, string> entry in _otherParameters)
                    {
                        parameters += (parameters == null ? string.Empty : '&') + $"{HttpUtility.UrlEncode(entry.Key)}={HttpUtility.UrlEncode(entry.Value)}";
                    }

                    if (_includes != null)
                    {
                        var includesString = HttpUtility.UrlEncode(string.Join(",", _includes));
                        parameters += (parameters == null ? string.Empty : '&') + $"includes={includesString}";
                    }
                    return parameters;
                }
            }

            public RequestSnapshot Snapshot(string result)
            {
                return new()
                {
                    Route = Route,
                    RouteParameter = RouteParameter,
                    Parameters = Parameters,
                    Response = result
                };
            }
        }

        public class KeywordSearchParent(string keyword, string[]? includes = null) : ResourcePathParent(
            keyword,
            route: "KeywordSearch",
            resourcePath: "Search/v3/Products/Keyword",
            includes: includes)
        {
            new public string Path = "Search/v3/Products/Keyword";

            public KeywordSearchRequest KeywordSearchRequest
            {
                get
                {
                    return new()
                    {
                        Keywords = RouteParameter ?? "P5555-ND",
                        RecordCount = 25
                    };
                }
            }
        }

        public class ProductDetailsParent(string digikeyPartNumber, string[]? includes = null) : ResourcePathParent(
            digikeyPartNumber,
            route: "ProductDetails",
            resourcePath: "Search/v3/Products",
            includes: includes)
        { }

        public class DigiReelPricingParent(string digikeyPartNumber, Dictionary<string, string>? otherParameters = null, string[]? includes = null) : ResourcePathParent(
            digikeyPartNumber,
            route: "DigiReelPricing",
            resourcePath: "Search/v3/Products",
            resourcePathSuffix: "DigiReelPricing",
            includes: includes,
            otherParameters: otherParameters)
        { }

        public class SuggestedPartsParent(string partNumber) : ResourcePathParent(
            partNumber,
            route: "SuggestedParts",
            resourcePath: "Search/v3/Products",
            resourcePathSuffix: "WithSuggestedProducts")
        { }

        public class ManufacturersParent() : ResourcePathParent(
            route: "Manufacturers",
            resourcePath: "Search/v3/Manufacturers")
        { }

        public class CategoriesParent() : ResourcePathParent(
            route: "Categories",
            resourcePath: "Search/v3/Categories")
        { }

        public class CategoriesByIDParent(int categoryID) : ResourcePathParent(
            categoryID.ToString(),
            route: "CategoriesByID",
            resourcePath: "Search/v3/Categories")
        { }

        public class RecommendedProductsParent(string digikeyPartNumber, Dictionary<string, string>? otherParameters = null, string[]? includes = null) : ResourcePathParent(
            digikeyPartNumber,
            route: "RecommendedProducts",
            resourcePath: "Recommendations/v3/Products",
            includes: includes,
            otherParameters: otherParameters)
        { }

        public class PackageByQuantityParent(string digikeyPartNumber, Dictionary<string, string>? otherParameters = null, string[]? includes = null) : ResourcePathParent(
            digikeyPartNumber,
            route: "PackageByQuantity",
            resourcePath: "PackageTypeByQuantity/v3/Products",
            includes: includes,
            otherParameters: otherParameters)
        { }

        public class ProductChangeNotificationsParent(string digikeyPartNumber, string[]? includes = null) : ResourcePathParent(
            digikeyPartNumber,
            route: "ProductChangeNotifications",
            resourcePath: "ChangeNotifications/v3/Products",
            includes: includes)
        { }

        public class ProductTracingDetailsParent(string tracingID) : ResourcePathParent(
            tracingID,
            route: "ProductTracingDetails",
            resourcePath: "ProductTracing/v1/Details")
        { }
    }
}
