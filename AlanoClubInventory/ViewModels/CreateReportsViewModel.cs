using AlanoClubInventory.Models;


using AlanoClubInventory.Utilites;
using PdfSharp.Pdf.Content.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Linq;
using Scmd = AlanoClubInventory.SqlServices;
using AlanoClubInventory.Reports;

namespace AlanoClubInventory.ViewModels
{
    public class CreateReportsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly CreateFlowDocument createFlowDocument = new CreateFlowDocument();
        private DateTime reportEDate;
        private DateTime reportSDate;
        private ICommand createReport;
        private FlowDocument reportContent;
        private RichTextBox reportContentTxt;
        private bool isGeneratingReport;
        private int _exportProgress;
        private bool isExporting;
        private ICommand printReport;
        private bool inventorySold;
        private readonly ALanoClubReport aLanoClubReport = new ALanoClubReport();
        private bool isStwertRep;
        private string buttonRepTyep;
        private string txtTitle;
        private bool isGeneratingSWReport;
        private ObservableCollection<ItemListModel> items;
        private ObservableCollection<ItemListModel> itemsEndMonthYear;
        private ItemListModel selectedYear;
        private ItemListModel selectedEndMonthYear;
        private bool isStwertInvRep;
        private bool upDateRepDate;
        public CreateReportsViewModel()
        {
            ReportSDate = DateTime.Now;
            ReportEDate = DateTime.Now;
            IsExporting = false;
            GetConnectionStr();
            GetDatesReport();
            //  GetTillSales();
            IsGeneratingReport = false;
            StwertSettings();
            InventorySold = true;
            GetItemsByYear();
        }
        public async void StwertSettings()
        {
            IsStwertRep = true;
            ButtonRepTyep = "Create Stwert Report";
            TxtTitle = "Alano Club Stwert Report";
            IsStwertInvRep = false;
            UpDateRepDate = false;
            //   IsGeneratingSWReport = true;
        }

        public bool UpDateRepDate
        {
            get => upDateRepDate;
            set
            {

                upDateRepDate = value;
                OnPropertyChanged(nameof(UpDateRepDate));
                // Trigger journaling overlay or animation here

            }
        }
        public ItemListModel SelectedYear
        {
            get => selectedYear;
            set
            {
                if (selectedYear != value)
                {
                    selectedYear = value;
                    OnPropertyChanged(nameof(SelectedYear));
                    // Trigger journaling overlay or animation here
                }
            }
        }
        public ItemListModel SelectedEndMonthYear
        {
            get => selectedEndMonthYear;
            set
            {
                if (selectedEndMonthYear != value)
                {
                    selectedEndMonthYear = value;
                    OnPropertyChanged(nameof(SelectedEndMonthYear));
                    // Trigger journaling overlay or animation here
                }
            }
        }
        public async void GetItemsByYear()
        {
            Items = new ObservableCollection<ItemListModel>();
            var itemsSold = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetACProductsList<AlanoCLubItemsSoldByYear>(SqlConnectionStr, SqlServices.SqlConstProp.SPGetAlanClubInventoryYears);
            Items = new ObservableCollection<ItemListModel>();
            ItemsEndMonthYear = new ObservableCollection<ItemListModel>();
            if ((itemsSold != null) && (itemsSold.Count > 0))
            {
                Items.Add(new ItemListModel { Label = $"Start Month Year", Value = "0" });
                ItemsEndMonthYear.Add(new ItemListModel { Label = $"End Month Year", Value = "0" });
                foreach (var item in itemsSold)
                {
                    Items.Add(new ItemListModel { Label = $"{item.ItemsYear}", Value = item.ItemsYear.ToString() });
                    ItemsEndMonthYear.Add(new ItemListModel { Label = $"{item.ItemsYear}", Value = item.ItemsYear.ToString() });
                }
                SelectedYear = Items.FirstOrDefault(m => m.Value == "0");
                SelectedEndMonthYear = ItemsEndMonthYear.FirstOrDefault(m => m.Value == "0");
            }
             
        }
         
        public async void InventorySetting()
        {
            IsStwertRep = false;
            ButtonRepTyep = "Create Items Sold Report";
            TxtTitle = "Alano Club Items Sold Report By Month for Year";
            IsStwertInvRep = true;
            //   IsGeneratingSWReport = true;
        }
        public string TxtTitle
        {
            get => txtTitle;
            set
            {

                txtTitle = value;
                OnPropertyChanged(nameof(TxtTitle));

            }
        }
        public bool IsGeneratingSWReport
        {
            get => isGeneratingSWReport;
            set
            {

                isGeneratingSWReport = value;
                OnPropertyChanged(nameof(IsGeneratingSWReport));

            }
        }
        public bool IsStwertInvRep
        {
            get => isStwertInvRep;
            set
            {

                isStwertInvRep = value;
                OnPropertyChanged(nameof(IsStwertInvRep));

            }
        }

