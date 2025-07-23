using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.Employee.Time.Clock.RestFul.Api.Constants;
using Edocs.Employees.Time.Clock.App.Service.Models;
using Edocs.Employee.Time.Clock.RestFul.Api.RestApis;
using Microsoft.Extensions.Configuration;

namespace Edocs.Employee.Time.Clock.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeClockInOutController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public TimeClockInOutController(IConfiguration config)
        {
            configuration = config;

        }
        // GET api/<AddEditTimeCLockUsersController>/5
        [HttpGet("{empID}/{isAdmin}")]
        public async Task<JsonResult> GetAsync(string empID,bool isAdmin)
        {
            return  TimeClock.ClockInOutInstance.ClockInOut(empID, configuration.GetConnectionString(EmpConsts.JsonKeyEdocsTimeClockCS),EmpConsts.SpClockInOutTimeClock,isAdmin).ConfigureAwait(false).GetAwaiter().GetResult();
            /*return AddEditTimeClockUsers.TimeClockUsersInstance.EdocsAddInventoryTrackingID(value, configuration.GetConnectionString(EmpConsts.JsonKeyEdocsTimeClockCS), EmpConsts.SpAddUpdateTimeClockUsers).ConfigureAwait(false).GetAwaiter().GetResult()*/;
        }
        [HttpPost]
        public async Task<JsonResult> PostAsync([FromBody] TimeClockEntriesModel value)
        {
            return TimeClock.ClockInOutInstance.TimeClockAddTimeEntry(value, configuration.GetConnectionString(EmpConsts.JsonKeyEdocsTimeClockCS), EmpConsts.SpAddEmpTimeClockEntry).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
