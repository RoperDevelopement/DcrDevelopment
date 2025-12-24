using AlanoClubInventory.Models;
using AlanoClubInventory.Models;
using AlanoClubInventory.Utilites;
using ScottPlot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Scmd = AlanoClubInventory.SqlServices;
using Utilites = AlanoClubInventory.Utilites;
//Button myButton = new Button();
//myButton.Margin = new Thickness(5, 10, 15, 20); // Left=5, Top=10, Right=15, Bottom=20
namespace AlanoClubInventory.ViewModels
{
    public class SendACMembersEmailsViewModel : INotifyPropertyChanged
    {

#pragma warning disable CS8612 // Nullability of reference types in type doesn't match implicitly implemented member.
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS8612 // Nullability of reference types in type doesn't match implicitly implemented member.
        private ObservableCollection<string> aCMembers;
        private bool isMemberSelected;
        private string selectedMembers = "Select Members";
        private ObservableCollection<string> membersSelected;
        private string emailSubject;
        private Thickness marginSpace;
        private string lineSpacing;
        private ICommand incrementCommand;
        private ICommand decrementCommand;
        private string myDocXaml;
        public ICommand attachFile;
        private bool isContMenuVis;
        private ICommand saveDoc;
        private ICommand emailDoc;
        private bool sendIndivalualEmails;
        private ICommand printDoc;
        public SendACMembersEmailsViewModel() 
        {
            GetSqlConn();
            LoadAcmembers();
            EmailSubject = "Butte Alano Club Member News Letter";
            IsContMenuVis = false;

            SpacingBetweenLines = 0;
            MarginSpace = new Thickness(0,0,0,SpacingBetweenLines);
            LineSpacing = $"Spacing Between Lines: {SpacingBetweenLines}";
           MyDocXaml= $"Dear Alano Club Member,{Utilites.AlanoCLubConstProp.CRLF}";
            SendIndivalualEmails=false;
        }
        private int NumberOfAttachments { get; set; } = 0;
        private string EmailHtmlBody { get; set; }
        public bool SendIndivalualEmails
        {
            get => sendIndivalualEmails;
            set { sendIndivalualEmails = value; OnPropertyChanged(nameof(SendIndivalualEmails)); }
        }
        public bool IsContMenuVis
        {
            get => isContMenuVis;
            set { isContMenuVis = value; OnPropertyChanged(nameof(IsContMenuVis)); }
        }
        public ICommand PrintDoc
        {
            get
            {
                if (printDoc == null)
                {
                    printDoc = new Utilites.RelayCommd(PrintMyDocXaml);
                }
                return printDoc;
            }
        }
        public ICommand EmailDoc
        {
            get
            {
                if (emailDoc == null)
                {
                    emailDoc = new Utilites.RelayCommd(SendEmailWithDoc);
                }
                return emailDoc;
            }
        }
        public ObservableCollection<MenuItemModel> ButtonMenuItems { get; }
            = new ObservableCollection<MenuItemModel>();
        public string MyDocXaml
        {
            get => myDocXaml;
            set { myDocXaml = value; OnPropertyChanged(nameof(MyDocXaml)); }
        }
        public ICommand SaveDoc
        {
            get
            {
                if (saveDoc == null)
                {
                    saveDoc = new Utilites.RelayCommd(SaveMyDocXaml);




                }
                return saveDoc;
            }
        }
        public System.Windows.Controls.RichTextBox MyDocRtf { get; set; } = new System.Windows.Controls.RichTextBox();
        public ICommand AttachFile
        {
            get
            {
                if (attachFile == null)
                {
                    attachFile = new Utilites.RelayCommd(AddAttachement);
                        
                }
                return attachFile;
            }
        }
        public ICommand IncrementCommand
        {
            get
            {
                if (incrementCommand == null)
                {
                    incrementCommand = new Utilites.RelayCommd(
                        param =>
                        {
                            if(SpacingBetweenLines < 10)
                            { 
                            MarginSpace = new Thickness(0,0,0,++SpacingBetweenLines);
                            LineSpacing = $"Spacing Between Lines: {SpacingBetweenLines}";
                            }

                        },
                        param => true
                    );
                }
                return incrementCommand;
            }
        }
        public ICommand DecrementCommand
        {
            get
            {
                if (decrementCommand == null)
                {
                    decrementCommand = new Utilites.RelayCommd(
                        param =>
                        {
                            if (SpacingBetweenLines > 0)
                            {
                                SpacingBetweenLines = SpacingBetweenLines - 1;
                                LineSpacing = $"Spacing Between Lines: {SpacingBetweenLines}";
                                MarginSpace = new Thickness(0, 0, 0,    SpacingBetweenLines);
                            }
                            else
                            {
                                SpacingBetweenLines = 0;
                                LineSpacing = $"Spacing Between Lines: {SpacingBetweenLines}";
                                MarginSpace = new Thickness(0);
                            }
                        },
                        param => true
                    );
                }
                return decrementCommand;
            }
        }
        public IList<AlanoCLubMembersModel> Members { get; set; }
        public string LineSpacing
        {
            get => lineSpacing;
            set { lineSpacing = value; OnPropertyChanged(nameof(LineSpacing)); }
        }
        private double SpacingBetweenLines { get; set; }
        public Thickness MarginSpace
        {
            get => marginSpace;
            set { marginSpace = value; OnPropertyChanged(nameof(MarginSpace)); }
        }
        public string EmailSubject
        {
            get => emailSubject;
            set { emailSubject = value; OnPropertyChanged(nameof(EmailSubject)); }
        }
        public ObservableCollection<string> MembersSelected
        {
            get

            {
                if (membersSelected == null)
                    membersSelected = new ObservableCollection<string>();
                return membersSelected;
            }
            set
            {
                membersSelected = value;
                OnPropertyChanged(nameof(MembersSelected));
               // OnPropertyChanged(nameof(SelectedMembers));
            }
        }
        //public string SelectedMembers =>
        //MembersSelected == null || MembersSelected.Count == 0
        //    ? "Select Member"
        //    : string.Join(", ", MembersSelected);

