using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EasyHL7MC40;
using System.Diagnostics;
using System.IO;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Mail;
using System.Reflection;
using System.Data.SqlClient;
using AL = EdocsUSA.Utilities.Logging.TraceLogger;
using ABS = Edocs.Upload.Azure.Blob.Storage.AzureBlobStorage;
namespace ProcessHL7Files
{
    class Program
    {
        #region Data Types

        class LabRecord
        {
            public DateTime DateOfService { get; set; }
            public string PatientID { get; set; }
            public string ClientCode { get; set; }
            public string PatientFirstName { get; set; }
            public string PatientLastName { get; set; }
            public string FinancialNumber { get; set; }
            public string DrCode { get; set; }
            public string DrName { get; set; }
            public string RequisitionNumber { get; set; }

            public string AccessionNumber { get; set; }
            public override string ToString()
            {
                return string.Format("DateOfService:{0}, PatientID:{1}, ClientCode:{2}, PatientLastName:{3}, PatientFirstName:{4}, FinancialNumber:{5}, DrCode:{6}, DrName:{7}, RequisitionNumber:{8},AccessionNumber:{9}"
                    , DateOfService, PatientID, ClientCode, PatientLastName, PatientFirstName, FinancialNumber, DrCode, DrName, RequisitionNumber, AccessionNumber);
            }
        }

        class ParseHL7MessageResult
        {

            private DateTime _MessageDate = DateTime.MinValue;
            public DateTime MessageDate
            {
                get { return _MessageDate; }
                set { _MessageDate = value; }
            }
            public bool Valid { get; set; }
            public string RawMessageText { get; set; }
            public string Message { get; set; }
            public LabRecord Record { get; set; }
        }

        #endregion Data Types

        #region External Settings

        static Settings.Demographics DemographicSettings = Settings.Demographics.Default;

        static Settings.Email EmailSettings = Settings.Email.Default;


        /// <summary>From address for the email message.</summary>
        string EmailSenderFrom
        { get { return EmailSettings.SenderFrom; } }

        /// <summary>Comma separated list of email addresses to send summary reports to.</summary>
        string EmailRecipients
        { get { return EmailRecipients; } }

        /// <summary>True to send an email summary if the process succeeds.</summary>
        bool EmailOnSuccess
        { get { return EmailSettings.EmailOnSuccess; } }

        /// <summary>True to send an email summary if the process fails.</summary>
        bool EmailOnError
        { get { return EmailSettings.EmailOnError; } }

        string SqlServer
        { get { return DemographicSettings.SqlServer; } }
        string SqlDataBase
        { get { return DemographicSettings.SqlDataBase; } }


        static int DaysToKeepLogFiles
        { get { return DemographicSettings.DaysToKeepLogFiles; } }
        static int TotalRecodsAdded
        { get; set; }
        #endregion External Settings
        static string AzureBlobAccountKey
        { get { return DemographicSettings.AzureBlobAccountKey; } }
        static string AzureBlobStorageConnectionString
        { get { return DemographicSettings.AzureBlobStorageConnectionString; } }
        static string AzureBlobAccountName
        {
            get { return DemographicSettings.AzureBlobAccountName; }
        }
        static string AzureBlobContanierAuditLogs
        {
            get { return DemographicSettings.AzureBlobContanierAuditLogs; }
        }

        static string AzureBlobContanierHl7Archive
        {
            get { return DemographicSettings.AzureBlobContanierHl7Archive; }
        }
        static bool ArchiveHl7Azure
        {
            get { return DemographicSettings.ArchiveHl7Azure; }
        }

