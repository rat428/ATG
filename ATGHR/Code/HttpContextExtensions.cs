using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ATGHR.Models;

namespace ATGHR.Code
{
	public static class HttpContextExtensions
	{
		public static AppSettings GetAppSettings(this HttpContext context)
			=> context.RequestServices.GetService<AppSettings>();

		public static LookupTables GetLookupTables(this HttpContext context)
			=> context.RequestServices.GetService<LookupTables>();
	}
}
