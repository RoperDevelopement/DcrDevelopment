using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace AlanoClubInventory.SqlServices
{
    public class SqlServerCheckDBSize
    {
        //private static SqlServerCheckDBSize sqlServerCheckDBSizeInstance;
        //private static readonly object _lock = new object();
        //private SqlServerCheckDBSize() { }
        //public static AlClubSqlCommands SqlServerCheckDBSizeInstance
        //{
        //    get
        //    {
        //        // Double-checked locking for thread safety
        //        if (sqlServerCheckDBSizeInstance == null)
        //        {
        //            lock (_lock)
        //            {
        //                if (sqlServerCheckDBSizeInstance == null)
        //                {
        //                    sqlServerCheckDBSizeInstance = new AlClubSqlCommands();
        //                }
        //            }
        //        }
        //        return sqlServerCheckDBSizeInstance;
        //    }
        //}
        public SqlServerCheckDBSize() 
        {
           GetSqlConnectionStr();
        }
        private string SqlConnectionString { get; set; }
        private async void GetSqlConnectionStr()
        {
            SqlConnectionString = await Utilites.ALanoClubUtilites.GetConnectionStr();
           // return SqlConnectionString;
        }
        public async Task<double> CheckDBSizeAsync(string dbName)
        {
            
            using (SqlConnection conn = new SqlConnection(SqlConnectionString))
            {
                await conn.OpenAsync();

                string sql = @"
                SELECT SUM(size) * 8 * 1024 AS SizeInBytes
                FROM sys.master_files
                WHERE database_id = DB_ID(@dbName);";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@dbName", dbName);

                    object result = await cmd.ExecuteScalarAsync();
                    if (result != DBNull.Value)
                    {
                        long sizeInBytes = Convert.ToInt64(result);
                        double sizeInGB = sizeInBytes / (1024.0 * 1024.0 * 1024.0);

                        return sizeInGB;
                    }
                }
            }
            return 0.0;
        }
        public async Task CheckDAllocatedUsedSize()
        {
            
            using (SqlConnection conn = new SqlConnection(SqlConnectionString))
            {
                await conn.OpenAsync();

                // sp_spaceused returns multiple columns: database_name, database_size, unallocated space
                string sql = "EXEC sp_spaceused";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        string dbName = reader["database_name"].ToString();
                        string dbSize = reader["database_size"].ToString(); // e.g. "12345.00 MB"
                        string unallocated = reader["unallocated space"].ToString();

                        Console.WriteLine($"Database: {dbName}");
                        Console.WriteLine($"Allocated Size: {dbSize}");
                        Console.WriteLine($"Unallocated Space: {unallocated}");

                        // Convert to GB if needed
                        if (double.TryParse(dbSize.Split(' ')[0], out double sizeMb))
                        {
                            double sizeGb = sizeMb / 1024.0;
                            Console.WriteLine($"Allocated Size in GB: {sizeGb:F2} GB");
                        }
                    }
                }
            }


        }
        public async Task<double> GetDataBaseSizeInGB(string dbName)
        {
            double ret = 0.0;
            try
            {
                using (SqlConnection conn = new SqlConnection(SqlConnectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(SqlConstProp.QueryDataBaseSizeInGB, conn))
                    {
                        cmd.Parameters.Add("@DbName", SqlDbType.NVarChar).Value = dbName;
                        object results = await cmd.ExecuteScalarAsync();
                        if (results != DBNull.Value && results != null)
                        {
                            return double.Parse(results.ToString());
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return -1.0;
            }
                return ret;
        }
    }
}