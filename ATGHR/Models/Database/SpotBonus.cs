using System;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using ATGHR.Code;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace ATGHR.Models.Database
{
	/// <summary>
	/// User ATG division. One:many to User table.
	/// </summary>
	public class SpotBonusLight : CRUDObject
	{
		/// <summary>
		/// Primary key.
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Int64? SpotBonusId { get; set; } = null;

		/// <summary>
		/// Amount of the bonus.
		/// </summary>
		public Int32? Amount { get; set; } = null;

		/// <summary>
		/// Reason the Spot Bonus is given
		/// </summary>
		public String Reason { get; set; } = null;

		/// <summary>
		/// Reason the Spot Bonus was rejected
		/// </summary>
		public String RejectReason { get; set; } = null;

		/// <summary>
		/// creation date of the bonus
		/// </summary>
		public DateTime? CreatedDatetime { get; set; } = null;

		/// <summary>
		/// the person the Spot Bonus is for
		/// </summary>
		public Int64? SubmitteeUserId { get; set; } = null;

		/// <summary>
		/// the person the spot bonus is created by 
		/// </summary>
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// The person submitting the spot bonus for approval
		/// </summary>
		public Int64? SubmitterUserId { get; set; } = null;

		/// <summary>
		/// The time the spot bonus was submitted
		/// </summary>
		public DateTime? SubmittedDateTime { get; set; } = null;

		///	<summary>
		///	Submitted bit 
		///	</summary>
		public Boolean Submitted { get; set; } = false;

		/// <summary>
		/// The user approving the spot bonus request
		/// </summary>
		public Int64? ApprovalUserId { get; set; } = null;

		/// <summary>
		/// the time the spot bonus was approved
		/// </summary>
		public DateTime? ApprovalDateTime { get; set; } = null;

		/// <summary>
		/// Approval bit
		/// </summary>
		public Boolean Approved { get; set; } = false;


		/// <summary>
		/// The user completing the spot bonus request
		/// </summary>
		public Int64? CompletedUserId { get; set; } = null;

		/// <summary>
		/// the time the spot bonus was completed
		/// </summary>
		public DateTime? CompleteDateTime { get; set; } = null;

		/// <summary>
		/// Complete bit
		/// </summary>
		public Boolean Complete { get; set; } = false;

		/// <summary>
		/// active spot bonus
		/// </summary>
		public Boolean ActiveFlag { get; set; } = true;

		/// <summary>
		/// The user who last edited the record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Timestamp of the last record edit.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;
	}

	public class SpotBonus : SpotBonusLight
	{
		#region Enumerables
		/// <summary>
		/// Approval type for a review sheet.
		/// </summary>
		public enum ApprovalType
		{
			/// <summary>
			/// submission of the award by the employee
			/// </summary>
			submitted = 1,
			/// <summary>
			/// approval of award by the manager
			/// </summary>
			approve = 2,
			/// <summary>
			/// final completion of the award by the accounting department
			/// </summary>
			complete = 4,
		}
		#endregion

		#region Database Operations
		public ExecuteResult Approve(
		ApprovalType approvalType,
		Boolean approved
		)
			{
				List<SqlParameter> extraParameters = new List<SqlParameter>()
				{
					new SqlParameter("@SpotBonusId", SpotBonusId),
					new SqlParameter("@Complete", approved),
					new SqlParameter("@DataUserId", DataUserId),
					new SqlParameter("@DataDatetime", DataDatetime)
				};

				String SQL = "";

				switch (approvalType)
				{
					case ApprovalType.submitted:
						SQL = "SP_SpotBonusSubmission_Update";
						break;
					case ApprovalType.approve:
						SQL = "SP_SpotBonusApproved_Update";
						break;
					case ApprovalType.complete:
						SQL = "SP_SpotBonusComplete_Update";
						break;
				}

				ExecuteResult result = DBNet.ExecuteToResult(SQL, parameters: extraParameters);

				if (result.Success && approvalType.ToString() != "submitted")
				{
					EMail.SendBonusNotification(SpotBonusId, approvalType.ToString(), approved, DataDatetime, UserId, DataUserId);
				}

				return result;
			}
		#endregion
	}
	public class SpotBonusBulk : SpotBonus
	{
		[ReadOnly]
		public IEnumerable<long?> SpotBonusIds { get; set; } = null;

		#region Database Operations
		public ExecuteResult ApproveBulk(
		ApprovalType approvalType,
		Boolean approved
		)
		{
			DataTable table = new DataTable();
			table.Columns.Add("id", typeof(Int64));

			foreach (int i in SpotBonusIds)
			{
				table.Rows.Add(i);
			}

			List<SqlParameter> extraParameters = new List<SqlParameter>()
				{
					new SqlParameter("@SpotBonusIds", table),
					new SqlParameter("@Complete", approved),
					new SqlParameter("@DataUserId", DataUserId),
					new SqlParameter("@DataDatetime", DataDatetime)
				};

			String SQL = "SP_SpotBonusCompleteBulk_Update";

			ExecuteResult result = DBNet.ExecuteToResult(SQL, parameters: extraParameters);

			//if (result.Success && approvalType.ToString() != "submitted")
			//{
			//	EMail.SendBonusNotification(db, SpotBonusId, approvalType.ToString(), approved, DataDatetime, UserId, DataUserId);
			//}

			return result;
		}
		#endregion
	}
}


