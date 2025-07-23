using System;
using System.Collections.Generic;

namespace EdocsUSA.Utilities.Logging
{
	public interface IDictionaryLogger
	{		
		void Append(Dictionary<string, string> entry);
	}
}
