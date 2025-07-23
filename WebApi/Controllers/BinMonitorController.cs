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
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class BinMonitorController : ControllerBase
    {
        private readonly IEmailSettings emailSettings;
        private readonly IConfiguration configuration;

        public BinMonitorController(IConfiguration config, IEmailSettings email)
        {
            emailSettings = email;
            configuration = config;
        }


        // GET: api/<controller>
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
                string emailSubject = $"Error running inMonitorController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Stored procedure {spName} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }



        public async Task<JsonResult> JsonResultAsync(string spName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(configuration.GetConnectionString("BinMonitorCloudConnectionString")))
            {
                using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    await sqlConnection.OpenAsync();
                    SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();
                    var dt = new DataTable();
                    dt.BeginLoadData();
                    dt.Load(dr);
                    dt.EndLoadData();
                    dt.AcceptChanges();
                    JsonResult jsonResult = new JsonResult(dt);
                    return jsonResult;
                }

            }
        }

        public async Task<JsonResult> JsonResultAsync<T>(T objectName, string spName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(configuration.GetConnectionString("BinMonitorCloudConnectionString")))
            {
                using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    await sqlConnection.OpenAsync();
                    SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();
                    var dt = new DataTable();
                    dt.BeginLoadData();
                    dt.Load(dr);
                    dt.EndLoadData();
                    dt.AcceptChanges();
                    JsonResult jsonResult = new JsonResult(dt);
                    return jsonResult;
                }

            }

        }
    }
}
