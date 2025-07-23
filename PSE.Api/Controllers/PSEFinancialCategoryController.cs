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
    public class PSEFinancialCategoryController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public PSEFinancialCategoryController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<PSEFinancialCategoryController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PSEFinancialCategoryController>/5
        [HttpGet("{spName}")]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<JsonResult> GetAsync(string spName)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
               return PSEApis.PSEApisInstance.GetPSEFinancialCategories(configuration.GetConnectionString(PSEConstants.PSEConnectionStr),spName).ConfigureAwait(false).GetAwaiter().GetResult();
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        // POST api/<PSEFinancialCategoryController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PSEFinancialCategoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PSEFinancialCategoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
