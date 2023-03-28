using ApiClient.Models;
using System.Web;

namespace ApiClient.EndpointAPI.ProductInformation
{
    public class PartSearch
    {
        private ApiClientService _clientService;

        public ApiClientService ClientService
        {
            get => _clientService;
            set => _clientService = value;
        }

        public PartSearch(ApiClientService clientService)
        {
            ClientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        }

        public async Task<string> KeywordSearch(string keyword)
        {
            var resourcePath = "/Search/v3/Products/Keyword";

            var request = new KeywordSearchRequest
            {
                Keywords = keyword ?? "P5555-ND",
                RecordCount = 25
            };

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var postResponse = await _clientService.PostAsJsonAsync(resourcePath, request);

            return ApiClientService.GetServiceResponse(postResponse).Result;
        }

        public async Task<string> ProductDetails(string digikeyPartNumber)
        {
            var resourcePath = "Search/v3/Products";

            var encodedPN = HttpUtility.UrlEncode(digikeyPartNumber);

            var fullPath = $"/{resourcePath}/{encodedPN}";

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var getResponse = await _clientService.GetAsync(fullPath);

            return ApiClientService.GetServiceResponse(getResponse).Result;
        }

        public async Task<string> DigiReelPricing(string digiKeyPartNumber, int requestedQuantity)
        {
            var resourcePathPrefix = "Search/v3/Products";
            var resourcePathSuffix = "DigiReelPricing";

            var encodedPN = HttpUtility.UrlEncode(digiKeyPartNumber);
            var parameters = $"requestedQuantity={requestedQuantity}";

            var fullPath = $"/{resourcePathPrefix}/{encodedPN}/{resourcePathSuffix}?{parameters}";

            await _clientService.ResetExpiredAccessTokenIfNeeded();
            var getResponse = await _clientService.GetAsync(fullPath);

            return ApiClientService.GetServiceResponse(getResponse).Result;
        }
    }
}