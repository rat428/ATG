using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATGHR.Models.Database
{
	public class IdTextList
	{
		/// <summary>
		/// The ID.
		/// </summary>
		public Int64? Id { get; set; } = null;

		public String Text { get; set; } = null;

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
