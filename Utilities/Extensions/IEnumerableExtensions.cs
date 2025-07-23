using System;
using System.Collections.Generic;
using System.Linq;

namespace EdocsUSA.Utilities.Extensions
{
	public static class IEnumerableExtensions
	{
		public static bool CanCount<T>(this IEnumerable<T> source)
		{
			return source as ICollection<T> != null;
		}
		
		public static bool TryCount<T>(this IEnumerable<T> source, out int count)
		{
			if (source.CanCount())
			{
				count = source.Count();
				return true;
			}
			else
			{
				count = -1;
				return false;
			}
		}
		
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (T item in source)
			{ action(item); }
		}
	}
}
