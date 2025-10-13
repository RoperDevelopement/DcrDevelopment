using AlanoClubInventory.Interfaces;
using AlanoClubInventory.Models;
using AlanoClubInventory.Utilites;
using AlanoClubInventory.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Scmd = AlanoClubInventory.SqlServices;
namespace AlanoClubInventory.ViewModels
{
   public class DailyTillReceiptViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private   ObservableCollection<AlanoClubTillPricesModel> tillPrices = new ObservableCollection<AlanoClubTillPricesModel>();
        private ICommand upDateTotalPrice;
        private float totalSales;
        private string totalTillReceipts;
        private string clubDeposit;
        private float tillOverShort;
        private DateTime recepitDate;
        private ICommand updateTillReceiptsCommand;
        public DailyTillReceiptViewModel()
        {
            GetConnectionStr();
            LoadTillPrices();
            TotalSales = 0.00f;
            TotalTillReceipts = "0.00";
            ClubDeposit = "0.00";
            CurrentTillRecDate();
           //  RecepitDate = DateTime.Now.ToString("MM/dd/yyyy") == DateTime.Now.ToString("MM/dd/yyyy") ? DateTime.Now : DateTime.Now;
        }
        public DateTime RecepitDate
        {
            get => recepitDate;

                 
            
            set
            {
                recepitDate = value;
                OnPropertyChanged(nameof(RecepitDate));
                CheckDailyTillDate();
                //  addCategory.Execute(true);
            }
        }
        public ICommand UpdateTillReceiptsCommand
        {
            get
            {
                if (updateTillReceiptsCommand == null)
                {
                    updateTillReceiptsCommand = new RelayCommd(UpdateTillReceipts, param => CanUpdateTotalPrice());
                }
                return updateTillReceiptsCommand;
            }
        }
        public ObservableCollection<AlanoClubTillPricesModel> TillPrices
        {
            get
            {
                if (tillPrices == null)
                    tillPrices = new ObservableCollection<AlanoClubTillPricesModel>();

                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return tillPrices;
            }
            set
            {
                tillPrices = value;
                OnPropertyChanged(nameof(TillPrices));
                //  addCategory.Execute(true);
            }
        }
        public string ClubDeposit
        {
            get => clubDeposit;
            set
            {
                clubDeposit = value;
                OnPropertyChanged(nameof(ClubDeposit));
            }
        }
        public float TillOverShort
        {
            get => tillOverShort;
            set
            {
                tillOverShort = value;
                OnPropertyChanged(nameof(TillOverShort));
            }
        }
        private async void CurrentTillRecDate()
        {
            RecepitDate = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetAlanoClubTillReciptSDate(SqlConnectionStr,Scmd.SqlConstProp.SPGetAlanoClubDailyTillReceiptDate);
        }
        private async void LoadTillPrices()
        {
            try
            {
             
               
                var prices = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetACProductsList<AlanoClubTillPricesModel>(SqlConnectionStr,Scmd.SqlConstProp.SPGetDailyTillReceipts);
                if (prices != null && prices.Count > 0)
                {
                    TillPrices = new ObservableCollection<AlanoClubTillPricesModel>(prices);
                }
                 ClubDeposit= "0.00";
                 TillOverShort = 0.00f;
               TotalSales = 0.00f;
               TotalTillReceipts = "0.00";
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine($"Error loading till prices: {ex.Message}");
            }
        }
        
