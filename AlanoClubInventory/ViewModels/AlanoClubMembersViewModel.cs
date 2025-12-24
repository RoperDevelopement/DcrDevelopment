using AlanoClubInventory.Models;
using PdfSharp.Snippets.Font;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AlanoClubInventory.Utilites;
using Scmd = AlanoClubInventory.SqlServices;

namespace AlanoClubInventory.ViewModels
{
    public class AlanoClubMembersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<AlanoCLubMembersModel> membersModels;
        private ICommand addUpdateMembers;
        private int memberID;
        private string memberFirstName;
        private string memberLastName;
        private string memberEmail;
        private string memberPhoneNumber;
        private DateTime sobrietyDate ;
        private ICommand clearMembers;
      //  private DateTime membershipEndDate;
        private bool isActiveMember;
        private bool isBoardMember;
        public AlanoClubMembersViewModel()
        {
            GetSqlConn();
            GetMembersList();
            //   MembershipStartDate = DateTime.Now;
            // MembershipEndDate= DateTime.Now.AddYears(1);
            SobrietyDate = DateTime.Now;
            SelectedMemberID = -1;
            isActiveMember= true;
        }
        public int MemberID
        {
            get => memberID;
            set
            {

                memberID = value;
                OnPropertyChanged(nameof(MemberID));
                // Trigger journaling overlay or animation here

            }
        }
        public bool IsBoardMember
        {
            get => isBoardMember;
            set
            {  isBoardMember = value;
            OnPropertyChanged(nameof(IsBoardMember));
            }
        }
        public int SelectedMemberID { get; set; }
        public string MemberFirstName
        {
            get => memberFirstName;
            set
            {

                memberFirstName = value;
                OnPropertyChanged(nameof(MemberFirstName));
                // Trigger journaling overlay or animation here

            }
        }
        public string MemberLastName
        {
            get => memberLastName;
            set
            {

                memberLastName = value;
                OnPropertyChanged(nameof(MemberLastName));
                // Trigger journaling overlay or animation here

            }
        }
        public string MemberEmail
        {
            get => memberEmail;
            set
            {

                memberEmail = value;
                OnPropertyChanged(nameof(MemberEmail));
                // Trigger journaling overlay or animation here

            }
        }
        public string MemberPhoneNumber
        {
            get => memberPhoneNumber;
            set
            {

                memberPhoneNumber = value;
                OnPropertyChanged(nameof(MemberPhoneNumber));
                // Trigger journaling overlay or animation here

            }
        }
        public DateTime SobrietyDate
        {
            get => sobrietyDate;
            set
            {

                sobrietyDate = value;
                OnPropertyChanged(nameof(SobrietyDate));
                // Trigger journaling overlay or animation here

            }
        }
        //public DateTime MembershipEndDate
        //{
        //    get => membershipEndDate;
        //    set
        //    {

        //        membershipEndDate = value;
        //        OnPropertyChanged(nameof(MembershipEndDate));
        //        // Trigger journaling overlay or animation here

        //    }
        //}
        public bool IsActiveMember
        {
            get => isActiveMember;
            set
            {

                isActiveMember = value;
                OnPropertyChanged(nameof(IsActiveMember));
                // Trigger journaling overlay or animation here

            }
        }
        private async void GetSqlConn()
        {
            SqlConnectionStr = Utilites.ALanoClubUtilites.GetSqlConnectionStrings(Utilites.ALanoClubUtilites.AlanoClubDatabaseName);

        }
        private string SqlConnectionStr { get; set; }
        public AlanoCLubMembersModel AlanoCLubMembers { get; set; } = new AlanoCLubMembersModel();
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
        public ICommand AddUpdateMembers
        {
            get
            {
                if (addUpdateMembers == null)
                {
                    addUpdateMembers = new RelayCommd(MembersAddUpDate, param => CanAddUP());
                }
                return addUpdateMembers;

            }
        }
        public ICommand ClearMembers
        {
            get
            {
                if (clearMembers == null)
                {
                    clearMembers = new RelayCommd(ResetMemberProperties, param => CanAddUP());
                }
                return clearMembers;

            }
        }
        
