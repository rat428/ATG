using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using ATGHR.Code;
using ATGHR.Code.Enums;
using ATG.DBNet;
using System.Text.Json;
using System.Data;

namespace ATGHR.Models.Database
{
	public class UserLookup : CRUDObject
	{
		#region Properties
		/// <summary>
		/// Primary key.
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// The user is "supervisor" for this quantity of people.
		/// </summary>
		[ReadOnly]
		public Int32? Manages { get; set; } = null;

		/// <summary>
		/// The user is "lead" for this quantity of people.
		/// </summary>
		[ReadOnly]
		public Int32? Leads { get; set; } = null;

		/// <summary>
		/// The user is "Spot Bonus Approver" for this quantity of people.
		/// </summary>
		[ReadOnly]
		public Int32? SpotBonusApprovers { get; set; } = null;

		/// <summary>
		/// Total number of people who are "supervisor" for this user.
		/// </summary>
		[ReadOnly]
		public Int32? HasManagers { get; set; } = null;

		/// <summary>
		/// Total number of people who are "lead" for this user.
		/// This determines whether some leads are required to complete lead approval of a sheet or if others can be relied upon to do so.
		/// </summary>
		[ReadOnly]
		public Int32? HasLeads { get; set; } = null;

		/// <summary>
		/// Total number of people who are "Spot Bonus Approver" for this user.
		/// This determines whether some Spot Bonus Approver are required to complete Spot Bonus Approver approval of a Bonus or if others can be relied upon to do so.
		/// </summary>
		[ReadOnly]
		public Int32? HasSpotBonusApprovers { get; set; } = null;

		/// <summary>
		/// E-mail address on the user's Microsoft account.
		/// </summary>
		[StringLength(150)]
		public String Username { get; set; } = null;

		/// <summary>
		/// First name.
		/// </summary>
		[StringLength(150)]
		public String NameFirst { get; set; } = null;

		/// <summary>
		/// Last name.
		/// </summary>
		[StringLength(150)]
		public String NameLast { get; set; } = null;

		/// <summary>
		/// "Goes by" name (nickname).
		/// </summary>
		[StringLength(150)]
		public String NameGoesBy { get; set; } = null;

		/// <summary>
		/// The user's name, informed by other name fields.
		/// </summary>
		[DBIgnore]
		public String Name
		{
			get
			{
				return String.Format(
					"{0} {1}",
					String.IsNullOrWhiteSpace(NameGoesBy) ? NameFirst : NameGoesBy,
					NameLast);
			}
		}

		/// <summary>
		/// Job title.
		/// </summary>
		[StringLength(100)]
		public String JobTitle { get; set; } = null;

		/// <summary>
		/// Career ladder classification
		/// </summary>
		[StringLength(100)]
		public String CareerLadder { get; set; } = null;

		/// <summary>
		/// Current training level.
		/// </summary>
		[StringLength(100)]
		public String TrainingLevel { get; set; } = null;

		/// <summary>
		/// The user's company division.
		/// </summary>
		public Int64? DivisionId { get; set; } = null;

		/// <summary>
		/// Whether the user's account is available to be used in the application.
		/// </summary>
		public Boolean ActiveFlag { get; set; } = false;

		/// <summary>
		/// Timestamp of the last record edit.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;
		#endregion
	}

	/// <summary>
	/// User object. Central to permissions and usage settings.
	/// </summary>
	[Table("User")]
	public class UserLight : UserLookup
	{
		#region Properties
		/// <summary>
		/// Full name as stored on Microsoft account.
		/// </summary>
		[StringLength(150)]
		public String MSName { get; set; } = null;

		/// <summary>
		/// Middle name.
		/// </summary>
		[StringLength(150)]
		public String NameMiddle { get; set; } = null;

		/// <summary>
		/// Name suffix.
		/// </summary>
		[StringLength(150)]
		public String NameSuffix { get; set; } = null;

		/// <summary>
		/// Honorific(s).
		/// </summary>
		[StringLength(150)]
		public String NameTitle { get; set; } = null;

		/// <summary>
		/// The last time the user logged in.
		/// </summary>
		public DateTime? LastLogin { get; set; } = null;

		/// <summary>
		/// Office phone number.
		/// </summary>
		[StringLength(15)]
		public String OfficePhone { get; set; } = null;

		/// <summary>
		/// Mobile phone number.
		/// </summary>
		[StringLength(15)]
		public String MobilePhone { get; set; } = null;

		/// <summary>
		/// Office phone internal extension number.
		/// </summary>
		[StringLength(5)]
		public String OfficeExtension { get; set; } = null;

		/// <summary>
		/// The date on which the user was hired.
		/// </summary>
		public DateTime? HireDate { get; set; } = null;

		/// <summary>
		/// The date on which the user started working.
		/// </summary>
		public DateTime? StartDate { get; set; } = null;

		/// <summary>
		/// The date on which the user's employment was terminated.
		/// </summary>
		public DateTime? TerminationDate { get; set; } = null;

		/// <summary>
		/// The company office in which the user works.
		/// </summary>
		public Int64? ATGOfficeId { get; set; } = null;

