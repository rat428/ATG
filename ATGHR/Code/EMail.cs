using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using ATG.DBNet;
using ATGHR.Models.Database;

namespace ATGHR.Code
{
	public class EMail
	{
		public static String Username = "ATGHR";

		public static String Password = "ATG124-HR124";

		public const String SmtpServer = "smtp.office365.com";

		public static String From = Username + "@emailatg.com";

		public class Recipient
		{
			[DBIgnore]
			public String Address
			{
				get
				{
					return Username;
				}
			}

			public String Username { get; set; } = null;
		}

		public class BodyInfo
		{
			public String Relation { get; set; } = null;

			public String SheetType { get; set; } = null;

			public Int64? SheetId { get; set; } = null;
			public String OwnerName { get; set; } = null;
			public String CustomMessage { get; set; } = null;

		}

		public class Notifee
		{
			[DBIgnore]
			public String Address
			{
				get
				{
					return Username;
				}
			}

			public String Username { get; set; } = null;

			public IEnumerable<BodyInfo> BodyInfos { get; set; } = null;
		}


		public static void SendMail(
			Recipient[] recipients = null,
			Recipient[] ccRecipients = null,
			Recipient[] bccRecipients = null,
			String subject = "",
			String body = "",
			String[] attachments = null)
		{
			if (recipients == null
				&& ccRecipients == null
				&& bccRecipients == null)
			{
				throw new Exception("No message recipients specified.");
			}

			if (String.IsNullOrWhiteSpace(subject))
			{
				throw new Exception("No message subject specified.");
			}

			SmtpClient smtpClient = new SmtpClient();
			NetworkCredential basicCredential = new NetworkCredential(From, Password);
			MailMessage message = new MailMessage();
			MailAddress fromAddress = new MailAddress(From);

			smtpClient.Host = SmtpServer;
			smtpClient.Port = 587;
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Credentials = basicCredential;
			smtpClient.Timeout = 60 * 5 * 1000;
			smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtpClient.EnableSsl = true;

			message.From = fromAddress;
			message.Subject = subject;
			message.IsBodyHtml = true;
			message.Body = body.Replace("\r\n", "<br>");
			foreach (Recipient recipient in recipients ?? new Recipient[] { })
			{
				message.To.Add(recipient.Address);
			}

			foreach (Recipient recipient in ccRecipients ?? new Recipient[] { })
			{
				message.CC.Add(recipient.Address);
			}

			foreach (Recipient recipient in bccRecipients ?? new Recipient[] { })
			{
				message.Bcc.Add(recipient.Address);
			}

			if (attachments != null)
			{
				foreach (String attachment in attachments)
				{
					message.Attachments.Add(new Attachment(attachment));
				}
			}

			if (!ATGHR.AppSettings.IsDev)
			{
				smtpClient.Send(message);
			}
		}

		public enum SheetChangeType
		{
			Update = 1,
			Approve = 2
		}

		public static void SendSheetNotification(
			Int64? SheetId,
			String SheetType,
			Int32? Year,
			String ApprovalType,
			SheetChangeType ChangeType,
			Boolean? approved = null,
			DateTime? DataDatetime = null,
			Int64? UserId = null,
			Int64? DataUserId = null)
		{
			IEnumerable<Recipient> recipients = new Recipient[0];

			DBNet.ReadMulti(
				ref recipients,
				SQL: "SP_Performance" + SheetType + "SheetNotificationRecipient_Read",
				useProperties: false,
				extraParameters: new List<SqlParameter>()
				{
					new SqlParameter("@Performance" + SheetType + "SheetId", SheetId),
					new SqlParameter("@ApprovalType", ApprovalType),
					new SqlParameter("@Approved", approved)
				});

			if (recipients.Count() > 0)
			{
				UserLookup owner = ATGHR.LookupTables.Users.FirstOrDefault(u => u.UserId == UserId);
				UserLookup updater = ATGHR.LookupTables.Users.FirstOrDefault(u => u.UserId == DataUserId);

				String subject = String.Format(
					"ATGHR: {0} {1} {2} sheet update",
					(owner?.Name ?? owner?.Username) ?? "User",
					Year,
					SheetType.ToLower(),
					ChangeType.ToString());

				String body = String.Format(
					"<p>The {1} {2} belonging to {0} was {3}{4} at {5:hh:mm:ss tt MM/dd/yyyy}.</p><p><a href='https://atghr/Performance/{6}Sheet/{7}'>Click here to view the current details.</a></p><p>Please do not reply to this email.</p>",
					(owner?.Name ?? owner?.Username) ?? "User",
					Year,
					SheetType.ToLower() == "goal" ? "goals sheet" : "performance review",
					(approved ?? true) ? (ApprovalType == "Employee" ? "completed" : "approved") : "reset",
					!(approved ?? true) || ApprovalType != "Employee" ? " by " + (updater?.Name ?? updater?.Username) ?? "a user" : "",
					DataDatetime,
					SheetType,
					SheetId);

				SendMail(
					bccRecipients: recipients.ToArray(),
					subject: subject,
					body: body);
			}
		}

