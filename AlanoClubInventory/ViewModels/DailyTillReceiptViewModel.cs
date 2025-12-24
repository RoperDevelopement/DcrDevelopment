using AlanoClubInventory.Interfaces;
using AlanoClubInventory.Models;
using AlanoClubInventory.Utilites;
using AlanoClubInventory.Views;
using LiveCharts.Maps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
        private bool isProgressBar;
        private ICommand updateTillReceiptsCommand;
        private ICommand oldTillReceiptsCommand;
        private bool isCurrentRec;
        private bool isOldRec;
        private ItemListModel selectedDate;
        private ObservableCollection<ItemListModel> itemList;
        private ObservableCollection<ItemListModel> oldRecList;
        private bool isListView;
        private ItemListModel selectedItemDate;
        private bool isButtonVis;
        private string txtOldNewRec;
        public DailyTillReceiptViewModel()
        {
           
            IsButtonVis = false;
            IsProgressBar = true;
            GetConnectionStr();
            LoadTillPrices(Scmd.SqlConstProp.SPGetDailyTillReceipts);
            TotalSales = 0.00f;
            TotalTillReceipts = "0.00";
            ClubDeposit = "0.00";
            IsCurrentRec = true;
            IsOldRec = false;
            CurrentTillRecDate();
          
           //  RecepitDate = DateTime.Now.ToString("MM/dd/yyyy") == DateTime.Now.ToString("MM/dd/yyyy") ? DateTime.Now : DateTime.Now;
        }
        public ProgressModel Progress { get; } = new ProgressModel();
        public ItemListModel SelectedDate
        {
            get
            {
                if (selectedDate == null)
                    selectedDate = new ItemListModel();
                return selectedDate;
            }
            set
            {
                selectedDate = value;
               // GetOldInvSheet();
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
        public ObservableCollection<ItemListModel> OldRecList
        {
            get
            {
                if (oldRecList == null)
                    oldRecList = new ObservableCollection<ItemListModel>();

                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return oldRecList;
            }
            set
            {
                oldRecList = value;
                OnPropertyChanged(nameof(OldRecList));
                //  addCategory.Execute(true);
            }
        }
        public ObservableCollection<ItemListModel> ItemsList
        {
            get
            {
                if (itemList == null)
                    itemList = new ObservableCollection<ItemListModel>();

                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return itemList;
            }
            set
            {
                itemList = value;
                OnPropertyChanged(nameof(ItemsList));
                //  addCategory.Execute(true);
            }
        }
        public ItemListModel SelectedItemDate
        {
            get
            {
                if (selectedItemDate == null)
                    selectedItemDate = new ItemListModel();
                return selectedItemDate;
            }
            set
            {
                selectedItemDate = value;
                
                OnPropertyChanged(nameof(SelectedItemDate));
                LoadTillPrices(Scmd.SqlConstProp.SPGetDailyOldTillReceipts, false);

            }
        }
        private IList<AlanoClubTillPricesModel> AllItems { get; set; } = new List<AlanoClubTillPricesModel>();
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
        
        public bool IsProgressBar
        {
            get { return isProgressBar; }
            set { isProgressBar = value; OnPropertyChanged(nameof(IsProgressBar)); }
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
        public ICommand OldTillReceiptsCommand
        {
            get
            {
                if (oldTillReceiptsCommand == null)
                {
                    oldTillReceiptsCommand = new RelayCommd(TillReceiptsOld, param => CanUpdateTotalPrice());
                }
                return oldTillReceiptsCommand;
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

        
            public bool IsButtonVis
        {
            get => isButtonVis;
            set
            {
                isButtonVis = value;
                OnPropertyChanged(nameof(IsButtonVis));
            }
        }
        public bool IsListView
        {
            get => isListView;
            set
            {
                isListView = value;
                OnPropertyChanged(nameof(IsListView));
            }
        }
        public bool IsCurrentRec
        {
            get => isCurrentRec;
            set
            {
                isCurrentRec = value;
                OnPropertyChanged(nameof(IsCurrentRec));
            }
        }
        public bool IsOldRec
        {
            get => isOldRec;
            set
            {
                isOldRec = value;
                OnPropertyChanged(nameof(IsOldRec));
            }
        }
        private int OldIDRec { get; set; } = 0;
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
        
            public string TxtOldNewRec
        {
            get => txtOldNewRec;
            set
            {
                txtOldNewRec = value;
                OnPropertyChanged(nameof(TxtOldNewRec));
            }
        }
        private async void CurrentTillRecDate()
        {
            Progress.ProgressValue = 75;
            RecepitDate = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetAlanoClubTillReciptSDate(SqlConnectionStr,Scmd.SqlConstProp.SPGetAlanoClubDailyTillReceiptDate);
            Progress.ProgressValue = 100;
            Task.Delay(1000);
             IsProgressBar = false;
        }
        private async void LoadTillPrices(string storProc,bool oldPrices=true)
        {
            try
            {
                

                IList<AlanoClubTillPricesModel> prices = new List<AlanoClubTillPricesModel>();
                ClubDeposit = "0.00";
                TillOverShort = 0.00f;
                TotalSales = 0.00f;
                TotalTillReceipts = "0.00";
                Progress.ProgressValue += 20;
                TxtOldNewRec = "Edit Old Receipts";
                OldIDRec = 0;
                if (oldPrices)
                {
                    prices = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetACProductsList<AlanoClubTillPricesModel>(SqlConnectionStr, storProc);
                    AllItems = prices;
                    
                }
                else
                {
                    if ((SelectedItemDate.Value == null) || (SelectedItemDate.Value == "0"))
                        return;
                    //List<StoredParValuesModel> sp = new List<StoredParValuesModel>();
                    prices = await GetOldTillRecp(storProc);
                   // OldTillReceipts = prices;
                    TxtOldNewRec = "Add New Receipts";
                }

                if (prices != null && prices.Count > 0)
                {
                    TillPrices = new ObservableCollection<AlanoClubTillPricesModel>(prices);
                    if (!oldPrices)
                        AddMissingProducts();
                }
             

                Progress.ProgressValue = 50;
                IsProgressBar = false;
                IsListView = true;
                IsButtonVis = true;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine($"Error loading till prices: {ex.Message}");
            }
        }
        private async Task<IList<AlanoClubTillPricesModel>> GetOldPrices(string storProc)
        {
            var sp = new List<StoredParValuesModel>();
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaDate, ParmaValue = SelectedItemDate.Value });
           return(await Scmd.AlClubSqlCommands.SqlCmdInstance.CallStoreProdByParmaters<AlanoClubTillPricesModel>(SqlConnectionStr, storProc, sp));
        }
        private async Task<IList<AlanoClubTillPricesModel>> GetOldTillRecp(string storProc)
        {

            var prices = await GetOldPrices(storProc);
            //  OldTillReceipts = prices;
            var sp = new List<StoredParValuesModel>();
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaDateTillReceipt, ParmaValue = SelectedItemDate.Value });
            var oldTotals = await Scmd.AlClubSqlCommands.SqlCmdInstance.CallStoreProdByParmaters<AlanoCLubDailyTillTapeModel>(SqlConnectionStr,Scmd.SqlConstProp.SPlanoCLubOldDailyTapeTill, sp);
            if (oldTotals.Count > 0)
            {
                OldIDRec= oldTotals[0].ID;
                ClubDeposit =  ALanoClubUtilites.TruncateToTwoDecimalPlaces(oldTotals[0].Depsoit);
                TillOverShort = oldTotals[0].DailyTillTape-oldTotals[0].DailyTillTotal ;
            //    var ts = tillOverShort.ToString("0.00");
                TillOverShort = await ALanoClubUtilites.TruncateToTwoDecimalPlacesFloat(TillOverShort);
                TotalSales = await ALanoClubUtilites.TruncateToTwoDecimalPlacesFloat(oldTotals[0].DailyTillTotal);
                TotalTillReceipts = ALanoClubUtilites.TruncateToTwoDecimalPlaces(oldTotals[0].DailyTillTape);
            }
            return prices;
        }
        private async void AddMissingProducts()
        {
            foreach (var item in AllItems)
            {
                var add = TillPrices.FirstOrDefault(p=>p.ProductName == item.ProductName);
                if(add == null)
                    TillPrices.Add(item);
                //  if(!(TillPrices.Contains(item)))
                //        
            }

        }
        private string SqlConnectionStr { get; set; }
        private async void GetConnectionStr()
        {

            Progress.ProgressValue = 20;
            SqlConnectionStr = Utilites.ALanoClubUtilites.GetSqlConnectionStrings(Utilites.ALanoClubUtilites.AlanoClubDatabaseName);

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
            IsProgressBar = true;
            Progress.ProgressValue = 20;
            var res = await Scmd.AlClubSqlCommands.SqlCmdInstance.CheckDailyTillDate(SqlConnectionStr, Scmd.SqlConstProp.SPAlanoClubCheckTillDate, RecepitDate.ToString("yyyy-MM-dd"));
            if (res)
            {
                Progress.ProgressValue = 50;
                Utilites.ALanoClubUtilites.ShowMessageBox($"Daily Till Receipts for this date {RecepitDate.ToString("MM-dd-yyyy")} already exists. Please choose another date.", "Information", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                RecepitDate = recepitDate.AddDays(1);
             
            }
            
            
           LoadTillPrices(Scmd.SqlConstProp.SPGetDailyTillReceipts);
            
            
            Progress.ProgressValue = 100;
            IsProgressBar = false;
        }
        private async void AddTillReceipts(AlanoClubTillPricesModel alanoClubTillPrices)
        {

            IsProgressBar = true;
            Progress.ProgressValue = 20;
            if(IsCurrentRec)
                await Scmd.AlClubSqlCommands.SqlCmdInstance.AlanoCLubDailyTillRecepits(SqlConnectionStr,alanoClubTillPrices,Scmd.SqlConstProp.SPDailyTellReceipts);
            else
            {
                UpDateOldRecpts(alanoClubTillPrices);
            }


                Progress.ProgressValue = 100;
            IsProgressBar = false;
            await Task.CompletedTask;
        }
        private async void UpDateOldRecpts(AlanoClubTillPricesModel alanoClubTill)
        {
            var sp = new List<StoredParValuesModel>();
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaID, ParmaValue = alanoClubTill.ID.ToString()});
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaMemberITems, ParmaValue = alanoClubTill.TotalMemberSold.ToString() });
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaMemberPrice, ParmaValue = alanoClubTill.ClubPrice.ToString() });
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaDailyProductTotal, ParmaValue = alanoClubTill.DailyProductTotal.ToString() });
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaNonMemberITems, ParmaValue = alanoClubTill.TotalNonMemberSold.ToString() });
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaNonMemberPrice, ParmaValue = alanoClubTill.ClubNonMemberPrice.ToString() });
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaDateTillReceipt, ParmaValue = alanoClubTill.DateCreated.ToString() });
            if (alanoClubTill.BarItem)
                sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaInventorySales, ParmaValue = "1" });
            else
                sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaInventorySales, ParmaValue = "0" });
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaProductName, ParmaValue = alanoClubTill.ProductName.ToString() });
            
            await Scmd.AlClubSqlCommands.SqlCmdInstance.UpDateInsertWithParma(SqlConnectionStr, Scmd.SqlConstProp.SPUpdateOldDailyTellReceipts, sp);
        }
        private async void UpdateTotalPrice(object pam)
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
            IsProgressBar = true;
            Progress.ProgressValue = 20;
            AlanoCLubDailyTillTapeModel cLubDailyTillTapeModel = new AlanoCLubDailyTillTapeModel {ID=0,DateCreated=RecepitDate,DailyTillTotal=TotalSales,DailyTillTape= float.Parse(TotalTillReceipts),Depsoit=float.Parse(ClubDeposit) };

            Progress.ProgressValue = 75;
                    if (IsCurrentRec)
            {
                await Scmd.AlClubSqlCommands.SqlCmdInstance.AddDailyTapTotal(SqlConnectionStr, Scmd.SqlConstProp.SPAlanoCLubDailyTapeTill, cLubDailyTillTapeModel);
            }
                    else
            {
                cLubDailyTillTapeModel.ID = OldIDRec;
                await Scmd.AlClubSqlCommands.SqlCmdInstance.AddDailyTapTotal(SqlConnectionStr, Scmd.SqlConstProp.SPUpDateAlanoClubOldTillDrop, cLubDailyTillTapeModel);
            }
            Progress.ProgressValue = 100;
            IsProgressBar = false;
            await Task.CompletedTask;
        }
        private async void TillReceiptsOld(object parma)
        {
            IsProgressBar = true;
            Progress.ProgressValue = 20;
            IsButtonVis = false;
            
            if (IsCurrentRec)
                GetOldTillRecpts();
            else
            {
                GetCurrentTillRec();
              
            }
            


        }
        private async void GetCurrentTillRec()
        {
            Progress.ProgressValue += 20;
            
            //TillPrices.Clear();
            LoadTillPrices(Scmd.SqlConstProp.SPGetDailyTillReceipts);
            Progress.ProgressValue += 20;
            CurrentTillRecDate();

            IsProgressBar = false;
            IsCurrentRec = true;
            IsOldRec = false;
            
            
        }
        private async void GetOldTillRecpts()
        {
            IList<StoredParValuesModel> storedPars = new List<StoredParValuesModel>();
            storedPars.Add(new StoredParValuesModel { ParmaValue = "1", ParmaName = Scmd.SqlConstProp.SPParmaDate });
            var oldRecDates = await Scmd.AlClubSqlCommands.SqlCmdInstance.CallStoreProdByParmaters<ALanoClubInventoryDateModel>(SqlConnectionStr, Scmd.SqlConstProp.SPGetInventoryDates, storedPars);
            if (oldRecDates != null)
            {
                IsListView = false;
                if (OldRecList.Count > 0)
                    OldRecList.Clear();
                OldRecList.Add(new ItemListModel { Label = "Select a Date", Value = "0" });
                Progress.ProgressValue += 10;
                foreach (var oldDate in oldRecDates)
                {
                    Progress.ProgressValue += 10;
                    OldRecList.Add(new ItemListModel { Label = oldDate.DateInventory?.ToString("MM-dd-yyyy"), Value = oldDate.DateInventory?.ToString("MM-dd-yyyy") });
                }

                var i = OldRecList.FirstOrDefault(i => i.Label == "Select a Date");
                SelectedItemDate = i;

                Progress.ProgressValue = 100;
                Task.Delay(1000);
                IsProgressBar = false;
                IsCurrentRec = false;
                IsOldRec = true;
            }
        }
        private async void AddNewTillRec(ListView lv)
        {
            

            if (lv == null) return;
            foreach (var items in TillPrices)
            {
                Progress.ProgressValue += 10;
                if (items.DailyProductTotal > 0)
                {
                    items.DateCreated = RecepitDate;
                     AddTillReceipts(items);
                }
                //f(iten is AlanoClubTillPricesModel item)i

            }
        }
        private async void UpDateInsetOldRec(ListView lv)
        {
               
            if (lv == null) return;
            IList<AlanoClubTillPricesModel> oldTillReceipts = await GetOldPrices(Scmd.SqlConstProp.SPGetDailyOldTillReceipts);
            foreach (var items in TillPrices)
            {
                Progress.ProgressValue += 10;
               var orec = oldTillReceipts.Where(p=>p.ProductName==items.ProductName).ToList();
                if (orec.Count != 0)
                {
                    if ((orec[0].TotalMemberSold != items.TotalMemberSold) || (orec[0].TotalNonMemberSold != items.TotalNonMemberSold))
                            { 

                         AddTillReceipts(items);
                            }

                }
                else
                {
                    if (items.DailyProductTotal > 0)
                    {
                        items.DateCreated = DateTime.Parse(SelectedItemDate.Value);
                        items.ID = 0;
                         AddTillReceipts(items);
                    }
                }
             //   if (items.DailyProductTotal > 0)
             //  {
             //    items.DateCreated = RecepitDate;
             //   await AddTillReceipts(items);
             // }
             //f(iten is AlanoClubTillPricesModel item)i

            }
        }
        private async void UpdateTillReceipts(object pam)
        {
           if(await Utilites.ALanoClubUtilites.ShowMessageBoxResults("Are you sure you want to add these daily till receipts?", "Confirmation", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.No)
                return;
            IsProgressBar = true;
            Progress.ProgressValue = 20;

            var lv = pam as System.Windows.Controls.ListView;
            if (IsCurrentRec)
                AddNewTillRec(lv);
            else
            {
                UpDateInsetOldRec(lv);
            }

            AddDailyTillReceipts();
            LoadTillPrices(Scmd.SqlConstProp.SPGetDailyTillReceipts);
            if(IsCurrentRec)
            RecepitDate = RecepitDate.AddDays(1);
            
            Progress.ProgressValue = 100;
            IsProgressBar = false;
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
