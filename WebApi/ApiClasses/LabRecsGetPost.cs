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
using Edocs.WebApi.ApiClasses;
using System.Threading;
using EDocs.Nyp.LabReqs.AppServices.Models;

namespace Edocs.WebApi.ApiClasses
{
    public class LabRecsGetPost
    {
        private static LabRecsGetPost instance = null;
        public static LabRecsGetPost PostLabRecsApisIntance
        {
            get
            {
                if (instance == null)
                    instance = new LabRecsGetPost();
                return instance;
            }
        }
        public async Task NewLabReqs(JsonFileLabRecsModel jsonFile, string sqlConnectionStr)
        {
            int numLoop = 0;
            while(numLoop++ <= 5)
            {
                try
                { 
                AddeNewLabRecs(jsonFile, sqlConnectionStr).ConfigureAwait(true).GetAwaiter().GetResult();
                    break;
                }
                catch(Exception ex)
                {
                   // if (ex.Message.ToLower().Contains("deadlock"))
                    //{
                        if (numLoop > 5)
                            throw new Exception(ex.Message);
                        Thread.Sleep(3000);
                    //}
                    //else
                     //   throw new Exception(ex.Message);

                }
                
            }
            
        }

        public async Task AddeNewLabRecs(JsonFileLabRecsModel jsonFile, string sqlConnectionStr)
        {
            try
            {
                string storedProcedueName = jsonFile.AzureStpredProcedureName;
                var js = JsonConvert.SerializeObject(jsonFile);
        

                
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 600;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaJasonFile,js));
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

        public async Task AddeNewLabRecBatch(AzureCloudBatchRecordsModel jsonFile, string sqlConnectionStr,string spName)
        {
            try
            {
                var js = JsonConvert.SerializeObject(jsonFile);
                //    var k  = JsonConvert.DeserializeObject<(jsonFile);

                //   var k1 = JsonConvert.DeserializeObject(jsonFile);


                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 600;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaJasonFile, js));
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

        public async Task AddNYPPDFFullText(PDFFullTextModel fullTextModel, string sqlConnectionStr)
        {
            try
            {
                
                //    var k  = JsonConvert.DeserializeObject<(jsonFile);

                //   var k1 = JsonConvert.DeserializeObject(jsonFile);


                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(ConstNypLabReqs.SpUploadLabReqsFullText, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 600;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaLabReqID,fullTextModel.ID));
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPDFFullText, fullTextModel.PDFFullText));
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
        public async Task UpDateNypLabReqNum(string sqlConnectionStr,int labReqID,string reqNUm)
        {
            try
            {

                //    var k  = JsonConvert.DeserializeObject<(jsonFile);

                //   var k1 = JsonConvert.DeserializeObject(jsonFile);


                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(ConstNypLabReqs.SpUpdateNypLabReqNumber, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 600;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaLabReqID, labReqID));
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaRequestionNumber, reqNUm));
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

        public async Task<JsonResult> GetRejectionLogsReason(string sqlConnectionString, string storedProcedure)
        {
            try
            {
                return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnectionString, storedProcedure).ConfigureAwait(true);
                //using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                //{
                //    using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                //    {
                //        sqlCmd.CommandType = CommandType.StoredProcedure;
                //        await sqlConnection.OpenAsync();
                //        SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();
                //        //if(dr.HasRows)
                //        //{
                //        //    dr.Read();
                //        //    string s = dr[0].ToString();
                //        //}
                //        var dt = new DataTable();
                //        dt.BeginLoadData();
                //        dt.Load(dr);
                //        dt.EndLoadData();
                //        dt.AcceptChanges();
                //        JsonResult jsonResult = new JsonResult(dt);

                //        return jsonResult;
                //    }

                //}
            }
            catch (Exception ex)
            {
                JsonResult jsonResult = new JsonResult(ex);
                return jsonResult;
                //throw new Exception(ex.Message);
            }

        }
        public async Task<JsonResult> GetLabRecs(string sqlConnectionString, string labRecCreateDate,string tableName)

        {
            try
            { 
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpGetLabRecs, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCreatedAt, labRecCreateDate));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmAzureTableName,tableName));
                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);
                    }

                }
            }
            catch (Exception ex)
            {
              
                throw new Exception(ex.Message);
            }

        }

        public async Task<JsonResult> GetLabRecsPDFToOcr(string sqlConnectionString, DateTime scanStartDate, DateTime scanEndDate)

        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(ConstNypLabReqs.SpNypLabReqsGetPDFSToOCR, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanStartDate, scanStartDate));
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanEndDate, scanEndDate));
                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<JsonResult> GetLabReqByID(string sqlConnectionString, int  labReqID)

        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(ConstNypLabReqs.SpNypLabRecGetByID, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaLabReqID, labReqID));
                        
                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<JsonResult> GetChangedLabReq(string sqlConnectionString, string startSDate, string endSDate, string labReqNum, string csnFinNumber, string patIDMRN,string MRN)

        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(ConstNypLabReqs.NypLabReqsChanged, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        if(string.Compare(SqlConstants.StrEdocsNoData,labReqNum,true) == 0)
                         sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaIndexNumber,DBNull.Value));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaIndexNumber, labReqNum));

                        if (string.Compare(SqlConstants.StrEdocsNoData, csnFinNumber, true) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaFinancialNumber, DBNull.Value));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaFinancialNumber, csnFinNumber));

                        

                        if (string.Compare(SqlConstants.StrEdocsNoData, patIDMRN, true) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientId, DBNull.Value));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientId, patIDMRN));

                        if (string.Compare(SqlConstants.StrEdocsNoData, MRN, true) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaMRN, DBNull.Value));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaMRN, MRN));

                        if (string.Compare(SqlConstants.StrEdocsNoData, startSDate, true) == 0)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, DBNull.Value));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, DBNull.Value));
                        }

                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, startSDate));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, endSDate));
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
    }
}
