using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace EdocsUSA.Utilities.Logging
{
    public interface ITraceLogging
    {
        void TraceWarning(string warningMessage);
        void TraceError(string errorMessage);
        void TraceInformation(string informationMessage);
        void WriteTraceHeader(string traceHeader);
        TextWriterTraceListener OpenTextWriterTraceListener(string fileName, string listnerName);
        void OpenTraceLogFile(string fileName, string listnerName, bool writeHeader);

        void TraceErrorConsole(string errorMessage);
        void TraceWaringConsole(string errorMessage);
        void TraceInformationConsole(string errorMessage);
        void OpenTraceFile(string traceFolderName,string traceFileName,int daysToKeepFiles, string fileExtDel, string folderDelete, string listnerName, bool writeHeader);
        void OpenTraceFile(string traceFolderName, string traceFileName,string listnerName, bool writeHeader);
        string GetTraceFileName(string traceFolder, string traceFileName, int daysToKeepFiles, string fileExtDel, string folderDelete);
        string GetTraceFileName(string traceFolder, string traceFileName);
        void DelTraceLogFiles(string traceFolderName, int daysToKeepFiles, string fileExtDel,bool delFolders);
        void CloseTraceFile();
        void CreateDirectory(string dirName);
        string CheckPath(string path);
        void StartStopStopWatch();
        string StopStopWatch();
        void StartWatch();
        TimeSpan TSStopWatch();
        string StopWatch();
        void CopyTraceFiles(string sourceFolder, string destFolder, bool overWriteFile, bool delSourceFile, string fileCopyPattern);
    }
}
