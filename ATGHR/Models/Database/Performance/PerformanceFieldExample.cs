using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using ATG.DBNet;
using ATGHR.Code;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATGHR.Models.Database.Performance
{
	public class PerformanceFieldExampleLight : CRUDObject
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Int64? PerformanceFieldExampleId { get; set; } = null;

		public String PerformanceFieldExampleText { get; set; } = null;

		public Int64? PerformanceFieldExampleCategoryId { get; set; } = null;

		/// <summary>
		/// Is this example for everyone? 
		/// </summary>
		public Boolean General { get; set; } = true;

		/// <summary>
		/// The user who last edited the record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Timestamp of the last record edit.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;

		/// <summary>
		/// Is this performance field active? 
		/// </summary>
		public Boolean ActiveFlag { get; set; } = true;
	}
	[StoredProcedure(StoredProcedureType = StoredProcedureType.Update, Name = "SP_TotalPerformanceFieldExample_Update")]
	[StoredProcedure(StoredProcedureType = StoredProcedureType.Create, Name = "SP_TotalPerformanceFieldExample_Create")]
	public class PerformanceFieldExample : PerformanceFieldExampleLight
	{
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceFieldExampleRole> PerformanceFieldExampleRoles { get; set; } = null;

		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceFieldExampleDivision> PerformanceFieldExampleDivisions { get; set; } = null;
	}
}
