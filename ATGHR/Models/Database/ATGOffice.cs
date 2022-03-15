using System;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	/// <summary>
	/// User ATG division. One:many to User table.
	/// </summary>
	public class ATGOffice : CRUDObject
	{
		/// <summary>
		/// Primary key.
		/// </summary>
		[Key]
		public Int64? ATGOfficeId { get; set; } = null;

		/// <summary>
		/// Office name.
		/// </summary>
		[StringLength(150)]
		public String Name { get; set; } = null;

		/// <summary>
		/// Office description - street address, other.
		/// </summary>
		public String Description { get; set; } = null;

		/// <summary>
		/// The office location's street address.
		/// </summary>
		public String StreetAddress { get; set; } = null;

		/// <summary>
		/// The office location's address unit.
		/// </summary>
		public String Unit { get; set; } = null;

		/// <summary>
		/// The office location's address city.
		/// </summary>
		public String City { get; set; } = null;

		/// <summary>
		/// The office location's address state.
		/// </summary>
		public String State { get; set; } = null;

		/// <summary>
		/// The office location's address zip code.
		/// </summary>
		public String Zip { get; set; } = null;

		/// <summary>
		/// The office location's full street address.
		/// </summary>
		public String Address
		{
			get
			{
				return String.Format(
					"{0} {1} {2}, {3} {4}",
					StreetAddress,
					Unit,
					City,
					State,
					Zip);
			}
		}

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
