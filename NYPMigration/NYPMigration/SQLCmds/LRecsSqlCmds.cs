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
    public class LRecsSqlCmds
    {
        private static LRecsSqlCmds instance = null;
        public static LRecsSqlCmds LRecsSqlCmdsInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LRecsSqlCmds();
                }
                return instance;
            }
        }
        private LRecsSqlCmds()
        { }

        public async Task<bool> UpdateMigratedLabReqs(string sqlConnectionString, string sqlCmdTxt)
        {

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                   
                    using (SqlCommand sqlCmd = new SqlCommand(sqlCmdTxt, sqlConnection))
                    {
                        
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.CommandTimeout = 180;
                        sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        SqlDataReader dr = sqlCmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                edl.TraceLogger.TraceLoggerInstance.TraceError($"Error running sql cmd text {sqlCmdTxt} {ex.Message}");
                edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"Error running sql cmd text {sqlCmdTxt} {ex.Message}");
                PropertiesConst.PropertiesConstInstance.MigrationErrors.Add($"Error running sql cmd text {sqlCmdTxt} {ex.Message}");
                PropertiesConst.PropertiesConstInstance.TotalErrors++;
                // throw new Exception(ex.Message);
                return false;
            }

        }

        public async Task UpdateMigratedLabReqs(string sqlConnectionString, string lReqsTable, int lReqsID)
        {

            try
            {
                string sqlCmdText = $"Update {lReqsTable} set [Migrated]= 1 where ID={lReqsID}";
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Running sqlcmdtext {sqlCmdText}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Running sqlcmdtext {sqlCmdText}");
                for (int i = 0; i < PropertiesConst.PropertiesConstInstance.NumberTimesLoop; i++)
                {
                    if (UpdateMigratedLabReqs(sqlConnectionString, sqlCmdText).ConfigureAwait(false).GetAwaiter().GetResult())
                        return;
                    if (PropertiesConst.PropertiesConstInstance.TotalErrors > PropertiesConst.PropertiesConstInstance.MaxErrors)
                    {
                        throw new Exception($"Could not update table {lReqsTable} for id {lReqsID}");
                    }
                    System.Threading.Thread.Sleep(PropertiesConst.PropertiesConstInstance.ThreadSleepSecs);
                    }
                
             
                throw new Exception($"Could not update table {lReqsTable} for id {lReqsID}");
            }
            catch (Exception ex)
            {
                PropertiesConst.PropertiesConstInstance.MigrationErrors.Add($"Could not update table {lReqsTable} for id {lReqsID} {ex.Message}");
                edl.TraceLogger.TraceLoggerInstance.TraceError($"{ex.Message}");
                edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"{ex.Message}");
                //PropertiesConst.PropertiesConstInstance.TotalErrors++;
                throw new Exception(ex.Message);
            }

        }
        public async Task UpdateMigratedPatID(string sqlConnectionString, string lReqsTable, string patID)
        {

            try
            {
                string sqlCmdText = $"Update {lReqsTable} set [Migrated]= 1 where PatientID='{patID}'";
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Running sqlcmdtext {sqlCmdText}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Running sqlcmdtext {sqlCmdText}");
                for (int i = 0; i < PropertiesConst.PropertiesConstInstance.NumberTimesLoop; i++)
                {
                    if (UpdateMigratedLabReqs(sqlConnectionString, sqlCmdText).ConfigureAwait(false).GetAwaiter().GetResult())
                        return;
                    if (PropertiesConst.PropertiesConstInstance.TotalErrors > PropertiesConst.PropertiesConstInstance.MaxErrors)
                    {
                        throw new Exception($"Could not update table {lReqsTable} for id {patID}");
                    }
                    System.Threading.Thread.Sleep(PropertiesConst.PropertiesConstInstance.ThreadSleepSecs);
                }


                throw new Exception($"Could not update table {lReqsTable} for id {patID}");
            }
            catch (Exception ex)
            {
                PropertiesConst.PropertiesConstInstance.MigrationErrors.Add($"Could not update table {lReqsTable} for id {patID} {ex.Message}");
                edl.TraceLogger.TraceLoggerInstance.TraceError($"{ex.Message}");
                edl.TraceLogger.TraceLoggerInstance.TraceErrorConsole($"{ex.Message}");
                //PropertiesConst.PropertiesConstInstance.TotalErrors++;
                throw new Exception(ex.Message);
            }

        }
        public async Task UpdateRecordsProcessed(string sqlConnectionString, string tableName, int totalProcess,int totalErrors)
        {

            try
            {
            
                edl.TraceLogger.TraceLoggerInstance.TraceInformation($"Adding Total Records Process to table {tableName} processed {totalProcess} errors {totalErrors}");
                edl.TraceLogger.TraceLoggerInstance.TraceInformationConsole($"Adding Total Records Process to table {tableName} processed {totalProcess} errors {totalErrors}");
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult(); ;
                    using (SqlCommand sqlCmd = new SqlCommand(PropertiesConst.PropertiesConstInstance.SPMigrationLabRecsRecordsProcessed, sqlConnection))
                    {
                        
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.Parameters.Add(new SqlParameter(PropertiesConst.PropertiesConstInstance.ParmaTableName,tableName));
                        sqlCmd.Parameters.Add(new SqlParameter(PropertiesConst.PropertiesConstInstance.ParmaTotalProcessed,totalProcess));
                        sqlCmd.Parameters.Add(new SqlParameter(PropertiesConst.PropertiesConstInstance.ParmaTotalErrors, totalErrors));
                        SqlDataReader dr = sqlCmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                }
                }
            catch (Exception ex)
            {
                PropertiesConst.PropertiesConstInstance.MigrationErrors.Add($"Adding Total Records Process to table {tableName}  processed {totalProcess} errors {totalErrors} { ex.Message}");
            }

        }
    }
}
