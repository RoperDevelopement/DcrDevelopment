using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.Demos.Restful.Api.Models;
using Edocs.Demos.Restful.Api.ApisConstants;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Demos.Restful.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpLoadBSBProDepRecordsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public UpLoadBSBProDepRecordsController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<UpLoadBSBProDepRecordsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

       

        // POST api/<UpLoadBSBProDepRecordsController>
        [HttpPost]
        public string PostAsync([FromBody] BSPProdDepUloadRecords value)
        {
            try
            {
                            
            return EdocsDemoApis.DemoInstance.UploadBSBProdDepRecords(value, configuration.GetConnectionString(EdocsDemoConstants.EdocsDemoCloudConnectionString), EdocsDemoConstants.SpUploadBSBProdDepJson).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT api/<UpLoadBSBProDepRecordsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UpLoadBSBProDepRecordsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
