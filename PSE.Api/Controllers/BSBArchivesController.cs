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
    public class BSBArchivesController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public BSBArchivesController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<BSBArchivesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BSBArchivesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BSBArchivesController>
        [HttpPost]
        public async Task<JsonResult> PostAsync(BSBLoockUpArchivesModel  value)
        {
            try
            {
               return PSEApis.PSEApisInstance.BSBArchiveRecords(value, configuration.GetConnectionString(PSEConstants.PSEConnectionStr)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
                catch(Exception ex)
            {
                 throw new Exception(ex.Message);
            }
            
            
            
        }

        // PUT api/<BSBArchivesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BSBArchivesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