        //HL7 Message counters
        private static readonly string EpicRecord = "EPIC";
        private static readonly string SqlSP = "[dbo].[AddHL7Files]";
        //  private static readonly string SqlSP = "[dbo].[AddHL7FilesTest]";
        static int invalidMessageCount = 0;
        static int validMessageCount = 0;
        static readonly string DBNullValue = "NULL";
        static readonly string OBRMessageStr = "OBR|";
        static readonly string ORCMessageStr = "ORC|";
        static string errorMessage = string.Empty;
        static void Main(string[] args)
        {

            DateTime startTime = DateTime.Now;
            TotalRecodsAdded = 0;
            //Start a stopwatch to keep track of execution runtime
            Stopwatch runtimeStopwatch = Stopwatch.StartNew();



            bool completedWithoutError = false;

            string currentHL7File = string.Empty;
            string invalidHL7Files = string.Empty;
            ABS.BlobStorageInstance.AzureBlobAccountKey = AzureBlobAccountKey;
            ABS.BlobStorageInstance.AzureBlobAccountName = AzureBlobAccountName;
            ABS.BlobStorageInstance.AzureBlobStorageConnectionString = AzureBlobStorageConnectionString;
            string hl7AzContainer = $"{AzureBlobContanierHl7Archive}/{DateTime.Now.ToString("yyyy-MM-dd")}/";
            try
            {


                //Initialize the output and copy directories
                Directory.CreateDirectory(DemographicSettings.OutputDir);
                if (DemographicSettings.CopyOriginal)
                { Directory.CreateDirectory(DemographicSettings.CopyDir); }

                //Initialize log file
                string logDir = Path.Combine(DemographicSettings.OutputDir, "Logs", startTime.ToString("yyyyMM"));
                string auditLogDir = Path.Combine(DemographicSettings.OutputDir, "AuditLogs");
                Directory.CreateDirectory(logDir);
                AL.TraceLoggerInstance.RunningAssembley = Process.GetCurrentProcess().ProcessName;
                AL.TraceLoggerInstance.OpenTraceFileAddHHMMSS(auditLogDir, $"{AL.TraceLoggerInstance.RunningAssembley}_ProcessHL7", AL.TraceLoggerInstance.RunningAssembley, true);
                string logFileName = startTime.ToString("yyyyMMddHHmmss") + "_ProcessHL7";
                logFileName = Path.ChangeExtension(logFileName, "log");
                string logFilePath = Path.Combine(logDir, logFileName);
                AL.TraceLoggerInstance.TraceInformation($"Azure settings AzureBlobAccountKey: {ABS.BlobStorageInstance.AzureBlobAccountKey} AzureBlobAccountName: {ABS.BlobStorageInstance.AzureBlobAccountName} AzureBlobStorageConnectionString: {ABS.BlobStorageInstance.AzureBlobStorageConnectionString} AzureContanier: {hl7AzContainer}");
                AL.TraceLoggerInstance.TraceInformation($"Start time {DateTime.Now} Logging folder {logFilePath}");
                AL.TraceLoggerInstance.TraceInformationConsole($"Start time {DateTime.Now} Logging folder {logFilePath}");
                AL.TraceLoggerInstance.TraceInformation($"Using DemographicSettings HL7VendorName: {DemographicSettings.HL7VendorName} HL7VendorVersion: {DemographicSettings.HL7VendorVersion}");
                HL7Vendor hl7Vendor = GetHL7Vendor(DemographicSettings.HL7VendorName, DemographicSettings.HL7VendorVersion);

                string invalidMessageDir = Path.Combine(DemographicSettings.OutputDir, "InvalidMessages", startTime.ToString("yyyyMMdd"));
                Directory.CreateDirectory(invalidMessageDir);
                AL.TraceLoggerInstance.TraceInformation($"Invalid message folder: {invalidMessageDir}");
                AL.TraceLoggerInstance.TraceInformationConsole($"Invalid message folder: {invalidMessageDir}");


                invalidHL7Files = Path.Combine(DemographicSettings.OutputDir, "InvalidHL7Files");
                AL.TraceLoggerInstance.TraceInformation($"Invalid message file: {invalidHL7Files}");
                AL.TraceLoggerInstance.TraceInformationConsole($"Invalid message file: {invalidHL7Files}");
                Directory.CreateDirectory(invalidHL7Files);

                AL.TraceLoggerInstance.TraceInformation($"Itterate through the incomming HL7Files in folder: {DemographicSettings.InputDir}");
                AL.TraceLoggerInstance.TraceInformationConsole($"Itterate through the incomming HL7Files in folder: {DemographicSettings.InputDir}");
                //Itterate through the incomming HL7Files
                IEnumerable<string> hl7Files = GetHL7Files(DemographicSettings.InputDir);
                AL.TraceLoggerInstance.TraceInformation($"Azure settings AzureBlobAccountKey: {ABS.BlobStorageInstance.AzureBlobAccountKey} AzureBlobAccountName: {ABS.BlobStorageInstance.AzureBlobAccountName} AzureBlobStorageConnectionString: {ABS.BlobStorageInstance.AzureBlobStorageConnectionString} AzureContanier: {hl7AzContainer}");
                int hl7FileCounter = 0;
                foreach (string hl7File in hl7Files)
                {
                    hl7FileCounter++;
                    if (hl7FileCounter % 500 == 0)
                    {
                        AL.TraceLoggerInstance.TraceInformation($"Processing: {hl7FileCounter.ToString()}");
                        AL.TraceLoggerInstance.TraceInformationConsole("Processing " + hl7FileCounter.ToString());
                    }

                    //  Debug.WriteLine("Processing " + hl7File);
                    currentHL7File = hl7File;
                    AL.TraceLoggerInstance.TraceInformation($"Processing HL & File:{ hl7File}");
                    AL.TraceLoggerInstance.TraceInformationConsole($"Processing HL& File:{hl7File}");
                    StreamWriter validMessageWriter = null;
                    StreamWriter invalidMessageWriter = null;
                    bool validFile = true;
                    try
                    {
                        string invalidMessageFileName = Path.GetFileName(hl7File);
                        string invalidMessageFilePath = Path.Combine(invalidMessageDir, invalidMessageFileName);
                        AL.TraceLoggerInstance.TraceInformation($"invalidMessageFileName:{invalidMessageFileName}");
                        AL.TraceLoggerInstance.TraceInformationConsole($"invalidMessageFileName:{invalidMessageFileName}");
                        AL.TraceLoggerInstance.TraceInformation($"invalidMessageFilePath:{invalidMessageFilePath}");
                        AL.TraceLoggerInstance.TraceInformationConsole($"invalidMessageFilePath:{invalidMessageFilePath}");
                        DateTime currentMessageDate = DateTime.MinValue;

                        //Loop through the individual messages in the current file.
                        foreach (ParseHL7MessageResult pResult in ParseHL7File(hl7Vendor, hl7File))
                        {
                            //continue;
                            //Debug.WriteLine(pResult.Message);

                            if (pResult.MessageDate.Date != currentMessageDate.Date)
                            {
                                AL.TraceLoggerInstance.TraceWarning("Changing date to " + pResult.MessageDate.Date.ToShortDateString());
                                AL.TraceLoggerInstance.TraceWaringConsole("Changing date to " + pResult.MessageDate.Date.ToShortDateString());
                                if (validMessageWriter != null)
                                {
                                    validMessageWriter.Flush();
                                    validMessageWriter.Close();
                                }
                                currentMessageDate = pResult.MessageDate.Date;

                                //Initialize the output dir for this message
                                string outputDir = Path.Combine(DemographicSettings.OutputDir, pResult.MessageDate.ToString("yyyyMM"));
                                Directory.CreateDirectory(outputDir);

                                //Initialize the writers for valid and invalid messages
                                string outputFileNameBase = pResult.MessageDate.ToString("yyyyMMdd");

                                string validMessageFileName = outputFileNameBase + "_valid";
                                validMessageFileName = Path.ChangeExtension(validMessageFileName, "hl7");
                                string validMessageFilePath = Path.Combine(outputDir, validMessageFileName);
                                validMessageWriter = File.AppendText(validMessageFilePath);

                            }

                            //If the message was invalid, just log it
                            if (pResult.Valid == false)
                            {
                                validFile = false;
                                AL.TraceLoggerInstance.TraceWarning($"Skipping invalid message {pResult.RawMessageText} message: {pResult.Message}");
                                AL.TraceLoggerInstance.TraceWarning("Skipping " + pResult.RawMessageText);
                                AL.TraceLoggerInstance.TraceWarning(pResult.Message);

                                AL.TraceLoggerInstance.TraceWaringConsole($"Skipping invalid message {pResult.RawMessageText} message: {pResult.Message}");
                                AL.TraceLoggerInstance.TraceWaringConsole("Skipping " + pResult.RawMessageText);
                                AL.TraceLoggerInstance.TraceWaringConsole(pResult.Message);

                                if (invalidMessageWriter == null)
                                { invalidMessageWriter = File.AppendText(invalidMessageFilePath); }
                                invalidMessageWriter.WriteLine(pResult.RawMessageText);
                                invalidMessageWriter.WriteLine($"Invalid file under:{Path.Combine(invalidHL7Files, Path.GetFileName(currentHL7File))}");
                                invalidMessageCount++;
                            }
                            else //Message was valid
                            {
                                validMessageCount++;
                                AddRecordToSqlServer(pResult.Record);
                                validMessageWriter.WriteLine(pResult.RawMessageText);
                            }
                        }

                        if (!(validFile))
                        {
                            AL.TraceLoggerInstance.TraceInformation($"Make a copy of hl7 file {CopyErrorHL7File(currentHL7File, invalidHL7Files, false)}");
                            AL.TraceLoggerInstance.TraceInformationConsole($"Make a copy of hl7 file {CopyErrorHL7File(currentHL7File, invalidHL7Files, false)}");
                        }
                        if (ArchiveHl7Azure)
                        {
                            AL.TraceLoggerInstance.TraceInformation($"Uploading file: {hl7File} to AzureContanier: {hl7AzContainer}");
                            AL.TraceLoggerInstance.TraceInformationConsole($"Uploading file: {hl7File} to AzureContanier: {hl7AzContainer}");

                            UploadAzureCloud(hl7File, hl7AzContainer).ConfigureAwait(true).GetAwaiter().GetResult();
                        }
                        else
                        {
                            hl7AzContainer = Path.Combine(DemographicSettings.CopyDir, currentMessageDate.ToString("yyyy-MM-dd"));

                            //hl7AzContainer = Path.Combine(DemographicSettings.CopyDir, DateTime.Now.ToString("yyyy-MM-dd"));
                            //Copy the original file if required.
                            if (DemographicSettings.CopyOriginal)
                            {
                                if (!(Directory.Exists(hl7AzContainer)))
                                    Directory.CreateDirectory(hl7AzContainer);
                                string copyFileName = Path.GetFileName(hl7File);
                                string copyFilePath = Path.Combine(hl7AzContainer, copyFileName);
                                AL.TraceLoggerInstance.TraceInformation($"Archiving HL7 file: {hl7File} to file: {copyFilePath}");
                                AL.TraceLoggerInstance.TraceInformationConsole($"Archiving HL7 file: {hl7File} to file: {copyFilePath}");
                                if (File.Exists(copyFilePath) == false)
                                { File.Copy(hl7File, copyFilePath); }
                                else
                                {
                                    AL.TraceLoggerInstance.TraceInformation($"Found old hl7 file under copyFilePath making backup under {CopyErrorHL7File(copyFilePath, invalidHL7Files, true)}");
                                    AL.TraceLoggerInstance.TraceInformationConsole($"Found old hl7 file under copyFilePath making backup under {CopyErrorHL7File(copyFilePath, invalidHL7Files, true)}");
                                    File.Copy(hl7File, copyFilePath, true);
                                }

                            }
                        }
                        //Delete the original file if required.
                        if (DemographicSettings.DeleteOriginal)
                        {
                            File.Delete(hl7File);
                            if (File.Exists(hl7File))
                            {
                                AL.TraceLoggerInstance.TraceError("Error deleting " + hl7File);
                                AL.TraceLoggerInstance.TraceErrorConsole("Error deleting " + hl7File);
                                errorMessage += "Error deleting " + hl7File + "<br/>";

                            }
                        }
                    }
                    finally
                    {
                        if (validMessageWriter != null)
                        {
                            validMessageWriter.Flush();
                            validMessageWriter.Close();
                        }
                        if (invalidMessageWriter != null)
                        {
                            invalidMessageWriter.Flush();
                            invalidMessageWriter.Close();
                        }
                    }
                }

                completedWithoutError = true;
            }
            catch (Exception ex)
            {
                AL.TraceLoggerInstance.TraceError($"{ex.Message}");
                AL.TraceLoggerInstance.TraceError($"{ex.StackTrace}");
                AL.TraceLoggerInstance.TraceError(ex.Message);
                AL.TraceLoggerInstance.TraceError(ex.StackTrace);
                AL.TraceLoggerInstance.TraceErrorConsole($"{ex.Message}");
                AL.TraceLoggerInstance.TraceErrorConsole($"{ex.StackTrace}");
                AL.TraceLoggerInstance.TraceErrorConsole(ex.Message);
                AL.TraceLoggerInstance.TraceErrorConsole(ex.StackTrace);
                completedWithoutError = false;
                errorMessage += ex.Message + "<br/>";
                errorMessage = $"{errorMessage} {CopyErrorHL7File(currentHL7File, invalidHL7Files, true)}<br/>";
                AL.TraceLoggerInstance.TraceError(errorMessage);
                AL.TraceLoggerInstance.TraceErrorConsole(errorMessage);
            }
            finally
            {
                DelLogFiles().ConfigureAwait(true).GetAwaiter().GetResult();

                if (!(ArchiveHl7Azure))
                {
                    DelHl7ArchiveFiles(DaysToKeepLogFiles, DemographicSettings.CopyDir).ConfigureAwait(true).GetAwaiter().GetResult();
                    DeleteEmptyDirs(DemographicSettings.CopyDir).ConfigureAwait(true).GetAwaiter().GetResult();
                }
                runtimeStopwatch.Stop();
                AL.TraceLoggerInstance.TraceInformation($"end time {DateTime.Now} total time {runtimeStopwatch.ElapsedMilliseconds} ms");
                AL.TraceLoggerInstance.TraceInformationConsole($"end time {DateTime.Now} total time {runtimeStopwatch.ElapsedMilliseconds} ms");
                CopyAuditLogs().ConfigureAwait(true).GetAwaiter().GetResult();
                AL.TraceLoggerInstance.TraceInformation("Process complete");
                AL.TraceLoggerInstance.TraceInformation("Valid Messages: " + validMessageCount);
                AL.TraceLoggerInstance.TraceInformation("Invalid Messages: " + invalidMessageCount);
                AL.TraceLoggerInstance.TraceInformation("Total Records added: " + TotalRecodsAdded);
                AL.TraceLoggerInstance.TraceInformation($"Process complete Valid Messages: {validMessageCount} Invalid Messages: {invalidMessageCount} Total Records added: {TotalRecodsAdded}");

                AL.TraceLoggerInstance.TraceInformationConsole("Process complete");
                AL.TraceLoggerInstance.TraceInformationConsole("Valid Messages: " + validMessageCount);
                AL.TraceLoggerInstance.TraceInformationConsole("Invalid Messages: " + invalidMessageCount);
                AL.TraceLoggerInstance.TraceInformationConsole("Total Records added: " + TotalRecodsAdded);
                AL.TraceLoggerInstance.TraceInformationConsole($"Process complete Valid Messages: {validMessageCount} Invalid Messages: {invalidMessageCount} Total Records added: {TotalRecodsAdded}");
                if (invalidMessageCount != 0)
                    completedWithoutError = false;
                if (EmailSettings.EmailOnSuccess ||
                    (EmailSettings.EmailOnError && (completedWithoutError == false)))
                {
                    string emailSubject = "Execution Summary";
                    string emailBody = "Summary"
                        + "<br/>Valid messages " + validMessageCount
                        + "<br/>Invalid Messages " + invalidMessageCount
                        + "<br/>Process Took " + runtimeStopwatch.Elapsed.ToString()
                        + "<br/>" + "<p>" + errorMessage + "</p>";
                    MailPriority emailPriority;
                    if (completedWithoutError == false)
                    { emailPriority = MailPriority.High; }
                    else if (validMessageCount == 0)
                    { emailPriority = MailPriority.High; }
                    else if (invalidMessageCount > 0)
                    { emailPriority = MailPriority.High; }
                    else
                    { emailPriority = MailPriority.Low; }
                    AL.TraceLoggerInstance.TraceInformation("Sending email");
                    AL.TraceLoggerInstance.TraceInformationConsole("Sending email");
                    SendEmail(emailSubject, emailBody, false, emailPriority);
                }

            }
        }
        static async Task DelLogFiles()
        {
            string lFolder = Path.Combine(DemographicSettings.OutputDir, "Logs", DateTime.Now.ToString("yyyyMM"));
            AL.TraceLoggerInstance.TraceInformation($"Method DelLogFiles for folder: {lFolder} numbber days to keep: {DaysToKeepLogFiles}");
            AL.TraceLoggerInstance.TraceInformationConsole($"Method DelLogFiles for folder: {lFolder} numbber days to keep: {DaysToKeepLogFiles}");
            AL.TraceLoggerInstance.DelTraceLogFiles(lFolder, DaysToKeepLogFiles, "*.*", false);
        }
        static async Task CopyAuditLogs()
        {
            try
            {

                string auditLogDir = Path.Combine(DemographicSettings.OutputDir, "AuditLogs");
                foreach (string aFile in Directory.GetFiles(auditLogDir, "*.*"))
                {
                    AL.TraceLoggerInstance.TraceInformation($"Uploading file: {aFile} to AzureContanier: {AzureBlobContanierAuditLogs}");
                    AL.TraceLoggerInstance.TraceInformationConsole($"Uploading file: {aFile} to AzureContanier: {AzureBlobContanierAuditLogs}");
                    ABS.BlobStorageInstance.UploadAzureBlob(aFile, AzureBlobContanierAuditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
                    File.Delete(aFile);
                }


            }
            catch (Exception ex)
            {
                AL.TraceLoggerInstance.TraceError($"Uploading audit log files  AzureContanier: {AzureBlobContanierAuditLogs} {ex.Message}");
                AL.TraceLoggerInstance.TraceErrorConsole($"Uploading audit log files: AzureContanier: {AzureBlobContanierAuditLogs} {ex.Message}");
            }
        }
        static async Task DelHl7ArchiveFiles(int numberDays, string folder)
        {
            AL.TraceLoggerInstance.TraceInformation($"Method DelHl7ArchiveFiles for number of days {numberDays} folder {folder}");
            AL.TraceLoggerInstance.TraceInformationConsole($"Method DelHl7ArchiveFiles for number of days {numberDays} folder {folder}");
            DirectoryInfo info = new DirectoryInfo(folder);
            foreach (var aFiles in info.GetFiles("*.*", SearchOption.AllDirectories))
            {
                try
                {
                    TimeSpan span = DateTime.Now - aFiles.CreationTime;
                    if (span.TotalDays >= numberDays)
                    {
                        AL.TraceLoggerInstance.TraceInformation($"Method DelHl7ArchiveFiles delete file {aFiles.FullName} since number days to keep file {numberDays} and file total days {span.TotalDays} file create time {aFiles.CreationTime.ToString()}");
                        AL.TraceLoggerInstance.TraceInformationConsole($"Method DelHl7ArchiveFiles delete file {aFiles.FullName} since number days to keep file {numberDays} and file total days {span.TotalDays} file create time {aFiles.CreationTime.ToString()}");
                        aFiles.Delete();
                    }
                }
                catch (Exception ex)
                {
                    AL.TraceLoggerInstance.TraceError($"Deleting file {aFiles.FullName} {ex.Message}");
                    AL.TraceLoggerInstance.TraceErrorConsole($"Deleting file {aFiles.FullName} {ex.Message}");
                    errorMessage += $"Deleting file {aFiles.FullName} {ex.Message}<br/>";
                }


            }
        }
        static async Task UploadAzureCloud(string fileName, string azureContanier)
        {
            try
            {
                AL.TraceLoggerInstance.TraceInformation($"Uploading file: {fileName} to AzureContanier: {azureContanier}");
                AL.TraceLoggerInstance.TraceInformationConsole($"Uploading file: {fileName} to AzureContanier: {azureContanier}");
                ABS.BlobStorageInstance.UploadAzureBlob(fileName, azureContanier).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                AL.TraceLoggerInstance.TraceError($"Uploading file: {fileName} to AzureContanier: {azureContanier} {ex.Message}");
                AL.TraceLoggerInstance.TraceErrorConsole($"Uploading file: {fileName} to AzureContanier: {azureContanier} {ex.Message}");
                errorMessage += ($"Uploading file: {fileName} to AzureContanier: {azureContanier} {ex.Message}<br/>");
            }
        }
        static HL7Vendor GetHL7Vendor(string vendorName, string vendorVersion)
        {
            HL7Vendor vendor = new HL7Vendor();
            string s = vendor.APIVersion;
            if (vendor.IsRegistered == false)

            {

                AL.TraceLoggerInstance.TraceErrorConsole("Not registered");
                throw new Exception("Not registered");
            }
            else if (vendor.IsDemo)
            {

                AL.TraceLoggerInstance.TraceInformationConsole("Licensed as demo");
                if (vendor.LicenseExpired)
                {
                    errorMessage += "Demo license expired<br/>";
                    throw new Exception("Demo license expired<br/>");
                }
                else
                {

                    AL.TraceLoggerInstance.TraceInformationConsole("License exires " + vendor.LicenseExpiresOn);
                    errorMessage += "License exires " + vendor.LicenseExpiresOn + "<br/>";
                }
            }


            else if (vendor.IsDevelopment)
            {

                AL.TraceLoggerInstance.TraceInformationConsole("Licensed as dev");
            }
            else
            {

                AL.TraceLoggerInstance.TraceInformationConsole("Licensed as full");
            }


            AL.TraceLoggerInstance.TraceInformationConsole("Loading vendor");
            vendor.VendorPath = "Vendors";

            AL.TraceLoggerInstance.TraceInformationConsole($"Loading Vendor {vendor.VendorPath}");
            if (vendor.VendorExists(vendorName, vendorVersion) == false)
            {

                AL.TraceLoggerInstance.TraceErrorConsole("Cannot find vendor " + vendorName + " " + vendorVersion);
                errorMessage += "Cannot find vendor " + vendorName + " " + vendorVersion + "<br/>";
                throw new Exception("Cannot find vendor " + vendorName + " " + vendorVersion);
            }
            if (vendor.OpenVendor(vendorName, vendorVersion) == false)

            {

                AL.TraceLoggerInstance.TraceErrorConsole("Error opening vendor: " + vendor.LastError);
                throw new Exception("Error opening vendor: " + vendor.LastError);
            }

            return vendor;
        }

        static ParseHL7MessageResult ParseHL7Message(EasyHL7Message msg)
        {
            ParseHL7MessageResult result = new ParseHL7MessageResult();
            result.RawMessageText = msg.RawHL7;
            LabRecord record = new LabRecord();
            //Debug.WriteLine("Parsing " + result.RawMessageText);
            try
            {
                if (msg.SegmentExists("MSH") == false)
                {
                    AL.TraceLoggerInstance.TraceError("Unable to find message segment MSH");
                    AL.TraceLoggerInstance.TraceErrorConsole("Unable to find message segment MSH");
                    errorMessage += "Unable to find message segment MSH<br/>";
                    throw new Exception("Unable to find message segment MSH");

                }
                if (msg.SegmentExists("PID") == false)
                {
                    AL.TraceLoggerInstance.TraceError("Unable to find message segment PID");
                    AL.TraceLoggerInstance.TraceErrorConsole("Unable to find message segment PID");
                    errorMessage += "Unable to find message segment PID<br/>";
                    throw new Exception("Unable to find message segment PID");
                }
                if (msg.SegmentExists("PV1") == false)
                {
                    AL.TraceLoggerInstance.TraceError("Unable to find message segment PV1");
                    AL.TraceLoggerInstance.TraceErrorConsole("Unable to find message segment PV1");
                    errorMessage += "Unable to find message segment PV1<br/>";
                    throw new Exception("Unable to find message segment PV1");
                }

                HL7MessageSegment msh = msg.GetSegment("MSH");
                string msgDate = msg.GetFieldValue(msh, 7).Substring(0, 8);
                result.MessageDate = DateTime.ParseExact(msgDate, "yyyyMMdd", CultureInfo.InvariantCulture).Date;

                //Admit date is sometimes incorrect, so use the message date for DOS
                record.DateOfService = result.MessageDate;


                HL7MessageSegment pid = msg.GetSegment("PID");
                record.PatientID = msg.GetFieldValue(pid, 3, 1); //Patient ID (external)
                record.ClientCode = msg.GetFieldValue(pid, 4, 1); //Alternate Patient ID
                record.PatientLastName = msg.GetFieldValue(pid, 5, 1);
                record.PatientFirstName = msg.GetFieldValue(pid, 5, 2);
                record.FinancialNumber = msg.GetFieldValue(pid, 18, 1); //Patient Account Number
                string epicRecord = msg.GetFieldValue(pid, 18, 4);

                HL7MessageSegment pv1 = msg.GetSegment("PV1");
                record.DrCode = msg.GetFieldValue(pv1, 17, 1); //Admitting Physician ID
                string drLastName = msg.GetFieldValue(pv1, 17, 2);
                string drFirstName = msg.GetFieldValue(pv1, 17, 3);
                if (string.IsNullOrWhiteSpace(drLastName) == false
                    && string.IsNullOrWhiteSpace(drFirstName) == false)
                { record.DrName = string.Format("{0}, {1}", drLastName, drFirstName); }
                else
                { record.DrName = null; }

                record.RequisitionNumber = msg.GetFieldValue(pv1, 50, 1); //Alternate visit id
                if (string.IsNullOrWhiteSpace(record.RequisitionNumber))
                {
                    if (string.Compare(epicRecord, EpicRecord, true) == 0)
                    {
                        if (msg.SegmentExists("OBR") == true)
                        {
                            HL7MessageSegment obr = msg.GetSegment("OBR");
                            record.RequisitionNumber = msg.GetFieldValue(obr, 2, 1);
                            if (string.IsNullOrEmpty(record.RequisitionNumber))
                            {
                                record.RequisitionNumber = msg.GetFieldValue(obr, 3, 1);
                                AL.TraceLoggerInstance.TraceWarning($"{epicRecord} order id not found using HNAM ORDERID {record.RequisitionNumber}");
                            }
                            record.AccessionNumber = msg.GetFieldValue(obr, 20);

                        }
                        else if (msg.SegmentExists("ORC") == true)
                        {
                            HL7MessageSegment ocr = msg.GetSegment("OCR");
                            record.RequisitionNumber = msg.GetFieldValue(ocr, 2, 1);
                            AL.TraceLoggerInstance.TraceWarning($"{epicRecord} getting order id using ORC message {record.RequisitionNumber}");
                            if (string.IsNullOrEmpty(record.RequisitionNumber))
                            {
                                record.RequisitionNumber = msg.GetFieldValue(ocr, 3, 1);
                                AL.TraceLoggerInstance.TraceWarning($"{epicRecord} order id not found using HNAM ORDERID {record.RequisitionNumber}");
                            }
                        }
                        else
                        {
                            record.RequisitionNumber = GetReqNumber(msg.RawHL7, OBRMessageStr);
                            if (string.IsNullOrWhiteSpace(record.RequisitionNumber))
                            {
                                record.RequisitionNumber = GetReqNumber(msg.RawHL7, ORCMessageStr);
                                if (string.IsNullOrWhiteSpace(record.RequisitionNumber))
                                {
                                    if (string.IsNullOrEmpty(record.FinancialNumber))
                                        record.FinancialNumber = "N/A";
                                    AL.TraceLoggerInstance.TraceError($"Unable to find message segment OBR or ORC for {epicRecord} patient id {record.PatientID}");
                                    AL.TraceLoggerInstance.TraceErrorConsole("Unable to find message segment OBR or ORC");
                                    errorMessage += $"Unable to find message segment OBR or ORC {epicRecord} patient id {record.PatientID} csn number {record.FinancialNumber} < br/>";
                                    invalidMessageCount++;
                                }
                            }
                        }
                    }
                }
                if ((string.IsNullOrWhiteSpace(record.AccessionNumber) && (msg.SegmentExists("OBR") == true)))
                {
                    HL7MessageSegment obr = msg.GetSegment("OBR");
                    record.AccessionNumber = msg.GetFieldValue(obr, 20);
                }

                    if (string.IsNullOrWhiteSpace(record.RequisitionNumber))
                    {
                        if (msg.SegmentExists("OBR") == true)
                        {
                            AL.TraceLoggerInstance.TraceWarning($"Not epic hl7 using message seg to get order id ");
                            HL7MessageSegment obr = msg.GetSegment("OBR");

                            record.RequisitionNumber = msg.GetFieldValue(obr, 2, 1);
                            if (string.IsNullOrWhiteSpace(record.RequisitionNumber))
                            {
                                AL.TraceLoggerInstance.TraceWarning($"Not epic hl7 using message seg to get order id using HNAM_ORDERID");
                                record.RequisitionNumber = msg.GetFieldValue(obr, 3, 1);
                            }
                        }
                    
                }

                if (string.IsNullOrEmpty(record.FinancialNumber))
                    record.FinancialNumber = msg.GetFieldValue(pv1, 50, 1);
                if ((string.IsNullOrWhiteSpace(record.DrName)) || (string.IsNullOrWhiteSpace(record.DrCode)))
                {
                    record = GetDrNameCode(record, pv1, msg);
                }
                //Debug.WriteLine("Message parsed to " + record.ToString());
                result.Record = record;
                result.Valid = true;
            }
            catch (Exception ex)
            {
                result.Valid = false;
                result.Message = ex.Message;
            }

            return result;
        }
        static string GetReqNumber(string rawMessage, string messSeg)
        {
            //OBR|1|268881880^EPIC|2428596987^HNAM_ORDERID|
            int index = rawMessage.IndexOf(messSeg);
            string retReqNum = string.Empty;
            AL.TraceLoggerInstance.TraceWarning($"getting  order id using raw message for mess seg {messSeg}");
            if (index > 0)
            {
                string[] reqNum = rawMessage.Substring(index + OBRMessageStr.Length).Split('|');
                if (!(string.IsNullOrWhiteSpace(reqNum[1])))
                {
                    index = reqNum[1].IndexOf("^EPIC");
                    if (index > 0)
                        retReqNum = reqNum[1].Substring(0, index).Trim();
                    else
                    {
                        index = reqNum[2].IndexOf("^HNAM_ORDERID");
                        if (index > 0)
                            retReqNum = reqNum[2].Substring(0, index).Trim();
                    }


                }
                else
                {
                    if (!(string.IsNullOrWhiteSpace(reqNum[2])))
                    {
                        index = reqNum[2].IndexOf("^HNAM_ORDERID");
                        if (index > 0)
                            retReqNum = reqNum[2].Substring(0, index).Trim();
                    }
                }

            }
            return retReqNum;
        }
        static LabRecord GetDrNameCode(LabRecord record, HL7MessageSegment pid, EasyHL7Message msg)
        {
            string drNameFN = msg.GetFieldValue(pid, 7, 3);
            string drNameLN = msg.GetFieldValue(pid, 7, 2);
            pid.Value = pid.Value.Replace("~", "^");
            string drCode = msg.GetFieldValue(pid, 7, 19);



            if ((string.IsNullOrWhiteSpace(drNameFN)) || (string.IsNullOrWhiteSpace(drNameLN)))
            {
                record.DrName = null;
                record.DrCode = null;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(drCode))
                    record.DrCode = null;
                else
                    record.DrCode = drCode;
                if (drNameFN.ToUpper().EndsWith("MD"))
                {
                    drNameFN = drNameFN.ToUpper().Replace("MD", string.Empty).Trim();
                    record.DrName = string.Format("{0},{1}-M.D.", drNameLN, drNameFN);
                }
                else
                {
                    record.DrName = string.Format("{0}, {1}", drNameLN, drNameFN);
                }

            }

            return record;
        }
        static string CopyErrorHL7File(string sourceFile, string destFolder, bool delFile)
        {
            string retStr = string.Empty;
            destFolder = Path.Combine(destFolder, Path.GetFileName(sourceFile));
            try
            {
                retStr = $"Copying error HL7 file:{sourceFile} to dest File:{destFolder}";
                AL.TraceLoggerInstance.TraceInformation(retStr);
                AL.TraceLoggerInstance.TraceInformation($"Copying error HL7 file:{sourceFile} to dest File:{destFolder}");
                AL.TraceLoggerInstance.TraceInformationConsole(retStr);
                AL.TraceLoggerInstance.TraceInformationConsole($"Copying error HL7 file:{sourceFile} to dest File:{destFolder}");
                File.Copy(sourceFile, destFolder, true);

                if ((File.Exists(sourceFile)) && (delFile))
                    File.Delete(sourceFile);
            }
            catch (Exception ex)
            {
                retStr = $"Error copying HL7 sourcefile:{sourceFile} to HL7 Dest File:{destFolder} {ex.Message}";
                errorMessage += retStr + "<br/>";
                AL.TraceLoggerInstance.TraceError($"Copying error HL7 file:{sourceFile} to dest File:{destFolder} {ex.Message}");
                AL.TraceLoggerInstance.TraceError(retStr);
                AL.TraceLoggerInstance.TraceErrorConsole($"Copying error HL7 file:{sourceFile} to dest File:{destFolder} {ex.Message}");
                AL.TraceLoggerInstance.TraceErrorConsole(retStr);

            }
            return retStr;
        }
        static IEnumerable<ParseHL7MessageResult> ParseHL7File(HL7Vendor vendor, string path)
        {
            AL.TraceLoggerInstance.TraceInformation("Analyzing file: " + path);
            AL.TraceLoggerInstance.TraceInformationConsole("Analyzing file: " + path);
            HL7FileAnalyzer hl7FileAnalyzer = vendor.NewFileAnalyzer();

            try
            {
                HL7FileAnalyzer.EHFA_ErrorType ret = hl7FileAnalyzer.AnalyzeHL7File(path);
                if (ret != HL7FileAnalyzer.EHFA_ErrorType.EHFA_NoError)
                {
                    AL.TraceLoggerInstance.TraceError("Error analyzing file " + ret.ToString());
                    AL.TraceLoggerInstance.TraceErrorConsole("Error analyzing file " + ret.ToString());
                    errorMessage += "Error analyzing file " + ret.ToString() + "<br/>";
                    throw new Exception("Error analyzing file " + ret.ToString());
                }

                AL.TraceLoggerInstance.TraceInformation("Processing " + hl7FileAnalyzer.FileMessageCount + " messages");
                AL.TraceLoggerInstance.TraceInformationConsole("Processing " + hl7FileAnalyzer.FileMessageCount + " messages");
                for (int messageIndex = 1; messageIndex <= hl7FileAnalyzer.FileMessageCount; messageIndex++)
                {
                    if (messageIndex % 500 == 0)
                    { Debug.WriteLine(DateTime.Now.ToString() + " Processing " + messageIndex.ToString()); }



                    EasyHL7Message msg = hl7FileAnalyzer.GetHL7MessageObject(messageIndex);


                    //EasyHL7Message msg = hl7FileAnalyzer.GetHL7MessageObject(messageIndex);

                    yield return ParseHL7Message(msg);

                    /*
                    HL7MsgFilePointer pMsg = hl7FileAnalyzer.GetHL7Message(messageIndex);
                    
                    sw.Restart();
                    EasyHL7Message msg = vendor.NewHL7Message(pMsg.MessageText);
                    Trace.TraceInformation(sw.Elapsed.ToString());
                    yield return ParseHL7Message(msg);
                     * */
                }
            }
            finally
            { hl7FileAnalyzer.Clear(); }

        }

