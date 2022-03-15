using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ATG.DBNet;
using ATGHR.Models;
using ATGHR.Code;
using ATGHR.Code.Enums;
using ATGHR.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ATGHR.Models.Database
{
	public class IdListYear: CRUDObject
	{
		public IEnumerable<long?> UserId { get; set; } = null;

		public String Year { get; set; } = null;

		public Int64? DataUserId { get; set; } = null;

		#region Database Operations
		public ExecuteResult AssignUserGoalSheet()
		{
			DataTable table = DBNet.GetTable(UserId.Select(u => new { id = u }), StoredProcedureType.Create);

			List<SqlParameter> extraParameters = new List<SqlParameter>()
			{
				new SqlParameter("@UserIds", table),
				new SqlParameter("@Year", Year),
				new SqlParameter("@DataUserId", DataUserId),
				new SqlParameter("@DataDatetime", DateTime.Now)
			};
			String SQL = "SP_TotalPerformanceGoalSheet_Create_Multiple";
			ExecuteResult result = DBNet.ExecuteToResult(SQL, parameters: extraParameters);
			return result;
		}

		public ExecuteResult AssignUserReviewSheet()
		{
			DataTable table = DBNet.GetTable(UserId.Select(u => new { id = u }), StoredProcedureType.Create);

			List<SqlParameter> extraParameters = new List<SqlParameter>()
			{
				new SqlParameter("@UserIds", table),
				new SqlParameter("@Year", Year),
				new SqlParameter("@DataUserId", DataUserId),
				new SqlParameter("@DataDatetime", DateTime.Now)
			};
			String SQL = "SP_TotalPerformanceReviewSheet_Create_Multiple";
			ExecuteResult result = DBNet.ExecuteToResult(SQL, parameters: extraParameters);
			return result;
		}
		#endregion
	}
}
