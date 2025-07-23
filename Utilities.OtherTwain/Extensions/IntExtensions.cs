/*
 * User: Sam Brinly
 * Date: 1/29/2013
 */
using System;
using System.Collections.Generic;

namespace EdocsUSA.Utilities.Extensions
{
	public static class IntExtensions
	{
		public static IEnumerable<int> Range(int start, int end)
		{			
			if (start <= end) { for (int i = start; i <= end; i++) yield return i; }
			else { for (int i = start; i >= end; i--) yield return i; }
		}
		
		public static int ConstrainTo(this int value, int bound1, int bound2)
		{
			int lowerBound;
			int upperBound;
			if (bound1 <= bound2)
			{
				lowerBound = bound1;
				upperBound = bound2;
			}
			else
			{
				lowerBound = bound2;
				upperBound = bound1;
			}
			
			if (value < lowerBound) value = lowerBound;
			if (value > upperBound) value = upperBound;
			
			return value;
		}
		
		
		
	}
}
