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
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AzureBatchesController : ControllerBase
    {

        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;

        public AzureBatchesController(IConfiguration config, IEmailSettings email)
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
        [HttpGet("{scanDate}/{tableName}")]
        public async Task<JsonResult> Get(string scanDate, string tableName)
        {
            try
            {
                var jr = await LabRecsGetPost.PostLabRecsApisIntance.GetLabRecs(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionStringLabRecs), scanDate, tableName);
                return jr;
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running AzureBatchesController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Para scandate {scanDate} table name {tableName} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);
                throw new Exception($"Error Message: {ex.Message}");
            }
        }

        // POST api/<controller>
        //[HttpPost]
        //public async Task Post([FromBody]AzureCloudBatchRecordsModel value)
        //{
             
        //}

        // PUT api/<controller>/5
        [HttpPut("{spName}/{tableName}")]
        public async Task Put(string spName, string tableName, [FromBody]AzureCloudBatchRecordsModel value)
        {
            try
            {
                await LabRecsGetPost.PostLabRecsApisIntance.AddeNewLabRecBatch(value, configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionStringLabRecs), spName);
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running AzureBatchesController Put run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Stored procedure {spName} table name {tableName} scan batch: {value.ScanBatch} FileName {value.FileName} FileUrl {value.FileUrl} ScanDate {value.ScanDate.ToString()} {ex.Message}";
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