        private async void SaveMyDocXaml(object parama)
        {
            Utilites.OpenFileDialog openFile = new Utilites.OpenFileDialog();
            await openFile.SaveFileRichText(MyDocRtf, "Rich Text Format (*.rtf)|*.rtf|XAML (*.xaml)|*.xaml|Text Files (*.txt)|*.txt|HTML Files (*.html)|*.html|PDF Files (*.pdf)|*.pdf");
        }
        public string SelectedMembers
        {
            get => selectedMembers;
            set { selectedMembers = value; OnPropertyChanged(nameof(SelectedMembers)); }
        }
        public bool IsMemberSelected
        {
            get => isMemberSelected;
            set
            {
                isMemberSelected = value;
                OnPropertyChanged(nameof(IsMemberSelected));
            }
        }
        public ObservableCollection<string> ACMembers
        {
            get
            {
                if (aCMembers == null)
                    aCMembers = new ObservableCollection<string>();
                return aCMembers;
            }
            set
            { aCMembers = value; OnPropertyChanged(nameof(ACMembers)); }
            
        }
        private string SqlConnectionStr { get; set; }
        public IList<string> SelectedFiles { get; set; }
        private async void GetSqlConn()
        {
            SqlConnectionStr = Utilites.ALanoClubUtilites.GetSqlConnectionStrings(Utilites.ALanoClubUtilites.AlanoClubDatabaseName);

        }
        public void UpdateSelectedSummary(IList selectedItems)
        {
            if (selectedItems.Count == 0)
                SelectedMembers = "Select Members";
            else
            {
                //var s = selectedItems[0];
                //if (string.Compare(s.ToString(), "Select All Members",true) == 0)
                //{
                //    string mem = string.Empty;
                //    foreach (var item in ACMembers)
                //    {
                //        if (string.Compare(item, "Select All Members", true) == 0)
                //            continue;
                //        mem += item;
                //    }
                //    SelectedMembers = mem;
                //}
                //else
                //{
                    SelectedMembers = string.Join(";", selectedItems.Cast<string>());
                }
                
            }
         
