using System;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	public class PerformanceReviewSheetText : CRUDObject
	{
		/// <summary>
		/// Primary Key.
		/// </summary>
		[Key]
		public Int64? PerformanceReviewSheetId { get; set; } = null;

		/// <summary>
		/// Primary Key.
		/// </summary>
		[Key]
		public Int64? PerformanceTextTypeId { get; set; } = null;

		/// <summary>
		/// Related user, Foreign Key.
		/// </summary>
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// Related text or questions.
		/// </summary>
		public String Contents { get; set; } = null;

		/// <summary>
		/// User who last edited this record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Timestamp of most recent change.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;
	}
}
