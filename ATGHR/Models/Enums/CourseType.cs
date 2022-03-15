using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ATGHR.Models.Enums
{
	public enum CourseType
	{
		[Display(Name = "ATG In-House Courses")]
		[Description("ATG In-House Courses")]
		ATG = 1,
		[Display(Name = "Linked In Learning Courses")]
		[Description("Linked In Learning Courses")]
		LIL = 2,
		[Display(Name = "3rd Party Training")]
		[Description("3rd Party Training")]
		ThirdParty = 3
	}
}
