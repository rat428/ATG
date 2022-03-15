using System;
using ATGHR.Models.Enums;
using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace ATGHR.Models.Database
{
	public class CommonFile
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string FileName { get; set; }
		public string OriginalFileName { get; set; }
		public string Extension { get; set; }
		public string Description { get; set; }
		public SopCategory SopCategory { get; set; }
		public DateTime? AddedDate { get; set; }
	}
}

