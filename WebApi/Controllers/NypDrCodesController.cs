using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Edocs.WebApi.ApiClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class NypDrCodesController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;
        public NypDrCodesController(IConfiguration config, IEmailSettings email
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
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        [HttpPost]
        public async Task<JsonResult> PostAsync([FromBody]DrCodeModel value)
        {
            try
            { 
            var jr = await NypDrCodes.NypDrCodesInstance.GetNypDrCodes(configuration.GetConnectionString(ConstNypLabReqs.LabRecsCloudConnectionString),ConstNypLabReqs.SpNypDrCodes, value).ConfigureAwait(true);
            return jr;
            }
            catch (Exception ex)
            { 
            EmailService emailService = new EmailService(emailSettings);
            string emailSubject = $"Error running NypDrCodesController Put run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
            string emailMessage = $"Model DrCodeModel scanbatch {value.ScanBatch} fileurl {value.FileUrl} ScanDate {value.ScanDate.ToString()} {ex.Message}";
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
