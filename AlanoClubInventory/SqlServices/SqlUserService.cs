using AlanoClubInventory.Models;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace AlanoClubInventory.SqlServices
{
    public class SqlUserService
    {
        private static SqlUserService sqlUserServiceIntance;
        private static readonly object _lock = new object();
        private SqlUserService() { }
        public static SqlUserService UserServiceIntance
        {
            get
            {
                // Double-checked locking for thread safety
                if (sqlUserServiceIntance == null)
                {
                    lock (_lock)
                    {
                        if (sqlUserServiceIntance == null)
                        {
                            sqlUserServiceIntance = new SqlUserService();
                        }
                    }
                }
                return sqlUserServiceIntance;
            }
        }
        public async Task<string> GenerateSalt(int size = 32)
        {
            byte[] saltBytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }
        public async Task<string> GenerateSaltString(int size = 32)
        {
            byte[] saltBytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }

        public async Task<(string Salt, string Hash)> HashPassword(string password, string salt)
        {
            
            string saltedPassword = password + salt;

            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(saltedPassword));
            string hash = Convert.ToBase64String(hashBytes);

            return (salt, hash);
        }
        public async Task<(string Salt, string Hash)> HashPassword(string password, int saltSize = 32)
        {
            byte[] saltBytes = new byte[saltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            string salt = Convert.ToBase64String(saltBytes);
            string saltedPassword = password + salt;

            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(saltedPassword));
            string hash = Convert.ToBase64String(hashBytes);

            return (salt, hash);
        }


        public async Task<bool> VerifyPassword(string password, string salt, string storedHash)
        {
            string saltedPassword = password + salt;
            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(saltedPassword));
            string computedHash = Convert.ToBase64String(hashBytes);

            return computedHash == storedHash;
        }
        public async Task UpdateAdduser(ALanoClubUsersModel user, string sqlConnection, bool emilInfo, string sp)
        {

            var salt = await GenerateSaltString();
            var pw =await HashPassword(user.UserPasswordString,salt);
            user.Salt = pw.Salt;
            user.UserPassword = pw.Hash;
            user.UserPasswordReversed = new string(user.UserPasswordString.Reverse().ToArray());
            pw = await HashPassword(user.UserPasswordReversed, salt);
            user.UserPasswordReversed = pw.Hash;
            await UpdateUserInfor(user, sqlConnection,sp);

            await Task.CompletedTask;
        }
        public async Task ChangeUserPW(int userID, string sqlConnection, string newPW)
        {

            var salt = await GenerateSaltString();
            var pw = await HashPassword(newPW, salt);
            
            var prev = await HashPassword(Utilites.ALanoClubUtilites.RevStr(newPW), salt);
            
            await UpdateUserInPW(userID, salt, pw.Hash, prev.Hash, sqlConnection);

            await Task.CompletedTask;
        }
        private async Task UpdateUserInfor(ALanoClubUsersModel user,string sqlConnection,string sp)
        {
            var parameters = new List<StoredParValuesModel>();
            parameters.Add(new StoredParValuesModel { ParmaName = SqlConstProp.SPParmaID, ParmaValue = user.ID.ToString() });
            parameters.Add(new StoredParValuesModel { ParmaName = SqlConstProp.SPParmaUserName, ParmaValue = user.UserName });
            parameters.Add(new StoredParValuesModel { ParmaName = SqlConstProp.SPParmaPasswordHash, ParmaValue = user.UserPassword });
            parameters.Add(new StoredParValuesModel { ParmaName = SqlConstProp.SPParmaSalt, ParmaValue = user.Salt });
            parameters.Add(new StoredParValuesModel { ParmaName = SqlConstProp.SPParmaIsAdmin, ParmaValue = user.IsAdmin.ToString() });
            parameters.Add(new StoredParValuesModel { ParmaName = SqlConstProp.SPParmaMemberFirstName, ParmaValue = user.UserFirstName });
            parameters.Add(new StoredParValuesModel { ParmaName = SqlConstProp.SPParmaMemberLastName, ParmaValue = user.UserLastName });
            parameters.Add(new StoredParValuesModel { ParmaName = SqlConstProp.SPParmaMemberEmail, ParmaValue = user.UserEmailAddress });
            parameters.Add(new StoredParValuesModel { ParmaName = SqlConstProp.SPParmaMemberPhoneNumber, ParmaValue = user.UserPhoneNumber });
            parameters.Add(new StoredParValuesModel { ParmaName = SqlConstProp.SPParmaIsActiveMember, ParmaValue = user.IsActive.ToString()});
            parameters.Add(new StoredParValuesModel { ParmaName = SqlConstProp.SPParmaUserPasswordReversed, ParmaValue = user.UserPasswordReversed });
          await  AlClubSqlCommands.SqlCmdInstance.UpDateInsertWithParma(sqlConnection, sp, parameters);

            // Implement email sending logic here
        }
        private async Task UpdateUserInPW(int id,string salt,string pw,string pwr, string sqlConnection)
        {
            var parameters = new List<StoredParValuesModel>();
            parameters.Add(new StoredParValuesModel { ParmaName = SqlConstProp.SPParmaID, ParmaValue = id.ToString() });
            parameters.Add(new StoredParValuesModel { ParmaName = SqlConstProp.SPParmaPasswordHash, ParmaValue = pw});
            parameters.Add(new StoredParValuesModel { ParmaName = SqlConstProp.SPParmaSalt, ParmaValue = salt });
            parameters.Add(new StoredParValuesModel { ParmaName = SqlConstProp.SPParmaUserPasswordReversed, ParmaValue = pwr});
         await   AlClubSqlCommands.SqlCmdInstance.UpDateInsertWithParma(sqlConnection, SqlConstProp.SPUpdateUserPassword, parameters);

            // Implement email sending logic here
        }
        public async void LogInOutVol()
        {
            if (LoginUserModel.LoginInstance.ID > 0)
            { 
            var sqlConn = await Utilites.ALanoClubUtilites.GetConnectionStr();
            Utilites.ALanoClubUtilites.UserVolHrsID= await AlClubSqlCommands.SqlCmdInstance.LogVolHrs(sqlConn, SqlConstProp.SPAlanoClubClockInOutVolHours);
            }
        }
    }
}
