using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ATG.DBNet;
using ATGHR.Code;
using ATGHR.Models.Database;
using ATGHR.Models.Database.Performance;

namespace ATGHR.Models
{
	/// <summary>
	/// All standard lookup tables, pre-filled to avoid database calls when they need to be referenced.
	/// </summary>
	[NoPrimaryRecord]
	[StoredProcedure(StoredProcedureType = StoredProcedureType.Read, Name = "SP_AllLookupTables_Read")]
	public class LookupTables : CRUDObject
	{
		#region Updated Timestamps
		/// <summary>
		/// When the Roles table was last updated in the database.
		/// </summary>
		[DBIgnore]
		public DateTime RolesUpdated
		{
			get
			{
				return Roles?.Max(o => o.DataDatetime) ?? DateTime.MinValue;
			}
		}

		/// <summary>
		/// When the Divisions table was last updated in the database.
		/// </summary>
		[DBIgnore]
		public DateTime DivisionsUpdated
		{
			get
			{
				return Divisions?.Max(o => o.DataDatetime) ?? DateTime.MinValue;
			}
		}

		/// <summary>
		/// When the ATGOffices table was last updated in the database.
		/// </summary>
		[DBIgnore]
		public DateTime ATGOfficesUpdated
		{
			get
			{
				return ATGOffices?.Max(o => o.DataDatetime) ?? DateTime.MinValue;
			}
		}

		/// <summary>
		/// When the UserRelationshipTypes table was last updated in the database.
		/// </summary>
		[DBIgnore]
		public DateTime UserRelationshipTypesUpdated
		{
			get
			{
				return UserRelationshipTypes?.Max(o => o.DataDatetime) ?? DateTime.MinValue;
			}
		}

		/// <summary>
		/// When the Permissions table was last updated in the database.
		/// </summary>
		[DBIgnore]
		public DateTime PermissionsUpdated
		{
			get
			{
				return Permissions?.Max(o => o.DataDatetime) ?? DateTime.MinValue;
			}
		}

		/// <summary>
		/// When the PerformanceTextTypes table was last updated in the database.
		/// </summary>
		[DBIgnore]
		public DateTime PerformanceTextTypesUpdated
		{
			get
			{
				return PerformanceTextTypes?.Max(o => o.DataDatetime) ?? DateTime.MinValue;
			}
		}

		/// <summary>
		/// When the Users table was last updated in the database.
		/// </summary>
		[DBIgnore]
		public DateTime UsersUpdated
		{
			get
			{
				return Users?.Max(o => o.DataDatetime) ?? DateTime.MinValue;
			}
		}

		/// <summary>
		/// When the PerformanceScores table was last updated in the database.
		/// </summary>
		[DBIgnore]
		public DateTime PerformanceScoresUpdated
		{
			get
			{
				return PerformanceScores?.Max(o => o.DataDatetime) ?? DateTime.MinValue;
			}
		}

		/// <summary>
		/// When the PerformanceFields table was last updated in the database.
		/// </summary>
		[DBIgnore]
		public DateTime PerformanceFieldsUpdated
		{
			get
			{
				return PerformanceFields?.Max(o => o.DataDatetime) ?? DateTime.MinValue;
			}
		}

		/// <summary>
		/// When the DivisionYearPerformanceFieldDescription table was last updated in the database.
		/// </summary>
		[DBIgnore]
		public DateTime DivisionPerformanceFieldDescriptionsUpdated
		{
			get
			{
				return DivisionPerformanceFieldDescriptions?.Max(o => o.DataDatetime) ?? DateTime.MinValue;
			}
		}//DivisionYearPerformanceFieldDescription
		#endregion

		#region Lookup Lists
		/// <summary>
		/// All the roles in the database.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<Role>  Roles { get; set; } = null;

		/// <summary>
		/// All ATG divisions in the database.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<DivisionLight>  Divisions { get; set; } = null;

		/// <summary>
		/// All ATG offices in the database.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<ATGOffice>  ATGOffices { get; set; } = null;

		/// <summary>
		/// All user relationship types in the database.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<UserRelationshipType>  UserRelationshipTypes { get; set; } = null;

		/// <summary>
		/// All permission types in the database.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<Permission>  Permissions { get; set; } = null;

		/// <summary>
		/// All performance (sheet/item) text types in the database.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceTextType>  PerformanceTextTypes { get; set; } = null;

		/// <summary>
		/// All users in the database.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<UserLookup>  Users { get; set; } = null;

		/// <summary>
		/// All possible scores for scored items in performance reviews.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceScore>  PerformanceScores { get; set; } = null;

		/// <summary>
		/// All possible performance fields for items, goals, texts, files, reviews, etc.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceFieldLight>  PerformanceFields { get; set; } = null;

		/// <summary>
		/// All possible performance field example categories
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceFieldExampleCategory> PerformanceFieldExampleCategories { get; set; } = null;

		/// <summary>
		/// All possible division-year-performance field descriptions.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<DivisionPerformanceFieldDescription>  DivisionPerformanceFieldDescriptions { get; set; } = null;
		#endregion

		/// <summary>
		/// Initialize by reading all tables from the database.
		/// </summary>
		/// <param name="connectionString"></param>
		public LookupTables(
			DBConnector db,
			IConfiguration configuration,
			String connectionString)
		{
			Read(db: db);
		}

		/// <summary>
		/// Initialize by reading all tables from the database.
		/// </summary>
		/// <param name="connectionString"></param>
		public LookupTables(
			IConfiguration configuration,
			String connectionString)
		{
			Read();
		}

		/// <summary>
		/// Refresh all lookup data from the database.
		/// </summary>
		public void Reload()
		{
			Read();
		}

		/// <summary>
		/// Return just timestamps for all lookup tables. For client-side caching.
		/// </summary>
		/// <returns>Anonymous object with all timestamps.</returns>
		public Object UpdateTimestamps()
		{
			return new
			{
				RolesUpdated,
				DivisionsUpdated,
				ATGOfficesUpdated,
				UserRelationshipTypesUpdated,
				PermissionsUpdated,
				PerformanceTextTypesUpdated,
				UsersUpdated,
				PerformanceScoresUpdated,
				PerformanceFieldsUpdated
			};
		}
	}
}