        private async void MembersAddUpDate(object obj)
        {
            var mID = await Utilites.ALanoClubUtilites.ConvertToInt(MemberID.ToString());
            if ((mID == int.MaxValue) || (mID == 0))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox("Please Enter a Valid Member ID", "Invalid Member ID", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(MemberFirstName))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox("Please Enter a Valid Member First Name", "Invalid Member First Name", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
            if (string.IsNullOrWhiteSpace(MemberLastName))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox("Please Enter a Valid Member Last Name", "Invalid Member First Name", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
            if (string.IsNullOrWhiteSpace(MemberPhoneNumber))
            {
                MemberPhoneNumber = Utilites.AlanoCLubConstProp.NA;

            }
            else
            {
                if (!(await Utilites.ALanoClubUtilites.RexMatchStr(MemberPhoneNumber, Utilites.AlanoCLubConstProp.VailPhoneNumber)))
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox($"Please Enter a Valid Member Phone Number {MemberPhoneNumber}", "Invalid Member Phone Number", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }
            }
            if (string.IsNullOrWhiteSpace(MemberEmail))
            {
                MemberEmail = Utilites.AlanoCLubConstProp.NA;
            }
            else
            {
                if (!(await Utilites.ALanoClubUtilites.RexMatchStr(MemberEmail, Utilites.AlanoCLubConstProp.VailEmailAddress)))
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox($"Please Enter a Valid Member Email Adress {MemberEmail}", "Invalid Member Email Adress", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }
                if (!(await Emails.EmailsInstance.IsValidEmail(MemberEmail)))
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox($"Please Enter a Valid Member Email Adress {MemberEmail}", "Invalid Member Email Adress", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }
            }
            AddUpdateMember();



        }
        private async void GetMembersList()
        {
            var alMem = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetACProductsList<AlanoCLubMembersModel>(SqlConnectionStr, Scmd.SqlConstProp.SPALanoCLubGetMembers);
            if((alMem!=null) && (alMem.Count>0))
            {
                Members = new ObservableCollection<AlanoCLubMembersModel>(alMem);
            }
        }
        public async Task AddUpdateMember()
        {
            var sp = new List<StoredParValuesModel>();
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaMemberID, ParmaValue = MemberID.ToString() });
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaMemberFirstName, ParmaValue = MemberFirstName });
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaMemberLastName, ParmaValue = MemberLastName });
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaMemberEmail, ParmaValue = MemberEmail });
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaMemberPhoneNumber, ParmaValue = MemberPhoneNumber });
           sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaSobrietyDate, ParmaValue = SobrietyDate.ToString("MM-dd-yyyy") });
         //   sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaMembershipEndDate, ParmaValue = MembershipEndDate.ToString("MM-dd-yyyy") });
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaIsActiveMember, ParmaValue = IsActiveMember.ToString() });
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaIsBoardMember, ParmaValue = IsBoardMember.ToString() });
            
            await Scmd.AlClubSqlCommands.SqlCmdInstance.UpDateInsertWithParma(SqlConnectionStr, Scmd.SqlConstProp.SPUpDateAddMemebers, sp);
          
           await UpdatedListView();
            ResetMemberProperties(null);
        }
        private async Task UpdatedListView()
        {
            if (Members == null)
                Members = new ObservableCollection<AlanoCLubMembersModel>();
            var upDateMemInfor =(new AlanoCLubMembersModel
            {
                MemberID = MemberID,
                MemberFirstName = MemberFirstName,
                MemberLastName = MemberLastName,
                MemberEmail = MemberEmail,
                MemberPhoneNumber = MemberPhoneNumber,
                SobrietyDate = SobrietyDate,
               // MembershipEndDate = MembershipEndDate,
                IsActiveMember = IsActiveMember
            });
            if (SelectedMemberID >= 0)
            {
                //var menExits = Members.Where(p => p.MemberID == MemberID).ToList();
                //if((menExits!= null) && (menExits.Count > 0))
                //{
                //  Members[SelectedMemberID].MemberID = MemberID;
                // Members[SelectedMemberID].MemberEmail = MemberEmail;
                // Members[SelectedMemberID].MemberLastName = MemberLastName;
                // Members[SelectedMemberID].MemberFirstName = MemberFirstName;
                // Members[SelectedMemberID].MemberPhoneNumber = MemberPhoneNumber;
                // Members[SelectedMemberID].IsActiveMember = IsActiveMember;
                //Members[SelectedMemberID].MembershipStartDate = MembershipStartDate;
                //Members[SelectedMemberID].MembershipEndDate = MembershipEndDate;
                Members.RemoveAt(SelectedMemberID);
                Members.Insert(SelectedMemberID, upDateMemInfor);


                //}
            }
            else
            {
                Members.Add(upDateMemInfor);
                
            }
            SelectedMemberID = -1;
            }
        private async void ResetMemberProperties(object param)
        {
            MemberID = 0;
            MemberFirstName = string.Empty;
            MemberLastName = string.Empty;
            MemberEmail = string.Empty;
            MemberPhoneNumber = string.Empty;
            SobrietyDate = DateTime.Now;
         //   MembershipEndDate = DateTime.Now.AddYears(1);
            IsActiveMember = true;
            IsBoardMember=false;
        }
        public async Task<bool> CheckMemberIDNotUsed()
        {
         return await Scmd.AlClubSqlCommands.SqlCmdInstance.CheckMemberID(SqlConnectionStr,Scmd.SqlConstProp.SPCheckMemberIDNotUsed, MemberID);
        }
        private bool CanAddUP()
        {
            // Add logic to determine if the command can execute
            return true; // For now, always return tru
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
