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

namespace ATGHR.Controllers.API
{
	[ApiController]
	[Authorize]
	[PermissionsRequired(PermissionType = PermissionType.Authorized)]
	public class LookupController : Controller
	{
		[HttpGet, Route("/api/Lookup/UpdateTimestamps")]
		public JsonResult Updates()
		{
			return Json(ATGHR.LookupTables?.UpdateTimestamps());
		}

		[HttpGet, Route("/api/Lookup/AllLookups")]
		public JsonResult AllLookups()
		{
			return Json(ATGHR.LookupTables);
		}

		[HttpGet, Route("/api/Lookup/Roles")]
		public JsonResult Roles()
		{
			return Json(ATGHR.LookupTables?.Roles);
		}

		[HttpGet, Route("/api/Lookup/Divisions")]
		public JsonResult Divisions()
		{
			return Json(ATGHR.LookupTables?.Divisions);
		}

		[HttpGet, Route("/api/Lookup/ATGOffices")]
		public JsonResult ATGOffices()
		{
			return Json(ATGHR.LookupTables?.ATGOffices);
		}

		[HttpGet, Route("/api/Lookup/UserRelationshipTypes")]
		public JsonResult UserRelationshipTypes()
		{
			return Json(ATGHR.LookupTables?.UserRelationshipTypes);
		}

		[HttpGet, Route("/api/Lookup/Permissions")]
		public JsonResult Permissions()
		{
			return Json(ATGHR.LookupTables?.Permissions);
		}

		[HttpGet, Route("/api/Lookup/PerformanceTextTypes")]
		public JsonResult PerformanceTextTypes()
		{
			return Json(ATGHR.LookupTables?.PerformanceTextTypes);
		}

		[HttpGet, Route("/api/Lookup/Users")]
		public JsonResult Users()
		{
			return Json(ATGHR.LookupTables?.Users);
		}

		[HttpGet, Route("/api/Lookup/PerformanceScores")]
		public JsonResult PerformanceScores()
		{
			return Json(ATGHR.LookupTables?.PerformanceScores);
		}

		[HttpGet, Route("/api/Lookup/PerformanceFields")]
		public JsonResult PerformanceFields()
		{
			return Json(ATGHR.LookupTables?.PerformanceFields);
		}

		[HttpGet, Route("/api/Lookup/DivisionPerformanceFieldDescriptions")]
		public JsonResult DivisionPerformanceFieldDescriptions()
		{
			return Json(ATGHR.LookupTables?.DivisionPerformanceFieldDescriptions);
		}

		[HttpGet, Route("/api/Lookup/ReloadLookups")]
		[PermissionsRequired(PermissionType = PermissionType.Administrator)]
		public JsonResult ReloadLookups()
		{
			ATGHR.LookupTables.Reload();

			return Json(ATGHR.LookupTables);
		}

		[HttpGet, Route("/api/WhoAmI")]
		[PermissionsRequired(PermissionType = PermissionType.Authorized)]
		public JsonResult WhoAmI()
		{
			return Json(DBNet.ReadAnonymous(String.Format("SELECT TOP(1) U.* FROM [User] AS U WHERE U.Username = '{0}'", Util.GetUser(HttpContext).Username), false, noTransaction: true));
		}
	}
}
