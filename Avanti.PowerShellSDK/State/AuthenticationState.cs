using System;

namespace Avanti.PowerShellSDK.State
{
    public class AuthenticationState
    {
        public string BaseUrl { get; set; }

        public DateTimeOffset ExpiresAt { get; set; }

        public string Token { get; set; }
    }
}
