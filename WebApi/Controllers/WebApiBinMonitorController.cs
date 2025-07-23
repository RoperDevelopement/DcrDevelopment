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
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebApiBinMonitorController : ControllerBase
    {
        private readonly IConfiguration configuration;

        private readonly IEmailSettings emailSettings;
        public WebApiBinMonitorController(IConfiguration config, IEmailSettings email)
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
        [HttpGet("{spName}")]
        public async Task<JsonResult> Get(string spName)
        {
            try
            {
                var jResult = await JsonResultAsync(spName);

                return jResult;
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running WebApiBinMonitorController  get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Stored procedure {spName} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }
        [HttpGet("{spName}/{batchId}")]
        public async Task<JsonResult> Get(string spName, string batchId)
        {
            try
            {
                var jResult = await JsonResultAsync(spName, batchId);

                return jResult;
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running WebApiBinMonitorController  get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"spName {spName} batchId {batchId} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }

       







        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{labReqID}/{deletedBy}")]
        public async Task<JsonResult> DeleteAsync(string labReqID, string deletedBy)
        {
            try
            {
                return await DeleteLabReq(SqlConstants.SpDeleteSpecLabRec, labReqID, deletedBy);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<JsonResult> JsonResultAsync(string spName)

        {
            try
            {


                return await JsonBasicApis.JsonInstance.GetJsonResults(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString), spName).ConfigureAwait(true);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<JsonResult> DeleteLabReq(string spName, string labReq, string deletedBy)

        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString)))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;


                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaLabReqID, labReq));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaDeletedBy, deletedBy));
                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<JsonResult> JsonResultAsync(string spName, string batchID)

        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString)))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        if (Guid.TryParse(batchID, out Guid value))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBatchId, batchID));
                        }
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinId, batchID));
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
