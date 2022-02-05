using System;
using System.Management.Automation;
using System.Net.Http;
using System.Threading.Tasks;

using Avanti.PowerShellSDK.Core;
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
            _avantiApi = new AvantiApi(BaseUrl);

            SessionState.PSVariable.Set(Constants.AuthenticationKey, null);
        }

        protected override void ProcessRecord()
        {
            var @object = ProcessRecordAsync();

            WriteObject(@object);
        }

        private async Task<AvantiToken> GetToken()
        {
            try
            {
                AvantiToken token = await _avantiApi.GetTokenAsync(new AvantiCredentials
                {
                    ClientId = ClientId,
                    ClientSecret = ClientSecret,
                    Company = Company,
                    UserName = UserName,
                    Password = Password
                });

                SessionState.PSVariable.Set(Constants.AuthenticationKey, new AuthenticationState
                {
                    BaseUrl = BaseUrl,
                    ExpiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn),
                    Token = token.AccessToken
                });

                return token;
            }
            catch (HttpRequestException exception)
            {
                ThrowTerminatingError(new ErrorRecord(
                    exception,
                    exception.HResult.ToString(),
                    ErrorCategory.ConnectionError,
                    null));

                return null;
            }
        }

        private AvantiToken ProcessRecordAsync()
        {
            var task = GetToken();

            task.Wait();

            return task.Result;
        }
    }
}