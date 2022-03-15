using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;
using ATG.DBNet;
using ATGHR.Models;
using ATGHR.Code;
using ATGHR.Code.Enums;
using ATGHR.Models.Database;
using System.IO;

namespace ATGHR.Controllers.API
{
	[ApiController]
	[Authorize]
	[PermissionsRequired(PermissionType = PermissionType.Authorized)]
	public class RewardsController : Controller
	{
		/// <summary>
		/// Bonus infromation for "Home Page" Bonuses (ie. bonus the user owns)
		/// </summary>
		/// <returns>Array of Bonuses.</returns>
		[HttpGet, Route("/api/Bonus/HomePageBonuses")]
		public JsonResult HomePageBonuses()
		{
			IEnumerable<SpotBonusLight> bonuses = Enumerable.Empty<SpotBonusLight>();

			DBNet.ReadMulti(
				ref bonuses,
				"SP_SpotBonusCards_Read",
				useProperties: false,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", Models.Database.User.GetSessionUser(HttpContext).UserId)
				});

			return Json(bonuses);
		}

		/// <summary>
		/// Bonus infromation for "Home Page" Bonuses (ie. bonus the user owns)
		/// </summary>
		/// <returns>Array of Bonuses.</returns>
		[HttpGet, Route("/api/Bonus/ManagePageBonuses")]
		public JsonResult ManagePageBonuses()
		{
			IEnumerable<SpotBonusLight> bonuses = Enumerable.Empty<SpotBonusLight>();

			DBNet.ReadMulti(
				ref bonuses,
				"SP_SpotBonusSupervisorCards_Read",
				useProperties: false,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", Models.Database.User.GetSessionUser(HttpContext).UserId)
				});

