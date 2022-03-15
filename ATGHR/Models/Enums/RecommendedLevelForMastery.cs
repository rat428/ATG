using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ATGHR.Models.Enums
{
	public enum RecommendedLevelForMastery
	{
		[Display(Name = "Learning A")]
		[Description("Learning A")]
		LearningA = 1,
		[Display(Name = "Learning B")]
		[Description("Learning B")]
		LearningB = 2,
		[Display(Name = "Teaching A")]
		[Description("Teaching A")]
		TeachingA = 3,
		[Display(Name = "Teaching B")]
		[Description("Teaching B")]
		TeachingB = 4,
	}
}
