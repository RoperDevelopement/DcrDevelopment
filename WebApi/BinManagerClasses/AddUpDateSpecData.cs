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
using Edocs.WebApi.ApiClasses;

namespace Edocs.WebApi.BinManagerClasses
{
    public class AddUpDateSpecData
    {
        public async Task<JsonResult> GetCategoryDuration(string sqlConnectionString, string categoryName,string spname)

        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spname, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        if (string.Compare(categoryName, "na", true) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategorieId,DBNull.Value));
                        else
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategorieId, categoryName));
                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);

                    }

                }
            }
            catch (Exception ex)
            {
               throw new Exception(ex.Message);
            }

        }

        public async Task UpdateCategoryDurations(CategoryCheckPointEmailModel catCPModel, string sqlConnectionStr)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpUpDateCategoryDurationsModel, sqlConnection))
                    {
                        if (!(catCPModel.EmailAlerts))
                        { 
                            catCPModel.EmailTo = string.Empty;
                            catCPModel.Duration = "0.0";
                        }
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategorieId, catCPModel.CategoryName));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategorieDuration,catCPModel.Duration));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaFlash,catCPModel.Flash.ToString()));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaEmailTo, catCPModel.EmailTo));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaEmailAlerts, catCPModel.EmailAlerts.ToString()));
                        await sqlConnection.OpenAsync();
                        SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<JsonResult>GetUserInforByCwid(string sqlConnectionStr,string cwid)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpGetUserInfoCwid, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        await sqlConnection.OpenAsync();
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCwid, cwid));
                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task UpdateTransFromWfEmails(OldNewBatchIdModel batchIds, string sqlConnectionStr)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpTransFerWFSendEMail, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBatchId, batchIds.NewBatchId));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaOldBatchId, batchIds.OldBatchId));
                        await sqlConnection.OpenAsync();
                        SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();

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
