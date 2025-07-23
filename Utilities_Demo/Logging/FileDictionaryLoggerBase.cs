using System;
using System.Collections.Generic;
using System.IO;

namespace EdocsUSA.Utilities.Logging
{
	public abstract class FileDictionaryLoggerBase : IDictionaryLogger
	{
		public string FilePath { get; set; }
	
		public FileDictionaryLoggerBase()
		{ }
		
		public FileDictionaryLoggerBase(string filePath) : this()
		{ FilePath = filePath; }
		
		protected void AppendLine(string entry)
		{ 
			if (File.Exists(FilePath) == false)
			{ Directory.CreateDirectory(Path.GetDirectoryName(FilePath)); }
			File.AppendAllLines(FilePath, new string[] { entry });
		}
		
		public abstract void Append(Dictionary<string, string> entry);
	}
}
