using System;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	public class PerformanceReviewSheetFile : CRUDObject
	{
		/// <summary>
		/// Primary Key.
		/// </summary>
		[Key]
		public Int64? PerformanceReviewSheetFileId { get; set; } = null;

		/// <summary>
		/// Related review sheet, Foreign Key.
		/// </summary>
		public Int64? PerformanceReviewSheetId { get; set; } = null;

		/// <summary>
		/// Related user, Foreign Key.
		/// </summary>
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// Name of the file
		/// </summary>
		public String Filename { get; set; } = null;

		/// <summary>
		/// Attachment contents.
		/// </summary>
		public Byte[] Contents { get; set; } = null;

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
