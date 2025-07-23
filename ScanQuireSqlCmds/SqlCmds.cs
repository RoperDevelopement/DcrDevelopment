using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using ScanQuireSqlCmds.Properties;

using EdocsUSA.Utilities;
namespace ScanQuireSqlCmds
{
    public class SqlCmds : ScanQuireUsers, IDisposable
    {
        public readonly string SqlSpGetUsersInformation = "[dbo].[GetUsersInformation]";
        public readonly string SqlSpDeleteUserLogin = "[dbo].[DeleteUserLogin]";
        public readonly string SqlSpResetUserPassword = "[dbo].[ResetUserPassword]";

        public readonly string SqlSpUpDateUserLogin = "[dbo].[UpDateUserLogin]";
        public readonly string SqlSpGetLoginIds = "[dbo].[GetLoginIds]";
        public readonly string SqlSpAddUserLogin = "[dbo].[AddUserLogin]";
        public readonly string SqlSpUserLogIn = "[dbo].[UserLogIn]";
        public readonly string SqlSpUpdateUserPassword = "[dbo].[UpdateUserPassword]";
        public readonly string SqlSpParamLoginName = "@LoginName";
        public readonly string SqlSpParamUserPassWord = "@UserPassWord";
        public readonly string SqlSpParamNumDaysChangePassWord = "@NumDaysChangePassWord";
        public readonly string SqlSpParamUserFirstName = "@UserFirstName";
        public readonly string SqlSpParamUserLastName = "@UserLastName";
        public readonly string SqlSpParamUserEmail = "@UserEmail";
        public readonly string SqlSpParamOldPassWord = "@UserOldPassWord";
        public readonly string SqlSpParamOldLoginName = "@OldLoginName";
        public readonly string IsAdminNo = "False";
        public readonly string IsAdminYes = "True";
        public readonly string SqlSpParamIsAdmin = "@admin";
        public readonly char Admin = '1';
        public readonly char IsNotAdmin = '0';

        public SqlConnection SqlConn
        { get; set; }

        public SqlCmds()
        {
            // SqlConn = OpenSqlConnection();
        }

        public SqlConnection OpenSqlConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(GetConnectionString());
            sqlConnection.Open();
            return sqlConnection;
        }
        private string GetConnectionString()
        {
            Edocs_Utilities.EdocsUtilitiesInstance.PasswordKey = Settings.Default.DbKey;
            string dbUserName = Edocs_Utilities.EdocsUtilitiesInstance.DecryptToString(Settings.Default.DbUserName, DataProtectionScope.LocalMachine);
            string dbPasswod = Edocs_Utilities.EdocsUtilitiesInstance.DecryptToString(Settings.Default.DbPassWord, DataProtectionScope.LocalMachine);
            //return string.Format("Server={0}; Database={1};Trusted_Connection = True; User Id={2};Integrated Security=true;Connect Timeout=120;", Settings.Default.SqlSever, Settings.Default.SqlDataBase, Settings.Default.DbUserName);
            return string.Format("Server={0}; Database={1};Trusted_Connection = True; User Id={2};PassWord={3};Connect Timeout={4};", Settings.Default.SqlSever, Settings.Default.SqlDataBase, dbUserName,dbPasswod,Settings.Default.SqlServerTimeOut);
            
        }

