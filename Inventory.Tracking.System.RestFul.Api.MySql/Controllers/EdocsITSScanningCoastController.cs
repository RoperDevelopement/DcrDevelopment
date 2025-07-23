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
    public class EdocsITSScanningCoastController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public EdocsITSScanningCoastController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<EdocsITSScanningCoastController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EdocsITSScanningCoastController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        //  GET api/<EdocsITSReportsController>/5
        [HttpGet("{stDate}/{endDate}/{custID}")]
        public async Task<JsonResult> GetAsync(string stDate, string endDate, int custID)
        {
            try
            {
                return EdocsITSApi.EdocsITSInstance.ScanningCost(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), stDate, endDate, custID).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST api/<EdocsITSScanningCoastController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<EdocsITSScanningCoastController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EdocsITSScanningCoastController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
