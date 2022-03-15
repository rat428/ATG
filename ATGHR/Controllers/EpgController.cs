using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using ATGHR.Models;
using ATGHR.Models.Dtos;
using ATGHR.Models.EntityFramework.DbContext;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Extensions;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;

namespace ATGHR.Controllers
{
	public class EpgController : Controller
	{
		private readonly ILogger<EpgController> _logger;

		private readonly GraphServiceClient _graphServiceClient;
		private string[] _graphScopes = new[] { "user.read" };
		private readonly ApplicationDbContext _context;

		public EpgController(ApplicationDbContext context, ILogger<EpgController> logger, GraphServiceClient graphServiceClient,
			IConfiguration configuration)
		{
			_context = context;
			_logger = logger;
			_graphServiceClient = graphServiceClient;
			_graphScopes = configuration.GetValue<string>("DownstreamApi:Scopes")?.Split(' ');
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult President()
		{
			return View();
		}

		public IActionResult VicePresident()
		{
			return View();
		}

		public IActionResult Secretary()
		{
			return View();
		}

		public IActionResult ProfessionalDevelopmentCommittee()
		{
			return View();
		}

		public IActionResult ProgramCalendarCommittee()
		{
			return View();
		}

		public IActionResult MembershipCommittee()
		{
			return View();
		}

		public IActionResult NominatingCommittee()
		{
			return View();
		}

		public IActionResult SocialCommittee()
		{
			return View();
		}

		public IActionResult CommunityEngagementCommittee()
		{
			return View();
		}

		public IActionResult RecruitingCommittee()
		{
			return View();
		}

		public IActionResult BusinessDevelopmentCommittee()
		{
			return View();
		}

		public IActionResult Membership()
		{
			return View();
		}

		public IActionResult LeadershipBoard()
		{
			return View();
		}


		public IActionResult Oversight()
		{
			return View();
		}

		[AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
		public async Task<IActionResult> Calendar()
		{
			var get = await _graphServiceClient.Me
				.GetMemberGroups(true)
				.Request()
				.PostAsync();

			return View();
		}

		[HttpGet]
		[AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
		public async Task<OkObjectResult> GetData()
		{
			// var test = "    { \"data\": [\n        {\n            \"AppointmentId\": 1,\n            \"Text\": \"Website Re-Design Plan\",\n            \"Description\": null,\n            \"StartDate\": \"2021-04-26T16:30:00.000Z\",\n            \"EndDate\": \"2021-04-26T18:30:00.000Z\",\n            \"AllDay\": false,\n            \"RecurrenceRule\": null,\n            \"RecurrenceException\": null\n        },\n        {\n            \"AppointmentId\": 2,\n            \"Text\": \"Book Flights to San Fran for Sales Trip\",\n            \"Description\": null,\n            \"StartDate\": \"2021-04-26T19:00:00.000Z\",\n            \"EndDate\": \"2021-04-26T20:00:00.000Z\",\n            \"AllDay\": true,\n            \"RecurrenceRule\": null,\n            \"RecurrenceException\": null\n        }] }";

			var eventList = new List<CalendarEventDto>();
			try
			{
				var calendar = await _graphServiceClient.Me.Calendar
					.Request()
					.GetAsync();

				var res = _graphServiceClient.Me.Calendars;

				var events = await _graphServiceClient.Me.Calendar.Events
					.Request()
					//.Filter("startsWith(subject,'All')")
					.GetAsync();

				var calendarGroups = await _graphServiceClient.Me.CalendarGroups
					.Request()
					.GetAsync();

				var asdf =  await _graphServiceClient.Me.Calendars["AAMkADJjZDY4NTkwLTQ0ZGYtNDZkZC1hYzEyLTFiOGYzOTQ3OWJiYwBGAAAAAABbEoP05qtsRY-inMwvpPvXBwCEN_b_aBPrTJkjQ3YvWQ6wAAAAAAEGAACEN_b_aBPrTJkjQ3YvWQ6wAAAAAGU3AAA="].Events.Request().GetAsync();
				var asddddf = await _graphServiceClient.Me.Calendars["226bab5b-39f4-4afd-94f1-cfd2b6f8abe9"].Events.Request().GetAsync();



				int i = 1;
				foreach (var item in events.CurrentPage)
				{
					var calEvent = new CalendarEventDto {
						AppointmentId = i++,
						StartDate = item.Start.ToDateTime().AddHours(-5).ToDateTimeTimeZone().DateTime,
						EndDate = item.End.ToDateTime().AddHours(-5).ToDateTimeTimeZone().DateTime,
						// StartDate = item.Start.DateTime,
						// EndDate = item.End.DateTime,
						Text = item.Subject ,
						Description = item.BodyPreview
					};

					eventList.Add(calEvent);
				}
			}
			// Catch CAE exception from Graph SDK
			catch (ServiceException svcex) when (svcex.Message.Contains("Continuous access evaluation resulted in claims challenge"))
			{
				try
				{
					// Console.WriteLine($"{svcex}");
					string claimChallenge = WwwAuthenticateParameters.GetClaimChallengeFromResponseHeaders(svcex.ResponseHeaders);
					// _consentHandler.ChallengeUser(_graphScopes, claimChallenge);
					return new OkObjectResult(new EmptyResult());
				}
				catch (Exception ex2)
				{
					// _consentHandler.HandleException(ex2);
				}
			}

			// var model = SampleData.Appointments;

			// return new OkObjectResult (new { data= model});
			return new OkObjectResult(new { data = eventList });
		}
	}
}
//
// public partial class SampleData
// {
// 	public static readonly IEnumerable<CalendarEventDto> Appointments = new[]
// 	{
// 		new CalendarEventDto
// 		{
// 			AppointmentId = 1,
// 			Text = "Website Re-Design Plan",
// 			StartDate = "2021-04-26T16:30:00.000Z",
// 			EndDate = "2021-04-26T18:30:00.000Z",
// 			AllDay = false,
// 			Description = "desc",
// 			RecurrenceException = null,
// 			RecurrenceRule = null
// 		},
// 		new CalendarEventDto
// 		{
// 			AppointmentId = 2,
// 			Text = "Book Flights to San Fran for Sales Trip",
// 			StartDate = "2021-04-26T19:00:00.000Z",
// 			EndDate = "2021-04-26T20:00:00.000Z",
// 			AllDay = true,
// 			Description = "desc",
// 			RecurrenceException = null,
// 			RecurrenceRule = null
// 		},
// 		new CalendarEventDto
// 		{
// 			AppointmentId = 3,
// 			Text = "Install New Router in Dev Room",
// 			StartDate = "2021-04-26T21:30:00.000Z",
// 			EndDate = "2021-04-26T22:30:00.000Z",
// 			AllDay = true,
// 			Description = "desc",
// 			RecurrenceException = null,
// 			RecurrenceRule = null
// 		},
// 		new CalendarEventDto
// 		{
// 			AppointmentId = 4,
// 			Text = "Approve Personal Computer Upgrade Plan",
// 			StartDate = "2021-04-27T17:00:00.000Z",
// 			EndDate = "2021-04-27T18:00:00.000Z",
// 			AllDay = true,
// 			Description = "desc",
// 			RecurrenceException = null,
// 			RecurrenceRule = null
// 		},
// 		new CalendarEventDto
// 		{
// 			AppointmentId = 5,
// 			Text = "Final Budget Review",
// 			StartDate = "2021-04-27T19:00:00.000Z",
// 			EndDate = "2021-04-27T20:35:00.000Z",
// 			AllDay = true,
// 			Description = "desc",
// 			RecurrenceException = null,
// 			RecurrenceRule = null
// 		},
// 	};
// }
