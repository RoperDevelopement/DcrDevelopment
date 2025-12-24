using AlanoClubInventory.Models;
using AlanoClubInventory.SqlServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Scm = AlanoClubInventory.SqlServices;
namespace AlanoClubInventory
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private AlanoClubInformaitonModel aclInfor;
        private bool isLoggingIn;
        private bool userIsLoggingIn;
        private bool isAdmin;
        private string homeTitle;
        public MainViewModel()
        {
            Utilites.ALanoClubUtilites.IsLoggin = false;
            UserIsLoggingIn = false;
            IsLoggingIn = false;
            IsAdmin = false;
            HomeTitle = "Login Into Home Alano Club Inventory Mangement System";
            GetAlnoClubInfo();
        }
        public string HomeTitle
        {
            get => homeTitle;
            set
            {
                homeTitle = value;
                OnPropertyChanged(nameof(HomeTitle));
            }
        }
        public bool IsAdmin
        {
            get => isAdmin;
            set { isAdmin = value; OnPropertyChanged(nameof(IsAdmin)); }
        }
        public bool UserIsLoggingIn
        {
            get => userIsLoggingIn;
            set { userIsLoggingIn = value; OnPropertyChanged(nameof(UserIsLoggingIn)); }
        }
        public AlanoClubInformaitonModel AclInfor
        {
            get => aclInfor;
            set { aclInfor = value; OnPropertyChanged(nameof(AclInfor)); }
        }
        private async void GetAlnoClubInfo()
        {
            ReadJsonFile readJson = new ReadJsonFile();

            var ac = readJson.GetJsonData<AlanoClubInformaitonModel>("AlanoClubInformaitonModel").Result;
            AclInfor = new AlanoClubInformaitonModel
            {
                ClubName = $"{ac.ClubName}",
                ClubAddress = $"{ac.ClubAddress}",
                ClubPOBox = $"{ac.ClubPOBox}",
                ClubCity = $"{ac.ClubCity} {ac.ClubSt} {ac.ClubZipCode}",
                ClubPhone = $"Phone: {ac.ClubPhone}",
                ClubEmail = $"Email: {ac.ClubEmail}",
                FacebookLink = $"{ac.FacebookLink}"

            };
        }

        public bool IsLoggingIn
        {
            get => isLoggingIn;
            set { isLoggingIn = value; OnPropertyChanged(nameof(IsLoggingIn)); }
        }
        public async void ChangeMainText()
        {
            if (!(string.IsNullOrWhiteSpace(LoginUserModel.LoginInstance.UserName)))
            {

                HomeTitle = $"{LoginUserModel.LoginInstance.UserIntils} User Name  {LoginUserModel.LoginInstance.UserName} Alano Club Inventory Mangement System Conected To Database {Utilites.ALanoClubUtilites.AlanoClubDatabaseName}";
            }
        }
        public async void SetIsAdmin()
        {
            IsAdmin = LoginUserModel.LoginInstance.IsAdmin;
            if (!(string.IsNullOrWhiteSpace(LoginUserModel.LoginInstance.UserName)))
            {
                ChangeMainText();


                Utilites.ALanoClubUtilites.ACInventoryWF = System.IO.Path.Combine(Utilites.ALanoClubUtilites.GetTempPath, $"{Utilites.AlanoCLubConstProp.ACTempSaveFolder}");
                Utilites.ALanoClubUtilites.CreateFolder(Utilites.ALanoClubUtilites.ACInventoryWF);
                CheckDiskandDBSize(Utilites.AlanoCLubConstProp.AlanoClubDBName, "C");
                Utilites.ALanoClubUtilites.AlanoClubDatabaseName = Utilites.AlanoCLubConstProp.AlanoClubDBName;
            }
        }
        public async void CheckDiskandDBSize(string dbName, string disk)
        {
            ReadJsonFile readJson = new ReadJsonFile();
            var sizeSettings = readJson.GetJsonData<DiskSpaceDBSizeModel>(nameof(DiskSpaceDBSizeModel)).Result;
            if (sizeSettings != null)
            {
                SqlServerCheckDBSize sqlServerCheck = new SqlServerCheckDBSize();
                var sizeGB = await sqlServerCheck.GetDataBaseSizeInGB(dbName);
                if (sizeGB < 0)
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox($"Error Could not get databse size", "Error Database Size", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (sizeGB > sizeSettings.DataBaseSize)
                    {
                        Utilites.ALanoClubUtilites.ShowMessageBox($"DataBase {sizeGB.ToString()} is a Max size {sizeSettings.DataBaseSize.ToString()} Database {dbName} needs purged", "Error Database Size", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                sizeGB = await Utilites.ALanoClubUtilites.CheckDriveSpaveDB(disk);
                if (sizeGB < 0)
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox($"Could not get diskspace size", "Error DiskSpace", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (sizeGB < sizeSettings.DiskSpace)
                    {
                        Utilites.ALanoClubUtilites.ShowMessageBox($"DiskSpace left {sizeGB.ToString()} need to free up diskspace", "Error DiskSpace", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            readJson.Dispose();
            await DeleteOldFilesBackupDab();

        }
        public async Task DeleteOldFilesBackupDab()
        {

            ReadJsonFile readJson = new ReadJsonFile();
            AlanoClubDatabaseMaintenanceService maintenanceService = new AlanoClubDatabaseMaintenanceService();
            try
            {
                var sizeSettings = readJson.GetJsonData<DiskSpaceDBSizeModel>(nameof(DiskSpaceDBSizeModel)).Result;
                if (sizeSettings != null)
                {

                    var databaseFolder = await maintenanceService.GetDatabaseFolder();
                    databaseFolder = databaseFolder.ToLower().Replace("data", "ACDBBackups");
                    await Utilites.ALanoClubUtilites.DeleteOldFiles(databaseFolder, sizeSettings.DaysToKeepBackups, $"{Utilites.AlanoCLubConstProp.AlanoClubDBName}_*.*");
                    databaseFolder = System.IO.Path.Combine(databaseFolder, $"{Utilites.AlanoCLubConstProp.ACDataBaseBackupDbName}.bak");
                    Utilites.ALanoClubUtilites.RunBackUp(databaseFolder, 10);
                }
            }
            catch { }
            finally
            {
                readJson.Dispose();
                maintenanceService.Dispose();
            }
        }
        protected void OnPropertyChanged(string propertyName) =>
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
