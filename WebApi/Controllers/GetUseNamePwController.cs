using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Edocs.WebApi.BinManagerClasses;
using BinMonitorAppService.Constants;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class GetUseNamePwController :ControllerBase
    {
        private readonly IEmailSettings emailSettings;

        private readonly IConfiguration configuration;

        public GetUseNamePwController(IConfiguration config, IEmailSettings email)
        {
            configuration = config;
            emailSettings = email;
        }
        //// GET: api/<controller>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<controller>/5
        [HttpGet("{cwid}")]
        public async Task<JsonResult> Get(string cwid)
        {
            try
            { 
            AddUpDateSpecData addUpDateSpecData = new AddUpDateSpecData();
            return addUpDateSpecData.GetUserInforByCwid(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionStringLabRecs), cwid).Result;
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running GetUseNamePwController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"Cwid {cwid} {ex.Message}";
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
 
