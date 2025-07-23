using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;
namespace SqlCommands
{
    public class SqlCmd : SqlConstants
    {



        public SqlCmd()
        {
             
            cloudDbServerName = GetApplicationSetting("SqlServerName");
            cloudDbName = GetApplicationSetting("DataBaseName");
            cloudDbUserName = GetApplicationSetting("DbUserName");
            cloudDbpassword = "6746edocs@";
        }
        public SqlCmd(string serverName,string dbName,string dbUserName,string dbPassWord)
        {
            cloudDbServerName = serverName;
            cloudDbName = dbName;
            cloudDbUserName = dbUserName;
            if(string.IsNullOrEmpty(dbPassWord))
                cloudDbpassword = "6746edocs@";
        }

        public SqlConnection SqlConnectionCloud()
        {
            SqlConnectionStringBuilder cb = new SqlConnectionStringBuilder();
            cb.DataSource = GetApplicationSetting("CloudSqlServerName");
            cb.UserID = cloudDbUserName;
            cb.Password = cloudDbpassword;
            cb.InitialCatalog = cloudDbName;
            cb.MultipleActiveResultSets = true;
            cb.ConnectTimeout = 60;
            cb.TrustServerCertificate = false;
            cb.PersistSecurityInfo = false;
            SqlConnection connection = new SqlConnection(cb.ConnectionString);
            UpdateApplicationSetting("SqlServerName", cb.DataSource);
            connection.Open();
         
            return connection;
        }
        public SqlConnection SqlConnection()
        {
            SqlConnection connection = null;
            try
            { 
            SqlConnectionStringBuilder cb = new SqlConnectionStringBuilder();
            cb.DataSource = cloudDbServerName;
            cb.UserID = cloudDbUserName;
            cb.Password = cloudDbpassword;
            cb.InitialCatalog = cloudDbName;
            cb.MultipleActiveResultSets = true;
            cb.ConnectTimeout = 60;
            cb.TrustServerCertificate = false;
            cb.PersistSecurityInfo = false;
                 connection = new SqlConnection(cb.ConnectionString);
            connection.Open();
            }
            catch
            {
                connection = SqlConnectionCloud();
            }
            return connection;
        }

        public SqlDataReader SqlDataReader(string spName, Dictionary<string, string> sqlParams, SqlConnection sqlConnection, Guid binId)
        {
            using (SqlCommand cmd = new SqlCommand(spName, sqlConnection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (sqlParams != null)
                {

                    if (binId != Guid.Empty)
                        cmd.Parameters.Add(new SqlParameter(SpParmaBatchId, binId));
                    foreach (KeyValuePair<string, string> keyValuePair in sqlParams)
                    {

                        cmd.Parameters.Add(new SqlParameter(keyValuePair.Key, keyValuePair.Value));
                    }
                }

                SqlDataReader dr = cmd.ExecuteReader();
                return dr;
            }

        }

        public DataSet GetEmailAddress(string spName, SqlConnection sqlConnection)
        {
            DataTable dt = new DataTable("EmailAddress");
            DataSet set = new DataSet("test");
            using (SqlCommand cmd = new SqlCommand(spName, sqlConnection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                DataColumn lName = new DataColumn("Email", typeof(string));
                dt.Columns.Add(lName);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    DataRow drNew = dt.NewRow();
                    drNew["Email"] = dr[0].ToString();
                    dt.Rows.Add(drNew);
                }
            }
            dt.AcceptChanges();
            set.Tables.Add(dt);
            return set;
        }

        public SqlDataReader SqlDataReader(string spName, Dictionary<string, string> sqlParams, SqlConnection sqlConnection)
        {
            using (SqlCommand cmd = new SqlCommand(spName, sqlConnection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (sqlParams != null)
                {
                    foreach (KeyValuePair<string, string> keyValuePair in sqlParams)
                    {

                        cmd.Parameters.Add(new SqlParameter(keyValuePair.Key, keyValuePair.Value));
                    }
                }

                SqlDataReader dr = cmd.ExecuteReader();
                return dr;
            }

        }


        public SqlDataReader SqlDataReader(string spName, SqlConnection sqlConnection)
        {
            using (SqlCommand cmd = new SqlCommand(spName, sqlConnection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();
                return dr;
            }

        }
        public DataTable GetCategories(SqlConnection sqlConnection)
        {
            DataTable retDt = null;
            using (SqlDataAdapter da = new SqlDataAdapter(SpGetCatgories, sqlConnection))
            {
                da.SelectCommand.CommandTimeout = 120;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                da.Fill(ds);
            }
            return retDt;
        }
        private string GetApplicationSetting(string key)
        {
            string retConfig = string.Empty;
            //ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ExeConfigurationFileMap exeConfigurationFileMap = new ExeConfigurationFileMap();
            exeConfigurationFileMap.ExeConfigFilename = string.Format("{0}\\SqlCommands.Common.dll.config",System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
           Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);
            if (configuration.HasFile)
            { 
               KeyValueConfigurationCollection keyValueConfigurationCollection  =configuration.AppSettings.Settings;// figurationManager.AppSettings.Get(key).ToString();
                foreach(KeyValueConfigurationElement configurationElement in keyValueConfigurationCollection)
                {
                    if(configurationElement.Key.ToLower() == key.ToLower())
                    {
                        retConfig = configurationElement.Value;
                        break;
                            }
                }
            }
            return retConfig;
        }
        private void UpdateApplicationSetting(string key,string newValue)
        {
            string retConfig = string.Empty;
            //ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ExeConfigurationFileMap exeConfigurationFileMap = new ExeConfigurationFileMap();
            exeConfigurationFileMap.ExeConfigFilename = string.Format("{0}\\SqlCommands.Common.dll.config", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);
            if (configuration.HasFile)
            {
                KeyValueConfigurationCollection keyValueConfigurationCollection = configuration.AppSettings.Settings;// figurationManager.AppSettings.Get(key).ToString();
                foreach (KeyValueConfigurationElement configurationElement in keyValueConfigurationCollection)
                {
                    if (configurationElement.Key.ToLower() == key.ToLower())
                    {
                        configurationElement.Value = newValue;
                        configuration.Save(ConfigurationSaveMode.Modified);
                        break;
                    }
                }
            }
            
        }
    }

}
