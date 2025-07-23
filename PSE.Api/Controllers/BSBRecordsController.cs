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
    [ApiController]
    public class BSBRecordsController : ControllerBase
    {

        private readonly IConfiguration configuration;
        public BSBRecordsController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<BSBRecordsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BSBRecordsController>/5
        [HttpGet("{archiveID}")]
        public async Task<JsonResult> Get(int archiveID)
        {
            try
            {
            
                 return   PSEApis.PSEApisInstance.GetBSBArchiveRecordByid(archiveID, configuration.GetConnectionString(PSEConstants.PSEConnectionStr)).ConfigureAwait(false).GetAwaiter().GetResult();
                }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        // POST api/<BSBRecordsController>
        [HttpPost]
        public async Task PostAsync([FromBody] BSBUploadJsonModel value)
        {
            try
            {
                PSEApis.PSEApisInstance.UploadBSBRecords(value, configuration.GetConnectionString(PSEConstants.PSEConnectionStr)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT api/<BSBRecordsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BSBRecordsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
