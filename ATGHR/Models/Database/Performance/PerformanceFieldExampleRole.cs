using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;

namespace ATGHR.Models.Database.Performance
{
	public class PerformanceFieldExampleRole : CRUDObject
	{
		[Key]
		public Int64? PerformanceFieldExampleRoleId { get; set; } = null;

		public virtual Int64? PerformanceFieldExampleId { get; set; } = null;

		public virtual Int64? RoleId { get; set; } = null;

		/// <summary>
		/// The user who last edited the record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Timestamp of the last record edit.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;
	}

	public class PerfromanceFieldExampleRoleList : PerformanceFieldExampleRole
	{
		#region Properties
		[ForeignKey(Name = "PerformanceFieldExampleId")]
		[ForeignKeyExclusivity(Type = ForeignKeyExclusivity.Only)]
		public override Int64? PerformanceFieldExampleId { get; set; } = null;

		[Key]
		[ForeignKey(Name = "RoleId")]
		[ForeignKeyExclusivity(Type = ForeignKeyExclusivity.Only)]
		public override Int64? RoleId { get; set; } = null;
		#endregion
	}
}