		public static void SendBonusNotification(
			Int64? SpotBonusId,
			String ApprovalType,
			Boolean? approved = null,
			DateTime? DataDatetime = null,
			Int64? UserId = null,
			Int64? DataUserId = null)
		{
			IEnumerable<Recipient> recipients = new Recipient[0];

			DBNet.ReadMulti(
				ref recipients,
				SQL: "SP_SpotBonus" + ApprovalType + "Notification_Read",
				useProperties: false,
				extraParameters: new List<SqlParameter>()
				{
					new SqlParameter("@SpotBonusId", SpotBonusId),
					new SqlParameter("@ApprovalType", ApprovalType),
					new SqlParameter("@Approved", approved)
				});

			if (recipients.Count() > 0)
			{
				UserLookup owner = ATGHR.LookupTables.Users.FirstOrDefault(u => u.UserId == UserId);
				UserLookup updater = ATGHR.LookupTables.Users.FirstOrDefault(u => u.UserId == DataUserId);

				String subject = String.Format(
					"ATGHR: {0}'s All Star Award update",
					(owner?.Name ?? owner?.Username) ?? "User");

				String body = String.Format(
					"<p>The All Star Award requested by {0} was {1}{2} at {3:hh:mm:ss tt MM/dd/yyyy}.</p><p><a href='https://atghr/Rewards/Bonus/{4}'>Click here to view the current details.</a></p><p>Please do not reply to this email.</p>",
					(owner?.Name ?? owner?.Username) ?? "User",
					(approved ?? true) ? (ApprovalType == "submitted" ? "submitted" : "approved") : "reset",
					!(approved ?? true) || ApprovalType != "submitted" ? " by " + (updater?.Name ?? updater?.Username) ?? "a user" : "",
					DataDatetime,
					SpotBonusId);

				SendMail(
					bccRecipients: recipients.ToArray(),
					subject: subject,
					body: body);
			}
		}

		public static void SendSheetReminder(
			String Username,
			IEnumerable<BodyInfo> BodyInfos)
		{
			String subject = String.Format(
				"ATGHR: Action(s) Required"
				);

			StringBuilder body = new StringBuilder("");

			body.Append(BodyInfos.FirstOrDefault(b => !String.IsNullOrEmpty(b.CustomMessage))?.CustomMessage ?? "");

			body.Append("<p>The following item(s) in ATGHR require your attention:</p>");

			foreach (BodyInfo item in BodyInfos)
			{
				body.Append("<p>");
				if (item.Relation == "Self")
				{
					if (item.SheetType == "Goal")
					{
						body.Append("Goal setting");
					}
					else
					{
						body.Append("Self-evaluation");
					}
					body.AppendFormat(
					" is required on <a href='https://atghr/Performance/{0}Sheet/{1}'> your sheet.</a></p>",
					item.SheetType,
					item.SheetId);
				}
				else
				{
					body.AppendFormat(
						"{0} level action is required on <a href='https://atghr/Performance/{1}Sheet/{2}'> {3}'s {1} sheet.</a></p>",
						item.Relation,
						item.SheetType,
						item.SheetId,
						item.OwnerName);
				}
			};

			SendMail(
				bccRecipients: new Recipient[] { new Recipient() { Username = Username } },
				subject: subject,
				body: body.ToString());
		}
	}
}
