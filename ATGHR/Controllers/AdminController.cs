using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ATGHR.Code;
using ATGHR.Code.Enums;
using ATGHR.Models.Database;
using ATGHR.Models.Dtos;
using ATGHR.Models.EntityFramework.DbContext;
using ATGHR.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace ATGHR.Controllers
{
	[Authorize]
	[PermissionsRequired(PermissionType = PermissionType.Self)]
	[PermissionsRequired(PermissionType = PermissionType.Administrator)]
	public class AdminController : Controller
	{
		private readonly ApplicationDbContext _context;

		public AdminController(ApplicationDbContext context)
		{
			_context = context;
		}

		[Route(template: "/Admin/User/{id?}")]
		public ActionResult UserManagement()
		{
			Int64? userId;
			if (this.RouteData.Values["id"] != null)
			{
				if (this.RouteData.Values["id"]?.ToString() == "new")
				{
					userId = -1;
				}
				else
				{
					userId = Util.Nz<Int64>(this.RouteData.Values["id"]?.ToString(), -1);
					((UserAction)HttpContext.Items["UserAction"]).UserId = userId;
				}

				return View("UserEdit", userId);
			}

			return View("User");
		}

		[Route(template: "/Admin/Reports")]
		public ActionResult AdminSheetList()
		{
			return View("Reports");
		}

		[Route(template: "/Admin/PerformanceFieldExample/{id?}")]
		public ActionResult ExampleManagement()
		{
			Int64? performanceFieldExampleId;
			if (this.RouteData.Values["id"] != null)
			{
				if (this.RouteData.Values["id"]?.ToString() == "new")
				{
					performanceFieldExampleId = -1;
				}
				else
				{
					performanceFieldExampleId = Util.Nz<Int64>(this.RouteData.Values["id"]?.ToString(), -1);
				}

				return View("performanceFieldExampleEdit", performanceFieldExampleId);
			}

			return View("performanceFieldExample");
		}

		public IActionResult Courses()
		{
			return View();
		}

		[HttpGet]
		public IActionResult CoursesList()
		{
			var courses = _context.Courses.OrderByDescending(x=>x.UpdatedDate).ToList();

			return new JsonResult(courses);
		}

		public IActionResult AddNewCourse()
		{
			var model = new AddNewCourseDto
			{
				IsError = false, IsFromPost = false, ErrorDescription = "",
				Course = new Course()
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> AddNewCourse(AddNewCourseDto model)
		{
			model.IsFromPost = true;

			if (string.IsNullOrWhiteSpace(model.Course.Title))
			{
				model.IsError = true;
				model.ErrorDescription = "Title is empty";
				return View(model);
			}

			if (string.IsNullOrWhiteSpace(model.Course.Source))
			{
				model.IsError = true;
				model.ErrorDescription = "Source is empty";
				return View(model);
			}

			if (string.IsNullOrWhiteSpace(model.Course.CourseDetails))
			{
				model.IsError = true;
				model.ErrorDescription = "Course Details is empty";
				return View(model);
			}

			if (model.Course.Title.Length >= 350)
			{
				model.IsError = true;
				model.ErrorDescription = "Title is too long";
				return View(model);
			}

			if (model.Course.Source.Length >= 350)
			{
				model.IsError = true;
				model.ErrorDescription = "Source is too long";
				return View(model);
			}

			if (model.Course.SkillsCovered?.Length >= 350)
			{
				model.IsError = true;
				model.ErrorDescription = "Skills Covered is too long";
				return View(model);
			}

			var course = new Course
			{
				Title = model.Course.Title,
				SubjectArea = model.Course.SubjectArea,
				CourseType = model.Course.CourseType,
				RecommendedLevelForMastery = model.Course.RecommendedLevelForMastery,
				Source = model.Course.Source,
				CourseDetails = model.Course.CourseDetails,
				LearningObjectives = model.Course.LearningObjectives,
				SkillsCovered = model.Course.SkillsCovered,
				ContinuingEducationUnits = model.Course.ContinuingEducationUnits,
				DisplayOrder = model.Course.DisplayOrder,
				SubjectAreaText = model.Course.SubjectAreaText,
				IsActive = model.Course.IsActive
			};

			await _context.AddAsync(course);
			await _context.SaveChangesAsync();

			model.IsError = false;

			return View(model);
		}

		public IActionResult UpdateCourse(int courseId)
		{
			var course = _context.Courses.FirstOrDefault(x => x.Id == courseId);
			var model = new AddNewCourseDto { IsError = false, IsFromPost = false, ErrorDescription = "", Course = course };

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateCourse(AddNewCourseDto model)
		{
			model.IsFromPost = true;

			var course = _context.Courses.FirstOrDefault(x => x.Id == model.Course.Id);
			if (course == null)
			{
				model.IsError = true;
				model.ErrorDescription = "Course not found";
				return View(model);
			}

			if (string.IsNullOrWhiteSpace(model.Course.Title))
			{
				model.IsError = true;
				model.ErrorDescription = "Title is empty";
				return View(model);
			}

			if (string.IsNullOrWhiteSpace(model.Course.Source))
			{
				model.IsError = true;
				model.ErrorDescription = "Source is empty";
				return View(model);
			}

			if (string.IsNullOrWhiteSpace(model.Course.CourseDetails))
			{
				model.IsError = true;
				model.ErrorDescription = "Course Details is empty";
				return View(model);
			}

			if (model.Course.Title.Length >= 350)
			{
				model.IsError = true;
				model.ErrorDescription = "Title is too long";
				return View(model);
			}

			if (model.Course.Source.Length >= 350)
			{
				model.IsError = true;
				model.ErrorDescription = "Source is too long";
				return View(model);
			}

			if (model.Course.SkillsCovered?.Length >= 350)
			{
				model.IsError = true;
				model.ErrorDescription = "Skills Covered is too long";
				return View(model);
			}

			course.Title = model.Course.Title;
			course.SubjectArea = model.Course.SubjectArea;
			course.CourseType = model.Course.CourseType;
			course.RecommendedLevelForMastery = model.Course.RecommendedLevelForMastery;
			course.Source = model.Course.Source;
			course.CourseDetails = model.Course.CourseDetails;
			course.LearningObjectives = model.Course.LearningObjectives;
			course.SkillsCovered = model.Course.SkillsCovered;
			course.ContinuingEducationUnits = model.Course.ContinuingEducationUnits;
			course.DisplayOrder = model.Course.DisplayOrder;
			course.SubjectAreaText = model.Course.SubjectAreaText;
			course.IsActive = model.Course.IsActive;

			course.UpdatedDate = DateTime.UtcNow;

			await _context.SaveChangesAsync();

			model.IsError = false;

			return View(model);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteCourse(int courseId)
		{
			var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
			if (course == null)
			{
				return Json(new {success = false, message = "Course not found."});
			}

			_context.Courses.Remove(course);
			await _context.SaveChangesAsync();

			return Json(new {success = true, message = "Delete successful."});
		}
	}
}
