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
using System.Reflection;
using System.IO;
using ATGHR.Models.Database.Performance;

namespace ATGHR.Controllers.API
{
	[ApiController]
	[Authorize]
	[PermissionsRequired(PermissionType = PermissionType.Administrator)]
	public class AdminController : Controller
	{
		/// <summary>
		/// Get all users for admin control page.
		/// </summary>
		/// <returns>Array of all users in the database.</returns>
		[HttpGet, Route("/api/Admin/Users")]
		public JsonResult Users()
		{
			IEnumerable<UserLight> users = Enumerable.Empty<UserLight>();

			DBNet.ReadMulti(
				ref users,
				"SP_AdminUsers_Read",
				useProperties: false,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", Util.GetUser(HttpContext).UserId)
				});

			return Json(users);
		}

		/// <summary>
		/// Get a user's details for editing.
		/// </summary>
		/// <param name="userId">The requested user's ID.</param>
		/// <returns>The requested user, if any.</returns>
		[HttpGet, Route("/api/Admin/User/{userId}")]
		public JsonResult UserRead(
			Int64? userId)
		{
			UserLight user = new UserLight();

			user.UserId = userId;

			((UserAction)HttpContext.Items["UserAction"]).UserId = userId;

			user.ReadFull(Util.GetUser(HttpContext).UserId);

			return Json(user);
		}

		/// <summary>
		/// Get a user's goals.
		/// </summary>
		/// <param name="userId">The requested user's ID.</param>
		/// <returns>The requested user's goals, if any.</returns>
		[HttpGet, Route("/api/Admin/User/Goals/{userId}")]
		public JsonResult UserGoalsRead(
			Int64? userId)
		{
			IEnumerable<PerformanceGoalSheet> sheets = Enumerable.Empty<PerformanceGoalSheet>();

			DBNet.ReadMulti(
				ref sheets,
				"SP_UserGoalSheetList_Read",
				useProperties: false,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@UserId", userId),
					new SqlParameter("@DataUserId", Models.Database.User.GetSessionUser(HttpContext).UserId)
				});

			((UserAction)HttpContext.Items["UserAction"]).UserId = userId;