        private string SqlConnectionStr { get; set; }
        private async void GetConnectionStr()
        {
           
            ReadJsonFile readJson = new ReadJsonFile();
            var appSettings = readJson.GetJsonData<SqlServerConnectionStrings>(nameof(SqlServerConnectionStrings)).Result;
            if (appSettings != null)
            {
                SqlConnectionStr = appSettings.AlanoClubSqlServer;
            }
        }
        public ICommand UpDateTotalPrice
        {
            get
            {
                if (upDateTotalPrice == null)
                {
                    upDateTotalPrice = new RelayCommd(UpdateTotalPrice, param => CanUpdateTotalPrice());
                }
                return upDateTotalPrice;
            }
        }
        private async void CheckDailyTillDate()
        {
            var res = await Scmd.AlClubSqlCommands.SqlCmdInstance.CheckDailyTillDate(SqlConnectionStr, Scmd.SqlConstProp.SPAlanoClubCheckTillDate, RecepitDate.ToString("yyyy-MM-dd"));
            if (res)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Daily Till Receipts for this date {RecepitDate.ToString("MM-dd-yyyy")} already exists. Please choose another date.", "Information", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                RecepitDate = recepitDate.AddDays(1);
            }
        }
        private async Task AddTillReceipts(AlanoClubTillPricesModel alanoClubTillPrices)
        { 
            await Scmd.AlClubSqlCommands.SqlCmdInstance.AlanoCLubDailyTillRecepits(SqlConnectionStr,alanoClubTillPrices,Scmd.SqlConstProp.SPDailyTellReceipts);
            await Task.CompletedTask;
        }
        private void UpdateTotalPrice(object pam)
        {
            // Add logic to update the total price
            // For example, recalculate the total based on selected items
            //decimal totalPrice = 0m;
            //foreach (var item in TillPrices)
            //{
            //    totalPrice += item.ClubPrice; // Assuming ClubPrice is a decimal property
            //}
            // You can then set this totalPrice to a property if needed
            // TotalPrice = totalPrice; // Uncomment and implement TotalPrice property if needed
        }
        public float TotalSales
            {
            get => totalSales;
            set
            {
                totalSales = value;
                OnPropertyChanged(nameof(TotalSales));
            }
        }

        public string TotalTillReceipts
            {
            get => totalTillReceipts;
            set
            {
                totalTillReceipts = value;
                OnPropertyChanged(nameof(TotalTillReceipts));
            }
        }
        private bool CanUpdateTotalPrice()
        {
            // Add logic to determine if the command can execute
            return true; // For now, always return true
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private async void AddDailyTillReceipts()
        {
            AlanoCLubDailyTillTapeModel cLubDailyTillTapeModel = new AlanoCLubDailyTillTapeModel {ID=0,DateCreated=RecepitDate,DailyTillTotal=TotalSales,DailyTillTape= float.Parse(TotalTillReceipts),Depsoit=float.Parse(ClubDeposit) };
            await Scmd.AlClubSqlCommands.SqlCmdInstance.AddDailyTapTotal(SqlConnectionStr, Scmd.SqlConstProp.SPAlanoCLubDailyTapeTill, cLubDailyTillTapeModel);
            await Task.CompletedTask;
        }
        
        private async void UpdateTillReceipts(object pam)
        {
           if(await Utilites.ALanoClubUtilites.ShowMessageBoxResults("Are you sure you want to add these daily till receipts?", "Confirmation", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.No)
                return;

            var lv = pam as System.Windows.Controls.ListView;
            
            if (lv == null) return;
            foreach (var items in TillPrices)
            {
                if (items.DailyProductTotal > 0)
                {
                    items.DateCreated = RecepitDate;
                    await AddTillReceipts(items);
                }
                //f(iten is AlanoClubTillPricesModel item)i

            }
            AddDailyTillReceipts();
            LoadTillPrices();
            RecepitDate = RecepitDate.AddDays(1);

            // Add logic to update the total price
            // For example, recalculate the total based on selected items
            //decimal totalPrice = 0m;
            //foreach (var item in TillPrices)
            //{
            //    totalPrice += item.ClubPrice; // Assuming ClubPrice is a decimal property
            //}
            // You can then set this totalPrice to a property if needed
            // TotalPrice = totalPrice; // Uncomment and implement TotalPrice property if needed
            await Task.CompletedTask;
        }
    }
}
