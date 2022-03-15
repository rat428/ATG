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
	/// Work anniversary information for users
	/// </summary>
	public class Anniversary : CRUDObject
	{

		/// <summary>
		/// Employee name.
		/// </summary>
		public String EmployeeName
		{
			get; set;
		} = null;

		/// <summary>
		/// Years of service.
		/// </summary>
		public Int32? Years
		{
			get; set;
		} = null;
	}
}


