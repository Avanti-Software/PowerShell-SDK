using System;
using System.Management.Automation;
using System.Security;

using Avanti.PowerShellSDK.Core;
using Avanti.PowerShellSDK.State;

namespace Avanti.PowerShellSDK.Commands
{
    public abstract class BaseAuthenticatedCmdlet : PSCmdlet
    {
        protected AuthenticationState AuthenticationState;

        protected override void BeginProcessing()
        {
            AuthenticationState authenticationState =
                SessionState.PSVariable.GetValue(Constants.AuthenticationKey, null)
                    as AuthenticationState;

            if (authenticationState is null)
            {
                ThrowTerminatingError(new ErrorRecord(
                    new SecurityException(),
                    Constants.MissingTokenErrorId,
                    ErrorCategory.AuthenticationError,
                    authenticationState));
            }

            if (authenticationState.ExpiresAt >= DateTime.UtcNow)
            {
                SessionState.PSVariable.Set(Constants.AuthenticationKey, null);

                ThrowTerminatingError(new ErrorRecord(
                    new SecurityException(),
                    Constants.ExpiredTokenErrorId,
                    ErrorCategory.AuthenticationError,
                    authenticationState));
            }

            AuthenticationState = authenticationState;
        }
    }
}
