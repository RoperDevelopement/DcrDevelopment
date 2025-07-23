using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Scanquire.Public.ArchivesConstants;
using EDL = EdocsUSA.Utilities.Logging;
namespace Scanquire.Clients.NYP
{
    public static class BatchHelper
    {
        public const string  JsonSettingsCsnNumber = "CsnNumber";
        public const string JsonSettingsIndexNumber = "IndexNumber";
        public static string GetBatchDir(string archiveRootDir, string batchId)
        {
            if (string.IsNullOrWhiteSpace(archiveRootDir))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("AzureBatchRootDir cannot be empty");
                throw new ArgumentNullException("archiveRootDir"); 
            }

            if (string.IsNullOrWhiteSpace(batchId))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("Batch id cannot be empty");
                throw new ArgumentNullException("batchId"); 
            }

            return Path.Combine(archiveRootDir, batchId);
        }
        public static string  RemoveClientID(string indexNum)
        {
            string retString = indexNum;
            
            if (indexNum.Length >= 15)
            {
                bool removeClinetID = false;
                int strStart = indexNum.Length - 10;
                if (retString.StartsWith("00"))
                {
                    
                   
                    for(int i=2; i< strStart;i++ )
                    {
                        if(indexNum[i] != '0' )
                        {
                            removeClinetID = true;
                            break;
                        }
                        if (i >= 4)
                            break;
                    }
                }

                if (removeClinetID)
                    {
                        string zeros = string.Empty;

                        retString = indexNum.Remove(0, strStart);
                        string pad = zeros.PadLeft(strStart, '0');
                        retString = $"{pad}{retString}";
                    }
                    else
                        return indexNum;
               
            }
                
            return retString;

        }
        public static string GetBatchId(string batchDir)
        { return new DirectoryInfo(batchDir).Name; }

        public static void DeleteBatch(string batchDir)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Deleting batch directory " + batchDir);
            Directory.Delete(batchDir, true);
            if (Directory.Exists(batchDir))
            { EDL.TraceLogger.TraceLoggerInstance.TraceWarning("Could not delete batch directory " + batchDir); }
        }
    }



    public class SharepointBatchSettings
    {
        public string SiteUrl { get; set; }
        public string LibraryName { get; set; }
        public string FolderRelativeUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        private Dictionary<string, string> _CommonFields = new Dictionary<string, string>();
        public Dictionary<string, string> CommonFields
        {
            get { return _CommonFields; }
            set { _CommonFields = value; }
        }

        public SharepointBatchSettings() { }

        public SharepointBatchSettings(string siteUrl, string libraryName, string folderRelativeUrl, string userName, string password, Dictionary<string, string> commonFields)
        {
            this.SiteUrl = siteUrl;
            this.LibraryName = libraryName;
            this.FolderRelativeUrl = folderRelativeUrl;
            this.UserName = userName;
            this.Password = password;
            this.CommonFields = commonFields;

        }
    }
    public static class JsonSettings
    {
        public static Dictionary<string, object> GetJsonBatchSettings(string currentBatchId, string receiptDate, string receiptStation, string category, string azureDataBaseName, string azureShareName
                                            , string azureTableName, string AzureSPName, string azureWebApiController)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting azure batch imformation for batch id:{currentBatchId}");
            Dictionary<string, object> batchSettings = new Dictionary<string, object>()
                {
                  {JsonsFieldConstants.JsonFieldScanBatch, currentBatchId },
                  {JsonsFieldConstants.JsonFieldReceiptDate, receiptDate},
                 {JsonsFieldConstants.JsonFieldReceiptStation,receiptStation  },
                 {JsonsFieldConstants.JsonFieldCategory, category },
                {JsonsFieldConstants.JsonFieldAzureDataBaseName, azureDataBaseName},
                {JsonsFieldConstants.JsonFieldScanOperator, EdocsUSA.Utilities.Logging.TraceLogger.TraceLoggerInstance.UserName},
                {JsonsFieldConstants.JsonFieldAzureShareName, azureShareName},
                {JsonsFieldConstants.JsonFieldAzureTableName, azureTableName},
                {JsonsFieldConstants.JsonFieldAzureSPName, AzureSPName},
                {JsonsFieldConstants.JsonFieldAzureWebApiController, azureWebApiController},
                {JsonsFieldConstants.JsonFieldScanDate, DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")}



        };
            return batchSettings;



        }
    }

    public class SharepointBatchRecord
    {
        public string FileName { get; set; }

        private Dictionary<string, string> _Fields = new Dictionary<string, string>();
        public Dictionary<string, string> Fields
        {
            get { return _Fields; }
            set { _Fields = value; }
        }

        public SharepointBatchRecord() { }

        public SharepointBatchRecord(string fileName) : this()
        { this.FileName = fileName; }

        public SharepointBatchRecord(string fileName, Dictionary<string, string> fields) : this(fileName)
        { this.Fields = fields; }
    }

    public static class SharepointBatchHelper
    {
        static JavaScriptSerializer Serializer = new JavaScriptSerializer();

        public static void WriteSettings(string batchDir, SharepointBatchSettings settings)
        {
            
            string batchId = BatchHelper.GetBatchId(batchDir);
            Directory.CreateDirectory(batchDir);

            string settingsFileName = batchId + "_settings";
            string settingsFileNameWithExt = Path.ChangeExtension(settingsFileName, "json");
            string settingsFilePath = Path.Combine(batchDir, settingsFileNameWithExt);
            if (File.Exists(settingsFilePath))
            { throw new Exception("Settings file already exists, at " + settingsFilePath + " cannot overwrite"); }
            Trace.TraceInformation("Writing settings to " + settingsFilePath);
            string entry = Serializer.Serialize(settings);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Writting settings for azure batch imformation to file:{settingsFilePath}");
            File.WriteAllText(settingsFilePath, entry);
        }
        public static void WriteSettings(string batchDir, Dictionary<string, object> settings)
        {
            string batchId = BatchHelper.GetBatchId(batchDir);
            Directory.CreateDirectory(batchDir);

            string settingsFileName = batchId + "_settings";
            string settingsFileNameWithExt = Path.ChangeExtension(settingsFileName, "json");
            string settingsFilePath = Path.Combine(batchDir, settingsFileNameWithExt);
            if (File.Exists(settingsFilePath))
            { throw new Exception("Settings file already exists, at " + settingsFilePath + " cannot overwrite"); }
            Trace.TraceInformation("Writing settings to " + settingsFilePath);
            string entry = Serializer.Serialize(settings);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Writting settings for azure batch imformation to file:{settingsFilePath}");
            File.WriteAllText(settingsFilePath, entry);
        }

        public static SharepointBatchSettings ReadSettings(string batchDir)
        {
            string batchId = BatchHelper.GetBatchId(batchDir);
            if (Directory.Exists(batchDir) == false)
            { throw new DirectoryNotFoundException(batchDir); }

            string settingsFileName = batchId + "_settings";
            string settingsFileNameWithExt = Path.ChangeExtension(settingsFileName, "json");
            string settingsFilePath = Path.Combine(batchDir, settingsFileNameWithExt);
            if (File.Exists(settingsFilePath) == false)
            { throw new FileNotFoundException("Settings file not found at " + settingsFilePath); }
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Getting settings for azure batch to file:{settingsFilePath}");
            string settingsFileText = File.ReadAllText(settingsFilePath);
            return Serializer.Deserialize<SharepointBatchSettings>(settingsFileText);
        }

        public static void WriteRecord(string batchDir, SharepointBatchRecord record, byte[] fileData)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Batch dir is " + batchDir);
            string batchId = BatchHelper.GetBatchId(batchDir);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Batch dir is " + batchDir);

            if (Directory.Exists(batchDir) == false)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceError("Batch dir not found " + batchDir);
                throw new DirectoryNotFoundException(batchDir); 
            }

            string batchFileName = batchId + "_records";
            string batchFileNameWithExt = Path.ChangeExtension(batchFileName, "json");
            string batchFilePath = Path.Combine(batchDir, batchFileNameWithExt);

            string recordEntry = Serializer.Serialize(record);
            File.AppendAllLines(batchFilePath, new string[] { recordEntry });

            string recordFilePath = Path.Combine(batchDir, record.FileName);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Writing record data to " + batchFilePath);
            File.WriteAllBytes(recordFilePath, fileData);
        }

        public static SharepointBatchRecord[] ReadRecords(string batchDir)
        {
            string batchId = BatchHelper.GetBatchId(batchDir);
            if (Directory.Exists(batchDir) == false)
            { throw new DirectoryNotFoundException(batchDir); }

            string batchFileName = batchId + "_records";
            string batchFileNameWithExt = Path.ChangeExtension(batchFileName, "json");
            string batchFilePath = Path.Combine(batchDir, batchFileNameWithExt);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Reading records for " + batchFilePath);
            string[] recordLines = File.ReadAllLines(batchFilePath);
            return (from string recordLine in recordLines select Serializer.Deserialize<SharepointBatchRecord>(recordLine)).ToArray();
        }
    }


}
