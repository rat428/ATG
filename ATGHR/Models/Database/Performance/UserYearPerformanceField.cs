using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;


namespace ATGHR.Models.Database
{
	public class UserYearPerformanceField
	{
		/// <summary>
		/// The User primary key.
		/// </summary>
		[Key]
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// Performance Field primary key.
		/// </summary>
		[Key]
		public Int64? PerformanceFieldId { get; set; } = null;

		/// <summary>
		/// The Year this pairing relates to
		/// </summary>
		public Int64? Year { get; set; } = null;


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
		/// Timestamp of the last record edit.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;
	}
}
