using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Models;
using BinMonitorAppService.Constants;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Edocs.Libaray.AzureCloud.Upload.Batches;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using EDocs.Nyp.LabReqs.AppServices.Models;

namespace Edocs.WebApi.ApiClasses
{
    public class ApiLabReqsUsers
    {
        private static ApiLabReqsUsers instance = null;

        private ApiLabReqsUsers() { }

        public static ApiLabReqsUsers NypLabReqUsersInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ApiLabReqsUsers();
                }
                return instance;
            }
        }

        public async Task<JsonResult> GetNypLabRecUserCwid(string sqlConnectionString,string cwid, string firstName, string lastName, string emailAddress, string command,bool active,bool isAdmin,bool viewAuditLogs,bool editLRDocs)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpNypLabReqUser, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 180;
                        
                       if(!(string.IsNullOrWhiteSpace(cwid))) 
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCwid, cwid.Trim()));
                       else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCwid, DBNull.Value));



                        if (!(string.IsNullOrWhiteSpace(firstName)))
                                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserFirstName,firstName));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserFirstName, DBNull.Value));
                        if (!(string.IsNullOrWhiteSpace(lastName)))
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserLastName, lastName));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserLastName, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(emailAddress)))
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserEmailAddress, emailAddress));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserEmailAddress, DBNull.Value));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaViewAuditLogs, viewAuditLogs));
                        

                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaActive,active));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaIsAdmin,isAdmin));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaAction, command));
                          sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaEditLRDocs, editLRDocs));

                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
    }
}
