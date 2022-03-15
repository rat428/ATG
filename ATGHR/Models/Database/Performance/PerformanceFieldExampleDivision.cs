using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;

namespace ATGHR.Models.Database.Performance
{
	public class PerformanceFieldExampleDivision : CRUDObject
	{
		[Key]
		public Int64? PerformanceFieldExampleDivisionId { get; set; } = null;

		public virtual Int64? PerformanceFieldExampleId { get; set; } = null;

		public virtual Int64? DivisionId { get; set; } = null;

		/// <summary>
		/// The user who last edited the record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Timestamp of the last record edit.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;
	}
	public class PerfromanceFieldExampleDivisionList : PerformanceFieldExampleDivision
	{
		#region Properties
		[Key]
		[ForeignKey(Name = "PerformanceFieldExampleId")]
		[ForeignKeyExclusivity(Type = ForeignKeyExclusivity.Only)]
		public override Int64? PerformanceFieldExampleId { get; set; } = null;

		[Key]
		[ForeignKey(Name = "DivisionId")]
		[ForeignKeyExclusivity(Type = ForeignKeyExclusivity.Only)]
		public override Int64? DivisionId { get; set; } = null;
		#endregion
	}
}
