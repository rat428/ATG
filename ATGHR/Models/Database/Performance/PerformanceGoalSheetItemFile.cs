using System;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	public class PerformanceGoalSheetItemFile : CRUDObject
	{
		/// <summary>
		/// Primary Key.
		/// </summary>
		[Key]
		public Int64? PerformanceGoalSheetItemFileId { get; set; } = null;

		/// <summary>
		/// Foreign Key.
		/// </summary>
		public Int64? PerformanceGoalSheetItemId { get; set; } = null;

		/// <summary>
		/// Foreign Key.
		/// </summary>
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// File name.
		/// </summary>
		public String Filename { get; set; } = null;

		/// <summary>
		/// Attached file contents.
		/// </summary>
		public Byte[] Contents { get; set; } = null;

		/// <summary>
		/// Last user who last edited this record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Time stamp.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;
	}
}
