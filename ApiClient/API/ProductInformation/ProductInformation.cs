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

using System.Web;
using static ApiClient.API.ProductInformationParent;

namespace ApiClient.API
{
    public partial class ProductInformation(ApiClientService clientService)
    {
        private readonly ApiClientService _clientService = clientService;

        #region PartSearch

        public async Task<string> KeywordSearch(string keyword, string[]? includes = null)
        {
            var parent = new KeywordSearchParent(keyword, includes);

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.PostAsJsonAsync(parent.Path, parent.KeywordSearchRequest);
            var result = _clientService.GetServiceResponse(response).Result;

            return result;
        }

        public async Task<string> ProductDetails(string digikeyPartNumber, string[]? includes = null)
        {
            var parent = new ProductDetailsParent(digikeyPartNumber, includes);

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            var result = _clientService.GetServiceResponse(response).Result;

            return result;
        }

        public async Task<string> DigiReelPricing(string digikeyPartNumber, int requestedQuantity, string[]? includes = null)
        {
            var otherParameters = new Dictionary<string, string>
            {
                { "requestedQuantity", requestedQuantity.ToString() }
            };

            var parent = new DigiReelPricingParent(digikeyPartNumber, otherParameters, includes);

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            var result = _clientService.GetServiceResponse(response).Result;

            return result;
        }

        public async Task<string> SuggestedParts(string partNumber)
        {
            var parent = new SuggestedPartsParent(partNumber);

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            var result = _clientService.GetServiceResponse(response).Result;

            return result;
        }

        public async Task<string> Manufacturers()
        {
            var parent = new ManufacturersParent();

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            var result = _clientService.GetServiceResponse(response).Result;

            return result;
        }

        public async Task<string> Categories()
        {
            var parent = new CategoriesParent();

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            var result = _clientService.GetServiceResponse(response).Result;

            return result;
        }

        public async Task<string> CategoriesByID(int categoryID)
        {
            var parent = new CategoriesByIDParent(categoryID);

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            var result = _clientService.GetServiceResponse(response).Result;

            return result;
        }

        #endregion

        #region RecommendedParts

        public async Task<string> RecommendedProducts(string digikeyPartNumber, int recordCount = 1, string[]? searchOptionList = null, bool excludeMarketPlaceProducts = false, string[]? includes = null)
        {
            var otherParameters = new Dictionary<string, string>
            {
                { "recordCount", recordCount.ToString() }
            };

            if (searchOptionList != null)
                otherParameters.Add("searchOptionList", HttpUtility.UrlEncode(string.Join(",", searchOptionList)));

            if (excludeMarketPlaceProducts == true)
                otherParameters.Add("excludeMarketPlaceProducts", "true");

            var parent = new RecommendedProductsParent(digikeyPartNumber, otherParameters, includes);

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            var result = _clientService.GetServiceResponse(response).Result;

            return result;
        }

        #endregion

        #region PackageTypeByQuantity

        public async Task<string> PackageByQuantity(string digikeyPartNumber, int requestedQuantity, string? packagingPreference = null, string[]? includes = null)
        {
            var otherParameters = new Dictionary<string, string>
            {
                { "requestedQuantity", requestedQuantity.ToString() }
            };

            if (packagingPreference != null)
                otherParameters.Add("packagingPreference", packagingPreference);

            var parent = new PackageByQuantityParent(digikeyPartNumber, otherParameters, includes);

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            var result = _clientService.GetServiceResponse(response).Result;

            return result;
        }

        #endregion

        #region ProductChangeNotifications

        public async Task<string> ProductChangeNotifications(string digikeyPartNumber, string[]? includes = null)
        {
            var parent = new ProductChangeNotificationsParent(digikeyPartNumber, includes);

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            var result = _clientService.GetServiceResponse(response).Result;

            return result;
        }

        #endregion

        #region ProductTracing

        public async Task<string> ProductTracingDetails(string tracingID)
        {
            var parent = new ProductTracingDetailsParent(tracingID);

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            var result = _clientService.GetServiceResponse(response).Result;

            return result;
        }

        #endregion
    }

