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
    public class GetNypLabReqs
    {

        public async Task<JsonResult> GetNypCOVID19LabRecsByIndexFinNum(string sqlConnectionString, string spName, LabReqsModel reqsModel)

        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 600;
                        if (string.Compare(spName, ConstNypLabReqs.SpNypCOVID19LabIndexFinancialNumber, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaFinancialNumber, reqsModel.FinancialNumber));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaIndexNumber, reqsModel.IndexNumber));
                        }
                        else if (string.Compare(spName, ConstNypLabReqs.SpNypCOVID19LabReqsPatientId, StringComparison.OrdinalIgnoreCase) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientId, reqsModel.PatientID));
                        else if (string.Compare(spName, ConstNypLabReqs.SpNypCOVID19LabReqsClientCode, StringComparison.OrdinalIgnoreCase) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaClientCode, reqsModel.ClientCode));
                        else if (string.Compare(spName, ConstNypLabReqs.SpNypCOVID19LabReqsDrCode, StringComparison.OrdinalIgnoreCase) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrCode, reqsModel.DrCode));
                        else if (string.Compare(spName, ConstNypLabReqs.SpNypCOVID19LabReqsPatientFirstName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            string[] pName = reqsModel.PatientName.Split('@');
                            if (pName[0] == "N/A")
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientFirstName, DBNull.Value));
                            else
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientFirstName, pName[0]));
                            if (pName[1] == "N/A")
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientLastName, DBNull.Value));
                            else
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientLastName, pName[1]));
                        }

                        //else if (string.Compare(spName, ConstNypLabReqs.SpNypCOVID19LabReqsPatientLastName, StringComparison.OrdinalIgnoreCase) == 0)
                        //    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientLastName, reqsModel.PatientName));
                        else if (string.Compare(spName, ConstNypLabReqs.SpNypCOVID19LabReqsDrName, StringComparison.OrdinalIgnoreCase) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrName, reqsModel.DrName));
                        else if (string.Compare(spName, ConstNypLabReqs.SpNypCOVID19LabReqsScanOperator, StringComparison.OrdinalIgnoreCase) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, reqsModel.ScanOperator));
                        else if (string.Compare(spName, ConstNypLabReqs.SpNypCOVID19LabReqsScanBatch, StringComparison.OrdinalIgnoreCase) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanBatchId, reqsModel.ScanBatch));
                        else if (string.Compare(spName, ConstNypLabReqs.SpNypCOVID19LabReqsRequestionNumber, StringComparison.OrdinalIgnoreCase) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaRequestionNumber, reqsModel.RequisitionNumber));
                        else if (string.Compare(spName, ConstNypLabReqs.SpNypCOVID19LabReqsDosRecScanDate, StringComparison.OrdinalIgnoreCase) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDosRecScanDate, reqsModel.ScanMachine));

                        else
                            throw new Exception($"Stored  procedure {spName} not found");



                        if (reqsModel.ScanDate.Year > 1999)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, reqsModel.ReceiptDate));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, reqsModel.ScanDate));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, DBNull.Value));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, DBNull.Value));
                        }
                        if (string.Compare(spName, ConstNypLabReqs.SpNypCOVID19LabReqsDosRecScanDate, StringComparison.OrdinalIgnoreCase) != 0)
                        {
                            if (reqsModel.SearchPartial)
                            {
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, true));
                            }
                            else
                            {
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, false));
                            }
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDateReceipteScan, reqsModel.ScanMachine));
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
        public async Task<JsonResult> GetNypLabRecsByIndexFinNum(string sqlConnectionString, string spName, LabReqsModel reqsModel)

        {


            try
            {
                if (spName.ToLower().StartsWith(ConstNypLabReqs.SpNypCOVID19StartsWith))
                {
                    return GetNypCOVID19LabRecsByIndexFinNum(sqlConnectionString, spName, reqsModel).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                else
                {
                    using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                    {
                        using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                        {
                            sqlCmd.CommandTimeout = 600;
                            sqlCmd.CommandType = CommandType.StoredProcedure;

                            if (string.Compare(spName, ConstNypLabReqs.SpNypLabIndexFinancialNumber, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                //  sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaFinancialNumber, reqsModel.FinancialNumber));

                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaIndexNumber, reqsModel.IndexNumber));
                            }
                            else if (string.Compare(spName, ConstNypLabReqs.SpNypLabIndexFinancialandCSNNumber, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaIndexNumber, reqsModel.IndexNumber));
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaFinancialNumber, reqsModel.FinancialNumber));
                            }

                            else if (string.Compare(spName, ConstNypLabReqs.SpNypLabReqsCSNNumber, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaFinancialNumber, reqsModel.FinancialNumber));
                                //sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaIndexNumber, reqsModel.IndexNumber));
                            }

                            else if (string.Compare(spName, ConstNypLabReqs.SpNypLabReqsPatientId, StringComparison.OrdinalIgnoreCase) == 0)
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientId, reqsModel.PatientID));
                            else if (string.Compare(spName, ConstNypLabReqs.SpNypLabReqsClientCode, StringComparison.OrdinalIgnoreCase) == 0)
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaClientCode, reqsModel.ClientCode));
                            else if (string.Compare(spName, ConstNypLabReqs.SpNypLabReqsDrCode, StringComparison.OrdinalIgnoreCase) == 0)
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrCode, reqsModel.DrCode));
                            else if (string.Compare(spName, ConstNypLabReqs.SpNypLabReqsPatientFirstName, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                string[] pName = reqsModel.PatientName.Split('@');
                                if (pName[0] == "N/A")
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientFirstName, DBNull.Value));
                                else
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientFirstName, pName[0]));
                                if (pName[1] == "N/A")
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientLastName, DBNull.Value));
                                else
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientLastName, pName[1]));
                            }
                            //sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientFirstName, reqsModel.PatientName));
                            else if (string.Compare(spName, ConstNypLabReqs.SpNypLabReqsPatientLastName, StringComparison.OrdinalIgnoreCase) == 0)
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientLastName, reqsModel.PatientName));
                            else if (string.Compare(spName, ConstNypLabReqs.SpNypLabReqsDrName, StringComparison.OrdinalIgnoreCase) == 0)
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrName, reqsModel.DrName));
                            else if (string.Compare(spName, ConstNypLabReqs.SpNypLabReqsScanOperator, StringComparison.OrdinalIgnoreCase) == 0)
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, reqsModel.ScanOperator));
                            else if (string.Compare(spName, ConstNypLabReqs.SpNypLabReqsScanBatch, StringComparison.OrdinalIgnoreCase) == 0)
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanBatchId, reqsModel.ScanBatch));
                            else if (string.Compare(spName, ConstNypLabReqs.SpNypLabRequestionNumber, StringComparison.OrdinalIgnoreCase) == 0)
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaRequestionNumber, reqsModel.RequisitionNumber));
                            else if (string.Compare(spName, ConstNypLabReqs.SpNypLabReqsDosRecScanDate, StringComparison.OrdinalIgnoreCase) == 0)
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDosRecScanDate, reqsModel.ScanMachine));
                            else if (string.Compare(spName, ConstNypLabReqs.SpNypLabMRNRecDate, StringComparison.OrdinalIgnoreCase) == 0)
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaMRN, reqsModel.MRN));
                            else if (string.Compare(spName, ConstNypLabReqs.SpNypLabMRNScanDate, StringComparison.OrdinalIgnoreCase) == 0)
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaMRN, reqsModel.MRN));

                            else
                                throw new Exception($"Stored  procedure {spName} not found");



                            if (reqsModel.ScanDate.Year > 1999)
                            {
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, reqsModel.ReceiptDate));
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, reqsModel.ScanDate));
                            }
                            else
                            {
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, DBNull.Value));
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, DBNull.Value));
                            }
                            if (string.Compare(spName, ConstNypLabReqs.SpNypLabReqsDosRecScanDate, StringComparison.OrdinalIgnoreCase) != 0)
                            {
                                if (reqsModel.SearchPartial)
                                {
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, true));
                                }
                                else
                                {
                                    sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, false));
                                }
                                sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDateReceipteScan, reqsModel.ScanMachine));
                            }
                            return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<JsonResult> GetLabRecsUpLoaded(string sqlConnectionString, string batchId, string jsonFile, string reportSrch, int totalJsonFile, string dateUpLoaded)

        {


            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(ConstNypLabReqs.SpNypNypLabReqsReport, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;


                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanBatchId, batchId));
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaJsonFile, jsonFile));
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaReportUploadSearch, reportSrch));
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaTotalJsonFile, totalJsonFile));
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDosRecScanDate, dateUpLoaded));
                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);
                    }

                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<JsonResult> UpDateNypLabReqsByID(string sqlConnectionString, string spName, LabReqsEditModel reqsModel)

        {


            try
            {
                // if (spName.ToLower().StartsWith(ConstNypLabReqs.SpNypCOVID19StartsWith))
                // {
                //     return GetNypCOVID19LabRecsByIndexFinNum(sqlConnectionString, spName, reqsModel).ConfigureAwait(false).GetAwaiter().GetResult();
                //  }

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 600;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaLabReqID, reqsModel.LabReqID));

                        if (!(string.IsNullOrWhiteSpace(reqsModel.PatientID)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientId, reqsModel.PatientID));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientId, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(reqsModel.MRN)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaMRN, reqsModel.MRN));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaMRN, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(reqsModel.ClientCode)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaClientCode, reqsModel.ClientCode));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaClientCode, DBNull.Value));
                        if (!(string.IsNullOrWhiteSpace(reqsModel.PatientFName)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientFirstName, reqsModel.PatientFName));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientFirstName, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(reqsModel.PatientLName)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientLastName, reqsModel.PatientLName));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaPatientLastName, DBNull.Value));

                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaIndexNumber, reqsModel.IndexNumber));
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaFinancialNumber, reqsModel.FinancialNumber));

                        if (!(string.IsNullOrWhiteSpace(reqsModel.DrCode)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrCode, reqsModel.DrCode));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrCode, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(reqsModel.DrFName)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrFirstName, reqsModel.DrFName));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrFirstName, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(reqsModel.DrLName)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrLastName, reqsModel.DrLName));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDrLastName, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(reqsModel.RequisitionNumber)))
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaRequestionNumber, reqsModel.RequisitionNumber));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaRequestionNumber, DBNull.Value));


                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaUserModified, reqsModel.ModifyBy));
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDateOfService, reqsModel.DateOfService));


                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);


                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<JsonResult> GetLabKeyWords(string sqlConnectionString, DateTime startdate, DateTime enddate, string searchBy, string keywords)

        {


            try
            {
                keywords = keywords.Replace(ConstNypLabReqs.DoubleQuotes, "");
                ;
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(ConstNypLabReqs.SpGetLabReqsByFullText, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;


                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaKeyWordSearch, keywords.Trim()));
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, startdate));
                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, enddate));
                        if (string.Compare(searchBy, "scandate", true) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDateReceipteScan, "scanDate"));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaDateReceipteScan, "recpdate"));
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
