using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Models;

using System.ComponentModel.DataAnnotations;
using BinMonitorAppService.Constants;
using BinMonitorAppService.ApiClasses;

using Microsoft.AspNetCore.Http;
using BinMonitorAppService.Logging;
namespace BinMonitorAppService
{
    public class EdocsManuelModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog auditLogs;
        public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
        { get; set; }

        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }

        public EdocsManuelModel(ILog logConfig, IConfiguration config)
        {
            auditLogs = logConfig;
            configuration = config;
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
            InitAuditLogs.StartStopWatch();
            GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
            auditLogs.LogInformation("Start EdocsManuelModel onGet");
            if (!(User.Identity.IsAuthenticated))
            {
                auditLogs.LogInformation($"End EdocsManuelModel onGet Session expired return url /BinUsers/LoginView?returnUrl=/BinMonitor/EdocsManuel total time: {InitAuditLogs.StopStopWatch()} ms");
                Redirect("/BinUsers/LoginView?returnUrl=/BinMonitor/EdocsManuel");
                return;
            }

            ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
            if (ViewData["CWID"] == null)
            {
                auditLogs.LogInformation($"End EdocsManuelModel onGet Session expired return url /BinUsers/LoginView?returnUrl=/BinMonitor/EdocsManuel total time: {InitAuditLogs.StopStopWatch()} ms");
                Redirect("/BinUsers/LoginView?returnUrl=/BinMonitor/EdocsManuel");
            }
            try
            {
                SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, auditLogs, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                GetViewData().ConfigureAwait(true).GetAwaiter().GetResult();
                AzureBlobStorage.BlobStorageInstance.InitIConfiguration();
                string[] manuel = AzureBlobStorage.BlobStorageInstance.AzureBlobShareManual.Split('/');
                if (manuel.Length != 2)
                {
                    throw new Exception($"Invalid contanier format {AzureBlobStorage.BlobStorageInstance.AzureBlobShareManual} expecting container format containername/usermaneulname.pdf");
                }
                auditLogs.LogInformation($"Getting BinMonitor user manuel {manuel[1]} in container: {manuel[0]}");
                byte[] pdfFile = AzureBlobStorage.BlobStorageInstance.DownloadFileBytesAzureBlob(manuel[1], manuel[0]).ConfigureAwait(false).GetAwaiter().GetResult();
                if (pdfFile == null)
                    throw new Exception($"Invalid pdf file {manuel[1]}");
                Response.Headers.Add("Content-Disposition", $"inline; filename=\"{manuel[1]}\"");

                Response.Body.WriteAsync(pdfFile, 0, pdfFile.Length).ConfigureAwait(true).GetAwaiter().GetResult();
                auditLogs.LogInformation($"End EdocsManuelModel total time: { InitAuditLogs.StopStopWatch()} ms");
            }
            catch (Exception ex)
            {
                Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model BinManagerHomeModel OnGetAsync {ex.Message}");
                return;
            }
           
        }
        private async Task GetViewData()
        {
            ViewData[SqlConstants.ViewDataChangeCategories] = SpectrumMonitorMenuRightsModel.Categories;
            ViewData[SqlConstants.ViewDataEmailReports] = SpectrumMonitorMenuRightsModel.EmailReports;
            ViewData[SqlConstants.ViewDataRunReports] = SpectrumMonitorMenuRightsModel.RunReports;

        }
    }
}