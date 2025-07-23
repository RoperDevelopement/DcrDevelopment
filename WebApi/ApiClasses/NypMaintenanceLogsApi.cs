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
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
namespace Edocs.WebApi.ApiClasses
{
    public class NypMaintenanceLogsApi
    {
        private static NypMaintenanceLogsApi instance = null;

        private NypMaintenanceLogsApi() { }

        public static NypMaintenanceLogsApi NypMLInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NypMaintenanceLogsApi();
                }
                return instance;
            }
        }

        public async Task<JsonResult> MLLogStations(string sqlConnectionString, string storedProcedure)
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

        public async Task<JsonResult> NypNypMaintenanceLogs(string sqlConnectionString, string spName, MaintenanceLogsModel logsModel)

        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        if ((string.Compare(spName, ConstNypLabReqs.SpNypMaintenanceLogsByLogDate, StringComparison.OrdinalIgnoreCase) == 0) || (string.Compare(spName, ConstNypLabReqs.SpNypMissingPunchFormsLocationByLogDate, StringComparison.OrdinalIgnoreCase) == 0))
                        {
                            if (!(string.IsNullOrWhiteSpace(logsModel.LogStation)))
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaLogStation, logsModel.LogStation));
                            else
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaLogStation, DBNull.Value));
                            if (!(string.IsNullOrWhiteSpace(logsModel.ScanOperator)))
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, logsModel.ScanOperator));
                            else
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, DBNull.Value));


                        }


                        if (logsModel.LogDate.Year > 1999)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, logsModel.LogDate));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, logsModel.ScanDate));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, DBNull.Value));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, DBNull.Value));
                        }
                        if ((string.Compare(spName, ConstNypLabReqs.SpNypMaintenanceLogsLogDate, StringComparison.OrdinalIgnoreCase) != 0) && (string.Compare(spName, ConstNypLabReqs.SpNypMissingPunchFormsLogDate, StringComparison.OrdinalIgnoreCase) != 0))
                        {
                            if (logsModel.SearchPartial)
                            {
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, true));
                            }
                            else
                            {
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, false));
                            }

                        }
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDateReceipteScan, logsModel.ScanMachine));
                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);
                    }

                }
            }
            catch (Exception ex)
            {
               
               throw new Exception(ex.Message);
            }

        }

        public async Task<JsonResult> NypPackingSlips(string sqlConnectionString, string spName, SendOutPackingSlipsModel sendOutPackingSlipsModel)

        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        if (!(string.IsNullOrWhiteSpace(sendOutPackingSlipsModel.ScanOperator)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, sendOutPackingSlipsModel.ScanOperator));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, DBNull.Value));





                        if (sendOutPackingSlipsModel.DateOfService.Year > 1999)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, sendOutPackingSlipsModel.DateOfService));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, sendOutPackingSlipsModel.ScanDate));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, DBNull.Value));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, DBNull.Value));
                        }

                        if (sendOutPackingSlipsModel.SearchPartial)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, true));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, false));
                        }


                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDateReceipteScan, sendOutPackingSlipsModel.ScanMachine));
                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);
                    }

                }
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }

        public async Task<JsonResult> NypSendOutResults(string sqlConnectionString, string spName, SendOutResultsModel sendOutResults)

        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 180;

                        if (!(string.IsNullOrWhiteSpace(sendOutResults.PerformingLabCode)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPerFormingLabCode, sendOutResults.PerformingLabCode));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPerFormingLabCode, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(sendOutResults.AccessionNumber)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaAccessionNumber, sendOutResults.AccessionNumber ));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaAccessionNumber, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(sendOutResults.FinancialNumber)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaFinancialNumber, sendOutResults.FinancialNumber));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaFinancialNumber, DBNull.Value));


                        if (!(string.IsNullOrWhiteSpace(sendOutResults.MRN)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaRequestionNumber, sendOutResults.MRN));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaRequestionNumber, DBNull.Value));


                        if (!(string.IsNullOrWhiteSpace(sendOutResults.FirstName)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientFirstName, sendOutResults.FirstName));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientFirstName, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(sendOutResults.LastName)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientLastName, sendOutResults.LastName));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientLastName, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(sendOutResults.ScanOperator)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, sendOutResults.ScanOperator));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, DBNull.Value));





                        if (sendOutResults.DateOFService.Year > 1999)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, sendOutResults.DateOFService));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, sendOutResults.ScanDate));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, DBNull.Value));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, DBNull.Value));
                        }

                        if (sendOutResults.SearchPartial)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, true));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, false));
                        }


                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDateReceipteScan, sendOutResults.ScanMachine));
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
