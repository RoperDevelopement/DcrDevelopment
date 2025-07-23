/*
 * User: Sam Brinly
 * Date: 2/20/2013
 */
using System;
using System.Diagnostics;

namespace EdocsUSA.Utilities
{
	public static class DebugEx
	{
		public static void WriteLine(string line)
		{
			#if DEBUG
    		StackTrace stackTrace = new StackTrace();
    		StackFrame previousFrame = stackTrace.GetFrame(1);
    		Debug.WriteLine(previousFrame.GetFileName() + "." + previousFrame.GetMethod().Name + ":" + line);
			#endif
		}
	}
}
