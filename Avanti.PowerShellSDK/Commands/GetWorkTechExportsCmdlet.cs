using System.Management.Automation;

using Avanti.SDK.Models.WorkTech;

namespace Avanti.PowerShellSDK.Commands
{
    [Cmdlet(VerbsCommon.Get, "AvantiWorkTechExports")]
    [OutputType(typeof(WorkTechExport))]
    public sealed class GetWorkTechExportsCmdlet : BaseAuthenticatedCmdlet
    {
        [Parameter(
            Mandatory = false,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string AccessToken { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
        }
    }
}
