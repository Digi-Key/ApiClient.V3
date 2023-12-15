using ApiClient.Models;
using System.Web;

namespace ApiClient.API
{
    public partial class ProductInformation(ApiClientService clientService)
    {
        private readonly ApiClientService _clientService = clientService;

        #region PartSearch

        public async Task<string> KeywordSearch(string keyword, string[]? includes = null, DateTime? afterDate = null)
        {
            afterDate ??= _clientService.AfterDate;
            string? previous = _clientService.PrevRequest(nameof(KeywordSearch), keyword, (DateTime)afterDate);
            if (previous != null) return previous;


            var resourcePath = "Search/v3/Products/Keyword";

            var parameters = string.Empty;

            if (includes != null)
            {
                var includesString = HttpUtility.UrlEncode(string.Join(",", includes));
                parameters += $"?includes={includesString}";
            }

            var fullPath = $"/{resourcePath}{parameters}";

            var request = new KeywordSearchRequest
            {
                Keywords = keyword ?? "P5555-ND",
                RecordCount = 25
            };


            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var postResponse = await _clientService.PostAsJsonAsync(fullPath, request);
            var result = _clientService.GetServiceResponse(postResponse).Result;
            RequestSnapshot digikeyAPIRequest = new()
            {
                Route = nameof(KeywordSearch),
                RouteParameter = keyword!,
                Parameters = parameters,
                Response = result
            };
            _clientService.SaveRequest.Save(digikeyAPIRequest);
            return result;
        }

        public async Task<string> ProductDetails(string digikeyPartNumber, string[]? includes = null, DateTime? afterDate = null)
        {
            afterDate ??= _clientService.AfterDate;
            string? previous = _clientService.PrevRequest(nameof(ProductDetails), digikeyPartNumber, (DateTime)afterDate);
            if (previous != null) return previous;


            var resourcePath = "Search/v3/Products";

            var parameters = string.Empty;

            if (includes != null)
            {
                var includesString = HttpUtility.UrlEncode(string.Join(",", includes));
                parameters += $"?includes={includesString}";
            }


            var encodedPN = HttpUtility.UrlEncode(digikeyPartNumber);

            var fullPath = $"/{resourcePath}/{encodedPN}{parameters}";

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var getResponse = await _clientService.GetAsync(fullPath);

            var result = _clientService.GetServiceResponse(getResponse).Result;

            RequestSnapshot digikeyAPIRequest = new()
            {
                Route = nameof(ProductDetails),
                RouteParameter = digikeyPartNumber!,
                Parameters = parameters,
                Response = result
            };

            _clientService.SaveRequest.Save(digikeyAPIRequest);

            return result;
        }

        public async Task<string> DigiReelPricing(string digikeyPartNumber, int requestedQuantity, string[]? includes = null, DateTime? afterDate = null)
        {
            afterDate ??= _clientService.AfterDate;
            string? previous = _clientService.PrevRequest(nameof(DigiReelPricing), digikeyPartNumber, (DateTime)afterDate);
            if (previous != null) return previous;


            var resourcePathPrefix = "Search/v3/Products";
            var resourcePathSuffix = "DigiReelPricing";

            var encodedPN = HttpUtility.UrlEncode(digikeyPartNumber);

            var parameters = $"requestedQuantity={requestedQuantity}";

            if (includes != null)
            {
                var includesString = HttpUtility.UrlEncode(string.Join(",", includes));
                parameters += $"&includes={includesString}";
            }

            var fullPath = $"/{resourcePathPrefix}/{encodedPN}/{resourcePathSuffix}?{parameters}";

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var getResponse = await _clientService.GetAsync(fullPath);

            var result = _clientService.GetServiceResponse(getResponse).Result;

            RequestSnapshot digikeyAPIRequest = new()
            {
                Route = nameof(DigiReelPricing),
                RouteParameter = digikeyPartNumber!,
                Parameters = parameters,
                Response = result
            };

            _clientService.SaveRequest.Save(digikeyAPIRequest);

            return result;
        }

