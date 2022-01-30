using System;
using System.Management.Automation;
using System.Net.Http;
using System.Text.Json;

using Avanti.PowerShellSDK.Models;

namespace Avanti.PowerShellSDK.Commands
{
    [Cmdlet(VerbsCommon.Get, "AvantiToken")]
    [OutputType(typeof(AvantiToken))]
    public sealed class GetAvantiTokenCmdlet : PSCmdlet
    {
        private HttpClient _httpClient;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            AllowTrailingCommas = false,
            PropertyNameCaseInsensitive = true
        };

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

        protected override void BeginProcessing()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://avanti.ca/{Company}-api")
            };

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Avanti-Software/PowerShell-SDK");
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            var request = new HttpRequestMessage(HttpMethod.Get, "/connect/token");
            var response = _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;

            response.EnsureSuccessStatusCode();

            var stream = response.Content.ReadAsStreamAsync().Result;

            WriteObject(JsonSerializer.Deserialize<AvantiToken>(stream, _jsonSerializerOptions));
        }
    }
}
