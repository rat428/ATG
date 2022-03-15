using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	public class PerformanceReviewSheetItemUserSupplement : CRUDObject
	{

		/// <summary>
		/// User primary key.
		/// </summary>
		[Key]
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// Performance Review Sheet Item primary key.
		/// </summary>
		[Key]
		public Int64? PerformanceReviewSheetItemId { get; set; } = null;

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
