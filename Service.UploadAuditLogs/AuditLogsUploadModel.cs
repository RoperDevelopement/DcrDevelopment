using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Edocs.Service.UploadAuditLogs
{
    public class AuditLogsUploadModel : AuditLogsModel
    {

        public const string AuditLogsController = "AuditLogs/";
        public static IList<string> UploadList
        { get; set; }
        public static AuditLogsUploadModel GetAuditLogsModels(string csvLine, string csvFileName)
        {
            string[] values = csvLine.Split(',');
            AuditLogsUploadModel auditLogs = new AuditLogsUploadModel();
            try
            {
                if (values.Length < 6)
                {
                    auditLogs.AuditLogApplicationName = "na";
                }
                else
                {
                    string addList = $"{values[0]}{Convert.ToDateTime(values[1]).ToString("MMddyyyy")}{values[2]}{values[4]}";
                    if (!UploadList.Contains(addList))
                    {
                        UploadList.Add(addList);
                        auditLogs.AuditLogApplicationName = values[0];
                        auditLogs.AuditLogDate = Convert.ToDateTime(values[1]);

                        auditLogs.AuditLogMessageType = values[2];
                        auditLogs.Cwid = values[4];
                    }
                    else
                        auditLogs.AuditLogApplicationName = "na";
                }
            }
            catch (Exception ex)
            {
                TL.TraceLoggerInstance.TraceError($"Processing scv file {csvFileName} file length {values.Length} {ex.Message}");
                auditLogs.AuditLogApplicationName = "na";
            }
            return auditLogs;
        }
    }
}
