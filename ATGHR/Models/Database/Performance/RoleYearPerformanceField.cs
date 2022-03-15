using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	public class RoleYearPerformanceField : CRUDObject
	{
		/// <summary>
		/// Role primary key.
		/// </summary>
		[Key]
		public Int64? RoleId { get; set; } = null;

		/// <summary>
		/// Year.
		/// </summary>
		[Key]
		public Int32? Year { get; set; } = null;

		/// <summary>
		/// Performance Field primary key.
		/// </summary>
		[Key]
		public Int64? PerformanceFieldId { get; set; } = null;

		/// <summary>
		/// Weight of a performance field based on the role it is associated with.
		/// </summary>
		public Decimal? Weight { get; set; } = null;

		/// <summary>
		/// Performance Field's on screen display order. back up is the order in which the items came from the database. 
		/// </summary>
		public Int32 DisplayOrder
		{
			get; set;
		} = 0;

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
