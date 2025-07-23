using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ITfoxtec.Identity.Saml2;
using Microsoft.AspNetCore.Hosting;
using ITfoxtec.Identity.Saml2.Schemas;
using ITfoxtec.Identity.Saml2.MvcCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using EDocs.Nyp.LabReqs.AppService.Logging;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;

namespace EDocs.Nyp.LabReqs.AppServices.Pages
{
    public class IndexModel : PageModel
    {
        private IHttpContextAccessor accessor;
        private readonly Saml2Configuration config;
        private readonly IEmailSettings emailSettings;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration configuration;
        private ILog log;
        public IndexModel(ILog logConfig, IHttpContextAccessor httpContextAccessor, IOptions<Saml2Configuration> configAccessor, IConfiguration labRecConfig, IEmailSettings email, IWebHostEnvironment webHostEnvironment)
        {
            log = logConfig;
            accessor = httpContextAccessor;
            config = configAccessor.Value;
            configuration = labRecConfig;
            emailSettings = email;
            env = webHostEnvironment;
        }
      
        private string WebApiUrl
        { get { return configuration.GetValue<string>("LabResApi").ToString(); } }

        private string NypEmailTo
        { get { return configuration.GetSection("NypContacts").GetValue<string>("EmailTo").ToString(); } }
        private string NypEmailCC
        { get { return configuration.GetSection("NypContacts").GetValue<string>("EmailCC").ToString(); } }
        private string EmailHtmlFile
        { get { return configuration.GetSection("NypContacts").GetValue<string>("EmailHtmlFile").ToString(); } }
        
        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async   Task<IActionResult> OnGetAsync()
        {
            // throw new   System.Runtime.InteropServices.ExternalException(404, "Not found");
            //Response.Clear();
            //Response.StatusCode = 404;
            //accessor.HttpContext. HttpContext.Current.ApplicationInstance.CompleteRequest();

            if (!(User.Identity.IsAuthenticated))
            {
            
                return Redirect("/NypUsersInfo/LoginView");
            }
            try
            {

            
            ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
            GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
            GetAudLogsAdmin();
            var clientIPAddress = accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            log.LogInformation($"User: {ViewData["CWID"]} logged in from ipaddress: {clientIPAddress}");
            }
            catch(Exception ex)
            {
                log.LogError($"Class Get IndexModel total time: {LabReqHelpers.StopStopWatch()} ms {ex.Message}");
                //  RedirectToPage($"/Error?ErrMEss=Model SearchLabReqsPartialViewModel  OngettAsync {ex.Message}");
                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model Class Get IndexModel OngettAsync {ex.Message}");
            }
            return Page();
        }
        private void GetAudLogsAdmin()
        {
             LabReqsIsAdminVeiwAuditLogs IsAdminVeiwAuditLogs = LabReqsConstants.GetSessionVariables.SessionVarInstance.GetJsonSessionObject<LabReqsIsAdminVeiwAuditLogs>(HttpContext.Session, ConstNypLabReqs.SessionIsAdminVeiwAuditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData["IsAdmin"] = IsAdminVeiwAuditLogs.IsAdmin;
            ViewData["ViewAuditLogs"] = IsAdminVeiwAuditLogs.ViewAuditLogs;
            ViewData["EditLRDocs"] = IsAdminVeiwAuditLogs.EditLRDocs;
        }
        private async Task NoAccess(string cwid,string emailAddress)
        {
            EmailService emailService = new EmailService();
            emailSettings.EmailTo = NypEmailTo;
            
            
            if(!(string.IsNullOrWhiteSpace(emailSettings.EmailCC)))
            {
                emailSettings.EmailCC = $"{emailSettings.EmailCC};{NypEmailCC};{emailAddress}";
             
            }
            else
            {
                if(!(string.IsNullOrWhiteSpace(NypEmailCC)))
                {
                    emailSettings.EmailCC = $"{NypEmailCC};{emailAddress}";
                }
                else
                    emailSettings.EmailCC = $"{emailAddress};";
            }

            
            //emailService.SendHtmlEmail($"{env.WebRootPath}//{EmailHtmlFile}",$"New Request LabReqs System CWID {cwid}", emailSettings, cwid,emailAddress);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            return Page();
        }
    }
}
