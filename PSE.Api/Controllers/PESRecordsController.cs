using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Edocs.PSE.Api.Models;
using Edocs.PSE.Api.ApisConstants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.PSE.Api.Controllers
{
    
      [Route("api/[controller]")]
    //  [ApiController]
    public class PESRecordsController : ControllerBase
    {

        private readonly IConfiguration configuration;
        public PESRecordsController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<PseUploadRecordsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PseUploadRecordsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PseUploadRecordsController>
        [HttpPost]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task PostAsync([FromBody] PSEJsonUploadModel value)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                PSEApis.PSEApisInstance.UploadPESRecords(value, configuration.GetConnectionString(PSEConstants.PSEConnectionStr)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT api/<PseUploadRecordsController>/5
        [HttpPut]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task PutAsync([FromBody] PSEJsonUploadModel value)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                PSEApis.PSEApisInstance.UploadPESRecords(value, configuration.GetConnectionString(PSEConstants.PSEConnectionStr)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // DELETE api/<PseUploadRecordsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
