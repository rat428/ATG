using System;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	/// <summary>
	/// User:user relationship type. Used in UserRelationship link table.
	/// </summary>
	public class UserRelationshipType : CRUDObject
	{
		/// <summary>
		/// Primary key.
		/// </summary>
		[Key]
		public Int64? UserRelationshipTypeId { get; set; } = null;

		/// <summary>
		/// Type name.
		/// </summary>
		[StringLength(150)]
		public String Name { get; set; } = null;

		/// <summary>
		/// Type description.
		/// </summary>
		public String Description { get; set; } = null;

		/// <summary>
		/// The user who last edited the record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Timestamp of the last record edit.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;
	}
}
