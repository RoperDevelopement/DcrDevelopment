using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
using BinMonitorAppService.Models;
using BinMonitorAppService.ApiClasses;

using BinMonitorAppService.Constants;
using BinMonitorAppService.Logging;


namespace BinMonitorAppService.Pages.BinUsers
{
    public class ErrorLoggingInViewModel : PageModel
    {
        
        private readonly IConfiguration configuration;
        private   ILog log;
        private string NypEmailTo
        { get { return configuration.GetSection("NypContacts").GetValue<string>("EmailTo").ToString(); } }

        private string NypEmailCC
        { get { return configuration.GetSection("NypContacts").GetValue<string>("EmailCC").ToString(); } }
     //   public SpectrumMonitorMenuRightsModel SpectrumMonitorMenuRightsModel
       // { get; set; }
        public string EmailTo
            {get;set;}
        public string EmailSecondTo
        { get; set; }
        public IList<string> EmailCc
        { get; set; }
        public   ErrorLoggingInViewModel(IConfiguration config, ILog auditLogs)
        {
            configuration = config;
            log = auditLogs;
        }
        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public void OnGet()
        {
            GetLogInformaiton().ConfigureAwait(true).GetAwaiter().GetResult();
            log.LogInformation("Start ErrorLoggingInViewModel method Onget error user logging into labreqs");
            log.LogInformation($"Emails to {NypEmailTo} emails cc {NypEmailCC}");
          //  SpectrumMonitorMenuRightsModel = ApiSetUserProfile.UserProfileInstance.GetBMMenuRights(User.Identity.Name, HttpContext.Session, log, WebApiUrl).ConfigureAwait(false).GetAwaiter().GetResult();
            EmailCc = new List<string>();
            string[] emailsTO = NypEmailTo.Split(';');
            if(emailsTO.Length == 2)
            {
                EmailTo = emailsTO[0];
                EmailSecondTo = emailsTO[1];
            }
            else
                EmailTo = emailsTO[0];
            foreach(string cc in NypEmailCC.Split(';'))
            {
                EmailCc.Add(cc);
            }
        }
        
    }

}