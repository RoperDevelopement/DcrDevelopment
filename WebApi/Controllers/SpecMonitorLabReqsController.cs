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
using Edocs.WebApi.ApiClasses;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecMonitorLabReqsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;
        public SpecMonitorLabReqsController(IConfiguration config, IEmailSettings email)
        {
            emailSettings = email;
            configuration = config;
        }
        // GET: api/<SpecMonitorLabReqsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SpecMonitorLabReqsController>/5
        [HttpGet("{labReqNum}/{repStDate}/{repEDate}/{spName}")]
        public async Task<JsonResult> GetAsync(string labReqNum,DateTime repStDate,DateTime repEDate, string spName)
        {
            try
            {
               return SpecimenRejectionGetPost.SpecimenRejectionInstance.NypSpecimenLookupLabReq(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString),spName, labReqNum, repStDate, repEDate).GetAwaiter().GetResult();
                //BinMonitorAddUpDate binMonitorAddUpDate = new BinMonitorAddUpDate();
                //if (string.Compare(spName, SqlConstants.SpUsageReportByCWID, true) == 0)
                //    return binMonitorAddUpDate.GetUsageReport(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString), cwid, binStatus, repStDate, repEDate).ConfigureAwait(false).GetAwaiter().GetResult();

                //throw new Exception($"SP name {spName} not found");
            
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                 string emailSubject = $"Error running SpecMonitorLabReqsController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                 string emailMessage = $"Getting LABREQ Number {labReqNum}  for   for date range {repStDate}-{repEDate} for sp {spName} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }

        // GET api/<SpecMonitorLabReqsController>/5
        [HttpGet("{repStDate}/{repEDate}/{transferType}")]
        public async Task<JsonResult> GetAsync(DateTime repStDate, DateTime repEDate, string transferType)
        {
            try
            {
                return SpecimenRejectionGetPost.SpecimenRejectionInstance.NypSpecimenTransferReport(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString),SqlConstants.SpTransFerReport, repStDate, repEDate,transferType).GetAwaiter().GetResult();
                //BinMonitorAddUpDate binMonitorAddUpDate = new BinMonitorAddUpDate();
                //if (string.Compare(spName, SqlConstants.SpUsageReportByCWID, true) == 0)
                //    return binMonitorAddUpDate.GetUsageReport(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString), cwid, binStatus, repStDate, repEDate).ConfigureAwait(false).GetAwaiter().GetResult();

                //throw new Exception($"SP name {spName} not found");

            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running SpecMonitorLabReqsController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Getting TransFer Report for date range {repStDate}-{repEDate} for trans type {transferType} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }

        // POST api/<SpecMonitorLabReqsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SpecMonitorLabReqsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SpecMonitorLabReqsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
