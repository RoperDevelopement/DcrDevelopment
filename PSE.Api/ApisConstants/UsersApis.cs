using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json;
using System.Data;
using Edocs.PSE.Api.Models;
using System.Data.SqlClient;
using Edocs.WebApi.ApiClasses;
using Microsoft.AspNetCore.Mvc;
namespace Edocs.PSE.Api.ApisConstants
{
    public class UsersApis
    {
        private static UsersApis instance = null;

        private UsersApis() { }

        public static UsersApis UsersInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UsersApis();
                }
                return instance;
            }
        }

        public async Task<JsonResult> LogInUser(string sqlConnectionString,LoginModel loginModel,string storedProcedure)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                {
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                   
                            sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaLoginName,loginModel.UserName));
                      
                            sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaUserPassWord,loginModel.Password));

 
                    return JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                }
            }

        }
        public async Task UpDateLastLastMFLA(string sqlConnectionString, string logInName,int numNextMFLA, string storedProcedure)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                {
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaLoginName, logInName));

                    sqlCmd.Parameters.Add(new SqlParameter(PSEConstants.SpParmaNumDaysNextMFLA, numNextMFLA));

                    JsonBasicApis.JsonInstance.ExecSP(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                }
            }

        }
    }
}
