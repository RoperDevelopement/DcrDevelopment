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
using Edocs.WebApi.ApiClasses;
namespace Edocs.WebApi.ApiClasses
{
    public class NypDrCodes
    {
        private static NypDrCodes instance = null;

        private NypDrCodes() { }

        public static NypDrCodes NypDrCodesInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NypDrCodes();
                }
                return instance;
            }
        }

        public async Task<JsonResult> GetNypDrCodes(string sqlConnectionString, string storedProcedure, DrCodeModel drCode)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                    
                        if (!(string.IsNullOrWhiteSpace(drCode.DrCode)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrCode,drCode.DrCode.Trim() ));
                        }
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrCode, DBNull.Value));
                        if (!(string.IsNullOrWhiteSpace(drCode.DrFName)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrFirstName, drCode.DrFName.Trim()));

                        }
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrFirstName,DBNull.Value));
                        if (!(string.IsNullOrWhiteSpace(drCode.DrLName)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrLastName, drCode.DrLName.Trim()));

                        }
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrLastName, DBNull.Value));
                        if (!(string.IsNullOrWhiteSpace(drCode.ScanOperator)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, drCode.ScanOperator.Trim()));

                        }
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, DBNull.Value));

                        if (drCode.ScanDate.Year > 2000)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, drCode.ScanDate));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, drCode.ScanEndDate));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, DBNull.Value));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, DBNull.Value));
                        }
                       
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, drCode.SearchPartial));

                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);
                    }

                }
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }

        public async Task<JsonResult> GetNypGrantRecp(string sqlConnectionString, string storedProcedure, GrantReceiptsModel receiptsModel)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 180;

                        if (!(string.IsNullOrWhiteSpace(receiptsModel.ClientCode)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaClientCode, receiptsModel.ClientCode.Trim()));
                        }
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaClientCode,"*"));
                        if (!(string.IsNullOrWhiteSpace(receiptsModel.ScanOperator)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator,receiptsModel.ScanOperator.Trim()));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, DBNull.Value));


                        if (receiptsModel.DocumentDate.Year > 2000)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, receiptsModel.DocumentDate));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, receiptsModel.ScanDate));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, DBNull.Value));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, DBNull.Value));
                        }

                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial,receiptsModel.SearchPartial));
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDateReceipteScan, receiptsModel.ScanByDate));

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
