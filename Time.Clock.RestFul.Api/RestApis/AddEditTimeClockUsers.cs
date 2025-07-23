using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Edocs.Employees.Time.Clock.App.Service.Models;
using Edocs.Employee.Time.Clock.RestFul.Api.Constants;
using System.Net;
using System.Net.NetworkInformation;
using System.Xml.Linq;

namespace Edocs.Employee.Time.Clock.RestFul.Api.RestApis
{
    public class AddEditTimeClockUsers
    {
        private static AddEditTimeClockUsers instance = null;

        private AddEditTimeClockUsers() { }

        public static AddEditTimeClockUsers TimeClockUsersInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AddEditTimeClockUsers();
                }
                return instance;
            }
        }
        public async Task<JsonResult> AddTimeClockUser(EmpModel emp, string sqlConnectionStr, string storedProcedueName)
        {
            try
            {
                if (emp.EmpTerminationDate.Year < 2020)
                    emp.EmpTerminationDate = Convert.ToDateTime("01/01/2020");
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpID, emp.EmpID));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpFirstName, emp.EmpFirstName));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpLastName, emp.EmpLastName));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpAddress, emp.EmpAddress));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpCity, emp.EmpCity));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpState, emp.EmpState));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpEmailAddress, emp.EmpEmailAddress));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpCellPhone, emp.EmpCellPhone));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpHomePhone, emp.EmpHomePhone));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpPayRate, emp.EmpPayRate));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpHolidayPayRate, emp.EmpHolidayPayRate));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaFedTaxesPercent, emp.FedTaxesPercent));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaStateTaxPercent, emp.StateTaxPercent));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpStartDate, emp.EmpStartDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpTerminationDate, emp.EmpTerminationDate));
                        if(!(string.IsNullOrWhiteSpace(emp.Comments)))
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaComments, emp.Comments));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaComments,DBNull.Value));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpActive, emp.EmpActive));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpSiteAdmin, emp.EdocsAdmin));
                        
                        return TimeClockJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding/editing edocs employee {emp.EmpID} {ex.Message}");
            }
        }
        public async Task<JsonResult> DelTimeClockEntry(int ID, string sqlConnectionStr, string storedProcedueName)
        {
            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaID, ID));


                        return TimeClockJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: Deleting timce clock entry for record id:{ID} {ex.Message}");

            }
        }
    }
}