        static private string GetConnectionString()
        {
            // To avoid storing the connection string in your code, 
            // you can retrieve it from a configuration file.
            return $"Data Source={DemographicSettings.SqlServer};Initial Catalog={DemographicSettings.SqlDataBase};Integrated Security=true;Connect Timeout=120;";
        }

        static void AddRecordToSqlServer(LabRecord record)
        {
            try
            {
                string sqlConnectionStr = GetConnectionString();

                using (SqlConnection connection = new SqlConnection(sqlConnectionStr))
                {

                    connection.Open();

                    string pLastName = CleanString(record.PatientLastName);
                    string pFirstName = CleanString(record.PatientFirstName);
                    string drName = CleanString(record.DrName);

                    using (SqlCommand cmd = new SqlCommand(SqlSP, connection))
                    {
                        cmd.CommandTimeout = 120;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@DateOfService", record.DateOfService.ToString("yyyy-MM-dd")));
                        if (!(string.IsNullOrWhiteSpace(record.PatientID)))
                            cmd.Parameters.Add(new SqlParameter("@PatientID", record.PatientID));
                        else
                            cmd.Parameters.Add(new SqlParameter("@PatientID", "000000"));
                        if (!(string.IsNullOrWhiteSpace(record.ClientCode)))
                            cmd.Parameters.Add(new SqlParameter("@ClientCode", record.ClientCode));
                        else
                            cmd.Parameters.Add(new SqlParameter("@ClientCode", "000000"));

                        if (!(string.IsNullOrWhiteSpace(pLastName)))
                            cmd.Parameters.Add(new SqlParameter("@PatientLastName", pLastName));
                        else
                            cmd.Parameters.Add(new SqlParameter("@PatientLastName", DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(pFirstName)))
                            cmd.Parameters.Add(new SqlParameter("@PatientFirstName", pFirstName));
                        else
                            cmd.Parameters.Add(new SqlParameter("@PatientFirstName", DBNull.Value));

                        if (!(string.IsNullOrEmpty(record.DrCode)))
                            cmd.Parameters.Add(new SqlParameter("@DrCode", record.DrCode));
                        else
                            cmd.Parameters.Add(new SqlParameter("@DrCode", DBNull.Value));

                        if (!(string.IsNullOrEmpty(drName)))
                            cmd.Parameters.Add(new SqlParameter("@DrName", drName));
                        else
                            cmd.Parameters.Add(new SqlParameter("@DrName", DBNull.Value));

                        if (!(string.IsNullOrEmpty(record.RequisitionNumber)))
                        {
                            if (string.Compare(record.RequisitionNumber, record.FinancialNumber, true) == 0)
                            {
                                cmd.Parameters.Add(new SqlParameter("@RequisitionNumber", record.FinancialNumber));
                                // cmd.Parameters.Add(new SqlParameter("@RequisitionNumber", DBNull.Value));
                            }
                            else
                            {
                                cmd.Parameters.Add(new SqlParameter("@RequisitionNumber", record.RequisitionNumber));
                            }
                        }
                        else
                            cmd.Parameters.Add(new SqlParameter("@RequisitionNumber", DBNull.Value));

                        if (!(string.IsNullOrEmpty(record.FinancialNumber)))
                            cmd.Parameters.Add(new SqlParameter("@FinancialNumber", record.FinancialNumber));
                        else
                            cmd.Parameters.Add(new SqlParameter("@FinancialNumber", DBNull.Value));
                        if (!(string.IsNullOrEmpty(record.AccessionNumber)))
                            cmd.Parameters.Add(new SqlParameter("@AccessionNumber", record.AccessionNumber));
                        else
                            cmd.Parameters.Add(new SqlParameter("@AccessionNumber", DBNull.Value));


                        cmd.ExecuteNonQuery();
                    }
                    // Do work here; connection closed on following line.  
                }
            }
            catch (Exception ex)
            {
                AL.TraceLoggerInstance.TraceError($"Adding records to sql sever:{ex.Message}");
                AL.TraceLoggerInstance.TraceErrorConsole($"Adding records to sql sever:{ex.Message}");
                errorMessage += $"Adding records to sql sever:{ex.Message}<br/>";
                throw new Exception($"Adding records to sql sever:{ex.Message}");
            }
            TotalRecodsAdded++;
        }




