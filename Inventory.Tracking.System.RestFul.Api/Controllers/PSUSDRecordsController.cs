using Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Inventory.Tracking.System.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PSUSDRecordsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public PSUSDRecordsController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<PSUSDRecordsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PSUSDRecordsController>/5
        [HttpGet("{spName}/{searchFor}/{repType}/{stDate}/{endDate}")]
        public async Task<JsonResult> GetAsync(string spName, string searchFor, string repType, DateTime stDate, DateTime endDate)
        {
            try
            {


                return EdocsITSApi.EdocsITSInstance.PSUSDRecordSearch(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), EdocsITSConstants.SpGetPSUSDRecordSearch, searchFor, repType, stDate, endDate).ConfigureAwait(false).GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // POST api/<PSUSDRecordsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PSUSDRecordsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PSUSDRecordsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
