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
using Edocs.Libaray.AzureCloud.Upload.Batches;
using Edocs.WebApi.ApiClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabReqsPDFFullTextController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;

        public LabReqsPDFFullTextController(IConfiguration config, IEmailSettings email)
        {
            configuration = config;
            emailSettings = email;

        }
        // GET: api/<LabReqsPDFFullTextController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LabReqsPDFFullTextController>/5
        [HttpGet("{scanStartDate}/{scanEndDate}")]
        public async Task<JsonResult> GetAsync(string scanStartDate, string scanEndDate)
        {
            try
            {

                DateTime st = DateTime.Parse(scanStartDate);
                DateTime et = DateTime.Parse(scanEndDate);
                return LabRecsGetPost.PostLabRecsApisIntance.GetLabRecsPDFToOcr(configuration.GetConnectionString(ConstNypLabReqs.LabRecsCloudConnectionString), st, et).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)

            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running LabReqsPDFFullTextController Get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"For scanStartdate {scanStartDate} scanEndDate {scanEndDate} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }

        // POST api/<LabReqsPDFFullTextController>
        [HttpPost]
        public async Task PostAsync([FromBody] PDFFullTextModel value)
        {
            try
            {


                LabRecsGetPost.PostLabRecsApisIntance.AddNYPPDFFullText(value, configuration.GetConnectionString(ConstNypLabReqs.LabRecsCloudConnectionString)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)

            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running LabReqsPDFFullTextController PostAsync run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"For labreq id {value.ID} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }

        }

        // PUT api/<LabReqsPDFFullTextController>/5
        [HttpPut("{labRecid}/{reqNum}")]
        public void Put(int labRecid, string reqNum, [FromForm] string value)
        {
            try
            {


                LabRecsGetPost.PostLabRecsApisIntance.UpDateNypLabReqNum(configuration.GetConnectionString(ConstNypLabReqs.LabRecsCloudConnectionString), labRecid, reqNum).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)

            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running LabReqsPDFFullTextController Put run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"For labreq id {labRecid} req number {reqNum} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }

        }

        // DELETE api/<LabReqsPDFFullTextController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
