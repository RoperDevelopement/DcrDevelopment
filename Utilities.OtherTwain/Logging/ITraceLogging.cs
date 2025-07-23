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
        void OpenTraceLogFile(string fileName, string listnerName);

        void TraceErrorConsole(string errorMessage);
        void TraceWaringConsole(string errorMessage);
        void TraceInformationConsole(string errorMessage);
        DateTime TraceLogFileCreated
        { get; set; }

        int GetTotalImagesScannedInt(bool includeBlank);
        string GetTotalImagesScannedString(bool includeBlank);


        void CloseTraceFile();
        int TotalImages
        { get; set; }
        string GetTraceFileName(string traceFolder, string traceFileName, int daysToKeepFiles, string fileExtDel, string folderDelete);
    }
}
