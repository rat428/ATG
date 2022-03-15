using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Configuration;
using ATG;
using ATG.DBNet;
using ATGHR.Code.Enums;
using ATGHR.Models;
using ATGHR.Models.Database;
using System.Text.Json;

namespace ATGHR.Code
{
	public class RequestFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(
			ActionExecutingContext context)
		{
			// Get the user, if we've already established a session
			User user = new User();
			if (!String.IsNullOrEmpty(context.HttpContext.Session.GetString("User")))
			{
				Reflection.CopyProperties(JsonSerializer.Deserialize<UserLight>(context.HttpContext.Session.GetString("User")), user);
			}

			System.Reflection.MethodInfo method = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.MethodInfo;
			Type controller = context.Controller.GetType();

			// Get the current user's information
			if (user.UserId == null)
			{
				user.Username = context.HttpContext.User.Identity.Name;
				if (!String.IsNullOrWhiteSpace(user.Username))
				{
					using (var db = new DBConnector())
					{
						user.ReadByUsername(db: db);
						UserLight userLight = new UserLight();
						Reflection.CopyProperties(user, userLight);
						context.HttpContext.Session.SetString("User", JsonSerializer.Serialize(userLight));
					}
				}
			}

			Int64? userId = null;

			try
			{
				userId = Int64.Parse(context.HttpContext.Request.Form["userId"].FirstOrDefault());
			}
			catch
			{
				try
				{
					userId = Int64.Parse(context.HttpContext.Request.Query["userId"].FirstOrDefault());
				}
				catch
				{
					userId = null;
				}
			}

			// Check for permissions to the current page
			if (!PermissionsRequiredAttribute.PermissionMatch(controller, method, user, employeeUserId: userId))
			{
				context.Result = new UnauthorizedResult();
			}

			context.HttpContext.Items["UserAction"] = new UserAction(context, user);

			// Continue action
			base.OnActionExecuting(context);
		}

		public override void OnResultExecuted(
			ResultExecutedContext context)
		{
			// Get the user, if we've already established a session
			User user = new User();
			if (!String.IsNullOrEmpty(context.HttpContext.Session.GetString("User")))
			{
				Reflection.CopyProperties(JsonSerializer.Deserialize<UserLight>(context.HttpContext.Session.GetString("User")), user);
			}

			var userAction = ((UserAction)context.HttpContext.Items["UserAction"]) ?? new UserAction(context, user);

			if (context.Exception != null)
			{
				Logger.WriteError(context.Exception);
				userAction.Error = true;
				userAction.ErrorTrace = context.Exception.StackTrace;
			}

			userAction.Create();

			// Disconnect from the database if a connection was opened
			DB.Dispose();

			base.OnResultExecuted(context);
		}
	}
}
