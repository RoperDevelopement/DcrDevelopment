using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using edl = EdocsUSA.Utilities.Logging;
using NYPMigration.Utilities;

namespace NYPMigration.SQLCmds
{
  public  class GetNYPRecords
    {
        private static GetNYPRecords instance = null;
        public static GetNYPRecords NYPRecordsInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GetNYPRecords();
                }
                return instance;
            }
        }
        private GetNYPRecords()
        { }
        public async Task<string> GetJsonResults(string sqlConnectionString, string storedProcedure)
        {
            try
            {
               // ChangeSpGetLabReqs(PropertiesConst.PropertiesConstInstance.SQLServer, 1).ConfigureAwait(false).GetAwaiter().GetResult();
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 180;
                        sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult(); ;
                        SqlDataReader dr =  sqlCmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                         
                        var dt = new DataTable();
                        dt.BeginLoadData();
                        dt.Load(dr);
                        dt.EndLoadData();
                        dt.AcceptChanges();
                        string jsonResult = JsonConvert.SerializeObject(dt);
                        sqlConnection.Close();
                        
                        return jsonResult;
                    }

                }
            }
            catch (Exception ex)
            {
               
                edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Error getting NYP Records for stored procedure {storedProcedure} {ex.Message}");
                PropertiesConst.PropertiesConstInstance.MigrationErrors.Add($"Error getting NYP Records for stored procedure {storedProcedure} {ex.Message}");
                PropertiesConst.PropertiesConstInstance.TotalErrors++;
                // throw new Exception(ex.Message);
            }
            return string.Empty;
        }
        public async Task ChangeSpGetLabReqs(string sqlConnectionString, int spRunning)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    string cmdText = $"UPDATE [dbo].[SPLabReqsRunning] SET[GetLabReqsRunning] = {spRunning}";
                    using (SqlCommand sqlCmd = new SqlCommand(cmdText,sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.CommandTimeout = 180;
                        sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult(); ;
                        SqlDataReader dr = sqlCmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        
                    }

                }
            }
            catch (Exception ex)
            {
                PropertiesConst.PropertiesConstInstance.UpdateErrors($"{ex.Message}");
                throw new Exception(ex.Message);
            }

        }
        public async Task<bool> ChecKSpGetLabReqsRunning(string sqlConnectionString)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    string cmdText = $"Select *  from [dbo].[SPLabReqsRunning]";
                    using (SqlCommand sqlCmd = new SqlCommand(cmdText, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.CommandTimeout = 180;
                        sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult(); ;
                        SqlDataReader dr = sqlCmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                string procName = dr[0].ToString().ToLower();
                                if (string.Compare(procName, "1") == 0)
                                    return true;
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
             //   edl.TraceLogger.TraceLoggerInstance.TraceError($"Error getting NYP Records for stored procedure {storedProcedure} {ex.Message}");
               // edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Error getting NYP Records for stored procedure {storedProcedure} {ex.Message}");
                throw new Exception(ex.Message);
            }
            return false;
        }
        public async Task<bool> CheckMigLabReqsRunning(string sqlConnectionString)
        {
            try
            {
                 
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(PropertiesConst.PropertiesConstInstance.SPCheckSPNypMerRunning, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 180;
                        sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        //sqlCmd.CommandText = PropertiesConst.PropertiesConstInstance.CheckSPGetLabReqsRonning;
                        SqlDataReader dr = sqlCmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                string procName = dr[0].ToString().ToLower();
                                if (string.Compare(procName, PropertiesConst.PropertiesConstInstance.SPGetMigrationLabReqsScanDate, true) == 0)
                                    return true;
                            }

                        }

                    }

                }
            }
            catch (Exception ex)
            {
               // edl.TraceLogger.TraceLoggerInstance.TraceError($"Error getting NYP Records for stored procedure {storedProcedure} {ex.Message}");
              //  edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Error getting NYP Records for stored procedure {storedProcedure} {ex.Message}");
                throw new Exception(ex.Message);
            }
            return false;
        }
        public async Task UpDateNYPLaReqsMigrated(string sqlConnectionString, string storedProcedure,int totrecords,string scandate)
        {
            try
            {
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Udating LabReqs for sp {storedProcedure} for scan date {scandate}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Udating LabReqs for sp {storedProcedure} for scan date {scandate}");
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {

                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 180;
                        sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        
                        sqlCmd.Parameters.Add(new SqlParameter(Utilities.PropertiesConst.PropertiesConstInstance.ParmaScanDate,scandate));
                        sqlCmd.Parameters.Add(new SqlParameter(Utilities.PropertiesConst.PropertiesConstInstance.ParmaTotalProcessed,totrecords));
                        SqlDataReader dr =   sqlCmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        
                    }

                }
            }
            catch (Exception ex)
            {


                PropertiesConst.PropertiesConstInstance.UpdateErrors($"Error updating NYP Records for stored procedure {storedProcedure} for scan date {scandate} {ex.Message}");
                
                //throw new Exception(ex.Message);
            }

        }

    }
}
