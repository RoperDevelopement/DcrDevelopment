using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Edocs.ITS.AppService.Models;
using Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.Inventory.Tracking.System.RestFul.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdocsITSReportsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public EdocsITSReportsController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<EdocsITSReportsController>
        [HttpGet("{custId}")]
        public async Task<JsonResult> GetAsync(int custID)
        {
            try
            { 
            return EdocsITSApi.EdocsITSInstance.GetProjectMinMaxDate(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS),custID).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // GET: api/<EdocsITSReportsController>
        [HttpGet("{trackingID}/{spName}")]
        public async Task<JsonResult> GetAsync(string trackingID,string spName)
        {
            try
            {
                

                 return EdocsITSApi.EdocsITSInstance.GetTrackingIds(trackingID,configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS),spName).ConfigureAwait(false).GetAwaiter().GetResult();
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET api/<EdocsITSReportsController>/5
        [HttpGet("{repType}/{stDate}/{endDate}/{spName}/{custName}")]
        public async Task<JsonResult> GetAsync(string repType, string stDate, string endDate, string spName,string custName)
        {
            try
            {
                             return   EdocsITSApi.EdocsITSInstance.ReportDocSent(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), repType, stDate, endDate, spName, custName).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // GET api/<EdocsITSReportsController>/5
        //[HttpGet("{custID}/{stDate}/{endDate}")]
        //public async Task<JsonResult> GetAsync(string custName,string stDate, string endDate)
        //{
        //    try
        //    {
        //        return EdocsITSApi.EdocsITSInstance.ReportDocSent(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), repType, stDate, endDate, spName, custName).ConfigureAwait(false).GetAwaiter().GetResult();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        //  GET api/<EdocsITSReportsController>/5
        //[HttpGet("{stDate}/{endDate}/{custID}")]
        //public async Task<JsonResult> GetAsync(string stDate, string endDate, int custID)
        //{
        //    try
        //    {
        //        return EdocsITSApi.EdocsITSInstance.ScanningCost(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), stDate, endDate, custID).ConfigureAwait(false).GetAwaiter().GetResult();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        // GET api/<EdocsITSReportsController>/5
        [HttpGet("{stDate}/{endDate}/{spName}")]
        public async Task<JsonResult> GetAsync(string stDate, string endDate,string spName)
        {
            try
            {
               
                return EdocsITSApi.EdocsITSInstance.ReportProjectNameNum(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS),stDate,endDate,spName).ConfigureAwait(false).GetAwaiter().GetResult();
               
               
            
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        // GET api/<EdocsITSReportsController>/5
        [HttpGet("{stDate}/{endDate}/{trackinIDDocname}/{repType}")]
        public async Task<JsonResult> GetAsync(string stDate, string endDate, string trackinIDDocname,string repType)
        {
            try
            {

                return EdocsITSApi.EdocsITSInstance.ReportTrackingIDDocNameNum(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), stDate, endDate, trackinIDDocname,repType).ConfigureAwait(false).GetAwaiter().GetResult();



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        // POST api/<EdocsITSReportsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<EdocsITSReportsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EdocsITSReportsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
