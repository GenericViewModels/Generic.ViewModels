using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenericViewModels.Net
{
    public class HttpHelper<T> : IHttpHelper<T>, IDisposable
    {
        private HttpClient _httpClient;
        private HttpClient HttpClient
        {
            //get => _httpClient ?? (_httpClient = new HttpClient(new RetryHandler(new HttpClientHandler())));
            get => _httpClient ?? (_httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false }));
        }

        public async Task<IEnumerable<T>> GetItemsAsync(string url)
        {
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            IEnumerable<T> items = JsonConvert.DeserializeObject<IEnumerable<T>>(json);

            return items;
        }

        public async Task<T> GetItemAsync(string url)
        {
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            T item = JsonConvert.DeserializeObject<T>(json);

            return item;
        }

        public async Task<T> AddItemAsync(string url, T item)
        {
            string json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await HttpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string jsonResult = await response.Content.ReadAsStringAsync();
            T itemResult = JsonConvert.DeserializeObject<T>(jsonResult);
            return itemResult;
        }

        public async Task UpdateItemAsync(string url, T item)
        {
            string json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await HttpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<T> DeleteItemAsync(string url)
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            string jsonResult = await response.Content.ReadAsStringAsync();
            T itemResult = JsonConvert.DeserializeObject<T>(jsonResult);
            return itemResult;
        }

        public void Dispose() => _httpClient?.Dispose();
    }

    public class RetryHandler : DelegatingHandler
    {
        private const int MaxRetries = 3;

        public RetryHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        { }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            for (int i = 0; i < MaxRetries; i++)
            {
                response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {

                    return response;
                }
                else
                {
                    // TODO: log errors, do not retry with every error
                    await Task.Delay(500);
                }
            }

            return response;
        }
    }
}