		/// <summary>
		/// Whether the user works remotely as a rule.
		/// </summary>
		public Boolean RemoteWorker { get; set; } = false;

		/// <summary>
		/// The user's birth date
		/// </summary>
		public DateTime? BirthDate
		{
			get; set;
		} = null;

		/// <summary>
		/// A description of the room in which the user works.
		/// </summary>
		[StringLength(100)]
		public String OfficeLocation { get; set; } = null;

		/// <summary>
		/// The user's home street address.
		/// </summary>
		public String StreetAddress { get; set; } = null;

		/// <summary>
		/// The user's home unit number for their street address.
		/// </summary>
		public String Unit { get; set; } = null;

		/// <summary>
		/// The user's home address city.
		/// </summary>
		public String City { get; set; } = null;

		/// <summary>
		/// The user's home address state.
		/// </summary>
		public String State { get; set; } = null;

		/// <summary>
		/// The user's home address zip code.
		/// </summary>
		public String Zip { get; set; } = null;

		/// <summary>
		/// The User's utilization (maybe be update yearly)
		/// </summary>
		public String Utilization { get; set; } = null;

		[ReadOnly]
		public Int32? TotalSpotBonus { get; set; } = null;

		[ReadOnly]
		public Int32? SpotBonusUsed { get; set; } = null;

		/// <summary>
		/// The user who last edited the record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Array of roles.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<UserRole> UserRoles { get; set; } = null;

		/// <summary>
		/// Array of relationships with other users.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<UserRelationship> UserRelationships { get; set; } = null;

		/// <summary>
		/// Array of permissions (global, user, sheet, division, etc).
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<OutgoingUserPermission> OutgoingUserPermissions { get; set; } = null;

		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<IncomingUserPermission> IncomingUserPermissions { get; set; } = null;
		#endregion

		#region Database Operations
		public override void Create(
			List<SqlParameter> extraParameters = null,
			DBConnector db = null)
		{
			if (UserId != null)
			{
				throw new Exception("Cannot create users who already exist.");
			}
			else
			{
				base.Create(extraParameters, db);
			}
		}

		public override void Read(
			List<SqlParameter> extraParameters = null,
			DBConnector db = null)
		{
			throw new Exception("Users may not be read without a requestor UserId (DataUserId).");
		}

		public void Read(
			Int64? DataUserId,
			DBConnector db = null)
		{
			DBNet.Read(this, extraParameters: new List<SqlParameter> { new SqlParameter("@DataUserId", DataUserId) }, db: db);
		}

		public void ReadFull(
			Int64? DataUserId,
			DBConnector db = null)
		{
			DBNet.Read(this, SQL: "SP_User_ReadFull", extraParameters: new List<SqlParameter> { new SqlParameter("@DataUserId", DataUserId) }, db: db);
		}

		public void ReadByUsername(
			Int64? DataUserId = null,
			DBConnector db = null)
		{
			DBNet.Read(
				this,
				SQL: "SP_User_Read_By_Username",
				useProperties: false,
				extraParameters: new List<SqlParameter>()
				{
					new SqlParameter("@Username", Username),
					new SqlParameter("@DataUserId", DataUserId)
				},
				db: db);
		}

		public override void Update(
			List<SqlParameter> extraParameters = null,
			DBConnector db = null)
		{
			if (UserId != null)
			{
				base.Update(extraParameters, db: db);
			}
			else
			{
				base.Create(extraParameters, db: db);
			}
		}
		#endregion
	}

	[StoredProcedure(StoredProcedureType = StoredProcedureType.Update, Name = "SP_TotalUser_Update")]
	[StoredProcedure(StoredProcedureType = StoredProcedureType.Create, Name = "SP_TotalUser_Create")]
	public class User : UserLight
	{
		#region Properties
		/// <summary>
		/// The user's full name, informed by all other name fields.
		/// </summary>
		[DBIgnore]
		public String NameFull
		{
			get
			{
				return String.Format(
					"{0}{1}{4} {2}{3}{4}",
					NameFirst,
					String.IsNullOrWhiteSpace(NameMiddle) ? "" : String.Format(" {0}", NameMiddle),
					NameLast,
					String.IsNullOrWhiteSpace(NameSuffix) ? "" : String.Format("({0})", NameSuffix),
					NameGoesBy,
					String.IsNullOrWhiteSpace(NameTitle) ? "" : String.Format(", {0}", NameTitle));
			}
		}

		/// <summary>
		/// The user's global system permission type.
		/// </summary>
		[DBIgnore]
		public IEnumerable<Permission> GlobalPermissions
		{
			get
			{
				return (OutgoingUserPermissions?.Where(up => up.EmployeeUserId == null && up.PerformanceGoalSheetId == null && up.PerformanceReviewSheetId == null && up.DivisionId == null) ?? Enumerable.Empty<UserPermissionLight>())
					.Join(ATGHR.LookupTables.Permissions, up => up.PermissionId, p => p.PermissionId, (up, p) => p);
			}
		}

