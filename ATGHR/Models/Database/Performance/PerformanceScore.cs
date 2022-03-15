using System;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	/// <summary>
	/// Possible scores for scored items on performance reviews.
	/// </summary>
	public class PerformanceScore : CRUDObject
	{
		/// <summary>
		/// Primary key.
		/// </summary>
		[Key]
		public Int64? PerformanceScoreId { get; set; } = null;

		/// <summary>
		/// The score.
		/// </summary>
		public Decimal Score { get; set; } = 0m;

		/// <summary>
		/// Score name.
		/// </summary>
		[StringLength(150)]
		public String Name { get; set; } = null;

		/// <summary>
		/// Score description.
		/// </summary>
		public String Description { get; set; } = null;

		/// <summary>
		/// Score "supervision": a description of the kind of supervision the employee requires in the related metric on the performance review.
		/// </summary>
		public String Supervision { get; set; } = null;

		/// <summary>
		/// Choosing this score requires a comment.
		/// </summary>
		public Boolean NeedsComment { get; set; } = false;

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
