
using BMRMobileApp.SQLiteDBCommands;
using BMRMobileApp.Utilites;
using BMRMobileApp.Models;
namespace BMRMobileApp;


public partial class LoginPage : ContentPage
{

    public LoginPage()
    {

        InitializeComponent();
        BackgroundColor= Color.FromArgb(Utilites.Consts.DefaultBackgroundColor);
        //  pLogin = psUserLoginMode;

    }
    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;
        if (!(File.Exists(Path.Combine(FileSystem.AppDataDirectory, "dcrpeersuppotusers.db"))))
        {
            //{
            //    File.Delete(Path.Combine(FileSystem.AppDataDirectory, "dcrpeersuppotusers.db"));
            //}
            // Simulate authentication (replace with real logic)
            if (username == "dan" && password == "123")
            {
                await DisplayAlert("Success", "Login successful!", "OK");
                // Navigate to the main page
                await Navigation.PushModalAsync(new FeelingPage(false));
            }
            //  else if (string.IsNullOrWhiteSpace(username))
            // {
            //     await DisplayAlert("Success", "Login successful!", "OK");
            // Navigate to the main page


            //   await Navigation.PushModalAsync(new FeelingPage());
            // }
            else
            {
              
                ErrorMessage.Text = "Invalid username or password.";
                ErrorMessage.IsVisible = true;
            }
        }
        else
        {
            UsernameEntry.Text = "mtcharles@hotmail.com";
            PasswordEntry.Text = "123456";
            await CheckUserNamePassword();
        }
    }
    private async Task CheckUserNamePassword()
    {
         SQLiteDBCommands.SQLiteService sqliteService = new SQLiteDBCommands.SQLiteService();
     //  await sqliteService.DropCreteTables();
        // PsUserLoginModel psUserLoginModel = new PsUserLoginModel();
        //var t = await sqliteService.GetPSUsers();
        // Check if the user exists in the database
        var user = await sqliteService.GetPSUserPassword(UsernameEntry.Text);

        if (user != null)
        {
            bool password = Utilites.PSUtilites.VerifyPassword(PasswordEntry.Text, user.PSUserPWSalt, user.PSPasswordHash);
            if (password)
            {
                // Set the user ID in the utility class
                PsUserLoginModel.instance.Value.ID = user.ID;
                PsUserLoginModel.instance.Value.PSUserName = user.PSUserName;
                PsUserLoginModel.instance.Value.PSUserAvatar = await Utilites.PSUtilites.ConvertByteToStr(user.PSUserAvatar);
                PsUserLoginModel.instance.Value.PSUserDisplayName = user.PSUserDisplayName;
                PsUserLoginModel.instance.Value.PSUserEmail = user.PSUserEmail;
                PsUserLoginModel.instance.Value.PSUserProfilePicture = user.PSUserProfilePicture;
                if (user.PSUserSex == 0)
                    PsUserLoginModel.instance.Value.PSUserSex = "Female";
                else if (user.PSUserSex == 1)
                    PsUserLoginModel.instance.Value.PSUserSex = "Male";   

                // Navigate to the main page
                var psSetting = await sqliteService.GetPSSettings();
                if (psSetting == null)
                {
                    await Navigation.PushModalAsync(new FeelingPage(false));
                }
                else if (psSetting.ShowFeelingsPage == 1)
                    await Navigation.PushModalAsync(new FeelingPage(false));
                else
                {
                    if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0] is Window window)
                    {
                        window.Page = new AppShell();
                    }
                }
                    
            }
            else
            {
                ErrorMessage.Text = "Invalid username or password.";
                ErrorMessage.IsVisible = true;
            }
            // Navigate to the main page

        }
        else
        {
            ErrorMessage.Text = "Invalid username or password.";
            ErrorMessage.IsVisible = true;
        }
    }
}

