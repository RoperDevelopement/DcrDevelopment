using AlanoClubInventory.Models;
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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Documents;

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
        private RichTextBox reportContentTxt;
        private FlowDocument reportContent;
        private readonly CreateInvSheetFlowDocument createInvSheetFlowDocument = new CreateInvSheetFlowDocument();
        public AlanoClubCurrentInventoryViewModel()
        {

            GetInventory();
            IsGeneratingInvSheet = false;
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
        private string SqlConnectionStr { get; set; }
        private async void GetInventory()
        {
            GetConnectionStr();
            var inv = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetACProductsList<AlanoClubCurrentInventoryModel>(SqlConnectionStr, Scmd.SqlConstProp.SPGetAlanoCLubCurrentInventory);
            if ((inv != null) && (inv.Count > 0))
            {
                foreach (var item in inv)
                {
                    Regex regex = new Regex(AlanoCLubConstProp.RegXInv.ToLower(), RegexOptions.IgnoreCase);

                    // Find matches
                    // MatchCollection matches = regex.Matches(AlanoCLubConstProp.RegXInv.ToLower());
                    bool matches = regex.IsMatch(item.ProductName.ToLower());
                    if (matches)
                    {
                        continue;
                    }
                    Inventory.Add(new AlanoClubCurrentInventoryModel
                    {
                        ID = item.ID,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        InStock = item.InStock,
                        ItemsSold = item.ItemsSold,
                        InventoryCurrent = 0,
                        NewCount=0
                    });
                }
            }
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

            ReadJsonFile readJson = new ReadJsonFile();
            var appSettings = readJson.GetJsonData<SqlServerConnectionStrings>(nameof(SqlServerConnectionStrings)).Result;
            if (appSettings != null)
            {
                SqlConnectionStr = appSettings.AlanoClubSqlServer;
            }
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
                if (Utilites.PrintHelper.PrintFlowDocument(ReportContent))
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
        private async void CreateInvSheet()
        {
            await ClearReportContent();
            var title = $"Alanco CLub Current Invertory taken on {DateTime.Now.ToString("MM-dd-yyyy")}";
            ReportContent = await createInvSheetFlowDocument.CreateInventorySheet(title, Inventory.ToList());
            IsGeneratingInvSheet = true;
            //TextRange textRange = new TextRange(ReportContent.ContentStart, ReportContent.ContentEnd);
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
     

}
}
