using ATGHR.Models.Database;

namespace ATGHR.Models.Dtos
{
	public class AddNewCourseDto
	{
		public bool IsFromPost { get; set; }
		public bool IsError { get; set; }
		public string ErrorDescription { get; set; }
		public Course Course { get; set; }
	}
}
