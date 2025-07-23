using Edocs.Demos.Restful.Api.ApisConstants;
using Edocs.Demos.Restful.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Demos.Restful.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BSBPlanDepSearchTextController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public BSBPlanDepSearchTextController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<BSBPlanDepSearchTextController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BSBPlanDepSearchTextController>/5
        [HttpGet("{searchTxt}")]
        public async Task<JsonResult> GetAsync(string searchTxt)
        {
            try
            {
                
                return EdocsDemoApis.DemoInstance.GetBSBProdDepPermitRecordsbyKeyWord(searchTxt,configuration.GetConnectionString(EdocsDemoConstants.EdocsDemoCloudConnectionString)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST api/<BSBPlanDepSearchTextController>
        [HttpPost]
        public async Task PostAsync([FromBody] BSPProdDeptUploadSearchTxt value)
        {
            try
            {
                EdocsDemoApis.DemoInstance.UploadBSBProdDepSearchText(value, configuration.GetConnectionString(EdocsDemoConstants.EdocsDemoCloudConnectionString),EdocsDemoConstants.SUpateBSBProdDepFullTextSearch).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT api/<BSBPlanDepSearchTextController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BSBPlanDepSearchTextController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
