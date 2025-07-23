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
using BinMonitorAppService.Constants;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Runtime.InteropServices.ComTypes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NypLabReqsKeyWordsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;


        public NypLabReqsKeyWordsController(IConfiguration config, IEmailSettings email)
        {
            configuration = config;
            emailSettings = email;
        }
        // GET: api/<NypLabReqsKeyWordsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<NypLabReqsKeyWordsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
{
return "value";
}
        [HttpGet("{startdate}/{enddate}/{searchBy}/{keywords}")]
        public async Task<JsonResult> GetAsync(DateTime startdate,DateTime enddate ,string searchBy,string keywords)
        {
            try
            {
                GetNypLabReqs getNypLabReqs = new GetNypLabReqs();
                var jr = await getNypLabReqs.GetLabKeyWords(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionStringLabRecs),startdate, enddate, searchBy,keywords);
                return jr;
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running NypLabReqsKeyWordsController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Search date {startdate}-{enddate} for search by {searchBy} keywords {keywords} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }

        }
        // POST api/<NypLabReqsKeyWordsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<NypLabReqsKeyWordsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<NypLabReqsKeyWordsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
