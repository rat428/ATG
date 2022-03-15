using System;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;
namespace ATGHR.Models.Database
{
	public class PerformanceGoalSheetText : CRUDObject
	{
		/// <summary>
		/// Primary Key
		/// </summary>
		[Key]
		public Int64? PerformanceGoalSheetId { get; set; } = null;

		/// <summary>
		/// Foreign Key.
		/// </summary>
		[Key]
		public Int64? PerformanceTextTypeId { get; set; } = null;

		/// <summary>
		/// Related user, Foreign Key.
		/// </summary>
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// Goal text.
		/// </summary>
		public String Contents { get; set; } = null;

		/// <summary>
		/// The user who last edited the record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Timestamp of the last record edit.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;
	}
}
