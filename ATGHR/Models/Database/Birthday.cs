using System;
using System.ComponentModel.DataAnnotations;
using ATG.DBNet;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using ATGHR.Code;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data;

namespace ATGHR.Models.Database
{
	/// <summary>
	/// Employee birthday information for users
	/// </summary>
	public class Birthday : CRUDObject
	{

		/// <summary>
		/// Employee name.
		/// </summary>
		public String EmployeeName
		{
			get; set;
		} = null;

		/// <summary>
		/// Employee birthday month.
		/// </summary>
		public Int32 Month
		{
			get; set;
		}

		/// <summary>
		/// Employee birthday day.
		/// </summary>
		public Int32 Day
		{
			get; set;
		}
	}
}


