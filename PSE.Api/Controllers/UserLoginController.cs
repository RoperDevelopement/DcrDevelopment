using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Edocs.PSE.Api.Models;
using Edocs.PSE.Api.ApisConstants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.PSE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {

        private readonly IConfiguration configuration;
        public UserLoginController(IConfiguration config)
        {
            configuration = config;

        }
        // GET: api/<UserLoginController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserLoginController>/5
        [HttpGet("{userLoginID}/{numberDaysNextMFLN}/{storedProcedure}")]
        public async Task GetAsync(string userLoginID,int numberDaysNextMFLN,string storedProcedure)
        {
            try
            {
             
               UsersApis.UsersInstance.UpDateLastLastMFLA(configuration.GetConnectionString(PSEConstants.PSEConnectionStr), userLoginID, numberDaysNextMFLN, storedProcedure).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST api/<UserLoginController>
        [HttpPost]
        public async Task Post([FromBody] string value)
        {
            try
            {
                Console.WriteLine();
              //  UsersApis.UsersInstance.UpDateLastLastMFLA(configuration.GetConnectionString(PSEConstants.PSEConnectionStr), userLoginID, numberDaysNextMFLN, storedProcedure).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // PUT api/<UserLoginController>/5
        [HttpPut("{storedProcedure}")]

        public async Task<JsonResult> PutAsync(string storedProcedure, [FromBody] LoginModel value)
        {
            try
            {
                return UsersApis.UsersInstance.LogInUser(configuration.GetConnectionString(PSEConstants.PSEConnectionStr), value, storedProcedure).ConfigureAwait(false).GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        // DELETE api/<UserLoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
