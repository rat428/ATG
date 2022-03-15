using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ATGHR.Models.Dtos;
using ATGHR.Models.EntityFramework.DbContext;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;

namespace ATGHR.Controllers
{
	public class AtguController : Controller
	{
		private readonly ILogger<AtguController> _logger;

		private readonly GraphServiceClient _graphServiceClient;

		// private readonly MicrosoftIdentityConsentAndConditionalAccessHandler _consentHandler;

		private string[] _graphScopes = new[] { "user.read" };
		private readonly ApplicationDbContext _context;

		public AtguController(ApplicationDbContext context, ILogger<AtguController> logger, GraphServiceClient graphServiceClient,
			// MicrosoftIdentityConsentAndConditionalAccessHandler consentHandler,
			IConfiguration configuration)
		{
			_context = context;
			_logger = logger;
			_graphServiceClient = graphServiceClient;
			// _consentHandler = consentHandler;
			_graphScopes = configuration.GetValue<string>("DownstreamApi:Scopes")?.Split(' ');
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Curriculum()
		{
			return View();
		}

		public IActionResult HrPracticeLadder()
		{
			return View();
		}

		public IActionResult HrBusinessLadder()
		{
			return View();
		}

		public IActionResult HrAdministrativeLadder()
		{
			return View();
		}

		public IActionResult LevelsAndTopics()
		{
			return View();
		}

		public IActionResult Levels()
		{
			return View();
		}

		public IActionResult RecommendedCourses()
		{
			return View();
		}

		public IActionResult CoursesByLearningLevel()
		{
			var courses = new CoursesByDto { Courses = _context.Courses.Where(x => x.IsActive).OrderBy(x => x.DisplayOrder).ToList() };

			return View(courses);
		}

		public IActionResult CourseInDetail(int courseId)
		{
			var course = new CourseInDetailDto { Course = _context.Courses.FirstOrDefault(x => x.Id == courseId) };

			return View(course);
		}

		public IActionResult CoursesBySubjectArea()
		{
			var courses = new CoursesByDto { Courses = _context.Courses.Where(x => x.IsActive).OrderBy(x => x.DisplayOrder).ToList() };

			return View(courses);
		}

		public IActionResult Roles()
		{
			return View();
		}

		public IActionResult RoleDefinition(string role)
		{
			return View(model: role);
		}

		public async Task<IActionResult> TestAzureTokexn()
		{
// 			var scopes = new[] { "User.Read" };
//
// // Multi-tenant apps can use "common",
// // single-tenant apps must use the tenant ID from the Azure portal
// 			var tenantId = "31b2883e-6cb9-4fb8-9190-c37247992f9e";
//
// // Value from app registration
// 			var clientId = "05e66a5b-335e-42e0-b9b0-3e4dc8279894";
//
// // using Azure.Identity;
// 			var options = new TokenCredentialOptions { AuthorityHost = AzureAuthorityHosts.AzurePublicCloud };
//
// 			var userName = "GBarba@emailatg.com";
// 			var password = "GB115-C115";
//
// // https://docs.microsoft.com/dotnet/api/azure.identity.usernamepasswordcredential
// 			var userNamePasswordCredential = new UsernamePasswordCredential(
// 				userName, password, tenantId, clientId, options);
//
// 			var graphClient = new GraphServiceClient(userNamePasswordCredential, scopes);
////////////////-------------------------------------------------------------------------------------------------------
////////////////-------------------------------------------------------------------------------------------------------

			var scopes = new[] { "https://graph.microsoft.com/.default" };


// Multi-tenant apps can use "common",
// single-tenant apps must use the tenant ID from the Azure portal
			var tenantId = "31b2883e-6cb9-4fb8-9190-c37247992f9e";

// Values from app registration
			var clientId = "1e3fa3b9-a491-434d-bea2-a71781f7489f";
			var clientSecret = "7GJ7Q~lsjrlGmYJ6OBAhl631dIDN~PAW3nUE4";

// using Azure.Identity;
			var options = new TokenCredentialOptions { AuthorityHost = AzureAuthorityHosts.AzurePublicCloud };

// https://docs.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
			var clientSecretCredential = new ClientSecretCredential(
				tenantId, clientId, clientSecret, options);

			var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

			var users = await graphClient.Me
				.Request()
				.GetAsync();

			return View();
		}

		[AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
		public async Task<IActionResult> TestAzureToken()
		{
			var asdf = await _graphServiceClient.Me
				.GetMemberGroups(true)
				.Request()
				.PostAsync();
			User currentUser = null;

			try
			{
				currentUser = await _graphServiceClient.Me.Request().GetAsync();
				var calendar = await _graphServiceClient.Me.Calendar
					.Request()
					.GetAsync();

				var events = await _graphServiceClient.Me.Calendar.Events
					.Request()
					//.Filter("startsWith(subject,'All')")
					.GetAsync();
			}
			// Catch CAE exception from Graph SDK
			catch (ServiceException svcex) when (svcex.Message.Contains("Continuous access evaluation resulted in claims challenge"))
			{
				try
				{
					// Console.WriteLine($"{svcex}");
					string claimChallenge = WwwAuthenticateParameters.GetClaimChallengeFromResponseHeaders(svcex.ResponseHeaders);
					// _consentHandler.ChallengeUser(_graphScopes, claimChallenge);
					return new EmptyResult();
				}
				catch (Exception ex2)
				{
					// _consentHandler.HandleException(ex2);
				}
			}

			try
			{
				// Get user photo
				using (var photoStream = await _graphServiceClient.Me.Photo.Content.Request().GetAsync())
				{
					byte[] photoByte = ((MemoryStream)photoStream).ToArray();
					ViewData["Photo"] = Convert.ToBase64String(photoByte);
				}
			}
			catch (Exception pex)
			{
				// Console.WriteLine($"{pex.Message}");
				ViewData["Photo"] = null;
			}

			ViewData["Me"] = currentUser;

			return View("TestAzureToken");
		}

		[AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
		public async Task<IActionResult> TestCreateEvent()
		{
// // The client credentials flow requires that you request the
// // /.default scope, and preconfigure your permissions on the
// // app registration in Azure. An administrator must grant consent
// // to those permissions beforehand.
// 			var scopes = new[] { "https://graph.microsoft.com/.default" };
//
// // Multi-tenant apps can use "common",
// // single-tenant apps must use the tenant ID from the Azure portal
// 			var tenantId = "31b2883e-6cb9-4fb8-9190-c37247992f9e";
//
// // Values from app registration
// 			var clientId = "1e3fa3b9-a491-434d-bea2-a71781f7489f";
// 			var clientSecret = "7GJ7Q~lsjrlGmYJ6OBAhl631dIDN~PAW3nUE4";
//
// // using Azure.Identity;
// 			var options = new TokenCredentialOptions { AuthorityHost = AzureAuthorityHosts.AzurePublicCloud };
//
// // https://docs.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
// 			var clientSecretCredential = new ClientSecretCredential(
// 				tenantId, clientId, clientSecret, options);
//
// 			var graphClient = new GraphServiceClient(clientSecretCredential, scopes);
//

			var @event = new Event
			{
				Subject = "Let's go for lunch",
				Body = new ItemBody { ContentType = BodyType.Html, Content = "Does mid month work for you?" },
				Start = new DateTimeTimeZone { DateTime = "2021-12-20T12:00:00", TimeZone = "Pacific Standard Time" },
				End = new DateTimeTimeZone { DateTime = "2021-12-20T14:00:00", TimeZone = "Pacific Standard Time" },
				Location = new Location { DisplayName = "Harry's Bar" },
				Attendees = new List<Attendee>() { new Attendee { EmailAddress = new EmailAddress { Address = "GBarba@emailatg.com", Name = "Adele Vance" }, Type = AttendeeType.Required } },
				TransactionId = "7E163156-7762-4BEB-A1C6-729EA81755A7"
			};


			try
			{
				var res = await _graphServiceClient.Me.Calendars["AAMkADJjZDY4NTkwLTQ0ZGYtNDZkZC1hYzEyLTFiOGYzOTQ3OWJiYwBGAAAAAABbEoP05qtsRY-inMwvpPvXBwCEN_b_aBPrTJkjQ3YvWQ6wAAAAAAEGAACEN_b_aBPrTJkjQ3YvWQ6wAAAAAGU3AAA="].Events
					.Request()
					.AddAsync(@event);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}

			return View("TestAzureToken");
		}

		[AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
		public async Task<IActionResult> GetScheduleTest()
		{
			var schedules = new List<String>()
			{
				"GBarba@emailatg.com",
				"MRamirez@emailatg.com"
			};

			var startTime = new DateTimeTimeZone
			{
				DateTime = "2021-12-01T09:00:00",
				TimeZone = "Pacific Standard Time"
			};

			var endTime = new DateTimeTimeZone
			{
				DateTime = "2021-12-30T18:00:00",
				TimeZone = "Pacific Standard Time"
			};

			var availabilityViewInterval = 60;

			var schedule = await _graphServiceClient.Me.Calendar
				.GetSchedule(schedules,endTime,startTime,availabilityViewInterval)
				.Request()
				.Header("Prefer","outlook.timezone=\"Pacific Standard Time\"")
				.PostAsync();

			return View("TestAzureToken");
		}
	}
}
