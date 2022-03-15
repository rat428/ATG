using System;
using System.Linq;
using System.Reflection;
using ATGHR.Code.Enums;

namespace ATGHR.Code
{
	/// <summary>
	/// Specifies permissions required for an action.
	/// All permission types in a single attribute must be concatenated with | operator.
	/// ALL permission types in a single attribute are required.
	/// Optional permission types must be separated in another attribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public class PermissionsRequiredAttribute : Attribute
	{
		/// <summary>
		/// Permission type required for this section.
		/// </summary>
		public PermissionType PermissionType { get; set; } = PermissionType.Authorized;

		/// <summary>
		/// Determine whether the given controller and method permission requirements
		/// </summary>
		/// <param name="controller">Controller for the request.</param>
		/// <param name="method">Method for the request.</param>
		/// <param name="user">The viewing user.</param>
		/// <param name="employeeUserId">The target user, if any.</param>
		/// <param name="performanceGoalSheetId">The target goal sheet, if any.</param>
		/// <param name="performanceReviewSheetId">The target review sheet, if any.</param>
		/// <param name="divisionId">The target division, if any.</param>
		/// <returns>True/false.</returns>
		public static Boolean PermissionMatch(
			Type controller,
			MethodInfo method,
			Models.Database.User user,
			Int64? employeeUserId = null,
			Int64? performanceGoalSheetId = null,
			Int64? performanceReviewSheetId = null,
			Int64? divisionId = null)
		{
			PermissionType userPermissions = user.GetPermissionTypes(employeeUserId, performanceGoalSheetId, performanceReviewSheetId, divisionId);

			// Method required permissions take priority
			if (method.CustomAttributes.OfType<PermissionsRequiredAttribute>().Any())
			{
				return method.CustomAttributes.OfType<PermissionsRequiredAttribute>()
					.Any(p => (userPermissions & p.PermissionType) == p.PermissionType);
			}
			else if (controller.CustomAttributes.OfType<PermissionsRequiredAttribute>().Any())
			{
				return controller.CustomAttributes.OfType<PermissionsRequiredAttribute>()
					.Any(p => (userPermissions & p.PermissionType) == p.PermissionType);
			}

			return true;
		}
	}
}
