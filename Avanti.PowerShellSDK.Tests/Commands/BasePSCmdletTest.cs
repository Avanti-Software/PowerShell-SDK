using Avanti.PowerShellSDK.Tests.Internal;

namespace Avanti.PowerShellSDK.Tests.Commands
{
    public abstract class BasePSCmdletTest
    {
        protected internal PowerShellEmulator Emulator { get; set; }

        protected BasePSCmdletTest()
        {
            Emulator = new PowerShellEmulator();
        }
    }
}