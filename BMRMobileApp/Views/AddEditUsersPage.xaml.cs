using BMRMobileApp.Models;
using BMRMobileApp.SQLiteDBCommands;
using BMRMobileApp.Utilites;
using BMRMobileApp.ViewModels;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BMRMobileApp
{
    public partial class AddEditUsersPage : ContentPage
    {
private        string cPText;
      private  Color cPBC;

        private readonly SQLiteService sqliteService = new SQLiteService();
        private readonly bool isAddUser;
        private bool DisplayNameIsValid = true;
        UploadPictureViedosFiles picture = new UploadPictureViedosFiles();
        private string userState = string.Empty;
        
        public AddEditUsersPage(bool addUser = false)
        {
            InitializeComponent();
            BindingContext = this;
            isAddUser = addUser;
            BackgroundColor = Color.FromArgb(Utilites.Consts.DefaultBackgroundColor);
            Title = "User Profile";
            if(addUser)
                Title = " Edit User Profile";

            if (addUser)
            {
                DisplayCancel.IsVisible = false;
                Title = "Add User";
                DisplayText.Text = "Add User";
                ProfileImage.Source = "peersupportgroupimage.png";


            }
            else
            {
                DisplayCancel.IsVisible = true;
                Title = "Edit User";
                DisplayText.Text = "Update User";
            }
            // Uncomment the following line if you want to bind the page to a ViewModel
            //	BindingContext = new UsersModel();
          //   GetStateNames();
            if (!addUser)
                GetUserInformation();
            ProfilePictureFolderPath = Microsoft.Maui.Storage.FileSystem.AppDataDirectory;
            GetMood();
        }
        public string CPText
        {
            get => cPText;
            set
            {
                cPText = value;
                OnPropertyChanged(nameof(CPText));
            }
        }
        public Color CPBC
        {
            get => cPBC;
            set
            {
                cPBC = value;
                OnPropertyChanged(nameof(CPBC));
            }
        }


        private async void GetMood()
        {
            var mood = sqliteService.GetUserCurrentMoodNonAsync();
            if( mood != null )
            {
                Title = $"{Title} {mood.Mood} {mood.MoodTag}";
                BackgroundColor = Color.FromArgb(mood.BackgroundColor);
            }
        }
        private async Task GetUserInformation()
        {
            SQLiteService sqliteService = new SQLiteService();
            // Fetch user information from the database
            var user = await sqliteService.GetPsUsers();
            if (user != null)
            {

                PSUserFirstName.Text = user.PSUserFirstName;
                PSUserLastName.Text = user.PSUserLastName;
                PSUserPhoneNumber.Text = user.PSUserPhoneNumber;
                SelectedOption = user.PSUserState;
                PSUserCountry.Text = user.PSUserCountry;
                //PSUserCity.Text = "N/A",//user.PSUserCity;
                //  PSUserAddress.Text = user.PSUserAddress;
                PSUserDateOfBirth.Date = DateTime.Parse(user.PSUserDateOfBirth);
                PSUserZipCode.Text = user.PSUserZipCode;
                PSUserNotes.Text = user.PSUserNotes;
                PSUserCountry.Text = user.PSUserCountry;
                //tatusPickerStates.SelectedItem = user.PSUserState;
            }
            var userLogin = await sqliteService.GetPsUsersLogin();

            if (userLogin != null)
            {
                PSEmail.Text = userLogin.PSUserEmail;
                PSEmail.IsReadOnly = true;
                PSEmail.BackgroundColor = Colors.LightGray;
                PSUserPassword.Text = userLogin.PSUserPassword;
                PSUserPassword.IsEnabled = false;
                
                ReTypePassWord.Text = userLogin.PSUserPassword;
                ReTypePassWord.IsEnabled = false;
                PSUserDisplayName.Text = userLogin.PSUserDisplayName;
                if (userLogin.PSUserProfilePicture != null && userLogin.PSUserProfilePicture != string.Empty)
                {
                    ProfileImage.Source = userLogin.PSUserProfilePicture;
                    ProfilePictureFolderPath = userLogin.PSUserProfilePicture;
                }
                if (userLogin.PSUserAvatar != null)
                {
                    var avatarStr = await Utilites.PSUtilites.ConvertByteToStr(userLogin.PSUserAvatar);
                    if (!string.IsNullOrEmpty(avatarStr))
                    {
                        PSUserAvatar.SelectedItem = avatarStr;
                    }
                }
                Male.IsChecked = true;
                Male.IsEnabled = false;
                Female.IsEnabled = false;
                if (userLogin.PSUserSex == 0)
                    Female.IsChecked = true;

            }

        }

        //private async Task GetStateNames()
        //{
        //    StateService stateName = new StateService();
        //    States = await stateName.GetStatesAsync();
        //    foreach (var state in States)
        //    {
        //        StatusPickerStates.Items.Add(state.Abbreviation);
        //    }
        //    //  StatusPickerStates.SelectedIndex = 0; // Set default selection to the first state
        //}
        public string SelectedOption { get; set; }

        private IList<StateInfoModel> States { get; set; }
        private bool UserDiaplayNameIsValid()
        {
            if (string.IsNullOrWhiteSpace(PSUserDisplayName.Text))
            {
                DisplayAlert("Validation Error", "Display Name is required.", "OK");
                DisplayNameIsValid = false;
                PSUserDisplayName.Focus();
                return false;
            }
            return true;
        }
        private bool UserEmailIsValid()
        {
            if (string.IsNullOrWhiteSpace(PSEmail.Text) || !PSEmail.Text.Contains("@"))
            {
                DisplayAlert("Validation Error", "A valid email is required.", "OK");
                return false;
            }
            return true;
        }
        private bool UserPasswordIsValid()
        {
            if (string.IsNullOrWhiteSpace(PSUserPassword.Text) || PSUserPassword.Text.Length < 6)
            {
                DisplayAlert("Validation Error", "Password must be at least 6 characters long.", "OK");
                return false;
            }
            if (PSUserPassword.Text != ReTypePassWord.Text)
            {
                DisplayAlert("Validation Error", "Passwords do not match.", "OK");
                return false;
            }
            return true;
        }
        private bool UserFirstNameIsValid()
        {
            if (string.IsNullOrWhiteSpace(PSUserFirstName.Text))
            {
                DisplayAlert("Validation Error", "First Name is required.", "OK");
                return false;
            }
            return true;
        }
        private bool UserLastNameIsValid()
        {
            if (string.IsNullOrWhiteSpace(PSUserLastName.Text))
            {
                DisplayAlert("Validation Error", "Last Name is required.", "OK");
                return false;
            }
            return true;
        }
        private string ProfilePictureFolderPath
        { get; set; }
        private async Task GetProfilePictureFolder(string profilePic)
        {
          // var ext = Path.GetExtension(profilePic);
           ProfilePictureFolderPath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, $"profilepictures.png");
           string cachePath = Path.Combine(FileSystem.CacheDirectory, "profilepictures.png"); 
            if (File.Exists(cachePath))
                File.Delete(cachePath);
            if (File.Exists(ProfilePictureFolderPath))
            {
                File.Delete(ProfilePictureFolderPath);
            }
            using var inputStream = File.OpenRead(profilePic);
           var image = PlatformImage.FromStream(inputStream);
            using var outputStream = File.Create(ProfilePictureFolderPath);
            image.Save(outputStream, ImageFormat.Png);
            
            // byte[] currPic = File.ReadAllBytes(profilePic);
            // File.WriteAllBytes(ProfilePictureFolderPath, currPic);
        }
        private bool UserPhoneNumberIsValid()
        {
            if (string.IsNullOrWhiteSpace(PSUserPhoneNumber.Text) || PSUserPhoneNumber.Text.Length < 10)
            {
                DisplayAlert("Validation Error", "A valid phone number is required.", "OK");
                return false;
            }
            if (!long.TryParse(PSUserPhoneNumber.Text, out _))
            {
                DisplayAlert("Validation Error", "Phone number must be numeric.", "OK");
                return false;
            }
            return true;
        }
        private bool UserStateIsValid()
        {
            //if (string.IsNullOrWhiteSpace(PSUserState.Text))
            //{
            //    DisplayAlert("Validation Error", "State is required.", "OK");
            //    return false;
            //}
            return true;
        }
        private bool UserCountryIsValid()
        {
            if (string.IsNullOrWhiteSpace(PSUserCountry.Text))
            {
                DisplayAlert("Validation Error", "Country is required.", "OK");
                return false;
            }
            return true;
        }
        //private bool UserCityIsValid()
        //{
        //    if (string.IsNullOrWhiteSpace(PSUserCity.Text))
        //    {
        //        DisplayAlert("Validation Error", "City is required.", "OK");
        //        return false;
        //    }
        //    return true;
        //}
        private bool UserZipCodeIsValid()
        {
            if (string.IsNullOrWhiteSpace(PSUserZipCode.Text) || PSUserZipCode.Text.Length < 5)
            {
                DisplayAlert("Validation Error", "A valid zip code is required.", "OK");
                return false;
            }
            return true;
        }
        private bool UserDateOfBirthIsValid()
        {
            if (PSUserDateOfBirth.Date > DateTime.Now)
            {
                DisplayAlert("Validation Error", "Date of Birth cannot be in the future.", "OK");
                return false;
            }
            return true;
        }
        private bool UserNotesIsValid()
        {
            // Assuming notes can be empty, so no validation needed
            return true;
        }
        private bool ValidateUserInput()
        {
            return UserDiaplayNameIsValid() &&
                   UserEmailIsValid() &&
                   UserPasswordIsValid() &&
                   UserFirstNameIsValid() &&
                   UserLastNameIsValid() &&
                   UserPhoneNumberIsValid() &&
                   
                   UserCountryIsValid() &&
                    
                   UserZipCodeIsValid() &&
                   UserDateOfBirthIsValid() &&
                   UserNotesIsValid();
        }

        private async void OnClickedCancel(object sender, EventArgs e)
        {
            if  ( Application.Current?.Windows.Count > 0 && Application.Current.Windows[0] is Window window)
            {
                window.Page = new AppShell();
            }

        }
        private async void OnSaveUpdateClicked(object sender, EventArgs e)
        {
            try
            {
                if (ValidateUserInput())
                {
                    //        var user = new UsersModel
                    //        {
                    //       public string FirstName { get; set; }
                    //    public string LastName { get; set; }
                    //    public string UserName { get; set; }
                    //    public string Email { get; set; }
                    //    public string Password { get; set; }
                    //    public string Address { get; set; }
                    //    public string PhoneNumber { get; set; }
                    //    public string ProfilePicture { get; set; } // URL or path to the profile picture
                    //    public DateTime DateOfBirth { get; set; } // User's date of birth
                    //    public string State { get; set; } // User's state or region
                    //    public string Country { get; set; } // User's country
                    //    public string Avatar { get; set; } // URL or path to the user's avatar image
                    //    public DateTime DateJoined { get; set; } // Date when the user joined the platform  
                    //};

                    // TODO: Save to SQLite or send to API
                    // await DisplayAlert("Saved", $"Customer {FirstName.Text} saved successfully!", "OK");
                    // var t = PSUserAvatar.SelectedItem;
                    //var ti = PSUserAvatar.SelectedItem as string;

                    await AddUPDatePSUserAsync();
                }
                else
                {
                    return; // Validation failed, do not proceed
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred while saving the user: {ex.Message}", "OK");
            }
            //  Application.Current.MainPage = new AppShell();
            if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0] is Window window)
            {

                window.Page = new AppShell();
            }

        }
        //private void OnStatusChanged(object sender, EventArgs e)
        //{
        //    userState = StatusPickerStates.SelectedItem?.ToString();
        //    //   Console.WriteLine($"Selected status: {selectedStatus}");
        //}
        private async Task AddUPDatePSUserAsync()
        {
            int psUserSex = 1;
            if (Female.IsChecked)
                psUserSex = 0;

            var user = new PSupportUsers
            {
                // Set to 0 for new users, or fetch existing ID for updates
                PSUserFirstName = PSUserFirstName.Text,
                PSUserLastName = PSUserLastName.Text,
                PSUserPhoneNumber = PSUserPhoneNumber.Text,
                PSUserState = "N/A",
                //"StatusPickerStates.SelectedItem.ToString(),
                //  PSUserState = SelectedOption,
                PSUserCountry = PSUserCountry.Text,
                PSUserCity = "N/A",//PSUserCity.Text,
                PSUserAddress = "N/A", // Assuming address is not required for now
                PSUserDateOfBirth = PSUserDateOfBirth.Date.ToString("MM/dd/yyyy"), // Format as needed
                PSUserZipCode = PSUserZipCode.Text,
                PSUserNotes = PSUserNotes.Text,
                PSUserDateJoined = DateTime.Now.ToString("MM/dd/yyyyy"), // Set current date as joined date
            };
            if (isAddUser)
            {
                // Add new user
                await sqliteService.AddPSUser(user);

                //  var t = await sqliteService.GetPSUsers();
            }
            else
            {
                // Update existing user
                await sqliteService.UpdatePSUser(user);

            }

            var userLogin = new PSUsersLogin
            {

                PSUserName = PSEmail.Text,
                PSUserPassword = PSUserPassword.Text,
                PSUserEmail = PSEmail.Text,
                PSUserProfilePicture = ProfilePictureFolderPath, // URL or path to the profile picture
                PSUserAvatar = System.Text.Encoding.UTF8.GetBytes(PSUserAvatar.SelectedItem.ToString()), // URL or path to the user's avatar image
                PSUserDisplayName = PSUserDisplayName.Text, // Display name for the user
                PSUserLastLogin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), // Last login date and time
                PSUserPassWordLastChanged = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), // Last password change date and time
                PSUserSex = psUserSex



        };

            await PSUtilites.GetUserID(PSUserFirstName.Text.Trim(), PSUserLastName.Text.Trim());
            userLogin.ID = PSUtilites.UserID;
            var results = PSUtilites.HashPassword(userLogin.PSUserPassword);
            userLogin.PSPasswordHash = results.Hash;
            userLogin.PSUserPWSalt = results.Salt;
            // Add new user login
            if (isAddUser)
            {
                await sqliteService.AddPSUserLogin(userLogin);
            }
            else
            {
                await sqliteService.UpdatePSUserLogin(userLogin);
            }
            //await DisplayAlert("Success", "Added successfully!", "OK");

            await UpDateLoginIngfo(userLogin);
        }

        private async Task UpDateLoginIngfo(PSUsersLogin pSUsersLogin)
        {
            PsUserLoginModel.instance.Value.ID = pSUsersLogin.ID;
            PsUserLoginModel.instance.Value.PSUserName = pSUsersLogin.PSUserName;
            PsUserLoginModel.instance.Value.PSUserAvatar = await Utilites.PSUtilites.ConvertByteToStr(pSUsersLogin.PSUserAvatar);
            PsUserLoginModel.instance.Value.PSUserDisplayName = pSUsersLogin.PSUserDisplayName;
            PsUserLoginModel.instance.Value.PSUserEmail = pSUsersLogin.PSUserEmail;
            PsUserLoginModel.instance.Value.PSUserProfilePicture =  pSUsersLogin.PSUserProfilePicture;
            PsUserLoginModel.instance.Value.PSUserSex = "Male";
            if (Female.IsChecked)
                PsUserLoginModel.instance.Value.PSUserSex = "Female";
            
            
            
        }
        private void OnClickTakePhoto(object sender, EventArgs e)
        {

            TakeProfilePhoto();
        }
        private void OnClickUploadPhoto(object sender, EventArgs e)
        {
            UploadProfilePhoto();

        }
        private async Task UploadProfilePhoto()
        {

            var uploadPic = await picture.PickFileAsync();
            if (uploadPic != null)
            {
                GetProfilePictureFolder(uploadPic.FullPath);
                ProfileImage.Source = uploadPic.FullPath;
            }


        }
        private async Task TakeProfilePhoto()
        {
            var takePic = await picture.TakePhotoAsync();
            if (takePic != null)
            {
                GetProfilePictureFolder(takePic);
                ProfileImage.Source = takePic;
                
                //byte[] pic = await Utilites.PSUtilites.ConvertImageToBlod(takePic);
                // ProfileImage.Source = await Utilites.PSUtilites.ConvertBlodToImageSource(pic);
            }


        }


    }
}
//oid ShowEmptyState()
// {
//   TitleLabel.Text = "Hmm... nothing here yet 🤔";
//  DescriptionLabel.Text = "Try refreshing or check back later. We're still warming up the vibes.";
// EmptyStateEmoji.Source = EmojiAssetManager.Get("thinking");
// BackgroundColor = Color.FromHex("#FCEFEF"); // Soft fallback tone
// }
//if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0] is Window window)/
//{
//  window.Page = new AppShell();
//}