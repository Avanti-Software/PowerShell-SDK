using System;
using System.Management.Automation;

using Avanti.PowerShellSDK.Exceptions;
using Avanti.PowerShellSDK.Internal;
using Avanti.PowerShellSDK.State;
using Avanti.SDK;
using Avanti.SDK.Models.Authentication;

namespace Avanti.PowerShellSDK.Commands
{
    public abstract class BaseAuthenticatedCmdlet : PSCmdlet
    {
        protected IAvantiApi AvantiApi;

        protected internal AuthenticationState AuthenticationState;

        protected override void BeginProcessing()
        {
            AuthenticationState authenticationState =
                SessionState.PSVariable.GetValue(Constants.AuthenticationKey, null)
                    as AuthenticationState;

            if (authenticationState is null)
            {
                ThrowTerminatingError(new ErrorRecord(
                    new AuthenticationStateException(),
                    Constants.AuthenticationStateErrorId,
                    ErrorCategory.AuthenticationError,
                    authenticationState));
            }

            if (authenticationState.ExpiresAt >= DateTimeOffset.Now)
            {
                ThrowTerminatingError(new ErrorRecord(
                    new TokenExpiredException(),
                    Constants.TokenExpiredErrorId,
                    ErrorCategory.AuthenticationError,
                    authenticationState));
            }

            AuthenticationState = authenticationState;

            AvantiApi = new AvantiApi(new AvantiToken
            {
                AccessToken = AuthenticationState.Token
            }, authenticationState.BaseUrl);
        }
    }
}
