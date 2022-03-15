using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	/// <summary>
	/// User:role link object.
	/// </summary>
	public class UserRole : CRUDObject
	{
		#region Properties
		/// <summary>
		/// User primary key.
		/// </summary>
		[Key]
		[ForeignKey]
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// Role primary key.
		/// </summary>
		[Key]
		[ForeignKey]
		public Int64? RoleId { get; set; } = null;

		/// <summary>
		/// Whether this role is the user's "main" role (it sets the contents of their review sheets).
		/// </summary>
		public Boolean MainRole { get; set; } = false;

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
}
