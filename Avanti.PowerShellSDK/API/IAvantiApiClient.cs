using System.Threading.Tasks;

using Avanti.PowerShellSDK.API.DTO.Authentication;

namespace Avanti.PowerShellSDK.API
{
    public interface IAvantiApiClient
    {
        Task<AuthenticationResponseDto> Authenticate(AvantiCredentials credentials);
    }
}