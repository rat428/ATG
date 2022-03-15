using System.ComponentModel.DataAnnotations;

namespace ATGHR.Models.Enums
{
	public enum SopCategory
	{
		[Display(Name = "Don't list under SOP")]
		Other = 0,
		Accounting = 1,
		Corporate = 2,
		[Display(Name = "Human Resources")] Hr = 3,

		[Display(Name = "Marketing & Business Development")]
		MarketingAndBusinessDevelopment = 4,
		//Operations = 5,
		Planning = 6,
		Engineering = 7
	}
}
