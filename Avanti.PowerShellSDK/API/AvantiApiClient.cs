using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

using Avanti.PowerShellSDK.API.DTO.Authentication;
using Avanti.PowerShellSDK.Core;

namespace Avanti.PowerShellSDK.API
{
    public sealed class AvantiApiClient : IAvantiApiClient
    {
        private readonly HttpClient _httpClient;

        public AvantiApiClient(string baseUrl = null)
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

        public async Task<AuthenticationResponseDto> Authenticate(AvantiCredentials credentials)
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

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/connect/token")
            {
                Content = new FormUrlEncodedContent(payload)
            };

            try
            {
                var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<TokenResponseDto>(
                    await response.Content.ReadAsStreamAsync());
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }
    }
}