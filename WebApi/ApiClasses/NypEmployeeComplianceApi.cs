using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;


namespace Edocs.WebApi.ApiClasses
{
    public class NypEmployeeComplianceApi
    {
        private static NypEmployeeComplianceApi instance = null;
        public static NypEmployeeComplianceApi EmployeeCompliamceIntance
        {
            get
            {
                if (instance == null)
                    instance = new NypEmployeeComplianceApi();
                return instance;
            }
        }
        private NypEmployeeComplianceApi()
        {
        }

        public async Task<JsonResult> EmployeeComplianceCodes(string sqlConnectionString, string storedProcedure)
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

        public async Task<JsonResult> GetNypEmployeeComplianaceLogs(string sqlConnectionString, string storedProcedure, EmployeeComplianceLogsModel employeeCompliance)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.CommandTimeout = 180;

                        if (!(string.IsNullOrWhiteSpace(employeeCompliance.LastName)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEmployeeLastLastName, employeeCompliance.LastName.Trim()));

                        }
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEmployeeLastLastName, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(employeeCompliance.FirstName)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEmployeeFirstName, employeeCompliance.FirstName.Trim()));
                        }
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEmployeeFirstName, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(employeeCompliance.EmpIdNumber)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEmployeeID, employeeCompliance.EmpIdNumber.Trim()));

                        }
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEmployeeID, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(employeeCompliance.EmpDepartment)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEmployeeDepartment, employeeCompliance.EmpDepartment.Trim()));

                        }
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEmployeeDepartment, DBNull.Value));

                        if (!(string.IsNullOrWhiteSpace(employeeCompliance.EmpJobTitle)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEmployeeJobTitle, employeeCompliance.EmpJobTitle.Trim()));

                        }
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEmployeeJobTitle, DBNull.Value));


                        if (!(string.IsNullOrWhiteSpace(employeeCompliance.EmpDocumentType)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEmployeeDocType, employeeCompliance.EmpDocumentType.Trim()));

                        }
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEmployeeDocType, DBNull.Value));





                        if (!(string.IsNullOrWhiteSpace(employeeCompliance.ScanOperator)))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, employeeCompliance.ScanOperator.Trim()));

                        }
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaScanOperator, DBNull.Value));

                        if (employeeCompliance.ScanDate.Year > 2000)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, employeeCompliance.ScanDate));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, employeeCompliance.DateUpload));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaStartReceiptDate, DBNull.Value));
                            sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaEndReceiptDate, DBNull.Value));
                        }

                        sqlCmd.Parameters.Add(new SqlParameter(ConstNypLabReqs.SpParmaSearchPartial, employeeCompliance.SearchPartial));
                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);
                        //await sqlConnection.OpenAsync();
                        //SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();
                        //var dt = new DataTable();
                        //dt.BeginLoadData();
                        //dt.Load(dr);
                        //dt.EndLoadData();
                        //dt.AcceptChanges();
                        //JsonResult jsonResult = new JsonResult(dt);

                        //return jsonResult;
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
