using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Edocs.Upload.Azure.Blob.Storage;
using System.Reflection;
using System.Security.Authentication;
using EDocs.Nyp.LabReqs.AppServices.Identity;
using System.Security.Claims;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using Microsoft.AspNetCore.Http;
namespace EDocs.Nyp.LabReqs.AppService.Logging
{
    public class AuditLogs : ILog
    {
        
        private readonly string AuditLogHeader = "Application Name,Log Time,Message Type,Message,UserName,MachineName,DomainName";
        public string UserName
        { get; set; }
        public string AppName
        { get; set; }

        public string AzureLogFileName
        { get; set; }
        public string AzureStorageBlobContainer
        { get; set; }
        public void GetLogFileName(string logFileName)
        {
            AzureLogFileName = $"{logFileName}_{DateTime.Now.ToString("MM_dd_yyyy")}.log";
        }
        public void TryLoggingMessagaAgain(string message)
        {
            try
            {
                message = message.Replace(",", "-");
                message = $"{AppName},{DateTime.Now.ToLocalTime()},Error,{message},{UserName},{Environment.MachineName},{Environment.UserDomainName}";
                WriteText(message).GetAwaiter().GetResult();
            }
            catch { }
        }
        public void LogWarning(string message)
        {
            // CreateLoggingDir(AzureLogFileName);
            try
            {
                message = message.Replace(",", "-");
                message = $"{AppName},{DateTime.Now.ToLocalTime()},Warning,{message},{UserName},{Environment.MachineName},{Environment.UserDomainName}";
                WriteText(message).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                message = $"Error Logging message {message} {ex.Message}";
                TryLoggingMessagaAgain(message);
            }
        }
        public void LogInformation(string message)
        {
            //CreateLoggingDir(AzureLogFileName);
            try
            {


                message = message.Replace(",", "-");
                message = $"{AppName},{DateTime.Now.ToLocalTime()},Information,{message},{UserName},{Environment.MachineName},{Environment.UserDomainName}";
                WriteText(message).GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {
                message = $"Error Logging message {message} {ex.Message}";
                TryLoggingMessagaAgain(message);
            }
        }
        public async Task WriteText(string text)
        {
            AzureBlobStorage.BlobStorageInstance.WriteAzureBlob(AzureLogFileName, AzureStorageBlobContainer, text, AuditLogHeader);
        }
        public void LogError(string message)
        {
            try
            {
                //CreateLoggingDir(AzureLogFileName);
                message = message.Replace(",", " ");
                message = $"{AppName},{DateTime.Now.ToLocalTime()},Error,{message},{UserName},{Environment.MachineName},{Environment.UserDomainName}";
                WriteText(message).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                message = $"Error Logging message {message} {ex.Message}";
                TryLoggingMessagaAgain(message);
            }
        }
        public void CreateLoggingDir(string logDir)
        {

            if (Path.HasExtension(logDir))
                logDir = Path.GetDirectoryName(logDir);
            if (!(Directory.Exists(logDir)))

                Directory.CreateDirectory(logDir);
            if (!(Directory.Exists(logDir)))
                throw new Exception($"Logging foldeer {logDir} noit found");
        }

    }
    public static class InitAuditLogs
    {
        private const string appName = "Nyp_LabReqs";
        public static async Task<ILog> LogAsync(ILog log, ISession session)
        {
            log.AppName = appName;
            log.UserName = await GetSessionVariables.SessionVarInstance.GetSessionVariable(session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(log.UserName))
                log.UserName = "Edocs";
            log.GetLogFileName($"Nyp_LabReqs_AuditLog_{log.UserName}_{log.AppName}");
           
            AzureBlobStorage.BlobStorageInstance.InitIConfiguration();
            //  log.AzureStorageBlobContainer = $"{AzureBlobStorage.BlobStorageInstance.AzureBlobShareAuditLogsUpLoad}/{DateTime.Now.ToString("MM_dd_yyyy")}";
            log.AzureStorageBlobContainer = $"{AzureBlobStorage.BlobStorageInstance.AzureBlobShareAuditLogsUpLoad}/";
            return log;
        }
    }
}
