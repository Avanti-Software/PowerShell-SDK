using System.Management.Automation;
using System.Net.Http;

namespace Avanti.PowerShellSDK.Commands
{
    public abstract class BaseAvantiCmdlet : PSCmdlet
    {
        private const string BaseUrl = "https://avanti.ca/{0}-api";

        protected readonly HttpClient httpClient;
    }
}
