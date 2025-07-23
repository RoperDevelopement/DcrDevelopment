using System;
using System.ComponentModel;

namespace EdocsUSA.Utilities.Extensions
{
	public static class TypeConverterExtensions
	{
		public static bool TryConvertTo(this TypeConverter converter, object value, Type destinationType, out object convertedValue)
		{
			try
			{
				convertedValue = converter.ConvertTo(value, destinationType);
				return true;
			}
			catch 
			{
				convertedValue = null;
				return false;
			}
		}
		
		public static bool TryConvertFrom(this TypeConverter converter, object value, out object convertedValue)
		{
			try
			{
				convertedValue = converter.ConvertFrom(value);
				return true;
			}
			catch 
			{
				convertedValue = null;
				return false;
			}
		}
	}
}