    public partial class ProductInformation<T3, T4>(ApiClientService<T3, T4> clientService)
    {
        private readonly ApiClientService<T3, T4> _clientService = clientService;

        #region PartSearch

        public async Task<string> KeywordSearch(string keyword, T3 database, string[]? includes = null, DateTime? afterDate = null)
        {
            var parent = new KeywordSearchParent(keyword, includes);

            var existing = _clientService.RequestQuerySave.Query(parent.Route, parent.RouteParameter, database, afterDate);
            if (existing != null)
                return existing;

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.PostAsJsonAsync(parent.Path, parent.KeywordSearchRequest);
            string result = _clientService.GetServiceResponse(response).Result;

            _clientService.RequestQuerySave.Save(new()
            {
                Route = parent.Route,
                RouteParameter = parent.RouteParameter,
                Parameters = parent.Parameters,
                Response = result
            }, database);

            return result;
        }

        public async Task<string> ProductDetails(string digikeyPartNumber, T3 database, string[]? includes = null, DateTime? afterDate = null)
        {
            var parent = new ProductDetailsParent(digikeyPartNumber, includes);

            var existing = _clientService.RequestQuerySave.Query(parent.Route, parent.RouteParameter, database, afterDate);
            if (existing != null)
                return existing;

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            string result = _clientService.GetServiceResponse(response).Result;

            _clientService.RequestQuerySave.Save(new()
            {
                Route = parent.Route,
                RouteParameter = parent.RouteParameter,
                Parameters = parent.Parameters,
                Response = result
            }, database);

            return result;
        }

        public async Task<string> DigiReelPricing(string digikeyPartNumber, int requestedQuantity, T3 database, string[]? includes = null, DateTime? afterDate = null)
        {
            var otherParameters = new Dictionary<string, string>
            {
                { "requestedQuantity", requestedQuantity.ToString() }
            };

            var parent = new DigiReelPricingParent(digikeyPartNumber, otherParameters, includes);

            var existing = _clientService.RequestQuerySave.Query(parent.Route, parent.RouteParameter, database, afterDate);
            if (existing != null)
                return existing;

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            string result = _clientService.GetServiceResponse(response).Result;

            _clientService.RequestQuerySave.Save(new()
            {
                Route = parent.Route,
                RouteParameter = parent.RouteParameter,
                Parameters = parent.Parameters,
                Response = result
            }, database);

            return result;
        }

        public async Task<string> SuggestedParts(string partNumber, T3 database, DateTime? afterDate = null)
        {
            var parent = new SuggestedPartsParent(partNumber);

            var existing = _clientService.RequestQuerySave.Query(parent.Route, parent.RouteParameter, database, afterDate);
            if (existing != null)
                return existing;

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            string result = _clientService.GetServiceResponse(response).Result;

            _clientService.RequestQuerySave.Save(new()
            {
                Route = parent.Route,
                RouteParameter = parent.RouteParameter,
                Parameters = parent.Parameters,
                Response = result
            }, database);

            return result;
        }

        public async Task<string> Manufacturers(T3 database, DateTime? afterDate = null)
        {
            var parent = new ManufacturersParent();

            var existing = _clientService.RequestQuerySave.Query(parent.Route, parent.RouteParameter, database, afterDate);
            if (existing != null)
                return existing;

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            string result = _clientService.GetServiceResponse(response).Result;

            _clientService.RequestQuerySave.Save(new()
            {
                Route = parent.Route,
                RouteParameter = parent.RouteParameter,
                Parameters = parent.Parameters,
                Response = result
            }, database);

            return result;
        }

        public async Task<string> Categories(T3 database, DateTime? afterDate = null)
        {
            var parent = new CategoriesParent();

            var existing = _clientService.RequestQuerySave.Query(parent.Route, parent.RouteParameter, database, afterDate);
            if (existing != null)
                return existing;

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            string result = _clientService.GetServiceResponse(response).Result;

            _clientService.RequestQuerySave.Save(new()
            {
                Route = parent.Route,
                RouteParameter = parent.RouteParameter,
                Parameters = parent.Parameters,
                Response = result
            }, database);

            return result;
        }

