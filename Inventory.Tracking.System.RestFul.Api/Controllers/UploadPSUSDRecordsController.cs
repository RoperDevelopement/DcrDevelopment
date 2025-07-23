using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Edocs.ITS.AppService.Models;
using Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Inventory.Tracking.System.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadPSUSDRecordsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public UploadPSUSDRecordsController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<UploadPSUSDRecordsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UploadPSUSDRecordsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UploadPSUSDRecordsController>
        [HttpPost]
        public async Task<JsonResult> PostAsync([FromBody] PSUSDUploadRecordsModel value)
        {
            try
            {
                return  EdocsITSApi.EdocsITSInstance.UploadPSUSDRecords(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), value).ConfigureAwait(false).GetAwaiter().GetResult(); ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT api/<UploadPSUSDRecordsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UploadPSUSDRecordsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
