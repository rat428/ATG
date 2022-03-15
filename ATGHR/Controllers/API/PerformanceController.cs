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
using Dapper;

namespace ATGHR.Controllers.API
{
	[ApiController]
	[Authorize]
	[PermissionsRequired(PermissionType = PermissionType.Authorized)]
	public class PerformanceController : Controller
	{
		/// <summary>
		/// Goal sheet list with only GoalGoalItemText for "GOAL" item for home page listing.
		/// </summary>
		/// <returns>Array of sheets.</returns>
		[HttpGet, Route("/api/Performance/HomePageGoals")]
		public JsonResult HomePageGoals()
		{
			IEnumerable<PerformanceGoalSheet> sheets = Enumerable.Empty<PerformanceGoalSheet>();

			DBNet.ReadMulti(
				ref sheets,
				"SP_MainPageGoalSheetList_Read",
				useProperties: false,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", Models.Database.User.GetSessionUser(HttpContext).UserId)
				});

			return Json(sheets);
		}

		/// <summary>
		/// Review sheet list with only sheet details and score.
		/// </summary>
		/// <returns>Array of sheets.</returns>
		[HttpGet, Route("/api/Performance/HomePageReviews")]
		public JsonResult HomePageReviews()
		{
			IEnumerable<PerformanceReviewSheet> sheets = Enumerable.Empty<PerformanceReviewSheet>();

			DBNet.ReadMulti(
				ref sheets,
				"SP_MainPageReviewSheetList_Read",
				useProperties: false,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", Models.Database.User.GetSessionUser(HttpContext).UserId)
				});

