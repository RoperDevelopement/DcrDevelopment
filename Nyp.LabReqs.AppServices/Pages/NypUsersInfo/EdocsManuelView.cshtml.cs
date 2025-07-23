using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.Extensions.Configuration;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using EDocs.Nyp.LabReqs.AppService.Logging;
using Edocs.Upload.Azure.Blob.Storage;
namespace EDocs.Nyp.LabReqs.AppServices
{
    public class EdocsManuelViewModel : PageModel
    {
        
        private ILog auditLogs;
        public EdocsManuelViewModel(ILog logConfig)
        {
            auditLogs = logConfig;
        }
        private async Task GetLogInformaiton()
        {

            // log.AppName = Assembly.GetEntryAssembly().GetName().Name;
            // log.GetLogFileName($"Nyp_AuditLog_{log.AppName}");
            //log.UserName = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(false);
            //AzureBlobStorage.BlobStorageInstance.InitIConfiguration();
            //log.AzureStorageBlobContainer = $"{AzureBlobStorage.BlobStorageInstance.AzureBlobShareAuditLogsUpLoad}/{DateTime.Now.ToString("MM_dd_yyyy")}";
            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task OnGetAsync()
        {
            if (!(User.Identity.IsAuthenticated))
            {
                 Redirect("/NypUsersInfo/LoginView?returnUrl=/NypUsersInfo/EdocsManuelView");
                return;
            }
            ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
            if (ViewData["CWID"] == null)
            {
                Redirect("/NypUsersInfo/LoginView?returnUrl=/LabReqs/EdocsManuelView");
                return;
            }
                
            try
            {
                LabReqHelpers.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start EdocsManuelViewModel onGet");
                GetViewAudLogsAdmin();
                AzureBlobStorage.BlobStorageInstance.InitIConfiguration();
                string[] manuel = AzureBlobStorage.BlobStorageInstance.AzureBlobShareManual.Split('/');
                if (manuel.Length != 2)
                {
                    throw new Exception($"Invalid contanier format {AzureBlobStorage.BlobStorageInstance.AzureBlobShareManual} expecting container format containername/usermaneulname.pdf");
                }
                auditLogs.LogInformation($"Getting LabReqs user manuel {manuel[1]} in container: {manuel[0]}");
                byte[] pdfFile = AzureBlobStorage.BlobStorageInstance.DownloadFileBytesAzureBlob(manuel[1], manuel[0]).ConfigureAwait(false).GetAwaiter().GetResult();
                if (pdfFile == null)
                    throw new Exception($"Invalid pdf file {manuel[1]}");
                Response.Headers.Add("Content-Disposition", $"inline; filename=\"{manuel[1]}\"");

                Response.Body.WriteAsync(pdfFile, 0, pdfFile.Length).ConfigureAwait(true).GetAwaiter().GetResult();
               
        
                    auditLogs.LogInformation($"End EdocsManuelViewModel total time: {LabReqHelpers.StopStopWatch()} ms");

            }
            catch(Exception ex)
            {
                auditLogs.LogError($"End EdocsManuelViewModel total time: {LabReqHelpers.StopStopWatch()} ms {ex.Message}");
                Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss= {ex.Message}");
                return;
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