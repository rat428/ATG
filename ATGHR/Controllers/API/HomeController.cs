using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;
using Newtonsoft.Json;
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
	public class HomeController : Controller
	{
		/// <summary>
		/// In/Out board current values
		/// </summary>
		/// <returns>Array of in/out statuses.</returns>
		[HttpGet, Route("/api/Home/InOutBoard")]
		public JsonResult InOutBoard()
		{
			IEnumerable<InOut> statuses = Enumerable.Empty<InOut>();

			DBNet.ReadMulti(
				ref statuses,
				"SP_InOutBoard_Read",
				useProperties: false,
				extraParameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", Models.Database.User.GetSessionUser(HttpContext).UserId)
				});

			return Json(statuses);
		}

		/// <summary>
		/// In/Out board update
		/// </summary>
		/// 
		[HttpPost, Route("/api/Home/InOutBoard")]
		public JsonResult InOutBoard_Update(
			[FromBody] InOut row
		)
		{
			return Json(DBNet.ExecuteToResult(
				"SP_InOutBoard_Update",
				parameters: new List<SqlParameter>
				{
					new SqlParameter("@DataUserId", Models.Database.User.GetSessionUser(HttpContext).UserId),
					new SqlParameter("@UserId", row.UserId),
					new SqlParameter("@WFHFlag", row.WFHFlag),
					new SqlParameter("@InOutFlag", row.InOutFlag),
					new SqlParameter("@InOutNote", row.InOutNote)
				}));
		}

		/// <summary>
		/// Get current month anniversaries
		/// </summary>
		[HttpGet, Route("/api/Home/Anniversaries")]
		public JsonResult Anniversaries()
		{
			IEnumerable<Anniversary> anniversaries = Enumerable.Empty<Anniversary>();

			DBNet.ReadMulti(
				ref anniversaries,
				"SP_Anniversaries_Read",
				useProperties: false
			);

			return Json(anniversaries);
		}

		/// <summary>
		/// Get current month birthdays
		/// </summary>
		[HttpGet, Route("/api/Home/Birthdays")]
		public JsonResult Birthdays()
		{
			IEnumerable<Birthday> birthdays = Enumerable.Empty<Birthday>();

			DBNet.ReadMulti(
				ref birthdays,
				"SP_Birthdays_Read",
				useProperties: false
			);

			return Json(birthdays);
		}
	}
}
