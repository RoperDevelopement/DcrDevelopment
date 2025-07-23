using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using BinMonitorAppService.Models;
using BinMonitorAppService.Constants;
using Microsoft.Extensions.Configuration;
namespace Edocs.WebApi.BinManagerClasses
{
    public class AddNewBatches
    {


        public async Task RegisterBinSqlServer(BinRegistorModel regBatch, string sqlConnectionStr)
        {
            try
            {
                string completedAt = regBatch.RegCompletedAt.ToString().Replace("/0001", "/1989");
                string startedAt = regBatch.RegStartedAt.ToString().Replace("/0001", "/1989");

                if (regBatch.BatchID == Guid.Empty)
                    regBatch.BatchID = Guid.NewGuid();


                if ((regBatch.RegProcesClose == SqlConstants.RegAllBatch) || (regBatch.RegProcesClose == SqlConstants.RegBatch))
                {


                    Dictionary<string, string> dicRegBin = new Dictionary<string, string>();

                    dicRegBin.Add(SqlConstants.SpParmaBatchId, regBatch.BatchID.ToString());
                    if (string.IsNullOrWhiteSpace(regBatch.LabRecNumber))
                        throw new Exception("LabReq number cannot be empty");
                    dicRegBin.Add(SqlConstants.SpParmaLabRecNumber, regBatch.LabRecNumber);
                    if ((string.IsNullOrWhiteSpace(regBatch.BinID)) || (regBatch.BinID.Length > 4))
                        throw new Exception($"Invalid BinID: {regBatch.BinID}");
                    dicRegBin.Add(SqlConstants.SpParmaBinId, regBatch.BinID);
                    dicRegBin.Add(SqlConstants.SpParmaCategorieId, regBatch.CategoryName);
                    if (string.IsNullOrEmpty(regBatch.RegCreatedBy))
                        dicRegBin.Add(SqlConstants.SpParmaCreatedBy, Environment.UserName);
                    else
                        dicRegBin.Add(SqlConstants.SpParmaCreatedBy, regBatch.RegCreatedBy);
                    if (string.IsNullOrEmpty(regBatch.RegAssignedBy))
                        dicRegBin.Add(SqlConstants.SpParmaAssignedBy, SqlConstants.NoValue);
                    else
                        dicRegBin.Add(SqlConstants.SpParmaAssignedBy, regBatch.RegAssignedBy);
                    if (string.IsNullOrEmpty(regBatch.RegAssignedTo))
                        dicRegBin.Add(SqlConstants.SpParmaAssignedTo, SqlConstants.NoValue);
                    else
                        dicRegBin.Add(SqlConstants.SpParmaAssignedTo, regBatch.RegAssignedTo);

                    dicRegBin.Add(SqlConstants.SpParmaStartedAt, startedAt);
                    if (string.IsNullOrEmpty(regBatch.RegCompletedBy))
                        dicRegBin.Add(SqlConstants.SpParmaCompletedBy, SqlConstants.NoValue);
                    else
                        dicRegBin.Add(SqlConstants.SpParmaCompletedBy, regBatch.RegCompletedBy);
                    dicRegBin.Add(SqlConstants.SpParmaCreatedAt, completedAt);

                    if (regBatch.RegProcesClose == SqlConstants.RegBatch)
                        await AddUpdateBatch(dicRegBin, sqlConnectionStr, SqlConstants.SpAddBinRegistration);
                    else
                    {
                        await AddUpdateBatch(dicRegBin, sqlConnectionStr, SqlConstants.SpAddBinRegistration);
                        await ProcessingBinSqlServer(regBatch, sqlConnectionStr);
                        await BinColsedSqlServer(regBatch, sqlConnectionStr);
                    }
                }

                else if (regBatch.RegProcesClose == SqlConstants.BatchRegistraton)
                    await RegisterBatch(regBatch, sqlConnectionStr);
                else if (regBatch.RegProcesClose == SqlConstants.RegProcessBatches)
                    await ProcessingBinSqlServer(regBatch, sqlConnectionStr);
                else
                {
                    if (regBatch.RegProcesClose == SqlConstants.ClosegRegBatch)
                    {
                        await BinColsedSqlServer(regBatch, sqlConnectionStr);
                    }
                    else
                    {
                        throw new Exception($"Could not find what batch to regiter {regBatch.RegProcesClose}");
                    }
                }
                await BinCommentsContentsSqlServer(regBatch, sqlConnectionStr);
                regBatch.RegCompletedBy = string.Empty;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task BinColsedSqlServer(BinRegistorModel closeBatch, string sqlConnectionStr)
        {
            string completedAt = closeBatch.BinsClosedModel.BinCompletedAt.ToString().Replace("/0001", "/1989");
            string datePickeddUpAt = closeBatch.BinsClosedModel.DateBatchPickedUp.ToString().Replace("/0001", "/1989");
            string closedBy = string.Empty;
            string batchClosedBy = string.Empty;
            if (!(string.IsNullOrWhiteSpace(closeBatch.BinsClosedModel.BinClosedBy)))
            {
                closedBy = closeBatch.BinsClosedModel.BinClosedBy;
            }
            if (!(string.IsNullOrWhiteSpace(closeBatch.BinsClosedModel.BatchPickedUpBy)))
            {
                batchClosedBy = closeBatch.BinsClosedModel.BatchPickedUpBy;
            }
            Dictionary<string, string> dicRegBin = new Dictionary<string, string>();
            dicRegBin.Add(SqlConstants.SpParmaBatchId, closeBatch.BatchID.ToString());
            dicRegBin.Add(SqlConstants.SpParmaAssignedTo, closedBy);
            dicRegBin.Add(SqlConstants.SpParmaCompletedAt, completedAt);
            dicRegBin.Add(SqlConstants.SpParmaDateBatchPickedUp, datePickeddUpAt);
            dicRegBin.Add(SqlConstants.SpParmaBatchPickedUpBy, batchClosedBy);
            
            await AddUpdateBatch(dicRegBin, sqlConnectionStr, SqlConstants.SpBinClosed);
        }
        public async Task RegisterBatch(BinRegistorModel binBatch, string sqlConnectionStr)
        {
            Dictionary<string, string> dicBatch = new Dictionary<string, string>();
            dicBatch.Add(SqlConstants.SpParmaBinId, binBatch.BinID);
            dicBatch.Add(SqlConstants.SpParmaCategorieId, binBatch.CategoryName);
            dicBatch.Add(SqlConstants.SpParmaCompletedBy, binBatch.RegCompletedBy);
            dicBatch.Add(SqlConstants.SpParmaCreatedAt, binBatch.RegCompletedAt.ToString());
            await AddUpdateBatch(dicBatch, sqlConnectionStr, SqlConstants.SpBinRegisterBatch);

        }
        public async Task BinCommentsContentsSqlServer(BinRegistorModel contentComments, string sqlConnectionStr)
        {
            Dictionary<string, string> dicRegBin = new Dictionary<string, string>();
            string strContents = string.Empty;
            dicRegBin.Add(SqlConstants.SpParmaBatchId, contentComments.BatchID.ToString());
            if (contentComments.RegProcesClose != SqlConstants.RegAllBatch)
            {
                if ((string.IsNullOrEmpty(contentComments.BinComments)) && (string.IsNullOrEmpty(contentComments.BinContents)))
                    return;
            }
            if (string.IsNullOrEmpty(contentComments.BinComments))
                dicRegBin.Add(SqlConstants.SpParmaComments, string.Empty);
            else
                dicRegBin.Add(SqlConstants.SpParmaComments, contentComments.BinComments);
            if (string.IsNullOrEmpty(contentComments.BinContents))
                dicRegBin.Add(SqlConstants.SpParmaContents, string.Empty);
            //if (contentComments.Count > 0)
            //{
            //    foreach (string cont in contentComments.Specimens)
            //    {
            //        strContents += strContents + "\r\n";
            //    }
            //}
            else
                dicRegBin.Add(SqlConstants.SpParmaContents, contentComments.BinContents);
            await AddUpdateBatch(dicRegBin, sqlConnectionStr, SqlConstants.SpBinCommentsContents);
        }
        public async Task ProcessingBinSqlServer(BinRegistorModel regBatch, string sqlConnectionStr)
        {


            string completedAt = regBatch.BinProcessBinModel.ProcessCompletedAt.ToString().Replace("/0001", "/1989");
            string startedAt = regBatch.BinProcessBinModel.ProcessStartAt.ToString().Replace("/0001", "/1989");
            Dictionary<string, string> dicRegBin = new Dictionary<string, string>();
            dicRegBin.Add(SqlConstants.SpParmaBatchId, regBatch.BatchID.ToString());
            dicRegBin.Add(SqlConstants.SpParmaStartedAt, startedAt);
            if (string.IsNullOrEmpty(regBatch.BinProcessBinModel.ProcessAssignedBy))
                dicRegBin.Add(SqlConstants.SpParmaAssignedBy, SqlConstants.NoValue);
            else
                dicRegBin.Add(SqlConstants.SpParmaAssignedBy, regBatch.BinProcessBinModel.ProcessAssignedBy);
            if (string.IsNullOrEmpty(regBatch.BinProcessBinModel.ProcessAssignedBy))
                dicRegBin.Add(SqlConstants.SpParmaAssignedTo, SqlConstants.NoValue);
            else
                dicRegBin.Add(SqlConstants.SpParmaAssignedTo, regBatch.BinProcessBinModel.ProcessAssignedTo);
            if (string.IsNullOrEmpty(regBatch.BinProcessBinModel.ProcessCompletedBy))
                dicRegBin.Add(SqlConstants.SpParmaCompletedBy, SqlConstants.NoValue);
            else
                dicRegBin.Add(SqlConstants.SpParmaCompletedBy, regBatch.BinProcessBinModel.ProcessCompletedBy);


            dicRegBin.Add(SqlConstants.SpParmaCompletedAt, completedAt);
            await AddUpdateBatch(dicRegBin, sqlConnectionStr, SqlConstants.SpBinProcessing);
        }

        private async Task AddUpdateBatch(Dictionary<string, string> sqlParams, string sqlConnectionStr, string spName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        foreach (KeyValuePair<string, string> keyValuePair in sqlParams)
                        {

                            sqlCmd.Parameters.Add(new SqlParameter(keyValuePair.Key, keyValuePair.Value));
                        }
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

        public async Task UpdateEmailInfo(EmailReportModel emailReportModel, string sqlConnectionStr)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.spEmailReports, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaEmailTo, emailReportModel.EmailTo));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaEmailCC, emailReportModel.EmailCC));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaEmailFrequency, emailReportModel.EmailFrequency));
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

        public async Task UpdateBinRegStatusByBinId(BinLabRecModel binID,string sqlConnectionStr)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpCloseBinsByBinID, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinId, binID.BinID));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCompletedBy, binID.BinAssignedTo));
                        if(binID.BinProcessStarted)
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParamRegStatus,"1"));
                        else
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParamRegStatus, "0"));
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



        public async Task UpdateCategoryColors(CategoryColorModel categoryColorModel, string sqlConnectionStr)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpUpDateCategoryColors, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategorieId, categoryColorModel.CategoryName));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategorieColorHex, categoryColorModel.CategoryColorHexValue));
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
        public async Task UpdateEmailWorkFlow(WorkFlowEmailModel workFlowEmailModel, string sqlConnectionStr, string workFlowType)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpSendSpecBatchsEmails, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBatchId, workFlowEmailModel.BatchID));
                        if (workFlowEmailModel.EmailOnStart)
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaSendEmailStart, "True"));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaSendEmailStart, "False"));
                        if (workFlowEmailModel.EmailOnComplete)
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaSendEmailComplete, "True"));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaSendEmailComplete, "False"));

                        if (workFlowEmailModel.EmailContents)
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaSendEmailContents, "True"));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaSendEmailContents, "False"));

                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaEmailTo, workFlowEmailModel.EmailTo));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUpdateEmailTable, workFlowType));

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
