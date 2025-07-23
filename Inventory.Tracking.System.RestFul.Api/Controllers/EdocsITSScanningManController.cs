using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Edocs.ITS.AppService.Models;
using Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Inventory.Tracking.System.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdocsITSScanningManController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public EdocsITSScanningManController(IConfiguration config)
        {
            configuration = config;
        }
        // GET: api/<EdocsITSScanningManController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EdocsITSScanningManController>/5
        [HttpGet("{trackingID}/{custName}")]
        public async Task<JsonResult> GetAsync(string trackingID, string custName)
        {
            try
            {
                return EdocsITSApi.EdocsITSInstance.EdocsITSGetTrackingId(trackingID, custName, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), EdocsITSConstants.SpGetITSTrackingID).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST api/<EdocsITSScanningManController>
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] EdocsITSScanningManModel value)
        {
            try
            {
                return EdocsITSApi.EdocsITSInstance.EdocsUpInventoryTrackingID(value, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), EdocsITSConstants.SpUpDateITSTrackingID).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                  JsonResult jsonResult = new JsonResult($"Error: UpDating tracking id {value.TrackingID} for customer {value.EdocsCustomerName} {ex.Message}");
                  throw new Exception(jsonResult.ToString());
            }
        }

        // PUT api/<EdocsITSScanningManController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EdocsITSScanningManController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
