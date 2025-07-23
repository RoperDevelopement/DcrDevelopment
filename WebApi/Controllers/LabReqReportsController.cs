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
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
using Edocs.WebApi.ApiClasses;

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
   
    public class LabReqReportsController :ControllerBase
    {
        private readonly IEmailSettings emailSettings;
        private readonly IConfiguration configuration;
        public LabReqReportsController(IConfiguration config, IEmailSettings email)
        {
            configuration = config;
            emailSettings = email;

        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet("{batchId}/{jsonFile}/{reportSrch}/{totalJsonFile}/{dateUploaded}")]
        public async Task<JsonResult> Get(string batchId, string jsonFile, string reportSrch, int totalJsonFile, string dateUpLoaded)
        {
            try
            {
                GetNypLabReqs nypLabReqs = new GetNypLabReqs();
                var jResult = nypLabReqs.GetLabRecsUpLoaded(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionStringLabRecs), batchId, jsonFile, reportSrch, totalJsonFile, dateUpLoaded).ConfigureAwait(false).GetAwaiter().GetResult();

                return jResult;
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running LabReqReportsController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"batchId {batchId} jsonFile {jsonFile} reportSrch {reportSrch} totalJsonFile {totalJsonFile.ToString()} dateUpLoaded {dateUpLoaded.ToString()} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }
    }
}
