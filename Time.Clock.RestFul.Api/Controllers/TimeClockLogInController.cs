using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.Employee.Time.Clock.RestFul.Api.Constants;
using Edocs.Employees.Time.Clock.App.Service.Models;
using Edocs.Employee.Time.Clock.RestFul.Api.RestApis;
using Microsoft.Extensions.Configuration;
using Edocs.Employees.Time.Clock.App.Service.InterFaces;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Employee.Time.Clock.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeClockLogInController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public TimeClockLogInController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<TimeClockLogInController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TimeClockLogInController>/5
        [HttpGet("{empID}/{empPW}")]
        public async Task<JsonResult> GetAsync(string empID,string empPW)
{
            return TimeClock.ClockInOutInstance.TimeClockAdminLogIn(empID,empPW, configuration.GetConnectionString(EmpConsts.JsonKeyEdocsTimeClockCS), EmpConsts.SpTimeClockUserLogIn).ConfigureAwait(false).GetAwaiter().GetResult();

            return null;
        }

        // POST api/<TimeClockLogInController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TimeClockLogInController>/5
        [HttpPut]
        public async Task<JsonResult> PutAsync([FromBody] TimeClockEntriesModel value)
        {
            return null;
        }

        // DELETE api/<TimeClockLogInController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
