using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
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

        public async Task<AuthenticationResponse> Authenticate(AvantiCredentials credentials)
        {
            var payload = new[]
            {
                new KeyValuePair<string, string>("client_id", credentials.ClientId),
                new KeyValuePair<string, string>("client_secret", credentials.ClientSecret),
                new KeyValuePair<string, string>("company", credentials.Company),
                new KeyValuePair<string, string>("device_id", credentials.DeviceId),
                new KeyValuePair<string, string>("grant_type", credentials.GrantType),
                new KeyValuePair<string, string>("username", credentials.UserName),
                new KeyValuePair<string, string>("password", credentials.Password)
            };

            return await PostAsync<TokenResponse>("/api/connect/token", new FormUrlEncodedContent(payload));
        }

        private async Task<TDto> GetAsync<TDto>(string resource, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, resource);
            var response = await _httpClient.SendAsync(request, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return JsonSerializer.Deserialize<TDto>(await response.Content.ReadAsStreamAsync());
        }

        private async Task<TDto> PutAsync<TDto>(string resource, TDto dto, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, resource)
            {
                Content = new StringContent(JsonSerializer.Serialize(dto))
            };

            var response = await _httpClient.SendAsync(request, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return JsonSerializer.Deserialize<TDto>(await response.Content.ReadAsStreamAsync());
        }

        private async Task<TDto> PostAsync<TDto>(string resource, TDto dto, CancellationToken cancellationToken = default)
        {
            return await PostAsync<TDto>(resource, new StringContent(JsonSerializer.Serialize(dto)), cancellationToken);
        }

        private async Task<TDto> PostAsync<TDto>(string resource, HttpContent content, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, resource)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return JsonSerializer.Deserialize<TDto>(await response.Content.ReadAsStreamAsync());
        }

        private async Task<bool> DeleteAsync(string resource, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, resource);
            var response = await _httpClient.SendAsync(request, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}