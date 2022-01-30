using System.Management.Automation;

namespace Avanti.PowerShellSDK.Commands
{
    public abstract class BaseAuthenticatedCmdlet : PSCmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }
    }
}
