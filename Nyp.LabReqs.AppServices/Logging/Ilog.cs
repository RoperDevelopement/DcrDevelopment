using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDocs.Nyp.LabReqs.AppService.Logging
{
   public interface ILog
    {
        
        void LogWarning(string message);
        void LogInformation(string message);
        void LogError(string message);
        void CreateLoggingDir(string logDir );
        string UserName
        { get; set; }
        string AzureStorageBlobContainer
        { get; set; }
        void GetLogFileName(string logFIleName);

        string AzureLogFileName
        { get; set; }
        
        string AppName
        { get; set; }
        void TryLoggingMessagaAgain(string message);
    }
}
