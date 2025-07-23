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
    public class AddInvoiceController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public AddInvoiceController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<AddInvoiceController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AddInvoiceController>/5
        [HttpGet("{custID}/{invStDate}/invEndDate")]
        public string GetAsync(int custID, DateTime invStDate, DateTime invEndDate)
        {
            return "value";
        }

        // POST api/<AddInvoiceController>
        [HttpPost]
        public async Task<JsonResult> PostAsync([FromBody] InvoiceModel  value)
{
            try 
            { 
            return EdocsITSApi.EdocsITSInstance.EdocsInventoryTrackingGenerateInvoice(value, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), EdocsITSConstants.SpAddAddInvoice).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

      

        // PUT api/<AddInvoiceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AddInvoiceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
