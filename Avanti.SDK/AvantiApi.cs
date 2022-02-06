using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

using Polly;
using Polly.Wrap;

using Avanti.SDK.Internal;
using Avanti.SDK.Models.Authentication;

namespace Avanti.SDK
{
    public class AvantiApi : IAvantiApi
    {
        private readonly AvantiCredentials _credentials;
        private readonly HttpClient _httpClient;

        private AsyncPolicyWrap<HttpResponseMessage> _effectivePolicy;

        public AvantiApi(AvantiCredentials credentials)
            : this(credentials, Constants.DefaultBaseUrl)
        { }

        public AvantiApi(AvantiCredentials credentials, string baseUrl)
        {
            _credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            _httpClient.DefaultRequestHeaders.Add("User-Agent", Constants.UserAgent);

            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            InitializePolicies();
        }

        public AvantiApi(AvantiToken token)
            : this(token, Constants.DefaultBaseUrl)
        { }

        public AvantiApi(AvantiToken token, string baseUrl)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            _httpClient.DefaultRequestHeaders.Add("User-Agent", Constants.UserAgent);

            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.AccessToken);

            InitializePolicies();
        }

        private void InitializePolicies()
        {
            HttpStatusCode[] httpStatusCodesWorthRetrying = {
                HttpStatusCode.RequestTimeout,
                HttpStatusCode.InternalServerError,
                HttpStatusCode.BadGateway,
                HttpStatusCode.ServiceUnavailable,
                HttpStatusCode.GatewayTimeout
            };

            var tokenPolicy = Policy
                .HandleResult<HttpResponseMessage>(response =>
                {
                    return response.StatusCode == HttpStatusCode.Unauthorized;
                })
                .RetryAsync(1, async (exception, retryCount) =>
                {
                    AvantiToken token = await GetTokenAsync();

                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token.AccessToken);
                });

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(retryCount: 3, retryNumber =>
                {
                    return TimeSpan.FromMilliseconds(300 * retryNumber);
                });

            _effectivePolicy = tokenPolicy.WrapAsync(retryPolicy);
        }

        internal async Task<AvantiToken> GetTokenAsync()
        {
            HttpContent content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", _credentials.ClientId),
                new KeyValuePair<string, string>("client_secret", _credentials.ClientSecret),
                new KeyValuePair<string, string>("company", _credentials.Company),
                new KeyValuePair<string, string>("device_id", _credentials.DeviceId),
                new KeyValuePair<string, string>("grant_type", _credentials.GrantType),
                new KeyValuePair<string, string>("username", _credentials.UserName),
                new KeyValuePair<string, string>("password", _credentials.Password)
            });

            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded")
            {
                CharSet = "UTF-8"
            };

            HttpResponseMessage response = await PostAsync("/api/connect/token", content);

            return JsonSerializer.Deserialize<AvantiToken>(
                await response.Content.ReadAsStreamAsync());
        }

        public Task<HttpResponseMessage> GetAsync(string resource)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, resource);

            return _effectivePolicy.ExecuteAsync(() =>
                _httpClient.SendAsync(request));
        }

        public Task<HttpResponseMessage> PostAsync(string resource, HttpContent content)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, resource)
            {
                Content = content
            };

            return _effectivePolicy.ExecuteAsync(() =>
                _httpClient.SendAsync(request));
        }

        public Task<HttpResponseMessage> PutAsync(string resource, HttpContent content)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, resource)
            {
                Content = content
            };

            return _effectivePolicy.ExecuteAsync(() =>
                _httpClient.SendAsync(request));
        }

        public Task<HttpResponseMessage> DeleteAsync(string resource)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, resource);

            return _effectivePolicy.ExecuteAsync(() =>
                _httpClient.SendAsync(request));
        }
    }
}