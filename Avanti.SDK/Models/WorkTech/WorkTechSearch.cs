using System;

using Avanti.SDK.Models.Common;

namespace Avanti.SDK.Models.WorkTech
{
    public class WorkTechSearch : BaseSearch
	{
		public int? TransactionNo { get; set; }

		public string EmpNo { get; set; }

		public int? BatchNo { get; set; }

		public string PayCode { get; set; }

		public int? Status { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }
	}
}
