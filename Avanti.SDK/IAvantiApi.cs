using System.Net.Http;
using System.Threading.Tasks;

using Avanti.SDK.Models.Authentication;

namespace Avanti.SDK
{
    public interface IAvantiApi
    {
        Task<AvantiToken> GetTokenAsync(AvantiCredentials credentials);

        Task<HttpResponseMessage> GetAsync(string resource);

        Task<HttpResponseMessage> PostAsync(string resource, HttpContent content);

        Task<HttpResponseMessage> PutAsync(string resource, HttpContent content);

        Task<HttpResponseMessage> DeleteAsync(string resource);

    }
}