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

        public async Task<JsonElement> KeywordSearchJson(string keyword, string[]? includes = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await KeywordSearch(keyword, includes));
        }

        public async Task<JsonElement> ProductDetailsJson(string digikeyPartNumber, string[]? includes = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await ProductDetails(digikeyPartNumber, includes));
        }

        public async Task<JsonElement> DigiReelPricingJson(string digikeyPartNumber, int requestedQuantity, string[]? includes = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await DigiReelPricing(digikeyPartNumber, requestedQuantity, includes));
        }

        public async Task<JsonElement> SuggestedPartsJson(string partNumber)
        {
            return JsonSerializer.Deserialize<JsonElement>(await SuggestedParts(partNumber));
        }
        public async Task<JsonElement> ManufacturersJson()
        {
            return JsonSerializer.Deserialize<JsonElement>(await Manufacturers());
        }

        public async Task<JsonElement> CategoriesJson()
        {
            return JsonSerializer.Deserialize<JsonElement>(await Categories());
        }

        public async Task<JsonElement> CategoriesByIDJson(int categoryID)
        {
            return JsonSerializer.Deserialize<JsonElement>(await CategoriesByID(categoryID));
        }

        #endregion

        #region RecommendedParts

        public async Task<JsonElement> RecommendedProductsJson(string digikeyPartNumber, int recordCount = 1, string[]? searchOptionList = null, bool excludeMarketPlaceProducts = false, string[]? includes = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await RecommendedProducts(digikeyPartNumber, recordCount, searchOptionList, excludeMarketPlaceProducts, includes));
        }

        #endregion

        #region PackageTypeByQuantity

        public async Task<JsonElement> PackageByQuantityJson(string digikeyPartNumber, int requestedQuantity, string? packagingPreference = null, string[]? includes = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await PackageByQuantity(digikeyPartNumber, requestedQuantity, packagingPreference, includes));
        }

        #endregion

        #region ProductChangeNotifications

        public async Task<JsonElement> ProductChangeNotificationsJson(string digikeyPartNumber, string[]? includes = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await ProductChangeNotifications(digikeyPartNumber, includes));
        }

        #endregion

        #region ProductTracing

        public async Task<JsonElement> ProductTracingDetailsJson(string tracingID)
        {
            return JsonSerializer.Deserialize<JsonElement>(await ProductTracingDetails(tracingID));
        }

        #endregion
    }

    public partial class ProductInformation<T3, T4>
    {
        #region PartSearch

        public async Task<JsonElement> KeywordSearchJson(string keyword, T3 database, string[]? includes = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await KeywordSearch(keyword, database, includes));
        }

        public async Task<JsonElement> ProductDetailsJson(string digikeyPartNumber, T3 database, string[]? includes = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await ProductDetails(digikeyPartNumber, database, includes));
        }

        public async Task<JsonElement> DigiReelPricingJson(string digikeyPartNumber, int requestedQuantity, T3 database, string[]? includes = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await DigiReelPricing(digikeyPartNumber, requestedQuantity, database, includes));
        }

        public async Task<JsonElement> SuggestedPartsJson(string partNumber, T3 database)
        {
            return JsonSerializer.Deserialize<JsonElement>(await SuggestedParts(partNumber, database));
        }
        public async Task<JsonElement> ManufacturersJson(T3 database)
        {
            return JsonSerializer.Deserialize<JsonElement>(await Manufacturers(database));
        }

        public async Task<JsonElement> CategoriesJson(T3 database)
        {
            return JsonSerializer.Deserialize<JsonElement>(await Categories(database));
        }

        public async Task<JsonElement> CategoriesByIDJson(int categoryID, T3 database)
        {
            return JsonSerializer.Deserialize<JsonElement>(await CategoriesByID(categoryID, database));
        }

        #endregion

        #region RecommendedParts

        public async Task<JsonElement> RecommendedProductsJson(string digikeyPartNumber, T3 database, int recordCount = 1, string[]? searchOptionList = null, bool excludeMarketPlaceProducts = false, string[]? includes = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await RecommendedProducts(digikeyPartNumber, database, recordCount, searchOptionList, excludeMarketPlaceProducts, includes));
        }

        #endregion

        #region PackageTypeByQuantity

        public async Task<JsonElement> PackageByQuantityJson(string digikeyPartNumber, int requestedQuantity, T3 database, string? packagingPreference = null, string[]? includes = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await PackageByQuantity(digikeyPartNumber, requestedQuantity, database, packagingPreference, includes));
        }

        #endregion

        #region ProductChangeNotifications

        public async Task<JsonElement> ProductChangeNotificationsJson(string digikeyPartNumber, T3 database, string[]? includes = null)
        {
            return JsonSerializer.Deserialize<JsonElement>(await ProductChangeNotifications(digikeyPartNumber, database, includes));
        }

        #endregion

        #region ProductTracing

        public async Task<JsonElement> ProductTracingDetailsJson(string tracingID, T3 database)
        {
            return JsonSerializer.Deserialize<JsonElement>(await ProductTracingDetails(tracingID, database));
        }

        #endregion
    }
}