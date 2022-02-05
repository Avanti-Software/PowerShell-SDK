using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

using Avanti.SDK.Models.Authentication;

namespace Avanti.SDK
{
    public sealed class AvantiApi : IAvantiApi
    {
        private readonly HttpClient _httpClient;

        public AvantiApi(string baseUrl = null)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl ?? Constants.DefaultBaseUrl)
            };

            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Add("User-Agent", Constants.UserAgent);
        }

        public async Task<AvantiToken> GetTokenAsync(AvantiCredentials credentials)
        {
            HttpResponseMessage response = await PostAsync("/api/connect/token", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", credentials.ClientId),
                new KeyValuePair<string, string>("client_secret", credentials.ClientSecret),
                new KeyValuePair<string, string>("company", credentials.Company),
                new KeyValuePair<string, string>("device_id", credentials.DeviceId),
                new KeyValuePair<string, string>("grant_type", credentials.GrantType),
                new KeyValuePair<string, string>("username", credentials.UserName),
                new KeyValuePair<string, string>("password", credentials.Password)
            }));

            return JsonSerializer.Deserialize<AvantiToken>(await response.Content.ReadAsStreamAsync());
        }

        public Task<HttpResponseMessage> GetAsync(string resource)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, resource);

            return _httpClient.SendAsync(request);
        }

        public Task<HttpResponseMessage> PostAsync(string resource, HttpContent content)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, resource)
            {
                Content = content
            };

            return _httpClient.SendAsync(request);
        }

        public Task<HttpResponseMessage> PutAsync(string resource, HttpContent content)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, resource)
            {
                Content = content
            };

            return _httpClient.SendAsync(request);
        }

        public Task<HttpResponseMessage> DeleteAsync(string resource)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, resource);

            return _httpClient.SendAsync(request);
        }
    }
}