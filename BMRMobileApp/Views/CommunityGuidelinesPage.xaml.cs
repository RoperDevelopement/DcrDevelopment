using BMRMobileApp.SQLiteDBCommands;
using BMRMobileApp.InterFaces;
namespace BMRMobileApp;

public partial class CommunityGuidelinesPage : ContentPage
{
	public CommunityGuidelinesPage()
	{
		InitializeComponent();
	}
    private async void OnAcceptButtonClicked(object sender, EventArgs e)
    {
        var sqliteService = new SQLiteService();
        sqliteService.CreateDatabases().Wait();
        // Application.Current.MainPage = new AddEditUsersPage(true); 
        if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0] is Window window)
        {
            window.Page = new AddEditUsersPage(true);
        }


    }
    private async void OnDeclineButtonClicked(object sender, EventArgs e)
    {
        DependencyService.Get<IAppExitService>()?.RequestExitAsync();


    }
}