using AlanoClubInventory.Models;
using AlanoClubInventory.Utilites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Navigation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using Scmd = AlanoClubInventory.SqlServices;
using System.Windows.Forms;
using RtfPipe.Tokens;

namespace AlanoClubInventory.ViewModels
{
    public class ChangeDatabaseViewModel :  INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IList<ItemListModel> itemsDatabases;
        private ItemListModel selectedItem;
        private ICommand exitCommand;
        public event EventHandler<bool> CloseApp;
        private string txtChangeDB;
        private bool isBackup;
        public ChangeDatabaseViewModel() 
        {
            GetSqlConnectionStr();
            GetACDatabases();
            TxtChangeDB = $"Change from Database {Utilites.ALanoClubUtilites.AlanoClubDatabaseName} to a Backup Database";
        }
        public bool IsBackup
        {
            get => isBackup;
            set
            {
                isBackup = value;
                OnPropertyChanged(nameof(IsBackup));
            }
        }
       public string TxtChangeDB
        {
            get => txtChangeDB;
            set
            {
                txtChangeDB = value;
                OnPropertyChanged(nameof(TxtChangeDB));
            }
        }
        public IList<ItemListModel> ItemList
        {
            get
            {
                if (itemsDatabases == null)
                {
                    itemsDatabases = new List<ItemListModel>();
                }
                return itemsDatabases;
            }
            set
            {
                itemsDatabases = value;
                OnPropertyChanged(nameof(ItemList));
            }
        }
        private string SqlConnectionStr {  get; set; }
        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new RelayCommd(Exit, param => true);
                }
                return exitCommand;

            }
        }
        private async void GetSqlConnectionStr()
        {
            //var conStr = readJson.GetJsonData<SqlServerConnectionStrings>(nameof(SqlServerConnectionStrings)).Result;
            SqlConnectionStr = Utilites.ALanoClubUtilites.GetSqlConnectionStrings("master");
        }
        public ItemListModel SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                ChangeDataBase();
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        private async void ChangeDataBase()
        {
            if ((SelectedItem.Value != null) && (SelectedItem.Value != "0"))
            {
                DialogResult dr = (DialogResult)await Utilites.ALanoClubUtilites.ShowMessageBoxResults($"Change to Database {SelectedItem.Value}", "Swith DataBase", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (dr == DialogResult.Yes)
                {
                    Utilites.ALanoClubUtilites.AlanoClubDatabaseName = SelectedItem.Value.Trim();
                    ExitApplicatin(true);
                }

            }
            
        }
        private async void GetACDatabases()
        {
            IsBackup = false;
             
            IList<string> dbNames = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetAlanoClubDataBaseNames(SqlConnectionStr,Utilites.AlanoCLubConstProp.AlanoClubDBName);
            if((dbNames != null) && (dbNames.Count > 0))
                   {
                if (dbNames.Count > 1)
                {
                    IsBackup = true;
                    ItemList.Add(new ItemListModel { Label = "Select a Backup DataBase", Value = "0" });
                    foreach (string dbName in dbNames)
                    {
                        if(string.Compare(dbName,Utilites.AlanoCLubConstProp.AlanoClubDBName,true) == 0)
                        {
                            ItemList.Add(new ItemListModel { Label = "Master DataBase", Value = dbName });
                            continue;
                        }

                        ItemList.Add(new ItemListModel { Label = dbName, Value = dbName });
                    }
                    var i = ItemList.FirstOrDefault(i => i.Value == "0");
                    SelectedItem = i;
                }
                else
                {
                    TxtChangeDB = "No Backup Databases Found";
                }
            }
          
        }
        private async void  Exit(object parma)
        {
            
            var navWindow = System.Windows.Application.Current.MainWindow as NavigationWindow;
            if (navWindow != null && navWindow.CanGoBack)
            {
                Utilites.ALanoClubUtilites.IsLoggin = true;

                navWindow.GoBack();
            }
        }
        protected void OnPropertyChanged(string propertyName = null) =>
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected virtual void ExitApplicatin(bool exitApp) => CloseApp?.Invoke(this, exitApp);

    }
}
