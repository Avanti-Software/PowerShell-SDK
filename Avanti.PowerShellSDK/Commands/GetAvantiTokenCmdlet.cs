using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

using Avanti.PowerShellSDK.Internal;

using Avanti.SDK;
using Avanti.SDK.Models.Authentication;

[assembly: InternalsVisibleTo("Avanti.PowerShellSDK.Tests")]

namespace Avanti.PowerShellSDK.Commands
{
    [Cmdlet(VerbsCommon.Get, "AvantiToken")]
    [OutputType(typeof(AvantiToken))]
    public sealed class GetAvantiTokenCmdlet : PSCmdlet
    {
        private IAvantiApi _avantiApi;

        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string UserName { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 1,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string Password { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 2,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string Company { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 3,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string ClientId { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 4,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string ClientSecret { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 5,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string BaseUrl { get; set; }

        internal void ProcessInternal()
        {
            BeginProcessing();
            ProcessRecord();
            EndProcessing();
        }

        protected override void BeginProcessing()
        {
            _avantiApi = new AvantiApi(new AvantiCredentials
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Company = Company,
                UserName = UserName,
                Password = Password
            }, BaseUrl);
        }

        protected override void ProcessRecord()
        {
            var @object = ProcessRecordAsync();

            WriteObject(@object);
        }

        private AvantiToken ProcessRecordAsync()
        {
            var task = GetToken();
            task.Wait();

            return task.Result;
        }

        private async Task<AvantiToken> GetToken()
        {
            HttpContent content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("company", Company),
                new KeyValuePair<string, string>("device_id", Constants.UserAgent),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", UserName),
                new KeyValuePair<string, string>("password", Password)
            });

            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded")
            {
                CharSet = "UTF-8"
            };

            HttpResponseMessage response = await _avantiApi.PostAsync("/api/connect/token", content);

            if (!response.IsSuccessStatusCode)
            {
                ThrowTerminatingError(new ErrorRecord(
                    new HttpRequestException(response.ReasonPhrase),
                    Constants.AuthenticationErrorId,
                    ErrorCategory.AuthenticationError,
                    response));
            }

            return JsonSerializer.Deserialize<AvantiToken>(
                await response.Content.ReadAsStreamAsync());
        }
    }
}