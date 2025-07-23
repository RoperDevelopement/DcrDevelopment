using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Edocs.ITS.AppService.Models;
using Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst;
using Edocs.Inventory.Tracking.System.RestFul.Api.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Inventory.Tracking.System.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdocsITSUploadHtmlFilesController : ControllerBase
    {

        private readonly IConfiguration configuration;
        public EdocsITSUploadHtmlFilesController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<EdocsITSUploadHtmlFilesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EdocsITSUploadHtmlFilesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            HtmlFileModel value = new HtmlFileModel { InvoiceNum = id, HtmlData = string.Empty, };
             return EdocsITSApi.EdocsITSInstance.UpLoadInvoiceHtml(value, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), EdocsITSConstants.SPGetInvoiceToPrint).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        // POST api/<EdocsITSUploadHtmlFilesController>
        [HttpPost]
        public async Task<JsonResult> PostAsync([FromBody] HtmlFileModel value)
        {
            
            return EdocsITSApi.EdocsITSInstance.UpLoadInvoiceHtml(value, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), EdocsITSConstants.SPUpdateUploadInvoice).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        //[HttpPost]
        //public async Task<JsonResult> PostAsync([FromBody] AddInvoiceNumberModel value)
        //{

        //    return EdocsITSApi.EdocsITSInstance.AddInvoiceNumber(value, configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS)).ConfigureAwait(false).GetAwaiter().GetResult();
        //}


         

        // DELETE api/<EdocsITSUploadHtmlFilesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