        public async Task<string> SuggestedParts(string partNumber, DateTime? afterDate = null)
        {
            afterDate ??= _clientService.AfterDate;
            string? previous = _clientService.PrevRequest(nameof(DigiReelPricing), partNumber, (DateTime)afterDate);
            if (previous != null) return previous;


            var resourcePathPrefix = "Search/v3/Products";
            var resourcePathSuffix = "WithSuggestedProducts";

            var encodedPN = HttpUtility.UrlEncode(partNumber);

            var fullPath = $"/{resourcePathPrefix}/{encodedPN}/{resourcePathSuffix}";

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var getResponse = await _clientService.GetAsync(fullPath);

            var result = _clientService.GetServiceResponse(getResponse).Result;

            RequestSnapshot digikeyAPIRequest = new()
            {
                Route = nameof(SuggestedParts),
                RouteParameter = partNumber!,
                Response = result
            };

            _clientService.SaveRequest.Save(digikeyAPIRequest);

            return result;
        }

        public async Task<string> Manufacturers(DateTime? afterDate = null)
        {
            afterDate ??= _clientService.AfterDate;
            string? previous = _clientService.PrevRequest(nameof(Manufacturers), null!, (DateTime)afterDate);
            if (previous != null) return previous;


            var resourcePath = "Search/v3/Manufacturers";

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var getResponse = await _clientService.GetAsync($"{resourcePath}");

            var result = _clientService.GetServiceResponse(getResponse).Result;

            RequestSnapshot digikeyAPIRequest = new()
            {
                Route = nameof(Manufacturers),
                Response = result
            };

            _clientService.SaveRequest.Save(digikeyAPIRequest);

            return result;
        }

        public async Task<string> Categories(DateTime? afterDate = null)
        {
            afterDate ??= _clientService.AfterDate;
            string? previous = _clientService.PrevRequest(nameof(Categories), null!, (DateTime)afterDate);
            if (previous != null) return previous;


            var resourcePath = "Search/v3/Categories";

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var getResponse = await _clientService.GetAsync($"{resourcePath}");

            var result = _clientService.GetServiceResponse(getResponse).Result;

            RequestSnapshot digikeyAPIRequest = new()
            {
                Route = nameof(Categories),
                Response = result
            };

            _clientService.SaveRequest.Save(digikeyAPIRequest);

            return result;
        }

        public async Task<string> CategoriesByID(int categoryID, DateTime? afterDate = null)
        {
            afterDate ??= _clientService.AfterDate;
            string? previous = _clientService.PrevRequest(nameof(CategoriesByID), categoryID.ToString(), (DateTime)afterDate);
            if (previous != null) return previous;

            var resourcePathPrefix = "Search/v3/Categories";

            var fullPath = $"/{resourcePathPrefix}/{categoryID}";

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var getResponse = await _clientService.GetAsync($"{fullPath}");

            var result = _clientService.GetServiceResponse(getResponse).Result;

            RequestSnapshot digikeyAPIRequest = new()
            {
                Route = nameof(CategoriesByID),
                RouteParameter = categoryID.ToString(),
                Response = result
            };

            _clientService.SaveRequest.Save(digikeyAPIRequest);

            return result;
        }

        #endregion

        #region RecommendedParts

        public async Task<string> RecommendedProducts(string digikeyPartNumber, int recordCount = 1, string[]? searchOptionList = null, bool excludeMarketPlaceProducts = false, string[]? includes = null, DateTime? afterDate = null)
        {
            afterDate ??= _clientService.AfterDate;
            string? previous = _clientService.PrevRequest(nameof(DigiReelPricing), digikeyPartNumber, (DateTime)afterDate);
            if (previous != null) return previous;


            var resourcePath = "Recommendations/v3/Products";

            var encodedPN = HttpUtility.UrlEncode(digikeyPartNumber);

            var parameters = $"recordCount={recordCount}";

            if (searchOptionList != null)
            {
                var optionListString = HttpUtility.UrlEncode(string.Join(",", searchOptionList));
                parameters += $"&searchOptionList={optionListString}";
            }

            if (excludeMarketPlaceProducts == true)
            {
                parameters += $"&excludeMarketPlaceProducts=true";
            }

            if (includes != null)
            {
                var includesString = HttpUtility.UrlEncode(string.Join(",", includes));
                parameters += $"&includes={includesString}";
            }

            var fullPath = $"/{resourcePath}/{encodedPN}?{parameters}";

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var getResponse = await _clientService.GetAsync(fullPath);

            var result = _clientService.GetServiceResponse(getResponse).Result;

            RequestSnapshot digikeyAPIRequest = new()
            {
                Route = nameof(RecommendedProducts),
                RouteParameter = digikeyPartNumber!,
                Parameters = parameters,
                Response = result
            };

            _clientService.SaveRequest.Save(digikeyAPIRequest);

            return result;
        }

