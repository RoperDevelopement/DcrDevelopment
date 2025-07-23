using System;
using System.Collections.Generic;

namespace EdocsUSA.Utilities
{
	public class IEnumeratorProgress<T>
	{
		public int Current { get; private set; }
		
		public int Total { get; private set; }
		
		public decimal PercentCompletePrecise { get; private set; }
		
		public int PercentComplete { get; private set; }
			
		public T Value { get; private set; }

		public string Caption { get; private set; }
		
		public IEnumeratorProgress(T value, string caption, int current, int total)
		{
			Value = value;
			Current = current;
			Total = total;
			PercentCompletePrecise = Percent.Calculate(Current, Total);
			PercentComplete = (int)PercentCompletePrecise;
			Caption = String.IsNullOrWhiteSpace(caption) 
				? Current + " of " + (Total <= 0 ? "?" : Total.ToString()) + " (" + (int)PercentComplete + "%)"
				: caption;
		}
		
		public IEnumeratorProgress(T value, int current, int total) 
			: this(value, null, current, total) { }
		
		public IEnumeratorProgress(string caption, int current, int total)
			: this(default(T), caption, current, total) { }
		
	}
}
