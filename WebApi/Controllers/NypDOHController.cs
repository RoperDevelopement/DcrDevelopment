using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
    public class NypDOHController : ControllerBase
    {
        private readonly IEmailSettings emailSettings;

        private readonly IConfiguration configuration;

        public NypDOHController(IConfiguration config, IEmailSettings email
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

        // GET api/<controller>/5
        [HttpGet("{storedProcedure}")]
        public async Task<JsonResult> GetAsync(string storedProcedure)
        {
            try
            { 
            storedProcedure = ConstNypLabReqs.GetHttpValue(storedProcedure, "=");
            var jr = await DOHApis.NypDOHInstance.DOHPerformingLabCodes(storedProcedure, configuration.GetConnectionString(ConstNypLabReqs.LabRecsCloudConnectionString)).ConfigureAwait(true);
            return jr;
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running NypDOHController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Stored procedure {storedProcedure} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }

        }

        // POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT api/<controller>/5
        [HttpPut("{storedProcedure}")]
        public async Task<JsonResult> Put(string storedProcedure, [FromBody]DohModel value)
        {
            try 
            { 
            storedProcedure = ConstNypLabReqs.GetHttpValue(storedProcedure, "=");
            return (await DOHApis.NypDOHInstance.DOHRecords(configuration.GetConnectionString(ConstNypLabReqs.LabRecsCloudConnectionString), storedProcedure, value).ConfigureAwait(true));
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running NypDOHController put run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"DohModel ScanBatch {value.ScanBatch} FileUrl {value.FileUrl} scandate {value.ScanDate.ToString()} Stored procedure {storedProcedure} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }


        }

        // DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
