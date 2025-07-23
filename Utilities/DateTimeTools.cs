/*
 * Created by SharpDevelop.
 * User: Sam Brinly
 * Date: 3/21/2011
 * Time: 9:53 AM
 */
using System;

namespace EdocsUSA.Utilities
{
	/// <summary>Utility methods for dealing with DateTime</summary>
	public static class DateTimeTools
	{
		public static DateTime GetPreviousDayOfWeek(DateTime startDate, DayOfWeek dayOfWeek)
		{
			DayOfWeek currentDayOfWeek = startDate.DayOfWeek;
			int c = (int)currentDayOfWeek;
			int d = (int)dayOfWeek;		
		
			if (dayOfWeek >= currentDayOfWeek) return startDate.Subtract(new TimeSpan(c + (7 - d), 0, 0, 0));
			else return startDate.Subtract(new TimeSpan(c - d, 0, 0, 0));
		}
		
		public static DateTime GetNextDayOfWeek(DateTime startDate, DayOfWeek dayOfWeek)
		{
			DayOfWeek currentDayOfWeek = startDate.DayOfWeek;
			int c = (int)currentDayOfWeek;
			int d = (int)dayOfWeek;
			
			if (dayOfWeek <= currentDayOfWeek) return startDate.Add(new TimeSpan(d + (7 - c), 0, 0, 0));
			else return startDate.Add(new TimeSpan(d - c, 0, 0, 0));
		}
	}
}
