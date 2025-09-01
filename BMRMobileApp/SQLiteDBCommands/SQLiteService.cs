using BMRMobileApp.InterFaces;
using BMRMobileApp.Models;
 
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//DependencyService.Get<IAppExitService>()?.Exit();
namespace BMRMobileApp.SQLiteDBCommands
{
    public class SQLiteService
    {
        private readonly string dbPath;
        private readonly SQLiteConnection connection;
        public SQLiteService()
        {
            dbPath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "dcrpeersuppotusers.db");
            connection = new SQLiteConnection(dbPath, Utilites.PSUtilites.Flags);
            //_connection.CreateTable<EmotionEntry>(); // Your model
        }
        public async Task<PSupportUsers> GetPsUsers()
                    {
            return await Task.Run(() =>
            {
                return connection.Table<PSupportUsers>().FirstOrDefault();
            });
        }
        public async Task<PSUsersLogin> GetPsUsersLogin()
        {
            return await Task.Run(() =>
            {
                return connection.Table<PSUsersLogin>().FirstOrDefault();
            });
        }
        public async Task<PSSettings> GetPsSettings()
        {
            return await Task.Run(() =>
            {
                return connection.Table<PSSettings>().FirstOrDefault();
            });
        }
        public async Task<IList<PSViedo>> GetPsUsersViedo()
        {
            return await Task.Run(() =>
            {
                return connection.Table<PSViedo>().ToList();
            });
        }
        public   bool TableExists(string tableName)
        {
            Task.Run(() =>
            {
                var query = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";
                var result = connection.ExecuteScalar<string>(query);
                return !string.IsNullOrEmpty(result);
            });
            return false;

        }
        public async Task<PSSettings> GetPSSettings()
        {
            if(!(TableExists("PSSettings")))
            {
                connection.CreateTable<PSSettings>();
                await AddSettings(1,true);
            }
            return await GetPsSettings();
        }
        public async Task CreateDatabases()
        {
            
            connection.CreateTable<PSupportUsers>();
            connection.CreateTable<PSUsersLogin>();
            connection.CreateTable<PSFeelingNotes>();
            connection.CreateTable<PSToDOList>();
            connection.CreateTable<PSChatGroups>();
            connection.CreateTable <PSChatUserRole>();
            connection.CreateTable<PSChatMessages>();
            connection.CreateTable<PSViedo>();
            connection.CreateTable<PSSettings>();

            await CreateRoles();
            await AddSettings(0,true);
            //   connection.CreateTable<PSGroupChats>();

            await Task.CompletedTask;



        }
        public async Task DropCreteTables()
        {

           await DropAllTables();
            //connection.CreateTable<PSUsersLogin>();
            connection.CreateTable<PSFeelingNotes>();
            connection.CreateTable<PSToDOList>();
            connection.CreateTable<PSChatGroups>();
            connection.CreateTable<PSChatUserRole>();
            connection.CreateTable<PSChatMessages>();
            connection.CreateTable<PSViedo>();
            connection.CreateTable<PSSettings>();


            await CreateRoles();
            await AddSettings(1,true);
            //   connection.CreateTable<PSGroupChats>();

            await Task.CompletedTask;



        }
        private async Task DropAllTables(bool dropUser=false)
        {
            if (dropUser)
            {
                connection.DropTable<PSupportUsers>();
                connection.DropTable<PSUsersLogin>();
            }
            
           // connection.DropTable<PSUsersLogin>();
            connection.DropTable<PSFeelingNotes>();
            connection.DropTable<PSToDOList>();
            connection.DropTable<PSChatGroups>();
            connection.DropTable<PSChatUserRole>();
            connection.DropTable<PSChatMessages>();
            connection.DropTable<PSViedo>();
            connection.DropTable<PSSettings>();
            
            //  connection.DropTable<PSGroupChats>();
            await Task.CompletedTask;
        }
        public async Task ResetDatabase()
        {
            await DropAllTables();
            await CreateDatabases();
        }
        public async Task DeleteDatabase()
        {
            connection.Close();
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }
            await Task.CompletedTask;
        }
        public void CloseConnection()
        {
            connection.Close();
        }

      public async Task CreateRoles()
        {
            var roles = new List<PSChatUserRole>
            {
                new PSChatUserRole { PSUserRole = "Admin", PSRoleDescription = "Administrator with full access", PSRolePermissions = "Read, Write, Delete" },
                new PSChatUserRole { PSUserRole = "Member", PSRoleDescription = "Regular member with limited access", PSRolePermissions = "Read, Write" },
                new PSChatUserRole { PSUserRole = "Moderator", PSRoleDescription = "Moderator with content management permissions", PSRolePermissions = "Read, Write, Delete" }
            };
            foreach (var role in roles)
            {
                var existingRole = connection.Table<PSChatUserRole>().FirstOrDefault(r => r.PSUserRole == role.PSUserRole);
                if (existingRole == null)
                {
                    connection.Insert(role);
                }
            }
            await Task.CompletedTask;
        }
        public async Task AddSettings(int showFeeling,bool createdTable=false)
        {
            PSSettings settings = new PSSettings();
            settings.ShowFeelingsPage = showFeeling;
            if (createdTable)
            {
                connection.Insert(settings);
            }
                
            else
            {
                settings.ID = 1;
                connection.Execute($"Update PSSettings set ShowFeelingsPage = {settings.ShowFeelingsPage} where ID = {settings.ID}");
              //  connection.Update(settings);
            }



            await Task.CompletedTask;
        }
        public Task AddPSUser(PSupportUsers usersModel)
        {             return Task.Run(() =>
            {
         
                connection.Insert(usersModel);
                
            });
        }
        public Task<int> AddViedo(PSViedo pSViedo)
        {
            return Task.Run(() =>
            {

                connection.Insert(pSViedo);
                return connection.ExecuteScalar<int>("SELECT MAX(ID) FROM PSViedo");


            });
        }
        public Task DelViedo(int id)
        {
            return Task.Run(() =>
            {
                connection.Delete<PSViedo>(id);

            });
        }
        public Task AddPSFeelings(PSFeelingNotes pSFeelingNotes)
        {
            return Task.Run(() =>
            {

                connection.Insert(pSFeelingNotes);

            });
        }
        public Task<List<PSupportUsers>> GetPSUsersByName(string psUserFirstName, string psUserLastName)
        {
            return Task.Run(() =>
            {
                return connection.Table<PSupportUsers>().Where(u => u.PSUserFirstName == psUserFirstName && u.PSUserLastName == psUserLastName).ToList();
            });
        }
        public Task<List<PSupportUsers>> GetPSUsers()
        {
            return Task.Run(() =>
            {
                return connection.Table<PSupportUsers>().ToList();
            });
        }
        public Task<PSupportUsers> GetPSUserById(string psUserFirstName,string psUserLastName)
        {
            return Task.Run(() =>
            {
                return connection.Table<PSupportUsers>().FirstOrDefault(u => u.PSUserFirstName == psUserFirstName &&  u.PSUserLastName == psUserLastName);
            });
        }
        public Task UpdatePSUser(PSupportUsers usersModel)
        {
            return Task.Run(() =>
            {
                connection.Update(usersModel);
            });
        }
        public Task UpdatePSUserLogin(PSUsersLogin pSUsersLogin)
        {
            return Task.Run(() =>
            {
                connection.Update(pSUsersLogin);
            });
        }
        public Task DeletePSUser(PSupportUsers usersModel)
        {
            return Task.Run(() =>
            {
                connection.Delete(usersModel);
            });
        }
        
        public Task<PSUsersLogin> GetPSUserLogins(int psUserID)
        {
            return Task.Run(() =>
            {
                return connection.Table<PSUsersLogin>().FirstOrDefault(u =>u.ID == psUserID);
            });
        }
        public Task<PSUsersLogin> GetPSUserPassword(string userName)
        {
            return Task.Run(() =>
            {
                return connection.Table<PSUsersLogin>().FirstOrDefault(u => u.PSUserName == userName);
            });
        }
        public Task AddPSUserLogin(PSUsersLogin pSUserLogin)
        {
            return Task.Run(() =>
            {
                connection.Insert(pSUserLogin);
            });
        }
    }
}
