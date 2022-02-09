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

        public int TransactionNo { get; set; }

        public int BatchNo { get; set; }

        [ValidateLength(0, 9)]
        public string EmployeeNo { get; set; }

        public DateTime WorkDateTime { get; set; }

        public decimal Hours { get; set; }

        public string PayCode { get; set; }

        public string Position { get; set; }

        public string ShiftId { get; set; }

        public string WorkTechJob { get; set; }

        public string WorkTechActivity { get; set; }

        public string TaskId { get; set; }

        public int OTOption { get; set; }

        public string GLAccount { get; set; }

        public int Status { get; set; }

        public string ImportedBy { get; set; }

        public DateTime ImportedDate { get; set; }

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
                ImportedDate = ImportedDate
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
