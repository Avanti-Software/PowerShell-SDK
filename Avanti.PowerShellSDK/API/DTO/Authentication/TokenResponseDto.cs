using System.Text.Json.Serialization;

namespace Avanti.PowerShellSDK.API.DTO.Authentication
{
    public sealed class TokenResponseDto : AuthenticationResponseDto
    {
        public TokenResponseDto()
        {
            StatusCode = 200;
        }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("auth_state")]
        public int AuthenticationState { get; set; }

        [JsonPropertyName("company")]
        public string Company { get; set; }
    }
}