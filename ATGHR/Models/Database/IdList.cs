using System;
using System.Collections.Generic;
using System.Linq;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	/// <summary>
	/// TY_IdList object. List of 64-bit integer IDs.
	/// </summary>
	public class IdList : CRUDObject
	{
		/// <summary>
		/// The ID.
		/// </summary>
		public Int64? Id { get; set; } = null;

		/// <summary>
		/// Create an array of IDList records from a list of Int64.
		/// </summary>
		/// <param name="list">The original list of 64-bit integers.</param>
		/// <returns>Array of IDList records.</returns>
		public static IEnumerable<IdList> FromList(
			IEnumerable<Int64?> list)
		{
			return list.Select(i => new IdList { Id = i });
		}
	}
}