		[DBIgnore]
		public Boolean IsAdmin
		{
			get
			{
				return GlobalPermissions.FirstOrDefault(p => p.Name == "Administrator") != null;
			}
		}

		[DBIgnore]
		public Boolean IsSpotBonusRequestor
		{
			get
			{
				return GlobalPermissions.FirstOrDefault(p => p.Name == "Spot Bonus Requestor") != null;
			}
		}

		[DBIgnore]
		public Boolean IsSpotBonusCompleter
		{
			get
			{
				return GlobalPermissions.FirstOrDefault(p => p.Name == "Spot Bonus Completer") != null;
			}
		}

		[DBIgnore]
		public Boolean IsViewer
		{
			get
			{
				return GlobalPermissions.FirstOrDefault(p => p.Name == "Viewer") != null;
			}
		}


		[DBIgnore]
		public Boolean IsSpotBonusApprover
		{
			get
			{
				return SpotBonusApprovers > 0;
			}
		}

		[DBIgnore]
		public Boolean IsManager
		{
			get
			{
				return Manages > 0;
			}
		}

		/// <summary>
		/// The user's company division.
		/// </summary>
		[DBIgnore]
		public DivisionLight Division
		{
			get
			{
				return ATGHR.LookupTables?.Divisions?.FirstOrDefault(d => d.DivisionId == DivisionId);
			}
		}

		/// <summary>
		/// The company office in which the user works.
		/// </summary>
		[DBIgnore]
		public ATGOffice ATGOffice
		{
			get
			{
				return ATGHR.LookupTables?.ATGOffices?.FirstOrDefault(o => o.ATGOfficeId == ATGOfficeId);
			}
		}

		/// <summary>
		/// Array of roles.
		/// </summary>
		[DBIgnore]
		public IEnumerable<Role> Roles
		{
			get
			{
				return ATGHR.LookupTables?.Roles?.Where(r => UserRoles?.Any(ur => ur.RoleId == r.RoleId) ?? false);
			}
		}
		#endregion

		#region Local Functionality
		/// <summary>
		/// Get all matching permission types for the user, with an optional viewed user.
		/// </summary>
		/// <param name="userId">The viewed user.</param>
		/// <returns>Applicable permissions.</returns>
		public PermissionType GetPermissionTypes(
			Int64? employeeUserId = null,
			Int64? performanceGoalSheetId = null,
			Int64? performanceReviewSheetId = null,
			Int64? divisionId = null)
		{
			return (UserId != null ? PermissionType.Authorized : PermissionType.Any) |
				((UserId ?? -1L) == employeeUserId ? PermissionType.Self : PermissionType.Any) |
				(OutgoingUserPermissions?
					.Where(up => up.Match(employeeUserId, performanceGoalSheetId, performanceReviewSheetId, divisionId))
					.Select(up => up.Permission?.PermissionType ?? PermissionType.Any)
					.Concat(Enumerable.Range(0, 1).Select(p => PermissionType.Any))
					.Aggregate((l, p) => l | p) ?? PermissionType.Any);
		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Gets the user object out of context session data.
		/// </summary>
		/// <param name="context">Current HTTP context.</param>
		/// <returns>User object.</returns>
		public static User GetSessionUser(
			HttpContext context)
		{
			String userString = context.Session.GetString("User");
			if (!String.IsNullOrEmpty(userString))
			{
				User user = new User();

				Reflection.CopyProperties(JsonSerializer.Deserialize<UserLight>(userString), user);

				return user;
			}

			return null;
		}

		/// <summary>
		/// Sets the user object in context session data after loading again from the database.
		/// </summary>
		/// <param name="context">Current HTTP context.</param>
		public static void RefreshSessionUser(
			HttpContext context)
		{
			User user = new User()
			{
				UserId = Util.GetUser(context).UserId
			};
			user.Read(user.UserId);
			UserLight userLight = new UserLight();
			Reflection.CopyProperties(user, userLight);
			context.Session.SetString("User", JsonSerializer.Serialize(userLight));
		}

		internal static object GetUser(object httpContext)
		{
			throw new NotImplementedException();
		}
		#endregion
	}

	public class InOut
	{
		/// <summary>
		/// Primary key.
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// Employee full name
		/// </summary>
		public String EmployeeName
		{
			get; set;
		}

		/// <summary>
		/// Office phone extension
		/// </summary>
		public String OfficeExtension
		{
			get; set;
		} = "";

		/// <summary>
		/// Office name
		/// </summary>
		public String Office
		{
			get; set;
		}

		/// <summary>
		/// The user's birth date
		/// </summary>
		public Boolean WFHFlag
		{
			get; set;
		}

		/// <summary>
		/// The user's birth date
		/// </summary>
		public Boolean InOutFlag
		{
			get; set;
		}

		/// <summary>
		/// The user's birth date
		/// </summary>
		public String InOutNote
		{
			get; set;
		} = "";

		/// <summary>
		/// The last time the in/out status was updated
		/// </summary>
		public DateTime? InOutDatetime
		{
			get; set;
		} = null;
	}
}
