using System;
using System.Security;

namespace Avanti.PowerShellSDK.State
{
    public class AuthenticationState
    {
        public string BaseUrl { get; set; }

        public DateTimeOffset ExpiresAt { get; set; }

        public SecureString Token { get; set; }
    }
}
