using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using ATG.DBNet;
using ATGHR.Code;
using ATGHR.Code.Enums;

namespace ATGHR.Models.Database
{
	public class PerformanceReviewSheetLight : CRUDObject
	{
		#region Properties
		/// <summary>
		/// Primary Key.
		/// </summary>
		[Key]
		public Int64? PerformanceReviewSheetId { get; set; } = null;

		/// <summary>
		/// Goal Sheet Related to this review
		/// </summary>
		public Int64? PerformanceGoalSheetId { get; set; } = null;

		/// <summary>
		/// Related user.
		/// </summary>
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// Year set for review sheet.
		/// </summary>
		public Int32? Year { get; set; } = null;

		/// <summary>
		/// Total score for the review (numeric).
		/// </summary>
		[ReadOnly]
		public Decimal? NumericScore { get; set; } = null;

		/// <summary>
		/// Total score for the review.
		/// </summary>
		[ReadOnly]
		public String Score { get; set; } = null;

		/// <summary>
		/// The user's Division at the time of the review.
		/// </summary>
		public Int64? DivisionId { get; set; } = null;

		/// <summary>
		/// The user's Role at the time of the review.
		/// </summary>
		public Int64? RoleId { get; set; } = null;

		/// <summary>
		/// Due date set by HR.
		/// </summary>
		public DateTime? DueDate { get; set; } = null;

		/// <summary>
		/// Flag indicating employee completion.
		/// </summary>
		public Boolean EmployeeComplete { get; set; } = false;

		/// <summary>
		/// The user who last completed the "EmployeeComplete" flag.
		/// </summary>
		public Int64? EmployeeCompleteUserId { get; set; } = null;

		/// <summary>
		/// The timestamp of the last completed "EmployeeComplete" flag.
		/// </summary>
		public DateTime? EmployeeCompleteDatetime { get; set; } = null;

		/// <summary>
		/// Flag indicating manager review.
		/// </summary>
		public Boolean ManagerComplete { get; set; } = false;

		/// <summary>
		/// The user who last completed the "ManagerComplete" flag.
		/// </summary>
		public Int64? ManagerCompleteUserId { get; set; } = null;

		/// <summary>
		/// The timestamp of the last completed "ManagerComplete" flag.
		/// </summary>
		public DateTime? ManagerCompleteDatetime { get; set; } = null;

		/// <summary>
		/// Flag indicating lead review.
		/// </summary>
		public Boolean LeadComplete { get; set; } = false;

		/// <summary>
		/// The user who last completed the "LeadComplete" flag.
		/// </summary>
		public Int64? LeadCompleteUserId { get; set; } = null;

		/// <summary>
		/// The timestamp of the last completed "LeadComplete" flag.
		/// </summary>
		public DateTime? LeadCompleteDatetime { get; set; } = null;

		/// <summary>
		/// Flag indicating HR review.
		/// </summary>
		public Boolean HRComplete { get; set; } = false;

		/// <summary>
		/// The user who last completed the "HRComplete" flag.
		/// </summary>
		public Int64? HRCompleteUserId { get; set; } = null;

		/// <summary>
		/// The timestamp of the last completed "HRComplete" flag.
		/// </summary>
		public DateTime? HRCompleteDatetime { get; set; } = null;

		/// <summary>
		/// User who last edited this record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Timestamp of most recent change.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;
		#endregion
	}

	public class PerformanceReviewSheet : PerformanceReviewSheetLight
	{
		#region Linked Objects
		/// <summary>
		/// Array of related questions.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceReviewSheetItem> PerformanceReviewSheetItems { get; set; } = null;

		/// <summary>
		/// Array of related attachments.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceReviewSheetFile> PerformanceReviewSheetFiles { get; set; } = null;

		/// <summary>
		/// Array of related text.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceReviewSheetText> PerformanceReviewSheetTexts { get; set; } = null;
		#endregion

		#region Enumerables
		/// <summary>
		/// Approval type for a review sheet.
		/// </summary>
		public enum ApprovalType
		{
			/// <summary>
			/// Employee has completed working on the sheet, so the supervisor should now review it.
			/// </summary>
			Employee = 1,
			/// <summary>
			/// Manager has completed reviewing the sheet, so the lead should now review it.
			/// </summary>
			Manager = 2,
			/// <summary>
			/// Lead (supervisor chain depth >= 2) has completed reviewing the sheet, so HR should now review it.
			/// </summary>
			Lead = 4,
			/// <summary>
			/// HR has completed reviewing the sheet, so it is fully completed.
			/// </summary>
			HR = 8
		}
		#endregion

