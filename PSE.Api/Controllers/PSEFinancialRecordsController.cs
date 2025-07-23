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
    public class PSEFinancialRecordsController : ControllerBase
    {

        private readonly IConfiguration configuration;
        public PSEFinancialRecordsController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<PSEFinancialRecordsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PSEFinancialRecordsController>/5
        [HttpGet("{startFinRecYear}/{endFinRecYear}/{finCat}/{spName}/{scanStDate}/{scanEndDate}")]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<JsonResult> GetAsync(DateTime startFinRecYear, DateTime endFinRecYear, string finCat,string spName, string scanStDate, string scanEndDate)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {

            
            return PSEApis.PSEApisInstance.GetFinancialRecords(configuration.GetConnectionString(PSEConstants.PSEConnectionStr), startFinRecYear, endFinRecYear, finCat,spName, scanStDate, scanEndDate).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        // POST api/<PSEFinancialRecordsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PSEFinancialRecordsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PSEFinancialRecordsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
