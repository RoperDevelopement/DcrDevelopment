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
    public class DOHApis
    {
        private static DOHApis instance = null;

        private DOHApis() { }

        public static DOHApis NypDOHInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DOHApis();
                }
                return instance;
            }
        }

        public async Task<JsonResult> DOHPerformingLabCodes(string storedProcedure, string sqlConnectionString)
        {

            try
            {
                return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnectionString, storedProcedure).ConfigureAwait(true);

            }
            catch (Exception ex)
            {
                JsonResult jsonResult = new JsonResult(ex.Message);
                return jsonResult;
            }
        }


        public async Task<JsonResult> DOHRecords(string sqlConnectionString, string spName, DohModel doh)

        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 180;
                        //if ((string.Compare(spName, ConstNypLabReqs.SpNypDohDateOfService, StringComparison.OrdinalIgnoreCase) != 0) && (string.Compare(spName, ConstNypLabReqs.SpNypDohScanOperatorDateOfService, StringComparison.OrdinalIgnoreCase) != 0))
                        //{
                        //    if (!(string.IsNullOrWhiteSpace(doh.AccessionNumber)))
                        //        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaAccessionNumber, doh.AccessionNumber));
                        //    else
                        //        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaAccessionNumber, DBNull.Value));
                        //    if (!(string.IsNullOrWhiteSpace(doh.MRN)))
                        //        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaRequestionNumber, doh.MRN));
                        //    else
                        //        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaRequestionNumber, DBNull.Value));


                        //}
                        //else
                        //{
                        //    if (string.Compare(spName, ConstNypLabReqs.SpNypDohScanOperatorDateOfService, StringComparison.OrdinalIgnoreCase) == 0)
                        //    {
                        //        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, doh.ScanOperator));

                        //    }
                        //}
                        if  (string.Compare(spName, ConstNypLabReqs.SpNypDohDateOfService, StringComparison.OrdinalIgnoreCase) != 0)
                            {
                            if (!(string.IsNullOrWhiteSpace(doh.AccessionNumber)) || (!(string.IsNullOrWhiteSpace(doh.MRN))))
                            {
                                if (!(string.IsNullOrWhiteSpace(doh.AccessionNumber)))
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaAccessionNumber, doh.AccessionNumber));
                                else
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaAccessionNumber, DBNull.Value));
                                if (!(string.IsNullOrWhiteSpace(doh.MRN)))
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaRequestionNumber, doh.MRN));
                                else
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaRequestionNumber, DBNull.Value));
                            }
                            else if (!(string.IsNullOrWhiteSpace(doh.DrFName)) || (!(string.IsNullOrWhiteSpace(doh.DrLName))))
                            {
                                if (!(string.IsNullOrWhiteSpace(doh.DrFName)))
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrFirstName, doh.DrFName));
                                else
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrFirstName, DBNull.Value));
                                if (!(string.IsNullOrWhiteSpace(doh.DrLName)))
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrLastName, doh.DrLName));
                                else
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrLastName, DBNull.Value));
                            }
                            else
                            {
                                if (!(string.IsNullOrWhiteSpace(doh.FirstName)))
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientFirstName, doh.FirstName));
                                else
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientFirstName, DBNull.Value));
                                if (!(string.IsNullOrWhiteSpace(doh.LastName)))
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientLastName, doh.LastName));
                                else
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientLastName, DBNull.Value));
                            }
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, doh.SearchPartial));
                        }
                        if (doh.ScanDate.Year > 1999)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, doh.DateOFService));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, doh.ScanDate));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, DBNull.Value));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, DBNull.Value));
                        }
                        //if (string.Compare(spName, ConstNypLabReqs.SpNypDohDateOfService, StringComparison.OrdinalIgnoreCase) != 0)
                        //{
                        //    if (doh.SearchPartial)
                        //    {
                        //        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, true));
                        //    }
                        //    else
                        //    {
                        //        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, false));
                        //    }

                        //}
                       
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDateReceipteScan, doh.ScanMachine));
                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);

                    }

                }
            }
            catch (Exception ex)
            {
                JsonResult jsonResult = new JsonResult(ex);
                return jsonResult;
                //throw new Exception(ex.Message);
            }

        }

    }
}
