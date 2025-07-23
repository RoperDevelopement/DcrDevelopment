using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using Edocs.Upload.Azure.Blob.Storage;
using EDocs.Nyp.LabReqs.AppService.Logging;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using EDocs.Nyp.LabReqs.AppServices.Models;

namespace EDocs.Nyp.LabReqs.AppServices
{
    public class DisplayPdfFilePartialViewModel : PageModel
    {
        //private readonly IConfiguration configuration;
        private  ILog log;
        //private string AzureBlobAccountKey
        //{ get { return configuration.GetSection("AzureStorageBlobSettings").GetValue<string>("AzureBlobAccountKey").ToString(); } }

        //private string AzureBlobAccountName
        //{ get { return configuration.GetSection("AzureStorageBlobSettings").GetValue<string>("AzureBlobAccountName").ToString(); } }

        //private string AzureBlobStorageConnectionString
        //{ get { return configuration.GetSection("AzureStorageBlobSettings").GetValue<string>("AzureBlobStorageConnectionString").ToString(); } }
        //private string AzureBlobShareName
        //{ get { return configuration.GetSection("AzureStorageBlobSettings").GetValue<string>("AzureBlobShareLabRecs").ToString(); } }

        public DisplayPdfFilePartialViewModel(ILog logConfig)
        {
            log = logConfig;
        }
        private async Task GetLogInformaiton()
        {
            // log.AppName = Assembly.GetEntryAssembly().GetName().Name;
            // log.GetLogFileName($"Nyp_AuditLog_{log.AppName}");
            //log.UserName = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(false);
            //AzureBlobStorage.BlobStorageInstance.InitIConfiguration();
            //log.AzureStorageBlobContainer = $"{AzureBlobStorage.BlobStorageInstance.AzureBlobShareAuditLogsUpLoad}/{DateTime.Now.ToString("MM_dd_yyyy")}";
           log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task OnGet()
        {
            var qString = Request.QueryString;
            try
            {
                if (!(User.Identity.IsAuthenticated))
                {
                    RedirectToPage($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/Partial_Pages/DisplayPdfFilePartialView{qString.Value}");
                }
                ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
                if (ViewData["CWID"] == null)
                     RedirectToPage("/Index");
                GetViewAudLogsAdmin();

                if (qString.HasValue)
                {
                   await GetLogInformaiton().ConfigureAwait(false);
                    // AzureBlobStorage.BlobStorageInstance.AzureBlobAccountKey = AzureBlobAccountKey;
                    //AzureBlobStorage.BlobStorageInstance.AzureBlobAccountName = AzureBlobAccountName;
                    // AzureBlobStorage.BlobStorageInstance.AzureBlobStorageConnectionString = AzureBlobStorageConnectionString;
                    
                    string qStringValue = Request.Query["pdfurl"];
                    log.LogInformation($"Opeing pdf file {qStringValue}");
                    AzureBlobStorage.BlobStorageInstance.InitIConfiguration();
                    string cloudStorage = Path.GetDirectoryName(qStringValue);
                    cloudStorage = cloudStorage.Substring(cloudStorage.IndexOf(AzureBlobStorage.BlobStorageInstance.AzureBlobShareName));
                    string pdfFName = Path.GetFileName(qStringValue);
                    log.LogInformation($"Getting Pdf file: {pdfFName} for cloud storage {cloudStorage}");
                    Response.Headers.Add("Content-Disposition", $"inline; filename=\"{pdfFName}\"");
                    byte[] pdfFile = AzureBlobStorage.BlobStorageInstance.DownloadFileBytesAzureBlob(pdfFName, cloudStorage).ConfigureAwait(false).GetAwaiter().GetResult();
                    if(pdfFile == null)
                    {
                        log.LogError($"Pdf file not found: {pdfFName} cloud storage {cloudStorage}");
                        throw new Exception($"Pdf file not found: {pdfFName} cloud storage {cloudStorage}");
                    }
                    log.LogInformation($"Opening Pdf file: {pdfFName}");
                    await Response.Body.WriteAsync(pdfFile, 0, pdfFile.Length).ConfigureAwait(true);
                }
                else
                    
                    throw new Exception("Invalid query string");
            }
            catch(Exception ex)
            {
                log.LogError($"Opeing pdf file for query sting {qString} {ex.Message}");
                RedirectToPage($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Opeing pdf file for query sting {qString}  OngettAsync {ex.Message}");
            }


 
        }
        private void GetViewAudLogsAdmin()
        {
            LabReqsIsAdminVeiwAuditLogs IsAdminVeiwAuditLogs = LabReqsConstants.GetSessionVariables.SessionVarInstance.GetJsonSessionObject<LabReqsIsAdminVeiwAuditLogs>(HttpContext.Session, ConstNypLabReqs.SessionIsAdminVeiwAuditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData["IsAdmin"] = IsAdminVeiwAuditLogs.IsAdmin;
            ViewData["ViewAuditLogs"] = IsAdminVeiwAuditLogs.ViewAuditLogs;
            ViewData["EditLRDocs"] = IsAdminVeiwAuditLogs.EditLRDocs;
        }
        private async Task<string> GetFileName(string fileName)
        {
            return Path.GetFileName(fileName);
        }
      
    }
}