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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CategoryDurationsController : ControllerBase
    {
        private readonly IEmailSettings emailSettings;
        private readonly IConfiguration configuration;

        public CategoryDurationsController(IConfiguration config, IEmailSettings email
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

        // GET api/<controller>/5
        [HttpGet("{categoryName}/{spName}")]
        public async Task<JsonResult> Get(string categoryName,string spName)
        {
            try
            {
                AddUpDateSpecData addUpDateSpecData = new AddUpDateSpecData();
                var jResult = await addUpDateSpecData.GetCategoryDuration(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString), categoryName, spName);
                return jResult;
            }
            catch(Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running CategoryDurationsController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Category name {categoryName} Stored procedure {spName} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
            
            
        }

        // POST api/<controller>
        [HttpPost]
        public async Task Post([FromBody]CategoryCheckPointEmailModel value)
        {
            try
            {
                AddUpDateSpecData addUpDateSpecData = new AddUpDateSpecData();
                await addUpDateSpecData.UpdateCategoryDurations(value, configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString));
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running CategoryDurationsController put run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"CategoryCheckPointEmailModel CategoryID {value.CategoryID} Emailto {value.EmailTo} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
            
        }

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
