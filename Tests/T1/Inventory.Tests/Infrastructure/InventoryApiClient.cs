using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Inventory.Tests.Infrastructure
{
    internal class InventoryApiClient
    {
        private readonly HttpClient _client;

        public InventoryApiClient(HttpClient client)
        {
            _client = client;
        }

        // --------------------
        // GET
        // --------------------
        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _client.GetAsync(url);
            return await ResolveResponse<T>(response);
        }

        public async Task<HttpResponseMessage> GetRawAsync(string url)
        {
            return await _client.GetAsync(url);
        }

        // --------------------
        // POST
        // --------------------
        public async Task<T> PostAsync<T>(string url, object body)
        {
            var content = Serialize(body);
            var response = await _client.PostAsync(url, content);
            return await ResolveResponse<T>(response);
        }

        public async Task<HttpResponseMessage> PostRawAsync(string url, object? body = null)
        {
            var json = body != null ? JsonConvert.SerializeObject(body) : "";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _client.PostAsync(url, content);
        }

        // --------------------
        // PUT
        // --------------------
        public async Task<HttpResponseMessage> PutRawAsync(string url, object body)
        {
            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _client.PutAsync(url, content);
        }

        public async Task<T> PutAsync<T>(string url, object body)
        {
            var content = Serialize(body);
            var response = await _client.PutAsync(url, content);
            return await ResolveResponse<T>(response);
        }

        // --------------------
        // DELETE
        // --------------------
        public async Task<HttpResponseMessage> DeleteRawAsync(string url)
        {
            return await _client.DeleteAsync(url);
        }

        // --------------------
        // RESPONSE HANDLING
        // --------------------
        private async Task<T> ResolveResponse<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                if (string.IsNullOrWhiteSpace(json))
                    return default!;

                return JsonConvert.DeserializeObject<T>(json)!;
            }

            throw new Exception($"API Error {(int)response.StatusCode}: {json}");
        }

        private static StringContent Serialize(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
