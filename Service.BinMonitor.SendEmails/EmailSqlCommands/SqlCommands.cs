using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Edocs.Service.BinMonitor.SendEmails;
using Edocs.Service.BinMonitor.SendEmails.Models;


namespace Edocs.Service.BinMonitor.SendEmails.EmailSqlCommands
{
    public class SqlCommands
    {
        public async Task<SqlConnection> SqlConnection(SqlServerModel sqlServerModel)
        {

            SqlConnectionStringBuilder cb = new SqlConnectionStringBuilder();
            cb.DataSource = sqlServerModel.SqlServerName;
            cb.UserID = sqlServerModel.SqlDBUserName;
            cb.Password = sqlServerModel.SqlDBPassWord;
            cb.InitialCatalog = sqlServerModel.SqlDBName;
            cb.MultipleActiveResultSets = true;
            cb.ConnectTimeout = 120;
            cb.TrustServerCertificate = false;
            cb.PersistSecurityInfo = false;
            SqlConnection connection = new SqlConnection(cb.ConnectionString);
            connection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            return connection;



        }
        public async Task<SqlDataReader> SqlDataReader(string spName, SqlConnection sqlConnection)
        {
            using (SqlCommand cmd = new SqlCommand(spName, sqlConnection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                return (cmd.ExecuteReaderAsync().ConfigureAwait(true).GetAwaiter().GetResult());


            }
        }

        public async Task<SqlDataReader> SqlDataReader(string spName, SqlConnection sqlConnection,Dictionary<string,string> keyValuePairs)
        {
            using (SqlCommand cmd = new SqlCommand(spName, sqlConnection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                foreach(KeyValuePair<string,string> pair in keyValuePairs)
                {
                    cmd.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
                   
                }
                return (cmd.ExecuteReaderAsync().ConfigureAwait(true).GetAwaiter().GetResult());


            }
        }


        //public async Task<JsonResult> GetJsonObject(string spName, SqlConnection sqlConnection) where T : class, new()
        //{
        //    using (SqlCommand cmd = new SqlCommand(spName, sqlConnection))
        //    {
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        SqlDataReader dr = cmd.ExecuteReaderAsync().ConfigureAwait(true).GetAwaiter().GetResult();
        //        if (dr.HasRows)
        //        {
        //            dr.Read();
        //            return JsonConvert.DeserializeObject<T>(dr.);
        //        }
        //        return null;
        //    }
        //}

        public async Task<IList<T>> SqlDataReader<T>(string spName, SqlConnection sqlConnection)
        {

            //IDictionary<string, EmailCategoriesModel> valuePairs = new Dictionary<string, EmailCategoriesModel>();
            //IDictionary<string, EmailCategoriesModel> valuePairs = null;
            IList <T> valuePairs = null;
            using (SqlCommand cmd = new SqlCommand(spName, sqlConnection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReaderAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                if (dr.HasRows)
                {
                    var dt = new DataTable();
                    dt.BeginLoadData();
                    dt.Load(dr);
                    dt.EndLoadData();
                    dt.AcceptChanges();
                    string serCat = JsonConvert.SerializeObject(dt);
                    valuePairs = JsonConvert.DeserializeObject<List<T>>(serCat);

                }
                return valuePairs;
            }
        }

        public async Task<T> SqlDataReader<T>(SqlConnection sqlConnection, string spName) where T : class,new()

        {

            //IDictionary<string, EmailCategoriesModel> valuePairs = new Dictionary<string, EmailCategoriesModel>();
            //IDictionary<string, EmailCategoriesModel> valuePairs = null;
            
            using (SqlCommand cmd = new SqlCommand(spName, sqlConnection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReaderAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                if (dr.HasRows)
                {
                    var dt = new DataTable();
                    dt.BeginLoadData();
                    dt.Load(dr);
                    dt.EndLoadData();
                    dt.AcceptChanges();
                    string serCat = JsonConvert.SerializeObject(dt);
                    return JsonConvert.DeserializeObject<T>(serCat);

                }
                return null;
            }
        }
    }
}
