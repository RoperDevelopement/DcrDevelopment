using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Edocs.ITS.AppService.Models;
using Edocs.Libaray.Upload.Archive.Batches.Models;
using Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Inventory.Tracking.System.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdocsITSTrackingByProjectNameController : ControllerBase
    {

        private readonly IConfiguration configuration;
        public EdocsITSTrackingByProjectNameController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<EdocsITSTrackingByProjectNameController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EdocsITSTrackingByProjectNameController>/5
        [HttpGet("{id}")]
        public async Task<JsonResult> GetAsync(int id)
        {
            try
            {

                return EdocsITSApi.EdocsITSInstance.EdocsITSGetFileName(id, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), EdocsITSConstants.SPGetRepFileNameByID).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // GET api/<EdocsITSReportsController>/5
        [HttpGet("{trackingID}/{edocsCustomerID}/{numberDocsScanned}/{numberDocsUploaded}/{spName}/{loginName}")]
        public async Task GetAsync(string trackingID, int edocsCustomerID, int numberDocsScanned, int numberDocsUploaded, string spName, string loginName)
        {
            try
            {

                EdocsITSApi.EdocsITSInstance.EdocsAddInventoryTrackingIDByProjectName(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), trackingID, edocsCustomerID, numberDocsScanned, numberDocsUploaded, spName, loginName).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // POST api/<EdocsITSTrackingByProjectNameController>
        [HttpPost]
        public async Task Post([FromBody] UploadTrackingByProjectNameModel value)
        {
            try
            {
               
                 
                    EdocsITSApi.EdocsITSInstance.UploadTrackingByProject(value, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), EdocsITSConstants.SpUpDateITSTrackingIDByProjectName).ConfigureAwait(false).GetAwaiter().GetResult();


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        // PUT api/<EdocsITSTrackingByProjectNameController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EdocsITSTrackingByProjectNameController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
