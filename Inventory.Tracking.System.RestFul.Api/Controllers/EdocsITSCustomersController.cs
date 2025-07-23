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
    public class EdocsITSCustomersController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public EdocsITSCustomersController(IConfiguration config)
        {
            configuration = config;
            
        }
        // GET: api/<EdocsITSCustomersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EdocsITSCustomersController>/5
        [HttpGet("{spName}")]
        public async Task<JsonResult> GetAsync(string spName)
        {
            try
            {
              return  EdocsITSApi.EdocsITSInstance.EdocsITSGetCustomer(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), spName,-1).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
      
        // GET api/<EdocsITSCustomersController>/5
        [HttpGet("{spName}/{custID}")]
        public async Task<JsonResult> GetAsync(string spName,int custID)
        {
            try
            {
                return EdocsITSApi.EdocsITSInstance.EdocsITSGetCustomer(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), spName,custID).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        // POST api/<EdocsITSCustomersController>
        [HttpPost]
        public async Task Post([FromBody] EdocsITSCustomersModel value)
        {
            try
            {
                EdocsITSApi.EdocsITSInstance.EdocsNewCustomer(value, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS)).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT api/<EdocsITSCustomersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EdocsITSCustomersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
