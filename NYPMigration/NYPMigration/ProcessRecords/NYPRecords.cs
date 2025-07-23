using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NYPMigration.Models;
using NYPMigration.SQLCmds;
using NYPMigration.Utilities;
using EdocsUSA.Utilities;
using edl = EdocsUSA.Utilities.Logging;
namespace NYPMigration.ProcessRecords
{
    public class NYPRecords
    {
        private static NYPRecords instance = null;
        public static NYPRecords NYPRecordsInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NYPRecords();
                }
                return instance;
            }
        }
        private NYPRecords()
        { }
        private async Task<string> GetRecordsToMigrate(string sqlConn, string spName)
        {
            // edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Checking SP {PropertiesConst.PropertiesConstInstance.SPGetMigrationLabReqsScanDate} is running ");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Getting records to migrate for sp {spName}");
            string retStr = string.Empty;
            for (int i = 0; i < PropertiesConst.PropertiesConstInstance.NumberTimesLoop; i++)
            {
                retStr = GetNYPRecords.NYPRecordsInstance.GetJsonResults(sqlConn, spName).ConfigureAwait(false).GetAwaiter().GetResult();
                if (!(string.IsNullOrWhiteSpace(retStr)))
                    break;
                System.Threading.Thread.Sleep(PropertiesConst.PropertiesConstInstance.ThreadSleepSecs);
            }
            return retStr;
        }

        public async Task<bool> GetNypLabReqs()
        {
            DateTime startTime = DateTime.Now;
            
            PropertiesConst.PropertiesConstInstance.TotalErrors = 0;
            PropertiesConst.PropertiesConstInstance.MigrationErrors = new List<string>();
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Getting LabReqs");
            edl.TraceLogger.TraceLoggerInstance.StartStopStopWatch();
            string labReqs = GetRecordsToMigrate(PropertiesConst.PropertiesConstInstance.SQLServer, PropertiesConst.PropertiesConstInstance.SPGetMigrationLabReqsScanDate).ConfigureAwait(false).GetAwaiter().GetResult();
            EmailHtmlFile emailHtmlFile = new EmailHtmlFile();
            if (string.IsNullOrWhiteSpace(labReqs))
            {
                PropertiesConst.PropertiesConstInstance.MigrationErrors.Add($"Could not get labreqs for sp PropertiesConst.PropertiesConstInstance.SPGetMigrationLabReqsScanDat");
                PropertiesConst.PropertiesConstInstance.TotalErrors++;
                emailHtmlFile.SendEmail(DateTime.Now.ToString("MM-dd-yyyy"), "LabReqs").ConfigureAwait(false).GetAwaiter().GetResult();

                edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Could not get labreqs for sp PropertiesConst.PropertiesConstInstance.SPGetMigrationLabReqsScanDat");
                return true;
            }
            var readTask = JsonConvert.DeserializeObject<NYPLabReqsModel[]>(labReqs);
            Utilities.DownLoadPDFFiles downLoadPDF = new DownLoadPDFFiles();
            IDictionary<int, NYPLabReqsModel> dicLabReqs = readTask.ToDictionary(k => k.ID);
            var firstValue = dicLabReqs.First();
            string scanDate = firstValue.Value.ScanDate.ToString("MM-dd");
            string scanYear = firstValue.Value.ScanDate.ToString("yyyy");
            OpenCloseLogFile.OpenCloseLogFileInstance.OpenLogFile($"Labreqs_ScanDate_{scanYear}_{scanDate}", System.IO.Path.Combine($"{PropertiesConst.PropertiesConstInstance.NYPMigrationLogFolder}\\LabReqs", DateTime.Now.ToString("MM-dd-yyyy"))).ConfigureAwait(false).GetAwaiter().GetResult();
            string labReqsPDFSf = System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.NYPSaveFolder, $"LabReqs\\{scanYear}\\{scanDate}\\");
            string pdfSaveFname = PropertiesConst.PropertiesConstInstance.GetSaveFolder(labReqsPDFSf);
            int totalRecs = dicLabReqs.Count();
            int totalPRocess = 0;
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total records to process {totalRecs} for scandate {scanDate}-{scanYear}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total records to process {totalRecs} for scandate {scanDate}-{scanYear}"); ;
            Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(labReqsPDFSf);
            foreach (var kvp in dicLabReqs)
            {
                try
                {
                    bool recordAdded = false;
                    string azureContainer = string.Empty;
                    string downloadFName = string.Empty;
                    PropertiesConst.PropertiesConstInstance.WriteInformation($"Processing file {kvp.Value.FileUrl.ToString()}");
                    PropertiesConst.PropertiesConstInstance.GetAzureCont(new Uri(kvp.Value.FileUrl), ref azureContainer, ref downloadFName);
                    PropertiesConst.PropertiesConstInstance.WriteInformation($"Saving file {downloadFName} for ID {kvp.Value.ID}");

                    string labRPDFSf = $"{labReqsPDFSf}{System.IO.Path.GetFileName(downloadFName)}";
                    downLoadPDF.DownloadFile(azureContainer.Replace("/", "").Trim(), downloadFName, labRPDFSf).ConfigureAwait(false).GetAwaiter().GetResult();
                    PropertiesConst.PropertiesConstInstance.WriteInformation($"Adding records to Sqlite table LabReqisitions {System.IO.Path.Combine(Utilities.PropertiesConst.PropertiesConstInstance.SqlLightDatabase, PropertiesConst.PropertiesConstInstance.SqliteNypLabReqsDB)} for pdf file {kvp.Value.FileName} id {kvp.Value.ID}");
                    for (int i = 0; i < Utilities.PropertiesConst.PropertiesConstInstance.NumberTimesLoop; i++)
                    {
                        int results = SqlLiteCmds.SqlLiteCmdsInstance.AddLabReqs(System.IO.Path.GetDirectoryName(pdfSaveFname), kvp.Value, PropertiesConst.PropertiesConstInstance.SqliteNypLabReqsDB, "LabReqisitions").ConfigureAwait(false).GetAwaiter().GetResult();

                        if (results == 1)
                        {
                            dicLabReqs[kvp.Key].FileUrl = System.IO.Path.GetDirectoryName(pdfSaveFname);
                            recordAdded = true;
                            break;
                        }
                        System.Threading.Thread.Sleep(PropertiesConst.PropertiesConstInstance.ThreadSleepSecs);
                        PropertiesConst.PropertiesConstInstance.WriteWarnings($"Record ID not added {kvp.Value.ID} for file {kvp.Value.FileName} number time retrying {i}");


                    }
                    if (!(recordAdded))
                    {
                        PropertiesConst.PropertiesConstInstance.UpdateErrors($"Could not add record {kvp.Value.ID} filename {kvp.Value.FileName} to SqlLite for labreqs");

                    }
                    else
                    {
                        PropertiesConst.PropertiesConstInstance.WriteInformation($"Updated table LabReqisitions for migrated for id {kvp.Value.ID}");
                        LRecsSqlCmds.LRecsSqlCmdsInstance.UpdateMigratedLabReqs(PropertiesConst.PropertiesConstInstance.SQLServer, "LabReqisitions", kvp.Value.ID).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    totalPRocess++;
                    totalRecs--;
                    PropertiesConst.PropertiesConstInstance.WriteInformation($"Total Records to Left process {totalRecs}");
                    PropertiesConst.PropertiesConstInstance.WriteInformation($"Total Records processed {totalPRocess}");
                    PropertiesConst.PropertiesConstInstance.WriteInformation($"Total errors found   {PropertiesConst.PropertiesConstInstance.TotalErrors}");
                    System.Threading.Thread.Sleep(PropertiesConst.PropertiesConstInstance.ThreadSleepSqlite);

                    if (PropertiesConst.PropertiesConstInstance.TotalErrors > PropertiesConst.PropertiesConstInstance.MaxErrors)
                    {
                        PropertiesConst.PropertiesConstInstance.UpdateErrors($"Total Errors for LabReqisitions {PropertiesConst.PropertiesConstInstance.TotalErrors} Max errors set at { PropertiesConst.PropertiesConstInstance.MaxErrors}");
                        break;
                    }
                    PropertiesConst.PropertiesConstInstance.TotalLabRecsProcessed++;
                    DateTime endTime = DateTime.Now; // Current date and time

                    // Calculate the difference
                    TimeSpan totalTime = endTime - startTime;
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Running Time:  {totalTime.ToString(@"hh\:mm\:ss")}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Running Time:  {totalTime.ToString(@"hh\:mm\:ss")}");
                   
                }
                catch (Exception ex)
                {
                    PropertiesConst.PropertiesConstInstance.UpdateErrors($"Error found for table LabReqisitions for migrated for id {kvp.Value.ID} {ex.Message}");
                }
            }

            string jsonfile = JsonConvert.SerializeObject(dicLabReqs, Formatting.Indented);
            PropertiesConst.PropertiesConstInstance.WriteInformation($"Saving Json File {System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.LabReqsJasonFolder, $"LabReqs_{scanYear}_{scanDate}_records.json")}");
            System.IO.File.AppendAllText($"{System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.LabReqsJasonFolder, $"LabReqs_{scanYear}_{scanDate}_records.json")}", jsonfile);
            PropertiesConst.PropertiesConstInstance.WriteInformation($"Total records process {totalRecs} for scandate {scanDate}-{scanYear}");
            GetNYPRecords.NYPRecordsInstance.UpDateNYPLaReqsMigrated(PropertiesConst.PropertiesConstInstance.SQLServer, PropertiesConst.PropertiesConstInstance.SPUpDateNYPLabReqsMigration, totalPRocess, $"{scanYear}-{scanDate}").ConfigureAwait(false).GetAwaiter().GetResult();
            LRecsSqlCmds.LRecsSqlCmdsInstance.UpdateRecordsProcessed(PropertiesConst.PropertiesConstInstance.SQLServer, "LabReqisitions", totalPRocess, PropertiesConst.PropertiesConstInstance.TotalErrors).ConfigureAwait(false).GetAwaiter().GetResult();
            if (PropertiesConst.PropertiesConstInstance.TotalErrors != 0)
            {

                emailHtmlFile.SendEmail($"{scanDate}_{scanYear}", "LabReqs", true).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            PropertiesConst.PropertiesConstInstance.WriteInformation($"Total time {edl.TraceLogger.TraceLoggerInstance.StopStopWatch()} to process records process {totalPRocess} for scandate {scanDate}-{scanYear}");
            PropertiesConst.PropertiesConstInstance.WriteInformation($"Total labrecs processed {PropertiesConst.PropertiesConstInstance.TotalLabRecsProcessed}");
            OpenCloseLogFile.OpenCloseLogFileInstance.CloseTraceLog();
            return false;
        }

        public async Task GetLabReqsPatientID()
        {
            Console.Clear();
            PropertiesConst.PropertiesConstInstance.MigrationErrors = new List<string>();
            EmailHtmlFile emailHtmlFile = new EmailHtmlFile();

            edl.TraceLogger.TraceLoggerInstance.StartStopStopWatch();
            OpenCloseLogFile.OpenCloseLogFileInstance.OpenLogFile($"Patient_Records", System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.NYPMigrationLogFolder, "PatientRecords")).ConfigureAwait(false).GetAwaiter().GetResult();
            PropertiesConst.PropertiesConstInstance.WriteInformation($"Getting Patient Records");
            string labReqsPatientID = GetRecordsToMigrate(PropertiesConst.PropertiesConstInstance.SQLServer, PropertiesConst.PropertiesConstInstance.SPGetLabReqsPatientID).ConfigureAwait(false).GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(labReqsPatientID))
            {
                PropertiesConst.PropertiesConstInstance.UpdateErrors($"Could not get labReqs PatientIDs for sp {PropertiesConst.PropertiesConstInstance.SPGetLabReqsPatientID}");
                emailHtmlFile.SendEmail(DateTime.Now.ToString("MM-dd-yyyy"), "Paitents").ConfigureAwait(false).GetAwaiter().GetResult();
                OpenCloseLogFile.OpenCloseLogFileInstance.CloseTraceLog();
                return;
            }
            //string labReqsPatientID = GetNYPRecords.NYPRecordsInstance.GetJsonResults(PropertiesConst.PropertiesConstInstance.SQLServer, PropertiesConst.PropertiesConstInstance.SPGetLabReqsPatientID).ConfigureAwait(false).GetAwaiter().GetResult();
            //GetNYPRecords.NYPRecordsInstance.ChangeSpGetLabReqs(PropertiesConst.PropertiesConstInstance.SQLServer, 0).ConfigureAwait(false).GetAwaiter().GetResult();
            // IDictionary<int, NYPLabReqs> dicLabReqs = new Dictionary<int, NYPLabReqs>();
            var readTask = JsonConvert.DeserializeObject<LabReqsPatientIDModel[]>(labReqsPatientID);

            IDictionary<string, LabReqsPatientIDModel> dicLabReqsPatID = readTask.ToDictionary(k => k.PatientID);
            int totalRecs = dicLabReqsPatID.Count();
            int totRecsProcessed = 0;
            PropertiesConst.PropertiesConstInstance.WriteInformation($"Total Records to process {totalRecs}");
            foreach (var kvp in dicLabReqsPatID)
            {
                try
                {


                    bool recordAdded = false;
                    for (int i = 0; i < Utilities.PropertiesConst.PropertiesConstInstance.NumberTimesLoop; i++)
                    {
                        PropertiesConst.PropertiesConstInstance.WriteInformation($"Adding PatientID Record {kvp.Value.PatientID}");

                        int results = SqlLiteCmds.SqlLiteCmdsInstance.AddLabReqsPatID(kvp.Value, PropertiesConst.PropertiesConstInstance.SqliteNypLabReqsDB).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (results == 1)
                        {
                            recordAdded = true;
                            break;
                        }
                        System.Threading.Thread.Sleep(PropertiesConst.PropertiesConstInstance.ThreadSleepSecs);
                        PropertiesConst.PropertiesConstInstance.WriteWarnings($"Record not added  number time retrying {i}");

                    }
                    if (!(recordAdded))
                    {
                        PropertiesConst.PropertiesConstInstance.UpdateErrors($"Could not add Patient ID {kvp.Value.PatientID} to Sqlite Tale");
                        //throw new Exception($"Could not add record Patient ID {kvp.Value.PatientID}");
                    }
                    else
                    {
                        PropertiesConst.PropertiesConstInstance.WriteInformation($"Updated table Patient for migrated for id {kvp.Value.PatientID}");

                        LRecsSqlCmds.LRecsSqlCmdsInstance.UpdateMigratedPatID(PropertiesConst.PropertiesConstInstance.SQLServer, "Patient", kvp.Value.PatientID).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    totRecsProcessed++;
                    PropertiesConst.PropertiesConstInstance.WriteInformation($"Total Records to Left process {totalRecs--}");

                    PropertiesConst.PropertiesConstInstance.WriteInformation($"Total Records process {totRecsProcessed}");


                    if (PropertiesConst.PropertiesConstInstance.TotalErrors > PropertiesConst.PropertiesConstInstance.MaxErrors)
                    {
                        PropertiesConst.PropertiesConstInstance.UpdateErrors($"Total Errors for MaintenanceLogs {PropertiesConst.PropertiesConstInstance.TotalErrors} Max errors set at { PropertiesConst.PropertiesConstInstance.MaxErrors}");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    PropertiesConst.PropertiesConstInstance.UpdateErrors($"Migratng PatientID for migrated for patid {kvp.Value.PatientID} {ex.Message}");
                }
            }
            if (PropertiesConst.PropertiesConstInstance.TotalErrors != 0)
            {
                emailHtmlFile.SendEmail(DateTime.Now.ToString("MM-dd-yyyy"), "Patient", true).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            PropertiesConst.PropertiesConstInstance.WriteInformation($"Total time {edl.TraceLogger.TraceLoggerInstance.StopStopWatch()} to process patient id records process {totRecsProcessed}");

            OpenCloseLogFile.OpenCloseLogFileInstance.CloseTraceLog();
        }
        public async Task GetMaintenanceLogs()
        {
            EmailHtmlFile emailHtmlFile = new EmailHtmlFile();
            PropertiesConst.PropertiesConstInstance.TotalErrors = 0;
            PropertiesConst.PropertiesConstInstance.MigrationErrors = new List<string>();
            edl.TraceLogger.TraceLoggerInstance.StartStopStopWatch();
            OpenCloseLogFile.OpenCloseLogFileInstance.OpenLogFile($"MaintenanceLogs_Records", System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.NYPMigrationLogFolder, "MaintenanceLogs")).ConfigureAwait(false).GetAwaiter().GetResult();
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting MaintenanceLogs Records");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Getting MaintenanceLogs Records");
            string lrMaintenanceLogs = GetRecordsToMigrate(PropertiesConst.PropertiesConstInstance.SQLServer, PropertiesConst.PropertiesConstInstance.SPGetMaintenanceLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(lrMaintenanceLogs))
            {
                PropertiesConst.PropertiesConstInstance.UpdateErrors($"Could not get labReqs MaintenanceLogs for sp {PropertiesConst.PropertiesConstInstance.SPGetMaintenanceLogs}");

                emailHtmlFile.SendEmail(DateTime.Now.ToString("MM-dd-yyyy"), "MaintenanceLog", true).ConfigureAwait(false).GetAwaiter().GetResult();
                OpenCloseLogFile.OpenCloseLogFileInstance.CloseTraceLog();
                return;
            }
            var readTask = JsonConvert.DeserializeObject<NYPMaintenanceLogsModel[]>(lrMaintenanceLogs);
            IDictionary<int, NYPMaintenanceLogsModel> dicMLogs = readTask.ToDictionary(k => k.ID);
            int totalRecs = dicMLogs.Count();
            int totRecsProcessed = 0;
            string labReqsPDFSf = string.Empty;
            Utilities.DownLoadPDFFiles downLoadPDF = new DownLoadPDFFiles();
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Records to process {totalRecs}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Records to process {totalRecs}");
            foreach (var kvp in dicMLogs)
            {
                try
                {
                    labReqsPDFSf = System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.NYPSaveFolder, $"MaintenanceLogs\\{kvp.Value.ScanDate.ToString("yyyy")}\\{kvp.Value.ScanDate.ToString("MM-dd")}\\");
                    Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(labReqsPDFSf);
                    string azureContainer = string.Empty;
                    string downloadFName = string.Empty;
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Processing ID {kvp.Value.ID}  file {kvp.Value.FileUrl.ToString()}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Processing file ID {kvp.Value.ID} {kvp.Value.FileUrl.ToString()}");
                    PropertiesConst.PropertiesConstInstance.GetAzureCont(new Uri(kvp.Value.FileUrl), ref azureContainer, ref downloadFName);
                    string pdfSaveFname = $"{labReqsPDFSf}{System.IO.Path.GetFileName(downloadFName)}";
                    downLoadPDF.DownloadFile(azureContainer.Replace("/", "").Trim(), downloadFName.Replace("%5C", "/"), pdfSaveFname).ConfigureAwait(false).GetAwaiter().GetResult();
                    labReqsPDFSf = PropertiesConst.PropertiesConstInstance.GetSaveFolder(labReqsPDFSf);
                    bool recordAdded = false;
                    for (int i = 0; i < Utilities.PropertiesConst.PropertiesConstInstance.NumberTimesLoop; i++)
                    {

                        edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Adding record for ID {kvp.Value.ID} to Sqlite");
                        edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Adding record for ID {kvp.Value.ID} to Sqlite");
                        string sqlCmd = $"INSERT INTO MaintenanceLogs (FileName,FileDirectory,BatchID,LogDate,LogStationID,CheckSum,DateUploaded) VALUES ('{kvp.Value.FileName}','{labReqsPDFSf}','{kvp.Value.BatchID}','{kvp.Value.LogDate.ToString("yyyyy-MM-dd")}','{kvp.Value.LogStationId}','{kvp.Value.Checksum}','{kvp.Value.DateUpload.ToString("yyyyy-MM-dd")}')";
                        int results = SqlLiteCmds.SqlLiteCmdsInstance.AddRecordsSqliteTabe(sqlCmd, PropertiesConst.PropertiesConstInstance.SqliteNypMaintenanceLogsDB).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (results == 1)
                        {
                            recordAdded = true;
                            break;
                        }
                        System.Threading.Thread.Sleep(PropertiesConst.PropertiesConstInstance.ThreadSleepSecs);
                        edl.TraceLogger.TraceLoggerInstance.TraceWaringConsole($"Record for id for ID {kvp.Value.ID} not added number time retrying {i}");
                        edl.TraceLogger.TraceLoggerInstance.TraceWaringConsole($"Record for for ID {kvp.Value.ID} not added number time retrying {i}");
                    }
                    if (!(recordAdded))
                    {
                        PropertiesConst.PropertiesConstInstance.UpdateErrors($"Could not add record ID to Sqlite Database {kvp.Value.ID} for MaintenanceLogs");
                    }
                    else
                    {
                        edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Updating table MaintenanceLogs for migrated for id {kvp.Value.ID}");
                        edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Updating table MaintenanceLogs for migrated for id {kvp.Value.ID}");
                        LRecsSqlCmds.LRecsSqlCmdsInstance.UpdateMigratedLabReqs(PropertiesConst.PropertiesConstInstance.SQLServer, "MaintenanceLogs", kvp.Value.ID).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    totRecsProcessed++;
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Records to Left process {totalRecs--}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Records Left to process {totalRecs--}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total errors found   {PropertiesConst.PropertiesConstInstance.TotalErrors}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total errors found   {PropertiesConst.PropertiesConstInstance.TotalErrors}");

                    if (PropertiesConst.PropertiesConstInstance.TotalErrors > PropertiesConst.PropertiesConstInstance.MaxErrors)
                    {
                        PropertiesConst.PropertiesConstInstance.UpdateErrors($"Total Errors for MaintenanceLogs {PropertiesConst.PropertiesConstInstance.TotalErrors} Max errors set at { PropertiesConst.PropertiesConstInstance.MaxErrors}");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    PropertiesConst.PropertiesConstInstance.UpdateErrors($"Migratng MaintenanceLogs for migrated for id {kvp.Value.ID} {ex.Message}");
                }

            }
            string jsonfile = JsonConvert.SerializeObject(dicMLogs, Formatting.Indented);
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Saving Json File {System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.LabReqsJasonFolder, $"MaintenanceLogs_{DateTime.Now.ToString("yyyy-MM-dd")} _records.json")}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Saving Json File {System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.LabReqsJasonFolder, $"MaintenanceLogs_{DateTime.Now.ToString("yyyy-MM-dd")} _records.json")}");
            System.IO.File.WriteAllText($"{System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.LabReqsJasonFolder, $"MaintenanceLogs_{DateTime.Now.ToString("yyyy-MM-dd")} _records.json")}", jsonfile);
            if (PropertiesConst.PropertiesConstInstance.TotalErrors != 0)
            {

                emailHtmlFile.SendEmail(DateTime.Now.ToString("MM-dd-yyyy"), "MaintenanceLog", true).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total time {edl.TraceLogger.TraceLoggerInstance.StopStopWatch()} to process patient id records process {totRecsProcessed}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total time {edl.TraceLogger.TraceLoggerInstance.StopStopWatch()} to process patient id records process {totRecsProcessed}");
            OpenCloseLogFile.OpenCloseLogFileInstance.CloseTraceLog();
        }

        public async Task GetDOH()
        {
            EmailHtmlFile emailHtmlFile = new EmailHtmlFile();
            edl.TraceLogger.TraceLoggerInstance.StartStopStopWatch();
            OpenCloseLogFile.OpenCloseLogFileInstance.OpenLogFile($"DOH Records", System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.NYPMigrationLogFolder, "DOH")).ConfigureAwait(false).GetAwaiter().GetResult();
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting DOH Records");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Getting DOH Records");
            string dohRec = GetRecordsToMigrate(PropertiesConst.PropertiesConstInstance.SQLServer, PropertiesConst.PropertiesConstInstance.SPGetGetDOH).ConfigureAwait(false).GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(dohRec))
            {
                PropertiesConst.PropertiesConstInstance.UpdateErrors($"Could not get labReqs DOH for sp {PropertiesConst.PropertiesConstInstance.SPGetGetDOH}");
                emailHtmlFile.SendEmail(DateTime.Now.ToString("MM-dd-yyyy"), "DOH", true).ConfigureAwait(false).GetAwaiter().GetResult();
                OpenCloseLogFile.OpenCloseLogFileInstance.CloseTraceLog();
                return;
            }
            var readTask = JsonConvert.DeserializeObject<NYPDohRecsModel[]>(dohRec);
            IDictionary<int, NYPDohRecsModel> dicMLogs = readTask.ToDictionary(k => k.ID);
            int totalRecs = dicMLogs.Count();
            int totRecsProcessed = 0;
            Utilities.DownLoadPDFFiles downLoadPDF = new DownLoadPDFFiles();
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Records to process {totalRecs}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Records to process {totalRecs}");
            string labReqsPDFSf = string.Empty;
            foreach (var kvp in dicMLogs)
            {
                try
                {
                    labReqsPDFSf = System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.NYPSaveFolder, $"DOH\\{kvp.Value.ScanDate.ToString("yyyy")}\\{kvp.Value.ScanDate.ToString("MM-dd")}\\");
                    Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(labReqsPDFSf);
                    string azureContainer = string.Empty;
                    string downloadFName = string.Empty;
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Processing ID {kvp.Value.ID}  file {kvp.Value.FileUrl.ToString()}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Processing file ID {kvp.Value.ID} {kvp.Value.FileUrl.ToString()}");
                    PropertiesConst.PropertiesConstInstance.GetAzureCont(new Uri(kvp.Value.FileUrl), ref azureContainer, ref downloadFName);
                    string pdfSaveFname = $"{labReqsPDFSf}{System.IO.Path.GetFileName(downloadFName)}";
                    downLoadPDF.DownloadFile(azureContainer.Replace("/", "").Trim(), downloadFName.Replace("%5C", "/"), pdfSaveFname).ConfigureAwait(false).GetAwaiter().GetResult();
                    labReqsPDFSf = PropertiesConst.PropertiesConstInstance.GetSaveFolder(labReqsPDFSf);
                    bool recordAdded = false;
                    for (int i = 0; i < Utilities.PropertiesConst.PropertiesConstInstance.NumberTimesLoop; i++)
                    {
                        edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Adding record for ID {kvp.Value.ID}");
                        edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Adding record for ID {kvp.Value.ID}");
                        string sqlCmd = $"INSERT INTO DOH (FileName,FileDirectory,BatchID,MedicalRecordNumber,AccessionNumber,ScanDate,DrID,DateOfService,DateUpLoaded) VALUES ('{kvp.Value.FileName}','{labReqsPDFSf}','{kvp.Value.BatchID}','{kvp.Value.MRN}','{kvp.Value.AccessionNumber}',{kvp.Value.ScanDate.ToString("yyyyy-MM-dd")},'{kvp.Value.DrID}','{kvp.Value.DateOfService.ToString("yyyyy-MM-dd")}','{kvp.Value.DateUpload.ToString("yyyyy-MM-dd")}')";
                        int results = SqlLiteCmds.SqlLiteCmdsInstance.AddRecordsSqliteTabe(sqlCmd, PropertiesConst.PropertiesConstInstance.SqliteNypDOHDB).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (results == 1)
                        {
                            recordAdded = true;
                            break;
                        }
                        System.Threading.Thread.Sleep(PropertiesConst.PropertiesConstInstance.ThreadSleepSecs);
                        edl.TraceLogger.TraceLoggerInstance.TraceWaringConsole($"Record for id for ID {kvp.Value.ID} not added number time retrying {i}");
                        edl.TraceLogger.TraceLoggerInstance.TraceWaringConsole($"Record for for ID {kvp.Value.ID} not added number time retrying {i}");
                    }
                    if (!(recordAdded))
                    {
                        PropertiesConst.PropertiesConstInstance.UpdateErrors($"Could not add record ID to Sqlite Database {kvp.Value.ID} for DOH");
                    }
                    else
                    {
                        edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Updating table DOH for migrated for id {kvp.Value.ID}");
                        edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Updating table DOH for migrated for id {kvp.Value.ID}");
                        LRecsSqlCmds.LRecsSqlCmdsInstance.UpdateMigratedLabReqs(PropertiesConst.PropertiesConstInstance.SQLServer, "DOH", kvp.Value.ID).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    if (PropertiesConst.PropertiesConstInstance.TotalErrors > PropertiesConst.PropertiesConstInstance.MaxErrors)
                    {
                        PropertiesConst.PropertiesConstInstance.UpdateErrors($"Total Errors for MaintenanceLogs {PropertiesConst.PropertiesConstInstance.TotalErrors} Max errors set at { PropertiesConst.PropertiesConstInstance.MaxErrors}");
                        break;
                    }
                }

                catch (Exception ex)
                {
                    PropertiesConst.PropertiesConstInstance.UpdateErrors($"Migratng DOH for migrated for id {kvp.Value.ID} {ex.Message}");
                }
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Records to Left process {totalRecs--}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Records Left to process {totalRecs--}");
                totRecsProcessed++;
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Records process {totRecsProcessed}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Records  process {totRecsProcessed}");
            }
            string jsonfile = JsonConvert.SerializeObject(dicMLogs, Formatting.Indented);
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Saving Jason File {jsonfile}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Saving Jason File {jsonfile}");
            System.IO.File.WriteAllText($"{System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.LabReqsJasonFolder, $"DOH_{DateTime.Now.ToString("yyyy-MM-dd")} _records.json")}", jsonfile);
            if (PropertiesConst.PropertiesConstInstance.TotalErrors != 0)
            {
                emailHtmlFile.SendEmail(DateTime.Now.ToString("MM-dd-yyyy"), "DOH", true).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total time {edl.TraceLogger.TraceLoggerInstance.StopStopWatch()} to process patient id records process {totRecsProcessed}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total time {edl.TraceLogger.TraceLoggerInstance.StopStopWatch()} to process patient id records process {totRecsProcessed}");
            OpenCloseLogFile.OpenCloseLogFileInstance.CloseTraceLog();
        }
        public async Task<bool> GetCovidLabReqs()
        {
            DateTime startTime = DateTime.Now;

            PropertiesConst.PropertiesConstInstance.TotalErrors = 0;
            PropertiesConst.PropertiesConstInstance.MigrationErrors = new List<string>();
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Getting Covid Lab Reqs");
            edl.TraceLogger.TraceLoggerInstance.StartStopStopWatch();
            string labReqs = GetRecordsToMigrate(PropertiesConst.PropertiesConstInstance.SQLServer, PropertiesConst.PropertiesConstInstance.SPMigrationGetCovidLabReqs).ConfigureAwait(false).GetAwaiter().GetResult();
            EmailHtmlFile emailHtmlFile = new EmailHtmlFile();
            if (string.IsNullOrWhiteSpace(labReqs))
            {
                PropertiesConst.PropertiesConstInstance.MigrationErrors.Add($"Could not get covid labreqs for sp {PropertiesConst.PropertiesConstInstance.SPMigrationGetCovidLabReqs}");
                PropertiesConst.PropertiesConstInstance.TotalErrors++;
                emailHtmlFile.SendEmail(DateTime.Now.ToString("MM-dd-yyyy"), "Covid LabReqs").ConfigureAwait(false).GetAwaiter().GetResult();

                edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Could not getcovid labreqs for sp {PropertiesConst.PropertiesConstInstance.SPMigrationGetCovidLabReqs}");
                return true;
            }
            var readTask = JsonConvert.DeserializeObject<NYPLabReqsModel[]>(labReqs);
            Utilities.DownLoadPDFFiles downLoadPDF = new DownLoadPDFFiles();
            IDictionary<int, NYPLabReqsModel> dicLabReqs = readTask.ToDictionary(k => k.ID);
            var firstValue = dicLabReqs.First();
            string scanDate = firstValue.Value.ScanDate.ToString("MM-dd");
            string scanYear = firstValue.Value.ScanDate.ToString("yyyy");
            OpenCloseLogFile.OpenCloseLogFileInstance.OpenLogFile($"COVID19LabReqs_ScanDate_{scanYear}_{scanDate}", System.IO.Path.Combine($"{PropertiesConst.PropertiesConstInstance.NYPMigrationLogFolder}\\Covid19LabReqs", DateTime.Now.ToString("MM-dd-yyyy"))).ConfigureAwait(false).GetAwaiter().GetResult();
            string labReqsPDFSf = System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.NYPSaveFolder, $"COVID19LabReqs\\{scanYear}\\{scanDate}\\");
            string pdfSaveFname = PropertiesConst.PropertiesConstInstance.GetSaveFolder(labReqsPDFSf);
            int totalRecs = dicLabReqs.Count();
            int totalPRocess = 0;
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total records to process {totalRecs} for scandate {scanDate}-{scanYear}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total records to process {totalRecs} for scandate {scanDate}-{scanYear}"); ;
            Edocs_Utilities.EdocsUtilitiesInstance.CreateDir(labReqsPDFSf);
            foreach (var kvp in dicLabReqs)
            {
                try
                {
                    bool recordAdded = false;
                    string azureContainer = string.Empty;
                    string downloadFName = string.Empty;
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Processing file {kvp.Value.FileUrl.ToString()}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Processing file {kvp.Value.FileUrl.ToString()}");
                    PropertiesConst.PropertiesConstInstance.GetAzureCont(new Uri(kvp.Value.FileUrl), ref azureContainer, ref downloadFName);
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Saving file {downloadFName} for ID {kvp.Value.ID}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Saving file {downloadFName} {kvp.Value.ID}");
                    string labRPDFSf = $"{labReqsPDFSf}{System.IO.Path.GetFileName(downloadFName)}";
                    downLoadPDF.DownloadFile(azureContainer.Replace("/", "").Trim(), downloadFName, labRPDFSf).ConfigureAwait(false).GetAwaiter().GetResult();
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Adding records to Sqlite table COVID19LabReqs for pdf file {kvp.Value.FileName} id {kvp.Value.ID}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Adding records tp Sqlite table COVID19LabReqs for pdf file {kvp.Value.FileName} id {kvp.Value.ID}");
                    for (int i = 0; i < Utilities.PropertiesConst.PropertiesConstInstance.NumberTimesLoop; i++)
                    {
                        int results = SqlLiteCmds.SqlLiteCmdsInstance.AddLabReqs(System.IO.Path.GetDirectoryName(pdfSaveFname), kvp.Value, PropertiesConst.PropertiesConstInstance.SqliteNypLabReqsDB, "COVID19LabReqs").ConfigureAwait(false).GetAwaiter().GetResult();
                        if (results == 1)
                        {
                            recordAdded = true;
                            break;
                        }
                        System.Threading.Thread.Sleep(PropertiesConst.PropertiesConstInstance.ThreadSleepSecs);
                        edl.TraceLogger.TraceLoggerInstance.TraceWaringConsole($"Record ID added {kvp.Value.ID} for file {kvp.Value.FileName} number time retrying {i}");
                        edl.TraceLogger.TraceLoggerInstance.TraceWarning($"Record ID added {kvp.Value.ID} for file {kvp.Value.FileName} number time retrying {i}"); ;

                    }
                    if (!(recordAdded))
                    {
                        PropertiesConst.PropertiesConstInstance.UpdateErrors($"Could not add record {kvp.Value.ID} filename {kvp.Value.FileName} to SqlLite for labreqs");

                    }
                    else
                    {
                        edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Updating table COVID19LabReqs for migrated for id {kvp.Value.ID}");
                        edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Updating table COVID19LabReqs for migrated for id {kvp.Value.ID}");
                        LRecsSqlCmds.LRecsSqlCmdsInstance.UpdateMigratedLabReqs(PropertiesConst.PropertiesConstInstance.SQLServer, "COVID19LabReqs", kvp.Value.ID).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    totalPRocess++;
                    totalRecs--;
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Records to Left process {totalRecs}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Records Left to process {totalRecs}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Records processed {totalPRocess}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Records processed {totalPRocess}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total errors found   {PropertiesConst.PropertiesConstInstance.TotalErrors}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total errors found   {PropertiesConst.PropertiesConstInstance.TotalErrors}");

                    if (PropertiesConst.PropertiesConstInstance.TotalErrors > PropertiesConst.PropertiesConstInstance.MaxErrors)
                    {
                        PropertiesConst.PropertiesConstInstance.UpdateErrors($"Total Errors for COVID19LabReqs {PropertiesConst.PropertiesConstInstance.TotalErrors} Max errors set at { PropertiesConst.PropertiesConstInstance.MaxErrors}");
                        break;
                    }
                    PropertiesConst.PropertiesConstInstance.TotalLabRecsProcessed++;
                    DateTime endTime = DateTime.Now; // Current date and time

                    // Calculate the difference
                    TimeSpan totalTime = endTime - startTime;
                    edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total Running Time:  {totalTime.ToString(@"hh\:mm\:ss")}");
                    edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total Running Time:  {totalTime.ToString(@"hh\:mm\:ss")}");
                }
                catch (Exception ex)
                {
                    PropertiesConst.PropertiesConstInstance.UpdateErrors($"Error found for table COVID19LabReqs for migrated for id {kvp.Value.ID} {ex.Message}");
                }
            }

            string jsonfile = JsonConvert.SerializeObject(dicLabReqs, Formatting.Indented);
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Saving Json File {System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.LabReqsJasonFolder, $"COVID19LabReqs-{scanYear}_{scanDate}_records.json")}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Saving Json File {System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.LabReqsJasonFolder, $"COVID19LabReqs_{scanYear}_{scanDate}_records.json")}");
            System.IO.File.WriteAllText($"{System.IO.Path.Combine(PropertiesConst.PropertiesConstInstance.LabReqsJasonFolder, $"COVID19LabReqs{scanYear}_{scanDate}_records.json")}", jsonfile);
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total records process {totalRecs} for scandate {scanDate}-{scanYear}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total records process {totalRecs} for scandate {scanDate}-{scanYear}");
            GetNYPRecords.NYPRecordsInstance.UpDateNYPLaReqsMigrated(PropertiesConst.PropertiesConstInstance.SQLServer, PropertiesConst.PropertiesConstInstance.SPMigrationUpDateNYPCovidRecsDownloaded, totalPRocess, $"{scanYear}-{scanDate}").ConfigureAwait(false).GetAwaiter().GetResult();
            LRecsSqlCmds.LRecsSqlCmdsInstance.UpdateRecordsProcessed(PropertiesConst.PropertiesConstInstance.SQLServer, "COVID19LabReqs", totalPRocess, PropertiesConst.PropertiesConstInstance.TotalErrors).ConfigureAwait(false).GetAwaiter().GetResult();
            if (PropertiesConst.PropertiesConstInstance.TotalErrors != 0)
            {

                emailHtmlFile.SendEmail($"{scanDate}_{scanYear}", "COVID19LabReqs", true).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total time {edl.TraceLogger.TraceLoggerInstance.StopStopWatch()} to process records process {totalPRocess} for scandate {scanDate}-{scanYear}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total time {edl.TraceLogger.TraceLoggerInstance.StopStopWatch()} to records process {totalPRocess} for scandate {scanDate}-{scanYear}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Total labrecs processed {PropertiesConst.PropertiesConstInstance.TotalLabRecsProcessed}");
            edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Total labrecs processed {PropertiesConst.PropertiesConstInstance.TotalLabRecsProcessed}");

            OpenCloseLogFile.OpenCloseLogFileInstance.CloseTraceLog();
            return false;
        }
    }
}
