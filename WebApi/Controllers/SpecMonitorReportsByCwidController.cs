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
    public class SpecMonitorReportsByCwidController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;


        public SpecMonitorReportsByCwidController(IConfiguration config, IEmailSettings email)
        {
            emailSettings = email;
            configuration = config;
        }
        // GET: api/<SoecMonitorReportsByCwidController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<SoecMonitorReportsByCwidController>/5
        [HttpGet]
        public async Task<JsonResult> GetAsync()
        {
            try
            {
               
                var jr = await JsonBasicApis.JsonInstance.GetJsonResults(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString), SqlConstants.SpGetActiveBinUsers);
                return jr;
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running SpecMonitorReportsByCwidController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Getting active bin users {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }
        [HttpGet("{cwid}/{binStatus}/{repStDate}/{repEDate}/{spName}")]
        public async Task<JsonResult> GetAsync(string cwid,string binStatus,string repStDate,string repEDate,string spName)
        {
            try
            {
                BinMonitorAddUpDate binMonitorAddUpDate = new BinMonitorAddUpDate();
                if (string.Compare(spName, SqlConstants.SpUsageReportByCWID, true) == 0)
                    return binMonitorAddUpDate.GetUsageReport(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString), cwid, binStatus, repStDate, repEDate).ConfigureAwait(false).GetAwaiter().GetResult();

                throw new Exception($"SP name {spName} not found");
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running SpecMonitorReportsByCwidController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Getting binuser {cwid} for rep type {binStatus} for date range {repStDate}-{repEDate} for sp {spName} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }
        [HttpGet("{cwid}/{binstatus}/{spName}/{sDate}/{eDate}/{categoryName}")]
        public async Task<JsonResult> Get(string cwid, string binStatus, string spName, DateTime sDate, DateTime eDate, string categoryName)
        {
            try
            {
                BinMonitorAddUpDate addUpDate = new BinMonitorAddUpDate();
                return addUpDate.GetSpecMonitorReprtsByCwid(configuration.GetConnectionString("BinMonitorCloudConnectionString"), cwid, binStatus, spName, sDate, eDate, categoryName).ConfigureAwait(false).GetAwaiter().GetResult();


                
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running BmReportsController  get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"spName {spName} cwid {cwid} binstatus {binStatus} catname {categoryName} date range {sDate}-{eDate} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }
        [HttpGet("{repStDate}/{repEDate}/{spName}/{binStatus}")]
        public async Task<JsonResult> GetAsync(string repStDate, string repEDate,string spName,string binStatus)
        {
            try
            {
                BinMonitorAddUpDate binMonitorAddUpDate = new BinMonitorAddUpDate();
                if(string.Compare(spName, SqlConstants.SpGetDeletedLabReqs,true)==0)
                { 
                return binMonitorAddUpDate.GetDelLabReqs(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString), repStDate, repEDate).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                else
                {
                    if (string.Compare(spName, SqlConstants.SpUsageChartReportByCWID, true) == 0)
                    {
                        return binMonitorAddUpDate.GetChartReport(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString), repStDate, repEDate,binStatus).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                }

                throw new Exception($"Stored procedure {spName} not found");


            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running SpecMonitorReportsByCwidController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Getting deleted lab reqs or chart report for  date range {repStDate}-{repEDate} binstaus {binStatus} spname {spName} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }
        // POST api/<SoecMonitorReportsByCwidController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SoecMonitorReportsByCwidController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SoecMonitorReportsByCwidController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
