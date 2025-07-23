using Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst;
using Edocs.Inventory.Tracking.System.RestFul.Api.Models;
using Edocs.ITS.AppService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Inventory.Tracking.System.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadPSUSDSearchTextController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public UploadPSUSDSearchTextController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<UploadPSUSDSearchTextController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UploadPSUSDSearchTextController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UploadPSUSDSearchTextController>
        [HttpPost]
        public async Task<JsonResult> PostAsync([FromBody] PSUSDUpLoadFullText value)
        {
            try
            {
                return EdocsITSApi.EdocsITSInstance.UploadPSUSDFullText(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), value).ConfigureAwait(false).GetAwaiter().GetResult(); ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT api/<UploadPSUSDSearchTextController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UploadPSUSDSearchTextController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
