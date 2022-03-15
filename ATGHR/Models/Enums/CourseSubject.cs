using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ATGHR.Models.Enums
{
	public enum CourseSubject
	{
		[Display(Name = "Business Development (BD)")]
		[Description("Business Development (BD)")]
		BD = 1,
		[Display(Name = "Communication (Comm)")]
		[Description("Communication (Comm)")]
		Comm = 2,
		[Display(Name = "Human Resource (HR)")]
		[Description("Human Resource (HR)")]
		HR = 3,
		[Display(Name = "Personal Development (Pers)")]
		[Description("Personal Development (Pers)")]
		Pers = 4,
		[Display(Name = "Project Delivery (PD)")]
		[Description("Project Delivery (PD)")]
		PD = 5,
		[Display(Name = "Project Management (PM)")]
		[Description("Project Management (PM)")]
		PM = 6,
		[Display(Name = "Technical Development (Tech)")]
		[Description("Technical Development (Tech)")]
		Tech = 7,
	}
}
