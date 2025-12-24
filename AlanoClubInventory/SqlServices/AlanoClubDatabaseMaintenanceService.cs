using AlanoClubInventory.Models;
using AlanoClubInventory.Utilites;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using RtfPipe.Tokens;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace AlanoClubInventory.SqlServices
{
    public class AlanoClubDatabaseMaintenanceService: IDisposable
    {
        public event EventHandler<double> UpdateProgessBar;
        public event EventHandler<bool> UpdateProcessDone;
        public event EventHandler<bool> ShowHideProgessBar;
        public event EventHandler<bool> IsProcessRunning;
        public event EventHandler<string> Message;

        public AlanoClubDatabaseMaintenanceService()
        {
            SendMessage("Connecting to DataBase");
            DataBaseName = $"{Utilites.AlanoCLubConstProp.AlanoClubDBName}_{DateTime.Now.ToString("yyyy")}";
            InitializeSqlConnectionStr();

        }
        private string DataBaseName { get; set; }
        private int ProgressCount { get; set; }
        public int ReorganizeThreshold { get; set; } = 5;   // % fragmentation
        public int RebuildThreshold { get; set; } = 30;     // % fragmentation
        private string SqlConnBackupDataBase { get; set; }

        private string SqlConnectionStr { get; set; }
        private async void InitializeSqlConnectionStr()
        {
            var sqlConnModel = Utilites.ALanoClubUtilites.GetSqlConnectionStrings();
            SqlConnectionStr = sqlConnModel.Item1;
            SqlConnBackupDataBase = sqlConnModel.Item2.Replace(Utilites.AlanoCLubConstProp.SqlConnBackupDataBase, $"{DataBaseName}");
        }

        public async Task<string> GetDatabaseFolder()
        {
            OnIsRunning(true);
            SendMessage("Getting Database Folder");
            using (SqlConnection sqlConnection = new SqlConnection(SqlConnectionStr))
            {
                await sqlConnection.OpenAsync();
                string query = @"SELECT physical_name FROM sys.database_files WHERE type_desc = 'ROWS';";
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlConnection))
                {

                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.CommandType = System.Data.CommandType.Text;
                    SendMessage("Running Query..");
                    using (var reader = await sqlCmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // You can log the progress here if needed
                            var folderPath = reader[0].ToString();
                            folderPath = System.IO.Path.GetDirectoryName(folderPath);
                            OnIsRunning(false);
                            SendMessage($"Got Folder Path {folderPath}");
                            return folderPath ?? string.Empty;
                        }
                    }
                    // var result = await sqlCmd.ExecuteScalarAsync();
                    //if (result != null)
                    // {
                    //   var filePath = result.ToString();
                    //  var folderPath = System.IO.Path.GetDirectoryName(filePath);
                    /// return folderPath ?? string.Empty;
                    //}
                    // else
                    //{
                    //  throw new Exception("Could not retrieve database file path.");
                    // }
                }
            }
            throw new Exception("Could not retrieve database file path.");

        }
        private async void DeleteOldDatabase(string databaseName)
        {
            var databaseFolder = await GetDatabaseFolder();
            databaseFolder = databaseFolder.ToLower().Replace("data", "ACDBBackups");
            Utilites.ALanoClubUtilites.CreateFolder(databaseFolder);
            databaseFolder = System.IO.Path.Combine(databaseFolder, databaseName);
            Utilites.ALanoClubUtilites.DeleteFile(databaseFolder);
        }
        public async Task BackupDataBase(string dbBackupName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(dbBackupName))
                {
                    dbBackupName = $"{Utilites.AlanoCLubConstProp.ACDataBaseBackupDbName}.bak";
                }

                SendMessage("Start Backing Up DataBase..");

                OnIsRunning(true);
                var databaseFolder = await GetDatabaseFolder();
                databaseFolder = databaseFolder.ToLower().Replace("data", "ACDBBackups");
                Utilites.ALanoClubUtilites.CreateFolder(databaseFolder);
                databaseFolder = System.IO.Path.Combine(databaseFolder, dbBackupName);
                Utilites.ALanoClubUtilites.DeleteFile(databaseFolder);
                using (SqlConnection sqlConnection = new SqlConnection(SqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    var cmdText = $"BACKUP DATABASE [{Utilites.AlanoCLubConstProp.AlanoClubDBName}] TO DISK = '{databaseFolder}' WITH INIT, SKIP, NOREWIND, NOUNLOAD, STATS = 10";
                    using (SqlCommand sqlCmd = new SqlCommand(cmdText, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = System.Data.CommandType.Text;
                        SendMessage("Running Query to backup database DataBase..");
                        //  sqlCmd.CommandText = $"BACKUP DATABASE [{Utilites.AlanoCLubConstProp.AlanoClubDBName}] TO DISK = '{databaseFolder}' WITH INIT, SKIP, NOREWIND, NOUNLOAD, STATS = 10";
                        using (var reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                // You can log the progress here if needed
                                var progressMessage = reader[0].ToString();
                                SendMessage($"Backing up Database message {progressMessage}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnIsRunning(false);
                SendMessage($"Error BackupDataBase {Utilites.AlanoCLubConstProp.ACDataBaseBackupFolderName} {ex.Message}");
                throw new Exception($"Error BackupDataBase {Utilites.AlanoCLubConstProp.ACDataBaseBackupFolderName} {ex.Message}");
            }
            SendMessage("Done Backing up Database");
            OnIsRunning(false);
        }
        private async Task<string> GetCreateDatabaseScript()
        {
            SendMessage("Getting create database script");
            OnIsRunning(true);
            var creteDatabaseScriptFile = System.IO.Path.Combine(Utilites.ALanoClubUtilites.ApplicationWorkingFolder, Utilites.AlanoCLubConstProp.CreateDbScrip);

            var script = await System.IO.File.ReadAllTextAsync(creteDatabaseScriptFile);
            script = script.Replace(Utilites.AlanoCLubConstProp.AlanoClubDBName, $"{DataBaseName}");
            script = script.Replace(Utilites.AlanoCLubConstProp.AlanoClubLogDBName, $"{DataBaseName}");
            script = script.Replace(Utilites.AlanoCLubConstProp.ACDataTaleNames, $"[{DataBaseName}]");
            script = script.Replace(Utilites.AlanoCLubConstProp.GO, "");
            script = script.Replace(Utilites.AlanoCLubConstProp.DBStorageFormat, $"'{DataBaseName}'");
            OnIsRunning(false);
            return script;

        }
        private async Task<string> GetCreateTableScript()
        {
            SendMessage("Getting create table script");
            OnIsRunning(true);
            var creteDatabaseScriptFile = System.IO.Path.Combine(Utilites.ALanoClubUtilites.ApplicationWorkingFolder, Utilites.AlanoCLubConstProp.CreateTablesScript);

            var script = await System.IO.File.ReadAllTextAsync(creteDatabaseScriptFile);
            script = script.Replace(Utilites.AlanoCLubConstProp.GO, "");
            script = script.Replace(Utilites.AlanoCLubConstProp.ACDataTaleNames, $"[{DataBaseName}]");
            OnIsRunning(false);

            return script;

        }
        private async Task<string> GetCreateSPScript()
        {
            SendMessage("Getting create store procedures script");
            OnIsRunning(true);
            var creteDatabaseScriptFile = System.IO.Path.Combine(Utilites.ALanoClubUtilites.ApplicationWorkingFolder, Utilites.AlanoCLubConstProp.CreateStoredProcsScript);

            var script = await System.IO.File.ReadAllTextAsync(creteDatabaseScriptFile);
            script = script.Replace(Utilites.AlanoCLubConstProp.ACDataTaleNames, $"[{DataBaseName}]");
            //   script = script.Replace(Utilites.AlanoCLubConstProp.GO, "");

            // script = $"{script} {Utilites.AlanoCLubConstProp.CRLF} {Utilites.AlanoCLubConstProp.GO}";
            return script;

        }
        private async Task<string> GetCreateUsersPScript()
        {
            SendMessage("Getting create user script");
            OnIsRunning(true);
            var creteDatabaseScriptFile = System.IO.Path.Combine(Utilites.ALanoClubUtilites.ApplicationWorkingFolder, Utilites.AlanoCLubConstProp.CreateUsersScript);

            var script = await System.IO.File.ReadAllTextAsync(creteDatabaseScriptFile);
            script = script.Replace(Utilites.AlanoCLubConstProp.CreateAlanoClubUser, $"[{DataBaseName}]");
            script = script.Replace(Utilites.AlanoCLubConstProp.GO, "");
            OnIsRunning(false);
            return script;

        }
        public async Task CreateNewDatabase()
        {
            try
            {
                OnIsRunning(true);
                if (await CheckPurgeNotDone(DataBaseName))
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox($"Purge all ready done", "Purge Done", MessageBoxButton.OK, MessageBoxImage.Error);
                    SendMessage("Purge Done..."); OnIsRunning(false);
                    return;
                }
                await BackupDataBase($"{DataBaseName}.bak");
                await DropDataBase(DataBaseName);
                var createScript = await GetCreateDatabaseScript();
                SendMessage($"Getting createing new database {DataBaseName}");
                await CreateNewDatabase(createScript, SqlConnectionStr);
                SendMessage($"Getting createing users for new database {DataBaseName}");
                createScript = await GetCreateUsersPScript();
                await CreateNewDatabase(createScript, SqlConnBackupDataBase);
                SendMessage($"Getting createing table for new database {DataBaseName}");
                createScript = await GetCreateTableScript();
                await CreateNewDatabase(createScript, SqlConnBackupDataBase);
                await CreateNewStoredProcedures();
                // createScript = await GetCreateSPScript();
                // SendMessage($"Getting createing store procedures for new  database {DataBaseName}");
                // await CreateNewDatabase(createScript, SqlConnBackupDataBase);
                await AddDataToNewTables();
                SendMessage($"Adding database name {DataBaseName}...");
                await PurgeCurrentData(Utilites.AlanoCLubConstProp.AlanoClubDBName, DateTime.Now.Year.ToString());
                await AlClubSqlCommands.SqlCmdInstance.AddItemStoreProd(SqlConnectionStr, SqlConstProp.SPAddPurgeDataBaseName, SqlConstProp.SPPurgedDBName, DataBaseName);
                OnIsRunning(false);

            }
            catch (Exception ex)
            {
                await DropDataBase(DataBaseName);

                OnIsRunning(false);
                throw new Exception($"Error CreateNewDatabase {Utilites.AlanoCLubConstProp.AlanoClubDBName}_{DateTime.Now.ToString("yyyy")} {ex.Message}");
            }

            // SendMessage($"Deleting backup database {DataBaseName}");
            // DeleteOldDatabase($"{DataBaseName}.bak");
            SendMessage($"Done creating new database {DataBaseName}");
        }
        public async Task CreateNewDatabase(string createScript, string connSql)
        {
            try
            {

                OnIsRunning(true);
                using (SqlConnection sqlConnection = new SqlConnection(connSql))
                {
                    await sqlConnection.OpenAsync();
                    var cmdText = createScript;
                    using (SqlCommand sqlCmd = new SqlCommand(cmdText, sqlConnection))
                    {
                        if (sqlConnection.State != ConnectionState.Open)
                        {
                            throw new Exception("SQL Connection is not open.");
                        }
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.Text;


                        await sqlCmd.ExecuteNonQueryAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                OnIsRunning(false);
                throw new Exception($"Error CreateNewDatabase {DataBaseName} {ex.Message}");
            }

            OnIsRunning(false);


        }
        public async Task<string> CheckDatabaseIntegrity()
        {
            SendMessage("Checking database integrity");

            string retStr = "INTEGRITY_OK";
            OnIsRunning(true);
            try
            {

                using (SqlConnection connection = new SqlConnection(SqlConnectionStr))
                {
                    connection.Open();

                    string sql = $"DBCC CHECKDB('{Utilites.AlanoCLubConstProp.AlanoClubDBName}') WITH NO_INFOMSGS, ALL_ERRORMSGS";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                retStr += reader[0].ToString() + Environment.NewLine;


                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error CheckDatabaseIntegrity {ex.Message}");
            }
            OnIsRunning(false);

            SendMessage("Done checking database integrity");
            return retStr;
        }

        public async Task<int> CheckDatabaseOptimize(string sp)
        {

            SendMessage("Checking database Optimize");
            int tTimeSeconds = 0;
            OnIsRunning(true);
            var stopwatch = Stopwatch.StartNew();
            try
            {

                using (SqlConnection connection = new SqlConnection(SqlConnectionStr))
                {
                    await connection.OpenAsync();


                    using (SqlCommand command = new SqlCommand(sp, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        await command.ExecuteReaderAsync();

                    }

                    connection.Close();
                }
                stopwatch.Stop();
                OnIsRunning(false);
                SendMessage("Done checking database Optimize");
                return (int)stopwatch.Elapsed.TotalSeconds;
            }
            catch (Exception ex)
            {
                OnIsRunning(false);
                SendMessage($"Error CheckDatabaseOptimize {ex.Message}");
                throw new Exception($"Error CheckDatabaseOptimize {ex.Message}");
            }


        }
        public async Task RebuildIndexes()
        {
            SendMessage($"Rebuilding database indeex");
            OnIsRunning(true);
            try
            {
                using (var conn = new SqlConnection(SqlConnectionStr))
                {
                    await conn.OpenAsync();

                    using (var cmd = new SqlCommand(SqlConstProp.SPGetAlanoClubTableNames, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var tableNames = new List<string>();
                        while (await reader.ReadAsync())
                        {
                            tableNames.Add(reader["TableName"].ToString());
                        }
                        foreach (var tableName in tableNames)
                        {
                            await AnalyzeAndOptimizeAsync(tableName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnIsRunning(false);
                throw new Exception($"Error RebuildIndexes {ex.Message}");
            }
            SendMessage($" Done Rebuilding database indeex");
            OnIsRunning(false);
        }
        public async Task<IList<string>> GetAlanoClubTableNames()
        {
            IList<string> tableNames = new List<string>();
            SendMessage($"Getting Table Names");
            OnIsRunning(true);
            try
            {
                using (var conn = new SqlConnection(SqlConnectionStr))
                {
                    await conn.OpenAsync();

                    using (var cmd = new SqlCommand(SqlConstProp.SPGetAlanoClubTableNames, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            var tName = reader["TableName"].ToString();
                            SendMessage($"Adding table {tName}");
                           // if(!(string.IsNullOrWhiteSpace(alanoClubTables)) && (tName.ToLower().StartsWith(alanoClubTables)))
                             //   tableNames.Add(tName);
                           // else
                                tableNames.Add(tName);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                OnIsRunning(false);
                throw new Exception($"Error RebuildIndexes {ex.Message}");
            }
            SendMessage($" Done Rebuilding database indeex");
            OnIsRunning(false);
            return tableNames;
        }
        private async Task AnalyzeAndOptimizeAsync(string tableName)
        {
            SendMessage($"Analyze And Optimize Datbase");
            OnIsRunning(true);
            using (var conn = new SqlConnection(SqlConnectionStr))
            {
                await conn.OpenAsync();
                string statsQuery = $@"
                SELECT 
                    i.name AS IndexName,
                    ips.avg_fragmentation_in_percent AS FragPercent
                FROM sys.dm_db_index_physical_stats(DB_ID(), OBJECT_ID('{tableName}'), NULL, NULL, 'LIMITED') ips
                INNER JOIN sys.indexes i ON ips.object_id = i.object_id AND ips.index_id = i.index_id
                WHERE i.name IS NOT NULL";


                using (var cmd = new SqlCommand(statsQuery, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string indexName = reader["IndexName"].ToString();
                        double fragPercent = Convert.ToDouble(reader["FragPercent"]);

                        if (fragPercent >= RebuildThreshold)
                            await RebuildIndexAsync(conn, tableName, indexName);
                        else if (fragPercent >= ReorganizeThreshold)
                            await ReorganizeIndexAsync(conn, tableName, indexName);
                    }
                }
            }
            SendMessage($"Done Analyze And Optimize Datbase");
            OnIsRunning(false);
        }
        private async Task RebuildIndexAsync(SqlConnection conn, string tableName, string indexName)
        {
            SendMessage($"Start Rebuild Indexs");
            OnIsRunning(true);
            string sql = $"ALTER INDEX [{indexName}] ON [{tableName}] REBUILD";
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0; // allow long-running
                await cmd.ExecuteNonQueryAsync();
            }
            SendMessage($"Done Rebuild Indexs");
            OnIsRunning(false);
            //Console.WriteLine($"Rebuilt index {indexName} on {tableName}");
        }

        private async Task ReorganizeIndexAsync(SqlConnection conn, string tableName, string indexName)
        {
            SendMessage($"Start Reorganize Index");
            OnIsRunning(true);
            string sql = $"ALTER INDEX [{indexName}] ON [{tableName}] REORGANIZE";
            using (var cmd = new SqlCommand(sql, conn))
            {
                await cmd.ExecuteNonQueryAsync();
            }
            OnIsRunning(false);
            SendMessage($"Done Reorganize Index");
            //  Console.WriteLine($"Reorganized index {indexName} on {tableName}");
        }
        //public async Task<List<AlanoClubIndexes>> GetIndexesFragmentationAsync()
        //{
        //    var indexes = new List<AlanoClubIndexes>();
        //    try
        //    {
        //        using (var conn = new SqlConnection(SqlConnectionStr))
        //        {
        //            await conn.OpenAsync();
        //            string statsQuery = $@"
        //        SELECT 
        //            s.name AS SchemaName,
        //            t.name AS TableName,
        //            i.name AS IndexName,
        //            ips.avg_fragmentation_in_percent AS AvgFragmentationInPercent,
        //            ips.page_count AS IndexPageCount
        //        FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'LIMITED') ips
        //        INNER JOIN sys.indexes i ON ips.object_id = i.object_id AND ips.index_id = i.index_id
        //        INNER JOIN sys.tables t ON ips.object_id = t.object_id
        //        INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
        //        WHERE i.name IS NOT NULL";
        //            using (var cmd = new SqlCommand(statsQuery, conn))
        //            using (var reader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    indexes.Add(new AlanoClubIndexes
        //                    {
        //                        Schema = reader["SchemaName"].ToString(),
        //                        Table = reader["TableName"].ToString(),
        //                        Index = reader["IndexName"].ToString(),
        //                        AvgFragmentationInPercent = Convert.ToInt32(reader["AvgFragmentationInPercent"]),
        //                        IndexPageCount = Convert.ToInt32(reader["IndexPageCount"])
        //                    });
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error GetIndexesFragmentationAsync {ex.Message}");
        //    }
        //    return indexes;
        //}
        public async Task ClearCache()
        {
            SendMessage($"Start Clear database cache");
            OnIsRunning(true);
            try
            {
                using (SqlConnection connection = new SqlConnection(SqlConnectionStr))
                {
                    await connection.OpenAsync();
                    string sql = "DBCC FREEPROCCACHE; DBCC DROPCLEANBUFFERS;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                OnIsRunning(false);
                SendMessage($"Error ClearCache {ex.Message}");
                throw new Exception($"Error ClearCache {ex.Message}");
            }
            SendMessage($"Done Clear database cache");
            OnIsRunning(false);
        }


        public async Task<IList<ErrorLogEntry>> LoadLogs(string keyword = null, DateTime? startDate = null, DateTime? endDate = null)
        {

            // SendMessage($"Start Checking ErrorLogs");
            //Logs.Clear();
            double count = 1.00;
            double updateProgCount = 10;
            OnProcessDone(false);
            OnShowHideProgessBar(true);
            OnUpdateProgessBar(count++);
            IList<ErrorLogEntry> entry = new List<ErrorLogEntry>();
            using (var conn = new SqlConnection(SqlConnectionStr))
            {
                await conn.OpenAsync();

                // xp_readerrorlog returns LogDate, ProcessInfo, Text
                string sql = "EXEC xp_readerrorlog";

                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (count % 20 == 0)
                        {
                            if (updateProgCount < 1000)
                                OnUpdateProgessBar(updateProgCount);
                            updateProgCount += 10.00;
                        }
                        count++;

                        var logEntry = new ErrorLogEntry
                        {
                            LogDate = Convert.ToDateTime(reader["LogDate"]),
                            ProcessInfo = reader["ProcessInfo"] is DBNull ? string.Empty : reader["ProcessInfo"]?.ToString() ?? string.Empty,
                            Text = reader["Text"].ToString()
                        };
                        //entry.Add(new ErrorLogEntry
                        //{
                        //    LogDate = Convert.ToDateTime(reader["LogDate"]),
                        //    ProcessInfo = reader["ProcessInfo"] is DBNull ? string.Empty : reader["ProcessInfo"]?.ToString() ?? string.Empty,
                        //    Text = reader["Text"].ToString() 
                        //});
                        if (string.IsNullOrWhiteSpace(logEntry.Text))
                            continue;
                        if (startDate.HasValue && logEntry.LogDate.Date > startDate.Value.Date)
                            continue;
                        if (endDate.HasValue && logEntry.LogDate.Date > endDate.Value.Date)
                            continue;
                        // Apply filters
                        if ((!string.IsNullOrEmpty(keyword)))
                        {

                            //   bool anyExist = Utilites.ALanoClubUtilites.ContainsAnyValue(logEntry.Text.Replace(" ", ","), keyword.Split(' ', StringSplitOptions.RemoveEmptyEntries),",");
                            bool anyExist = Utilites.ALanoClubUtilites.ContainsAnyValue(logEntry.Text.Replace(" ", ","), keyword.Split(","));
                            if (!anyExist)
                                continue;
                        }
                        entry.Add(logEntry);
                        //    continue;
                        //if (startDate.HasValue && entry.LogDate < startDate.Value)
                        //    continue;
                        //if (endDate.HasValue && entry.LogDate > endDate.Value)
                        //    continue;


                    }
                }
            }
            // OnUpdateProgessBar(1000.00);
            //  await Task.Delay(2000);
            OnShowHideProgessBar(false);
            OnProcessDone(true);

            return entry;
            //    LastRefresh = $"Last refreshed: {DateTime.Now}";
            //  EntryCount = Logs.Count;
        }
        public async Task<IList<ErrorLogEntry>> LoadLogs()
        {
            //Logs.Clear();
            double count = 1.00;
            double updateProgCount = 10;
            OnProcessDone(false);
            OnShowHideProgessBar(true);
            OnUpdateProgessBar(count++);
            IList<ErrorLogEntry> entry = new List<ErrorLogEntry>();
            using (var conn = new SqlConnection(SqlConnectionStr))
            {
                await conn.OpenAsync();

                // xp_readerrorlog returns LogDate, ProcessInfo, Text
                string sql = "EXEC xp_readerrorlog";

                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {

                        if (count % 20 == 0)
                        {
                            if (updateProgCount < 1000)
                                OnUpdateProgessBar(updateProgCount);
                            updateProgCount += 10;
                        }
                        count++;

                        if (string.IsNullOrWhiteSpace(reader["Text"].ToString()))
                            continue;
                        entry.Add(new ErrorLogEntry
                        {
                            LogDate = Convert.ToDateTime(reader["LogDate"]),
                            ProcessInfo = reader["ProcessInfo"] is DBNull ? string.Empty : reader["ProcessInfo"]?.ToString() ?? string.Empty,
                            Text = reader["Text"].ToString()
                        });

                        // Apply filters
                        //if (!string.IsNullOrEmpty(keyword) && !entry.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        //    continue;
                        //if (startDate.HasValue && entry.LogDate < startDate.Value)
                        //    continue;
                        //if (endDate.HasValue && entry.LogDate > endDate.Value)
                        //    continue;


                    }
                }
            }
            //   OnUpdateProgessBar(1000.00);
            //    await Task.Delay(2000);
            OnShowHideProgessBar(false);
            OnProcessDone(true);

            return entry;
            //    LastRefresh = $"Last refreshed: {DateTime.Now}";
            //  EntryCount = Logs.Count;
        }
        private async Task AddDataToNewTables()
        {
            SendMessage("Adding data to new tables");
            IList<string> tableNames = await GetAlanoClubTableNames();
            var dbName = $"{DataBaseName}";
            if ((tableNames != null) && (tableNames.Count > 0))
            {
                foreach (string tableName in tableNames)
                {
                    try
                    {


                        SendMessage($"Updating data for table {tableName} for database {DataBaseName}");
                        var cmdText = string.Format(SqlServices.SqlConstProp.AlanoClubInsertNewTableQuery, dbName, tableName, tableName);
                        await CreateNewDatabase(cmdText, SqlConnectionStr);
                    }
                    catch (Exception ex)
                    {
                        SendMessage($"Error updating table {tableName} for database {DataBaseName} {ex.Message}");
                    }
                    //public const string AlanoClubInsertNewTableQuery = "INSERT INTO [{0}].[dbo].[{1}] select * from [AlanoClub].[dbo].[{2}]";
                }
            }
            SendMessage("Done data to new tables");

        }
        private async Task<IList<SqlStoreProceduresDefinitionModel>> GetStoreProceduresDefinition()
        {
            SendMessage($"Getting Store Procedures Definitions for DataBase {DataBaseName}");
            IList<SqlStoreProceduresDefinitionModel> definitionModels = new List<SqlStoreProceduresDefinitionModel>();
            using (SqlConnection conn = new SqlConnection(SqlConnectionStr))
            {
                conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(AlanoCLubConstProp.SqlQueryStoredProcedures, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                definitionModels.Add(new SqlStoreProceduresDefinitionModel { StoredProcedureName = reader.GetString(0), StoredProcedureDefinition = reader.GetString(1) });
                            }

                        }
                    }

                }

            }
            return definitionModels;
        }

        private async Task CreateNewStoredProcedures()
        {
            var storeProdDef = await GetStoreProceduresDefinition();

            if ((storeProdDef != null) && (storeProdDef.Count > 0))
            {
                foreach (var item in storeProdDef)
                {
                    try
                    {
                        SendMessage($"Creating stored procedure {item.StoredProcedureName} for database {DataBaseName}");
                        await CreateNewDatabase(item.StoredProcedureDefinition, SqlConnBackupDataBase);

                    }
                    catch (Exception ex)
                    {
                        SendMessage($"Error creating stored procedure {item.StoredProcedureName} for database {DataBaseName} {ex.Message}");
                    }
                }
            }
            SendMessage($"Done creating stored procedures for database {DataBaseName}");
        }
        private async Task DropDataBase(string dbName)
        {
            await KillConnectionDataBase(dbName);
            var connStr = SqlConnectionStr.Replace(Utilites.AlanoCLubConstProp.AlanoClubDBName, "master");
            SendMessage($"Droping database {dbName}");
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Always connect to master when dropping another database
                string sql = $@"
                IF EXISTS (SELECT name FROM sys.databases WHERE name = N'{dbName}')
                BEGIN
                    ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    DROP DATABASE [{dbName}];
                END";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                    SendMessage($"Droped database {dbName}");
                    //Console.WriteLine($"Database {dbName} dropped successfully (if it existed).");
                }
            }
        }
        private async Task KillConnectionDataBase(string dbName)
        {
            var connStr = SqlConnectionStr.Replace(Utilites.AlanoCLubConstProp.AlanoClubDBName, "master");
            SendMessage($"Droping Connection to database {dbName}");
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Step 1: Kill all connections
                string killSql = $@"
                DECLARE @sql NVARCHAR(MAX) = N'';
                SELECT @sql += 'KILL ' + CAST(session_id AS NVARCHAR(10)) + ';'
                FROM sys.dm_exec_sessions
                WHERE database_id = DB_ID('{dbName}');
                EXEC(@sql);";

                using (SqlCommand killCmd = new SqlCommand(killSql, conn))
                {
                    killCmd.ExecuteNonQuery();
                    SendMessage($"All connections to {dbName} killed.");
                }

            }
        }
        private async Task<bool> CheckPurgeNotDone(string dbName)
        {
            bool retExists = false;
            SendMessage($"Checking Database not purged {dbName}");
            using (SqlConnection conn = new SqlConnection(SqlConnectionStr))
            {
                conn.Open();

                // Step 1: Kill all connections
                string cmd = string.Format(SqlConstProp.AlanoCheckPurgeTable, dbName);

                using (SqlCommand sqlCmd = new SqlCommand(cmd, conn))
                {
                    sqlCmd.CommandType = CommandType.Text;
                    using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            SendMessage($"Database {dbName} all ready purged.");
                            retExists = true;
                        }

                        reader.Close();
                    }
                    sqlCmd.Dispose();
                }
                conn.Close();

            }
            return retExists;
        }
        private async Task PurgeCurrentData(string dbName, string year)
        {
            try
            {
                SendMessage($"Deleting old data in database {dbName}");
                await AlClubSqlCommands.SqlCmdInstance.AddItemStoreProd(SqlConnectionStr, SqlConstProp.SPPurgeAlacnoDataBaseData, SqlConstProp.SPParmaYear, year);
            }
            catch (Exception ex)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox("Error adding Data Need to Restore Current ALanoCLub Database..", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                GetRestoreErrorBakupDBFileName();
                throw new Exception($"Error deleting old data in {dbName} {ex.Message}");
            }



        }
        public async Task RestoreDataBase()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            var path = await GetDatabaseFolder();
            path = $"{path.ToLower().Replace("data", "ACDBBackups")}";
            //SaveDocumentFolder = await openFileDialog.SaveFile("PDF Files(*.pdf) | *.pdf|HTML Files (*.html)|*.html");
            var fileName = await fileDialog.OpenFile(path, $"Database Backup ({Utilites.AlanoCLubConstProp.AlanoClubDBName}*.bak)|{Utilites.AlanoCLubConstProp.AlanoClubDBName}*.bak|All files (*.*)|*.*");
            if (string.IsNullOrEmpty(fileName))
            {
                SendMessage("Cancel restore databse..");
                return;
            }
            var dbName = Path.GetFileNameWithoutExtension(fileName);
            if (string.Compare(dbName, Utilites.AlanoCLubConstProp.ACDataBaseBackupDbName, true) == 0)
            {
                int indexACDB = dbName.IndexOf("Backup");
                if (indexACDB == -1)
                {
                    return;
                }
                dbName = dbName.Substring(0, indexACDB);
            }
            var mess = ALanoClubUtilites.ShowMessageBoxResults($"Restore Database {Path.GetFileNameWithoutExtension(dbName)} Using Backup Database {fileName}", "Restore", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (mess.Result == MessageBoxResult.Yes)
            {
                await RestoreDataBase(dbName, fileName);

            }
            else
            {
                SendMessage($"Cancel Restoring {dbName} Database");
            }
            //   string dbName = Path.GetFileNameWithoutExtension(fileName);
            //   if (string.Compare(dbName, Utilites.AlanoCLubConstProp.ACDataBaseBackupDbName, true) == 0)
            //   {
            //       int indexACDB = dbName.IndexOf("Backup");
            //       if (indexACDB == -1)
            //       {
            //           return;
            //       }
            //       dbName = dbName.Substring(0, indexACDB);
            //   }
            //var mess= Utilites.ALanoClubUtilites.ShowMessageBoxResults($"Restore Database {Path.GetFileNameWithoutExtension(dbName)} Using Backup Database {fileName}","Restore",MessageBoxButton.YesNoCancel,MessageBoxImage.Warning);
            //     if(mess.Result == MessageBoxResult.Yes)
            //   {
            //       if (string.Compare(dbName, Utilites.AlanoCLubConstProp.AlanoClubDBName, true) == 0)
            //       {
            //           await BackupDataBase($"{Utilites.AlanoCLubConstProp.ACDataBaseBackupDbName}_{DateTime.Now.ToString("MM-dd-yyyy")}");
            //       }

            //           await KillConnectionDataBase(dbName);
            //  await DropDataBase(dbName);
            //   var connStr = SqlConnectionStr.Replace(Utilites.AlanoCLubConstProp.AlanoClubDBName, "master");
            //   var restoreSql = string.Format(SqlConstProp.RestoreDB, dbName, fileName);
            //   using (SqlConnection conn = new SqlConnection(connStr))
            //   {
            //       conn.Open();
            //       using (SqlCommand restoreCmd = new SqlCommand(restoreSql, conn))
            //       {
            //           SendMessage($"Restoring database {dbName} from {fileName}");
            //           await restoreCmd.ExecuteNonQueryAsync();
            //           // Console.WriteLine($"Database {dbName} restored successfully from {backupFile}.");
            //       }
            //   }
            //   }

        }

        public async void GetRestoreErrorBakupDBFileName()
        {
            //var dbBackupName = $"{Utilites.AlanoCLubConstProp.ACDataBaseBackupDbName}.bak";
            var dbBackupName = $"{DataBaseName}.bak";
            var databaseFolder = await GetDatabaseFolder();
            databaseFolder = databaseFolder.ToLower().Replace("data", "ACDBBackups");
            databaseFolder = System.IO.Path.Combine(databaseFolder, dbBackupName);
            await RestoreDataBase(AlanoCLubConstProp.AlanoClubDBName, databaseFolder);

        }
        public async Task RestoreDataBase(string dbName, string fileName)
        {

            var connStr = SqlConnectionStr.Replace(Utilites.AlanoCLubConstProp.AlanoClubDBName, "master");
            // SetSingleMutiUser(dbName, connStr, false);
            await KillConnectionDataBase(dbName);
            await DropDataBase(dbName);

            var restoreSql = string.Format(SqlConstProp.RestoreDB, dbName, fileName);
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand restoreCmd = new SqlCommand(restoreSql, conn))
                {
                    SendMessage($"Restoring database {dbName} from {fileName}");
                    await restoreCmd.ExecuteNonQueryAsync();
                    // Console.WriteLine($"Database {dbName} restored successfully from {backupFile}.");
                }
            }
            SetSingleMutiUser(dbName, connStr, true);

            SendMessage($"Done Restoring {dbName} Database");
        }
        private async void SetSingleMutiUser(string dbName, string sqlConn, bool muitlUser = true)
        {
            string cmdTxt = string.Format(SqlConstProp.SetSingleUser, dbName);
            if (muitlUser)
            {
                cmdTxt = string.Format(SqlConstProp.SetMultiUser, dbName);
            }
            using (SqlConnection connection = new SqlConnection(sqlConn))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(cmdTxt, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    await cmd.ExecuteNonQueryAsync();
                }

            }
        }

        // Public implementation of Dispose pattern
        public void Dispose()
        {
          //  Dispose(true);

            // Suppress finalization to avoid calling the finalizer if already disposed
            GC.SuppressFinalize(this);
        }
        // Protected virtual Dispose method to allow overriding in derived classes
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!disposed)
        //    {
        //        if (disposing)
        //        {
        //            // Dispose managed resources
        //            if (managedStream != null)
        //            {
        //                managedStream.Dispose();
        //                managedStream = null;
        //            }
        //        }

        //        // Free unmanaged resources
        //        if (unmanagedHandle != IntPtr.Zero)
        //        {
        //            // Example: release unmanaged handle
        //            unmanagedHandle = IntPtr.Zero;
        //        }

        //        disposed = true;
        //    }
        //}


        public async Task<int> GetMinYear()
        {
            
            int minYear = DateTime.Now.Year;
            
            try
            {

                using (SqlConnection connection = new SqlConnection(SqlConnectionStr))
                {
                    await connection.OpenAsync();

                  
                    using (SqlCommand command = new SqlCommand(SqlConstProp.QueryMinYear, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if(reader.HasRows)
                            {
                               await reader.ReadAsync();
                                if(!(reader.IsDBNull(0)))
                                    minYear = reader.GetInt32(0);
                            }
                            
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error GetMinYear {ex.Message}");
            }
                    return minYear;
        }


        protected virtual void OnUpdateProgessBar(double count) => UpdateProgessBar?.Invoke(this, count);
        protected virtual void OnProcessDone(bool done) => UpdateProcessDone?.Invoke(this, done);
        protected virtual void OnShowHideProgessBar(bool showProBar) => ShowHideProgessBar?.Invoke(this, showProBar);
        protected virtual void OnIsRunning(bool isRunning) => IsProcessRunning?.Invoke(this, isRunning);
        protected virtual void SendMessage(string message) => Message?.Invoke(this, message);




    }

}

//string sql = @"
//CREATE TABLE #ErrorLog (LogDate DATETIME, ProcessInfo NVARCHAR(50), Text NVARCHAR(MAX));
//INSERT INTO #ErrorLog EXEC xp_readerrorlog 0, 1;
//SELECT * FROM #ErrorLog WHERE LogDate >= @StartDate AND LogDate < @EndDate;";

//using (var cmd = new SqlCommand(sql, conn))
//{
//    cmd.Parameters.AddWithValue("@StartDate", new DateTime(2025, 12, 1));
//    cmd.Parameters.AddWithValue("@EndDate", new DateTime(2025, 12, 8));
//    using (var reader = cmd.ExecuteReader())
//    {
//        while (reader.Read())
//        {
//            Console.WriteLine($"{reader["LogDate"]} | {reader["ProcessInfo"]} | {reader["Text"]}");
//        }
//    }



