using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Edocs.WebApi.ApiClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class NypLabReqsUsersController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;

        public NypLabReqsUsersController(IConfiguration config, IEmailSettings email)
        {
            configuration = config;
            emailSettings = email;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5

        [HttpGet("{cwid}/{firstName}/{lastName}/{emailAddress}/{command}")]
        public async Task<JsonResult> Get(string cwid,string firstName,string lastName,string emailAddress, string command)
        {
            try
            {
                var jResult = ApiLabReqsUsers.NypLabReqUsersInstance.GetNypLabRecUserCwid(configuration.GetConnectionString(ConstNypLabReqs.LabRecsCloudConnectionString), cwid, firstName, lastName, emailAddress, command,false,false,false,false).ConfigureAwait(true).GetAwaiter().GetResult();
                
                return jResult;
                
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running inMonitorController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"cwid {cwid}, firstName {firstName} lastName {lastName} emailAddress {emailAddress}Stored procedure {command} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }


       


        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{command}")]
        public async Task Put(string command, [FromBody]NypLabReqsUsersModel value)
        {
            
            ApiLabReqsUsers.NypLabReqUsersInstance.GetNypLabRecUserCwid(configuration.GetConnectionString(ConstNypLabReqs.LabRecsCloudConnectionString), value.Cwid, value.FirstName,value.LastName,value.EmailAddress, command,value.Active,value.IsAdmin,value.ViewAuditLogs,value.EditLRDocs ).ConfigureAwait(true).GetAwaiter().GetResult();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{cwid}")]
        public void Delete(string cwid)
        {
            ApiLabReqsUsers.NypLabReqUsersInstance.GetNypLabRecUserCwid(configuration.GetConnectionString(ConstNypLabReqs.LabRecsCloudConnectionString), cwid, string.Empty, string.Empty, string.Empty, "delUser",false,false,false,false).ConfigureAwait(true).GetAwaiter().GetResult();
        }
    }
}
