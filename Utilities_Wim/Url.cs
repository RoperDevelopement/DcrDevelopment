/*
 * User: Sam Brinly
 * Date: 7/23/2014
 */
using System;

namespace EdocsUSA.Utilities
{
	public static class Url
	{
		public static string Join(params string[] values)
      {
            Logging.TraceLogger.TraceLoggerInstance.TraceError("Joining strings");
            if (values.Length < 2)
		    {
                Logging.TraceLogger.TraceLoggerInstance.TraceError("2 or more values are required");
                throw new ArgumentException("2 or more values are required");
            }
		
		    string value = values[0];
		    for (int i = 1; i < values.Length; i++)
		    { value = string.Join("/", value.TrimEnd('/'), values[i].TrimStart('/')); }

            Logging.TraceLogger.TraceLoggerInstance.TraceError($"Joined string:{value}");

            return value;
		}
	}
}
