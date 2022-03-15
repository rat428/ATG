using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ATG.DBNet;
using System.ComponentModel.DataAnnotations;

namespace ATGHR.Models.Database
{
	public class PerformanceReviewSheetItemText : CRUDObject
	{
		/// <summary>
		/// Primary Key incremented 
		/// </summary>
		[Key]
		[TableTypeProperty]
		public Int64? PerformanceReviewSheetItemTextId { get; set; } = null;

		/// <summary>
		/// foreign key
		/// </summary>
		[TableTypeProperty]
		[ForeignKeyExclusivity(Type = ForeignKeyExclusivity.Only)]
		public Int64? PerformanceReviewSheetItemId { get; set; } = null;

		/// <summary>
		/// Foreign Key.
		/// </summary>
		[TableTypeProperty]
		public Int64? PerformanceTextTypeId { get; set; } = null;

		/// <summary>
		/// parentage foreign key
		/// </summary>
		[TableTypeProperty]
		[ForeignKey(Name = "PerformanceReviewSheetItemTextId")]
		[ForeignKeyExclusivity(Type = ForeignKeyExclusivity.Only | ForeignKeyExclusivity.NonNull)]
		public Int64? ParentPerformanceReviewSheetItemTextId { get; set; } = null;

		/// <summary>
		/// Related user, Foreign Key.
		/// </summary>
		[TableTypeProperty]
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// Review questions
		/// </summary>
		[TableTypeProperty]
		public String Contents { get; set; } = null;

		/// <summary>
		/// Temporary ID, for use in table uploads only.
		/// </summary>
		[TableTypeProperty(Only = true)]
		public Int64? PerformanceReviewSheetItemIdTemp { get; set; } = null;

		/// <summary>
		/// The user who last edited the record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Timestamp of last record edit.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;

		/// <summary>
		/// Child Items under this item.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceReviewSheetItemText> PerformanceReviewSheetItemTexts { get; set; }

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

				if (PerformanceReviewSheetItemTextId == null)
				{
					extraParameters = new List<SqlParameter>()
					{
						new SqlParameter("@PerformanceReviewSheetItemId", PerformanceReviewSheetItemId),
						new SqlParameter("@PerformanceTextTypeId", PerformanceTextTypeId),
						new SqlParameter("@ParentPerformanceReviewSheetItemTextId", (Object)ParentPerformanceReviewSheetItemTextId ?? DBNull.Value),
						new SqlParameter("@UserId", DataUserId)
					};

					SQL = "SP_PerformanceReviewSheetComment_Create";
				}
				else
				{
					extraParameters = new List<SqlParameter>()
					{
						new SqlParameter("@PerformanceReviewSheetItemTextId", PerformanceReviewSheetItemTextId)
					};

					SQL = "SP_PerformanceReviewSheetComment_Update";
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
		/// <param name="PerformanceReviewSheetItemTextId">Comment ID to be deleted.</param>
		/// <param name="DataUserId">The user who is deleting the comment.</param>
		/// <returns>Execute result object.</returns>
		public static ExecuteResult CommentDelete(
			Int64? PerformanceReviewSheetItemTextId,
			Int64? DataUserId)
		{
			List<SqlParameter> extraParameters = new List<SqlParameter>()
			{
				new SqlParameter("@PerformanceReviewSheetItemTextId", PerformanceReviewSheetItemTextId),
				new SqlParameter("@DataUserId", DataUserId),
				new SqlParameter("@DataDatetime", DBNull.Value)
			};

			String SQL = "SP_PerformanceReviewSheetComment_Delete";

			return DBNet.ExecuteToResult(SQL, parameters: extraParameters);
		}
	}
}
