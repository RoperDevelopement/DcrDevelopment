/*
 * User: Sam Brinly
 * Date: 10/29/2013
 */
using System;

namespace EdocsUSA.Utilities
{
	/// <summary>
	/// Description of Percent.
	/// </summary>
	public static class Percent
	{
		public static decimal Calculate(decimal value, decimal total)
		{
			if (total <= 0) return 0;
			
			decimal percentPerUnit = 100 / total;
			decimal valuePercent = percentPerUnit * value;
			
			return valuePercent;
		}
	}
}
