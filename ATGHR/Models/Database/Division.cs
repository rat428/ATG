using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	public class DivisionLight : CRUDObject
	{
		#region Properties
		/// <summary>
		/// Primary key.
		/// </summary>
		[Key]
		public Int64? DivisionId { get; set; } = null;

		/// <summary>
		/// Division name.
		/// </summary>
		[StringLength(150)]
		public String Name { get; set; } = null;

		/// <summary>
		/// Division description.
		/// </summary>
		public String Description { get; set; } = null;

		/// <summary>
		/// Parent Division, if any.
		/// </summary>
		[RequiredProperty]
		public Int64? ParentDivisionId { get; set; } = null;

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
	/// User ATG division. One:many to User table.
	/// </summary>
	public class Division : DivisionLight
	{
		#region Properties
		/// <summary>
		/// Parent Division, if any.
		/// </summary>
		[DBIgnore]
		public DivisionLight ParentDivision
		{
			get
			{
				return ATGHR.LookupTables?.Divisions?.FirstOrDefault(d => d.DivisionId == ParentDivisionId);
			}
		}
		#endregion

		public Division() : base() { }
	}
}
