using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using Edocs.ITS.AppService.Models;
using Edocs.Inventory.Tracking.System.RestFul.Api.Models;

namespace Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst
{
    public class EdocsITSUsersApis
    {
        private static EdocsITSUsersApis instance = null;

        private EdocsITSUsersApis() { }

        public static EdocsITSUsersApis UsersInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EdocsITSUsersApis();
                }
                return instance;
            }
        }

        public async Task<JsonResult> LogInUser(string sqlConnectionString, LoginModel loginModel, string storedProcedure)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                {
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaLoginName, loginModel.UserName));

                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaUserPassWord, loginModel.Password));


                    return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                }
            }

        }
        public async Task UpDateLastLastMFLA(string sqlConnectionString, string logInName, int numNextMFLA, string storedProcedure)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                {
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaLoginName, logInName));

                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaNumDaysNextMFLA, numNextMFLA));

                    EdocsITSJsonBasicApis.JsonInstance.ExecSP(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                }
            }

        }
        public async Task<JsonResult> EdocsITSGetUsers(string sqlConnectionStr, string storedProcedueName, string custName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        if (string.Compare(storedProcedueName, EdocsITSConstants.SpEdocsITSGetUserByUserName, true) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaLoginName, custName));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaEdocsCustomerName, custName));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting EdocsITS customers {ex.Message}");
            }
        }
      

        public async Task<JsonResult> AddNewUserByCustomerName(string sqlConnectionString, string storedProcedure, EdocsITSUsersModel iTSUsersModel)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                {
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, "-1"));
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaLoginName, iTSUsersModel.UserName));
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaUserPassWord, iTSUsersModel.Password));
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaUserFirstName, iTSUsersModel.FirstName));
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaUserLastName, iTSUsersModel.LastName));
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaUserEmail, iTSUsersModel.EmailAddress));
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerAdmin, iTSUsersModel.IsCustomerAdmin));
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaEdocsAdmin, iTSUsersModel.IsEdocsAdmin));
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCellPhoneNumber, iTSUsersModel.CellPhoneNumber));
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaEdocsCustomerName, iTSUsersModel.EdocsCustomerName));


                    return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                }
            }

        }
        public async Task<JsonResult> UpdateUsersProfile(string sqlConnectionString, string storedProcedure, EdocsITSUsersModel iTSUsersModel)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                {
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaLoginName, iTSUsersModel.UserName));
                    if (!(string.IsNullOrEmpty(iTSUsersModel.Password)))
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaUserPassWord, iTSUsersModel.Password));
                    else
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaUserPassWord, DBNull.Value));
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaUserFirstName, iTSUsersModel.FirstName));
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaUserLastName, iTSUsersModel.LastName));
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaUserEmail, iTSUsersModel.EmailAddress));
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerAdmin, iTSUsersModel.IsCustomerAdmin));
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaEdocsAdmin, iTSUsersModel.IsEdocsAdmin));
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCellPhoneNumber, iTSUsersModel.CellPhoneNumber));
                    sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaActive, iTSUsersModel.IsUserActive));
                    if (!(string.IsNullOrEmpty(iTSUsersModel.NewPassword)))
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaUserNewPassWord, iTSUsersModel.NewPassword));
                    else
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaUserNewPassWord, DBNull.Value));



                    return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                }
            }

        }
    }
}