			return Json(sheets);
		}

		/// <summary>
		/// Goal sheet details.
		/// </summary>
		/// <param name="PerformanceReviewSheetId">The sheet ID.</param>
		/// <returns>Goal sheet with all contents.</returns>
		[HttpGet, Route("/api/Performance/GoalSheet/{PerformanceGoalSheetId}")]
		public JsonResult GoalSheet(
			Int64? PerformanceGoalSheetId = null)
		{
			PerformanceGoalSheet sheet = new PerformanceGoalSheet();
			User user = Util.GetUser(HttpContext);

			sheet.PerformanceGoalSheetId = PerformanceGoalSheetId;

			DBNet.Read(
				sheet,
				"SP_TotalPerformanceGoalSheet_Read",
				useProperties: true,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", user.UserId)
				});

			((UserAction)HttpContext.Items["UserAction"]).PerformanceGoalSheetId = PerformanceGoalSheetId;
			((UserAction)HttpContext.Items["UserAction"]).UserId = sheet.UserId;

			PermissionType permissionType = user.GetPermissionTypes(employeeUserId: sheet.UserId, performanceGoalSheetId: sheet.PerformanceGoalSheetId, divisionId: ATGHR.LookupTables?.Users?.FirstOrDefault(u => u.UserId == sheet.UserId)?.DivisionId);

			return Json(
				new
				{
					UserId = user.UserId,
					Self = user.UserId == sheet.UserId,
					Supervisor = (permissionType & PermissionType.Supervisor) == PermissionType.Supervisor,
					Lead = (permissionType & PermissionType.Lead) == PermissionType.Lead,
					Administrator = (permissionType & PermissionType.Administrator) == PermissionType.Administrator,
					Sheet = sheet
				});
		}

		/// <summary>
		/// Goal sheet history details.
		/// </summary>
		/// <param name="PerformanceReviewSheetHistoryId">The sheet history ID.</param>
		/// <returns>Goal sheet with all contents.</returns>
		[HttpGet, Route("/api/Performance/GoalSheetHistory/{PerformanceGoalSheetHistoryId}")]
		public JsonResult GoalSheetHistory(
			Int64? PerformanceGoalSheetHistoryId = null)
		{
			PerformanceGoalSheet sheet = new PerformanceGoalSheet();
			User user = Util.GetUser(HttpContext);

			sheet.PerformanceGoalSheetId = PerformanceGoalSheetHistoryId;

			DBNet.Read(
				sheet,
				"SP_TotalPerformanceGoalSheetHistory_Read",
				useProperties: true,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", user.UserId)
				});

			((UserAction)HttpContext.Items["UserAction"]).PerformanceGoalSheetId = sheet.PerformanceGoalSheetId;
			((UserAction)HttpContext.Items["UserAction"]).UserId = sheet.UserId;

			PermissionType permissionType = user.GetPermissionTypes(employeeUserId: sheet.UserId, performanceGoalSheetId: sheet.PerformanceGoalSheetId, divisionId: ATGHR.LookupTables?.Users?.FirstOrDefault(u => u.UserId == sheet.UserId)?.DivisionId);

			return Json(
				new
				{
					UserId = user.UserId,
					Self = user.UserId == sheet.UserId,
					Supervisor = (permissionType & PermissionType.Supervisor) == PermissionType.Supervisor,
					Lead = (permissionType & PermissionType.Lead) == PermissionType.Lead,
					Administrator = (permissionType & PermissionType.Administrator) == PermissionType.Administrator,
					Sheet = sheet
				});
		}

		/// <summary>
		/// Review sheet details.
		/// </summary>
		/// <param name="PerformanceReviewSheetId">The sheet ID.</param>
		/// <returns>Review sheet with all contents.</returns>
		[HttpGet, Route("/api/Performance/ReviewSheet/{PerformanceReviewSheetId}")]
		public JsonResult ReviewSheet(
			Int64? PerformanceReviewSheetId = null)
		{
			PerformanceReviewSheet sheet = new PerformanceReviewSheet();
			User user = Util.GetUser(HttpContext);

			sheet.PerformanceReviewSheetId = PerformanceReviewSheetId;

			DBNet.Read(
				sheet,
				"SP_TotalPerformanceReviewSheet_Read",
				useProperties: true,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", user.UserId)
				});

			((UserAction)HttpContext.Items["UserAction"]).PerformanceReviewSheetId = PerformanceReviewSheetId;
			((UserAction)HttpContext.Items["UserAction"]).UserId = sheet.UserId;

			PermissionType permissionType = user.GetPermissionTypes(employeeUserId: sheet.UserId, performanceReviewSheetId: sheet.PerformanceReviewSheetId, divisionId: ATGHR.LookupTables?.Users?.FirstOrDefault(u => u.UserId == sheet.UserId)?.DivisionId);

			return Json(
				new
				{
					UserId = user.UserId,
					Self = user.UserId == sheet.UserId,
					Supervisor = (permissionType & PermissionType.Supervisor) == PermissionType.Supervisor,
					Lead = (permissionType & PermissionType.Lead) == PermissionType.Lead,
					Administrator = (permissionType & PermissionType.Administrator) == PermissionType.Administrator,
					Sheet = sheet
				});

		}

		/// <summary>
		/// Goal sheet "cards" for all users the current user may view.
		/// </summary>
		/// <returns>Goal sheet "cards".</returns>
		[HttpGet, Route("/api/Performance/GoalSheetSupervisorCards")]
		public JsonResult GoalSheetSupervisorCards()
		{
			IEnumerable<PerformanceGoalSheetSupervisorCard> sheets = Enumerable.Empty<PerformanceGoalSheetSupervisorCard>();
			
			DBNet.ReadMulti(
				ref sheets,
				"SP_SupervisorGoalSheetCards_Read",
				useProperties: false,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", Models.Database.User.GetSessionUser(HttpContext).UserId)
				});

			return Json(sheets);
		}

		/// <summary>
		/// Review sheet "cards" for all users the current user may view.
		/// </summary>
		/// <returns>Review sheet "cards".</returns>
		[HttpGet, Route("/api/Performance/ReviewSheetSupervisorCards")]
		public JsonResult ReviewSheetSupervisorCards()
		{
			IEnumerable<PerformanceReviewSheetSupervisorCard> sheets = Enumerable.Empty<PerformanceReviewSheetSupervisorCard>();

			DBNet.ReadMulti(
				ref sheets,
				"SP_SupervisorReviewSheetCards_Read",
				useProperties: false,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", Models.Database.User.GetSessionUser(HttpContext).UserId)
				});

			return Json(sheets);
		}

		/// <summary>
		/// Create goal sheets for a set of users.
		/// </summary>
		/// <param name="UserIdList">Optional list of UserId values to limit the total number of sheets that is being created.</param>
		/// <returns>Success/failure response.</returns>
		[HttpPost, Route("api/Performance/CreateGoalSheets")]
		[PermissionsRequired(PermissionType = PermissionType.Administrator)]
		public JsonResult CreateGoalSheets(
			DateTime DueDate,
			Int64 Year,
			IEnumerable<Int64?> UserIdList = null)
		{
			return Json(DBNet.ExecuteToResult(
				SQL: "SP_TotalPerformanceGoalSheet_Create",
				parameters: new List<SqlParameter>()
				{
					new SqlParameter("DueDate", DueDate),
					new SqlParameter("Year", Year),
					new SqlParameter("UserIds", DBNet.GetTable(IdList.FromList(UserIdList), StoredProcedureType.UpdateMulti)),
					new SqlParameter("DataUserId", Models.Database.User.GetSessionUser(HttpContext).UserId),
					new SqlParameter("DataDatetime", DateTime.Now)
				},
				timeout: 30));
		}

		/// <summary>
		/// Create review sheets for a set of users.
		/// </summary>
		/// <param name="UserIdList">Optional list of UserId values to limit the total number of sheets that is being created.</param>
		/// <returns>Success/failure response.</returns>
		[HttpPost, Route("api/Performance/CreateReviewSheets")]
		[PermissionsRequired(PermissionType = PermissionType.Administrator)]
		public JsonResult CreateReviewSheets(
			DateTime DueDate,
			Int64 Year,
			IEnumerable<Int64?> UserIdList = null)
		{
			return Json(DBNet.ExecuteToResult(
				SQL: "SP_TotalPerformanceReviewSheet_Create",
				parameters: new List<SqlParameter>()
				{
					new SqlParameter("DueDate", DueDate),
					new SqlParameter("Year", Year),
					new SqlParameter("UserIds", DBNet.GetTable(IdList.FromList(UserIdList), StoredProcedureType.UpdateMulti)),
					new SqlParameter("DataUserId", Models.Database.User.GetSessionUser(HttpContext).UserId),
					new SqlParameter("DataDatetime", DateTime.Now)
				},
				timeout: 30));
		}

		/// <summary>
		/// Update a goal sheet and its contents.
		/// </summary>
		/// <param name="inSheet">Full json object containing the sheet's udpated data.</param>
		/// <returns>Updated sheet json.</returns>
		[HttpPost, Route("api/Performance/GoalSheetUpdate")]
		public JsonResult GoalSheetUpdate(
			[FromBody] PerformanceGoalSheet sheet)
		{
			((UserAction)HttpContext.Items["UserAction"]).PerformanceReviewSheetId = sheet.PerformanceGoalSheetId;
			((UserAction)HttpContext.Items["UserAction"]).UserId = sheet.UserId;

			sheet.DataUserId = Util.GetUser(HttpContext).UserId;
			sheet.DataDatetime = DateTime.Now;
			sheet.Update();

			return Json(sheet);
		}

		/// <summary>
		/// Update a review sheet and its contents.
		/// </summary>
		/// <param name="sheetForm">Full json object containing the sheet's udpated data.</param>
		/// <returns>Updated sheet json.</returns>
		[HttpPost, Route("api/Performance/ReviewSheetUpdate")]
		public JsonResult ReviewSheetUpdate(
			[FromBody] PerformanceReviewSheet sheet)
		{
			((UserAction)HttpContext.Items["UserAction"]).PerformanceReviewSheetId = sheet.PerformanceReviewSheetId;
			((UserAction)HttpContext.Items["UserAction"]).UserId = sheet.UserId;

			sheet.DataUserId = Util.GetUser(HttpContext).UserId;
			sheet.DataDatetime = DateTime.Now;
			sheet.Update();

			return Json(sheet);
		}

		/// <summary>
		/// Change one of the approval statuses of a sheet.
		/// </summary>
		/// <param name="approveForm">A JSON object reflecting a PerformanceGoalSheetApproval object.</param>
		/// <returns>Execution result object.</returns>
		[HttpPost, Route("api/Performance/GoalSheet/Approve")]
		public JsonResult GoalSheetApprove(
			[FromBody] PerformanceGoalSheetApproval approval)
		{
			PerformanceGoalSheet sheet = new PerformanceGoalSheet
			{
				PerformanceGoalSheetId = approval.PerformanceGoalSheetId,
				DataUserId = Util.GetUser(HttpContext).UserId,
				DataDatetime = DateTime.Now
			};

			((UserAction)HttpContext.Items["UserAction"]).PerformanceGoalSheetId = sheet.PerformanceGoalSheetId;

			return Json(sheet.Approve(approval.ApprovalType, approval.Approved, approval.UserId));
		}

		/// <summary>
		/// Change one of the approval statuses of a sheet.
		/// </summary>
		/// <param name="approveForm">A JSON object reflecting a PerformanceReviewSheetApproval object.</param>
		/// <returns>Execution result object.</returns>
		[HttpPost, Route("api/Performance/ReviewSheet/Approve")]
		public JsonResult ReviewSheetApprove(
			[FromBody] PerformanceReviewSheetApproval approval)
		{
			PerformanceReviewSheet sheet = new PerformanceReviewSheet
			{
				PerformanceReviewSheetId = approval.PerformanceReviewSheetId,
				DataUserId = Util.GetUser(HttpContext).UserId,
				DataDatetime = DateTime.Now
			};

			((UserAction)HttpContext.Items["UserAction"]).PerformanceReviewSheetId = sheet.PerformanceReviewSheetId;

			return Json(sheet.Approve(approval.ApprovalType, approval.Approved, approval.UserId));
		}

		/// <summary>
		/// Create or update a comment.
		/// </summary>
		/// <param name="text">A JSON object reflecting the comment's details.</param>
		/// <returns>Execution result object.</returns>
		[HttpPost, Route("api/Performance/GoalSheet/Comment")]
		public JsonResult GoalSheetCommentSave(
			[FromBody] PerformanceGoalSheetItemText text)
		{
			text.UserId = Util.GetUser(HttpContext).UserId;
			text.DataUserId = Util.GetUser(HttpContext).UserId;
			text.DataDatetime = DateTime.Now;

			return Json(text.CommentSave());
		}

		/// <summary>
		/// Delete a comment.
		/// </summary>
		/// <param name="text">A JSON object reflecting the comment's details.</param>
		/// <returns>Execution result object.</returns>
		[HttpPost, Route("api/Performance/GoalSheet/Comment/{PerformanceGoalSheetItemTextId}/Delete")]
		public JsonResult GoalSheetCommentDelete(
			Int64? PerformanceGoalSheetItemTextId = null)
		{
			return Json(PerformanceGoalSheetItemText.CommentDelete(PerformanceGoalSheetItemTextId, Util.GetUser(HttpContext).UserId));
		}

		/// <summary>
		/// Create or update a comment.
		/// </summary>
		/// <param name="text">A JSON object reflecting the comment's details.</param>
		/// <returns>Execution result object.</returns>
		[HttpPost, Route("api/Performance/ReviewSheet/Comment")]
		public JsonResult ReviewSheetCommentSave(
			[FromBody] PerformanceReviewSheetItemText text)
		{
			text.UserId = Util.GetUser(HttpContext).UserId;
			text.DataUserId = Util.GetUser(HttpContext).UserId;
			text.DataDatetime = DateTime.Now;

			return Json(text.CommentSave());
		}

		/// <summary>
		/// Delete a comment.
		/// </summary>
		/// <param name="text">A JSON object reflecting the comment's details.</param>
		/// <returns>Execution result object.</returns>
		[HttpPost, Route("api/Performance/ReviewSheet/Comment/{PerformanceReviewSheetItemTextId}/Delete")]
		public JsonResult ReviewSheetCommentDelete(
			Int64? PerformanceReviewSheetItemTextId = null)
		{
			return Json(PerformanceReviewSheetItemText.CommentDelete(PerformanceReviewSheetItemTextId, Util.GetUser(HttpContext).UserId));
		}

		/// <summary>
		/// Returns the list of all current sheets the user can interact with. 
		/// </summary>
		/// <param name="DataUserId">The id of the session user.</param>
		/// <returns>DBNet.ExecuteResult object</returns>
		[HttpPost, Route("/api/ViewerGoalSheetList")]
		public JsonResult ViewerGoalSheets()
		{
			return Json(DBNet.ReadAnonymous("SP_PerformanceGoalSheetViewerList_Read", parameters: new List<SqlParameter> { new SqlParameter("@DataUserId", Util.GetUser(HttpContext).UserId) }, noTransaction: true));
		}

		/// <summary>
		/// Returns the list of all current sheets the user can interact with. 
		/// </summary>
		/// <param name="DataUserId">The id of the session user.</param>
		/// <returns>DBNet.ExecuteResult object</returns>
		[HttpPost, Route("/api/ViewerReviewSheetList")]
		public JsonResult ViewerReviewSheets()
		{
			return Json(DBNet.ReadAnonymous("SP_PerformanceReviewSheetViewerList_Read", parameters: new List<SqlParameter> { new SqlParameter("@DataUserId", Util.GetUser(HttpContext).UserId) }, noTransaction: true));
		}

		/// <summary>
		/// Returns the list of all past sheets the user interacted with. 
		/// </summary>
		/// <param name="DataUserId">The id of the session user.</param>
		/// <returns>DBNet.ExecuteResult object</returns>
		[HttpPost, Route("/api/PastInteractableGoalSheetList")]
		public JsonResult PastInteractableGoalSheets()
		{
			return Json(DBNet.ReadAnonymous("SP_PerformanceGoalSheetApprovedList_Read", parameters: new List<SqlParameter> { new SqlParameter("@DataUserId", Util.GetUser(HttpContext).UserId) }, noTransaction: true));
		}

		/// <summary>
		/// Returns the list of all past sheets the user interacted with. 
		/// </summary>
		/// <param name="DataUserId">The id of the session user.</param>
		/// <returns>DBNet.ExecuteResult object</returns>
		[HttpPost, Route("/api/PastInteractableReviewSheetList")]
		public JsonResult PastInteractableReviewSheets()
		{
			return Json(DBNet.ReadAnonymous("SP_PerformanceReviewSheetApprovedList_Read", parameters: new List<SqlParameter> { new SqlParameter("@DataUserId", Util.GetUser(HttpContext).UserId) }, noTransaction: true));
		}

		/// <summary>
		/// Returns the list of performancefield exampls the user has availible. 
		/// </summary>
		/// <param name="DataUserId">The id of the session user.</param>
		/// <returns>DBNet.ExecuteResult object</returns>
		[HttpPost, Route("/api/PerformanceFieldExamples")]
		public JsonResult PerformanceFieldExamples( [FromBody] IdTextList idTextList)
		{
			return Json(DBNet.ReadAnonymous("SP_PerformanceFieldExampleSheet_Read", parameters: new List<SqlParameter> {new SqlParameter("@PerformanceFieldCategory", idTextList.Text), new SqlParameter("@DataUserId", Util.GetUser(HttpContext).UserId) }, noTransaction: true));
		}

		/// <summary>
		/// Returns the list of past goal sheets associated with a performance review. 
		/// </summary>
		/// <param name="DataUserId">The id of the session user.</param>
		/// <returns>DBNet.ExecuteResult object</returns>
		[HttpPost, Route("/api/PerformanceReviewSheetPastGoals")]
		public JsonResult PerformanceReviewSheetGoals([FromBody] IdTextList idTextList)
		{
			return Json(DBNet.ReadAnonymous("SP_PerformanceReviewSheetPastGoals_Read", parameters: new List<SqlParameter> { new SqlParameter("@Year", idTextList.Text), new SqlParameter("@DataUserId", Util.GetUser(HttpContext).UserId), new SqlParameter("@UserId", idTextList.Id) }, noTransaction: true));
		}

		///
		[HttpPost, Route("/api/Admin/Dapper/UserResults")]
		public JsonResult UserResults([FromBody] IdTextList idTextList)
		{
			using (var connection = new SqlConnection(HttpContext.GetAppSettings().ConnectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("@UserId", idTextList.Id);
				parameters.Add("@Year", idTextList.Text);

				var results = connection.QueryAsync<dynamic>(
					sql: "SP_PerformanceReviewSheetUserQuantitativeResults_Read",
					parameters,
					commandType: System.Data.CommandType.StoredProcedure
					).Result;

				return Json(results);
			}
		}
	}
}
