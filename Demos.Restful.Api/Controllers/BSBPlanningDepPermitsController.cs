using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.Demos.Restful.Api.ApisConstants;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Demos.Restful.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BSBPlanningDepPermitsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public BSBPlanningDepPermitsController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<BSBPlanningDepPermitsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BSBPlanningDepPermitsController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        // GET api/<UpLoadBSBProDepRecordsController>/5
        //[HttpGet("{permitNum}/{parcelNumber},{exePermitNumber}/{zoneNumber}/{goCode}/{constCompany}/{ownerLot}")]
        //public async Task<JsonResult> GetAsync(int permitNum, int parcelNumber, int exePermitNumber, int zoneNumber, string goCode, string constCompany, string ownerLot)
        //{
        //    try
        //    { 
        //    return EdocsDemoApis.DemoInstance.GetBSBProdDepPermitRecords(permitNum, parcelNumber, exePermitNumber, zoneNumber, goCode, constCompany, ownerLot, configuration.GetConnectionString(EdocsDemoConstants.EdocsDemoCloudConnectionString), EdocsDemoConstants.SpBSBGetByPermit).ConfigureAwait(false).GetAwaiter().GetResult();
        //    }
        //    catch(Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        // GET api/<UpLoadBSBProDepRecordsController>/5
        [HttpGet("{permitNum}/{address}/{addSW}")]
        public async Task<JsonResult> GetAsync(int permitNum,string address,bool addSW)
        {
            try
            {
                return EdocsDemoApis.DemoInstance.GetBSBProdDepPermitRecords(permitNum, address, addSW, 0,0,0,"NA", "NA", "NA", configuration.GetConnectionString(EdocsDemoConstants.EdocsDemoCloudConnectionString), EdocsDemoConstants.SpBSBGetByPermit).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // POST api/<BSBPlanningDepPermitsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BSBPlanningDepPermitsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BSBPlanningDepPermitsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
