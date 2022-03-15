using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	public class PerformanceFieldLight
	{
		/// <summary>
		/// Primary key.
		/// </summary>
		[Key]
		public Int64? PerformanceFieldId { get; set; } = null;

		/// <summary>
		/// Year during which the performance field is applicable.
		/// </summary>
		public Int32 Year { get; set; } = DateTime.Now.Year;

		/// <summary>
		/// Key of parent performance field.
		/// </summary>
		[RequiredProperty]
		public Int64? ParentPerformanceFieldId { get; set; } = null;

		/// <summary>
		/// Is this field related to goals.
		/// </summary>
		public Boolean Goals { get; set; } = true;

		/// <summary>
		/// Is this field locked for editing on the goals sheet?
		/// </summary>
		public Boolean GoalsReadOnly { get; set; } = false;

		/// <summary>
		/// Is this field related to reviews.
		/// </summary>
		public Boolean Review { get; set; } = true;

		/// <summary>
		/// Is this field locked for editing on the review sheet?
		/// </summary>
		public Boolean ReviewReadOnly { get; set; } = false;

		/// <summary>
		/// Whether the text from this field in goal setting should be used as the description of the same field in reviews.
		/// </summary>
		public Boolean GoalTextInReview { get; set; } = false;

		/// <summary>
		/// Whether the field (and its possible children) are duplicatable?
		/// </summary>
		public Boolean AllowMultiple { get; set; } = false;

		/// <summary>
		/// For duplicatable fields, the minimum allowable quantity.
		/// </summary>
		public Int32? MinimumMultiple { get; set; } = null;

		/// <summary>
		/// For duplicatable fields, the maximum allowable quantity.
		/// </summary>
		public Int32? MaximumMultiple { get; set; } = null;

		/// <summary>
		/// Whether this field may be weighted by the employee during performance reviews.
		/// </summary>
		public Boolean EmployeeWeighted { get; set; } = false;

		/// <summary>
		/// Is this field scored.
		/// </summary>
		public Boolean Scored { get; set; } = true;

		/// <summary>
		/// Is the score of this performance field held in the parent performance field.
		/// </summary>
		public Boolean ScoreApplied { get; set; } = true;

		/// <summary>
		/// Is this performance field a date.
		/// </summary>
		public Boolean Date { get; set; } = true;

		/// <summary>
		/// Performance Field's name.
		/// </summary>
		[StringLength(150)]
		public String Name { get; set; } = null;

		/// <summary>
		/// Performance Field's primary description.
		/// </summary>
		public String Description { get; set; } = null;

		/// <summary>
		/// Performance Field's secondary description.
		/// </summary>
		public String Description2 { get; set; } = null;

		/// <summary>
		/// Whether the field requires associated text.
		/// </summary>
		public Boolean NeedsText { get; set; } = true;

		/// <summary>
		/// Whether, for fields requiring text, an empty text box will be accepted.
		/// </summary>
		public Boolean AllowEmpty { get; set; } = true;

		/// <summary>
		/// Whether the field should allow comments to be made when it appears in goal and review sheets.
		/// </summary>
		public Boolean AllowComment { get; set; } = false;

		/// <summary>
		/// Is this performance field active? 
		/// </summary>
		public Boolean ActiveFlag { get; set; } = true;

		/// <summary>
		/// The user who last edited the record.
		/// </summary>
		public Int64? DataUserId { get; set; } = null;

		/// <summary>
		/// Timestamp of the last record edit.
		/// </summary>
		public DateTime? DataDatetime { get; set; } = null;

		/// <summary>
		/// Determines if the performance field is an example
		/// </summary>
		public Boolean Example { get; set; } = false;

		/// <summary>
		/// The nullable performanceFieldExampleCategory foreign key 
		/// </summary>
		public Int64? PerformanceFieldExampleCategoryId { get; set; } = null;
	}

	public class PerformanceField : PerformanceFieldLight
	{
		/// <summary>
		/// Is this performance field an employee's comment.
		/// </summary>
		public Boolean EmployeeComment { get; set; } = true;

		/// <summary>
		/// Is this performance field Supervisor's Comment.
		/// </summary>
		public Boolean SupervisorComment { get; set; } = true;

		/// <summary>
		/// Is this performance field HR's comment.
		/// </summary>
		public Boolean HRComment { get; set; } = true;

		/// <summary>
		/// Is this performance field hidden from the employee for confidential supervisor comments.
		/// </summary>
		public Boolean HideFromEmployee { get; set; } = true;

		/// <summary>
		/// Is this performance field hidden from the supervisor. To be eventually used for manager review by a subordinate. 
		/// </summary>
		public Boolean HideFromSupervisor { get; set; } = true;

		/// <summary>
		/// Whether the field requires a comment from the employee or supervisor.
		/// </summary>
		public Boolean NeedsComment { get; set; } = false;

		/// <summary>
		/// Date a performance field was made active. 
		/// </summary>
		public DateTime? ActiveDate { get; set; } = null;

		/// <summary>
		/// Date a performance field was made inactive. 
		/// </summary>
		public DateTime? DeactiveDate { get; set; } = null;
	}
}
