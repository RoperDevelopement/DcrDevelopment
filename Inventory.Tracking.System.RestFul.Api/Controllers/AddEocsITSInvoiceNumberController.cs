using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Edocs.ITS.AppService.Models;
using Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst;
using Edocs.Inventory.Tracking.System.RestFul.Api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Inventory.Tracking.System.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddEocsITSInvoiceNumberController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public AddEocsITSInvoiceNumberController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<AddEocsITSInvoiceNumberController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AddEocsITSInvoiceNumberController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AddEocsITSInvoiceNumberController>
        [HttpPost]
        public async Task<JsonResult> PostAsync([FromBody] AddInvoiceNumberModel value)
        {

            return EdocsITSApi.EdocsITSInstance.AddInvoiceNumber(value, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS)).ConfigureAwait(false).GetAwaiter().GetResult();
        }


        // PUT api/<AddEocsITSInvoiceNumberController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AddEocsITSInvoiceNumberController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
