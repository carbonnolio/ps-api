using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
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

        public async Task<T> GetInventory<T>(string url) =>
            await GetData<T>(url);

        private async Task<T> GetData<T>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            try
            {
                var response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var jObject = JObject.Parse(content);

                if (!jObject.TryGetValue("statusCode", out var statusCode) ||
                    (int)statusCode != 200)
                {
                    // throw exception here..
                }

                if (jObject.TryGetValue("body", out var body))
                {
                    return body.ToObject<T>();
                }
            }
            catch (Exception e)
            {
                // Handle exception here...
                throw;
            }

            throw new ApplicationException();
        }
    }
}
