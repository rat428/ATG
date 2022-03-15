using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;
using System.Collections.Generic;

namespace ATGHR.Models.Database
{
	public class PerformanceGoalSheetItem : CRUDObject
	{
		#region Properties
		/// <summary>
		/// Primary Key
		/// </summary>
		[Key]
		[TableTypeProperty]
		public Int64? PerformanceGoalSheetItemId { get; set; } = null;

		/// <summary>
		/// Foreign Key
		/// </summary>
		[Required]
		[TableTypeProperty]
		[ForeignKeyExclusivity(Type = ForeignKeyExclusivity.Only)]
		public Int64? PerformanceGoalSheetId { get; set; } = null;

		/// <summary>
		/// Foreign key. Parent Item.
		/// </summary>
		[TableTypeProperty]
		[ForeignKey(Name = "PerformanceGoalSheetItemId")]
		[ForeignKeyExclusivity(Type = ForeignKeyExclusivity.Only | ForeignKeyExclusivity.NonNull)]
		public Int64? ParentPerformanceGoalSheetItemId { get; set; } = null;

		/// <summary>
		/// Foreign key. Used when placing goal sheet items into review sheet items.
		/// </summary>
		[ForeignKeyExclusivity(Type = ForeignKeyExclusivity.Only | ForeignKeyExclusivity.NonNull)]
		public Int64? PerformanceReviewSheetItemId { get; set; } = null;

		/// <summary>
		/// Goal due date.
		/// </summary>
		[TableTypeProperty]
		public DateTime? PerformanceGoalSheetItemDate { get; set; } = null;

		/// <summary>
		/// The performance review in which this goal item is referenced.
		/// </summary>
		[TableTypeProperty]
		public Int64? PerformanceFieldId { get; set; } = null;

		/// <summary>
		/// Display order specific to the user's sheet.
		/// </summary>
		[ReadOnly]
		public Int32? DisplayOrder { get; set; } = null;

		/// <summary>
		/// Weight within parent category, if scored.
		/// </summary>
		[TableTypeProperty]
		public Decimal? Weight { get; set; } = null;

		/// <summary>
		/// Display order within its parent context.
		/// </summary>
		[TableTypeProperty]
		public Int32? ItemOrder { get; set; } = null;

		/// <summary>
		/// Temporary ID, for use in table uploads only.
		/// </summary>
		[TableTypeProperty(Only = true)]
		public Int64? PerformanceGoalSheetItemIdTemp { get; set; } = null;

		/// <summary>
		/// Temporary parent ID, for use in table uploads only.
		/// </summary>
		[TableTypeProperty(Only = true)]
		public Int64? ParentPerformanceGoalSheetItemIdTemp { get; set; } = null;

		/// <summary>
		/// The user who last edited the record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Time stamp of last record change.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;

		/// <summary>
		/// Goal texts
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceGoalSheetItemText> PerformanceGoalSheetItemTexts { get; set; } = null;

		/// <summary>
		/// File attached to goal
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceGoalSheetItemFile> PerformanceGoalSheetItemFiles { get; set; } = null;

		/// <summary>
		/// Child Items under this item.
		/// </summary>
		[LinkType(LinkType = LinkType.Table)]
		public IEnumerable<PerformanceGoalSheetItem> PerformanceGoalSheetItems { get; set; } = null;
		#endregion
	}
}
