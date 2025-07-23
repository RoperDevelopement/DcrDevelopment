using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Models;
using BinMonitorAppService.Constants;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using Edocs.WebApi.BinManagerClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class BmReportsController : ControllerBase
    {
        private readonly IEmailSettings emailSettings;
        private readonly IConfiguration configuration;

        public BmReportsController(IConfiguration config, IEmailSettings email)
        {
            configuration = config;
            emailSettings = email;
        }
        // GET: api/<controller>
        [HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<controller>/5
        [HttpGet("{binID}/{labReq}/{regCreatedBY}/{processCreatedBY}/{binCLosedBY}/{categoryName}/{labReqRegStDate}/{labReqRegEndDate}")]
        public async Task<JsonResult> Get(string binID, string labReq, string regCreatedBY, string processCreatedBY, string binCLosedBY, string categoryName, DateTime labReqRegStDate, DateTime labReqRegEndDate)
        {
            try
            {
                BinMonitorAddUpDate addUpDate = new BinMonitorAddUpDate();
                var jr = await addUpDate.GetSpecMonitorReprts(configuration.GetConnectionString("BinMonitorCloudConnectionString"), binID, labReq, regCreatedBY, processCreatedBY, binCLosedBY, categoryName, labReqRegStDate, labReqRegEndDate);
                return jr;
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running BmReportsController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"binID:{binID} labReq:{labReq} regCreatedBY: {regCreatedBY} processCreatedBY:{processCreatedBY} binCLosedBY:{binCLosedBY} categoryName:{categoryName} labReqRegStDate:{labReqRegStDate} labReqRegEndDate:{labReqRegEndDate} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }
        

        //// POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
