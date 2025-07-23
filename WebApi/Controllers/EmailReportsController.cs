using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Models;
using Edocs.WebApi.BinManagerClasses;
using BinMonitorAppService.Constants;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class EmailReportsController : ControllerBase
    {

        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;

        public EmailReportsController(IConfiguration config, IEmailSettings email
)
        {
            configuration = config;
            emailSettings = email;
        }
        //// GET: api/<controller>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<controller>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        [HttpPost]
        public async Task Put([FromBody]EmailReportModel emailReportModel)
        {
            try
            { 
            AddNewBatches addNewBatches = new AddNewBatches();
            await addNewBatches.UpdateEmailInfo(emailReportModel, configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString));
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running EmailReportsController Put run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"EmailReportModel EmailTo {emailReportModel.EmailTo} EmailFrequency {emailReportModel.EmailFrequency} EmailCC {emailReportModel.EmailCC}  {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }

        }

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

       
    }
}
