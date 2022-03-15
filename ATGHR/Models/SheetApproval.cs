using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ATGHR.Models
{
	/// <summary>
	/// An approval request for a review sheet.
	/// </summary>
	public class PerformanceReviewSheetApproval
	{
		/// <summary>
		/// Sheet primary key.
		/// </summary>
		public Int64? PerformanceReviewSheetId { get; set; } = null;

		/// <summary>
		/// The user to which the sheet belongs.
		/// </summary>
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// Type of approval.
		/// </summary>
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Database.PerformanceReviewSheet.ApprovalType ApprovalType { get; set; } = Database.PerformanceReviewSheet.ApprovalType.Employee;

		/// <summary>
		/// Whether the approval is being set on or off.
		/// </summary>
		public Boolean Approved { get; set; } = true;
	}

	/// <summary>
	/// an approval of a spot bonus
	/// </summary>
	public class SpotBonusApproval
	{
		/// <summary>
		/// bonus primary key.
		/// </summary>
		public Int64? SpotBonusId { get; set; } = null;

		/// <summary>
		/// Type of approval.
		/// </summary>
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Database.SpotBonus.ApprovalType ApprovalType { get; set; } = Database.SpotBonus.ApprovalType.submitted;

		/// <summary>
		/// Whether the approval is being set on or off.
		/// </summary>
		public Boolean Approved { get; set; } = true;
	}

	/// <summary>
	/// a bulk approval of spot bonuses
	/// </summary>
	public class SpotBonusBulkApproval
	{
		/// <summary>
		/// bonus primary key.
		/// </summary>
		public IEnumerable<long?> SpotBonusIds { get; set; } = null;

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Database.SpotBonus.ApprovalType ApprovalType { get; set; } = Database.SpotBonus.ApprovalType.submitted;

		/// <summary>
		/// Whether the approval is being set on or off.
		/// </summary>
		public Boolean Approved { get; set; } = true;
	}

	/// <summary>
	/// An approval request for a goal sheet.
	/// </summary>
	public class PerformanceGoalSheetApproval
	{
		/// <summary>
		/// Sheet primary key.
		/// </summary>
		public Int64? PerformanceGoalSheetId { get; set; } = null;

		/// <summary>
		/// The user to which the sheet belongs.
		/// </summary>
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// Type of approval.
		/// </summary>
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Database.PerformanceGoalSheet.ApprovalType ApprovalType { get; set; } = Database.PerformanceGoalSheet.ApprovalType.Employee;

		/// <summary>
		/// Whether the approval is being set on or off.
		/// </summary>
		public Boolean Approved { get; set; } = true;
	}
}

