using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EdocsUSA.Utilities.Logging
{
	public class CsvFileDictionaryLogger : FileDictionaryLoggerBase
	{		
		private string _Separator = ",";
		public string Separator 
		{ 
			get { return _Separator; }
			set { _Separator = value; }
		}
		
		private string _SeparatorReplacement = " ";
		public string SeparatorReplacement 
		{ 
			get { return _SeparatorReplacement; }
			set { _SeparatorReplacement = value; }
		}		
		
		private string _NewlineReplacement = " ";
		public string NewlineReplacement
		{
			get { return _NewlineReplacement; }
			set { _SeparatorReplacement = value; }
		}
		
		public CsvFileDictionaryLogger() : base()
		{ }
		
		public CsvFileDictionaryLogger(string filePath) : base(filePath)
		{ }
		
		public CsvFileDictionaryLogger(string filePath, string separator) : this(filePath)
		{ Separator = separator; }
		
		public CsvFileDictionaryLogger(string filePath, string separator, string separatorReplacement) : this(filePath, separator)
		{ SeparatorReplacement = separatorReplacement; }
		
		public CsvFileDictionaryLogger(string filePath, string separator, string separatorReplacement, string newlineReplacement) : this(filePath, separator, separatorReplacement)
		{ NewlineReplacement = newlineReplacement; }
		
		/// <remarks>Newlines are replaced by a single space.</remarks>
		public override void Append(Dictionary<string, string> entry)
		{			
			//Copy the dictionary to arrays so indexing can be used
			//Could be more efficient to use the two enumerators, but this seems simpler for now
			string[] keys = entry.Keys.ToArray();
			string[] values = entry.Values.ToArray();
			
			//If the file doesn't exists, we must create the header.
			bool createHeader = (File.Exists(FilePath) == false);
			
			if (createHeader)
			{
				string headerLine = StringTools.GenerateSeparatedList(entry.Keys, Separator, SeparatorReplacement);
				AppendLine(headerLine);
			}
			
			string entryLine = StringTools.GenerateSeparatedList(entry.Values, Separator, SeparatorReplacement);
			AppendLine(entryLine.RemoveLineBreaks(NewlineReplacement));
		}
	}
}
