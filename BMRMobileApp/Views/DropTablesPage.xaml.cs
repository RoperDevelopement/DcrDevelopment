using BMRMobileApp.SQLiteDBCommands;
namespace BMRMobileApp
{ 
public partial class DropTablesPage : ContentPage
{
	public DropTablesPage()
	{
		InitializeComponent();
	}
    private async void OnClickDropTables(object sender, EventArgs e)
	{
			SQLiteService cmd = new SQLiteService();
		await	cmd.DropCreteTables();
            await Application.Current.Windows[0].Page.DisplayAlert("Message","Done Dropping Creating Tables", "OK");   // Handle permissions or capture errors

            if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0] is Window window)
{
  window.Page = new AppShell();
}

        }
    }
}