using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDUT = Edocs.HelperUtilities;
using ELOG = Edocs.Logger;
using ESE = Edocs.Send.Emails;
namespace Edocs.Check.Scanquire.LogFIle
{
    class CheckLogFile
    {
        static readonly string AppConfigKeyLogFoldery = "LogFolder";
        static readonly string AppConfigKeyReStartScanquire = "ReStartScanquire";
        static readonly string AppConfigKeyReStartScanquireTime = "ReStartScanquireTime";
        static readonly string AppConfigKeyProcessReSrated = "ProcessReStarted";
        static readonly string AppConfigKeyScanquireExe = "ScanquireExe";
        static readonly string AppConfigKeyStartTime = "StartTime";

        static readonly string AppConfigKeyScanquireLogFileDestFolder = "ScanquireLogFileDestFolder";
        //static readonly int ThreadSleep = 60000;
        static readonly int ThreadSleep = 60000;
        static readonly string AppConfigKeyScanquireLogFile = "ScanquireLogFile";
        static readonly string AppConfigKeyScanquireExeFolder = "ScanquireExeFolder";
        static readonly string AppConfigKeyDaysToKeepLogFiles = "DaysToKeepLogFiles";
        static readonly string AppConfigKeyEmailServer = "EmailServer";
        static readonly string AppConfigKeyEmailFrom = "EmailFrom";
        static readonly string AppConfigKeyEmailPassword = "EmailPassword";
        static readonly string AppConfigKeyEmailPort = "EmailPort";
        static readonly string AppConfigKeyEmailSubject = "EmailSubject";
        static readonly string AppConfigKeyEmailTo = "EmailTo";
        static readonly string AppConfigKeyEmailCC = "EmailCC";
        static readonly string RepStrAppDir = "{AppDataFolder}";
        
