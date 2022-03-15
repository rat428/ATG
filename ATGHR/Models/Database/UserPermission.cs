using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ATG.DBNet;
using System.Text.Json.Serialization;

namespace ATGHR.Models.Database
{
	public class UserPermissionLight : CRUDObject
	{
		#region Properties
		/// <summary>
		/// The user whose permission this is.
		/// </summary>
		[Key]
		public virtual Int64? UserId { get; set; } = null;

		/// <summary>
		/// The user, if any, to whom the permission applies.
		/// </summary>
		[Key]
		public virtual Int64? EmployeeUserId { get; set; } = null;

		/// <summary>
		/// The goal sheet, if any, to whom the permission applies.
		/// </summary>
		[Key]
		public Int64? PerformanceGoalSheetId { get; set; } = null;

		/// <summary>
		/// The review sheet, if any, to whom the permission applies.
		/// </summary>
		[Key]
		public Int64? PerformanceReviewSheetId { get; set; } = null;

		/// <summary>
		/// The division, if any, to whom the permission applies.
		/// </summary>
		[Key]
		public Int64? DivisionId { get; set; } = null;

		/// <summary>
		/// Permission type.
		/// </summary>
		[Key]
		public Int64? PermissionId { get; set; } = null;

		/// <summary>
		/// The user who last edited the record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Timestamp of the last record edit.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;
		#endregion
	}

	/// <summary>
	/// User:user relationship. Link table between users.
	/// </summary>
	public class UserPermission : UserPermissionLight
	{
		#region Properties
		/// <summary>
		/// Relationship type.
		/// </summary>
		[DBIgnore]
		[JsonIgnore]
		public Permission Permission
		{
			get
			{
				return ATGHR.LookupTables?.Permissions?.FirstOrDefault(up => up.PermissionId == PermissionId);
			}
		}

		/// <summary>
		/// Whether this permission applies globally.
		/// </summary>
		[DBIgnore]
		public Boolean IsGlobal
		{
			get
			{
				return EmployeeUserId == null && PerformanceGoalSheetId == null && PerformanceReviewSheetId == null && DivisionId == null;
			}
		}

		/// <summary>
		/// Whether this permission applies to a requested user.
		/// </summary>
		/// <param name="employeeUserId">The employee's user ID.</param>
		/// <returns>True/false.</returns>
		public Boolean MatchesUser(
			Int64? employeeUserId)
		{
			return EmployeeUserId == (employeeUserId ?? -1L) || IsGlobal;
		}

		/// <summary>
		/// Whether this permission applies to a requested goal sheet.
		/// </summary>
		/// <param name="performanceGoalSheetId">The goal sheet ID.</param>
		/// <returns>True/false.</returns>
		public Boolean MatchesGoalSheet(
			Int64? performanceGoalSheetId)
		{
			return PerformanceGoalSheetId == (performanceGoalSheetId ?? -1L) || IsGlobal;
		}

		/// <summary>
		/// Whether this permission applies to a requested review sheet.
		/// </summary>
		/// <param name="performanceReviewSheetId">The review sheet ID.</param>
		/// <returns>True/false.</returns>
		public Boolean MatchesReviewSheet(
			Int64? performanceReviewSheetId)
		{
			return PerformanceReviewSheetId == (performanceReviewSheetId ?? -1L) || IsGlobal;
		}

		/// <summary>
		/// Whether this permission applies to a requested division.
		/// </summary>
		/// <param name="divisionId">The division ID.</param>
		/// <returns>True/false.</returns>
		public Boolean MatchesDivision(
			Int64? divisionId)
		{
			return DivisionId == (divisionId ?? -1L) || IsGlobal;
		}

		/// <summary>
		/// Whether this permission applies to a requested user, review sheet, goal sheet, or division.
		/// </summary>
		/// <param name="employeeUserId">The employee's user ID.</param>
		/// <param name="performanceGoalSheetId">The review sheet ID.</param>
		/// <param name="performanceReviewSheetId">The goal sheet ID.</param>
		/// <param name="divisionId">The division ID.</param>
		/// <returns>True/false.</returns>
		public Boolean Match(
			Int64? employeeUserId = null,
			Int64? performanceGoalSheetId = null,
			Int64? performanceReviewSheetId = null,
			Int64? divisionId = null)
		{
			return MatchesUser(employeeUserId) || MatchesGoalSheet(performanceGoalSheetId) || MatchesReviewSheet(performanceReviewSheetId) || MatchesDivision(divisionId) || IsGlobal;
		}
		#endregion

		public UserPermission() : base() { }
	}

	/// <summary>
	/// UserPermission wherein the permission holder is the parent object.
	/// </summary>
	public class IncomingUserPermission : UserPermission
	{
		#region Properties
		[Key]
		public override Int64? UserId { get; set; } = null;

		[Key]
		[ForeignKey(Name = "UserId")]
		[ForeignKeyExclusivity(Type = ForeignKeyExclusivity.Only)]
		public override Int64? EmployeeUserId { get; set; } = null;
		#endregion
	}

	/// <summary>
	/// UserPermission wherein the permission pertains to the parent object.
	/// </summary>
	public class OutgoingUserPermission : UserPermission
	{
		#region Properties
		[Key]
		[ForeignKey(Name = "UserId")]
		[ForeignKeyExclusivity(Type = ForeignKeyExclusivity.Only)]
		public override Int64? UserId { get; set; } = null;

		[Key]
		public override Int64? EmployeeUserId { get; set; } = null;
		#endregion
	}
}
