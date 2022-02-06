﻿using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

using Avanti.PowerShellSDK.Internal;
using Avanti.PowerShellSDK.State;

using Avanti.SDK;
using Avanti.SDK.Models.Authentication;

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
            _avantiApi = new AvantiApi(new AvantiCredentials
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Company = Company,
                UserName = UserName,
                Password = Password
            }, BaseUrl);

            SessionState.PSVariable.Set(Constants.AuthenticationKey, null);
        }

        protected override void ProcessRecord()
        {
            var @object = ProcessRecordAsync();

            WriteObject(@object);
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

            AvantiToken token = JsonSerializer.Deserialize<AvantiToken>(
                await response.Content.ReadAsStreamAsync());

            SessionState.PSVariable.Set(Constants.AuthenticationKey, new AuthenticationState
            {
                BaseUrl = BaseUrl,
                ExpiresAt = DateTimeOffset.Now.AddSeconds(token.ExpiresIn),
                Token = token.AccessToken
            });

            return token;
        }

        private AvantiToken ProcessRecordAsync()
        {
            var task = GetToken();

            task.Wait();

            return task.Result;
        }
    }
}