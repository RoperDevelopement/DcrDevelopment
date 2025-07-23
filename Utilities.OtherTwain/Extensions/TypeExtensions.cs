/*
 * User: Sam Brinly
 * Date: 1/23/2013
 */
using System;

namespace EdocsUSA.Utilities.Extensions
{
	/// <summary>
	/// Description of TypeExtensions.
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Provides support for C#'s default keyword to other languages
		/// </summary>
		/// <returns></returns>
		public static T Default<T>()
		{
			return default(T);
		}
		
		public static object GetDefaultValue(this Type type)
		{
			if ((type == null) || (type.IsValueType == false) || (type == typeof(void)))
			{ return null; }
			
			if (type.ContainsGenericParameters)
			{ throw new ArgumentException("The supplied value type <" + type.ToString() + "> contains generic parameters, so the default value cannot be retrieved"); }
			
			if (type.IsPrimitive || (type.IsNotPublic == false))
			{
				try
				{ return Activator.CreateInstance(type); }
				catch (Exception ex)
				{
					throw new ArgumentException("Could not create default instance of <" + type.ToString() + ">" + ex.Message);
				}
			}
			
			throw new ArgumentException("Could not create default instance of <" + type.ToString() + ">");
		}
		
		public static T GetDefaultValue<T>()
		{ return (T)GetDefaultValue(typeof(T)); }
	}
}