        #endregion

        #region PackageTypeByQuantity

        public async Task<string> PackageByQuantity(string digikeyPartNumber, int requestedQuantity, string? packagingPreference = null, string[]? includes = null, DateTime? afterDate = null)
        {
            afterDate ??= _clientService.AfterDate;
            string? previous = _clientService.PrevRequest(nameof(DigiReelPricing), digikeyPartNumber, (DateTime)afterDate);
            if (previous != null) return previous;


            var resourcePath = "PackageTypeByQuantity/v3/Products";

            var encodedPN = HttpUtility.UrlEncode(digikeyPartNumber);

            var parameters = HttpUtility.UrlEncode($"requestedQuantity={requestedQuantity}");

            if (packagingPreference != null)
                parameters += $"&packagingPreference={HttpUtility.UrlEncode(packagingPreference)}";

            if (includes != null)
            {
                var includesString = HttpUtility.UrlEncode(string.Join(",", includes));
                parameters += $"&includes={includesString}";
            }

            var fullPath = $"/{resourcePath}/{encodedPN}?{parameters}";

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var getResponse = await _clientService.GetAsync(fullPath);

            var result = _clientService.GetServiceResponse(getResponse).Result;

            RequestSnapshot digikeyAPIRequest = new()
            {
                Route = nameof(PackageByQuantity),
                RouteParameter = digikeyPartNumber!,
                Parameters = parameters,
                Response = result
            };

            _clientService.SaveRequest.Save(digikeyAPIRequest);

            return result;
        }

        #endregion

        #region ProductChangeNotifications

        public async Task<string> ProductChangeNotifications(string digikeyPartNumber, string[]? includes = null, DateTime? afterDate = null)
        {
            afterDate ??= _clientService.AfterDate;
            string? previous = _clientService.PrevRequest(nameof(DigiReelPricing), digikeyPartNumber, (DateTime)afterDate);
            if (previous != null) return previous;


            string? parameters = null;
            var resourcePath = "ChangeNotifications/v3/Products";

            var encodedPN = HttpUtility.UrlEncode(digikeyPartNumber);

            string fullPath;

            if (includes == null)
                fullPath = $"/{resourcePath}/{encodedPN}";

            else
            {
                var includesString = HttpUtility.UrlEncode(string.Join(",", includes));
                parameters = $"inlcudes={includesString}";
                fullPath = $"/{resourcePath}/{encodedPN}?{parameters}";
            }

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var getResponse = await _clientService.GetAsync(fullPath);

            var result = _clientService.GetServiceResponse(getResponse).Result;

            RequestSnapshot digikeyAPIRequest = new()
            {
                Route = nameof(ProductChangeNotifications),
                RouteParameter = digikeyPartNumber!,
                Parameters = parameters!,
                Response = result
            };

            _clientService.SaveRequest.Save(digikeyAPIRequest);

            return result;
        }

        #endregion

        #region ProductTracing

        public async Task<string> ProductTracingDetails(string tracingID, DateTime? afterDate = null)
        {
            afterDate ??= _clientService.AfterDate;
            string? previous = _clientService.PrevRequest(nameof(DigiReelPricing), tracingID, (DateTime)afterDate);
            if (previous != null) return previous;


            var resourcePath = "ProductTracing/v1/Details";

            var encodedID = HttpUtility.UrlEncode(tracingID);

            var fullPath = $"/{resourcePath}/{encodedID}";

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var getResponse = await _clientService.GetAsync(fullPath);

            var result = _clientService.GetServiceResponse(getResponse).Result;

            RequestSnapshot digikeyAPIRequest = new()
            {
                Route = nameof(ProductTracingDetails),
                RouteParameter = tracingID!,
                Response = result
            };

            _clientService.SaveRequest.Save(digikeyAPIRequest);

            return result;
        }

        #endregion
    }
}
