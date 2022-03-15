using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ATGHR.Code;
using ATGHR.Code.Enums;
using ATGHR.Models.Database;

namespace ATGHR.Controllers
{
	[Authorize]
	[PermissionsRequired(PermissionType = PermissionType.Authorized)]
	public class PerformanceController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[Route(template: "Performance/ReviewSheet/{id}")]
		[PermissionsRequired(PermissionType = PermissionType.Self)]
		[PermissionsRequired(PermissionType = PermissionType.Supervisor)]
		[PermissionsRequired(PermissionType = PermissionType.Lead)]
		[PermissionsRequired(PermissionType = PermissionType.Administrator)]
		[PermissionsRequired(PermissionType = PermissionType.Viewer)]
		public IActionResult ReviewSheet(Int64? id)
		{
			ViewData["PerformanceReviewSheetId"] = id;
			((UserAction)HttpContext.Items["UserAction"]).PerformanceReviewSheetId = id;
			return View();
		}

		[Route(template: "Performance/GoalSheet/{id}")]
		[PermissionsRequired(PermissionType = PermissionType.Self)]
		[PermissionsRequired(PermissionType = PermissionType.Supervisor)]
		[PermissionsRequired(PermissionType = PermissionType.Lead)]
		[PermissionsRequired(PermissionType = PermissionType.Administrator)]
		[PermissionsRequired(PermissionType = PermissionType.Viewer)]
		public IActionResult GoalSheet(Int64? id)
		{
			ViewData["PerformanceGoalSheetId"] = id;
			((UserAction)HttpContext.Items["UserAction"]).PerformanceGoalSheetId = id;
			return View();
		}

		[Route(template: "Performance/GoalSheetHistory/{id}")]
		[PermissionsRequired(PermissionType = PermissionType.Self)]
		[PermissionsRequired(PermissionType = PermissionType.Supervisor)]
		[PermissionsRequired(PermissionType = PermissionType.Lead)]
		[PermissionsRequired(PermissionType = PermissionType.Administrator)]
		[PermissionsRequired(PermissionType = PermissionType.Viewer)]
		public IActionResult GoalSheetHistory(Int64? id)
		{
			ViewData["PerformanceGoalSheetHistoryId"] = id;
			return View();
		}

		[Route(template: "/Performance/GoalCards")]
		public ActionResult GoalCards()
		{
			return View("GoalCards");
		}

		[Route(template: "/Performance/ReviewCards")]
		public ActionResult ReviewCards()
		{
			return View("ReviewCards");
		}

		[Route(template: "/Performance/Viewer")]
		public ActionResult SheetList()
		{
			return View("Viewer");
		}
	}
}

