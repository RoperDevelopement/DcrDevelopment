
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;

namespace EdocsUSA.Utilities.Logging
{
	public class JsonFileDictionaryLogger : FileDictionaryLoggerBase
	{	
		public JsonFileDictionaryLogger() : base()
		{ }
		
		public JsonFileDictionaryLogger(string filePath) : base(filePath)
		{ }
		
		public override void Append(Dictionary<string, string> entry)
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			AppendLine(serializer.Serialize(entry));
		}
	}
}
