using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Collections.Generic;
using System.Data.SqlClient;
using ATG.DBNet;

namespace ATGHR.Models.Database
{
	/// <summary>
	/// User action log entry.
	/// </summary>
	public class UserAction : CRUDObject
	{
		#region Properties
		/// <summary>
		/// Primary key.
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Int64? UserActionId { get; set; } = null;

		/// <summary>
		/// The user doing the action.
		/// </summary>
		public Int64? ActionUserId { get; set; } = null;

		/// <summary>
		/// Timestamp when the action was completed.
		/// </summary>
		public DateTime ActionDatetime { get; set; } = new DateTime();

		/// <summary>
		/// IP address for the client.
		/// </summary>
		public String IP { get; set; } = null;

		/// <summary>
		/// The user's reported browser.
		/// </summary>
		public String UserAgent { get; set; } = null;

		/// <summary>
		/// Request URL.
		/// </summary>
		public String URL { get; set; } = null;

		/// <summary>
		/// Controller for the request.
		/// </summary>
		public String Controller { get; set; } = null;

		/// <summary>
		/// Action for the request.
		/// </summary>
		public String Action { get; set; } = null;

		/// <summary>
		/// The affected user, if any.
		/// </summary>
		public Int64? UserId { get; set; } = null;

		/// <summary>
		/// The affected goal sheet, if any.
		/// </summary>
		public Int64? PerformanceGoalSheetId { get; set; } = null;

		/// <summary>
		/// The affected review sheet, if any.
		/// </summary>
		public Int64? PerformanceReviewSheetId { get; set; } = null;

		/// <summary>
		/// Whether the request encountered an error.
		/// </summary>
		public Boolean Error { get; set; } = false;

		/// <summary>
		/// Stack trace from the error, if any.
		/// </summary>
		public String ErrorTrace { get; set; } = null;
		#endregion

		#region Constructors
		public UserAction(
			FilterContext context,
			User user)
		{
			ActionUserId = user?.UserId;
			ActionDatetime = DateTime.Now;
			IP = context.HttpContext.Connection.RemoteIpAddress.ToString();
			UserAgent = context.HttpContext.Request.Headers["User-Agent"];
			URL = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.HttpContext.Request);
			Controller = ((ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
			Action = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.Name;
		}
		#endregion
	}
}
