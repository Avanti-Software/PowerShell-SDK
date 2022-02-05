using System;

namespace Avanti.PowerShellSDK.State
{
    /// <summary>
    /// Represents the persisted authentication for Avanti Cmdlets in a pipeline.
    /// </summary>
    public class AuthenticationState
    {
        /// <summary>
        /// The default or custom base URL.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// The application access token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The point in time the token expires.
        /// </summary>
        public DateTimeOffset ExpiresAt { get; set; }
    }
}
