using System;
using System.Management.Automation;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

using Avanti.SDK.Models.WorkTech;

namespace Avanti.PowerShellSDK.Commands
{
    [Cmdlet(VerbsCommon.Add, "AvantiWorkTechImport")]
    [OutputType(typeof(WorkTechImport))]
    public sealed class AddWorkTechImportCmdlet : BaseAuthenticatedCmdlet
    {
        [Parameter(
            Mandatory = false,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string AccessToken { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 1,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public int TransactionNo { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 2,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public int BatchNo { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 3,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        [ValidateLength(0, 9)]
        public string EmployeeNo { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 4,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public DateTime WorkDateTime { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 5,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public decimal Hours { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 6,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateLength(0, 9)]
        public string PayCode { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 7,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateLength(0, 12)]
        public string Position { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 8,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateLength(0, 9)]
        public string ShiftId { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 9,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        [ValidateLength(0, 15)]
        public string WorkTechJob { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 10,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        [ValidateLength(0, 15)]
        public string WorkTechActivity { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 11,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateLength(0, 9)]
        public string TaskId { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 12,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public int OTOption { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 13,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        [ValidateLength(0, 60)]
        public string GLAccount { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 14,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public int Status { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 15,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string ImportedBy { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 16,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public DateTime? ImportedDate { get; set; }

        protected override void ProcessRecord()
        {
            var @object = ProcessRecordAsync();

            WriteObject(@object);
        }

        private WorkTechImport ProcessRecordAsync()
        {
            var task = AddWorkTechImport();
            task.Wait();

            return task.Result;
        }

        private async Task<WorkTechImport> AddWorkTechImport()
        {
            WorkTechImport dto = new WorkTechImport()
            {
                TransactionNo = TransactionNo,
                BatchNo = BatchNo,
                EmployeeNo = EmployeeNo,
                WorkDateTime = WorkDateTime,
                Hours = Hours,
                PayCode = PayCode,
                Position = Position,
                ShiftId = ShiftId,
                WorkTechJob = WorkTechJob,
                WorkTechActivity = WorkTechActivity,
                TaskId = TaskId,
                OTOption = OTOption,
                GLAccount = GLAccount,
                Status = Status,
                ImportedBy = ImportedBy,
                ImportedDate = ImportedDate ?? DateTime.UtcNow
            };

            string json = JsonSerializer.Serialize(dto);

            HttpContent content = new StringContent(json);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json")
            {
                CharSet = "UTF-8"
            };

            HttpResponseMessage response = await AvantiApi.PostAsync("/api/worktech/import", content);

            if (!response.IsSuccessStatusCode)
            {
                ThrowTerminatingError(new ErrorRecord(
                    new HttpRequestException(response.ReasonPhrase),
                    $"{response.StatusCode}",
                    ErrorCategory.InvalidOperation,
                    response));
            }

            WorkTechImport result = JsonSerializer.Deserialize<WorkTechImport>(
                await response.Content.ReadAsStreamAsync());

            return result;
        }
    }
}