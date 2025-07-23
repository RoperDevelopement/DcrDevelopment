using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Models;
using BinMonitorAppService.Constants;
using Edocs.WebApi.BinManagerClasses;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class SpecMonitorUserInfoController : ControllerBase
    {

        private readonly IConfiguration configuration;

        public SpecMonitorUserInfoController(IConfiguration config)
        {
            configuration = config;
        }
     

        // GET api/<controller>/5
        [HttpGet("{cwid}/{passWord}")]
        public async Task<JsonResult> Get(string cwid,string passWord)
        {
            BinMonitorAddUpDate AddUpDate = new BinMonitorAddUpDate();
           var jr = await AddUpDate.LoginSpecMonitorUser(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString),cwid,passWord);
            return jr;
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
