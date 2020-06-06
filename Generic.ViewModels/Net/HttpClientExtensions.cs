using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GenericViewModels.Net
{
    public static class HttpClientExtensions
    {
        public static async Task<IEnumerable<T>> GetItemsAsync<T>(this HttpClient httpClient, string url)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (url == null) throw new ArgumentNullException(nameof(url));

            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);
            return new JsonSerializer().Deserialize<IEnumerable<T>>(jsonReader);
        }

        public static async Task<T> GetItemAsync<T>(this HttpClient httpClient, string url)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (url == null) throw new ArgumentNullException(nameof(url));

            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);
            return new JsonSerializer().Deserialize<T>(jsonReader);
        }

        public static async Task<T> AddItemAsync<T>(this HttpClient httpClient, string url, T item)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (item == null) throw new ArgumentNullException(nameof(item));

            string json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);
            return new JsonSerializer().Deserialize<T>(jsonReader);
        }

        public static async Task UpdateItemAsync<T>(this HttpClient httpClient, string url, T item)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (item == null) throw new ArgumentNullException(nameof(item));

            string json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
        }

        public static async Task<T> DeleteItemAsync<T>(this HttpClient httpClient, string url)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (url == null) throw new ArgumentNullException(nameof(url));

            HttpResponseMessage response = await httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);
            return new JsonSerializer().Deserialize<T>(jsonReader);
        }
    }
}
