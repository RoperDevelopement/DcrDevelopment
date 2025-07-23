using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace EdocsUSA.Utilities.Extensions
{
	public static class BlockingCollectionExtensions
	{
		public static void AddRange<T>(this BlockingCollection<T> col, IEnumerable<T> items)
		{
			foreach (T item in items)
			{ col.Add(item); }
		}
	}
}
