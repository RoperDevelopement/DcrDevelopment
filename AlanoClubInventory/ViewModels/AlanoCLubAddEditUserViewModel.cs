using AlanoClubInventory.Models;
using AlanoClubInventory.SqlServices;
using AlanoClubInventory.Utilites;
using AlanoClubInventory.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Scmd = AlanoClubInventory.SqlServices;
namespace AlanoClubInventory.ViewModels
{
   public class AlanoCLubAddEditUserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        
        private bool emailInfoUser;
        private   ICommand addEditUser;
        private ICommand clearTxt;
        private int iD;
        private string userName;
       
        private bool isAdmin;
        private string userFirstName ;
        private string userLastName ;
        private string userEmailAddress ;
        private string userPasswordString ;
        private string userPhoneNumber ;
        private bool isAcvite;
        private bool editUsers;
        private ALanoClubUsersModel members = new ALanoClubUsersModel();
        private ItemListModel membersModel;
        public bool isEditing;
        private string txtButSaveUpDate;
        private bool isPWVis;
        private ObservableCollection <ItemListModel> itemList;
        public string editACUser;
        private string textTitle;
        private ICommand delUser;
        // Add properties and methods for adding/editing users here
        public AlanoCLubAddEditUserViewModel()
        {
            // Initialize properties and commands here
            
            GetSqlConn();
            CreateNewUser();
            IsAcvite = true;
            EmailInfoUser=true;
            IsEditing = false;
            GetMembers();
            TxtButSaveUpDate = "Create User";
            IsPWVis = true;
            EditACUser = "Edit User:";
            TextTitle = "Alano CLub Add User";

        }
        public string TextTitle
        {
            get => textTitle; 
            set
            {
                textTitle = value;
                OnPropertyChanged(nameof(TextTitle));
            }
        }
        public string EditACUser
        {
            get => editACUser;
            set { 
                editACUser = value; 
                OnPropertyChanged(nameof(EditACUser));
            }
        }
        public bool IsPWVis
        {
            get => isPWVis;
            set
            {
                isPWVis = value;
                OnPropertyChanged(nameof(IsPWVis));
            }
        }

