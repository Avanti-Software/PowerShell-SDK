using Microsoft.VisualStudio.TestTools.UnitTesting;

using Avanti.PowerShellSDK.Commands;
using Avanti.PowerShellSDK.Tests.Internal;

namespace Avanti.PowerShellSDK.Tests.Commands
{
    [TestClass]
    public class GetAvantiTokenCmdletTests
    {
        [TestMethod]
        public void GetAvantiTokenCmdlet_ValidAuthenticationFlow()
        {
            GetAvantiTokenCmdlet cmdlet = new()
            {
                Company = "avtesting",
                ClientId = "12345"
            };

            cmdlet.Invoke();

            Assert.IsNotNull(cmdlet);
        }

        public void GetAvantiTokenCmdlet_Emulator()
        {
            var emulator = new PowerShellEmulator();

            var cmdlet = new GetAvantiTokenCmdlet
            {
                BaseUrl = "http://192.168.1.2",
                UserName = "ASTIRTAN",
                Password = "0j@@83$tiRt",
                Company = "ADAM",
                ClientId = "ASI010962",
                ClientSecret = "CAD8E837-833F-4F1C-AE72-A9834997C814"
            };

            cmdlet.CommandRuntime = emulator;
            cmdlet.ProcessInternal();

            var results = emulator.OutputObjects;

            Assert.IsNotNull(results);
        }
    }
}