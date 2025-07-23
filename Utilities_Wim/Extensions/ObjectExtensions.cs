using System;
using System.Diagnostics;
using System.Reflection;

namespace EdocsUSA.Utilities.Extensions
{
	public static class ObjectExtensions
	{
		public static void SetProperty(this object source, string propertyName, object propertyValue)
		{
			if (source.TrySetProperty(propertyName, propertyValue) == false)
			{ throw new InvalidOperationException("Cannot set " + propertyName); }
		}
		
		public static bool TrySetProperty(this object source, string propertyName, object propertyValue)
		{
			if (source == null)
			{ throw new ArgumentNullException("source"); }
			
			PropertyInfo pi = source.GetType().GetProperty(propertyName);
			if (pi != null)
			{
				if (pi.CanWrite == false)
				{ 
					Trace.TraceWarning(propertyName + " is read-only");
					return false;
				}
				if (pi.PropertyType != propertyValue.GetType())
				{ 
					Trace.TraceWarning(propertyValue.GetType().ToString() + " is not a valid type for " + propertyName);
					return false;
				}
				
				pi.SetValue(source, propertyValue, null);
				return true;
			}
			else
			{
				Trace.TraceWarning(source.GetType() + " does not have a public property named " + propertyName);
				return false;
			}
		}
	}
}
