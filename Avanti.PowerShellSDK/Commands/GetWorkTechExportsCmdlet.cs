using System.Management.Automation;
using System.Runtime.CompilerServices;

using Avanti.SDK.Models.WorkTech;

[assembly: InternalsVisibleTo("Avanti.PowerShellSDK.Tests")]

namespace Avanti.PowerShellSDK.Commands
{
    [Cmdlet(VerbsCommon.Get, "AvantiWorkTechExports")]
    [OutputType(typeof(WorkTechExport))]
    public sealed class GetWorkTechExportsCmdlet : PSCmdlet
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