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
    public class NypEmployeeComplianceController : Controller
    {

        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;
        public NypEmployeeComplianceController(IConfiguration config, IEmailSettings email)
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
        // GET api/<controller>/5
        [HttpGet("{storedProcedure}")]
        public async Task<JsonResult> GetAsync(string storedProcedure)
        {
            try
            {


                storedProcedure = ConstNypLabReqs.GetHttpValue(storedProcedure, "=");
                return (await NypEmployeeComplianceApi.EmployeeCompliamceIntance.EmployeeComplianceCodes(configuration.GetConnectionString(ConstNypLabReqs.LabRecsCloudConnectionString), storedProcedure).ConfigureAwait(true));
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running NypEmployeeComplianceController Get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Stored procedure {storedProcedure} {ex.Message}";
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
        [HttpPut("{storedProcedure}")]
        public async Task<JsonResult> Put(string storedProcedure, [FromBody]EmployeeComplianceLogsModel value)
        {
            try
            {
                storedProcedure = ConstNypLabReqs.GetHttpValue(storedProcedure, "=");
                return (await NypEmployeeComplianceApi.EmployeeCompliamceIntance.GetNypEmployeeComplianaceLogs(configuration.GetConnectionString(ConstNypLabReqs.LabRecsCloudConnectionString), storedProcedure, value).ConfigureAwait(true));
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running NypEmployeeComplianceController Put run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"EmployeeComplianceLogsModel Batch {value.ScanBatch} scandate {value.ScanDate.ToString()} FileUrl {value.FileUrl} Stored procedure {storedProcedure} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }

            //  int indexLab = labResp.IndexOf("=");
            //labResp = labResp.Substring(indexLab + 1).Trim();



        }


        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
