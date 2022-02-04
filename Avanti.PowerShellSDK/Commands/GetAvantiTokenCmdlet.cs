using System;
using System.Management.Automation;
using System.Security;
using System.Threading.Tasks;

using Avanti.PowerShellSDK.Core;
using Avanti.PowerShellSDK.Models;
using Avanti.PowerShellSDK.State;

using Avanti.SDK;
using Avanti.SDK.Models.Authentication;

namespace Avanti.PowerShellSDK.Commands
{
    [Cmdlet(VerbsCommon.Get, "AvantiToken")]
    [OutputType(typeof(TokenResponse))]
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
            GetAvantiTokenResponse @object = ProcessRecordAsync();

            WriteObject(@object);
        }

        private async Task<GetAvantiTokenResponse> GetToken()
        {
            var response = await _avantiApi.Authenticate(new AvantiCredentials
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Company = Company,
                UserName = UserName,
                Password = Password
            });

            if (response.StatusCode == 200)
            {
                TokenResponse tokenResponse = response as TokenResponse;

                SessionState.PSVariable.Set(Constants.AuthenticationKey, new AuthenticationState
                {
                    BaseUrl = BaseUrl,
                    ExpiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn),
                    Token = tokenResponse.AccessToken
                });

                return new GetAvantiTokenResponse
                {
                    AccessToken = tokenResponse.AccessToken,
                    AuthenticationState = tokenResponse.AuthenticationState,
                    Company = tokenResponse.Company,
                    ExpiresIn = tokenResponse.ExpiresIn,
                    Scope = tokenResponse.Scope,
                    TokenType = tokenResponse.TokenType
                };
            }

            ThrowTerminatingError(new ErrorRecord(
                new SecurityException(),
                Constants.AuthenticationFailedErrorId,
                ErrorCategory.ConnectionError,
                null));

            return null;
        }

        private GetAvantiTokenResponse ProcessRecordAsync()
        {
            var task = GetToken();

            task.Wait();

            return task.Result;
        }
    }
}