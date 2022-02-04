using System.Text.Json.Serialization;

namespace Avanti.SDK.Models.Authentication
{
    public sealed class AuthenticationFailedResponse : AuthenticationResponse
    {
        public AuthenticationFailedResponse()
        {
            StatusCode = 400;
        }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; }
    }
}