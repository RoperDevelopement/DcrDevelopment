using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Models;
using BinMonitorAppService.Constants;
using Edocs.WebApi.BinManagerClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Edocs.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecMonitorUserController : ControllerBase
    {

        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;


        public SpecMonitorUserController(IConfiguration config, IEmailSettings email)
        {
            emailSettings = email;
            configuration = config;
        }


        // GET api/<controller>/5
        // [HttpGet("{cwid}/{passWord}")]
        [HttpGet("{cwid}/{passWord}")]
        public async Task<JsonResult> Get(string cwid, string passWord)
        {
            try
            {
                BinMonitorAddUpDate AddUpDate = new BinMonitorAddUpDate();
                var jr = await AddUpDate.LoginSpecMonitorUser(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString), cwid, passWord);
                return jr;
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running SpecMonitorUserController get run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $" cwid {cwid} password {passWord} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }


        }

        // POST api/<controller>
        [HttpPost]
        public async Task Post([FromBody]UsersModel value)
        {
            try
            {
                BinMonitorAddUpDate addUpDate = new BinMonitorAddUpDate();
                await addUpDate.AddSpectrumUsersLogin(value, configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString), SqlConstants.SpAddSpectrumMonitorUserLogin);
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running SpecMonitorUserController post run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $" Model UsersModel Cwid {value.Id} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{userPassWord}&{pwModify}")]
        public async Task<JsonResult> Put(string userPassWord, string pwModify, [FromBody]UsersModel value)
        {
            try
            {
                BinMonitorAddUpDate AddUpDate = new BinMonitorAddUpDate();
                // userPassWord = Request.Query["userPassWord"].ToString();
                // pwModify = Request.Query["pwModify"].ToString();

                var jr = await AddUpDate.AddSpectrumUsersLogin(value, configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString), SqlConstants.SpUpdateSpectrumMonitorUsers, userPassWord, pwModify);
                return jr;
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running SpecMonitorUserController put run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"userPassWord {userPassWord} pwModify {pwModify}  Model UsersModel {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }


        }

        // PUT api/<controller>/5
        [HttpPut("{spname}")]
        public async Task Put(string spname, [FromBody]SpectrumMonitorSettings value)
        {
            try
            {
                BinMonitorAddUpDate AddUpDate = new BinMonitorAddUpDate();
                // userPassWord = Request.Query["userPassWord"].ToString();
                // pwModify = Request.Query["pwModify"].ToString();

                 await AddUpDate.UpdateSpecMonSettings(value, configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString),spname).ConfigureAwait(true);
                 
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running SpecMonitorUserController put run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"spname {spname} Model SpectrumMonitorSettings {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }


        }

        // DELETE api/<controller>/5
        [HttpDelete("{cwid}")]
        public async Task Delete(string cwid)
        {
            try
            {
                BinMonitorAddUpDate addUpDate = new BinMonitorAddUpDate();
                await addUpDate.DelSpecMonitorUser(configuration.GetConnectionString(SqlConstants.CLoudConfigSqlConnectionString), cwid);
            }
            catch (Exception ex)
            {
                EmailService emailService = new EmailService(emailSettings);
                string emailSubject = $"Error running SpecMonitorUserController delete run time {DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}";
                string emailMessage = $"cwid {cwid} {ex.Message}";
                emailService.SendEmail(emailMessage, emailSubject);

                throw new Exception($"Error Message: {ex.Message}");
            }

        }
    }
}
