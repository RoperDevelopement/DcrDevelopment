using Edocs.Inventory.Tracking.System.RestFul.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Models;
using Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Inventory.Tracking.System.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdocsITSInventoryTransferByTrackIDController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public EdocsITSInventoryTransferByTrackIDController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<EdocsITSInventoryTransferByTrackIDController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET: api/<EdocsITSInventoryTransferByTrackIDController>
        [HttpGet("{custID}")]
        public async Task<JsonResult> GetAsync(int custID)
        {
            return EdocsITSApi.EdocsITSInstance.GetTrackingIDSByCustomerID(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS),custID).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        // GET: api/<EdocsITSInventoryTransferByTrackIDController>
        [HttpGet("{custID}/{trackingID}/{repType}/{repSDate}/{repEDate}")]
        public async Task<JsonResult> GetAsync(int custID,string trackingID,string repType,string repSDate,string repEDate)
        {
            return EdocsITSApi.EdocsITSInstance.GetTrackingIDDocNamesByCustomer(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), custID,trackingID,repType,repSDate,repEDate).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        // GET api/<EdocsITSInventoryTransferByTrackIDController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        // GET api/<EdocsITSReportsController>/5
        [HttpGet("{stDate}/{endDate}/{custNum}")]
        public async Task<JsonResult> GetAsync(string stDate, string endDate, string custNum)
        {
            try
            {



                return EdocsITSApi.EdocsITSInstance.GetDocumentNames(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), custNum, stDate, endDate, EdocsITSConstants.SpGetDocumentNames).ConfigureAwait(false).GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        // POST api/<EdocsITSInventoryTransferByTrackIDController>
        [HttpPost]
        public async Task<JsonResult> PostAsync([FromBody] EdocsITSTrackingIDModel value)
        {
            return EdocsITSApi.EdocsITSInstance.EdocsITSUpDateByTrackingID(value, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), EdocsITSConstants.SpUpDateITSTrackingIDByTrackingID).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        // PUT api/<EdocsITSInventoryTransferByTrackIDController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/<EdocsITSInventoryTransferByTrackIDController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
