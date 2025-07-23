using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Edocs.HelperUtilities;

namespace Edocs.Delete.Records
{
    class SqlCmds
    {
        private static SqlCmds instance = null;
        private SqlConnection SqlConn
        { get; set; }
        SqlCmds()
        {
        }


        public static SqlCmds SqlCmdsInstance
        {
            get
            {
                if (instance == null)
                    instance = new SqlCmds();
                return instance;
            }
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task<SqlConnection> OpenSqlConnection(ModelDataBaseInfo dataBaseInfo )
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {

            string connStr = GetConnectionString(dataBaseInfo);
         //   TL.TraceLogger.TraceLoggerInstance.TraceInformation($"Opening sql connection connection string for database name {dbName}");
            SqlConnection sqlConnection = new SqlConnection(connStr);
            sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            return sqlConnection;
        }
        private string GetConnectionString(ModelDataBaseInfo dataBaseInfo)
        {
            
            //return string.Format("Server={0};Database={1};Trusted_Connection=True; User={2};PassWord={3};Connect Timeout={4};MultipleActiveResultSets=True;", SqlSever, dbName, DbUserName, DbPasswod, 1000);
           // TL.TraceLogger.TraceLoggerInstance.TraceInformation(string.Format("Using Connectin Server={0};Database={1};Connect Timeout={2};MultipleActiveResultSets=true;", SqlSever, dbName, 180));
            return string.Format("Server={0};Database={1};User={2};PassWord={3};Connect Timeout={4};MultipleActiveResultSets=true;", dataBaseInfo.SqlServer,dataBaseInfo.DataBaseName, dataBaseInfo.DBUserName,dataBaseInfo.DBPassWord, dataBaseInfo.SqlServerTimeOut);
        }

        public async Task<IDictionary<int,ModelLabReqs>> GetLabReqsToDelete(ModelDataBaseInfo dataBaseInfo)
        {
            IDictionary<int, ModelLabReqs> pairs = new Dictionary<int, ModelLabReqs>();
            using (SqlConnection sqlConn = OpenSqlConnection(dataBaseInfo).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                using(SqlCommand cmd = new SqlCommand(dataBaseInfo.GetLabRecsStoredProcedure,sqlConn))
                {
                    cmd.CommandTimeout = dataBaseInfo.SqlServerTimeOut;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter(Constants.LabReqsSpParmaDataBaseName,dataBaseInfo.TableName.ToLower()));
                    cmd.Parameters.Add(new SqlParameter(Constants.SpParmaDateToDelete, dataBaseInfo.DateToDelete));
                    SqlDataReader dr = cmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    while(dr.Read())
                    {
                        ModelLabReqs modelLab = new ModelLabReqs();
                        modelLab.ID = Utilities.ParseInt(dr[Constants.LabReqsRetParaID].ToString());
                        modelLab.FileUrl = dr[Constants.LabReqsRetParaFileURl].ToString();
                        modelLab.ScanBatch = Guid.Parse(dr[Constants.LabReqsRetParaScanBatch].ToString());
                        modelLab.ScanDate = DateTime.Parse(dr[Constants.LabReqsRetParaScanDate].ToString());
                        pairs.Add(modelLab.ID, modelLab);


                    }
                }
            }
                return pairs;
        }
        public async Task DeleteOldRecords(ModelDataBaseInfo dataBaseInfo,string id)
        {
            
            using (SqlConnection sqlConn = OpenSqlConnection(dataBaseInfo).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                using (SqlCommand cmd = new SqlCommand(dataBaseInfo.DeleteStoredProcedure, sqlConn))
                {
                    cmd.CommandTimeout = dataBaseInfo.SqlServerTimeOut;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter(Constants.LabReqsParmaID,id));
                    cmd.Parameters.Add(new SqlParameter(Constants.LabReqsSpParmaDataBaseName, dataBaseInfo.TableName));
                    cmd.ExecuteNonQueryAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                }
            }
            
        }
    }
}
