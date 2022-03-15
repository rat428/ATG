using System;

namespace ATGHR.Code.Enums
{
	/// <summary>
	/// Permission type requirements for use in authentication checks.
	/// </summary>
	public enum PermissionType : UInt32
	{
		/// <summary>
		/// The user may load this resource in any situation.
		/// </summary>
		Any = 1 << 0,
		/// <summary>
		/// The user may load this resource if they are logged in.
		/// </summary>
		Authorized = 1 << 1,
		/// <summary>
		/// The user may load this resource pertaining to their own account.
		/// This type will only be effective when applied to resources for which there is a UserId specified.
		/// </summary>
		Self = 1 << 2,
		/// <summary>
		/// The user may load this resource if they are in the supervisor chain for the account in question.
		/// This type will only be effective when applied to resources for which there is a UserId specified.
		/// </summary>
		Supervisor = 1 << 3,
		/// <summary>
		/// The user may load this resource if they are 2 or more steps up the supervisor chain for the account in question.
		/// This type will only be effective when applied to resources for which there is a UserId specified.
		/// </summary>
		Lead = 1 << 4,
		/// <summary>
		/// The user may load this resource if they are a system administrator.
		/// </summary>
		Administrator = 1 << 5,
		/// <summary>
		/// The user may load this resource if they have "Viewer" or "Commenter" type permissions.
		/// </summary>
		Viewer = 1 << 6,
		/// <summary>
		/// The user may load this resource if they have "Commenter" type permissions.
		/// </summary>
		Commenter = 1 << 7,
		/// <summary>
		/// The user may request spot bonuses.
		/// </summary>
		SpotBonusRequestor = 1 << 8,
		/// <summary>
		/// The user may approve spot bonuses, usually for specific other users.
		/// </summary>
		SpotBonusApprover = 1 << 9,
		/// <summary>
		/// The user may complete spot bonuses after they have been completed in the payroll system.
		/// </summary>
		SpotBonusCompleter = 1 << 10
	}

	static class PermissionTypeExtensions
	{
		/// <summary>
		/// Bitwise comparison.
		/// To check for multiple required permissions, use BEquals(permissiontype1 | permissiontype2).
		/// </summary>
		/// <param name="left">This instance.</param>
		/// <param name="right">A permission type to compare with this instance.</param>
		/// <returns>True/false.</returns>
		public static Boolean BEquals(
			this PermissionType left,
			PermissionType right)
		{
			return (right & left) == right;
		}
	}
}
