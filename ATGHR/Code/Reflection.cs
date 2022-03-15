using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace ATGHR.Code
{
	public static class Reflection
	{
		/// <summary>
		/// Enumerate all properties in a class.
		/// </summary>
		/// <param name="obj">The object to be inspected.</param>
		/// <returns>Array of properties.</returns>
		internal static IEnumerable<PropertyDescriptor> GetProperties(
			Object obj)
		{
			return TypeDescriptor.GetProperties(obj).Cast<PropertyDescriptor>();
		}

		/// <summary>
		/// Determine whether a property exists on an object.
		/// </summary>
		/// <param name="obj">The object to be inspected.</param>
		/// <param name="name">The property to be found.</param>
		/// <returns>True/false.</returns>
		internal static Boolean HasProperty(
			Object obj,
			String name)
		{
			return GetProperties(obj).Any(p => String.Equals(p.Name, name, StringComparison.InvariantCulture));
		}

		/// <summary>
		/// Get a property from a class by name.
		/// </summary>
		/// <param name="obj">The object to be inspected.</param>
		/// <param name="name">The property to be found.</param>
		/// <returns>Property.</returns>
		internal static PropertyDescriptor GetProperty(
			Object obj,
			String name)
		{
			return GetProperties(obj).FirstOrDefault(p => String.Equals(p.Name, name, StringComparison.InvariantCulture));
		}

		/// <summary>
		/// Copy the properties of one object to another.
		/// This is intended to duplicate an inherited object into the inheritor.
		/// Any mismatched types or other unexpected errors WILL be sent up the call stack.
		/// </summary>
		/// <param name="From">The object from which to copy properties (inherited type).</param>
		/// <param name="To">The object to which to copy properties (inheriting type).</param>
		public static void CopyProperties(
			Object From,
			Object To)
		{
			GetProperties(From).ToList().ForEach(
				p =>
				{
					if (HasProperty(To, p.Name))
					{
						GetProperty(To, p.Name).SetValue(To, p.GetValue(From));
					}
				});
		}

		/// <summary>
		/// Copy an object from one type to another and return the result as a new object.
		/// This requires a parameterless constructor, so dependency injection requirements are not allowed.
		/// This is intended to duplicated an inherited object into the inheritor.
		/// Any mismatched types or other unexpected errors WILL be sent up the call stack.
		/// </summary>
		/// <typeparam name="T">The type of object to be returned.</typeparam>
		/// <param name="From">The object from which to copy properties.</param>
		/// <returns>A new object of requested type.</returns>
		public static T CopyObject<T>(
			Object From)
		{
			T To = Activator.CreateInstance<T>();

			CopyProperties(From, To);

			return To;
		}
	}
}