        static readonly string SearchStr = "error";
        static string destFileName = string.Empty;
        static StringBuilder SB = null;
        static void Main(string[] args)
        {
            try
            {

                string d = string.Format("{0:hh:mm:ss tt}", DateTime.Now);
                SB = new StringBuilder();
                if (CheckToRun())
                {
                    CheckScanquireLog();
                    KillProcess();
                    CleanUpFiles();
                    SendEmail();
                    ELOG.Logger.LogInstance.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Checking session log:{ex.Message}");
            }
        }
        static void CleanUpFiles()
        {
            try
            {
                EDUT.Utilities.DeleteFiles(ELOG.Logger.LogInstance.LogFileName, EDUT.Utilities.ParseInt(EDUT.Utilities.GetAppConfigSetting(AppConfigKeyDaysToKeepLogFiles)));
                EDUT.Utilities.DeleteFiles(destFileName, EDUT.Utilities.ParseInt(EDUT.Utilities.GetAppConfigSetting(AppConfigKeyDaysToKeepLogFiles)));
            }
            catch { }
        }

        static void SendEmail()
        {
            if (SB.Length > 0)
            {
                try
                {
                    SB.AppendLine($"Log file under{ELOG.Logger.LogInstance.LogFileName}");
                    SB.AppendLine($"Scanquire Log file under{destFileName}");
                    SB.AppendLine($"Scanner Computer:{Environment.MachineName} for useer:{Environment.UserName}");
                    ESE.Send_Emails.EmailInstance.EmailAttachment = destFileName;
                    ESE.Send_Emails.EmailInstance.EmailBody = SB.ToString();
                    ESE.Send_Emails.EmailInstance.EmailCC = EDUT.Utilities.GetAppConfigSetting(AppConfigKeyEmailCC);
                    ESE.Send_Emails.EmailInstance.EmailFrom = EDUT.Utilities.GetAppConfigSetting(AppConfigKeyEmailFrom);
                    ESE.Send_Emails.EmailInstance.EmailPassord = EDUT.Utilities.GetAppConfigSetting(AppConfigKeyEmailPassword);
                    ESE.Send_Emails.EmailInstance.EmailPort = EDUT.Utilities.ParseInt(EDUT.Utilities.GetAppConfigSetting(AppConfigKeyEmailPort));
                    ESE.Send_Emails.EmailInstance.EmailServer = EDUT.Utilities.GetAppConfigSetting(AppConfigKeyEmailServer);
                    ESE.Send_Emails.EmailInstance.EmailSubject = EDUT.Utilities.GetAppConfigSetting(AppConfigKeyEmailSubject).Replace("{dateTime}", DateTime.Now.ToString());
                    ESE.Send_Emails.EmailInstance.EmailTo = EDUT.Utilities.GetAppConfigSetting(AppConfigKeyEmailTo);
                    ESE.Send_Emails.EmailInstance.SendEmail(false);
                }
                catch (Exception ex)
                {
                    ELOG.Logger.LogInstance.WriteLoggingLogFile($"Sending email:{ex.Message}", false, ELOG.LoggingErrorType.Error);
                }
            }
        }
        static bool CheckToRun()
        {
            if ((DateTime.Now.DayOfWeek == DayOfWeek.Saturday) || (DateTime.Now.DayOfWeek == DayOfWeek.Sunday))
                    return false;
            int currTime = EDUT.Utilities.ParseInt(DateTime.Now.ToString("HH"));
            bool reStarted = EDUT.Utilities.GetBool(EDUT.Utilities.GetAppConfigSetting(AppConfigKeyProcessReSrated));
            int startTime = EDUT.Utilities.ParseInt(EDUT.Utilities.GetAppConfigSetting(AppConfigKeyStartTime));
            if ((startTime >= currTime))
            {
                if (reStarted)
                    EDUT.Utilities.UpDateApplicationKey(AppConfigKeyProcessReSrated, "false");
                return true;
            }
            return false;

        }
        static void KillProcess()
        {
            try
            {
                

                if (EDUT.Utilities.GetBool(EDUT.Utilities.GetAppConfigSetting(AppConfigKeyReStartScanquire)))
                {
                    int restartTime = EDUT.Utilities.ParseInt(EDUT.Utilities.GetAppConfigSetting(AppConfigKeyReStartScanquireTime));
                    int currTime = EDUT.Utilities.ParseInt(DateTime.Now.ToString("HH"));
                    bool reStarted = EDUT.Utilities.GetBool(EDUT.Utilities.GetAppConfigSetting(AppConfigKeyProcessReSrated));

                    ELOG.Logger.LogInstance.WriteLoggingLogFile($"Restart process time {restartTime.ToString()} current time:{currTime.ToString()} restarted on machine:{Environment.MachineName} usename:{Environment.UserName}", false, ELOG.LoggingErrorType.Warning);

                    if (currTime >= restartTime)
                    {

                        if (!(reStarted))
                        {
                            if (EDUT.Utilities.TaskRunning(EDUT.Utilities.GetAppConfigSetting(AppConfigKeyScanquireExe)))
                            {
                                ELOG.Logger.LogInstance.WriteLoggingLogFile($"Shuttdown Process {EDUT.Utilities.GetAppConfigSetting(AppConfigKeyScanquireExe)} restarted on machine:{Environment.MachineName} usename:{Environment.UserName}", false, ELOG.LoggingErrorType.Warning);
                                EDUT.Utilities.EndTask(EDUT.Utilities.GetAppConfigSetting(AppConfigKeyScanquireExe));
                                System.Threading.Thread.Sleep(ThreadSleep);
                                if (EDUT.Utilities.TaskRunning(EDUT.Utilities.GetAppConfigSetting(AppConfigKeyScanquireExe)))
                                    throw new Exception($"Process not stopped:{EDUT.Utilities.GetAppConfigSetting(AppConfigKeyScanquireExe)}");
                                ELOG.Logger.LogInstance.WriteLoggingLogFile($"Process {EDUT.Utilities.GetAppConfigSetting(AppConfigKeyScanquireExe)} restarted on machine:{Environment.MachineName} usename:{Environment.UserName}", false, ELOG.LoggingErrorType.Info);
                                EDUT.Utilities.UpDateApplicationKey(AppConfigKeyProcessReSrated, "true");
                                EDUT.Utilities.RunTask(EDUT.Utilities.GetAppConfigSetting(AppConfigKeyScanquireExe), string.Empty, EDUT.Utilities.GetAppConfigSetting(AppConfigKeyScanquireExeFolder), false);
                                System.Threading.Thread.Sleep(ThreadSleep);
                                if (!(EDUT.Utilities.TaskRunning(EDUT.Utilities.GetAppConfigSetting(AppConfigKeyScanquireExe))))
                                    throw new Exception($"Process not started:{EDUT.Utilities.GetAppConfigSetting(AppConfigKeyScanquireExe)}");
                                ELOG.Logger.LogInstance.WriteLoggingLogFile($"Process started {EDUT.Utilities.GetAppConfigSetting(AppConfigKeyScanquireExe)} restarted on machine:{Environment.MachineName} usename:{Environment.UserName}", false, ELOG.LoggingErrorType.Info);
                                SB.AppendLine($"Process restarted {EDUT.Utilities.GetAppConfigSetting(AppConfigKeyScanquireExe)} restarted on machine:{Environment.MachineName} usename:{Environment.UserName}");
                            }
                            else
                                ELOG.Logger.LogInstance.WriteLoggingLogFile($"Process {EDUT.Utilities.GetAppConfigSetting(AppConfigKeyScanquireExe)}not running for machine:{Environment.MachineName} usename:{Environment.UserName}", false, ELOG.LoggingErrorType.Info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ELOG.Logger.LogInstance.WriteLoggingLogFile($"Killing Process {EDUT.Utilities.GetAppConfigSetting(AppConfigKeyScanquireExe)} Log File Under:{ELOG.Logger.LogInstance.LogFileName} for machine:{Environment.MachineName} usename:{Environment.UserName} message:{ex.Message}", false, ELOG.LoggingErrorType.Error);
                SB.AppendLine($"Error message:{ex.Message}");
            }
        }
        static void CheckScanquireLog()
        {
            //     ELOG.Logger.LogInstance.LogFileName = "c:\\temp\\ttt.o";

            try
            {
                // Utilities.CheckFolderPath(HelperUtilities.Utilities.GetAppConfigSetting(AppConfigKeyLogFoldery));
                ELOG.Logger.LogInstance.LogFileName = EDUT.Utilities.CheckFolderPath(EDUT.Utilities.GetAppConfigSetting(AppConfigKeyLogFoldery));
                ELOG.Logger.LogInstance.LogFileName = EDUT.Utilities.ReplaceString(ELOG.Logger.LogInstance.LogFileName, RepStrAppDir, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                ELOG.Logger.LogInstance.LogFileName = $"{ELOG.Logger.LogInstance.LogFileName}Session_Log_{DateTime.Now.ToString("yyyy_MM_dd")}.log";
                string sessionLogFile = EDUT.Utilities.ReplaceString(EDUT.Utilities.GetAppConfigSetting(AppConfigKeyScanquireLogFile), RepStrAppDir, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
                ELOG.Logger.LogInstance.WriteLoggingLogFile($"Starting checking scanquire log file:{sessionLogFile} for machine:{Environment.MachineName} usename:{Environment.UserName}", false, Logger.LoggingErrorType.Info);
                if (EDUT.Utilities.CheckFileExists(sessionLogFile))
                {
                    string destSennsionName = $"Session_Log_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.log";
                    destFileName = EDUT.Utilities.CheckFolderPath(EDUT.Utilities.GetAppConfigSetting(AppConfigKeyScanquireLogFileDestFolder));
                    destFileName = EDUT.Utilities.ReplaceString(destFileName, RepStrAppDir, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                    EDUT.Utilities.CreateDirectory(destFileName);
                    destFileName = $"{destFileName}{destSennsionName}";
                    EDUT.Utilities.CopyFile(sessionLogFile, destFileName, false);
                    if (EDUT.Utilities.CheckFileExists(destFileName))
                    {
                        string strSessionFile = EDUT.Utilities.ReadFile(destFileName);
                        if (!(string.IsNullOrWhiteSpace(strSessionFile)))
                        {
                            if (strSessionFile.ToLower().Contains(SearchStr))
                                ELOG.Logger.LogInstance.WriteLoggingLogFile($"Errors found in file:{destFileName}", false, ELOG.LoggingErrorType.Error);
                         //       throw new Exception($"Errors found in file:{destFileName}");
                        }
                        else
                            throw new Exception($"No Data found in file:{destFileName}");
                    }
                    else
                        throw new Exception($"Dest file not found:{destFileName} copyied from souce file name:{sessionLogFile}");
                }
                else
                    ELOG.Logger.LogInstance.WriteLoggingLogFile($"Session File:{sessionLogFile} not found",false,ELOG.LoggingErrorType.Warning);
            }
            catch (Exception ex)
            {
                ELOG.Logger.LogInstance.WriteLoggingLogFile($"Checking session log file for machine:{Environment.MachineName} usename:{Environment.UserName} message:{ex.Message}", false, ELOG.LoggingErrorType.Error);
                SB.AppendLine($"Error Checking session log file mssage:{ex.Message}");
            }
        }
    }
}
