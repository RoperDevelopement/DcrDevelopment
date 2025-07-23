using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Edocs.ITS.AppService.Models;
using Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst;
using System.Xml.Linq;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Inventory.Tracking.System.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MDTOCRController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public MDTOCRController(IConfiguration config)
        {
            configuration = config;

        }
       
        // GET api/<MDTOCRController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpGet("{id}/{docsOcr}")]
        public string GetAsync(int id,int docsOcr)
        {
            string retStr = $"Updated {id} with total ocr {docsOcr}";
            try
            {
                EdocsITSApi.EdocsITSInstance.MDTUpDateOCRTotals(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS),EdocsITSConstants.SPUpDateMDTOCRResults,id,docsOcr).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                retStr = ex.Message;
            }
            return retStr;
        }
        [HttpGet]
        public async Task<JsonResult> GetAsync()
        {
            try
            { 
            return EdocsITSApi.EdocsITSInstance.MDTGetRecsToOCT(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // POST api/<MDTOCRController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MDTOCRController>/5
        [HttpPut("{id}/numOCR")]
        public async Task<JsonResult> PutAsync(int id, int numOCR)
        {
            return null;
        }

        // DELETE api/<MDTOCRController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