        static string CleanString(string value)
        { return (string.IsNullOrWhiteSpace(value) ? null : value.Replace('\'', ' ')); }




        static List<string> GetHL7Files(string dir)
        {
            List<string> hl7Files = new List<string>();
            foreach (string hl7File in Directory.GetFiles(dir))
            {
                try
                {
                    using (Stream stream = new FileStream(hl7File, FileMode.Open, FileAccess.Read))
                    { }
                    hl7Files.Add(hl7File);
                }
                catch
                {
                    AL.TraceLoggerInstance.TraceError("Unable to access " + hl7File + " skipping");
                    AL.TraceLoggerInstance.TraceErrorConsole("Unable to access " + hl7File + " skipping");

                }
            }
            return hl7Files;
        }

        /// <summary>
        /// Send an email to support
        /// </summary>
        /// <param name="pauseOnError">Pause and wait for user input if sendign the email fails</param>
        static void SendEmail(string subject, string body, bool pauseOnError, MailPriority priority)
        {
            try
            {
                AL.TraceLoggerInstance.TraceInformation("Sending email");
                AL.TraceLoggerInstance.TraceInformationConsole("Sending email");

                SmtpClient mailClient = new SmtpClient(EmailSettings.ServerAddress);
                mailClient.Port = EmailSettings.ServerPort;
                mailClient.Credentials = new System.Net.NetworkCredential(EmailSettings.SenderUserName, EmailSettings.SenderPassword);
                mailClient.EnableSsl = true;
                MailMessage mailMessage = new MailMessage();
                mailMessage.Sender = new MailAddress(EmailSettings.SenderUserName);
                mailMessage.From = new MailAddress(EmailSettings.SenderFrom);
                mailMessage.To.Add(EmailSettings.Recipients);
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = priority;
                mailMessage.Subject = "e-Docs NYP server: " + Assembly.GetExecutingAssembly().GetName().Name + " " + subject;
                mailMessage.Body = body;

                mailClient.Send(mailMessage);
                AL.TraceLoggerInstance.TraceInformation("Email sent");
            }
            catch (Exception ex)
            {
                AL.TraceLoggerInstance.TraceError(((System.Net.Mail.SmtpException)(ex)).StatusCode.ToString());
                AL.TraceLoggerInstance.TraceError(ex.GetType().ToString());
                AL.TraceLoggerInstance.TraceError(ex.Message);
                AL.TraceLoggerInstance.TraceError(ex.StackTrace);
                AL.TraceLoggerInstance.TraceError($"Sending email {ex.Message}");
                if (pauseOnError == true)
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("WARNING:");
                    Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name
                                    + " encountered an unexpected message and was not able to send an email alert.");
                    Console.WriteLine();
                    Console.WriteLine("Please contact support.");
                    Console.ReadLine();
                }
            }
        }

        static async Task DeleteEmptyDirs(string dir)
        {
            AL.TraceLoggerInstance.TraceInformation($"Checking for empty dirs unde {dir}");
            AL.TraceLoggerInstance.TraceInformationConsole($"Checking for empty dirs undeR {dir}");
            if (String.IsNullOrEmpty(dir))
                throw new ArgumentException(
                    "Starting directory is a null reference or an empty string",
                    "dir");

            try
            {
                foreach (var d in Directory.EnumerateDirectories(dir))
                {
                    DeleteEmptyDirs(d).ConfigureAwait(true).GetAwaiter().GetResult();
                }

                var entries = Directory.EnumerateFileSystemEntries(dir);

                if (!entries.Any())
                {
                    try
                    {
                        AL.TraceLoggerInstance.TraceInformation($"Deleting empty dir {dir}");
                        AL.TraceLoggerInstance.TraceInformationConsole($"Deleting empty dir {dir}");
                        Directory.Delete(dir);
                    }
                    catch (UnauthorizedAccessException ua)
                    {
                        AL.TraceLoggerInstance.TraceError($"Error Deleting empty dir {dir} UnauthorizedAccessException {ua.Message}");
                        AL.TraceLoggerInstance.TraceInformationConsole($"Deleting empty dir {dir} UnauthorizedAccessException {ua.Message}");
                        errorMessage += $"Deleting empty dir {dir} UnauthorizedAccessException {ua.Message}<br/>";

                    }
                    catch (DirectoryNotFoundException nf)
                    {
                        AL.TraceLoggerInstance.TraceError($"Error Deleting empty dir {dir} DirectoryNotFoundException {nf.Message}");
                        AL.TraceLoggerInstance.TraceInformationConsole($"Deleting empty dir {dir} DirectoryNotFoundException {nf.Message}");
                        errorMessage += $"Deleting empty dir {dir} DirectoryNotFoundException {nf.Message}<br/>";
                    }
                    catch (Exception ex)
                    {
                        AL.TraceLoggerInstance.TraceError($"Error Deleting empty dir {dir} exception {ex.Message}");
                        AL.TraceLoggerInstance.TraceInformationConsole($"Deleting empty dir {dir} exception {ex.Message}");
                        errorMessage += $"Deleting empty dir {dir} exception {ex.Message}<br/>";
                    }
                }
            }
            catch (UnauthorizedAccessException ua)
            {
                AL.TraceLoggerInstance.TraceError($"Error Deleting empty dir {dir} UnauthorizedAccessException {ua.Message}");
                AL.TraceLoggerInstance.TraceInformationConsole($"Deleting empty dir {dir} UnauthorizedAccessException {ua.Message}");
                errorMessage += $"Deleting empty dir {dir} UnauthorizedAccessException {ua.Message}<br/>";
            }
            catch (Exception ex)
            {
                AL.TraceLoggerInstance.TraceError($"Error Deleting empty dir {dir} exception {ex.Message}");
                AL.TraceLoggerInstance.TraceInformationConsole($"Deleting empty dir {dir} exception {ex.Message}");
                errorMessage += $"Deleting empty dir {dir} exception {ex.Message}<br/>";
            }
        }
    }
}
