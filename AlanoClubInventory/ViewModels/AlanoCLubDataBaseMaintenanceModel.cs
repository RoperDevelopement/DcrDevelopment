using AlanoClubInventory.Utilites;
using PdfSharp.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Scmd=AlanoClubInventory.SqlServices;

namespace AlanoClubInventory.ViewModels
{
    public class AlanoCLubDataBaseMaintenanceModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ICommand backUpDataBaseCommand;
        private ICommand purgeDataBaseCommand;
        private ICommand checkDatabaseIntegrityCommand;
        private ICommand checkDatabaseOptimizeCommand;
        private ICommand rebuildIndexesCommand;
        private ICommand clearCachecCommand;
        private ICommand clearLogCommand;
        private ICommand restoreDataBaseCommand;
        private bool isPurgeDataBase;
        Scmd.AlanoClubDatabaseMaintenanceService maintenanceService = new Scmd.AlanoClubDatabaseMaintenanceService();
        private readonly Scmd.SqlLogMaintenanceService sqlLogMaintenance = new Scmd.SqlLogMaintenanceService();
        private string sBMessage;
        public AlanoCLubDataBaseMaintenanceModel()
        {
            sqlLogMaintenance.LogsCleared += (s, msg) => AddMessage(msg);
            sqlLogMaintenance.ErrorOccurred += (s, msg) => AddMessage("Error"+msg);
            sqlLogMaintenance.StatusUpdated += (s, msg) => AddMessage("Status" + msg);
            maintenanceService.IsProcessRunning += (s, isRunning) => ProcessRunning(isRunning);
            maintenanceService.Message += (s, message) => ProcessMessage(message);
            
            IsRunning = false;
            SBMessage = "Ready:";
            GetMinYear();
        }
        private int MinYear {  get; set; }
        public bool IsPurgeDataBase
        {
            get => isPurgeDataBase;
            set
            {
                isPurgeDataBase = value;
                OnPropertyChanged(nameof(IsPurgeDataBase));
            }
        }
         
            
        public  string SBMessage
        {
            get => sBMessage;
            set
            { 
                sBMessage = value; 
                OnPropertyChanged(nameof(SBMessage));
            }
        }
        private bool IsRunning { get; set; }
        public string StatusMessage { get; set; }
        public ICommand RestoreDataBaseCommand
        {
            get
            {
                if (restoreDataBaseCommand == null)
                {
                    restoreDataBaseCommand = new RelayCommdNoPar(RestoreDataBase, param => true);
                }
                return restoreDataBaseCommand;
            }
        }
        public ICommand ClearLogCommand => new RelayCommd(async param =>
        {
            if (IsRunning)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox("Another Process is Running..", "Running", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            await sqlLogMaintenance.ClearLogsAsync(Utilites.AlanoCLubConstProp.AlanoClubDBName);
        });
       



        public ICommand ClearCachecCommand
        {
            get
            {
                if (clearCachecCommand == null)
                {
                    clearCachecCommand = new RelayCommdNoPar(ClearCachec, param => true);
                }
                return clearCachecCommand;
            }
        }
        public ICommand RebuildIndexesCommand
        {
            get
            {
                if (rebuildIndexesCommand == null)
                {
                    rebuildIndexesCommand = new RelayCommdNoPar(RebuildIndexes, param => true);
                }
                return rebuildIndexesCommand;
            }
        }
        public ICommand PurgeDataBaseCommand
        {
            get
            {
                if (purgeDataBaseCommand == null)
                {
                    purgeDataBaseCommand = new RelayCommdNoPar(PurgeDataBase, param => true);
                }
                return purgeDataBaseCommand;
            }
        }
        private void AddMessage(string message)
        {
            // Append to collection (UI will update automatically)
            //   LogMessages.Add(message);
            if (message.ToLower().StartsWith("err"))
                            
            
            Utilites.ALanoClubUtilites.ShowMessageBox(message, "Log Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public ICommand CheckDatabaseOptimizeCommand
        {
            get
            {
                if (checkDatabaseOptimizeCommand == null)
                {
                    checkDatabaseOptimizeCommand = new RelayCommdNoPar(CheckDatabaseOptimize, param => true);
                }
                return checkDatabaseOptimizeCommand;
            }
        }
        public ICommand CheckDatabaseIntegrityCommand
        {
            get
            {
                if (checkDatabaseIntegrityCommand == null)
                {
                    checkDatabaseIntegrityCommand = new RelayCommdNoPar(CheckDatabaseIntegrity, param => true);
                }
                return checkDatabaseIntegrityCommand;
            }
        }
        public ICommand BackUpDataBaseCommand
        {
            get
            {
                if (backUpDataBaseCommand == null)
                {
                    backUpDataBaseCommand = new RelayCommdNoPar(BackUpDataBase, param => true);
                }
                return backUpDataBaseCommand;
            }
        }
        private async void BackUpDataBase()
        {
            try
            {
                if (IsRunning)
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox("Another Process is Running..", "Running", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //await maintenanceService.BackupDataBase();
                await maintenanceService.BackupDataBase();
                Utilites.ALanoClubUtilites.ShowMessageBox("Database Backup Created Successfully.","Backup",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error in BackUpDataBase: {ex.Message}","Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
        private async void PurgeDataBase()
        {
            try
            {
                if (IsRunning)
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox("Another Process is Running..", "Running", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
               
                //await maintenanceService.BackupDataBase();
                await maintenanceService.CreateNewDatabase();
                Utilites.ALanoClubUtilites.ShowMessageBox("Database Backup Created Successfully.", "Backup", MessageBoxButton.OK, MessageBoxImage.Information);
               
            }
            catch (Exception ex)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error in BackUpDataBase: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void CheckDatabaseIntegrity()
        {
            try
            {
                if (IsRunning)
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox("Another Process is Running..", "Running", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                //await maintenanceService.BackupDataBase();
                string isIntact= await maintenanceService.CheckDatabaseIntegrity();
                if (string.Equals(isIntact, "INTEGRITY_OK",StringComparison.OrdinalIgnoreCase))
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox("Database Integrity Check Passed.", "Database Integrity", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox($"Database Integrity Check Failed. Message {isIntact}", "Database Integrity", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error in CheckDatabaseIntegrity: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void CheckDatabaseOptimize()
        {
            try
            {
                if (IsRunning)
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox("Another Process is Running..", "Running", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                    
                //await maintenanceService.BackupDataBase();
                var queryTime = await maintenanceService.CheckDatabaseOptimize(Scmd.SqlConstProp.SPCheckDatabaseOptimize);
                if (queryTime < 0)
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox("Database Optimization Failed.", "Database Optimization", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else if (queryTime == 0)
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox("Database Optimization Good.", "Database Optimization", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (queryTime >=240 )
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox($"Database Optimization Completed in {queryTime.ToString()} secs. Need to check Database", "Database Optimization", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox($"Database Optimization Completed in {queryTime.ToString()} secs.", "Database Optimization", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                    
            }
            catch (Exception ex)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error in CheckDatabaseOptimize: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void RebuildIndexes()
        {
            try
            {
                if (IsRunning)
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox("Another Process is Running..", "Running", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var stopwatch = Stopwatch.StartNew();
                 
                //await maintenanceService.BackupDataBase();
                await maintenanceService.RebuildIndexes();
                stopwatch.Stop();
                Utilites.ALanoClubUtilites.ShowMessageBox($"Database Indexes Rebuilt Successfully in {stopwatch.Elapsed.TotalSeconds} secs", "Rebuild Indexes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error in RebuildIndexes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void ClearCachec()
        {
            try
            {
                if (IsRunning)
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox("Another Process is Running..", "Running", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                //await maintenanceService.BackupDataBase();
                await maintenanceService.ClearCache();
                Utilites.ALanoClubUtilites.ShowMessageBox("Database Cache Cleared Successfully.", "Clear Cache", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error in ClearCachec: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void GetMinYear()
        {
            IsPurgeDataBase = true;
           MinYear =await maintenanceService.GetMinYear();
            if((DateTime.Now.Month >= 6) && (MinYear != DateTime.Now.Year)) 
            {
                IsPurgeDataBase = true;
            }
        }
       private async void RestoreDataBase()
        {
           await maintenanceService.RestoreDataBase();
        }
        private void ProcessRunning(bool running)
        {
            IsRunning = running; 
        }
        private void ProcessMessage(string message)
        {
            SBMessage=message;
        }
        protected void OnPropertyChanged(string propertyName) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
