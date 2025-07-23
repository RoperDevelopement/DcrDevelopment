using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using edl = EdocsUSA.Utilities.Logging;
namespace NYPMigration.Utilities
{
    class OpenCloseLogFile
    {
        private static OpenCloseLogFile instance = null;
        public static OpenCloseLogFile OpenCloseLogFileInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OpenCloseLogFile();
                }
                return instance;
            }
        }
        private OpenCloseLogFile()
        { }
        public async Task OpenLogFile(string logFileName,string logFilePath)
        {
            CloseTraceLog();
            EdocsUSA.Utilities.Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(logFilePath);
            OpenTraceLog(logFileName,logFilePath);
        }
        private void OpenTraceLog(string logFileName, string logFilePath)
        {
            

            logFileName = System.IO.Path.Combine(logFilePath,$"{logFileName}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.log");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Opening trace log file for trace folder:{logFileName}");
            edl.TraceLogger.TraceLoggerInstance.RunningAssembley = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe";
            edl.TraceLogger.TraceLoggerInstance.OpenTraceLogFile(logFileName,System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            edl.TraceLogger.TraceLoggerInstance.WriteTraceHeader();
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Opened trace log file:{logFileName}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyTitle:{AssemblyInfo.GetAssemblyTitle()}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyCopyright:{AssemblyInfo.GetAssemblyCopyright()}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyDescription:{AssemblyInfo.GetAssemblyDescription()}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"AssemblyVersion:{AssemblyInfo.GetAssemblyVersion()}");
        }
        public void CloseTraceLog()
        {
            edl.TraceLogger.TraceLoggerInstance.TraceInformation("Closing trace logging file");
            edl.TraceLogger.TraceLoggerInstance.CloseTraceFile();
          



          
        }
    }
}
