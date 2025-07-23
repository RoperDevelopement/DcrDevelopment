using Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Edocs.ITS.AppService.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Inventory.Tracking.System.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PSUSDSearchTextController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public PSUSDSearchTextController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<PSUSDSearchTextController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PSUSDSearchTextController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PSUSDSearchTextController>
        [HttpPost]
 
        public async Task<JsonResult> PostAsync([FromBody] PSUSDFullTextModel value)
        {
           return EdocsITSApi.EdocsITSInstance.GetpsusdRecordsbyKeyWord(value.SearchText, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS)).ConfigureAwait(false).GetAwaiter().GetResult();

        }

        // PUT api/<PSUSDSearchTextController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PSUSDSearchTextController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
