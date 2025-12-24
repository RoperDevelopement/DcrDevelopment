using AlanoClubInventory.Interfaces;
using AlanoClubInventory.Models;
using AlanoClubInventory.Reports;
using AlanoClubInventory.Utilites;
using AlanoClubInventory.Views;
  
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using Scmd = AlanoClubInventory.SqlServices;

namespace AlanoClubInventory.ViewModels
{
    public class PrintEmailReciptViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private FlowDocument reportContent;
        private RichTextBox reportContentTxt;
        private string emailAddress;
        private ICommand printRec;
        private ICommand emailRec;
        private ICommand printEmailRec;



#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public PrintEmailReciptViewModel(DateTime recpDate, IList<PayDuesModel> payDues, int reciptNumber = 0, int memberID = 0)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
         
            Init(recpDate,payDues,reciptNumber,memberID);
        }
        private bool IsEmailed { get; set; }
        private string ReciptNumberStr { get; set; }
        private string ReciptDate { get; set; }
        public ICommand EmailRec
        {
            get
            {
                if (emailRec == null)
                {
                    emailRec = new RelayCommdNoPar(PrintJustEmail, param => CanGenerateReport());
                }
                return emailRec;
            }
        }
        
              public ICommand PrintEmailRec
        {
            get
            {
                if (printEmailRec == null)
                {
                    printEmailRec = new RelayCommdNoPar(RecPrintEmail, param => CanGenerateReport());
                }
                return printEmailRec;
            }
        }
        public ICommand PrintRec
        {
            get
            {
                if (printRec == null)
                {
                    printRec = new RelayCommdNoPar(PrintJustRecipt, param => CanGenerateReport());
                }
                return printRec;
            }
        }
        private IList<AlanoCLubMembersModel> MembersModels { get; set; } = new List<AlanoCLubMembersModel>();

      
        public string EmailAddress
        {
            get => emailAddress;
            set
            {
                emailAddress = value;
                OnPropertyChanged(nameof(EmailAddress));
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
        
        private string SqlConnectionStr { get; set; }
        private async void GetSqlConn()
        {
            SqlConnectionStr = Utilites.ALanoClubUtilites.GetSqlConnectionStrings(Utilites.ALanoClubUtilites.AlanoClubDatabaseName);

        }
        private async void Init(DateTime recpDate, IList<PayDuesModel> payDues, int reciptNumber = 0, int memberID = 0)
        {
            ReciptNumberStr = reciptNumber.ToString();
            ReciptDate = recpDate.ToString("MM-dd-yyyy");
            GetSqlConn();
            if (memberID > 0)
            {

                GetMemnberInformation(memberID);

            }
            else
            {
                CreateNonMember();
            }

            CreateReciept(recpDate, payDues, reciptNumber);
        }
        private async void PrintJustRecipt()
        {
            if (ReportContent != null)
            {
                if (PrintHelper.PrintFlowDocument(ReportContent, 5))
                {
                    GoBack();
                }
            }
        }
        private async void PrintJustEmail()
        {
            if (ReportContent != null)
            {

                CreatePdfDocument();
                if(IsEmailed)
                {
                    GoBack();
                }
                    //createPdf.PDFDocRec(textRange, null,string.Empty,DateTime.Now,null,1);
            }
        }

        private async void RecPrintEmail()
        {
            
            if (ReportContent != null)
            {
                CreatePdfDocument();
                if(IsEmailed)
                {
                    PrintJustRecipt();
                }
            }
        }
        public void GoBack()
        {
            var navWindow = Application.Current.MainWindow as NavigationWindow;
            if (navWindow != null && navWindow.CanGoBack)
            {
             //   navWindow.RemoveBackEntry();
                ReceiptsPage viewModel = new ReceiptsPage();
                navWindow.NavigationService.Navigate(viewModel);
                
            }
        }
        private async void CreateReciept(DateTime recpDate, IList<PayDuesModel> payDues, int reciptNumber = 0)
        {
            MemberShipListFlowDocument recioptFD = new MemberShipListFlowDocument();
           var recTabHeaders = AddTableHeadersRec();
          //  if(MembersModels.Count > 0)
            ReportContent = await recioptFD.Receipt(payDues, "", recTabHeaders, recpDate, MembersModels[0], reciptNumber);
            //else
           // ReportContent = await recioptFD.Receipt(payDues, "", recTabHeaders, recpDate, null, reciptNumber);
        }
        private List<FlowDocumentModel>  AddTableHeadersRec()
        {
           var recFlowDocuments = new List<FlowDocumentModel>();
            recFlowDocuments.Add(new FlowDocumentModel { GridLength = 100, TableHeader = "Quanity" });
            recFlowDocuments.Add(new FlowDocumentModel { GridLength = 300, TableHeader = "Description" });
            recFlowDocuments.Add(new FlowDocumentModel { GridLength = 100, TableHeader = "Price" });
            recFlowDocuments.Add(new FlowDocumentModel { GridLength = 100, TableHeader = "Amount" });
            return recFlowDocuments;
            
        }
        private async void CreatePdfDocument()
        {
            IsEmailed = false;
            if (string.IsNullOrEmpty(EmailAddress))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox("Missing User Email", "Missing Email", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                return;
            }

            else
            {
                if (!(await Utilites.ALanoClubUtilites.RexMatchStr(EmailAddress, Utilites.AlanoCLubConstProp.VailEmailAddress)))
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox($"Please Enter a Valid Member Email Adress {EmailAddress}", "Invalid Member Email Adress", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }
                if (!(await Emails.EmailsInstance.IsValidEmail(EmailAddress)))
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox($"Please Enter a Valid Member Email Adress {EmailAddress}", "Invalid Member Email Adress", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }
            }
            TextRange textRange = new TextRange(ReportContent.ContentStart, ReportContent.ContentEnd);
            CreatePdfDocument createPdf = new CreatePdfDocument();
           var pdfFName = await createPdf.PDFDocRec(textRange, ReciptNumberStr, ReciptDate);
            SendEmail(pdfFName);
            IsEmailed = true;


        }
        private async void SendEmail(string pdfFname)
        {
            Utilites.CreateHtmlEMails createHtmlE = new Utilites.CreateHtmlEMails();
            string header = $"Alano CLub Receipt #{ReciptNumberStr} Receipt Date {ReciptDate}";
            var sb = await createHtmlE.CreateHeader(DateTime.Now, header, "pack://application:,,,/Resources/Images/butteac.ico");
            sb = await createHtmlE.CreateSig(sb); 
            sb = await createHtmlE.CloseHtmlFile(sb);
            Emails.EmailsInstance.SendEmail(EmailAddress, "buttealano@gmail.com",$"Alano CLub Receipt #{ReciptNumberStr}", sb.ToString(), true,pdfFname);

        }
        private async void CreateNonMember()
        {
            MembersModels.Add(new AlanoCLubMembersModel
            {
                MemberID = -1,
                MemberEmail = Utilites.AlanoCLubConstProp.NA,
                MemberFirstName = Utilites.AlanoCLubConstProp.NA,
                MemberLastName = Utilites.AlanoCLubConstProp.NA,
                MemberPhoneNumber = Utilites.AlanoCLubConstProp.NA


            });
        }
        private async void GetMemnberInformation(int memID)
        {
            var parms = new List<StoredParValuesModel>();
            parms.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaID, ParmaValue = memID.ToString() });
           MembersModels = await Scmd.AlClubSqlCommands.SqlCmdInstance.CallStoreProdByParmaters<AlanoCLubMembersModel>(SqlConnectionStr,Scmd.SqlConstProp.SPALanoCLubGetMemberByID,parms);
            if(MembersModels.Count != 0)
                EmailAddress = MembersModels[0].MemberEmail;
        }
        private bool CanGenerateReport()
        {
            // Add logic to determine if the command can execute
            return true; // For now, always return true
        }
        protected void OnPropertyChanged(string propertyName=null) =>
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
