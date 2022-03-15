using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using ATGHR.Code;
using ATGHR.Models;
using ATG.DBNet;
using ATGHR.Models.EntityFramework.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

namespace ATGHR
{
	public class ATGHR
	{
		public ATGHR(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public static IConfiguration Configuration { get; set; }

		public static AppSettings AppSettings { get; set; }

		public static LookupTables LookupTables { get; set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			string[] initialScopes = Configuration.GetValue<string>("DownstreamApi:Scopes")?.Split(' ');

			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
				.AddMicrosoftIdentityWebApp(options => Configuration.Bind("AzureAD", options))
				.EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
				.AddMicrosoftGraph(Configuration.GetSection("DownstreamApi"))
				.AddInMemoryTokenCaches();

			services.AddMvc(options =>
				{
					var policy = new AuthorizationPolicyBuilder()
						.RequireAuthenticatedUser()
						.Build();
					options.Filters.Add(new AuthorizeFilter(policy));
					options.EnableEndpointRouting = false;
				})
				.SetCompatibilityVersion(CompatibilityVersion.Latest)
				.AddJsonOptions(
					options =>
					{
						options.JsonSerializerOptions.IgnoreNullValues = true;
						options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
					})
				.AddRazorRuntimeCompilation();

			services.AddMvc(options =>
			{
				options.Filters.Add<RequestFilter>();
			});

			services.AddDistributedMemoryCache();
			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(60);
				options.Cookie.HttpOnly = true;
			});

			services.AddDbContext<ApplicationDbContext>(
				options => options.UseSqlServer("name=ConnectionStrings:ATGHR"));

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			AppSettings = new AppSettings(Configuration, "ATGHR");

			using (var db = new DBConnector(AppSettings.ConnectionString))
			{
				LookupTables = new LookupTables(db, Configuration, "ATGHR");
			}

			services.AddSingleton(AppSettings);
			services.AddSingleton(LookupTables);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseAuthentication();

			app.UseSession();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
				routes.MapRoute(
					name: "api",
					template: "api/{controller=Performance}/{action=Index}/{id?}");
				routes.MapRoute(
					name: "Home",
					template: "{action=Index}/{id?}",
					defaults: new { controller = "Home" });
			});
		}
	}
}
