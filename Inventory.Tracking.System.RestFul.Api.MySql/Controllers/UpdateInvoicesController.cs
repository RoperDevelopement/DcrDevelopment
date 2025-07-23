using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Inventory.Tracking.System.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateInvoicesController : ControllerBase
    {

        private readonly IConfiguration configuration;
        public UpdateInvoicesController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<UpdateInvoicesController>
        [HttpGet]
        public async Task<JsonResult> GetAsync()
        {

            try
            {
                return EdocsITSApi.EdocsITSInstance.GetCustomerUnPaidInvoices(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), EdocsITSConstants.SPGetCustomerOpenInvoiceNumbers).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET api/<UpdateInvoicesController>/5
        [HttpGet("{custId}")]
        public async Task<JsonResult> GetAsync(int custID)
        {
            try
            {
                return EdocsITSApi.EdocsITSInstance.GetInvoices(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), custID).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST api/<UpdateInvoicesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UpdateInvoicesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UpdateInvoicesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