        public async Task<string> CategoriesByID(int categoryID, T3 database, DateTime? afterDate = null)
        {
            var parent = new CategoriesByIDParent(categoryID);

            var existing = _clientService.RequestQuerySave.Query(parent.Route, parent.RouteParameter, database, afterDate);
            if (existing != null)
                return existing;

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            string result = _clientService.GetServiceResponse(response).Result;

            _clientService.RequestQuerySave.Save(new()
            {
                Route = parent.Route,
                RouteParameter = parent.RouteParameter,
                Parameters = parent.Parameters,
                Response = result
            }, database);

            return result;
        }

        #endregion

        #region RecommendedParts

        public async Task<string> RecommendedProducts(string digikeyPartNumber, T3 database, int recordCount = 1, string[]? searchOptionList = null, bool excludeMarketPlaceProducts = false, string[]? includes = null, DateTime? afterDate = null)
        {
            var otherParameters = new Dictionary<string, string>
            {
                { "recordCount", recordCount.ToString() }
            };

            if (searchOptionList != null)
                otherParameters.Add("searchOptionList", HttpUtility.UrlEncode(string.Join(",", searchOptionList)));

            if (excludeMarketPlaceProducts == true)
                otherParameters.Add("excludeMarketPlaceProducts", "true");

            var parent = new RecommendedProductsParent(digikeyPartNumber, otherParameters, includes);

            var existing = _clientService.RequestQuerySave.Query(parent.Route, parent.RouteParameter, database, afterDate);
            if (existing != null)
                return existing;

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            string result = _clientService.GetServiceResponse(response).Result;

            _clientService.RequestQuerySave.Save(new()
            {
                Route = parent.Route,
                RouteParameter = parent.RouteParameter,
                Parameters = parent.Parameters,
                Response = result
            }, database);

            return result;
        }

        #endregion

        #region PackageTypeByQuantity

        public async Task<string> PackageByQuantity(string digikeyPartNumber, int requestedQuantity, T3 database, string? packagingPreference = null, string[]? includes = null, DateTime? afterDate = null)
        {
            var otherParameters = new Dictionary<string, string>
            {
                { "requestedQuantity", requestedQuantity.ToString() }
            };

            if (packagingPreference != null)
                otherParameters.Add("packagingPreference", packagingPreference);

            var parent = new PackageByQuantityParent(digikeyPartNumber, otherParameters, includes);

            var existing = _clientService.RequestQuerySave.Query(parent.Route, parent.RouteParameter, database, afterDate);
            if (existing != null)
                return existing;

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            string result = _clientService.GetServiceResponse(response).Result;

            _clientService.RequestQuerySave.Save(new()
            {
                Route = parent.Route,
                RouteParameter = parent.RouteParameter,
                Parameters = parent.Parameters,
                Response = result
            }, database);

            return result;
        }

        #endregion

        #region ProductChangeNotifications

        public async Task<string> ProductChangeNotifications(string digikeyPartNumber, T3 database, string[]? includes = null, DateTime? afterDate = null)
        {
            var parent = new ProductChangeNotificationsParent(digikeyPartNumber, includes);

            var existing = _clientService.RequestQuerySave.Query(parent.Route, parent.RouteParameter, database, afterDate);
            if (existing != null)
                return existing;

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            string result = _clientService.GetServiceResponse(response).Result;

            _clientService.RequestQuerySave.Save(new()
            {
                Route = parent.Route,
                RouteParameter = parent.RouteParameter,
                Parameters = parent.Parameters,
                Response = result
            }, database);

            return result;
        }

        #endregion

        #region ProductTracing

        public async Task<string> ProductTracingDetails(string tracingID, T3 database, DateTime? afterDate = null)
        {
            var parent = new ProductTracingDetailsParent(tracingID);

            var existing = _clientService.RequestQuerySave.Query(parent.Route, parent.RouteParameter, database, afterDate);
            if (existing != null)
                return existing;

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var response = await _clientService.GetAsync(parent.Path);
            string result = _clientService.GetServiceResponse(response).Result;

            _clientService.RequestQuerySave.Save(new()
            {
                Route = parent.Route,
                RouteParameter = parent.RouteParameter,
                Parameters = parent.Parameters,
                Response = result
            }, database);

            return result;
        }

        #endregion
    }
}
