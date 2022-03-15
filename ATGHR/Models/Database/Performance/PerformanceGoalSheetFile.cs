using System;
using ATG.DBNet;
using System.ComponentModel.DataAnnotations;

namespace ATGHR.Models.Database
{
	public class PerformanceGoalSheetFile : CRUDObject
	{
		/// <summary>
		/// Primary Key.
		/// </summary>
		[Key]
		public Int64? PerformanceGoalSheetField { get; set; } = null;

		/// <summary>
		/// Related Goal Sheet, Foreign Key.
		/// </summary>
		public Int64? PerformanceGoalSheetId { get; set; } = null;

		/// <summary>
		/// Related user, Foreign Key.
		/// </summary>
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// Attachment filename.
		/// </summary>
		public String Filename { get; set; } = null;

		/// <summary>
		/// Contents of the attachment.
		/// </summary>
		public Byte[] Contents { get; set; } = null;

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
