using System;
using System.ComponentModel.DataAnnotations;

namespace Avanti.SDK.Models.WorkTech
{
    public class WorkTechExport
	{
		public int? ExportNo { get; set; }

		[Required]
		public int TransactionNo { get; set; }

		[Required]
		[StringLength(9)]
		public string EmployeeNo { get; set; }

		[Required]
		public int BatchNo { get; set; }

		[Required]
		public DateTime WorkDateTime { get; set; }

		[Required]
		public decimal Hours { get; set; }

		[StringLength(9)]
		public string PayCode { get; set; }

		[StringLength(12)]
		public string Position { get; set; }

		[StringLength(9)]
		public string ShiftId { get; set; }

		[Required]
		[StringLength(15)]
		public string WorkTechJob { get; set; }

		[Required]
		[StringLength(15)]
		public string WorkTechActivity { get; set; }

		[StringLength(9)]
		public string TaskId { get; set; }

		[Required]
		public decimal PayRate { get; set; }

		[Required]
		public int Status { get; set; }

		[Required]
		[StringLength(32)]
		public string ExportedBy { get; set; }

		public DateTime ExportedDate { get; set; }
	}
}
