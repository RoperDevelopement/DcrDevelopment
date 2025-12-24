using Microsoft.Data.SqlClient;
using System;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.SqlServices
{
    

        public class SqlLogMaintenanceService
        {
            // Events you can bind to in your ViewModel/UI
            public event EventHandler<string> LogsCleared;
            public event EventHandler<string> ErrorOccurred;
            public event EventHandler<string> StatusUpdated;
        public SqlLogMaintenanceService() 
        {
            InitializeSqlConnectionStr();
        }
        private async void InitializeSqlConnectionStr()
        {
            var sqlConnModel = Utilites.ALanoClubUtilites.GetSqlConnectionStrings();
            SqlConnectionStr = sqlConnModel.Item1;
            SqlConnBackupDataBase = sqlConnModel.Item2.Replace(Utilites.AlanoCLubConstProp.SqlConnBackupDataBase, $"{Utilites.AlanoCLubConstProp.AlanoClubDBName}_{DateTime.Now.ToString("yyyy")}");
        }
        private string SqlConnBackupDataBase { get; set; }

        private string SqlConnectionStr { get; set; }
        /// <summary>
        /// Clears (shrinks) the transaction log asynchronously.
        /// </summary>
        public async Task ClearLogsAsync(string databaseName)
            {
                try
                {
                    OnStatusUpdated($"Starting log clear for {databaseName}...");

                    using (SqlConnection conn = new SqlConnection(SqlConnectionStr))
                    {
                        await conn.OpenAsync();

                        string sql = $@"
                        ALTER DATABASE [{databaseName}] SET RECOVERY SIMPLE;
                        DBCC SHRINKFILE ({databaseName}_Log, 1);
                        ALTER DATABASE [{databaseName}] SET RECOVERY FULL;";

                        using (var cmd = new SqlCommand(sql, conn))
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }

                    OnLogsCleared($"Logs cleared successfully for {databaseName}");
                }
                catch (Exception ex)
                {
                    OnErrorOccurred($"Error clearing logs for {databaseName}: {ex.Message}");
                }
            }

            /// <summary>
            /// Shrinks a specific log file asynchronously.
            /// </summary>
            public async Task ShrinkLogAsync(string connectionString, string databaseName, string logFileName)
            {
                try
                {
                    OnStatusUpdated($"Shrinking log file {logFileName}...");

                    using (var conn = new SqlConnection(connectionString))
                    {
                        await conn.OpenAsync();

                        string sql = $"DBCC SHRINKFILE ({logFileName}, 1);";

                        using (var cmd = new SqlCommand(sql, conn))
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }

                    OnLogsCleared($"Log file {logFileName} shrunk for {databaseName}");
                }
                catch (Exception ex)
                {
                    OnErrorOccurred($"Error shrinking log file {logFileName}: {ex.Message}");
                }
            }

            // Helper methods to raise events
            protected virtual void OnLogsCleared(string message) => LogsCleared?.Invoke(this, message);
            protected virtual void OnErrorOccurred(string message) => ErrorOccurred?.Invoke(this, message);
            protected virtual void OnStatusUpdated(string message) => StatusUpdated?.Invoke(this, message);
        }
    }

 
