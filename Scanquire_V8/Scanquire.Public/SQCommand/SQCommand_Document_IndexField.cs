using System;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public
{
    /// <summary>A command to specify the value of a named metadata field to apply to a document.</summary>
	public class SQCommand_Document_IndexField : ISQCommand_Document
	{
        private readonly string _Name;
        /// <summary>The name of the metadata field associated with the document.</summary>
        public string Name { get { return _Name; } }

        private readonly object _Value;
        /// <summary>The value associated with the named metadata field.</summary>
        public object Value { get { return _Value; } }

        public SQCommand_Document_IndexField(string name, object value)
		{
            this._Name = name;
            this._Value = value;
            ETL.TraceLoggerInstance.TraceInformation($"Applying metadata value:{value} to field:{name} to the document.");

        }
	}
}
