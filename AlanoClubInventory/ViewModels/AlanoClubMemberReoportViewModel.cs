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
namespace AlanoClubInventory.ViewModels
{
    public class AlanoClubMemberReoportViewModel : INotifyPropertyChanged
    {
        private readonly MemberShipListFlowDocument memberShipListFlow = new MemberShipListFlowDocument();
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<AlanoCLubMembersModel> membersModels;
        private ICommand printReport;
        private RichTextBox reportContentTxt;
        private bool isGeneratingReport;
        private bool isPrintingReport;
        private string buttPrint;
        private FlowDocument reportContent;
        public AlanoClubMemberReoportViewModel()
        {
            GetSqlConn();
            GetMembersList();
            IsGeneratingReport = true;
            IsPrintingReport = false;
            ButtPrint = "Generate Report";
        }
        private IList<FlowDocumentModel> MemFlowDocuments { get; set; }
        public string ButtPrint
        {
            get => buttPrint;
            set
            {
                buttPrint = value;
                OnPropertyChanged(nameof(ButtPrint));
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
        public bool IsPrintingReport
        {
            get => isPrintingReport;
            set
            {
                if (isPrintingReport != value)
                {
                    isPrintingReport = value;
                    OnPropertyChanged(nameof(IsPrintingReport));
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
        private string SqlConnectionStr { get; set; }
        private async void GetSqlConn()
        {
            SqlConnectionStr = Utilites.ALanoClubUtilites.GetSqlConnectionStrings(Utilites.ALanoClubUtilites.AlanoClubDatabaseName);

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
        public ObservableCollection<AlanoCLubMembersModel> Members
        {
            get => membersModels;
            set
            {

                membersModels = value;
                OnPropertyChanged(nameof(Members));
                // Trigger journaling overlay or animation here

            }
        }
        private async void GetMembersList()
        {
            var alMem = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetACProductsList<AlanoCLubMembersModel>(SqlConnectionStr, Scmd.SqlConstProp.SPALanoCLubGetMembers);
            if ((alMem != null) && (alMem.Count > 0))
            {
                Members = new ObservableCollection<AlanoCLubMembersModel>(alMem);
            }
        }
        private bool CanAddUP()
        {
            // Add logic to determine if the command can execute
            return true; // For now, always return tru
        }
        public ICommand PrintReport
        {
            get
            {
                if (printReport == null)
                {
                    printReport = new RelayCommd(CurrentMembers, param => CanAddUP());
                }
                return printReport;

            }
        }
        private async Task PMemberShipList()
        {
            ReportContent = await memberShipListFlow.MemberShipList(Members.ToList(), "ALano Club Current Member List", MemFlowDocuments);
        }
        private async void AddTableHeadersMemList()
        {
            MemFlowDocuments = new List<FlowDocumentModel>();
             MemFlowDocuments.Add(new FlowDocumentModel { GridLength = 100,TableHeader="Member ID" });
            MemFlowDocuments.Add(new FlowDocumentModel { GridLength = 100, TableHeader = "First Name" });
            MemFlowDocuments.Add(new FlowDocumentModel { GridLength = 100, TableHeader = "Last Name" });
            MemFlowDocuments.Add(new FlowDocumentModel { GridLength = 100, TableHeader = "Email" });
            MemFlowDocuments.Add(new FlowDocumentModel { GridLength = 100, TableHeader = "Phone Number" });
            MemFlowDocuments.Add(new FlowDocumentModel { GridLength = 100, TableHeader = "Soberity Date" });
            MemFlowDocuments.Add(new FlowDocumentModel { GridLength = 100, TableHeader = "Member Active" });
            MemFlowDocuments.Add(new FlowDocumentModel { GridLength = 100, TableHeader = "Board Member" });
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
        private async void CurrentMembers(object parameter)
        {
            
            IsPrintingReport = true;
            ButtPrint = "Print Report";
            if(isGeneratingReport)
            {
              await  ClearReportContent();
                AddTableHeadersMemList();
            await PMemberShipList();
                IsGeneratingReport = false;
            }
            else
            {
                PrintReportExecute();
            }
            
            //var reportParams = new Dictionary<string, object>
            //{
            //    { "ReportTitle", "Alano Club Members Report" },
            //    { "GeneratedOn", DateTime.Now.ToString("g") }
            //};
            //Utilites.ReportUtilites.GenerateAndShowReport<AlanoCLubMembersModel>("AlanoClubMembersReport.rdlc", Members.ToList(), reportParams);
        }
        private async void PrintReportExecute()
        {
            if (ReportContent != null)
            {
                if (PrintHelper.PrintFlowDocument(ReportContent))
                {
                    IsPrintingReport = false;
                    IsGeneratingReport = true;
                    ButtPrint = "Generate Report";
                    GetMembersList();

                }
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