        //public async void AddSelectedMembers()
        //{
        //   if((MembersSelected == null) || (MembersSelected.Count == 0))
        //    {
        //        SelectedMembers = "Select Members";
        //    }
        //   else
        //        SelectedMembers = string.Join(", ", MembersSelected.Cast<string>());
        //}
        private async void AddAttachement(object obj)
        {
            Utilites.OpenFileDialog openFile = new Utilites.OpenFileDialog();
            // Title = "Select an Image"
            var fileName = await openFile.OpenFile(string.Empty, "Files|,*.*;*.png;*.jpg;*.jpeg;*.bmp;*.gif");
            if (!(string.IsNullOrEmpty(fileName)))
            {
                if(SelectedFiles == null)
                {
                    SelectedFiles = new List<string>();
                    

                }
                if(NumberOfAttachments < 6)
                {
                    if (!SelectedFiles.Contains(fileName))
                    {
                        ButtonMenuItems.Add(new MenuItemModel($"Del Att File:{System.IO.Path.GetFileName(fileName)}", new Utilites.RelayCommand<string>(DelAttcahment), fileName));
                        SelectedFiles.Add(fileName);
                        IsContMenuVis = true;
                        NumberOfAttachments++;
                    }
                    else
                    {
                        Utilites.ALanoClubUtilites.ShowMessageBox($"File all ready attached {fileName}", "Dup File ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox($"There are {NumberOfAttachments} and only Maximum of 6 Attachments Allowed", "Attachment Limit Reached", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                //  MyRichTextBox.Document.Blocks.Add(new BlockUIContainer(image));
            }
        }
        private async void LoadAcmembers()
        {
           
            Members = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetACProductsList<AlanoCLubMembersModel>(SqlConnectionStr, Scmd.SqlConstProp.SPALanoCLubGetMembers);
            if ((Members != null) && (Members.Count > 0))
            {
                if( ACMembers.Count > 0)
                {
                    ACMembers.Clear();
                }
                ACMembers.Add("Select All Members");
                foreach (var member in Members)
                {
                    ACMembers.Add($"{member.MemberFirstName} {member.MemberLastName} {member.MemberEmail}");
                }
            }
        }
        
        private async void DelAttcahment(string obj)
        {
            var itemToRemove = ButtonMenuItems.FirstOrDefault(i => i.CommandParameter as string == obj);
            if (itemToRemove != null)
            {
                ButtonMenuItems.Remove(itemToRemove);
                NumberOfAttachments--;
            }
            if(SelectedFiles.Contains(obj))
            {
                SelectedFiles.Remove(obj);
            }
            if(ButtonMenuItems.Count == 0)
            {
                IsContMenuVis = false;
            }
        }
        private async void SendEmailWithDoc(object obj)
        {
            if((string.IsNullOrWhiteSpace(SelectedMembers)) || (SelectedMembers == "Select Members"))
            {
                System.Windows.MessageBox.Show("No Members Selected to Send Email", "Send Email Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var emailBody = await CreateHtmlBody();
            CreateHtmlFile();
            if (!(SendIndivalualEmails))
            {
                SendEmail(SelectedMembers, emailBody);
            }
            else
            {
                foreach (var selMem in SelectedMembers.Split(';'))
                {
                    var memParts = selMem.Split(' ');
                    var member = Members.FirstOrDefault(m => string.Compare(m.MemberFirstName, memParts[0], true) == 0 &&
                                                             string.Compare(m.MemberLastName, memParts[1], true) == 0 &&
                                                             string.Compare(m.MemberEmail, memParts[2], true) == 0);
                    if (member != null)
                    {
                        SendEmail(member.MemberEmail, emailBody);
                    }
                }
            }
        }
        //{
        //private async void SendEmailWithDoc(object obj)
        //{
        //  //  Utilites.EmailUtilites emailUtil = new Utilites.EmailUtilites();
        //    IList<AlanoCLubMembersModel> selectedMembers = new List<AlanoCLubMembersModel>();
        //    if((ed != null) && (MembersSelected.Count > 0))
        //    {
        //        if(MembersSelected.Contains("Select All Members"))
        //        {
        //            selectedMembers = Members;
        //        }
        //        else
        //        {
        //            foreach (var selMem in MembersSelected)
        //            {
        //                var memParts = selMem.Split(' ');
        //                var member = Members.FirstOrDefault(m => string.Compare(m.MemberFirstName, memParts[0], true) == 0 &&
        //                                                         string.Compare(m.MemberLastName, memParts[1], true) == 0 &&
        //                                                         string.Compare(m.MemberEmail, memParts[2], true) == 0);
        //                if (member != null)
        //                {
        //                    selectedMembers.Add(member);
        //                }
        //            }
        //        }
        //       // await emailUtil.SendEmailToACMembersWithDoc(SqlConnectionStr, selectedMembers, EmailSubject, MyDocRtf, SelectedFiles);
        //    }
        //}
        private async void CreateHtmlFile()
        {
            Utilites.OpenFileDialog openFile = new Utilites.OpenFileDialog();
            string htmlTxt = await openFile.RichTextToHtmlFile(MyDocRtf);


            CreateHtmlEMails createHtmlE = new Utilites.CreateHtmlEMails();
            string header = EmailSubject;
            var sb = await createHtmlE.CreateHeader(DateTime.Now, header, "pack://application:,,,/Resources/Images/butteac.ico");
            sb.AppendLine(htmlTxt);
            sb = await createHtmlE.CreateSig(sb);
            sb = await createHtmlE.CloseHtmlFile(sb);
            
            var emailHtmlBody = System.IO.Path.Combine(Utilites.ALanoClubUtilites.ACInventoryWF, $"AlanoClubMemberNewsLetter_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.html");
            File.WriteAllText(emailHtmlBody,sb.ToString());
            if(SelectedFiles == null)
            {
                SelectedFiles = new List<string>();
            }
            if (SelectedFiles.Count == 0)
                SelectedFiles.Add(emailHtmlBody);
            else
                SelectedFiles.Insert(0, emailHtmlBody);
        }
       private async Task<string> CreateHtmlBody()
        {
            CreateHtmlEMails createHtmlE = new Utilites.CreateHtmlEMails();
            var sb = await createHtmlE.CreateHeader(DateTime.Now, EmailSubject, "pack://application:,,,/Resources/Images/butteac.ico");
            sb = await createHtmlE.CreateSig(sb);
            sb = await createHtmlE.CloseHtmlFile(sb);
            return sb.ToString();

        }
        private async void SendEmail(string emailAddress, string emailBody)
        {
            try
            {

                Emails.EmailsInstance.SendEmail(emailAddress, "buttealano@gmail.com", EmailSubject, emailBody, true, SelectedFiles);
                Utilites.ALanoClubUtilites.ShowMessageBox($"Email Sent to {emailAddress}", "Send Email", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error Sending Email to {emailAddress} Error:{ex.Message}", "Send Email Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        public async void PrintMyDocXaml(object obj)
        {
         Reports.PrintHelper.PrintFlowDocument(MyDocRtf.Document);
        }
        protected void OnPropertyChanged(string propertyName = null) =>
     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