		#region Operation Related Properties
		/// <summary>
		/// Get all items from the sheet item tree recursively; primarily for writing updates to the database.
		/// </summary>
		/// <param name="performanceReviewSheetItems">Current level list of items from which to get lists of items.</param>
		/// <returns></returns>
		private IEnumerable<PerformanceReviewSheetItem> PerformanceReviewSheetItemsRecursive(
			IEnumerable<PerformanceReviewSheetItem> performanceReviewSheetItems)
		{
			return performanceReviewSheetItems?.Concat(performanceReviewSheetItems.SelectMany(i => PerformanceReviewSheetItemsRecursive(i.PerformanceReviewSheetItems)))
				?? Enumerable.Empty<PerformanceReviewSheetItem>();
		}

		/// <summary>
		/// Get all items from the sheet item tree recursively; primarily for writing updates to the database.
		/// </summary>
		[JsonIgnore]
		[LinkType(LinkType = LinkType.Table, Writable = true, Readable = false, ParameterName = "SheetItemTable")]
		public IEnumerable<PerformanceReviewSheetItem> AllPerformanceReviewSheetItems
		{
			get
			{
				return PerformanceReviewSheetItemsRecursive(PerformanceReviewSheetItems);
			}
		}

		/// <summary>
		/// Get all texts from the recursive sheet item tree.
		/// </summary>
		[JsonIgnore]
		[LinkType(LinkType = LinkType.Table, Writable = true, Readable = false, ParameterName = "SheetItemTextTable")]
		public IEnumerable<PerformanceReviewSheetItemText> AllPerformanceReviewSheetItemTexts
		{
			get
			{
				return AllPerformanceReviewSheetItems.SelectMany(i => i.PerformanceReviewSheetItemTexts ?? Enumerable.Empty<PerformanceReviewSheetItemText>());
			}
		}
		#endregion

		#region Database Operations
		public override void Create(
			List<SqlParameter> extraParameters = null,
			DBConnector db = null)
		{
			throw new Exception("Review sheets may not be created this way.");
		}

		public override void Update(
			List<SqlParameter> extraParameters = null,
			DBConnector db = null)
		{
			extraParameters = new List<SqlParameter>()
			{
				new SqlParameter("@PerformanceReviewSheetId", PerformanceReviewSheetId),
				new SqlParameter("@SheetItemTable", DBNet.GetTable(AllPerformanceReviewSheetItems, StoredProcedureType.UpdateMulti)),
				new SqlParameter("@SheetItemTextTable", DBNet.GetTable(AllPerformanceReviewSheetItemTexts, StoredProcedureType.UpdateMulti)),
				new SqlParameter("@DataUserId", DataUserId),
				new SqlParameter("@DataDatetime", DataDatetime)
			};

			DBNet.Update(this, SQL: "SP_TotalPerformanceReviewSheet_Update", useProperties: false, extraParameters: extraParameters, db: db);

			//EMail.SendSheetNotification(db, PerformanceReviewSheetId, "Review", Year, "Update", EMail.SheetChangeType.Update, DataDatetime, UserId, DataUserId);
		}

		public ExecuteResult Approve(
			ApprovalType approvalType,
			Boolean approved,
			Int64? SheetUserId = null)
		{
			List<SqlParameter> extraParameters = new List<SqlParameter>()
			{
				new SqlParameter("@PerformanceReviewSheetId", PerformanceReviewSheetId),
				new SqlParameter("@Complete", approved),
				new SqlParameter("@DataUserId", DataUserId),
				new SqlParameter("@DataDatetime", DataDatetime)
			};

			String SQL = "";

			switch (approvalType)
			{
				case ApprovalType.Employee:
					SQL = "SP_PerformanceReviewSheetEmployeeApproved_Update";
					break;
				case ApprovalType.Manager:
					SQL = "SP_PerformanceReviewSheetManagerApproved_Update";
					break;
				case ApprovalType.Lead:
					SQL = "SP_PerformanceReviewSheetLeadApproved_Update";
					break;
				case ApprovalType.HR:
					SQL = "SP_PerformanceReviewSheetHRApproved_Update";
					break;
			}

			ExecuteResult result = DBNet.ExecuteToResult(SQL, parameters: extraParameters, timeout: 180);

			if (result.Success)
			{
				EMail.SendSheetNotification(PerformanceReviewSheetId, "Review", Year, approvalType.ToString(), EMail.SheetChangeType.Approve, approved, DataDatetime, UserId ?? SheetUserId, DataUserId);
			}

			return result;
		}
		#endregion
	}

	public class PerformanceReviewSheetSupervisorCard : PerformanceReviewSheetLight
	{
		#region Properties
		/// <summary>
		/// Whether the viewer is in the sheet owner.
		/// </summary>
		public Boolean Self { get; set; } = false;

		/// <summary>
		/// Whether the viewer is in the sheet owner's normal supervisor chain.
		/// </summary>
		public Boolean Supervisor { get; set; } = false;
		#endregion
	}
}
