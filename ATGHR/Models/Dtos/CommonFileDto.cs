using ATGHR.Models.Database;

namespace ATGHR.Models.Dtos
{
	public class CommonFileDto
	{
		public bool IsFromPost { get; set; }
		public bool IsError { get; set; }
		public string ErrorDescription { get; set; }
		public CommonFile CommonFile { get; set; }
		public string ReturnUrl { get; set; }
	}
}
