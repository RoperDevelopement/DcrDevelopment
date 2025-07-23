using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using BinMonitorAppService.Models;
using BinMonitorAppService.Constants;
using Edocs.WebApi.ApiClasses;
using Edocs.Libaray.AzureCloud.Upload.Batches;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class LabRecsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;

        public LabRecsController(IConfiguration config, IEmailSettings email)
        {
            configuration = config;
            emailSettings = email;
        }


        //  GET: api/<controller>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

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
                string emailSubject = $"Error running LabRecsController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"scandate {scanDate} tablename {tableName} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }


        }

        // POST api/<controller>
        [HttpPost]
        public async Task Post([FromBody] JsonFileLabRecsModel request)
        {
            try
            {
                  await LabRecsGetPost.PostLabRecsApisIntance.AddeNewLabRecs(request, configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionStringLabRecs));
             //   await LabRecsGetPost.PostLabRecsApisIntance.NewLabReqs(request, configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionStringLabRecs));
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running LabRecsController post run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"JsonFileLabRecsModel AzureStpredProcedureName {request.AzureStpredProcedureName} AzureTableName {request.AzureTableName} FileName {request.FileName} FileUrl {request.FileUrl} ScanBatch {request.ScanBatch} Scandate {request.ScanDate.ToString()} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }

        }

        //// PUT api/<controller>/5
        //[HttpPut("{jsonString}")]
        //public async Task Put(string jsonString, [FromBody]string value)
        //{
        //    string r = Response.ContentType;
        //    using (StreamReader sr = new StreamReader(Response.Body))
        //    {
        //        sr.ReadToEnd();
        //    }
        //        string s = jsonString;
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
