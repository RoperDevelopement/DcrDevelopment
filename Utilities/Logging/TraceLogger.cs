using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
namespace EdocsUSA.Utilities.Logging
{
    enum TraceType
    {
        Error,
        Warning,
        Information
    }
    public class TraceLogger : ITraceLogging, IDisposable
    {
        private Stopwatch watch;
        private readonly string Header = "Application Name,Log Time,Message Type,Message,UserName,MachineName,DomainName";
        public DateTime TraceLogFileCreated
        { get; set; }
        public string UserName
        { get; set; }
        public string RunningAssembley
        { get; set; }
        public int TotalImages
        { get; set; }
        public static TraceLogger TraceLoggerInstance = null;

        protected TraceLogger() { }
        static TraceLogger()
        {
            if (TraceLoggerInstance == null)
            {
                TraceLoggerInstance = new TraceLogger();
            }
        }

        public int GetTotalImagesScannedInt(bool includeBlank)
        {
            if (includeBlank)
            {
                TotalImages *= 2;
                if ((TotalImages % 2) != 0)
                    TotalImages--;

            }
            return TotalImages;
        }
        public string GetTotalImagesScannedString(bool includeBlank)
        {
            if (includeBlank)
            {
                TotalImages *= 2;
                if ((TotalImages % 2) != 0)
                    TotalImages--;

            }
            return TotalImages.ToString();
        }
        public void StartStopStopWatch()
        {
            watch = new Stopwatch();
            watch.Start();

        }
        public string StopStopWatch()
        {
            watch.Stop();
            return watch.Elapsed.ToString();
        }
        // Begin timing.

        public void WriteTraceHeader(string traceHeader)
        {
            Trace.WriteLine(Header);
        }
        public void WriteTraceHeader()
        {
            Trace.WriteLine(Header);
        }
        public void TraceWarning(string warningMessage)
        {
            WriteTraceEntry(warningMessage, TraceType.Warning);
        }
        public void TraceError(string errorMessage)
        {
            WriteTraceEntry(errorMessage, TraceType.Error);
        }
        public void TraceInformation(string informationMessage)
        {
            WriteTraceEntry(informationMessage, TraceType.Information);
        }

        public void OpenTraceLogFile(string fileName, string listnerName)
        {
            UserName = Environment.UserName;
            Trace.Listeners.Remove(listnerName);
            Trace.AutoFlush = true;
            TextWriterTraceListener textFileListener = new TextWriterTraceListener(fileName, listnerName);
            Trace.Listeners.Add(textFileListener);
        }

        public TextWriterTraceListener OpenTextWriterTraceListener(string fileName, string listnerName)
        {
            UserName = Environment.UserName;
            Trace.Listeners.Remove(listnerName);
            Trace.AutoFlush = true;
            return new TextWriterTraceListener(fileName, listnerName);



        }
        public string GetTraceFileName(string traceFolder, string traceFileName, int daysToKeepFiles, string fileExtDel, string folderDelete)
        {
            TraceLogFileCreated = DateTime.Now;
            string traceLogFileName = Path.Combine(traceFolder, string.Format("{0}_{1}.log", traceFileName, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
            Directory.CreateDirectory(traceFolder);
            if (!(string.IsNullOrWhiteSpace(folderDelete)))
                Edocs_Utilities.EdocsUtilitiesInstance.CleanUpLogFiles(folderDelete, daysToKeepFiles, fileExtDel);
            return traceLogFileName;
        }

        private void WriteTraceEntry(string message, TraceType traceType)
        {
            if (message.Contains(","))
                message = message.Replace(",", " ");
            Trace.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", RunningAssembley, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), traceType.ToString(), message, UserName, Environment.MachineName, Environment.UserDomainName));
        }
        public void CloseTraceFile()
        {
            if (Trace.Listeners.Count > 1)
            {
                Trace.Flush();
                Trace.Close();
            }
        }

        public void TraceErrorConsole(string errorMessage)
        {
            WriteConsole(errorMessage, TraceType.Error, ConsoleColor.Red);
        }
        public void TraceWaringConsole(string errorMessage)
        {
            WriteConsole(errorMessage, TraceType.Warning, ConsoleColor.Yellow);
        }
        public void TraceInformationConsole(string errorMessage)
        {
            WriteConsole(errorMessage, TraceType.Information, ConsoleColor.Blue);
        }
        private void WriteConsole(string message, TraceType traceType, ConsoleColor consoleColor)
        {
            ConsoleColor currentColor = Console.BackgroundColor;
            Console.BackgroundColor = consoleColor;
            Console.WriteLine(string.Format("{0},{1},{2},{3}", RunningAssembley, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), traceType.ToString(), message));
            Console.BackgroundColor = currentColor;
        }
        #region IDisposable Support
        public bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Trace.Listeners.Count > 1)
                    {
                        Trace.Flush();
                        Trace.Close();
                    }
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TraceLogger()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            try
            {


                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                GC.SuppressFinalize(this);
            }
            catch { }
        }
        #endregion
    }
}
