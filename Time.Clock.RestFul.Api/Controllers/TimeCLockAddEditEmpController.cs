using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.Employee.Time.Clock.RestFul.Api.Constants;
using Edocs.Employees.Time.Clock.App.Service.Models;
using Edocs.Employee.Time.Clock.RestFul.Api.RestApis;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Employee.Time.Clock.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeCLockAddEditEmpController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public TimeCLockAddEditEmpController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<TimeCLockAddEditEmpController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TimeCLockAddEditEmpController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TimeCLockAddEditEmpController>
        [HttpPost]
        public async Task<JsonResult> PostAsync([FromBody] EmpModel value)
        {
            return AddEditTimeClockUsers.TimeClockUsersInstance.AddTimeClockUser(value, configuration.GetConnectionString(EmpConsts.JsonKeyEdocsTimeClockCS), EmpConsts.SpAddUpdateTimeClockUsers).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        // PUT api/<TimeCLockAddEditEmpController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

      //  DELETE api/<TimeCLockAddEditEmpController>/5
        [HttpDelete("{id}")]
        public async Task<JsonResult> DeleteAsync(int id)
{
                 return AddEditTimeClockUsers.TimeClockUsersInstance.DelTimeClockEntry(id, configuration.GetConnectionString(EmpConsts.JsonKeyEdocsTimeClockCS), EmpConsts.SpDelTimeClockEntry).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
