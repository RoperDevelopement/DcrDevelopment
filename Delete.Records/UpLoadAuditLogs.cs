using Edocs.HelperUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS = Edocs.Upload.Azure.Blob.Storage.AzureBlobStorage;
using TL = EdocsUSA.Utilities.Logging.TraceLogger;

namespace Edocs.Delete.Records
{
    class UpLoadAuditLogs
    {
        private static UpLoadAuditLogs instance = null;

        public static UpLoadAuditLogs UpLoadAuditLogsInstance
        {
            get
            {
                if (instance == null)
                    instance = new UpLoadAuditLogs();
                return instance;
            }
        }
        UpLoadAuditLogs()
        {
        }
        public async Task<string> CopyAuditLogs(string auditLogFolder,string azContaier)
        {
            StringBuilder sb = new StringBuilder();
            if (Path.HasExtension(auditLogFolder))
                auditLogFolder = Path.GetDirectoryName(auditLogFolder);
            sb.AppendLine($"<p style={Constants.Quote}color:008000{Constants.Quote}>Uploading audit logs from folder {auditLogFolder}</p>");
            TL.TraceLoggerInstance.TraceInformation($"UpLoading audit Logs from folder {auditLogFolder}");
            TL.TraceLoggerInstance.CloseTraceFile();
            string fileName = string.Empty;
            foreach (var aLog in Utilities.GetDirFilesName(auditLogFolder))
            {
                try
                {
                    fileName = Path.Combine(auditLogFolder, aLog);
                string fileContents = Utilities.ReadFile(fileName);
                BS.BlobStorageInstance.UploadAzureBlobTextFile(aLog, azContaier, fileContents).ConfigureAwait(false).GetAwaiter().GetResult();
                Utilities.DeleteFile(fileName);
                }
                catch(Exception ex)
                {
                    sb.AppendLine($"<p style={Constants.Quote}color:red{Constants.Quote}>Error UpLoading audit log {fileName} {ex.Message}</p>");
                }
            }
            return sb.ToString();
        }
    }
}
