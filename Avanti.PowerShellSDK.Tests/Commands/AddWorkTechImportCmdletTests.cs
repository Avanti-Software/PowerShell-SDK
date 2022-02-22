using System;

using Avanti.PowerShellSDK.Commands;
using Avanti.PowerShellSDK.Tests.Internal;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Avanti.PowerShellSDK.Tests.Commands
{
    [TestClass]
    public class AddWorkTechImportCmdletTests
    {
        public void AddWorkTechImport()
        {
            var emulator = new PowerShellEmulator();

            var cmdlet = new AddWorkTechImportCmdlet
            {
                AccessToken = "y6hqP2OUKQUP33X9D7llIKM-2yCTw4OtQeuTyBonXQ0",
                TransactionNo = 12345,
                BatchNo = 12345,
                EmployeeNo = "12345678",
                WorkDateTime = DateTime.UtcNow,
                Hours = 10,
                PayCode = "pc12345",
                ShiftId = "wt12345",
                WorkTechJob = "job12345",
                WorkTechActivity = "Activity",
                TaskId = "task1234",
                OTOption = 99,
                GLAccount = "GL-12345",
                Status = 5,
                ImportedBy = "Adam",
                ImportedDate = DateTime.UtcNow
            };

            cmdlet.CommandRuntime = emulator;
            cmdlet.ProcessInternal();

            var results = emulator.OutputObjects;

            Assert.IsNotNull(results);
        }
    }
}