        public Dictionary<string, ScanQuireUsers> GetScanQuireUserInfo()
        {
            Dictionary<string, ScanQuireUsers> retDic = new Dictionary<string, ScanQuireUsers>();
            using (SqlDataReader dr = DataReader(SqlSpGetUsersInformation))
            {
                while (dr.Read())
                {
                    ScanQuireUsers scanQuireUsers = new ScanQuireUsers();
                    scanQuireUsers.LoginId = dr["loginId"].ToString();
                    scanQuireUsers.UserFName = dr["frstrName"].ToString();
                    scanQuireUsers.UserLName = dr["lastName"].ToString();
                    scanQuireUsers.UserEmailAddress = dr["userEmail"].ToString();
                    scanQuireUsers.PWLastChanged = dr["dateLastChangePW"].ToString();
                    scanQuireUsers.LastLogin = dr["dateLastLogin"].ToString();
                    scanQuireUsers.IsAdmin = dr["isadmin"].ToString();
                    retDic.Add(scanQuireUsers.LoginId, scanQuireUsers);

                }
            }
            return retDic;
        }
        public string ResetPassword(string userLoginId,string newPassWord,string oldPwaasord)
        {
            string retMessage = string.Empty;
            Dictionary<string, string> dicRestPW = new Dictionary<string, string>();
            dicRestPW.Add(SqlSpParamLoginName, userLoginId);
            dicRestPW.Add(SqlSpParamUserPassWord,newPassWord);
            if (string.IsNullOrWhiteSpace(oldPwaasord))
            {
                dicRestPW.Add(SqlSpParamOldPassWord, SqlSpParamOldPassWord);
            }
            else
            {
                dicRestPW.Add(SqlSpParamOldPassWord,oldPwaasord);
            }

            using (SqlDataReader dr = DataReader(SqlSpResetUserPassword, dicRestPW))
            {
                {
                    while (dr.Read())
                    {
                        retMessage += dr[0].ToString();
                    }
                }
            }
            return retMessage;
        }
        public string DeleteUser(string userLoginId)
        {
            string retMessage = string.Empty;
            using (SqlDataReader dr = DataReader(SqlSpDeleteUserLogin, new Dictionary<string, string>() { { SqlSpParamLoginName, userLoginId } }))
            {
                {
                while (dr.Read())
                {
                    retMessage += dr[0].ToString();
                }
            }
        }
            return retMessage;
    }
        public string UpdateUsers(Dictionary<string, string>  dicUsers)
        {
            string retMessage = string.Empty;
            try
            { 
            using (SqlDataReader dr = DataReader(SqlSpUpDateUserLogin, dicUsers))
            {
                while(dr.Read())
                {
                    retMessage += dr[0].ToString();
                }
            }
            }
            catch(Exception ex)
            {
                retMessage = $"Error updating user:{ex.Message}";
            }
            return retMessage.Trim();
        }
        public string AddUsers(Dictionary<string, string> usersSq)
        {

            string retStr = string.Empty;
            using (SqlDataReader dr = DataReader(SqlSpAddUserLogin, usersSq))
            {
                while (dr.Read())
                {
                    retStr += $"{retStr} {dr[0].ToString()}";
                }
            }

            return retStr.Trim();
        }

        //  public bool LogIn(string userName,string passWord,SqlConnection sqlConnection)
        public bool LogIn(string userName, string passWord)
        {

            //  using (SqlDataReader dr = DataReader(SqlSpUserLogIn, new Dictionary<string, string>() { { SqlSpParamLoginName, userName }, { SqlSpParamUserPassWord, passWord } },sqlConnection))
            using (SqlDataReader dr = DataReader(SqlSpUserLogIn, new Dictionary<string, string>() { { SqlSpParamLoginName, userName }, { SqlSpParamUserPassWord, passWord } }))
            {
                if (!(dr.HasRows))
                    throw new Exception($"Error logging in for username:{userName}");
                dr.Read();
                if (dr[0].ToString().Length == 1)
                {
                    char[] admin = dr[0].ToString().ToCharArray();
                    if (admin[0] == Admin)
                        return true;

                }
                else
                    throw new Exception(dr[0].ToString());
            }

            return false;
        }
        private SqlDataReader DataReader(string sqlSP)
        {
            using (SqlCommand cmd = new SqlCommand(sqlSP, SqlConn))
            {
                cmd.CommandTimeout = Settings.Default.SqlServerTimeOut;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                return dr;


            }
        }
        public DataSet UserLoginDataTabe()
        {
            DataTable userDataTable = new DataTable("UserLoginIs");
            DataSet set = new DataSet("logIDS");
            userDataTable.Columns.Add("loginID", typeof(string));
            DataColumn logName = new DataColumn("loginIDS", typeof(string));
            userDataTable.Columns.Add(logName);
            using (SqlDataReader dr = DataReader(SqlSpGetLoginIds))
            {
                if (dr.HasRows)
                {
                    
                    while (dr.Read())
                    {
                        DataRow dataRow = userDataTable.NewRow();
                        dataRow["loginIDS"] = dr["loginId"].ToString();
                        userDataTable.Rows.Add(dataRow);
                    }
                    userDataTable.AcceptChanges();
                    set.Tables.Add(userDataTable);
                }
            }
            return set;

        }
        //  private SqlDataReader DataReader(string sqlSP, Dictionary<string, string> dicSpParam,SqlConnection sqlConnection)
        private SqlDataReader DataReader(string sqlSP, Dictionary<string, string> dicSpParam)
        {

            string strMessage = string.Empty;
            try
            {
                if ((dicSpParam.Count == 0) || (string.IsNullOrEmpty(sqlSP)))
                {
                    throw new Exception("Invalid arguments passed");
                }


                using (SqlCommand cmd = new SqlCommand(sqlSP, SqlConn))
                {
                    cmd.CommandTimeout = Settings.Default.SqlServerTimeOut;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;


                    foreach (KeyValuePair<string, string> spPara in dicSpParam)
                    {
                        cmd.Parameters.Add(new SqlParameter(spPara.Key, spPara.Value));
                    }
                    SqlDataReader dr = cmd.ExecuteReader();
                    return dr;


                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (SqlConn.State == System.Data.ConnectionState.Open)
                        SqlConn.Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SqlCmds()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            try
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
            }

            catch
            { }
            GC.SuppressFinalize(this);

        }
        #endregion
    }
}
