//xml:L:\EdocsGitHub\Edocs.Delete.Records\XmlFile\NypDeleteRecords.xml /db:labrecs
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Edocs.HelperUtilities;
using BS = Edocs.Upload.Azure.Blob.Storage.AzureBlobStorage;
using TL = EdocsUSA.Utilities.Logging.TraceLogger;
using SE = Edocs.Send.Emails.Send_Emails;
namespace Edocs.Delete.Records
{
    class DelRecords
    {
        static int TotalRecordsDel
        {
            get;set;
        }
        static int TotalErrors
        {
            get; set;
        }
        static int TotalErrorsAllowed
        {
            get; set;
        }
      
        static int TotalRecordsToDel
        {
            get; set;
        }
        static string AzureNotDeleteFile
        { get; set; }

        static StringBuilder Sb
        { get; set; }
        static void Main(string[] args)
        {
            try
            {
                TL.TraceLoggerInstance.StartStopStopWatch();
                Init().ConfigureAwait(false).GetAwaiter().GetResult();
                OpenLogFile().ConfigureAwait(false).GetAwaiter().GetResult();
                
                if (args.Length >= 4)
                    throw new Exception("Invalid arg expecting xml file name");
                else
                    GetInputArgs(args);
            }
            catch (Exception ex)
            {
                Sb.AppendLine($"<p style={Constants.Quote}color:red{Constants.Quote}>Error running del old data {ex.Message}</p>");
                ESend().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            TL.TraceLoggerInstance.CloseTraceFile();
        }
        static async Task Init()
        {
            Sb = new StringBuilder();
            Sb.AppendLine($"<!DOCTYPE html>");
            Sb.AppendLine($"<html lang={Constants.Quote}en{Constants.Quote} xmlns={Constants.Quote}http://www.w3.org/1999/xhtml{Constants.Quote}>");
            Sb.AppendLine($"<head>");
            Sb.AppendLine($"<meta charset={Constants.Quote}utf-8{Constants.Quote}/>");
            Sb.AppendLine($"</head>");
            Sb.AppendLine($"<body style={Constants.Quote}background-color:#C0C0C0{Constants.Quote}>");
            Sb.AppendLine($"<h1 style ={Constants.Quote}text-align:center{Constants.Quote}>Delete old LabReqs</h1>");
            Sb.AppendLine($"<h2 style ={Constants.Quote}text-align:center{Constants.Quote}>Running on machine {Environment.MachineName} run time {DateTime.Now.ToString()}</h2>");
            string appDir = Utilities.GetApplicationDir();
            BS.BlobStorageInstance.AzureBlobAccountKey = Utilities.GetAppConfigSetting(Constants.AppKeyAzureBlobAccountKey);
            BS.BlobStorageInstance.AzureBlobAccountName = Utilities.GetAppConfigSetting(Constants.AppKeyAzureBlobAccountName);
            BS.BlobStorageInstance.AzureBlobStorageConnectionString = Utilities.GetAppConfigSetting(Constants.AppKeyAzureBlobStorageConnectionString);
            AzureNotDeleteFile = Utilities.GetAppConfigSetting(Constants.AppKeyAzureNotDeleteFile);
            AzureNotDeleteFile = AzureNotDeleteFile.Replace(Constants.RepStrApplicationDir, appDir);
            Utilities.CreateDirectory(AzureNotDeleteFile);
            if(Utilities.CheckFileExists(AzureNotDeleteFile))
            {
                string destFile = Path.GetDirectoryName(AzureNotDeleteFile);
                string fName = $"{Path.GetFileNameWithoutExtension(AzureNotDeleteFile)}_{DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss")}.txt";
                destFile = Path.Combine(destFile, fName);
                Utilities.CopyFile(AzureNotDeleteFile, destFile, true);
                Utilities.DeleteFile(AzureNotDeleteFile);
            }

        }
        static async Task OpenLogFile()
        {

            string logFolder = LogFolder();
            TL.TraceLoggerInstance.RunningAssembley = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}";
            TL.TraceLoggerInstance.TraceInformationConsole($"Opeing log file: {logFolder}{Utilities.GetAssemblyTitle()}");
            TL.TraceLoggerInstance.OpenTraceFileAddHHMMSS(logFolder, Utilities.GetAssemblyTitle(), Utilities.GetAssemblyTitle(), true);

