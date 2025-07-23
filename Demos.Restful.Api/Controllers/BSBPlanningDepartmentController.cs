using Edocs.Demos.Restful.Api.ApisConstants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Demos.Restful.Api.Controllers
{
  //        [ApiController]
   // [Route("[controller]")]
   [ApiController]
    [Route("api/[controller]")] 
  
    public class BSBPlanningDepartmentController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public BSBPlanningDepartmentController(IConfiguration config)
        {
            configuration = config;

        }

        // GET: api/<BSBPlanningDepartmentController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BSBPlanningDepartmentController>/5
        [HttpGet("{sp}")]
        public async Task<JsonResult> GetAsync(string sp)
        {
            try
            {

                return EdocsDemoApis.DemoInstance.GetBSBPWDProjectNameYears(configuration.GetConnectionString(EdocsDemoConstants.EdocsDemoCloudConnectionString),sp).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // GET api/<BSBPlanningDepartmentController>/5
        [HttpGet("{pName}/{sp}")]
        public async Task<JsonResult> GetAsync(string pName,string sp)
        {
            try
            {

                return EdocsDemoApis.DemoInstance.GetBSBPWDPNames(pName,configuration.GetConnectionString(EdocsDemoConstants.EdocsDemoCloudConnectionString), sp).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("{projectDepartment}/{projectYear}/{projectName}/{keyWords}")]
        public async Task<JsonResult> GetAsync(string projectDepartment,string projectYear,string projectName,string keyWords)
        {
            try
            {

                return EdocsDemoApis.DemoInstance.GetBSBPWDRecords(projectDepartment,projectYear,projectName,keyWords,configuration.GetConnectionString(EdocsDemoConstants.EdocsDemoCloudConnectionString)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST api/<BSBPlanningDepartmentController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BSBPlanningDepartmentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BSBPlanningDepartmentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
