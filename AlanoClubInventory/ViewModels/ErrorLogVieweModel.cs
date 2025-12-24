using AlanoClubInventory.Models;
using AlanoClubInventory.SqlServices;
using AlanoClubInventory.Utilites;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace AlanoClubInventory.ViewModels
{
    public class ErrorLogVieweModel: INotifyPropertyChanged
    {
        private string lastRefresh;
        private string entryCount;
        private ICommand refreshLogsCommand;
        private ObservableCollection<ErrorLogEntry> logs;
        private AlanoClubDatabaseMaintenanceService dbMaintenanceModel = new AlanoClubDatabaseMaintenanceService();
        private bool isPageEnable;
        private ICommand clearCommand;
        private string keyWord;
        private DateTime eDate;
        private DateTime sDate;
        private ICommand exportToPdfCommand;
        private double progressValue;
        private bool isProgressBar;
        private bool isDataGrid;
        private bool isBusy;
        
        public ErrorLogVieweModel()
        {
            LastRefresh =$"LastRefresh {DateTime.Now.ToString("g")}";
            EntryCount = $"Total Logs 0";
            IsPageEnable = true;
            KeyWord = "Seperate by comma for multiple keywords";
            SDate = DateTime.Now;
            EDate= DateTime.Now;
            IsProgressBar = false;
            IsDataGrid=false;
            IsBusy =false;
            dbMaintenanceModel.UpdateProgessBar += (s, count) => UpProgBar(count);
            dbMaintenanceModel.ShowHideProgessBar += (s, showProBar) => ShowProBar(showProBar);
            dbMaintenanceModel.UpdateProcessDone += (s, done) => ShowDG(done);
        }
        
        public bool IsBusy
        {


             get => isBusy; 
            set
            {
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }
        public bool IsDataGrid
        {
            get => isDataGrid;
            set
            {
                if (isDataGrid != value)
                {
                    isDataGrid = value;
                    OnPropertyChanged(nameof(IsDataGrid));
                }
            }
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
        public ICommand ExportToPdfCommand
        {
            get
            {
                if (exportToPdfCommand == null)
                {
                    exportToPdfCommand = new RelayCommd(async param =>
                    {
                        if((Logs != null) && (Logs.Count()>0))
                        {
                            SaveDocument();
                            if (!string.IsNullOrEmpty(SaveDocumentFolder))
                            {
                                string ext = System.IO.Path.GetExtension(SaveDocumentFolder).ToLower();
                                if (ext == ".pdf")
                                {
                                    var pdfCreator = new Utilites.CreatePDFFileErrorLogs();
                                    await pdfCreator.CreateErrorLogsPdgFile(Logs, SaveDocumentFolder);
                                }
                                else if (ext == ".html")
                                {
                                    var htmlCreator = new Utilites.CreateErrorLogHtmlFile();
                                    await htmlCreator.CreatLogHtmlFile(logs, SaveDocumentFolder);
                                }
                                else
                                {
                                    
                                        Utilites.ALanoClubUtilites.ShowMessageBox("No Logs to export to PDF File", "Export to PDF", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                                    
                                }

                            }
                        }
                        else
                        {
                            Utilites.ALanoClubUtilites.ShowMessageBox("No Logs to export to PDF File or HTML file", "Export to PDF or HTML", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                        }
                    }, param => true);
                }
                return exportToPdfCommand;
            }
        }
        public DateTime SDate
        {
            get => sDate;
            set { sDate = value; OnPropertyChanged(); }
        }
        public DateTime EDate
        {
            get => eDate;
            set { eDate = value; OnPropertyChanged(); }
        }
        public string KeyWord
        {
            get => keyWord;
            set { keyWord = value; OnPropertyChanged(); }
        }
        public bool IsPageEnable
        {
            get => isPageEnable;
            set { isPageEnable = value; OnPropertyChanged(); }
        }
        private string SaveDocumentFolder { get; set; }
        public ICommand ClearCommand
        {
            get
            {
                if (clearCommand == null)
                {
                    clearCommand = new RelayCommd(async param =>
                    {
                        
                        Logs = new ObservableCollection<ErrorLogEntry>();
                        EntryCount = $"Total Logs 0";
                        LastRefresh = $"LastRefresh {DateTime.Now.ToString("g")}";
                    }, param => true);
                }
                return clearCommand;
            }
        }
        public string EntryCount
        {
            get => entryCount;
            set { entryCount = value; OnPropertyChanged(); }
        }
        public ObservableCollection<ErrorLogEntry> Logs
        {
            get => logs;
            set { logs = value; OnPropertyChanged(); }
        }
        public ICommand RefreshLogsCommand
        {
            get
            {
                if (refreshLogsCommand == null)
                {
                    refreshLogsCommand = new RelayCommd(RefreshLogs, param => true);
                }
                return refreshLogsCommand;
            }
        }
        public string LastRefresh
        {
            get => lastRefresh;
            set { lastRefresh = value; OnPropertyChanged(); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private async void RefreshLogs(object obj)
        {
          //  IsPageEnable = false;
            
            if ((string.IsNullOrWhiteSpace(KeyWord) || KeyWord.StartsWith("Seperate by comma")) && (SDate.ToString("MM-dd-yyyy") == DateTime.Now.ToString("MM-dd-yyyy")) && (EDate.ToString("MM-dd-yyyy") == DateTime.Now.ToString("MM-dd-yyyy")))
            {
                
                ClearLogs();
                RefreshLogsWithOutFilter();
                
            }
            else
            {
                if(KeyWord.StartsWith("Seperate by comma"))
                {
                    KeyWord = "";
                }
                
                RefreshLogsWithFilter();
                
                
            }
            
            
        }
        //private async void RefreshLogs(object obj)
        //{
        //    IsPageEnable = false;
        //    var logs = await  dbMaintenanceModel.LoadLogs();
        //    if((logs != null) && (logs.Count() > 0))
        //    {

        //        EntryCount = $"Total Logs {logs.Count()}";
        //        Logs =    new ObservableCollection<ErrorLogEntry>(logs); 

        //    }
        //    IsPageEnable = true;
        //    //  var logs = await dbMaintenanceModel.li
        //    //foreach (var log in logs)
        //    //{
        //    //    Logs.Add(log);
        //    //}
        //    LastRefresh = $"LastRefresh {DateTime.Now.ToString("g")}";
        //}
        private async void SaveDocument()

        {
            Utilites.OpenFileDialog openFileDialog = new Utilites.OpenFileDialog();
            SaveDocumentFolder = await openFileDialog.SaveFile("PDF Files(*.pdf) | *.pdf|HTML Files (*.html)|*.html");
            
            //var filename = System.IO.Path.Combine(Utilites.ALanoClubUtilites.ACInventoryWF, $"AlanoClubReceipt.pdf");

        }
        private async void RefreshLogsWithFilter()
        {
            
            IList<ErrorLogEntry> logs = new List<ErrorLogEntry>();
            if((EDate<SDate) || (SDate>EDate))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Invalid Dates Start Date {SDate.ToString("MM-dd-yyyy")} End Date {EDate.ToString("MM-dd-yyyy")} ", "Date Selection Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

                return;
            }
            ClearLogs();
            //var keywords = KeyWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(k => k.Trim()).ToList();
            if (((SDate.ToString("MM-dd-yyyy") == DateTime.Now.ToString("MM-dd-yyyy"))) && (EDate.ToString("MM-dd-yyyy") == DateTime.Now.ToString("MM-dd-yyyy")))
                      logs = await  dbMaintenanceModel.LoadLogs(KeyWord);
            else
            {
                logs = await dbMaintenanceModel.LoadLogs(KeyWord, SDate, EDate);
            }
            if ((logs != null) && (logs.Count() > 0))
            {
                EntryCount = $"Total Logs {logs.Count()}";
                Logs = new ObservableCollection<ErrorLogEntry>(logs);
               
            }
           
            LastRefresh = $"LastRefresh {DateTime.Now.ToString("g")}";
            
        }
        private async void RefreshLogsWithOutFilter()
        {
            ClearLogs();
            
            
            var logs = await dbMaintenanceModel.LoadLogs();
            if ((logs != null) && (logs.Count() > 0))
            {
                EntryCount = $"Total Logs {logs.Count()}";
                Logs = new ObservableCollection<ErrorLogEntry>(logs);
            }
            
            LastRefresh = $"LastRefresh {DateTime.Now.ToString("g")}";
        }
        private void ClearLogs()
        {
            Logs = new ObservableCollection<ErrorLogEntry>();
            EntryCount = $"Total Logs 0";
            LastRefresh = $"LastRefresh {DateTime.Now.ToString("g")}";
        }
        private void UpProgBar(double count)
        {
            ProgressValue = count;
            
        }
        private void ShowProBar( bool show)
        {
            ProgressValue = 0.00;
            IsProgressBar = show;

            IsBusy = show;
        }
        private void ShowDG(bool done)
        {
            IsDataGrid=done;
            IsPageEnable = done;

        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
      => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
