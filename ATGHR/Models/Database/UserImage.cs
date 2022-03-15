using System;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	/// <summary>
	/// User profile picture. One:one to User table.
	/// </summary>
	public class UserImage : CRUDObject
	{
		/// <summary>
		/// Related user. Primary key.
		/// </summary>
		[Key]
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// Image binary contents.
		/// </summary>
		public Byte[] Image { get; set; } = null;

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
