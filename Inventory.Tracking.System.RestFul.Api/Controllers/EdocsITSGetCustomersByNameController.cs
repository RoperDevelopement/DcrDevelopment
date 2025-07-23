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
    public class EdocsITSGetCustomersByNameController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public EdocsITSGetCustomersByNameController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<EdocsITSGetCustomersByNameController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EdocsITSGetCustomersByNameController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpGet("{custName}/{spName}")]
        public async Task<JsonResult> GetAsync(string custName, string spName)
        {
            try
            {
                return EdocsITSApi.EdocsITSInstance.EdocsITSGetCustomerByCustName(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), spName, custName).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        // POST api/<EdocsITSGetCustomersByNameController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<EdocsITSGetCustomersByNameController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EdocsITSGetCustomersByNameController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
