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
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditLabReqsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;

        public EditLabReqsController(IConfiguration config, IEmailSettings email)
        {
            configuration = config;
            emailSettings = email;
        }

        // GET: api/<EditLabReqsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EditLabReqsController>/5
        [HttpGet("{labReqId}")]
        public async Task<JsonResult> GetAsync(int labReqId)
        {
            try
            {
                var jr = await LabRecsGetPost.PostLabRecsApisIntance.GetLabReqByID(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionStringLabRecs), labReqId);
                return jr;
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running EditLabReqsController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"LabReqid {labReqId} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }

        }
        [HttpGet("{startSDate}/{endSDate}/{labReqNum}/{csnFinNumber}/{patIDMRN}/{MRN}")]
        public async Task<JsonResult> GetAsync(string startSDate,string endSDate,string labReqNum,string csnFinNumber,string patIDMRN,string MRN)
        {
            try
            {
               var jr = await LabRecsGetPost.PostLabRecsApisIntance.GetChangedLabReq(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionStringLabRecs),startSDate,endSDate, labReqNum,csnFinNumber,patIDMRN,MRN);
                return jr;
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running EditLabReqsController for GetChangedLabReq get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Date Range {startSDate}-{endSDate} labReqNum {labReqNum} csnFinNumber {csnFinNumber} patIDMRN {patIDMRN} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }

        }

        // POST api/<EditLabReqsController>
        [HttpPost]
        public async Task Post([FromBody] LabReqsEditModel value)
        {
            try
            {
                GetNypLabReqs getNypLab = new GetNypLabReqs();
                await getNypLab.UpDateNypLabReqsByID(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionStringLabRecs), ConstNypLabReqs.SpUpDateLabRecInformation,value);
              
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running EditLabReqsController post run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Updateing labreqid {value.LabReqID} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }

        }

        // PUT api/<EditLabReqsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EditLabReqsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