        public string TxtButSaveUpDate
        {
            get => txtButSaveUpDate;
            set
            {
                txtButSaveUpDate = value;
                OnPropertyChanged(nameof(TxtButSaveUpDate));
            }
        }
        public bool EditUsers
        {
            get => editUsers;
            set
            {
                editUsers = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection <ItemListModel> ItemList
        {
            get => itemList;
            set
            {
                itemList = value;
                OnPropertyChanged(nameof(ItemList));
            }
        }
        public bool IsEditing
        { 
            get => isEditing;
            set
            { 
                isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }
        public ItemListModel MembersModel
        {
            get
            {
                if (membersModel == null)
                    membersModel = new ItemListModel();
                return membersModel;
            }
            set
            {
                membersModel = value;
                EditUserInfor();
                OnPropertyChanged(nameof(MembersModel));
            }
        }
        public IList<ALanoClubUsersModel> AlanoCLubUsers { get; set; }
        public ALanoClubUsersModel Members
        {
            get => members;
            set
            {
                members = value;
                OnPropertyChanged(nameof(Members));
            }
        }
        public bool IsAcvite
        {
            get => isAcvite;
            set
            {
                isAcvite = value;
                OnPropertyChanged(nameof(IsAcvite));
            }
        }
        public int ID { get; set; } = 0;
        
             
        
        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
       
       
       private async void GetMembers()
        {
            AlanoCLubUsers = await Scmd.AlClubSqlCommands.SqlCmdInstance.GetACProductsList<ALanoClubUsersModel>(SqlConnectionStr, Scmd.SqlConstProp.SPGetALanoCLubUsers);
            if((AlanoCLubUsers != null) && (AlanoCLubUsers.Count >0))
                {
                ItemList = new ObservableCollection<ItemListModel>();


                ItemList.Add(new ItemListModel { Label = "Select User to Edit", Value = "0" });
                foreach (var item in AlanoCLubUsers)
                {
                    ItemList.Add(new ItemListModel { Label = $"{item.UserFirstName}-{item.UserLastName}", Value = item.ID.ToString() });
                }
                var i = ItemList.FirstOrDefault(i => i.Label == "Select User to Edit");
                MembersModel = i;
            }
        }
       
        public string UserFirstName
        {
            get => userFirstName;
            set
            {
                userFirstName = value;
                OnPropertyChanged(nameof(UserFirstName));
            }
        }
        public string UserLastName
        {
            get => userLastName;
            set
            {
                userLastName = value;
                OnPropertyChanged(nameof(UserLastName));
            }
        }
        public string UserEmailAddress
        {             get => userEmailAddress;
            set
            {
                userEmailAddress = value;
                OnPropertyChanged(nameof(UserEmailAddress));
            }
        }
        public string UserPasswordString
        {
                     get => userPasswordString;
            set
            {
                userPasswordString = value;
                OnPropertyChanged(nameof(UserPasswordString));
            }
        }
        public string UserPhoneNumber
        {
            get => userPhoneNumber;
            set
            {
                userPhoneNumber = value;
                OnPropertyChanged(nameof(UserPhoneNumber));
            }
        }


        private string SqlConnectionStr { get; set; }
        public ICommand ClearTxt
        {
            get
            {
                if (clearTxt == null)
                {
                    clearTxt = new RelayCommd(param => CreateNewUser(), param => CanGenerateReport());
                }
                return clearTxt;
            }
        }
        
            public ICommand DelUser
        {
            get
            {
                if (delUser == null)
                {
                    delUser = new RelayCommd(param => UserDel(), param => CanGenerateReport());
                }
                return delUser;
            }
        }
        public ICommand AddEditUser
        {
            get
            {
                if (addEditUser == null)
                {
                    addEditUser = new RelayCommd(AddUpdateUsers, param => CanGenerateReport());
                }
                return addEditUser;
            }
        }
        private ALanoClubUsersModel ClubUsersModel { get; set; }
        
        public bool EmailInfoUser
        {
            get => emailInfoUser;
            set
            {
                emailInfoUser = value;
                
                OnPropertyChanged(nameof(EmailInfoUser));
            }
        }
        public bool IsAdmin
        {
            get => isAdmin;
            set
            {
                isAdmin = value;
               
                OnPropertyChanged(nameof(IsAdmin));
            }
        }
        private bool CanGenerateReport()
        {
            // Add logic to determine if the command can execute
            return true; // For now, always return true
        }
        public async void CreateNewUser()
        {

                

            ID  = 0;
            UserName = string.Empty;
            UserPasswordString = string.Empty;
            UserFirstName = string.Empty;
            UserLastName = string.Empty;
            UserEmailAddress = string.Empty;
            UserPhoneNumber = string.Empty;
           EmailInfoUser = true;
            IsAcvite = true;
            IsAdmin = false;
            GenetatePw();
        }
        private async void UserDel()
        {
            if(ID != 0)
            {
                if(await Utilites.ALanoClubUtilites.ShowMessageBoxResults($"Delete user {UserFirstName} {UserLastName} UserName {UserName}","Delete User",MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    await Scmd.AlClubSqlCommands.SqlCmdInstance.DeleteByID(SqlConnectionStr, ID, Scmd.SqlConstProp.SPAlanoClubDeleByID);
                    CreateNewUser();
                    GetMembers();
                }

            }
        }
        private async void EditUserInfor()
        {
            if((MembersModel != null) && (!(string.IsNullOrWhiteSpace(MembersModel.Value))) && (string.Compare(MembersModel.Value,"0",false) != 0))
            {
                int uID = await Utilites.ALanoClubUtilites.ConvertToInt(MembersModel.Value);
                var upMember = AlanoCLubUsers.Where(p=>p.ID == uID).FirstOrDefault();
                if (upMember != null)
                {
                    ID=upMember.ID;
                    UserFirstName = upMember.UserFirstName; 
                    UserLastName = upMember.UserLastName;
                    UserEmailAddress = upMember.UserEmailAddress;
                    UserPhoneNumber = upMember.UserPhoneNumber;
                    UserName=upMember.UserName;
                    IsAdmin = upMember.IsAdmin;
                    IsAcvite = upMember.IsActive;
                }

            }
        }
        private async void GenetatePw()
        {
           
            UserPasswordString =   ALanoClubUtilites.GeneratePassword(10);
            OnPropertyChanged(nameof(ClubUsersModel));
        }
        private async void GetSqlConn()
        {
            SqlConnectionStr = Utilites.ALanoClubUtilites.GetSqlConnectionStrings(Utilites.ALanoClubUtilites.AlanoClubDatabaseName);

        }
        private async void AddUpdateUsers(object parameter)
        {
            if (string.IsNullOrEmpty(UserPasswordString))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox("Missing Password Please Generate A Password", "Missing Password", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                return;
            }
            if (string.IsNullOrEmpty(UserName))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox("Missing User Name", "Missing UserName", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                return;
            }
            if (string.IsNullOrEmpty(UserEmailAddress))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox("Missing User Email", "Missing Email", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                return;
            }

            else
            {
                if (!(await Utilites.ALanoClubUtilites.RexMatchStr(UserEmailAddress, Utilites.AlanoCLubConstProp.VailEmailAddress)))
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox($"Please Enter a Valid Member Email Adress {ClubUsersModel.UserEmailAddress}", "Invalid Member Email Adress", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }
                if (!(await Emails.EmailsInstance.IsValidEmail(UserEmailAddress)))
                {
                    Utilites.ALanoClubUtilites.ShowMessageBox($"Please Enter a Valid Member Email Adress {ClubUsersModel.UserEmailAddress}", "Invalid Member Email Adress", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }
            }
                if (string.IsNullOrEmpty(UserPhoneNumber))
            {
                UserPhoneNumber = Utilites.AlanoCLubConstProp.NA;
            }
            else
             if (!(await Utilites.ALanoClubUtilites.RexMatchStr(UserPhoneNumber, Utilites.AlanoCLubConstProp.VailPhoneNumber)))
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Please Enter a Valid Member Phone Number {UserPhoneNumber}", "Invalid Member Phone Number", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(UserFirstName))
            {
                UserFirstName = Utilites.AlanoCLubConstProp.NA;
            }
            if (string.IsNullOrEmpty(UserLastName))
            {
                UserLastName = Utilites.AlanoCLubConstProp.NA;
            }
            AddUserInfo();
            if (IsEditing)
            {
                Scmd.SqlUserService.UserServiceIntance.UpdateAdduser(ClubUsersModel, SqlConnectionStr, EmailInfoUser, SqlConstProp.SPAddUpdateAlanoClubUsers).ConfigureAwait(false).GetAwaiter().GetResult();
                ClubUsersModel.Salt = string.Empty;
                ClubUsersModel.UserPassword = Utilites.AlanoCLubConstProp.NA;
                
            }
            else
            {
                Scmd.SqlUserService.UserServiceIntance.UpdateAdduser(ClubUsersModel, SqlConnectionStr, EmailInfoUser, SqlConstProp.SPAddUpdateAlanoClubUsers).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            
            if (EmailInfoUser)
                SendUserEmail();
            CreateNewUser();
            GetMembers();

        }
        private async void SendUserEmail()
        {
            Utilites.CreateHtmlEMails createHtmlE = new Utilites.CreateHtmlEMails();
            var sb = await createHtmlE.CreateHeader(DateTime.Now, "Alano Club Inventory System New User Login Information", "pack://application:,,,/Resources/Images/butteac.ico");
            sb = await createHtmlE.SendNewUsereHtmlFile(ClubUsersModel, sb);
            sb = await createHtmlE.CreateSig(sb);
            sb = await createHtmlE.CloseHtmlFile(sb);
            // Specify the last parameter as (string)null to resolve ambiguity
            Emails.EmailsInstance.SendEmail(ClubUsersModel.UserEmailAddress, "buttealano@gmail.com", "Alano CLub Inventory Loging Information", sb.ToString(), true, (string)null);
        }
            // Add logic to add or update users here
            //   await ALanoClubUtilites.AddUpdateAlanoClubUserAsync(ClubUsersModel, EmailUser);
        private async void AddUserInfo()
        {
            ClubUsersModel = new ALanoClubUsersModel
            {
                ID = ID,
                UserName = UserName,
                UserPasswordString = UserPasswordString,
                UserFirstName = UserFirstName,
                UserLastName = UserLastName,
                UserEmailAddress = UserEmailAddress,
                UserPhoneNumber = UserPhoneNumber,
                IsAdmin = IsAdmin,
                IsActive = IsAcvite

            };
           
        }
        protected void OnPropertyChanged(string propertyName = null) =>
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
