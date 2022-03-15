using System;
using System.Collections.Generic;
using System.Linq;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	/// <summary>
	/// TY_IdPairList object. List of two associated 64-bit integer IDs.
	/// </summary>
	public class IdPairList : CRUDObject
	{
		/// <summary>
		/// The left ID.
		/// </summary>
		public Int64? Id1 { get; set; } = null;

		/// <summary>
		/// The right ID.
		/// </summary>
		public Int64? Id2 { get; set; } = null;

		/// <summary>
		/// Create an array of IDList records from a list of Int64 pairs.
		/// </summary>
		/// <param name="list">The original list of 64-bit integer pairs.</param>
		/// <returns>Array of IDList records.</returns>
		public static IEnumerable<IdPairList> FromList(
			IEnumerable<Tuple<Int64?, Int64?>> list)
		{
			return list.Select(i => new IdPairList { Id1 = i.Item1, Id2 = i.Item2 });
		}
	}
}
