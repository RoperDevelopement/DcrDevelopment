using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using BinMonitorAppService.ApiClasses;
namespace BinMonitorAppService.Logging
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
                message = $"{AppName},{DateTime.Now},Error,{message},{UserName},{Environment.MachineName},{Environment.UserDomainName}";
                WriteText(message).GetAwaiter().GetResult();
            }
            catch { }
        }
        public void LogWarning(string message)
        {
            
            try
            {
            // CreateLoggingDir(AzureLogFileName);
            message = message.Replace(",", "-");
            message = $"{AppName},{DateTime.Now},Warning,{message},{UserName},{Environment.MachineName},{Environment.UserDomainName}";
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
            try
            { 
            //CreateLoggingDir(AzureLogFileName);
            message = message.Replace(",", "-");
            message = $"{AppName},{DateTime.Now},Information,{message},{UserName},{Environment.MachineName},{Environment.UserDomainName}";
            WriteText(message).GetAwaiter().GetResult();
            }
            catch(Exception ex)
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

                message = message.Replace(",", " ");
                message = $"{AppName},{DateTime.Now},Error,{message},{UserName},{Environment.MachineName},{Environment.UserDomainName}";
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
        private static System.Diagnostics.Stopwatch watch = null;
        private const string appName = "Nyp_BinMonitor";
        public static async Task<ILog> LogAsync(ILog log, ISession session)
        {
            
        log.UserName = await GetSessionVariables.SessionVarInstance.GetSessionVariable(session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(log.UserName))
                log.UserName = "Edocs";
            log.AppName = appName;
            log.GetLogFileName($"Nyp_AuditLog_{log.AppName}_{log.UserName}");


            AzureBlobStorage.BlobStorageInstance.InitIConfiguration();
            //  log.AzureStorageBlobContainer = $"{AzureBlobStorage.BlobStorageInstance.AzureBlobShareAuditLogsUpLoad}/{DateTime.Now.ToString("MM_dd_yyyy")}";
            log.AzureStorageBlobContainer = $"{AzureBlobStorage.BlobStorageInstance.AzureBlobShareAuditLogsUpLoad}/";
            return log;
        }
        public static void StartStopWatch()
        {
            watch = new System.Diagnostics.Stopwatch();
            watch.Start();
        }
        public static string StopStopWatch()
        {
            try
            {
                watch.Stop();
                return ($"{watch.ElapsedMilliseconds} ms");
            }
            catch(Exception ex)
            {
                return $"Error:Could not get end time {ex.Message}";
            }
            
        }

    }
}
