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
using Edocs.WebApi.BinManagerClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class WorkFlowEmailController : ControllerBase
    {


        private readonly IEmailSettings emailSettings;
        private readonly IConfiguration configuration;

        public WorkFlowEmailController(IConfiguration config, IEmailSettings email)
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

        //// GET api/<controller>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
       

        [HttpPost]
        public async Task Post([FromBody] OldNewBatchIdModel value)
        {
            try
            {
                AddUpDateSpecData addNewBatches = new AddUpDateSpecData();
                await addNewBatches.UpdateTransFromWfEmails(value, configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString));
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running WorkFlowEmailController post run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Model OldNewBatchIdModel {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody]WorkFlowEmailModel value)
        {
            try
            {
                AddNewBatches addNewBatches = new AddNewBatches();
                await addNewBatches.UpdateEmailWorkFlow(value, configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString), id);
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running WorkFlowEmailController post run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Model WorkFlowEmailModel for id {id} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
