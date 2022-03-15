using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	public class DivisionPerformanceFieldDescription : CRUDObject
	{
		/// <summary>
		/// Division primary key.
		/// </summary>
		[Key]
		public Int64? DivisionId { get; set; } = null;

		/// <summary>
		/// Performance Field primary key.
		/// </summary>
		[Key]
		public Int64? PerformanceFieldId { get; set; } = null;

		/// <summary>
		/// Description associated with this combination.
		/// </summary>
		public String Description { get; set; } = null;

		/// <summary>
		/// The user who last edited the record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Timestamp of the last recorded edit.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;
	}
}