			return Json(sheets);
		}

		/// <summary>
		/// Get a user's reviews.
		/// </summary>
		/// <param name="userId">The requested user's ID.</param>
		/// <returns>The requested user's goals, if any.</returns>
		[HttpGet, Route("/api/Admin/User/Reviews/{userId}")]
		public JsonResult UserReviewsRead(
			Int64? userId)
		{
			IEnumerable<PerformanceReviewSheet> sheets = Enumerable.Empty<PerformanceReviewSheet>();

			DBNet.ReadMulti(
				ref sheets,
				"SP_UserReviewSheetList_Read",
				useProperties: false,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@UserId", userId),
					new SqlParameter("@DataUserId", Models.Database.User.GetSessionUser(HttpContext).UserId)
				});

			((UserAction)HttpContext.Items["UserAction"]).UserId = userId;

			return Json(sheets);
		}

		/// <summary>
		/// Create or update a user's details.
		/// </summary>
		/// <param name="userForm">Json serialized user object.</param>
		/// <returns>DBNet.ExecuteResult object.</returns>
		[HttpPost, Route("/api/Admin/User/Save")]
		public JsonResult UserSave(
			[FromBody] User user)
		{
			((UserAction)HttpContext.Items["UserAction"]).UserId = user.UserId;

			user.DataUserId = Util.GetUser(HttpContext).UserId;
			user.DataDatetime = DateTime.Now;
			user.UpdateOrCreate();
			ATGHR.LookupTables.Reload();
			return Json(user);
		}

		/// <summary>
		/// Assign Goal Sheets to new users.
		/// </summary>
		/// <param name="assign_form">Json serialized list of Ids and a year.</param>
		/// <returns>DBNet.ExecuteResult object.</returns>
		[HttpPost, Route("/api/Admin/User/CreateGoalSheets")]
		[PermissionsRequired(PermissionType = PermissionType.Administrator)]
		public JsonResult AssignUserGoalSheet(
			[FromBody] IdListYear idListYear)
		{
			idListYear.DataUserId = Util.GetUser(HttpContext).UserId;
			return Json(idListYear.AssignUserGoalSheet());
		}

		/// <summary>
		/// Assign review Sheets to new users.
		/// </summary>
		/// <param name="assign_form">Json serialized list of Ids and a year.</param>
		/// <returns>DBNet.ExecuteResult object.</returns>
		[HttpPost, Route("/api/Admin/User/CreateReviewSheets")]
		[PermissionsRequired(PermissionType = PermissionType.Administrator)]
		public JsonResult AssignUserReviewSheet(
			[FromBody] IdListYear idListYear)
		{
			idListYear.DataUserId = Util.GetUser(HttpContext).UserId;
			return Json(idListYear.AssignUserReviewSheet());
		}

		/// <summary>
		/// Returns the list of all past sheets the user interacted with. 
		/// </summary>
		/// <param name="DataUserId">The id of the session user.</param>
		/// <returns>DBNet.ExecuteResult object</returns>
		[HttpPost, Route("/api/Admin/AcitveSheetList")]
		public JsonResult ActiveSheets()
		{
			return Json(DBNet.ReadAnonymous("SP_AdminActiveSheets_Read", parameters: new List<SqlParameter> { new SqlParameter("@DataUserId", Util.GetUser(HttpContext).UserId) }, noTransaction: true));
		}
		/// <summary>
		/// Returns the list of all past sheets the user interacted with. 
		/// </summary>
		/// <param name="DataUserId">The id of the session user.</param>
		/// <returns>DBNet.ExecuteResult object</returns>
		[HttpPost, Route("/api/Admin/CompletedSheetList")]
		public JsonResult CompletedSheets()
		{
			var data = DBNet.ReadAnonymous("SP_AdminCompleteSheets_Read", parameters: new List<SqlParameter> { new SqlParameter("@DataUserId", Util.GetUser(HttpContext).UserId) }, noTransaction: true);
			return Json(data);
		}

		/// <summary>
		/// report api 
		/// </summary>
		/// <returns></returns>
		[HttpPost, Route("/api/Admin/Reports")]
		public ActionResult DownloadReport(
			[FromBody] IdTextList textList)
		{
			string sql = "";
			string reportName = "Report.csv";
			switch (textList.Text)
			{
				case "UserSupervisionReport":
					sql = "SP_AdminUserSupervisionReport_Read";
					reportName = "UserSupervisionReport.csv";
					break;
				case "ActiveGoalsReport":
					sql = "SP_AdminActiveGoalSheetsReport_Read";
					reportName = "ActiveGoalsReport.csv";
					break;
				case "UserRoleReport":
					sql = "SP_AdminUserRoleReport_Read";
					reportName = "UserRoleReport.csv";
					break;
				case "ActivePerformanceReviewReport":
					sql = "SP_AdminActivePerformanceReviewReport_Read";
					reportName = "ActivePerformanceReviewReport.csv";
					break;
				case "PerformanceReviewReportScores":
					sql = "SP_AdminPerformanceReviewReportScores_Read";
					reportName = "PerformanceReviewScoresReport.csv";
					break;
				case "UserDetailsReport":
					sql = "SP_AdminUserDetailsReport_Read";
					reportName = "UserDetailsReport.csv";
					break;
			}
			var data = DBNet.ReadAnonymous(sql, parameters: new List<SqlParameter> { new SqlParameter("@DataUserId", Util.GetUser(HttpContext).UserId) }, noTransaction: true);
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
				FileDownloadName = reportName
			};
		}

		[HttpPost, Route("/api/Admin/Remind")]
		public ActionResult Remind(
			[FromBody] IEnumerable<IdTextList> recipientList)
		{
			var notifees = Enumerable.Empty<EMail.Notifee>();
			var recipeintListPartial = recipientList.Select(t => new IdList() { Id = t.Id });

			var recipientTable = DBNet.GetTable(recipeintListPartial, StoredProcedureType.Create);
			var temp = DBNet.ReadAnonymous(
				"SP_PerformanceSheetReminder_Read",
				parameters: new List<SqlParameter>
				{
					new SqlParameter("@Recipients", recipientTable),
					new SqlParameter("@DataUserId", Util.GetUser(HttpContext).UserId),
					new SqlParameter("@DataDatetime", DateTime.Now)
				},
				noTransaction: true);
			if (temp.Any())
			{
				notifees = temp.GroupBy(
					n => n.UserId,
					(key, group) => new EMail.Notifee()
					{
						Username = ATGHR.LookupTables.Users.FirstOrDefault(u => u.UserId == (Int64?)key).Username,
						BodyInfos = group.Select(n => new EMail.BodyInfo()
						{
							Relation = n.Relation,
							SheetType = n.SheetType,
							SheetId = (Int64?)n.SheetId,
							OwnerName = ATGHR.LookupTables.Users.FirstOrDefault(u => u.UserId == (Int64?)n.OwnerId).Name,
							CustomMessage = recipientList.FirstOrDefault(u => u.Id == key).Text
						})
					});
			}
			foreach (EMail.Notifee r in notifees)
			{
				EMail.SendSheetReminder(r.Username, r.BodyInfos);
			}
			return Json(notifees);
		}

		/// <summary>
		/// Get all example for admin control page.
		/// </summary>
		/// <returns>Array of all users in the database.</returns>
		[HttpGet, Route("/api/Admin/PerformanceFieldExamples")]
		public JsonResult PerformanceFieldExample()
		{
			IEnumerable<PerformanceFieldExample> examples = Enumerable.Empty<PerformanceFieldExample>();

			DBNet.ReadMulti(
				ref examples,
				"SP_PerformanceFieldExamples_Read",
				useProperties: false,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", Util.GetUser(HttpContext).UserId)
				});

			return Json(examples);
		}
		/// <summary>
		/// Create or update a user's details.
		/// </summary>
		/// <param name="userForm">Json serialized user object.</param>
		/// <returns>DBNet.ExecuteResult object.</returns>
		[HttpPost, Route("/api/Admin/PerformanceFieldExample/Save")]
		public JsonResult PerformanceFieldExampleSave(
			[FromBody] PerformanceFieldExample example)
		{
			example.DataUserId = Util.GetUser(HttpContext).UserId;
			example.DataDatetime = DateTime.Now;
			example.UpdateOrCreate();
			return Json(example);
		}

		/// <summary>
		/// Get a performance field example for editing.
		/// </summary>
		/// <param name="performanceFieldExampleId">The performance Field Example's ID.</param>
		/// <returns>performance Field Example with all contents.</returns>
		[HttpGet, Route("/api/Admin/PerformanceFieldExample/{PerformanceFieldExampleId}")]
		public JsonResult ExampleRead(
			Int64? performanceFieldExampleId = null)
		{
			PerformanceFieldExample example = new PerformanceFieldExample();
			User user = Util.GetUser(HttpContext);

			example.PerformanceFieldExampleId = performanceFieldExampleId;

			DBNet.Read(
				example,
				"SP_TotalPerformanceFieldExample_Read",
				useProperties: true,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", user.UserId)
				});

			return Json(
				new
				{
					PerformanceFieldExample = example
				});
		}
	}
}

