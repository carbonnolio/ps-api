using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using PetStore.Api.Exceptions;
using PetStore.Api.Settings;

namespace PetStore.Api.Services.Implementations
{
    public class InventoryClient : IInventoryClient
    {
        private readonly HttpClient _httpClient;

        public InventoryClient(HttpClient httpClient, IOptions<AppSettings> options)
        {
            var settings = options?.Value;

            if (string.IsNullOrWhiteSpace(settings?.InventoryApiUrl))
                throw new ArgumentException("Unable to create HTTP client. Resouce URL cannot be null or empty.");

            httpClient.BaseAddress = new Uri(settings.InventoryApiUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            _httpClient = httpClient;
        }

        public async Task<T> GetInventory<T>(string route, Action<int> errorHandler = null) =>
            await GetData<T>(route, errorHandler);

        private async Task<T> GetData<T>(string route, Action<int> errorHandler)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, route);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var jObject = JObject.Parse(content);

            if (!jObject.TryGetValue("statusCode", out var statusCode) ||
                (int)statusCode != 200)
            {
                var responseCode = (int)statusCode;

                errorHandler?.Invoke(responseCode);
            }

            if (jObject.TryGetValue("body", out var body))
            {
                return body.ToObject<T>();
            }
            else
            {
                throw new CustomHttpException(HttpStatusCode.InternalServerError, 
                    "Failed to parse Inventory API response.");
            }
        }
    }
}
