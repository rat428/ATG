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
	public class PerformanceFieldExampleCategory : CRUDObject
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Int64? PerformanceFieldExampleCategoryId { get; set; } = null;

		[RequiredProperty]
		public String PerformanceFieldExampleCategoryText { get; set; } = null;

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
}
