using AlanoClubInventory.Models;
using AlanoClubInventory.Reports;
using AlanoClubInventory.Utilites;
using Microsoft.ReportingServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Scmd = AlanoClubInventory.SqlServices;
using AlanoClubInventory.Reports;
namespace AlanoClubInventory.ViewModels
{
    public class ProfitLossViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<AlanoCLubProfitLossModel> profitLosses;
        private bool isGeneratingReport;
        private bool isRunningRep;
        private ICommand printReport;
        private FlowDocument reportContent;
        private RichTextBox reportContentTxt;
        private ICommand createReport;
        private readonly CreateProfitLossReport createProfitLoss = new CreateProfitLossReport();
        public ProfitLossViewModel()
        {
            GetConnectionStr();
            GetProcucts();
           
        }

        private string ExcludeInventory {  get; set; }
        public ICommand CreateReport
        {
            get
            {
                if (createReport == null)
                {
                    createReport = new RelayCommd(GenerateReport, param => CanGenerateReport());
                }
                return createReport;
            }
        }

        public ICommand PrintReport
        {
            get
            {
                if (printReport == null)
                {
                    printReport = new RelayCommd(PrintReportExecute, param => CanGenerateReport());
                }

                return printReport;
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
        public ObservableCollection<AlanoCLubProfitLossModel> ProfitLosses
        {
            get
            {
                if (profitLosses == null)
                    profitLosses = new ObservableCollection<AlanoCLubProfitLossModel>();

                //   doSomething = new RelayCommandNew<CategoryModel>(ExecuteMyButtonLogic, CanExecuteAddEdit);

                return profitLosses;
            }
            set
            {
                profitLosses = value;
                OnPropertyChanged(nameof(ProfitLosses));
                //  addCategory.Execute(true);
            }
        }

        public bool IsGeneratingReport
        {
            get => isGeneratingReport;
            set
            {
                isGeneratingReport = value;
                OnPropertyChanged(nameof(IsGeneratingReport));
            }
        }
        public bool IsRunningRep
        {
            get => isRunningRep;
            set
            {
                isRunningRep = value;
                OnPropertyChanged(nameof(IsRunningRep));
            }
        }
        private string SqlConnectionStr { get; set; }
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
            ClearReportContent();
            ReportContent = await createProfitLoss.AlanoClubCreateProfitLossReport("Alano CLub Profit BarItems Report", ProfitLosses.ToList(),ExcludeInventory);
            IsRunningRep = false;
            IsGeneratingReport = true;
        }
        private async void PrintReportExecute(object par)
        {
            if (ReportContent != null)
            {
               /// CreateDataGridToPrint printDataGrid = new CreateDataGridToPrint();
              //  await printDataGrid.PrintDataGrid(ProfitLosses.ToList());
                if (PrintHelper.PrintFlowDocument(ReportContent))
                 {
                  await ClearReportContent();
                 GetProcucts();
                 }
            }
        }
        private bool CanGenerateReport()
        {
            // Add logic to determine if the command can execute
            return true; // For now, always return true
        }
        private async void GetProcucts()
        {
            if((ProfitLosses!=null) && (ProfitLosses.Count>0))
            {
                ProfitLosses.Clear();
            }
            IsRunningRep = true;
            IsGeneratingReport = false;
            var prod = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetACProductsList<AlanoCLubProfitLossModel>(SqlConnectionStr, Scmd.SqlConstProp.SPGetAlanoClubInventoryProfitLoss);
            if ((prod != null) && (prod.Count > 0))
            {
                float volume = 0.0f;
                foreach (var item in prod)
                {
                    
                    if (await ALanoClubUtilites.RexMatchStr(item.ProductName, ExcludeInventory))
                        continue;
                    if (item.ItemsPerCase != 0)
                    { 
                        
                    if (item.ItemsPerCase > 0)
                    {
                        volume = (float)item.Quantity / (float)item.ItemsPerCase;
                        
                    }
                    else
                        volume = item.ItemsPerCase;
                    if (volume > 1.0f)
                        item.TotalPrice = volume * item.Price;
                    else
                    { 
                      if(volume.ToString().StartsWith("0"))
                        {
                            volume = 1.0f;
                            item.TotalPrice = volume * item.Price;
                        }
                        else
                                    item.TotalPrice = item.Quantity * item.Price;
                    }
                    item.CostPerIteam = (float)item.TotalPrice / (float)item.Quantity;
                    item.ProfitMemnber = (float)item.ClubPrice - (float)item.CostPerIteam;
                    item.ProfitNonMemnber = (float)item.ClubNonMemberPrice - (float)item.CostPerIteam;
                    }
                    ProfitLosses.Add(new AlanoCLubProfitLossModel
                    {
                        ID = item.ID,
                        ProductName = item.ProductName,
                        ItemsPerCase = item.ItemsPerCase,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Volume = volume,
                        TotalPrice = item.TotalPrice,
                        ClubPrice = item.ClubPrice,
                        ClubNonMemberPrice = item.ClubNonMemberPrice,
                        CostPerIteam = item.CostPerIteam,
                        ProfitMemnber = item.ProfitMemnber,
                        ProfitNonMemnber = item.ProfitNonMemnber,
                        TotalProfitMember = (float)item.ProfitMemnber*(float)item.Quantity,
                        TotalProfitNonMember = (float)item.ProfitNonMemnber * (float)item.Quantity

                    });
                }
            }
        }
        private async void GetConnectionStr()
        {

            ReadJsonFile readJson = new ReadJsonFile();
            SqlConnectionStr = Utilites.ALanoClubUtilites.GetSqlConnectionStrings(Utilites.ALanoClubUtilites.AlanoClubDatabaseName);

            var ex = readJson.GetJsonData<RegXExpressionsModel>(nameof(RegXExpressionsModel)).Result;
                if (ex != null)
            {
                ExcludeInventory = ex.RegXMatchStr;
            }
        }
        protected void OnPropertyChanged(string propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
