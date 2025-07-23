using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{
    public class ArchivingTextWriterTraceListener : TextWriterTraceListener
    {
        //private string _DirectoryPath = @"Logs";
        private string _DirectoryPath = BinUtilities.BinMonLogFolder;
        public string DirectoryPath
        {
            get {return _DirectoryPath;}
            set {_DirectoryPath = value;}
        }

        private string _LogFileExtension = "log";
        public string LogFileExtension
        {
            get { return _LogFileExtension; }
            set { _LogFileExtension = value; }
        }

        public ArchivingTextWriterTraceListener()
        {
            Directory.CreateDirectory(DirectoryPath);
            DateTime timestamp = DateTime.Now;
            string logFileName = TimestampToLogFileName(timestamp);
            string logFileNameWithExt = Path.ChangeExtension(logFileName, _LogFileExtension);
            string logFilePath = Path.Combine(this.DirectoryPath, logFileNameWithExt);
            base.Writer = new StreamWriter(logFilePath);
        }

        public string TimestampToLogFileName(DateTime time)
        {
            return time.ToString("yyyyMMdd_HHmmss");
        }
    }
}