			return Json(bonuses);
		}

		/// <summary>
		/// Get all approval level bonuses for bonus complete page.
		/// </summary>
		/// <returns>Array of all bonuses in the database that are ready to be marked complete.</returns>
		[HttpGet, Route("/api/Bonus/Complete")]
		public JsonResult BonusCompletion()
		{
			IEnumerable<SpotBonusLight> bonuses = Enumerable.Empty<SpotBonusLight>();

			DBNet.ReadMulti(
				ref bonuses,
				"SP_SpotBonusCompleter_Read",
				useProperties: false,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", Util.GetUser(HttpContext).UserId)
				});

			return Json(bonuses);
		}

		/// <summary>
		/// Get all complete level bonuses for bonsu complete page.
		/// </summary>
		/// <returns>Array of all bonuses in the database that are ready to be marked complete.</returns>
		[HttpGet, Route("/api/Bonus/Completed")]
		public JsonResult BonusCompleted()
		{
			IEnumerable<SpotBonusLight> bonuses = Enumerable.Empty<SpotBonusLight>();

			DBNet.ReadMulti(
				ref bonuses,
				"SP_SpotBonusCompleted_Read",
				useProperties: false,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", Util.GetUser(HttpContext).UserId)
				});

			return Json(bonuses);
		}

		/// <summary>
		/// bonus details.
		/// </summary>
		/// <param name="SpotBonusId">The bonus ID.</param>
		/// <returns>bonus with all contents.</returns>
		[HttpGet, Route("/api/Bonus/{SpotBonusId}")]
		public JsonResult SpotBonus(
			Int64? SpotBonusId = null)
		{
			SpotBonusLight bonus = new SpotBonusLight();
			User user = Util.GetUser(HttpContext);

			PermissionType permissionType = user.GetPermissionTypes(employeeUserId: bonus.UserId);

			bonus.SpotBonusId = SpotBonusId;

			DBNet.Read(
				bonus,
				"SP_SpotBonus_Read",
				useProperties: true,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", user.UserId)
				});

			return Json(
				new
				{
					UserId = user.UserId,
					Self = user.UserId == bonus.UserId,
					Supervisor = (permissionType & PermissionType.Supervisor) == PermissionType.Supervisor,
					Lead = (permissionType & PermissionType.Lead) == PermissionType.Lead,
					Administrator = (permissionType & PermissionType.Administrator) == PermissionType.Administrator,
					SpotBonusRequestor = (permissionType & PermissionType.SpotBonusRequestor) == PermissionType.SpotBonusRequestor,
					SpotBonusApprover = (permissionType & PermissionType.SpotBonusApprover) == PermissionType.SpotBonusApprover,
					SpotBonusCompleter = (permissionType & PermissionType.SpotBonusCompleter) == PermissionType.SpotBonusCompleter,
					Bonus = bonus
				});
		}

		/// <summary>
		/// Create or update a Spot Bonus' details.
		/// </summary>
		/// <param name="userForm">Json serialized spot bonus object.</param>
		/// <returns>DBNet.ExecuteResult object.</returns>
		[HttpPost, Route("/api/Rewards/Bonus/Save")]
		public JsonResult BonusSave(
			[FromBody] SpotBonus bonus)
		{
			bonus.DataUserId = Util.GetUser(HttpContext).UserId;
			bonus.DataDatetime = DateTime.Now;
			bonus.UpdateOrCreate();
			Models.Database.User.RefreshSessionUser(HttpContext);
			return Json(bonus);
		}
 
		/// <summary>
		/// Delete a spot bonus.
		/// </summary>
		/// <param name="userForm">Json serialized spot bonus object.</param>
		/// <returns>DBNet.ExecuteResult object.</returns>
		[HttpPost, Route("/api/Rewards/Bonus/Delete")]
		public JsonResult BonusDelete(
			[FromBody] SpotBonus bonus)
		{
			bonus.DataUserId = Util.GetUser(HttpContext).UserId;
			bonus.DataDatetime = DateTime.Now;
			bonus.Delete(new List<SqlParameter>() {
				new SqlParameter("@DataUserId", bonus.DataUserId),
				new SqlParameter("@DataDateTime", bonus.DataDatetime)
			});
			return Json(bonus);
		}


		/// <summary>
		/// Change one of the approval statuses of a award.
		/// </summary>
		/// <param name="approveForm">A JSON object reflecting a SpotBonusApproval object.</param>
		/// <returns>Execution result object.</returns>
		[HttpPost, Route("/api/Rewards/Bonus/Approve")]
		public JsonResult SpotBonusApproval(
			[FromBody] SpotBonusApproval approval)
		{
			SpotBonus bonus = new SpotBonus
			{
				SpotBonusId = approval.SpotBonusId,
				DataUserId = Util.GetUser(HttpContext).UserId,
				DataDatetime = DateTime.Now
			};

			return Json(bonus.Approve(approval.ApprovalType, approval.Approved));
		}

		/// <summary>
		/// Change one of the approval statuses of a award.
		/// </summary>
		/// <param name="approveForm">A JSON object reflecting a SpotBonusApproval object.</param>
		/// <returns>Execution result object.</returns>
		[HttpPost, Route("/api/Rewards/Bonus/BulkComplete")]
		public JsonResult SpotBonusBulkApproval(
			[FromBody] SpotBonusBulkApproval approval)
		{
		SpotBonusBulk bonus = new SpotBonusBulk
		{
			SpotBonusIds = approval.SpotBonusIds,
			DataUserId = Util.GetUser(HttpContext).UserId,
			DataDatetime = DateTime.Now
		};

		return Json(bonus.ApproveBulk(approval.ApprovalType, approval.Approved));
		}


		[HttpPost, Route("/api/Rewards/Bonus/ActiveAllStarAwardsReport")]
		public ActionResult Download()
		{
			var data = DBNet.ReadAnonymous("SP_SpotBonusCompleterReport_Read", parameters: new List<SqlParameter> { new SqlParameter("@DataUserId", Util.GetUser(HttpContext).UserId) }, noTransaction: true);
			MemoryStream m = new MemoryStream();
			TextWriter tw = new StreamWriter(m);
			foreach (var record in Util.ToCsv(data))
			{
				tw.WriteLine(record);
			}
			tw.Flush();
			m.Position = 0;
			return new FileStreamResult(m, "text/csv")
			{
				FileDownloadName = "Report.csv"
			};
		}

	}
}


