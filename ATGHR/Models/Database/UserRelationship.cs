using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	public class UserRelationshipLight : CRUDObject
	{
		#region Properties
		/// <summary>
		/// First user.
		/// </summary>
		[Key]
		public Int64? UserId1 { get; set; } = null;

		/// <summary>
		/// Second user.
		/// </summary>
		[Key]
		public Int64? UserId2 { get; set; } = null;

		/// <summary>
		/// Relationship type.
		/// </summary>
		public Int64? UserRelationshipTypeId { get; set; } = null;

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
	public class UserRelationship : UserRelationshipLight
	{
		#region Properties
		/// <summary>
		/// Relationship type.
		/// </summary>
		[DBIgnore]
		public UserRelationshipType UserRelationshipType
		{
			get
			{
				return ATGHR.LookupTables?.UserRelationshipTypes?.FirstOrDefault(t => t.UserRelationshipTypeId == UserRelationshipTypeId);
			}
		}
		#endregion
	}
}
