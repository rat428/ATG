using System.Collections.Generic;
using ATGHR.Models.Database;

namespace ATGHR.Models.Dtos
{
	public class ResourcesFileListDto
	{
		public List<CommonFile> Files { get; set; }
		public User UserI  { get; set; }
	}
}
