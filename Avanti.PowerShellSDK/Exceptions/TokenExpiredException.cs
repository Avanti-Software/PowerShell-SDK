using System;

namespace Avanti.PowerShellSDK.Exceptions
{
    internal sealed class TokenExpiredException : Exception
    {
        public TokenExpiredException()
            : base("Access token has expired.")
        { }
    }
}
