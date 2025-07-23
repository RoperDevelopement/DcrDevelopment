using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Edocs.Employees.Time.Clock.App.Service.Models;
using Edocs.Employee.Time.Clock.RestFul.Api.Constants;
using System.Net;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Edocs.Employee.Time.Clock.RestFul.Api.RestApis
{
    public class TimeClock
    {
        private static TimeClock instance = null;

        private TimeClock() { }

        public static TimeClock ClockInOutInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TimeClock();
                }
                return instance;
            }
        }

        public async Task<JsonResult> ClockInOut(string empID, string sqlConnectionStr, string storedProcedueName, bool isAdmin)
        {
            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpID, empID));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaClockInAdmin, isAdmin));
                        return TimeClockJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding/editing edocs employee {empID} {ex.Message}");
            }


        }
        public async Task<JsonResult> EmpTimeClockLoginID(string sqlConnectionStr, string storedProcedueName)
        {
            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                         
                    
                        return TimeClockJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting emp login id's {ex.Message}");
            }


        }
        public async Task<JsonResult> TimeClockAdminLogIn(string empID, string empPW,string sqlConnectionStr, string storedProcedueName )
        {
            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaLoginName, empID));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaPassWord, empPW));
                        return TimeClockJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding/editing edocs employee {empID} {ex.Message}");
            }
        }
        public async Task<JsonResult> TimeClockWorkWeekReport(string empID, DateTime workWeekStartDate,DateTime workWeekEndDate, string sqlConnectionStr, string storedProcedueName)
        {
            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaLoginName, empID));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaTimeClockWorkWeekStartDate, workWeekStartDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaTimeClockWorkWeekEndDate, workWeekEndDate));
                       
                        return TimeClockJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding/editing edocs employee {empID} {ex.Message}");
            }
        }
        public async Task<JsonResult> TimeClockAddTimeEntry(TimeClockEntriesModel timeClockEntriesModel, string sqlConnectionStr, string storedProcedueName)
        {
            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaEmpID, timeClockEntriesModel.EmpID));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaClockInTime, timeClockEntriesModel.ClockInTime));
                        sqlCmd.Parameters.Add(new SqlParameter(EmpConsts.SpParmaClockOutTime, timeClockEntriesModel.ClockOutTime));
       
                        return TimeClockJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: adding new employee time card for emp id:{timeClockEntriesModel.EmpID} {ex.Message}");
                
            }
        }
       
    }
}
