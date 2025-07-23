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
    public class EdocsITSUserLoginController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public EdocsITSUserLoginController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<EdocsITSUserLoginController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EdocsITSUserLoginController>/5
        [HttpGet("{userLoginID}/{numberDaysNextMFLN}/{storedProcedure}")]
        public async Task GetAsync(string userLoginID, int numberDaysNextMFLN, string storedProcedure)
        {
            try
            {

                EdocsITSUsersApis.UsersInstance.UpDateLastLastMFLA(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), userLoginID, numberDaysNextMFLN, storedProcedure).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST api/<EdocsITSUserLoginController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<EdocsITSUserLoginController>/5
        [HttpPut("{storedProcedure}")]

        public async Task<JsonResult> PutAsync(string storedProcedure, [FromBody] LoginModel value)
        {
            try
            {
                return EdocsITSUsersApis.UsersInstance.LogInUser(configuration.GetConnectionString(EdocsITSConstants.EdocsInventoryTrackingSystemCS), value, storedProcedure).ConfigureAwait(false).GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        // DELETE api/<EdocsITSUserLoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
