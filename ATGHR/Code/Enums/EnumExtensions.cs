using System;
using System.ComponentModel;

namespace ATGHR.Code.Enums
{
	public static class EnumExtensions
	{
		private static T GetAttribute<T>(this Enum value) where T : Attribute
		{
			if (value == null)
				return null;

			var memberInfo = value.GetType().GetMember(value.ToString());
			var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
			return (T)attributes[0];
		}


		public static string ToDescription(this Enum value) //enumlarin descriptionlarini alabilmek icin.
		{
			if (value == null)
				return null;

			var attribute = value.GetAttribute<DescriptionAttribute>();

			return attribute == null ? value.ToString() : attribute.Description;
		}
	}
}
