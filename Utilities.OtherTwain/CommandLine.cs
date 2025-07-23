/*
 * User: Sam Brinly
 * Date: 7/23/2014
 */
using System;
using System.Diagnostics;

namespace EdocsUSA.Utilities
{
	public static class CommandLine
	{
		public static string GetParameter(string[] args, string parameterName)
		{
			if (parameterName.StartsWith(@"/") == false)
			{ parameterName = @"/" + parameterName; }
			if (parameterName.EndsWith(@":") == false)
			{ parameterName = @":" + parameterName; }
			foreach (string arg in args)
			{
				if (arg.Trim().StartsWith(parameterName, StringComparison.OrdinalIgnoreCase))
				{ 
					if (arg.Trim().Length <= parameterName.Length)
					{

                        EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.TraceWarning("Arg found, but no value provided");
						return null;
					}
					else
                    {
                        Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Returing arg{arg.Substring(parameterName.Length)}");
                        return arg.Substring(parameterName.Length);
                    }
				}
			}
			return null;
		}
	}
}
