using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Edocs.WebApi.BinManagerClasses;
using BinMonitorAppService.Models;
using BinMonitorAppService.Constants;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UpdateBinsByBInIDController : ControllerBase
    {

        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;

        public UpdateBinsByBInIDController(IConfiguration config, IEmailSettings email
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

        //// GET api/<controller>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        [HttpPost]
        public async Task Post([FromBody]BinLabRecModel value)
        {
            try
            {
                AddNewBatches newBatches = new AddNewBatches();
                newBatches.UpdateBinRegStatusByBinId(value, configuration.GetConnectionString("BinMonitorCloudConnectionString")).Wait();
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running UpdateBinsByBInIDController post run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Model BinLabRecModel BinId {value.BinID} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }

        }

        //// PUT api/<controller>/5
        //[HttpPut("{binID}/{userName}/{regStatus}")]
        //public async Task Put(string binID,string userName,int regStatus)
        //{
        //    AddNewBatches newBatches = new AddNewBatches();
        //   // newBatches.UpdateBinRegStatusByBinId(binID, userName, configuration.GetConnectionString("BinMonitorCloudConnectionString"), regStatus).Wait();


        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
