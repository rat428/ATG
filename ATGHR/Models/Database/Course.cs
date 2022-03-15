using System;
using System.ComponentModel;
using ATGHR.Models.Enums;

namespace ATGHR.Models.Database
{
	public class Course
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public RecommendedLevelForMastery RecommendedLevelForMastery { get; set; }
		public CourseSubject SubjectArea { get; set; }
		public string SubjectAreaText { get; set; }
		public CourseType CourseType { get; set; }
		public string Source { get; set; }
		public string CourseDetails { get; set; }
		public string LearningObjectives { get; set; }
		public string SkillsCovered { get; set; }
		public string ContinuingEducationUnits { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }
		public bool IsActive { get; set; }

		public Course()
		{
			DisplayOrder = 500;
			CreatedDate = DateTime.UtcNow;
			UpdatedDate = DateTime.UtcNow;
			IsActive = true;
		}
	}
}
