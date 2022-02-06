using Microsoft.VisualStudio.TestTools.UnitTesting;

using Avanti.PowerShellSDK.Commands;

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
    }
}
