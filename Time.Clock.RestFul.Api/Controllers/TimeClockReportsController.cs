using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.Employee.Time.Clock.RestFul.Api.Constants;
using Edocs.Employees.Time.Clock.App.Service.Models;
using Edocs.Employee.Time.Clock.RestFul.Api.RestApis;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Employee.Time.Clock.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeClockReportsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public TimeClockReportsController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<TimeClockReportsController>
        [HttpGet]
        public async Task<JsonResult> GetAsync()
        {
            return TimeClock.ClockInOutInstance.EmpTimeClockLoginID(configuration.GetConnectionString(EmpConsts.JsonKeyEdocsTimeClockCS), EmpConsts.SpTimeClockEmps).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        // GET api/<TimeClockReportsController>/5
        [HttpGet("{loginID}/{workWeekStartDate}/{workWeekEndDate}/{spName}")]
        public async Task<JsonResult> GetAsync(string loginID,DateTime workWeekStartDate,DateTime workWeekEndDate,string spName)
        {
               return TimeClock.ClockInOutInstance.TimeClockWorkWeekReport(loginID,workWeekStartDate,workWeekEndDate, configuration.GetConnectionString(EmpConsts.JsonKeyEdocsTimeClockCS), spName).ConfigureAwait(false).GetAwaiter().GetResult(); 
        
    }

        // POST api/<TimeClockReportsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TimeClockReportsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TimeClockReportsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
