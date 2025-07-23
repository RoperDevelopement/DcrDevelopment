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
    public class ApiNypAuditLogs
    {
        private static ApiNypAuditLogs instance = null;

        private ApiNypAuditLogs() { }

        public static ApiNypAuditLogs NypAuditLogsInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ApiNypAuditLogs();
                }
                return instance;
            }
        }

        public async Task UpLoadNypAuditLogs(AuditLogsModel auditLogsModel, string sqlConnectionString)
        {

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpUpLoadNypAuditLogs, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCwid, auditLogsModel.Cwid.Trim()));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaAuditLogDate, auditLogsModel.AuditLogDate));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaAuditLogApplicationName, auditLogsModel.AuditLogApplicationName));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaAuditLogUrl, auditLogsModel.AuditLogUrl.ToString()));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaAuditLogMessageType, auditLogsModel.AuditLogMessageType));
                        sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        SqlDataReader dr = sqlCmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();


                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Method UpLoadNypAuditLogs for cwid {auditLogsModel.Cwid} audit log url {auditLogsModel.AuditLogUrl.ToString()} {ex.Message}");
            }

        }


        public async Task<JsonResult> GetNypAuditLogs(AuditLogsModel auditLogsModel, string sqlConnectionString, string spName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 180;
                        if (!(string.IsNullOrWhiteSpace(auditLogsModel.Cwid)))
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCwid, auditLogsModel.Cwid));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCwid, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(auditLogsModel.AuditLogApplicationName)))
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaAuditLogApplicationName, auditLogsModel.AuditLogApplicationName));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaAuditLogApplicationName, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(auditLogsModel.AuditLogMessageType)))
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaAuditLogMessageType, auditLogsModel.AuditLogMessageType));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaAuditLogMessageType, DBNull.Value));

                        if (auditLogsModel.AuditLogDate.Year > 1900)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, auditLogsModel.AuditLogDate));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, auditLogsModel.AuditLogUpLoadDate));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, DBNull.Value));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, DBNull.Value));
                        }
                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);
                    }
                }


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }

        public async Task<JsonResult> GetAuditLogsComBox(string storedProcedure, string sqlConnectionString)
        {

            try
            {
                return JsonBasicApis.JsonInstance.GetJsonResults(sqlConnectionString, storedProcedure).ConfigureAwait(false).GetAwaiter().GetResult();


            }
            catch (Exception ex)
            {

                throw new Exception($"Method GetAuditLogsComBox for sp {storedProcedure} sql string {sqlConnectionString} {ex.Message}");
            }

        }
    }


}

