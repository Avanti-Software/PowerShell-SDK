namespace Avanti.SDK.Models.Authentication
{
    public sealed class AvantiCredentials
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Company { get; set; }

        public string DeviceId { get; set; } = Constants.UserAgent;

        public string GrantType { get; set; } = "password";

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}