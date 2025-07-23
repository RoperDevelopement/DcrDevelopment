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
    public class PSEStudentRecordsController : ControllerBase
    {

        private readonly IConfiguration configuration;
        public PSEStudentRecordsController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<PSEStudentRecordsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PSEStudentRecordsController>/5
        [HttpGet("{studentFirstName}/{studentLastName}/{studentDOB}/{spName}/{scanStDate}/{scanEndDate}")]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<JsonResult> GetAsync(string studentFirstName,string studentLastName,DateTime studentDOB, string spName, DateTime scanStDate,DateTime scanEndDate)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        {
            try
            { 
            return PSEApis.PSEApisInstance.GetStudentRecords(configuration.GetConnectionString(PSEConstants.PSEConnectionStr), studentFirstName, studentLastName, studentDOB, spName, scanStDate, scanEndDate).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }




        }

        // POST api/<PSEStudentRecordsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PSEStudentRecordsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PSEStudentRecordsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
