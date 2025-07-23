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

  
    public class CreateBatchesController : ControllerBase
    {
        private readonly IEmailSettings emailSettings;
        private readonly IConfiguration configuration;

        public CreateBatchesController(IConfiguration config, IEmailSettings email)
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
        //[HttpGet("{createBatchModel}")]
        //public string Get(BinCreateBatchModel binCreateBatchModel)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        [HttpPost()]
        public async Task Post([FromBody] BinRegistorModel RegistBatch)
        {
            try
            {
                AddNewBatches addNewBatches = new AddNewBatches();
                await addNewBatches.RegisterBinSqlServer(RegistBatch, configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString));
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running CreateBatchesController post run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"BatchID: {RegistBatch.BatchID} BinID: {RegistBatch.BinID} LabRecNumber: {RegistBatch.LabRecNumber} CategoryName {RegistBatch.CategoryName} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }

        }
          
         
        

        //// PUT api/<controller>/5
        //[HttpPut("{BinCreateBatchModel}")]
        //public void Put(BinCreateBatchModel binCreateBatchModel)
        //{
        //    binCreateBatchModel.BatchID = new Guid();
        //}


        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

    }
}
