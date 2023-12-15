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

using System.Text.Json;

namespace ApiClient.API
{
    public partial class ProductInformation
    {
        #region PartSearch

        public async Task<JsonElement> KeywordSearchJson(string keyword, string[]? includes = null, DateTime? afterDate = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await KeywordSearch(keyword, includes, afterDate));
        }

        public async Task<JsonElement> ProductDetailsJson(string digikeyPartNumber, string[]? includes = null, DateTime? afterDate = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await ProductDetails(digikeyPartNumber, includes, afterDate));
        }

        public async Task<JsonElement> DigiReelPricingJson(string digikeyPartNumber, int requestedQuantity, string[]? includes = null, DateTime? afterDate = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await DigiReelPricing(digikeyPartNumber, requestedQuantity, includes, afterDate));
        }

        public async Task<JsonElement> SuggestedPartsJson(string partNumber, DateTime? afterDate = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await SuggestedParts(partNumber, afterDate));
        }
        public async Task<JsonElement> ManufacturersJson(DateTime? afterDate = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await Manufacturers(afterDate));
        }

        public async Task<JsonElement> CategoriesJson(DateTime? afterDate = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await Categories(afterDate));
        }

        public async Task<JsonElement> CategoriesByIDJson(int categoryID, DateTime? afterDate = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await CategoriesByID(categoryID, afterDate));
        }

        #endregion

        #region RecommendedParts

        public async Task<JsonElement> RecommendedProductsJson(string digikeyPartNumber, int recordCount = 1, string[]? searchOptionList = null, bool excludeMarketPlaceProducts = false, string[]? includes = null, DateTime? afterDate = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await RecommendedProducts(digikeyPartNumber, recordCount, searchOptionList, excludeMarketPlaceProducts, includes, afterDate));
        }

        #endregion

        #region PackageTypeByQuantity

        public async Task<JsonElement> PackageByQuantityJson(string digikeyPartNumber, int requestedQuantity, string? packagingPreference = null, string[]? includes = null, DateTime? afterDate = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await PackageByQuantity(digikeyPartNumber, requestedQuantity, packagingPreference, includes, afterDate));
        }

        #endregion

        #region ProductChangeNotifications

        public async Task<JsonElement> ProductChangeNotificationsJson(string digikeyPartNumber, string[]? includes = null, DateTime? afterDate = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await ProductChangeNotifications(digikeyPartNumber, includes, afterDate));
        }

        #endregion

        #region ProductTracing

        public async Task<JsonElement> ProductTracingDetailsJson(string tracingID, DateTime? afterDate = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await ProductTracingDetails(tracingID, afterDate));
        }

        #endregion
    }
}