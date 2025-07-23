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
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;

namespace Edocs.WebApi.ApiClasses
{
    public class SpecimenRejectionGetPost
    {
        private static SpecimenRejectionGetPost instance = null;

        private SpecimenRejectionGetPost() { }

        public static SpecimenRejectionGetPost SpecimenRejectionInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SpecimenRejectionGetPost();
                }
                return instance;
            }
        }

        public async Task<JsonResult> GetRejectionLogsReason(string sqlConnectionString, string storedProcedure)
        {
            try
            {
                return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnectionString, storedProcedure).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }

        public async Task<JsonResult> NypSpecimenRejectionLogs(string sqlConnectionString, string storedProcedure, SpecimenRejectionModel rejectionModel)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 180;

                        if (!(string.IsNullOrWhiteSpace(rejectionModel.FromYear)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaFromYear, rejectionModel.FromYear.Trim()));

                        }
                        if (!(string.IsNullOrWhiteSpace(rejectionModel.FromNumber)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaFromNumber, rejectionModel.FromNumber.Trim()));

                        }
                        if (!(string.IsNullOrWhiteSpace(rejectionModel.ScanOperator)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, rejectionModel.ScanOperator.Trim()));

                        }
                        if (!(string.IsNullOrWhiteSpace(rejectionModel.FromReason)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaRejectionReason, rejectionModel.FromReason.Trim()));

                        }
                        if (!(string.IsNullOrWhiteSpace(rejectionModel.ScanBatch)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanBatchId, rejectionModel.ScanBatch.Trim()));

                        }
                        if (rejectionModel.LogDate.Year > 2000)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, rejectionModel.LogDate));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, rejectionModel.ScanDate));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, DBNull.Value));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, DBNull.Value));
                        }
                        if ((string.Compare(storedProcedure, ConstNypLabReqs.SpNypSpecimenRejectionReasonByLogDate, true) == 0) || (string.Compare(storedProcedure, ConstNypLabReqs.SpNypSpecimenRejectionReasonByLogDate, true) == 0))
                        {
                            rejectionModel.ModifyBy = string.Empty;
                        }
                        if ((string.Compare(ConstNypLabReqs.SearchParStr, rejectionModel.ModifyBy, true) == 0))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, rejectionModel.SearchPartial));
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

        public async Task<JsonResult> NypSpecimenLookupLabReq(string sqlConnectionString, string storedProcedure,string labReqNum,DateTime sDate,DateTime eDate)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 180;

                        
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaLabRecNumber,labReqNum));

                        
                        
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusStartDate,sDate));

                        
                        
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusEndDate, eDate));

                        
                        
                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<JsonResult> NypSpecimenTransferReport(string sqlConnectionString, string storedProcedure,DateTime sDate, DateTime eDate, string transFerType)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 180;


                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaTransFerType, transFerType));



                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusStartDate, sDate));



                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusEndDate, eDate));



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
