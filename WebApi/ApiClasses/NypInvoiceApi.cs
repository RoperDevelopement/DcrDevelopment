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
    public class NypInvoiceApi
    {
        private static NypInvoiceApi instance = null;
        public static NypInvoiceApi InvoiceIntance
        {
            get
            {
                if (instance == null)
                    instance = new NypInvoiceApi();
                return instance;
            }
        }
        private NypInvoiceApi()
        {
        }


        public async Task<JsonResult> NypIvoiceOptions(string sqlConnectionString, string storedProcedure)
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

        public async Task<JsonResult> NypInvoices(string sqlConnectionString, string spName, InvoicesModel invoices)

        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        if (!(string.IsNullOrWhiteSpace(invoices.Department)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDepartment, invoices.Department));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDepartment, DBNull.Value));
                        if (!(string.IsNullOrWhiteSpace(invoices.Category)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaCategory, invoices.Category));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaCategory, DBNull.Value));
                        if (!(string.IsNullOrWhiteSpace(invoices.Account)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaAccount, invoices.Account));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaAccount, DBNull.Value));
                        if (!(string.IsNullOrWhiteSpace(invoices.Reference)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaReferance, invoices.Reference));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaReferance, DBNull.Value));
                        if (!(string.IsNullOrWhiteSpace(invoices.ScanOperator)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, invoices.ScanOperator));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, DBNull.Value));





                        if (invoices.InvoiceDate.Year > 1999)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, invoices.InvoiceDate));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, invoices.ScanDate));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, DBNull.Value));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, DBNull.Value));
                        }

                        if (invoices.SearchPartial)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, true));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, false));
                        }


                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDateReceipteScan, invoices.ScanMachine));
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
