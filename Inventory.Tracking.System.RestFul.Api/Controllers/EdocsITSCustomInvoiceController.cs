using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Models;
using Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Inventory.Tracking.System.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdocsITSCustomInvoiceController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public EdocsITSCustomInvoiceController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<EdocsITSCustomInvoiceController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EdocsITSCustomInvoiceController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<EdocsITSCustomInvoiceController>
        [HttpPost]
        public async Task<string> Post([FromBody] IList<CustomInvoiceModel> value)
        {
            try
            {
                EdocsITSApi.EdocsITSInstance.AddCustomInvoice(value,configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS)).ConfigureAwait(false).GetAwaiter().GetResult();
                return "No Errors Invoice Added";

            }
            catch (Exception ex)
            {
                return $"Errors: {ex.Message}";
                //throw new Exception(ex.Message);
            }
           

        }

        // PUT api/<EdocsITSCustomInvoiceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EdocsITSCustomInvoiceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
