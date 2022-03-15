using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ATG.DBNet;
using ATGHR.Models;
using ATGHR.Models.Database;
using System.Reflection;

namespace ATGHR.Code
{
	public static class Util
	{
		/// <summary>
		/// Get the session user.
		/// </summary>
		/// <param name="lookupTables">LookupTables object from services.</param>
		/// <param name="context">Request HttpContext.</param>
		/// <returns>The session user.</returns>
		public static User GetUser(
			HttpContext context)
		{
			return User.GetSessionUser(context);
		}

		public static IEnumerable<string> ToCsv<T>(IEnumerable<T> objectlist, string separator = ",", bool header = true)
		{
			FieldInfo[] fields = objectlist.GetType().GetGenericArguments().FirstOrDefault().GetFields();
			PropertyInfo[] properties = objectlist.GetType().GetGenericArguments().FirstOrDefault().GetProperties();
			if (header)
			{
				yield return String.Join(separator, fields.Select(f => f.Name).Concat(properties.Select(p => p.Name)).ToArray());
			}
			foreach (var o in objectlist)
			{
				yield return string.Join(separator, fields.Select(f => (f.GetValue(o) ?? "").ToString())
					.Concat(properties.Select(p => (p.GetValue(o, null) ?? "").ToString())).ToArray());
			}
		}

		/// <summary>
		/// Non-null check/return with data typing.
		/// Null or empty string input will return defaultOutput.
		/// </summary>
		/// <typeparam name="T">Type of output</typeparam>
		/// <param name="input">Input value to check for null</param>
		/// <param name="defaultOutput">Output value if input is null</param>
		/// <returns>Type T converted: input or defaultOutput if input is null</returns>
		public static T Nz<T>(
			Object input,
			Object defaultOutput)
		{
			if (DBNull.Value.Equals(input)
				|| input == null
				|| (input.GetType() == typeof(String)
					&& String.IsNullOrEmpty(input.ToString())))
			{
				return defaultOutput == null ? (T)defaultOutput : (T)Convert.ChangeType(defaultOutput, Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
			}
			else
			{
				try
				{
					return (T)Convert.ChangeType(input, Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
				}
				catch
				{
					if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?))
					{
						try
						{
							return (T)Convert.ChangeType(DateTime.FromOADate(Double.Parse(input.ToString())), Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
						}
						catch
						{
							// Nothing here, just proceed
						}
					}
					if (typeof(T) == typeof(Boolean) || typeof(T) == typeof(Boolean?))
					{
						try
						{
							switch (input.ToString().ToUpper().Trim())
							{
								case "Y":
								case "YES":
								case "T":
								case "TRUE":
								case "1":
									return (T)Convert.ChangeType(true, Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
								default:
									return (T)Convert.ChangeType(false, Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
							}
						}
						catch
						{
							// Nothing here, just proceed
						}
					}

					return (T)Convert.ChangeType(defaultOutput, Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
				}
			}
		}
	}
}
