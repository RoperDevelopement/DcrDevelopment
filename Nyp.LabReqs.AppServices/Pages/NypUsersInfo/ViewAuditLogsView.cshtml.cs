using System;

using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using Edocs.Upload.Azure.Blob.Storage;
using EDocs.Nyp.LabReqs.AppService.Logging;

namespace EDocs.Nyp.LabReqs.AppServices.Pages.NypUsersInfo
{
    public class ViewAuditLogsViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog log;
        private string WebApiUrl
        { get { return configuration.GetValue<string>("LabResApi").ToString(); } }

        public string TotalQueryTime
        { get; set; }
        public IList<AuditLogCsvModel> LogCsvModels
        { get; set; }
        public string AuditLogCsvFname
        { get; set; }
        public ViewAuditLogsViewModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            log = logConfig;
        }
        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }


        public async Task<IActionResult> OnGetAsync( )
        {
            var qString = Request.QueryString;
            try
            {
                if (!(User.Identity.IsAuthenticated))
                {
                    if (qString.HasValue)
                    {
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/NypUsersInfo/ViewAuditLogsView{qString.Value}");
                    }
                    else
                        return Redirect("/NypUsersInfo/LoginView?returnUrl=?returnUrl=/NypUsersInfo/ViewAuditLogsView");
                }
                ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
                if (ViewData["CWID"] == null)
                    return Redirect("/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpGrantReceiptsView");
                LabReqHelpers.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();

                log.LogInformation("Start ViewAuditLogsView");
                if(qString.HasValue)
                {
                    log.LogInformation($"ViewAuditLogsView query string {qString.Value}");
                    GetAuditLogInfo(Request.Query["ViewAuditLog"].ToString()).ConfigureAwait(false).GetAwaiter().GetResult(); 
                 
                }
                GetViewAudLogsAdmin();
                TotalQueryTime = LabReqHelpers.StopStopWatch();
                log.LogInformation($"End LookUpGrantReceiptsViewModel total time: {TotalQueryTime}");

            }
            catch (Exception ex)
            {
                log.LogError($"ViewAuditLogsViewModel {LabReqHelpers.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model ViewAuditLogsViewModel OngettAsync {ex.Message}");
            }
            return Page();
        }
        private async Task GetAuditLogInfo(string auditLogFileName)
        {
            log.LogInformation($"Method GetAuditLogInfo ViewAuditLogsView getting audit log file {auditLogFileName}");
            string azureContainer = Path.GetDirectoryName(auditLogFileName);
            azureContainer = azureContainer.Substring(azureContainer.IndexOf(AzureBlobStorage.BlobStorageInstance.AzureBlobShareAuditLogs));
            AuditLogCsvFname = Path.GetFileName(auditLogFileName);
            log.LogInformation($"Method Getauditloginfo downloading file {AuditLogCsvFname} from azure container {azureContainer}");
            MemoryStream auditLog = AzureBlobStorage.BlobStorageInstance.DownloadTextMemoryStreamAzureAppendBlob(AuditLogCsvFname, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
            ReadAuditLog(auditLog).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        private async Task ReadAuditLog(MemoryStream memoryStream)
        {
            log.LogInformation($"Method ReadAuditLog displaying audit log file {AuditLogCsvFname}");
            LogCsvModels = new List<AuditLogCsvModel>();
            //byte[] fs = new byte[memoryStream.Length];
            //fs = ;
            string result = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
               
            using (StringReader sr = new StringReader(result))
            {
                string audLogStr = string.Empty;
                audLogStr= sr.ReadLineAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                while((audLogStr = sr.ReadLineAsync().ConfigureAwait(true).GetAwaiter().GetResult()) != null)
                {
                    if (audLogStr.StartsWith("Application Name"))
                        continue;
                    string[] csvInStr = audLogStr.Split(",");
                    AuditLogCsvModel auditLogCsv = new AuditLogCsvModel();
                    if (csvInStr.Length == 7)
                    {
                        auditLogCsv.AuditLogApplicationName = csvInStr[0];
                        auditLogCsv.AuditLogDate = csvInStr[1];
                        auditLogCsv.AuditLogMessageType = csvInStr[2];
                        auditLogCsv.AuditLogMessage = csvInStr[3].Trim();
                        auditLogCsv.Cwid = csvInStr[4];
                        if (csvInStr[5].ToLower().StartsWith("rd"))
                            auditLogCsv.AuditLogMachine = "Azure Cloud App Service";
                        else
                            auditLogCsv.AuditLogMachine = csvInStr[5];
                        if (csvInStr[6].ToLower().StartsWith("iis"))
                            auditLogCsv.AuditLogDomain = "Azure Cloud ISS";
                        else
                            auditLogCsv.AuditLogDomain = csvInStr[6];


                    }
                    else
                    {
                        log.LogWarning($"Method ReadAuditLog audit log {AuditLogCsvFname} line invalid {csvInStr[0]}");
                        auditLogCsv.AuditLogApplicationName = csvInStr[0];
                        auditLogCsv.AuditLogDate = "Error";
                        auditLogCsv.AuditLogMessageType = "Error";
                        auditLogCsv.AuditLogMessage = "Error";
                        auditLogCsv.Cwid = "Error";
                        auditLogCsv.AuditLogMachine = "Error";
                        auditLogCsv.AuditLogDomain = "Error";

                    }
                    LogCsvModels.Add(auditLogCsv);
                }

            }
        }
        private void GetViewAudLogsAdmin()
        {
            LabReqsIsAdminVeiwAuditLogs IsAdminVeiwAuditLogs = LabReqsConstants.GetSessionVariables.SessionVarInstance.GetJsonSessionObject<LabReqsIsAdminVeiwAuditLogs>(HttpContext.Session, ConstNypLabReqs.SessionIsAdminVeiwAuditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData["IsAdmin"] = IsAdminVeiwAuditLogs.IsAdmin;
            ViewData["ViewAuditLogs"] = IsAdminVeiwAuditLogs.ViewAuditLogs;
            ViewData["EditLRDocs"] = IsAdminVeiwAuditLogs.EditLRDocs;
        }

    }
}