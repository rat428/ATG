using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ATGHR.Code.Enums;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	/// <summary>
	/// User permission. One:many to User table.
	/// </summary>
	public class Permission : CRUDObject
	{
		/// <summary>
		/// Primary key.
		/// </summary>
		[Key]
		public Int64? PermissionId { get; set; } = null;

		/// <summary>
		/// Permission name.
		/// </summary>
		[StringLength(150)]
		public String Name { get; set; } = null;

		/// <summary>
		/// Permission description.
		/// </summary>
		public String Description { get; set; } = null;

		/// <summary>
		/// Whether the permission is able to be used.
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

		/// <summary>
		/// Get the enumerable permission type that matches this permission.
		/// </summary>
		[DBIgnore]
		public PermissionType PermissionType
		{
			get
			{
				return (PermissionType)Enum.Parse(typeof(PermissionType), Name.Replace(" ", ""));
			}
		}
	}
}
