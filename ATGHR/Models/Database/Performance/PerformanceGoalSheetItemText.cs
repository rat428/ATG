using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
  public class PerformanceGoalSheetItemText
  {
		/// <summary>
		/// primary incremented key 
		/// </summary>
		[Key]
		[TableTypeProperty]
		public Int64? PerformanceGoalSheetItemTextId { get; set; } = null;

		 /// <summary>
		 /// Primary foreign key.
		 /// </summary>
		[TableTypeProperty]
		[ForeignKeyExclusivity(Type = ForeignKeyExclusivity.Only)]
		public Int64? PerformanceGoalSheetItemId { get; set; } = null;

		/// <summary>
		/// Primary foreign key.
		/// </summary>
		[TableTypeProperty]
		public Int64? PerformanceTextTypeId { get; set; } = null;

		[TableTypeProperty]
		[ForeignKey(Name = "PerformanceGoalSheetItemTextId")]
		[ForeignKeyExclusivity(Type = ForeignKeyExclusivity.Only | ForeignKeyExclusivity.NonNull)]
		public Int64? ParentPerformanceGoalSheetItemTextId { get; set; } = null;

		/// <summary>
		/// Foreign Key.
		/// </summary>
		[TableTypeProperty]
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// Goal text.
		/// </summary>
		[TableTypeProperty]
		public String Contents { get; set; } = null;

		/// <summary>
		/// Temporary ID, for use in table uploads only.
		/// </summary>
		[TableTypeProperty(Only = true)]
		public Int64? PerformanceGoalSheetItemIdTemp { get; set; } = null;

		/// <summary>
		/// The user who last edited this record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Time stamp of last record change.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;

		/// <summary>
		/// Child Items under this item.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceGoalSheetItemText> PerformanceGoalSheetItemTexts { get; set; }

		/// <summary>
		/// Create or save a comment.
		/// </summary>
		/// <returns>Execute result object.</returns>
		public ExecuteResult CommentSave()
		{
			if (PerformanceTextTypeId == 2) // todo: put lookups in this object; hard-coded text type ID is bad
			{
				List<SqlParameter> extraParameters;
				String SQL;

				if (PerformanceGoalSheetItemTextId == null)
				{
					extraParameters = new List<SqlParameter>()
					{
						new SqlParameter("@PerformanceGoalSheetItemId", PerformanceGoalSheetItemId),
						new SqlParameter("@PerformanceTextTypeId", PerformanceTextTypeId),
						new SqlParameter("@ParentPerformanceGoalSheetItemTextId", (Object)ParentPerformanceGoalSheetItemTextId ?? DBNull.Value),
						new SqlParameter("@UserId", DataUserId)
					};

					SQL = "SP_PerformanceGoalSheetComment_Create";
				}
				else
				{
					extraParameters = new List<SqlParameter>()
					{
						new SqlParameter("@PerformanceGoalSheetItemTextId", PerformanceGoalSheetItemTextId)
					};

					SQL = "SP_PerformanceGoalSheetComment_Update";
				}

				extraParameters.AddRange(new List<SqlParameter>()
				{
					new SqlParameter("@Contents", Contents),
					new SqlParameter("@DataUserId", DataUserId),
					new SqlParameter("@DataDatetime", DataDatetime)
				});

				return DBNet.ExecuteToResult(SQL, parameters: extraParameters);
			}

			return new ExecuteResult()
			{
				Success = false,
				ErrorMessage = "Only comments may be saved using this method."
			};
		}

		/// <summary>
		/// Delete a comment.
		/// </summary>
		/// <param name="PerformanceGoalSheetItemTextId">Comment ID to be deleted.</param>
		/// <param name="DataUserId">The user who is deleting the comment.</param>
		/// <returns>Execute result object.</returns>
		public static ExecuteResult CommentDelete(
			Int64? PerformanceGoalSheetItemTextId,
			Int64? DataUserId)
		{
			List<SqlParameter> extraParameters = new List<SqlParameter>()
			{
				new SqlParameter("@PerformanceGoalSheetItemTextId", PerformanceGoalSheetItemTextId),
				new SqlParameter("@DataUserId", DataUserId),
				new SqlParameter("@DataDatetime", DBNull.Value)
			};

			String SQL = "SP_PerformanceGoalSheetComment_Delete";

			return DBNet.ExecuteToResult(SQL, parameters: extraParameters);
		}
	}
}
