using Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst;
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
    public class PSUSDFLNameController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public PSUSDFLNameController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<PSUSDFLNameController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PSUSDFLNameController>/5
        [HttpGet("{sdate}/{endDate}/{fName}/{lname}")]
          public async Task<JsonResult> GetAsync(DateTime sDate,DateTime endDate,string fName,string lName)
        {
            return EdocsITSApi.EdocsITSInstance.PSUSDRecordFLName(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS),fName,lName,sDate,endDate).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        [HttpGet("{sdate}/{endDate}/{fName}/{lname}/{dept}/{orgDept}")]
        public async Task<JsonResult> GetAsync(DateTime sDate, DateTime endDate, string fName, string lName,string dept,string orgDept)
        {
            return EdocsITSApi.EdocsITSInstance.PSUSDRecordDept(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), fName, lName,dept,orgDept,sDate,endDate).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        //PSUSDRecordDept
        // POST api/<PSUSDFLNameController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PSUSDFLNameController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PSUSDFLNameController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
