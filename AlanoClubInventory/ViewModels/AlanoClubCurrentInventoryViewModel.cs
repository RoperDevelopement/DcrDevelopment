using AlanoClubInventory.Models;
using AlanoClubInventory.Reports;
using AlanoClubInventory.Reports;
using AlanoClubInventory.Utilites;
using Microsoft.ReportingServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Scmd = AlanoClubInventory.SqlServices;

namespace AlanoClubInventory.ViewModels
{
    public class AlanoClubCurrentInventoryViewModel : INotifyPropertyChanged
    {
        private ICommand printReport;
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<AlanoClubCurrentInventoryModel> inventory;
        private bool isGeneratingInvSheet;
        private ICommand getInvSheet;
        private ICommand upDateInventory;
        private string buttonText;
        private bool isInvUpDated;
        private RichTextBox reportContentTxt;
        private FlowDocument reportContent;
        private ObservableCollection<ItemListModel> itemList;
        private readonly CreateInvSheetFlowDocument createInvSheetFlowDocument = new CreateInvSheetFlowDocument();
        private ItemListModel selectedDate;
        private bool isListViewRep;
        private bool isOldRep;
        private ICommand printOldInv;
        private bool isProgressBar;
        private double progressValue;
        public AlanoClubCurrentInventoryViewModel()
        {

            GetConnectionStr();
            IsProgressBar = true;
            GetInventory();
            IsInvUpDated = true;
            IsListViewRep = true;
            IsOldRep = false;
            ButtonText = "Print Old Inventory Report";
            

        }
        public double ProgressValue
        {
            get => progressValue;
            set
            {
                if (progressValue != value)
                {
                    progressValue = value;
                    OnPropertyChanged(nameof(ProgressValue));
                }
            }
        }
        public bool IsProgressBar
        {
            get => isProgressBar; 
            set {
                isProgressBar = value;
                 OnPropertyChanged(nameof(IsProgressBar)); 
            }
           }
        public string ButtonText
        {
            get => buttonText;
            set
            {

                buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }
        public bool IsListViewRep
        {
            get => isListViewRep;
            set
            {
                isListViewRep = value;
                OnPropertyChanged(nameof(IsListViewRep));
            }
        }
        public bool IsOldRep
        {
            get => isOldRep;
            set
            {
                isOldRep = value;
                OnPropertyChanged(nameof(IsOldRep));
            }
        }
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
                GetOldInvSheet();
                OnPropertyChanged(nameof(SelectedDate));
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

        public FlowDocument ReportContent
        {
            get => reportContent;
            set
            {
                if (reportContent != value)
                {
                    reportContent = value;
                    OnPropertyChanged(nameof(ReportContent));
                }
            }
        }

        public ICommand PrintReport
        {
            get
            {
                if (printReport == null)
                {
                    printReport = new RelayCommdNoPar(PrintReportExecute, param => CanGenerateReport());
                }
                return printReport;
            }
        }
        public ICommand PrintOldInv
        {
            get
            {
                if (printOldInv == null)
                {
                    printOldInv = new RelayCommdNoPar(GetOldInvDates, param => CanGenerateReport());
                }
                return printOldInv;
            }
        }

        public ICommand UpDateInventory
        {
            get
            {
                if (upDateInventory == null)
                {
                    upDateInventory = new RelayCommdNoPar(InventoryUpdate, param => CanGenerateReport());
                }
                return upDateInventory;
            }
        }
        public async Task ClearReportContent()
        {
            if (ReportContent != null)
            {
                ReportContent.BeginInit();
                ReportContent.Blocks.Clear();
                ReportContent.EndInit();
                IsGeneratingInvSheet = false;
            }
        }

        public RichTextBox ReportContentTxt
        {
            get => reportContentTxt;
            set
            {
                if (reportContentTxt != value)
                {
                    reportContentTxt = value;
                    OnPropertyChanged(nameof(ReportContentTxt));
                }
            }
        }
        public bool IsInvUpDated
        {
            get => isInvUpDated;
            set
            {
                isInvUpDated = value;
                OnPropertyChanged(nameof(IsInvUpDated));
            }

        }
        private string SqlConnectionStr { get; set; }
        private async void GetInventory()
        {
            ProgressValue = 10.00;
            IsGeneratingInvSheet = false;
            GetConnectionStr();
            if ((Inventory != null) && Inventory.Count > 0)

            {
                Inventory.Clear();
            }
            ProgressValue += 10.00;
            var inv = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetACProductsList<AlanoClubCurrentInventoryModel>(SqlConnectionStr, Scmd.SqlConstProp.SPGetAlanoCLubCurrentInventory);
            ProgressValue = 50.00;
            if ((inv != null) && (inv.Count > 0))
            {
                foreach (var item in inv)
                {
                    ProgressValue = 10.00;
                    if (item.ID == 0)
                    {
                        continue;
                    }
                    Regex regex = new Regex(AlanoCLubConstProp.RegXInv.ToLower(), RegexOptions.IgnoreCase);

                    // Find matches
                    // MatchCollection matches = regex.Matches(AlanoCLubConstProp.RegXInv.ToLower());
                    bool matches = regex.IsMatch(item.ProductName.ToLower());
                    if (matches)
                    {
                        continue;
                    }
                    Inventory?.Add(new AlanoClubCurrentInventoryModel
                    {
                        ID = item.ID,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        InStock = item.InStock,
                        ItemsSold = item.ItemsSold,
                        InventoryCurrent = 0,
                        NewCount = 0
                    });
                }
            }
            ProgressValue = 100.00;
            Task.Delay(2000)
;            IsProgressBar = false;
        }
        public bool IsGeneratingInvSheet
        {
            get => isGeneratingInvSheet;

            set
            {
                isGeneratingInvSheet = value;
                OnPropertyChanged(nameof(IsGeneratingInvSheet));
            }
        }
        private async void GetConnectionStr()
        {

            SqlConnectionStr = Utilites.ALanoClubUtilites.GetSqlConnectionStrings(Utilites.ALanoClubUtilites.AlanoClubDatabaseName);

        }
        public ObservableCollection<AlanoClubCurrentInventoryModel> Inventory
        {

            get
            {
                if (inventory == null)
                    inventory = new ObservableCollection<AlanoClubCurrentInventoryModel>();
                //        //delCategory.Execute(true);
                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return inventory;
            }
            set
            {
                inventory = value;
                OnPropertyChanged(nameof(Inventory));
            }
        }
        private void PrintReportExecute()
        {
            if (ReportContent != null)
            {
                if (AlanoClubInventory.Reports.PrintHelper.PrintFlowDocument(ReportContent))
                {
                    IsGeneratingInvSheet = false;

                }
            }
        }
        public ICommand GetInvSheet
        {
            get
            {
                if (getInvSheet == null)
                {
                    getInvSheet = new RelayCommdNoPar(CreateInvSheet, param => CanGenerateReport());
                }
                return getInvSheet;
            }
        }
        
        private async void GetOldInvDates()
        {
            OldInvDates();
        }

        private async void OldInvDates()
        {
            if (IsOldRep)
            {
                ButtonText = "Print Old Inventory Report";
                GetInventory();
                IsInvUpDated = true;
                IsListViewRep = true;
                IsOldRep = false;
            }
            else
            {
                ProgressValue = 10;
                 IsProgressBar = true;
                var oldDates = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetACProductsList<ALanoClubInventoryDateModel>(SqlConnectionStr, Scmd.SqlConstProp.SPGetInventoryDates);
                if (oldDates != null)
                {
                    if (ItemsList.Count > 0)
                        ItemsList.Clear();
                    ItemsList.Add(new ItemListModel { Label = "Select a Date", Value = "0" });
                    ProgressValue += 10;
                    foreach (var oldDate in oldDates)
                    {
                        ProgressValue += 10;
                        ItemsList.Add(new ItemListModel { Label = oldDate.DateInventory?.ToString("MM-dd-yyyy"), Value = oldDate.DateInventory?.ToString("MM-dd-yyyy") });
                    }
                    IsInvUpDated = false;
                    IsListViewRep = false;
                    IsOldRep = true;
                    var i = ItemsList.FirstOrDefault(i => i.Label == "Select a Date");
                    SelectedDate = i;
                    ButtonText = "UpDate Inventory";
                    ProgressValue = 100;
                    Task.Delay(1000);
                    IsProgressBar = false;
                }
            }
            }
        private bool CanExecuteAction()
        {
            return true;
        }
        private bool CanGenerateReport()
        {
            return true;
        }
        private bool CanExecuteAction(object parm)
        {
            return parm is CategoryModel;
        }
        private async void GetOldInvSheet()
        {
            if ((SelectedDate !=null) && (SelectedDate.Value != null) && (SelectedDate.Value !="0"))
            {
                IsProgressBar = true;
                ProgressValue = 10;
                IList<StoredParValuesModel> storedPars = new List<StoredParValuesModel>();
                storedPars.Add(new StoredParValuesModel { ParmaValue = SelectedDate.Value, ParmaName = Scmd.SqlConstProp.SPParmaDate });

              
                var ilInv = await Scmd.AlClubSqlCommands.SqlCmdInstance.CallStoreProdByParmaters<AlanoClubCurrentInventoryModel>(SqlConnectionStr, Scmd.SqlConstProp.SPALanoClubRePrintInventory, storedPars);
                ProgressValue = 20;
                  CreateInvReport(ilInv);
                
            }
        }
       private async void CreateInvSheet()
        {
            ProgressValue += 10;
            CreateInvReport(Inventory.ToList());
        }
        private async void CreateInvReport(IList<AlanoClubCurrentInventoryModel> inventoryViewModels)
        {
            ProgressValue += 10;
            if (inventoryViewModels.Count == 0)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error no inventroy", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ProgressValue += 20;
            await ClearReportContent();
            var title = $"Alanco CLub Current Invertory taken on {DateTime.Now.ToString("MM-dd-yyyy")}";
            ProgressValue += 20;
            ReportContent = await createInvSheetFlowDocument.CreateInventorySheet(title, inventoryViewModels);
            ProgressValue += 100;
            IsGeneratingInvSheet = true;
            Task.Delay(1000);
            IsProgressBar = false;
            //TextRange textRange = new TextRange(ReportContent.ContentStart, ReportContent.ContentEnd);
        }
        private async void InventoryUpdate()
        {
            // var ilist = Inventory.ToList();
            // var blankCounts = ilist.FirstOrDefault(p => p.InventoryCurrent == 0);
            // if((blankCounts != null))
            // { 
            //     Utilites.ALanoClubUtilites.ShowMessageBox($"Error {blankCounts.ProductName} Missing Count In Stock","Error New Counts",MessageBoxButton.OK, MessageBoxImage.Error);    
            //     return;
            //}
            IsProgressBar = true;
            ProgressValue = 10;
            await UpInv();
            ProgressValue = 100;
            Task.Delay(1000);
            IsProgressBar = false;

        }
        private async Task UpInv()
        {
            if (Inventory.Count == 0)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error no inventroy", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            foreach (var item in Inventory)
            {
                if (item != null)
                {
                    ProgressValue += 20;
                    AlanoClubCurrentInventoryModel alanoClubCurrent = new AlanoClubCurrentInventoryModel
                    {
                        ID = item.ID,
                        InventoryCurrent = item.InventoryCurrent,
                        Quantity = item.Quantity,
                        InStock = item.InStock,
                        ItemsSold = item.ItemsSold,


                    };
                    await Scmd.AlClubSqlCommands.SqlCmdInstance.UpDateInventory(SqlConnectionStr, Scmd.SqlConstProp.SPALanoClubInventroyCount, alanoClubCurrent);
                    ProgressValue += 5;
                }
            }
            //  GetInventory();
            IsInvUpDated = false;
            await Task.CompletedTask;
        }
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    }
}
