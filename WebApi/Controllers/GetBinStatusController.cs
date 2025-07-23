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
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
using Edocs.WebApi.ApiClasses;
 

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class GetBinStatusController : ControllerBase
    {
        private readonly IEmailSettings emailSettings;
        private readonly IConfiguration configuration;

        public GetBinStatusController(IConfiguration config, IEmailSettings email)
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
        [HttpGet("{spName}&{categoryName}&{stDate}&{endDate}")]
        public async Task<JsonResult> Get(string spName, string categoryName, DateTime stDate, DateTime endDate)
        {
            try
            { 
            var jResult = await JsonResultAsync(spName, categoryName, stDate, endDate);

            return jResult;
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running GetBinStatusController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"spName {spName} categoryName {categoryName} stDate {stDate.ToString()} endDate {endDate.ToString()} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }

        // POST api/<controller>
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

        public async Task<JsonResult> JsonResultAsync(string spName, string categoryName, DateTime stDate, DateTime endDate)

        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString)))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategorieId, categoryName));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusStartDate, stDate));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusEndDate, endDate));
                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);
                    }

                }
            }
            catch (Exception ex)
            {
                
                 throw new Exception(ex.Message);
            }

        }
    }
}
