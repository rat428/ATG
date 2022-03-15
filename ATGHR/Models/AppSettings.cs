using System;
using Microsoft.Extensions.Configuration;
using ATG.DBNet;

namespace ATGHR.Models
{
	/// <summary>
	/// All globally-available application settings for ATGHR.
	/// </summary>
	public class AppSettings
	{
		/// <summary>
		/// Main database connection string, should point to ATGHR.
		/// </summary>
		internal String ConnectionString { get; set; }

		/// <summary>
		/// Whether the application is running in a development environment.
		/// </summary>
		public Boolean IsDev { get; set; }

		public AppSettings(
			IConfiguration configuration,
			String connectionString)
		{
			this.ConnectionString = configuration.GetConnectionString(connectionString).ToString();
			this.IsDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

			DB.ConnectionString = this.ConnectionString;
		}
	}
}
