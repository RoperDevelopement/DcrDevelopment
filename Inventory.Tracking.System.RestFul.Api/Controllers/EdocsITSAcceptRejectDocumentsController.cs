using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Models;
using Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst;
using Microsoft.Extensions.Configuration;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Inventory.Tracking.System.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdocsITSAcceptRejectDocumentsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public EdocsITSAcceptRejectDocumentsController(IConfiguration config)
        {
            configuration = config;
        }

            // GET: api/<EdocsITSAcceptRejectDocumentsController>
            [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EdocsITSAcceptRejectDocumentsController>/5
        [HttpGet("{custID}")]
        public async Task<JsonResult> GetAsync(int custID)
        {
            return EdocsITSApi.EdocsITSInstance.EdocsInventoryTrackingGetDocuments(custID, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), EdocsITSConstants.SpGetDocumentsForReview).ConfigureAwait(true).GetAwaiter().GetResult();
        }
        // GET api/<EdocsITSAcceptRejectDocumentsController>/5
        [HttpGet("{custID}/{trackID}/{repType}/{repSDate}/{repEDate}")]
        public async Task<JsonResult> GetAsync(int custID,string trackID,string repType,string repSDate,string repEDate)
        {
            return EdocsITSApi.EdocsITSInstance.EdocsInventoryTrackingGetDocumentsByTrackID(custID, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), EdocsITSConstants.SpGetDocumentsForReviewByTrackingIDDocName, trackID,repType,repSDate,repEDate).ConfigureAwait(true).GetAwaiter().GetResult();
        }

        // POST api/<EdocsITSAcceptRejectDocumentsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            
        }

        // PUT api/<EdocsITSAcceptRejectDocumentsController>/5
        [HttpPut("{sp}")]
        public async Task<JsonResult> PutAsync(string sp, [FromBody] AcceptRejectDocumentsModel value)
        {
            return EdocsITSApi.EdocsITSInstance.UpDateAcceptRejectDocs(value, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), sp).ConfigureAwait(true).GetAwaiter().GetResult();
        }

        // DELETE api/<EdocsITSAcceptRejectDocumentsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
