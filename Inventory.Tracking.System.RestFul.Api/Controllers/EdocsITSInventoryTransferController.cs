using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Edocs.ITS.AppService.Models;
using Newtonsoft.Json;
using Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Inventory.Tracking.System.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class EdocsITSInventoryTransferController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public EdocsITSInventoryTransferController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<EdocsITSInventoryTransferController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EdocsITSInventoryTransferController>/5
        [HttpGet("{custID}")]
        public async Task<JsonResult> GetAsync(int custID)
        {
            return EdocsITSApi.GetInvNumDateSent(custID, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), EdocsITSConstants.SPGetInvoiceNumbers).ConfigureAwait(false).GetAwaiter().GetResult();

        }
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


        // POST api/<EdocsITSInventoryTransferController>
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] EdocsITSInventoryTransfer value)
        {
            try
            { 
           return EdocsITSApi.EdocsITSInstance.EdocsAddInventoryTrackingID(value, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), EdocsITSConstants.SpAddInventoryTransfer).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                //    JsonResult jsonResult = new JsonResult($"Error: Adding tracking id {value.TrackingID} for customer {value.EdocsCustomerName} {ex.Message}");
                //  throw new Exception(jsonResult.ToString());
                throw new Exception(ex.Message);

            }
        }

        // PUT api/<EdocsITSInventoryTransferController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EdocsITSInventoryTransferController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
