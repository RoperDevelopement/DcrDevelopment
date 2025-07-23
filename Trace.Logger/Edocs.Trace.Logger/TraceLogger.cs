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

        public static TraceLogger TraceLoggerInstance = null;

        protected TraceLogger() { }
        static TraceLogger()
        {
            if (TraceLoggerInstance == null)
            {
                TraceLoggerInstance = new TraceLogger();
            }
        }
        public void StartStopStopWatch()
        {
            watch = new Stopwatch();
            watch.Start();

        }
        public TimeSpan TSStopWatch()
        {
            watch.Stop();
            return watch.Elapsed;
        }
        public string StopStopWatch()
        {
            watch.Stop();
            return watch.Elapsed.ToString();
        }
        public void StartWatch()
        {
            watch = new Stopwatch();
            watch.Start();

        }
        public string StopWatch()
        {
            watch.Stop();
            return watch.Elapsed.ToString();
        }
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

        public void OpenTraceLogFile(string fileName, string listnerName,bool writeHeader)
        {
            CloseTraceFile();
            TraceLogFileCreated = DateTime.Now;
            UserName = Environment.UserName;
            TextWriterTraceListener textFileListener = OpenTextWriterTraceListener(fileName, listnerName);
            Trace.Listeners.Add(textFileListener);
            if (writeHeader)
            {
                if(!(File.Exists(fileName)))
                    WriteTraceHeader();
            }

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
            TraceLogFileCreated = DateTime.Now;
            UserName = Environment.UserName;
            Trace.Listeners.Remove(listnerName);
            Trace.AutoFlush = true;
            return new TextWriterTraceListener(fileName, listnerName);



        }
        public void DelTraceLogFiles(string traceFolderName, int daysToKeepFiles, string fileExtDel, bool delFolders)
        {
            try
            {
                CloseTraceFile();
                if (Path.HasExtension(traceFolderName))
                    traceFolderName = Path.GetDirectoryName(traceFolderName);

                var dir = new DirectoryInfo(traceFolderName);
                DateTime dateTime = DateTime.Now;

                foreach (var file in dir.GetFiles(fileExtDel, SearchOption.AllDirectories))
                {
                    TimeSpan ts = dateTime - file.LastWriteTime;
                    if (ts.Days >= daysToKeepFiles)
                    {
                        file.Delete();
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        public string GetTraceFileName(string traceFolder, string traceFileName)
        {
            //  string traceLogFileName = Path.Combine(traceFolder, string.Format("{0}_{1}.log", traceFileName, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
            CreateDirectory(traceFolder);
            string traceLogFileName = Path.Combine(traceFolder, string.Format("{0}_{1}.log", traceFileName, DateTime.Now.ToString("yyyy_MM_dd")));
            

            return traceLogFileName;
        }
        public string GetTraceFileNameAddHHMMSS(string traceFolder, string traceFileName)
        {
            //  string traceLogFileName = Path.Combine(traceFolder, string.Format("{0}_{1}.log", traceFileName, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
            CreateDirectory(traceFolder);
            string traceLogFileName = Path.Combine(traceFolder, string.Format("{0}_{1}.log", traceFileName, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));


            return traceLogFileName;
        }
        public void   OpenTraceFile(string traceFolderName, string traceFileName, int daysToKeepFiles, string fileExtDel, string folderDelete, string listnerName,bool writeHeader)
        {
            
            DelTraceLogFiles(folderDelete,daysToKeepFiles,fileExtDel,true);
            TraceLogFileCreated = DateTime.Now;
            string traceLogFileName = GetTraceFileName(traceFolderName,traceFileName);
            OpenTraceLogFile(traceLogFileName, listnerName, writeHeader);
        }
       public void OpenTraceFile(string traceFolderName, string traceFileName, string listnerName, bool writeHeader)
        {
             
            TraceLogFileCreated = DateTime.Now;
            string traceLogFileName = GetTraceFileName(traceFolderName, traceFileName);
            CreateDirectory(traceFolderName);
            OpenTraceLogFile(traceLogFileName, listnerName,writeHeader);
        }
        public void OpenTraceFileAddHHMMSS(string traceFolderName, string traceFileName, string listnerName, bool writeHeader)
        {

            TraceLogFileCreated = DateTime.Now;
            string traceLogFileName = GetTraceFileNameAddHHMMSS(traceFolderName, traceFileName);
            CreateDirectory(traceFolderName);
            OpenTraceLogFile(traceLogFileName, listnerName, writeHeader);
        }


        public string GetTraceFileName(string traceFolder, string traceFileName, int daysToKeepFiles, string fileExtDel, string folderDelete)
        {
            
            DelTraceLogFiles(folderDelete, daysToKeepFiles, fileExtDel, true);
            string traceLogFileName = GetTraceFileName(traceFolder, traceFileName);
            CreateDirectory(traceFolder);
          
            return traceLogFileName;
        }

        public void CopyTraceFiles(string sourceFolder, string destFolder, bool overWriteFile, bool delSourceFile,string fileCopyPattern)
        {
            CreateDirectory(destFolder);
            sourceFolder = CheckDirNameHasFile(sourceFolder);
            var dir = new DirectoryInfo(sourceFolder);
            foreach (var file in dir.GetFiles(fileCopyPattern, SearchOption.AllDirectories))
            {
                try
                {


                    string dFolder = $"{destFolder}{file.Name}";
                    File.Copy(file.FullName, dFolder, overWriteFile);
                    if (delSourceFile)
                        File.Delete(file.FullName);
                }
                catch { }
            }
        }
        private void WriteTraceEntry(string message, TraceType traceType)
        {
            if (message.Contains(","))
                message = message.Replace(",", " ");
            Trace.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", RunningAssembley, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), traceType.ToString(), message, UserName, Environment.MachineName, Environment.UserDomainName));
        }
        public void CloseTraceFile()
        {
            try
            {
                if (Trace.Listeners.Count > 1)
                {
                    Trace.Flush();
                    Trace.Close();
                }
            }
            catch { }
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

       public void CreateDirectory(string dirName)
        {
            dirName = CheckDirNameHasFile(dirName);
            if (!(Directory.Exists(dirName)))
                Directory.CreateDirectory(dirName);
        }
       private string CheckDirNameHasFile(string dirName)
        {
            if (Path.HasExtension(dirName))
                return Path.GetDirectoryName(dirName);
            return dirName;
        }
        public string CheckPath(string path)
        {
            if(!(path.EndsWith("\\")))
            {
                return $"{path}\\";
            }
            return path;
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

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
