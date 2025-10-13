using BMRMobileApp.InterFaces;
using BMRMobileApp.Models;
 
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SQLite.SQLite3;


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
        public async Task<PSUserMood> GetPSUserMood()
        {
            return await Task.Run(() =>
            {
                return connection.Table<PSUserMood>().FirstOrDefault();
            });
        }
        
        public async Task<PSUsersLogin> GetPsUsersLogin()
        {
            return await Task.Run(() =>
            {
                return connection.Table<PSUsersLogin>().FirstOrDefault();
            });
        }


        public async Task InsetRec<T>(T record)
        {
            await Task.Run(() =>
           {
               return connection.Insert(record);
           });
            
        }

        public async Task<PSSettings> GetPsSettings()
        {
            return await Task.Run(() =>
            {
                
                return connection.Table<PSSettings>().FirstOrDefault();
                
            });
             
        }
        public  PSSettings GetPsSettingsNonAsync()
        {
         

                return connection.Table<PSSettings>().FirstOrDefault();

         

        }
        public async Task<IList<PSViedo>> GetPsUsersViedo()
        {
            return await Task.Run(() =>
            {
                return connection.Table<PSViedo>().ToList();
            });
        }
        public bool TableExists(string tableName)
        {
            try
            {
                Task.Run(() =>
                {
                    var query = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";
                    var result = connection.ExecuteScalar<string>(query);
                    //  Console.WriteLine(result);
                    return !string.IsNullOrEmpty(result);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking table existence: {ex.Message}");
                return false;
            }
            return false;

        }
        public async Task AddUpdateJournalEntryGoals(PSJounalEntryGoals pSJounalEntryGoals)
        {
            await Task.Run(() =>
            {
                var maxId = connection.Table<PSJJournalEntry>().OrderByDescending(x => x.ID).FirstOrDefault()?.ID ?? 0;
                var item = connection.Table<PSJounalEntryGoals>().FirstOrDefault(x => x.ID == maxId);
                pSJounalEntryGoals.ID = maxId;
                if (item != null)
                {
                    item.ID = maxId;
                    item.PSUserID = pSJounalEntryGoals.PSUserID;
                    item.JournalEntry = pSJounalEntryGoals.JournalEntry;
                    item.DateAdded = DateTime.Now.ToString();
                   
                    connection.Update(pSJounalEntryGoals);

                }
                else
                {
                    connection.Insert(pSJounalEntryGoals);
                }


            });
        }
        public async Task UpDateInsertPSUserMood(PSUserMood pSUserMood)
        {
            await Task.Run(() =>
            {
                var existingMood = connection.Table<PSUserMood>().FirstOrDefault();
                if (existingMood == null)
                {
                    connection.Insert(pSUserMood);
                }
                else
                {
                    pSUserMood.ID = existingMood.ID; // Ensure the ID is set for the update
                    connection.Update(pSUserMood);
                }
            });
        }

        public async Task<PSSettings> GetPSSettings()
        {
            //if (!(TableExists("PSSettings")))
            //{
            //    connection.CreateTable<PSSettings>();
            //    await AddSettings(1, true);
            //    await AddSettings("AutoScroll", "1");
            //    await AddSettings("ScrollWaits", "3000");
            //}
            return await GetPsSettings();
        }
        #region create tables
        public async Task CreateDatabases()
        {

            connection.CreateTable<PSupportUsers>();
            connection.CreateTable<PSUsersLogin>();
            connection.CreateTable<PSJJournalEntry>();
            connection.CreateTable<PSUserTask>();
            connection.CreateTable<PSChatGroups>();
            connection.CreateTable<PSChatUserRole>();
            connection.CreateTable<PSChatMessages>();
            connection.CreateTable<PSViedo>();
            connection.CreateTable<PSSettings>();
            connection.CreateTable<PSUserMood>();
            connection.CreateTable<PSJounalEntryGoals>();
            connection.CreateTable<PSIconTask>();
            connection.CreateTable<PSTaskCatgory>();
            connection.CreateTable<PSTaskTags>();
            connection.CreateTable<PSEmotionsTag>();
            connection.CreateTable<PSTaskTagIDs>();
            connection.CreateTable< PSTaskTagIDs>();
            connection.CreateTable<PSCopingMechanismsl>();
            connection.CreateTable<PSSharedExperienceIDs>();
            connection.CreateTable<PSRecourceSites>();
            await CreatePSResourceSites();
            await CreateTablePSSharedExperience();
            await PSAddTaskCategory();
            await PSAddTaskIcons();
            await CreateRoles();
            
            await PSAddTaskTags();
            await AddPSEmotionsTag();
            await CreateSettingTable();
            //   connection.CreateTable<PSGroupChats>();

            await Task.CompletedTask;



        }
        public async Task DropCreteTables()
        {

            await DropAllTables();
            //connection.CreateTable<PSUsersLogin>();
            connection.CreateTable<PSJJournalEntry>();
            connection.CreateTable<PSUserTask>();
            connection.CreateTable<PSChatGroups>();
            connection.CreateTable<PSChatUserRole>();
            connection.CreateTable<PSChatMessages>();
            connection.CreateTable<PSViedo>();
            connection.CreateTable<PSSettings>();
            connection.CreateTable<PSUserMood>();
            connection.CreateTable<PSJounalEntryGoals>();
            connection.CreateTable<PSIconTask>();
            connection.CreateTable<PSTaskCatgory>();
            connection.CreateTable<PSTaskTags>();
            connection.CreateTable<PSEmotionsTag>();
            connection.CreateTable<PSTaskTagIDs>();
            connection.CreateTable<PSTaskTagIDs>();
            connection.CreateTable<PSCopingMechanismsl>();
            connection.CreateTable<PSSharedExperienceIDs>();
            connection.CreateTable<PSRecourceSites>();
            await CreatePSResourceSites();
            await CreateTablePSSharedExperience();
            await PSAddTaskCategory();
            await PSAddTaskIcons();
            await CreateRoles();
            await PSAddTaskTags();
            await CreateSettingTable();
            await AddPSEmotionsTag();
          
          
            //   connection.CreateTable<PSGroupChats>();

            await Task.CompletedTask;



        }
        private async Task DropAllTables(bool dropUser = false)
        {
            if (dropUser)
            {
                connection.DropTable<PSupportUsers>();
                connection.DropTable<PSUsersLogin>();
            }

            // connection.DropTable<PSUsersLogin>();
            connection.DropTable<PSJJournalEntry>();
            connection.DropTable<PSUserTask>();
            connection.DropTable<PSChatGroups>();
            connection.DropTable<PSChatUserRole>();
            connection.DropTable<PSChatMessages>();
            connection.DropTable<PSViedo>();
            connection.DropTable<PSSettings>();
            connection.DropTable<PSUserMood>();
            connection.DropTable<PSJounalEntryGoals>();
            connection.DropTable<PSIconTask>();
            connection.DropTable<PSTaskCatgory>();
            connection.DropTable<PSTaskTags>();
            connection.DropTable<PSEmotionsTag>();
            connection.DropTable<PSTaskTagIDs>();
            connection.DropTable<PSTaskTagIDs>();
            connection.DropTable<PSSharedExperienceIDs>();
            connection.DropTable<PSCopingMechanismsl>();
            connection.DropTable<PSRecourceSites>();

            //    await CreateTablePSSharedExperience();
            //  await AddPSEmotionsTag();
            //  connection.DropTable<PSGroupChats>();
            await Task.CompletedTask;
        }
        #endregion

        public async Task AddPSEmotionsTag()
        {
            IList<PSEmotionsTag> tags = new List<PSEmotionsTag>();
            tags.Add(new PSEmotionsTag { Emotion = Utilites.EmojiTags.EmotionsTagAngry, EmotionsColor = Utilites.EmojiTags.EmotionsTagAngryColor, EmotionIcon = Utilites.EmojiTags.AngryFace });
            tags.Add(new PSEmotionsTag { Emotion = Utilites.EmojiTags.EmotionsTagAnxious, EmotionsColor = Utilites.EmojiTags.EmotionsTagAnxiousColor, EmotionIcon = Utilites.EmojiTags.Anxious });
            tags.Add(new PSEmotionsTag { Emotion = Utilites.EmojiTags.EmotionsTagCalm, EmotionsColor = Utilites.EmojiTags.EmotionsTagCalmColor, EmotionIcon = Utilites.EmojiTags.Calm });
            tags.Add(new PSEmotionsTag { Emotion = Utilites.EmojiTags.EmotionsTagConfused, EmotionsColor = Utilites.EmojiTags.EmotionsTagConfusedColor, EmotionIcon = Utilites.EmojiTags.Confused });
            tags.Add(new PSEmotionsTag { Emotion = Utilites.EmojiTags.EmotionsTagEnergized, EmotionsColor = Utilites.EmojiTags.EmotionsTagEnergizedColor, EmotionIcon = Utilites.EmojiTags.Energetic });
            tags.Add(new PSEmotionsTag { Emotion = Utilites.EmojiTags.EmotionsTagGrateful, EmotionsColor = Utilites.EmojiTags.EmotionsTagGratefulColor, EmotionIcon = Utilites.EmojiTags.GratefulFace });
            tags.Add(new PSEmotionsTag { Emotion = Utilites.EmojiTags.EmotionsTagHappy, EmotionsColor = Utilites.EmojiTags.EmotionsTagHappyColor, EmotionIcon = Utilites.EmojiTags.Happy });
            tags.Add(new PSEmotionsTag { Emotion = Utilites.EmojiTags.EmotionsTagHopeful, EmotionsColor = Utilites.EmojiTags.EmotionsTagHopefulColor, EmotionIcon = Utilites.EmojiTags.Hopeful });
            tags.Add(new PSEmotionsTag { Emotion = Utilites.EmojiTags.EmotionsTagJoyful, EmotionsColor = Utilites.EmojiTags.EmotionsTagJoyfulColor, EmotionIcon = Utilites.EmojiTags.JoyFul });
            tags.Add(new PSEmotionsTag { Emotion = Utilites.EmojiTags.EmotionsTagLonely, EmotionsColor = Utilites.EmojiTags.EmotionsTagLonelyfulColor, EmotionIcon = Utilites.EmojiTags.Lonely });
            tags.Add(new PSEmotionsTag { Emotion = Utilites.EmojiTags.EmotionsTagLoved, EmotionsColor = Utilites.EmojiTags.EmotionsTagLovedColor, EmotionIcon = Utilites.EmojiTags.Loved });
            tags.Add(new PSEmotionsTag { Emotion = Utilites.EmojiTags.EmotionsTagNeutral, EmotionsColor = Utilites.EmojiTags.EmotionsTagNeutralColor, EmotionIcon = Utilites.EmojiTags.Neutral });
            tags.Add(new PSEmotionsTag { Emotion = Utilites.EmojiTags.EmotionsTagPeaceful, EmotionsColor = Utilites.EmojiTags.EmotionsTagPeacefulColor, EmotionIcon = Utilites.EmojiTags.Sad });

            tags.Add(new PSEmotionsTag { Emotion = Utilites.EmojiTags.EmotionsTagReflective, EmotionsColor = Utilites.EmojiTags.EmotionsTagReflectiveColor, EmotionIcon = Utilites.EmojiTags.Reflective });
            
            foreach (var tag in tags)
            {
                connection.Insert(tag);
            }


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

        public async Task AddPSProjects(PSUserTask pSUserProjects)
        {
            connection.Insert(pSUserProjects);
        }
        //Deserialize<T>(string json, string model) where T : new ()
        public async Task<List<T>> GetTAsync<T>() where T : new()
        {
            var list = connection.Table<T>().ToList();
            return list;
            //return Task.Run(() =>
            //{
            //    var list = connection.Table<T>().ToList();
            //   return new List<T>(list);
            //});


        }
        //public async Task<ObservableCollection<T>> ToObservableCollection<T>(this IEnumerable<T> source)
        //{
        //    return new ObservableCollection<T>(source);
        //}
        public async Task PSAddTaskIcons()
        {
            var icons = new List<PSIconTask>();
            icons.Add(new PSIconTask { TaskTags = Utilites.EmojiTags.Ribbon, Descripotion = "Ribbon" });
            icons.Add(new PSIconTask { TaskTags = Utilites.EmojiTags.Star, Descripotion = "Star" });
            icons.Add(new PSIconTask { TaskTags = Utilites.EmojiTags.Trophy, Descripotion = "Trophy" });
            icons.Add(new PSIconTask { TaskTags = Utilites.EmojiTags.Books, Descripotion = "Book" });

            foreach (var icon in icons)
            {
                connection.Insert(icon);
            }

        }
        public async Task PSAddTaskTags()
        {
            var tags = new List<PSTaskTags>();
            tags.Add(new PSTaskTags { TagName = "Work", TagColor = "#3068df" });
            tags.Add(new PSTaskTags { TagName = "Personal", TagColor = "#FF4500" });
            tags.Add(new PSTaskTags { TagName = "Health", TagColor = "#32CD32" });
            tags.Add(new PSTaskTags { TagName = "Family", TagColor = "#1E90FF" });
            tags.Add(new PSTaskTags { TagName = "Friends", TagColor = "#FF69B4" });
            // tags.Add(new PSTaskTags { TagName = "DefaultColor", TagColor = "#FF0000" });

            foreach (var icon in tags)
            {
                connection.Insert(icon);
            }

        }
        public async Task PSAddTaskCategory()
        {
            var cat = new List<PSTaskCatgory>();
            cat.Add(new PSTaskCatgory { TaskCategoryName = "Medical" });
            cat.Add(new PSTaskCatgory { TaskCategoryName = "Meeting" });
            cat.Add(new PSTaskCatgory { TaskCategoryName = "Work" });
            cat.Add(new PSTaskCatgory { TaskCategoryName = "Education" });
            cat.Add(new PSTaskCatgory { TaskCategoryName = "Self" });
            cat.Add(new PSTaskCatgory { TaskCategoryName = "Relationships" });

            foreach (var category in cat)
            {
                connection.Insert(category);
            }

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
        public async Task CreateSettingTable()
        {
            PSSettings settings = new PSSettings();
            settings.ShowFeelingsPage = 1;
            settings.AutoScroll = 1;
            settings.ScrollWaits = "3 Sec";
            connection.Insert(settings);

        }
        public int GetDelyTime(string delTime)
        {
            var index = delTime.IndexOf(" ");
            var wTime = delTime.Substring(0, index);
            if(index > 0)
            {
                if (int.TryParse(delTime.Substring(0, index).Trim(), out int result))
                {
                    if (delTime.ToLower().EndsWith("sec"))
                        return result * Utilites.Consts.AutoScroll;
                    else
                        return result * Utilites.Consts.AutoScrollHR;
                }
            }
            return 3000;
        }
        public int GetDelyTimeHrSec(string delTime)
        {
            var index = delTime.IndexOf(" ");
            var wTime = delTime.Substring(0, index);
            if (index > 0)
            {
                if (int.TryParse(delTime.Substring(0, index).Trim(), out int result))
                {
                    if (delTime.ToLower().EndsWith("sec"))
                        return result;
                    else
                        return result * Utilites.Consts.AutoScrollHR;
                }
            }
            return 3000;
        }
        //public async Task AddSettings(int showFeeling, bool createdTable = false)
        //{
        //    PSSettings settings = new PSSettings();
        //    settings.ShowFeelingsPage = showFeeling;
        //    if (createdTable)
        //    {
        //        connection.Insert(settings);
        //    }

        //    else
        //    {
        //        settings.ID = 1;
        //        connection.Execute($"Update PSSettings set ShowFeelingsPage = {settings.ShowFeelingsPage} where ID = {settings.ID}");
        //        //  connection.Update(settings);
        //    }



        //    await Task.CompletedTask;
        //}
        public async Task AddSettings(string coloumToUpDate,string newValue)
        {

                connection.Execute($"Update PSSettings set {coloumToUpDate} = {newValue} where ID = 1");
                //  connection.Update(settings);




            await Task.CompletedTask;
        }
        public Task AddPSUser(PSupportUsers usersModel)
        {
            return Task.Run(() =>
            {

                connection.Insert(usersModel);

            });
        }
        public async Task DelDataAsyncByID<T>(T data, int id)
        {
           await Task.Run(async () =>
            {
                connection.Delete<T>(id);
                

            });
            await Task.CompletedTask;
        }
        public async Task<int> GetMaxId(string tableName)
        {
            

          return(connection.ExecuteScalar<int>($"SELECT MAX(ID) FROM {tableName}"));

            
        }
        public async Task UpDateTableByID(string coloumToUpDate, string coloumValue, string tableName,int id=0,bool getMaxValueID=false)
            {
            if(getMaxValueID)
            {
                id =  await GetMaxId(tableName);
            }
            if(string.IsNullOrWhiteSpace(coloumValue))
                coloumValue = id.ToString();
            if(id > 0)
            {
                connection.Execute($"Update {tableName} set {coloumToUpDate} = {coloumValue} where ID ={id}");
            }
            }

        public async Task<int> AddViedo(PSViedo pSViedo)
        {



            connection.Insert(pSViedo);
                return connection.ExecuteScalar<int>("SELECT MAX(ID) FROM PSViedo");


         
        }
        public Task DelViedo(int id)
        {
            return Task.Run(() =>
            {
                connection.Delete<PSViedo>(id);

            });
        }
        public Task AddPSFeelings(PSJJournalEntry pSFeelingNotes)
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
        public Task<PSupportUsers> GetPSUserById(string psUserFirstName, string psUserLastName)
        {
            return Task.Run(() =>
            {
                return connection.Table<PSupportUsers>().FirstOrDefault(u => u.PSUserFirstName == psUserFirstName && u.PSUserLastName == psUserLastName);
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
                return connection.Table<PSUsersLogin>().FirstOrDefault(u => u.ID == psUserID);
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
        public async Task UpdatePSUserTaskTagID(int tagID)
        {
            await Task.Run(() =>
            {
                connection.Execute($"Update PSUserTask set TagID = {tagID} where ID = {tagID}");
            } );
        }
        
        public async Task<IList<PSTaskTagIDs>>  GetPSTaskTagIDs(int id)
        {
            return await Task.Run(() =>
            {
                return (connection.Table<PSTaskTagIDs>().Where(p => p.ID == id).ToList());
            });

            
            
        }
        public async Task<IList<T>> GetPSUserTask<T>(string sqlQuery ) where T : new()
        {

            return await Task.Run(() =>
            {
                return (connection.Query<T>(sqlQuery).ToList());
            });
            
        }

        public  IList<T> GetPSUserTaskNonTasl<T>(string sqlQuery) where T : new()
        {

            return (connection.Query<T>(sqlQuery).ToList());;

        }
        public async Task<PSUserMood> GetUserCurrentMood()
        {
            try
            {


                PSUserMood usermood = await GetPSUserMood();
                if (usermood != null)
                {
                    return (usermood);
                    
                }
            }
            catch { }
            await Task.CompletedTask;
            return null;
        }
        public PSUserMood GetUserCurrentMoodNonAsync()
        {
            try
            {


               return(connection.Table<PSUserMood>().FirstOrDefault());
            }
            catch { }
            
            return null;
        }

        public async Task UpDateTable(string sqlQuery)
        { 
            await Task.Run(() =>
            {
                  connection.Execute(sqlQuery);
            });

            

        }
        
        public async Task CreateTablePSSharedExperience()
        {
            IList<PSSharedExperienceIDs> psE  = new List<PSSharedExperienceIDs>();
            psE.Add(new PSSharedExperienceIDs { SharedExperienceTags = "UnDecided" });
            psE.Add(new PSSharedExperienceIDs { SharedExperienceTags= "Lost a Parent"});

            psE.Add(new PSSharedExperienceIDs { SharedExperienceTags = "Burnout"});
            psE.Add(new PSSharedExperienceIDs { SharedExperienceTags = "Anxiety" });
           psE.Add(new PSSharedExperienceIDs { SharedExperienceTags = "Comming Out" });
            psE.Add(new PSSharedExperienceIDs { SharedExperienceTags = "Death" });
            psE.Add(new PSSharedExperienceIDs { SharedExperienceTags = "PTSD" });
            psE.Add(new PSSharedExperienceIDs { SharedExperienceTags = "Drug Addiction" });
            psE.Add(new PSSharedExperienceIDs { SharedExperienceTags = "Sex Addiction" });
            psE.Add(new PSSharedExperienceIDs { SharedExperienceTags = "Alcoholism" });
            psE.Add(new PSSharedExperienceIDs { SharedExperienceTags = "Grief" });
            psE.Add(new PSSharedExperienceIDs { SharedExperienceTags = "Caregiving" });
            psE.Add(new PSSharedExperienceIDs { SharedExperienceTags = "Identity" });
            foreach(var item in psE)
            connection.Insert(item);



        }
        public async Task CreatePSResourceSites()
        {
           
            IList<PSRecourceSites> psSites = new List<PSRecourceSites>();
            psSites.Add(new PSRecourceSites { SupportedSiteName= "Beautiful Minds Recovery", SupportWebSites = "https://www.beautifulmindsrecovery.com/",DeleteResource=0 });
            
            psSites.Add(new PSRecourceSites { SupportedSiteName = "Journaling Insights", SupportWebSites = "https://journalinginsights.com/effective-journaling-prompts-self-reflection/", DeleteResource = 1 });
            
            psSites.Add(new PSRecourceSites { SupportedSiteName = "Positive Psychology Jornal Prompts", SupportWebSites = "https://positivepsychology.com/journaling-prompts/", DeleteResource = 1 });
            
            psSites.Add(new PSRecourceSites { SupportedSiteName = "Jmir Publications", SupportWebSites = "https://jmirpublications.com/", DeleteResource = 1 });
            
            psSites.Add(new PSRecourceSites { SupportedSiteName = "BMC Medicine", SupportWebSites = "https://bmcmedicine.biomedcentral.com/", DeleteResource = 1 });
            
            psSites.Add(new PSRecourceSites { SupportedSiteName = "MHA Mental Helth America", SupportWebSites = "https://mhanational.org/peer-support-research-and-reports/" , DeleteResource = 1 });
            
            psSites.Add(new PSRecourceSites { SupportedSiteName= "Life Adjustment Team", SupportWebSites = "https://www.lifeadjustmentteam.com/peer-support-for-psychosocial-rehabilitation/" , DeleteResource = 1 });
            
            psSites.Add(new PSRecourceSites { SupportedSiteName= "Sun Flower Radiance", SupportWebSites = "https://www.sunflowerradiance.com/" , DeleteResource = 1 });
            
            psSites.Add(new PSRecourceSites { SupportedSiteName = "PT Peer Net Work", SupportWebSites = "https://mtpeernetwork.org/" , DeleteResource = 1 });
            
            foreach (var site in psSites)
                connection.Insert(site);

        }
        public   IList<PSRecourceSites> GetResSites()
            {
               
                    return connection.Table<PSRecourceSites>().ToList();
               
            
            

        }
        public async Task InserDataAsync<T>(T record)
        {
            await Task.Run(async () =>
            {
                connection.Insert(record);


            });
            await Task.CompletedTask;
        }

        //public async Task GetAllTableData<T>(T record)
        //{
        //    await Task.Run(async () =>
        //    {
        //        connection.Table<T>(T).ToList();


        //    });
        //    await Task.CompletedTask;
        //}





    }
}

