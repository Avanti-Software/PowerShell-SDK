using System;

namespace Avanti.PowerShellSDK.Exceptions
{
    internal sealed class AuthenticationStateException : Exception
    {
        public AuthenticationStateException()
            : base("Expected session state token is missing. Use Get-AvantiToken to authenticate.")
        { }
    }
}