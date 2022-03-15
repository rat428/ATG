using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;
using System.Text.Json.Serialization;

namespace ATGHR.Models.Database
{
	public class PerformanceReviewSheetItem : CRUDObject
	{
		/// <summary>
		/// Primary Key.
		/// </summary>
		[Key]
		[TableTypeProperty]
		public Int64? PerformanceReviewSheetItemId { get; set; } = null;

		/// <summary>
		/// Related review sheet, Foreign Key.
		/// </summary>
		[Required]
		[TableTypeProperty]
		[ForeignKeyExclusivity(Type = ForeignKeyExclusivity.Only)]
		public Int64? PerformanceReviewSheetId { get; set; } = null;

		/// <summary>
		/// Foreign key. Parent Item.
		/// </summary>
		[ForeignKey(Name = "PerformanceReviewSheetItemId")]
		[TableTypeProperty]
		[ForeignKeyExclusivity(Type = ForeignKeyExclusivity.Only | ForeignKeyExclusivity.NonNull)]
		public Int64? ParentPerformanceReviewSheetItemId { get; set; } = null;

		/// <summary>
		/// Foreign key. Not directly used in functionality at the moment.
		/// </summary>
		[JsonIgnore]
		public Int64? PerformanceGoalSheetItemId { get; set; } = null;

		/// <summary>
		/// Foreign Key.
		/// </summary>
		[TableTypeProperty]
		public Int64? PerformanceFieldId { get; set; } = null;

		/// <summary>
		/// Foreign Key.
		/// </summary>
		[TableTypeProperty]
		public Int64? PerformanceScoreId { get; set; } = null;

		/// <summary>
		/// User-set weight for the item, applied in scores instead of PerformanceField.Weight.
		/// </summary>
		[TableTypeProperty]
		public Decimal? Weight { get; set; } = null;

		/// <summary>
		/// Display order specific to the user's sheet.
		/// </summary>
		[ReadOnly]
		public Int32? DisplayOrder { get; set; } = null;

		/// <summary>
		/// Display order within its parent context.
		/// </summary>
		[TableTypeProperty]
		public Int32? ItemOrder { get; set; } = null;

		/// <summary>
		/// Temporary ID, for use in table uploads only.
		/// </summary>
		[TableTypeProperty(Only = true)]
		public Int64? PerformanceReviewSheetItemIdTemp { get; set; } = null;

		/// <summary>
		/// Temporary parent ID, for use in table uploads only.
		/// </summary>
		[TableTypeProperty(Only = true)]
		public Int64? ParentPerformanceReviewSheetItemIdTemp { get; set; } = null;

		/// <summary>
		/// User who last edited this record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Timestamp of most recent change.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;

		/// <summary>
		/// Array of related attachments.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceReviewSheetItemFile> PerformanceReviewSheetItemFiles { get; set; } = null;

		/// <summary>
		/// Array of related questions.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceReviewSheetItemText> PerformanceReviewSheetItemTexts { get; set; } = null;

		/// <summary>
		/// Child items under this item.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceReviewSheetItem> PerformanceReviewSheetItems { get; set; } = null;

		/// <summary>
		/// Related goal item, for displaying goal text.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public PerformanceGoalSheetItem PerformanceGoalSheetItem { get; set; } = null;
	}
}
