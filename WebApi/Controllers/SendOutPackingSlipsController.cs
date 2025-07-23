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
using Edocs.Libaray.AzureCloud.Upload.Batches;
using Edocs.WebApi.ApiClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class SendOutPackingSlipsController : ControllerBase
    {

        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;

        public SendOutPackingSlipsController(IConfiguration config, IEmailSettings email)
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
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT api/<controller>/5
        [HttpPut("{storedProcedure}")]
        public async Task<JsonResult> Put(string storedProcedure, [FromBody]SendOutPackingSlipsModel value)
        {
            try
            {
                storedProcedure = ConstNypLabReqs.GetHttpValue(storedProcedure, "=");
                return (await NypMaintenanceLogsApi.NypMLInstance.NypPackingSlips(configuration.GetConnectionString(ConstNypLabReqs.LabRecsCloudConnectionString), storedProcedure, value).ConfigureAwait(true));
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running SendOutPackingSlipsController Put run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Stored procedure {storedProcedure} model SendOutPackingSlipsModel {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }


        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
