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
    public class BinMonitorReportsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;
        public BinMonitorReportsController(IConfiguration config, IEmailSettings email)
        {
            emailSettings = email;
            configuration = config;
        }
       
        [HttpGet("{spName}&{categoryName}&{stDate}&{endDate}")]
        public void Get(string spName, string categoryName, DateTime stDate, DateTime endDate)
        {
            //   BinMonitorAddUpDate addUpDate = new BinMonitorAddUpDate();
            // var jr = await addUpDate.GetSpecMonitorReprts(configuration.GetConnectionString("BinMonitorCloudConnectionString"), binID, labReq, regCreatedBY, processCreatedBY, binCLosedBY, categoryName, labReqRegStDate, labReqRegEndDate);
            //var jResult = await JsonResultAsync(spName, categoryName, stDate, endDate);
            Console.WriteLine();
            //return jResult;
            
        }

    // POST api/<controller>
    [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
