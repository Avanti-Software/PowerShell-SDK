using System.Text.Json.Serialization;

namespace Avanti.PowerShellSDK.API.DTO.Authentication
{
    public sealed class AuthenticationFailedResponseDto : AuthenticationResponseDto
    {
        public AuthenticationFailedResponseDto()
        {
            StatusCode = 400;
        }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; }
    }
}