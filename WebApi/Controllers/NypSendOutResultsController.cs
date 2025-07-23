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
    public class NypSendOutResultsController : Controller
    {

        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;
        public NypSendOutResultsController(IConfiguration config, IEmailSettings email
)
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
     //   [HttpPost]
       // public async Task<JsonResult> PostAsync([FromBody]DrCodeModel value)
       // {
         //   var jr = await NypDrCodes.NypDrCodesInstance.GetNypDrCodes(configuration.GetConnectionString(ConstNypLabReqs.LabRecsCloudConnectionString), ConstNypLabReqs.SpNypDrCodes, value).ConfigureAwait(true);
           // return jr;
       // }

        // PUT api/<controller>/5
        [HttpPut("{storedProcedure}")]
        public async Task<JsonResult> Put(string storedProcedure, [FromBody]SendOutResultsModel value)
        {
            try
            {
                storedProcedure = ConstNypLabReqs.GetHttpValue(storedProcedure, "=");

                var jr = await NypMaintenanceLogsApi.NypMLInstance.NypSendOutResults(configuration.GetConnectionString(ConstNypLabReqs.LabRecsCloudConnectionString), storedProcedure, value).ConfigureAwait(true);
                return jr;
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running NypSendOutResultsController Put run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Stored procedure {storedProcedure} model SendOutResultsModel {ex.Message}";
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
