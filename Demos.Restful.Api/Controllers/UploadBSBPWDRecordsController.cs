using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.Demos.Restful.Api.ApisConstants;
using Edocs.Demos.Restful.Api.Models;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Demos.Restful.Api.Controllers
{
  //    [ApiController]
  //   [Route("[controller]")]
   [Route("api/[controller]")]
   [ApiController]
    public class UploadBSBPWDRecordsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public UploadBSBPWDRecordsController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<UploadBSBPWDRecordsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UploadBSBPWDRecordsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UploadBSBPWDRecordsController>
        [HttpPost]
        public void Post([FromBody] BSBPWDRecords value)
        {
            try
            {
                EdocsDemoApis.DemoInstance.UploadBSBPWDRec(value, configuration.GetConnectionString(EdocsDemoConstants.EdocsDemoCloudConnectionString), EdocsDemoConstants.SpBSBPWDUpload).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT api/<UploadBSBPWDRecordsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] BSBPWDRecords value)
        {
           
        }

        // DELETE api/<UploadBSBPWDRecordsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