        public ObservableCollection<ItemListModel> Items
        {
            get => items;
            set
            {

                items = value;
                OnPropertyChanged(nameof(Items));

            }

        }
            public ObservableCollection<ItemListModel> ItemsEndMonthYear
        {
            get => itemsEndMonthYear;
            set
            {

                itemsEndMonthYear = value;
                OnPropertyChanged(nameof(ItemsEndMonthYear));

            }
        }
        public string ButtonRepTyep
        {

            get => buttonRepTyep;
            set
            {

                buttonRepTyep = value;
                OnPropertyChanged(nameof(ButtonRepTyep));

            }
        }
        public bool IsStwertRep
        {
            get => isStwertRep;
            set
            {

                isStwertRep = value;
                OnPropertyChanged(nameof(IsStwertRep));

            }
        }
        public bool InventorySold
        {
            get => inventorySold;
            set
            {
                if (inventorySold != value)
                {
                    inventorySold = value;
                    OnPropertyChanged(nameof(InventorySold));
                }
            }
        }
        public DateTime ReportSDate
        {
            get { return reportSDate; }
            set
            {
                if (reportSDate != value)
                {
                    reportSDate = value;
                    OnPropertyChanged(nameof(ReportSDate));
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
        public bool IsGeneratingReport
        {
            get => isGeneratingReport;
            set
            {
                if (isGeneratingReport != value)
                {
                    isGeneratingReport = value;
                    OnPropertyChanged(nameof(IsGeneratingReport));
                }
            }
        }
        public bool IsExporting
        {
            get => isExporting;
            set
            {
                if (isExporting != value)
                {
                    isExporting = value;
                    OnPropertyChanged(nameof(IsExporting));
                }
            }
        }
        public IList<AlanClubPrintReportModel> ReportModel { get; set; }
        public DateTime ReportEDate
        {
            get { return reportEDate; }
            set
            {
                if (reportEDate != value)
                {
                    reportEDate = value;
                    OnPropertyChanged(nameof(ReportEDate));
                }
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
        public ICommand CreateReport
        {
            get
            {
                if (createReport == null)
                {
                    createReport = new RelayCommd(GenerateReport, param => CanGenerateReport()
                    );
                }
                return createReport;
            }
        }

        public int ExportProgress
        {
            get => _exportProgress;
            set
            {
                if (_exportProgress != value)
                {
                    _exportProgress = value;
                    OnPropertyChanged(nameof(ExportProgress));
                }
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
        private async void GetConnectionStr()
        {

            SqlConnectionStr = Utilites.ALanoClubUtilites.GetSqlConnectionStrings(Utilites.ALanoClubUtilites.AlanoClubDatabaseName);

        }
        private async void RunReportStwert()
        {
            // Implement the logic to generate the report here

            try
            {

                var sDate = ReportSDate;
                var eDate = ReportEDate;

                IList<AlanoClubReportModel> rep = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetReport(SqlConnectionStr, Scmd.SqlConstProp.SPGetAlanoCLubReport, sDate, eDate);
                if((rep == null)  || (rep.Count == 0))
                 {
                    Utilites.ALanoClubUtilites.ShowMessageBox($"No Data Found for report date {sDate.ToString("MM-dd-yyyy")} - {eDate.ToString("MM-dd-yyyy")}", "No Data", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                IList<AlanoCLubDailyTillTapeModel> tillDep = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetDailyTillReport(SqlConnectionStr, Scmd.SqlConstProp.SPGetAlanoCLubDailyTapeTill, sDate, eDate);
                ReportModel = await aLanoClubReport.InitReportWithDates(sDate, eDate);
                var barItems = await aLanoClubReport.GetAsyncBarItems(rep);
                var res = await aLanoClubReport.GenerateReportAsync(barItems);
                var nonBarItems = await aLanoClubReport.GetAsyncNonBarItems(rep);
                var resCat = await aLanoClubReport.GenerateReportCatAsync(nonBarItems);
                ReportModel = await aLanoClubReport.GetTapeTotals(tillDep, ReportModel);
                ReportModel = await aLanoClubReport.AddBarItems(res, ReportModel);
                ReportModel = await aLanoClubReport.AddOtherItems(resCat, ReportModel);

                var ts = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetAlanoTotalsByYear<AlanoCLubDailyTillTapeModel>(SqlConnectionStr, Scmd.SqlConstProp.SpGetAlanoCLubTotalsTapeTill, eDate.Year);
                var totalMontlySales = await aLanoClubReport.GenerateMonthlySalesAsync(ts);
                //  var dailyTill = await aLanoClubReport.GetMonthlySales(SqlConnectionStr, Scmd.SqlConstProp.SpGetAlanoCLubTotalsTapeTill);
                string title = $"Alano Club Stwert Report From {ReportSDate.ToShortDateString()} To {ReportEDate.ToShortDateString()}";


                ReportContent = await createFlowDocument.CreateAlanoMonthlyStwwertReport(ReportModel, title, totalMontlySales);
                // TextRange textRange = new TextRange(ReportContent.ContentStart, ReportContent.ContentEnd);

                IsGeneratingReport = true;
            }
            catch (Exception ex)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
       

        public async void InventorySoldReport()
        {
            try
            {
                string repSYear = SelectedYear.Value.Replace("-", "-01-");
                string repEYear = SelectedEndMonthYear.Value.Replace("-", "-01-");
                //IList<AlanoCLubDailyTillTapeModel> tillDep = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetDailyInventoryByMonth<AlanoCLubDailyTillTapeModel>(SqlConnectionStr,
                var tm = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetReportMonthlyTotalsByYear(SqlConnectionStr, Scmd.SqlConstProp.SPGetAlanoCLubReportItemsSoldByMonth,repSYear,repEYear );
                var totalMontlySales = await aLanoClubReport.GenerateMonthlyInventorySalesAsync(tm);
                var title = $"Alano Club Items Sold Report For The Month Year {repSYear} to {repEYear}";
                ReportContent = await createFlowDocument.CreateAlanoMontlyInvReport(title, totalMontlySales);
                IsGeneratingReport = true;
            }
            catch (Exception ex)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task<bool> CheckReportYears()
        {
           int indexDash= SelectedEndMonthYear.Value.IndexOf("-");
            int eMonth = await Utilites.ALanoClubUtilites.ConvertToInt(SelectedEndMonthYear.Value.Substring(0,indexDash).Trim());
            string endYear = SelectedEndMonthYear.Value.Substring(++indexDash).Trim();
            
            indexDash = SelectedYear.Value.IndexOf("-");
            int sMonth = await Utilites.ALanoClubUtilites.ConvertToInt(SelectedYear.Value.Substring(0, indexDash).Trim());
            string sYear = SelectedYear.Value.Substring(++indexDash).Trim();
            
            if(string.Compare(sYear,endYear, StringComparison.OrdinalIgnoreCase) != 0)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error years have to be the same {sYear} and {endYear}", "Error Years", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
           }
            if (sMonth > eMonth)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error months start month {sMonth} has to be smaller then end month {eMonth}", "Error Months", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;

        }
        public async Task ClearReportContent()
        {
            if (ReportContent != null)
            {
                ReportContent.BeginInit();
                ReportContent.Blocks.Clear();
                ReportContent.EndInit();
                IsGeneratingReport = false;
            }
        }
        public async void GenerateReport(object parameter)
        {
            try
            {
                ClearReportContent();
                // ReportContent.BeginInit();
            
                

                //  ReportContent.EndInit();
                if (InventorySold)
                {
                    if (ReportSDate > ReportEDate)
                    {
                        Utilites.ALanoClubUtilites.ShowMessageBox($"Error Report dates End Date Invalid {ReportEDate.ToString("MM-dd-yyyy")} has to be bigger or equal to Start Date {ReportSDate.ToString("MM-dd-yyyy")}", "Invalid Report Dates", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (ReportSDate.Year != ReportEDate.Year)
                    {
                        Utilites.ALanoClubUtilites.ShowMessageBox($"Error report years have to be the same {ReportSDate.Year} and {ReportEDate.Year}", "Error Years", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    RunReportStwert();
                }
                    
                else
                {
                    //var repYear = await Utilites.ALanoClubUtilites.ConvertToInt(SelectedYear.Value);
                    if ((SelectedEndMonthYear.Value == "0"))
                    {
                        Utilites.ALanoClubUtilites.ShowMessageBox($"Error Invalid End Month Year", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if ((SelectedYear.Value == "0"))
                    {
                        Utilites.ALanoClubUtilites.ShowMessageBox($"Error Invalid Start Month Year", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if(await CheckReportYears())
                        InventorySoldReport();
                }


            }
            catch (Exception ex)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //  Utilites.PrintHelper.PrintFlowDocument(ReportContent);
            //   ReportContentTxt.Document = ReportContent;
            // Implement the logic to generate the report here
            //  Scmd.AlClubSqlCommands.SqlCmdInstance.GenerateReport(SqlConnectionStr, Scmd.SqlConstProp.SPGenerateReport, ReportSDate, ReportEDate);
        }
        private void PrintReportExecute()
        {
            if (ReportContent != null)
            {
                if (PrintHelper.PrintFlowDocument(ReportContent,5))
                {
                    IsGeneratingReport = false;
                    if ((isStwertRep) && (UpDateRepDate))
                        UpDateReportDate();
                }
            }
        }
        private async void GetDatesReport()
        {
            var eDate = DateTime.Now;
            var sDate = DateTime.Now;
            Scmd.AlClubSqlCommands.SqlCmdInstance.GetReportDate(SqlConnectionStr, Scmd.SqlConstProp.SPGetDateReoprtLastRan, ref sDate, ref eDate);
            ReportEDate = eDate;
            ReportSDate = sDate;
        }
        private async void UpDateReportDate()
        {
            await Scmd.AlClubSqlCommands.SqlCmdInstance.UpdateReportRundDate(SqlConnectionStr, Scmd.SqlConstProp.SPGetDateReoprtLastRan, ReportEDate);
        }
        private bool CanGenerateReport()
        {
            // Add logic to determine if the command can execute
            return true; // For now, always return true
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
