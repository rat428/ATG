using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ATGHR.Code;
using ATGHR.Code.Enums;
using ATGHR.Models;
using ATGHR.Models.Database;
using ATG.DBNet;
using System.Data.SqlClient;

namespace ATGHR.Controllers
{
	[Authorize]
	[PermissionsRequired(PermissionType = PermissionType.Authorized)]
	public class RewardsController : Controller
	{

		[Route(template: "Rewards/Bonus/{id?}")]
		[PermissionsRequired(PermissionType = PermissionType.SpotBonusRequestor)]
		public IActionResult Bonus()
		{
			Int64? id;
			if (this.RouteData.Values["id"] != null)
			{
				if (this.RouteData.Values["id"]?.ToString() == "new")
				{
					id = -1;
				}
				else
				{
					id = Util.Nz<Int64>(this.RouteData.Values["id"]?.ToString(), -1);
				}
				return View("Edit", id);
			}
			return View("Bonus");
		}

		[Route(template: "/Rewards/Manage")]
		[PermissionsRequired(PermissionType = PermissionType.SpotBonusApprover)]
		public ActionResult BonusSupervisorCard()
		{
			return View("Manage");
		}

		[Route(template: "/Rewards/Complete")]
		[PermissionsRequired(PermissionType = PermissionType.SpotBonusApprover)]
		public ActionResult BonusCompletion()
		{
			return View("Complete");
		}

	}
}

