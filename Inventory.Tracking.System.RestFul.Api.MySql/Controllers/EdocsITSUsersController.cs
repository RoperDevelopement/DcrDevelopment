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
    public class EdocsITSUsersController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public EdocsITSUsersController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<EdocsITSUsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EdocsITSUsersController>/5
        [HttpGet("{edocsCusNameUserID}/{spName}")]
        public async Task<JsonResult> GetAsync(string edocsCusNameUserID,string spName)
        {
            try
            {

            
            //return EdocsITSApi.EdocsITSInstance.EdocsITSGetCustomer(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), spName, -1).ConfigureAwait(true).GetAwaiter().GetResult();
            return EdocsITSUsersApis.UsersInstance.EdocsITSGetUsers(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), spName, edocsCusNameUserID).ConfigureAwait(false).GetAwaiter().GetResult(); ;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST api/<EdocsITSUsersController>
        [HttpPost]
        public async Task<JsonResult> PostAsync([FromBody] EdocsITSUsersModel value)
        {
            try
            {
                return EdocsITSUsersApis.UsersInstance.UpdateUsersProfile(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS),EdocsITSConstants.SpEdocsITSUpDateUserProfile,value).ConfigureAwait(false).GetAwaiter().GetResult(); ;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT api/<EdocsITSUsersController>/5
        [HttpPut("{spName}")]
        public async Task<JsonResult> PutAsync(string spName, [FromBody] EdocsITSUsersModel value)
        {
            try
            {


                //return EdocsITSApi.EdocsITSInstance.EdocsITSGetCustomer(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), spName, -1).ConfigureAwait(true).GetAwaiter().GetResult();
                return EdocsITSUsersApis.UsersInstance.AddNewUserByCustomerName(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), spName, value).ConfigureAwait(false).GetAwaiter().GetResult(); ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // DELETE api/<EdocsITSUsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
