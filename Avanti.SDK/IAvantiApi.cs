using System.Threading.Tasks;

using Avanti.SDK.Models.Authentication;

namespace Avanti.SDK
{
    public interface IAvantiApi
    {
        Task<AuthenticationResponse> Authenticate(AvantiCredentials credentials);
    }
}