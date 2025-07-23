using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Models;
using BinMonitorAppService.Constants;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using Edocs.WebApi.ApiClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AuditLogsController : Controller
    {

        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;

        public AuditLogsController(IConfiguration config, IEmailSettings email)
        {
            configuration = config;
            emailSettings = email;
        }
        // GET: api/<controller>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<controller>/5
        [HttpGet("{spName}")]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<JsonResult> OnGet(string spName)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
               // spName = ConstNypLabReqs.GetHttpValue(spName, "=");
                return ApiNypAuditLogs.NypAuditLogsInstance.GetAuditLogsComBox(spName, configuration.GetConnectionString(ConstNypLabReqs.AuditLogsCloudConnectionString)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running Upload Audit Logs get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"For stored procedure: {spName} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);
                throw new Exception(ex.Message);
            }

        }

        // POST api/<controller>
        [HttpPost]
        public async Task Post([FromBody]AuditLogsModel value)
        {
            try
            {
                ApiNypAuditLogs.NypAuditLogsInstance.UpLoadNypAuditLogs(value, configuration.GetConnectionString(ConstNypLabReqs.AuditLogsCloudConnectionString)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running Upload Audit Logs post run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"For model AuditLogsModel file url: {value.AuditLogUrl.ToString()} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);
                throw new Exception(ex.Message);
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{spName}")]
        public async Task<JsonResult> Put(string spName, [FromBody] AuditLogsModel value)
        {
            try
            {
               return ApiNypAuditLogs.NypAuditLogsInstance.GetNypAuditLogs(value, configuration.GetConnectionString(ConstNypLabReqs.AuditLogsCloudConnectionString),spName).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running Upload Audit Logs put run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"For model AuditLogsModel start date: {value.AuditLogDate.ToString()} for end date: {value.AuditLogUpLoadDate.ToString()} for sp {spName} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);
                throw new Exception(ex.Message);
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
