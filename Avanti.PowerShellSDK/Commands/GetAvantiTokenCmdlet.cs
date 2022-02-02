using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text.Json;

using Avanti.PowerShellSDK.Core;
using Avanti.PowerShellSDK.Models;
using Avanti.PowerShellSDK.State;

namespace Avanti.PowerShellSDK.Commands
{
    [Cmdlet(VerbsCommon.Get, "AvantiToken")]
    [OutputType(typeof(GetAvantiTokenResponse))]
    public sealed class GetAvantiTokenCmdlet : PSCmdlet
    {
        private HttpClient _httpClient;

        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string UserName { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 1,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string Password { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 2,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string Company { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 3,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string ClientId { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 4,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string ClientSecret { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 5,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string BaseUrl { get; set; }

        protected override void BeginProcessing()
        {
            if (string.IsNullOrEmpty(BaseUrl))
            {
                BaseUrl = Constants.DefaultBaseUrl;
            }

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };

            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
            _httpClient.DefaultRequestHeaders.Add("User-Agent", Constants.UserAgent);

            SessionState.PSVariable.Set(Constants.AuthenticationKey, null);
        }

        protected override void ProcessRecord()
        {
            var payload = new[]
            {
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("company", Company),
                new KeyValuePair<string, string>("device_id", Constants.UserAgent),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", UserName),
                new KeyValuePair<string, string>("password", Password)
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/connect/token")
            {
                Content = new FormUrlEncodedContent(payload)
            };

            var response = _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                .GetAwaiter()
                .GetResult();

            if (response.IsSuccessStatusCode is false)
            {
                ThrowTerminatingError(new ErrorRecord(
                    new SecurityException(),
                    Constants.MissingTokenErrorId,
                    ErrorCategory.AuthenticationError,
                    null));
            }

            var stream = response.Content.ReadAsStreamAsync()
                .GetAwaiter()
                .GetResult();

            var tokenResponse = JsonSerializer.Deserialize<GetAvantiTokenResponse>(stream);

            SessionState.PSVariable.Set(Constants.AuthenticationKey, new AuthenticationState
            {
                BaseUrl = BaseUrl,
                ExpiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn),
                Token = tokenResponse.AccessToken
            });

            WriteObject(tokenResponse);
        }
    }
}
