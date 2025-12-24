using AlanoClubInventory.Models;
using AlanoClubInventory.Reports;
using AlanoClubInventory.Utilites;
using AlanoClubInventory.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
namespace AlanoClubInventory.ViewModels
{
    public class VolunteerHrsPrintViewModel:INotifyPropertyChanged
    {
        private readonly CreateVolHrsReport createVolHrs = new CreateVolHrsReport();
        public event PropertyChangedEventHandler PropertyChanged;
        private IList<VolunteerHoursModel> volunteerHours;
        private DateTime repEnddate;
        private DateTime repStartdate;
        private string txtTitle;
        private FlowDocument reportContent;
        private ICommand printReport;
        private RichTextBox reportContentTxt;
        private bool isProgressBar;
        private double progressValue;
        public VolunteerHrsPrintViewModel(IList<VolunteerHoursModel> hrs,DateTime sDate,DateTime eDate)
        {
            createVolHrs.UpdateProgessBar += (s, count) => UpProgBar(count);
            createVolHrs.ShowHideProgessBar += (s, showProBar) => ShowProBar(showProBar);
            volunteerHours = hrs;
            repEnddate = eDate;
            repStartdate = sDate;
            TxtTitle = $"Vol Hrs Report for dates {sDate.ToString("MM-dd-yyyy")}-{eDate.ToString("MM-dd-yyyy")}";
            CreateReport(hrs, sDate,eDate);
            IsProgressBar = false;

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
            set
            {
                isProgressBar = value;
                OnPropertyChanged(nameof(IsProgressBar));
            }
        }
        public string TxtTitle
        {
            get => txtTitle;
            set { txtTitle = value;  OnPropertyChanged(nameof(TxtTitle)); }
        }
        public ICommand PrintReportCmd
        {
            get
            {
                if (printReport == null)
                {
                    printReport = new RelayCommdNoPar(PrintReportExecute, param => true);
                }
                return printReport;
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
        private void PrintReportExecute()
        {
            if (ReportContent != null)
            {
                PrintHelper.PrintFlowDocument(ReportContent, 5);
                
                    
                
            }
        }
        private void UpProgBar(double count)
        {
            ProgressValue=count;
        }
        private void ShowProBar(bool show)
        {
            IsProgressBar=show;
        }
        private async void CreateReport(IList<VolunteerHoursModel> hrs, DateTime sDate, DateTime eDate)
        {
           
            var th = GetTableHeaders();
            ReportContent = await createVolHrs.VolReport(hrs, sDate, eDate, th);


        }
        private IList<FlowDocumentModel> GetTableHeaders()
        {
           IList<FlowDocumentModel> fDoc = new List<FlowDocumentModel>();
            fDoc.Add(new FlowDocumentModel { GridLength = 150, TableHeader = "User Name" });
            fDoc.Add(new FlowDocumentModel { GridLength = 125, TableHeader = "First Name" });
            fDoc.Add(new FlowDocumentModel { GridLength = 125, TableHeader = "Lase Name" });
            fDoc.Add(new FlowDocumentModel { GridLength = 150, TableHeader = "Clocked In" });
            fDoc.Add(new FlowDocumentModel { GridLength = 150, TableHeader = "Clocked Out" });
            fDoc.Add(new FlowDocumentModel { GridLength = 75, TableHeader = "Total Hrs" });
            return fDoc;
            
           
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
