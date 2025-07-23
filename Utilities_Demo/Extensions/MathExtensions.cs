/*
 * User: Sam Brinly
 * Date: 12/19/2013
 */
using System;

namespace EdocsUSA.Utilities.Extensions
{
	public static class MathExtensions
	{
		public static decimal RoundToNearestDecimal(decimal value, decimal precision)
		{
			if (precision == 0)
			{ return Math.Round(value, MidpointRounding.AwayFromZero); }
			
			decimal integerPart = Decimal.Truncate(value);
			decimal fractionPart = Frac(value);
			decimal multiplier = 1 / precision;
			decimal newFractionPart = Math.Round(fractionPart * multiplier, 0) / multiplier;
			return integerPart + newFractionPart;
		}
		
		public static float RoundToNearestDecimal(float value, decimal precision)
		{
			return (float)RoundToNearestDecimal((decimal)value, precision);
		}
		
		public static decimal Frac(decimal source)
		{ return (source - Decimal.Truncate(source)); }
		
		public static float Frac(float source)
		{ return (float)Frac((decimal)source); }
		
		public static double Frac(double source)
		{ return (double)Frac((decimal)source); }
	
	}
}