            TL.TraceLoggerInstance.TraceInformation($"Ass Title {Utilities.GetAssemblyTitle()}");
            TL.TraceLoggerInstance.TraceInformation($"Ass Description {Utilities.GetAssemblyDescription()}");
            TL.TraceLoggerInstance.TraceInformation($"Ass Version {Utilities.GetAssemblyVersion()}");


        }
        static async Task WriteHtmlEnd()
        {
            TL.TraceLoggerInstance.StopStopWatch();
            Sb.AppendLine($"<br/><br/><p> Total run time {TL.TraceLoggerInstance.RunTimer.Elapsed} ms");
            Sb.AppendLine($"</body>");
            Sb.AppendLine($"</html>");
        }
        static async Task ESend()
        {
            try
            {
                string azureContaier = Utilities.GetAppConfigSetting(Constants.AppKeyAzureBlobContanierAuditShare);
        
                string emailTo = Utilities.GetAppConfigSetting(Constants.AppKeyEmailTo);
                string emailCC = Utilities.GetAppConfigSetting(Constants.AppKeyEmailCC);
                TL.TraceLoggerInstance.TraceInformation($"Sending email to {emailTo} email cc to {emailCC}");
                //string alFolder = LogFolder();
                Sb.AppendLine(UpLoadAuditLogs.UpLoadAuditLogsInstance.CopyAuditLogs(LogFolder(),azureContaier).ConfigureAwait(true).GetAwaiter().GetResult());
                WriteHtmlEnd().ConfigureAwait(true).GetAwaiter().GetResult();
                string emailSubject = $"{Utilities.GetAppConfigSetting(Constants.AppKeyEmailSubject)} run time {DateTime.Now.ToString()}";
                if(Utilities.CheckFileExists(AzureNotDeleteFile))
                    SE.EmailInstance.SendEmail(emailTo, emailCC, Sb.ToString(), emailSubject, AzureNotDeleteFile, true, string.Empty);
                else
                SE.EmailInstance.SendEmail(emailTo, emailCC, Sb.ToString(), emailSubject, string.Empty, true, string.Empty);
                Utilities.WriteTextAppend(AzureNotDeleteFile, $"Email sent email to {emailTo} email cc to {emailCC}");
               // TL.TraceLoggerInstance.TraceInformation("email sent");
            }
            catch (Exception ex)
            {
                Utilities.WriteTextAppend(AzureNotDeleteFile, $"Email not sent  {ex.Message}");

            }
        }
        static string LogFolder()
        {
            string logFolder = Utilities.CheckFolderPath(Utilities.GetAppConfigSetting(Constants.AppKeyAuditLogsFolder));

            if (string.IsNullOrWhiteSpace(logFolder))
            {
                logFolder = Utilities.CheckFolderPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                logFolder = $"{logFolder}\\Local\\DelLabReqs";
            }
            else
            {
                logFolder = Utilities.CheckFolderPath(Utilities.ReplaceString(logFolder, Constants.AppConfigKeyRepStrApplicationDir, Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)));

            }
            TL.TraceLoggerInstance.TraceInformationConsole($"Using log folder: {logFolder}");
            return Utilities.CheckFolderPath(logFolder);
        }
        static async Task GetLabReqsToDel(string xmlFile)
        {
            Sb.AppendLine($"<p style={Constants.Quote}color:#008000{Constants.Quote}>Deleting old labreqs for databases {Utilities.GetAppConfigSetting(Constants.AppKeyLabReqsDataBaseName)}</p>");
            TL.TraceLoggerInstance.TraceInformation($"Deleting old labreqs for databases {Utilities.GetAppConfigSetting(Constants.AppKeyLabReqsDataBaseName)}");
            string labReqsDBs = Utilities.GetAppConfigSetting(Constants.AppKeyLabReqsDataBaseName);
          
            foreach (string lr in labReqsDBs.Split(','))
            {
                TL.TraceLoggerInstance.TraceInformation($"Getting information for table {lr}");
                Sb.AppendLine($"<p style={Constants.Quote}color:#008000{Constants.Quote}>Getting information for table {lr}</p>");
                ModelDataBaseInfo model = GetDataBaseInfo(lr, xmlFile).ConfigureAwait(false).GetAwaiter().GetResult();
                if ((model != null) && (model.DateToDelete.Year > 2000))
                {
                    Console.WriteLine(lr);
                    IDictionary<int, ModelLabReqs> keyValues = SqlCmds.SqlCmdsInstance.GetLabReqsToDelete(model).ConfigureAwait(false).GetAwaiter().GetResult();
                    if ((keyValues != null) && (keyValues.Count() > 0))
                    {
                        Sb.AppendLine($"<p style={Constants.Quote}color:#008000{Constants.Quote}>Total records to delete {keyValues.Count().ToString()} for table {model.TableName}</p>");
                        TL.TraceLoggerInstance.TraceInformation($"Total records to delete {keyValues.Count().ToString()} for table {model.TableName}");
                        DelLabReqs(model, keyValues).ConfigureAwait(false).GetAwaiter().GetResult();
                    }

                    else
                    {
                        TL.TraceLoggerInstance.TraceWarning($"No records found for table {model.TableName} for date range {model.DateToDelete}");
                        Sb.AppendLine($"<p style={Constants.Quote}color:red{Constants.Quote}>No records found for table {model.TableName} for date range {model.DateToDelete}</p>");
                    }

                }
                else
                {
                    Sb.AppendLine($"<p style={Constants.Quote}color:red{Constants.Quote}>{lr} not found in xml file {xmlFile}</p>");
                    TL.TraceLoggerInstance.TraceInformation($"{lr} not found in xml file {xmlFile}");
                }

            }
        }
        static async Task DelLabReqs(ModelDataBaseInfo dataBaseInfo, IDictionary<int, ModelLabReqs> valuePairs)
        {
            string dirName = string.Empty;
            string fileName = string.Empty;
            TotalRecordsToDel = valuePairs.Count();
            
            foreach (KeyValuePair<int, ModelLabReqs> keyValuePair in valuePairs)
            {
                try
                {
                    dirName = Path.GetDirectoryName(keyValuePair.Value.FileUrl);
                   // fileName = Path.GetFileName(keyValuePair.Value.FileUrl);
                    string[] splitDName = dirName.Split('\\');
                    string azureCont = string.Empty;
                    for (int i = 2; i < splitDName.Length; i++)
                    {
                        azureCont += $"{splitDName[i]}/";

                    }
                    TL.TraceLoggerInstance.TraceInformationConsole($"Deleting ID {keyValuePair.Key} for database {dataBaseInfo.DataBaseName} from table {dataBaseInfo.TableName} for scandate {dataBaseInfo.DateToDelete.ToString()}");
                    TL.TraceLoggerInstance.TraceInformation($"Deleting ID {keyValuePair.Key} for database {dataBaseInfo.DataBaseName} from table {dataBaseInfo.TableName} for scandate {dataBaseInfo.DateToDelete.ToString()}");
                    SqlCmds.SqlCmdsInstance.DeleteOldRecords(dataBaseInfo, keyValuePair.Key.ToString()).ConfigureAwait(false).GetAwaiter().GetResult();
                    DeleteAzureBlobFile(keyValuePair.Value.FileUrl, azureCont).ConfigureAwait(false).GetAwaiter().GetResult();
                    TotalRecordsDel++;
                }
                catch (Exception ex)
                {
                    TotalErrors++;
                    Sb.AppendLine($"<p style={Constants.Quote}color:red{Constants.Quote}>Processing file directory name {dirName} file url {keyValuePair.Value.FileUrl} {ex.Message} </p>");
                    Utilities.WriteTextAppend(AzureNotDeleteFile, $"Error deleting file id {keyValuePair.Key} {keyValuePair.Value.FileUrl}");
                    TL.TraceLoggerInstance.TraceError($"Processing file directory name {dirName} file url {keyValuePair.Value.FileUrl}  {ex.Message} ");
                   
                }
                if(TotalErrors > TotalErrorsAllowed)
                {
                    Sb.AppendLine($"<p style={Constants.Quote}color:red{Constants.Quote}>Stopping process total errors found {TotalErrors} total erros allowed {TotalErrorsAllowed} </p>");
                    TL.TraceLoggerInstance.TraceError($"Stopping process total errors found {TotalErrors} total erros allowed {TotalErrorsAllowed}");
                    break;
                }
            }
            Sb.AppendLine($"<p style={Constants.Quote}color:red{Constants.Quote}>Total Records to Delete {TotalRecordsToDel} Total Records Deleted {TotalRecordsDel} Total Errors {TotalErrors} for table {dataBaseInfo.TableName} Delete date {dataBaseInfo.DateToDelete.ToString()}</p>");
            TL.TraceLoggerInstance.TraceInformation($"Total Records to Delete {TotalRecordsToDel} Total Records Deleted {TotalRecordsDel} Total Errors {TotalErrors} for table {dataBaseInfo.TableName} Delete date {dataBaseInfo.DateToDelete.ToString()}");

        }
        static async Task DeleteAzureBlobFile(string file,string azureContainer)
        {
            try
            {
                TL.TraceLoggerInstance.TraceInformation($"Deleting file {file} from azure container {azureContainer}");


                BS.BlobStorageInstance.DeleteAzureBlobFile(file,azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Sb.AppendLine($"<p style={Constants.Quote}color:red{Constants.Quote}>Deleting azure blob file {file} for azure container {azureContainer} {ex.Message} </p>");
                Utilities.WriteTextAppend(AzureNotDeleteFile, $"Deleting azure blob file {file} for azure container {azureContainer}");
                DeleteAzureAppenFile(file, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            
        }
        static async Task DeleteAzureAppenFile(string file, string azureContainer)
        {
            try
            {
                BS.BlobStorageInstance.DeleteAzureAppendBlobFile(file, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Sb.AppendLine($"<p style={Constants.Quote}color:red{Constants.Quote}>Deleting azure append file {file} for azure container {azureContainer} {ex.Message} </p>");
                Utilities.WriteTextAppend(AzureNotDeleteFile, $"Deleting azure append file {file} for azure container {azureContainer}");
                throw new Exception($"Deleting azure append file {file} for azure container {azureContainer} {ex.Message}");
            }

        }
        static async Task<ModelDataBaseInfo> GetDataBaseInfo(string nodeName, string xmlFile)
        {
            ModelDataBaseInfo baseInfo = new ModelDataBaseInfo();
            baseInfo.DBPassWord = HelperUtilities.Utilities.GetAppConfigSetting(Constants.AppKeySqlServerUserPw);
            baseInfo.DBUserName = HelperUtilities.Utilities.GetAppConfigSetting(Constants.AppKeySqlServerUserName);
            baseInfo.SqlServer = HelperUtilities.Utilities.GetAppConfigSetting(Constants.AppKeySqlServerName);
            baseInfo.SqlServerTimeOut = Utilities.ParseInt(HelperUtilities.Utilities.GetAppConfigSetting(Constants.AppKeySqlServerTimeOut));
            XDocument document = XDocument.Load(xmlFile);
            var query = from useInfo in document.Descendants(nodeName)
                        select useInfo;


            foreach (var record in query)
            {
                baseInfo.DataBaseName = record.Element(Constants.XmlElementDataBaseName).Value.ToString();
                baseInfo.TableName = record.Element(Constants.XmlElementTableName).Value.ToString();
                baseInfo.DeleteStoredProcedure = record.Element(Constants.XmlElementDeleteStoredProcedure).Value.ToString();
                baseInfo.GetLabRecsStoredProcedure = record.Element(Constants.XmlElementGetLabRecsToDeleteStoredProcedure).Value.ToString();
                baseInfo.NumberYrsKeep = Utilities.ParseInt(record.Element(Constants.XmlElementNumberYrsKeep).Value.ToString());
                baseInfo.NumberMonthsKeep = Utilities.ParseInt(record.Element(Constants.XmlElementNumberMonthsKeep).Value.ToString());
                baseInfo.NumberDaysKeep = Utilities.ParseInt(record.Element(Constants.XmlElementNumberDaysKeep).Value.ToString());
                if (baseInfo.NumberYrsKeep > 0)
                    baseInfo.NumberYrsKeep = baseInfo.NumberYrsKeep * -1;
                if (baseInfo.NumberMonthsKeep > 0)
                    baseInfo.NumberMonthsKeep = baseInfo.NumberMonthsKeep * -1;
                if (baseInfo.NumberDaysKeep > 0)
                    baseInfo.NumberDaysKeep = baseInfo.NumberDaysKeep * -1;

                if (baseInfo.NumberYrsKeep < 0)
                    baseInfo.DateToDelete = DateTime.Now.AddYears(baseInfo.NumberYrsKeep);
                else if (baseInfo.NumberMonthsKeep < 0)
                    baseInfo.DateToDelete = DateTime.Now.AddMonths(baseInfo.NumberMonthsKeep);
                else if (baseInfo.NumberDaysKeep < 0)
                    baseInfo.DateToDelete = DateTime.Now.AddDays(baseInfo.NumberDaysKeep);

            }
            return baseInfo;
        }

        static void GetInputArgs(string[] args)
        {
            string xmlFile = string.Empty;
            string dbName = "All";
            bool showUsage = false;
            try
            {
                foreach (string arg in args)
                {
                    if (arg.StartsWith(Constants.ArgXmlFile, StringComparison.OrdinalIgnoreCase))
                    {
                        xmlFile = arg.Substring(Constants.ArgXmlFile.Length);
                        TL.TraceLoggerInstance.TraceInformation($"Using xml file {xmlFile}");
                        continue;
                    }
                    if (arg.StartsWith(Constants.ArgDataBase, StringComparison.OrdinalIgnoreCase))
                    {
                        dbName = arg.Substring(Constants.ArgDataBase.Length);
                        TL.TraceLoggerInstance.TraceInformation($"Deleting records from database {dbName}");
                        Sb.AppendLine($"<p style={Constants.Quote}color:#008000{Constants.Quote}>Deleting records from database {dbName}</p>");
                        continue;
                    }
                    if (arg.StartsWith(Constants.ArgUsage, StringComparison.OrdinalIgnoreCase))
                    {
                        ShowUsage().ConfigureAwait(false).GetAwaiter().GetResult();
                        showUsage = true;
                        break;
                    }
                    
                }
                if (showUsage)
                    return;
                else if (string.IsNullOrWhiteSpace(xmlFile))
                    throw new Exception("Invalid arg xml file name expected");
                else if (string.Compare(dbName, Constants.LabRecDB, true) == 0)
                    GetLabReqsToDel(xmlFile).ConfigureAwait(false).GetAwaiter().GetResult();
                else if (string.Compare(dbName, Constants.BMDB, true) == 0)
                    GetLabReqsToDel(xmlFile).ConfigureAwait(false).GetAwaiter().GetResult();
                else if (string.Compare(dbName, Constants.ALDB, true) == 0)
                    GetLabReqsToDel(xmlFile).ConfigureAwait(false).GetAwaiter().GetResult();
                else if (string.Compare(dbName, Constants.AllDB, true) == 0)
                    GetLabReqsToDel(xmlFile).ConfigureAwait(false).GetAwaiter().GetResult();
                else if (string.Compare(dbName, Constants.HL7DB, true) == 0)
                    GetLabReqsToDel(xmlFile).ConfigureAwait(false).GetAwaiter().GetResult();
                else
                    throw new Exception($"Invalid dbname {dbName}");

            }
            catch (Exception ex)
            {
                ShowUsage().ConfigureAwait(false).GetAwaiter().GetResult();
                TL.TraceLoggerInstance.TraceError($"Error Deleting lab reqs {ex.Message}");
                Sb.AppendLine($"<p style={Constants.Quote}color:red{Constants.Quote}>Error Deleting lab reqs {ex.Message}</p>");
                Sb.AppendLine($"</body>");
                Sb.AppendLine($"</html>");

            }
            ESend().ConfigureAwait(false).GetAwaiter().GetResult();
        }
        private static async Task ShowUsage()
        {
            Console.WriteLine($"Usage:Edocs.Delete.Records.exe {Constants.ArgXmlFile}Xml File {Constants.ArgDataBase} database name");
            Console.WriteLine($"Usage Edocs.Delete.Records.exe {Constants.ArgXmlFile} {Utilities.GetApplicationDir()}\\NypDeleteRecords.xml");
            Console.WriteLine($"Usage Edocs.Delete.Records.exe {Constants.ArgXmlFile} {Utilities.GetApplicationDir()}\\NypDeleteRecords.xml {Constants.ArgDataBase}{Constants.LabRecDB}");
            Console.WriteLine($"Usage Edocs.Delete.Records.exe {Constants.ArgXmlFile} {Utilities.GetApplicationDir()}\\NypDeleteRecords.xml {Constants.ArgDataBase}{Constants.BMDB}");
            Console.WriteLine($"Usage Edocs.Delete.Records.exe {Constants.ArgXmlFile} {Utilities.GetApplicationDir()}\\NypDeleteRecords.xml {Constants.ArgDataBase}{Constants.HL7DB}");
            Console.WriteLine($"Usage Edocs.Delete.Records.exe {Constants.ArgXmlFile} {Utilities.GetApplicationDir()}\\NypDeleteRecords.xml {Constants.ArgDataBase}{Constants.AllDB}");
            Console.WriteLine($"Usage Edocs.Delete.Records.exe {Constants.ArgXmlFile} {Utilities.GetApplicationDir()}\\NypDeleteRecords.xml {Constants.ArgDataBase}{Constants.ALDB}");
        }
            }
}

