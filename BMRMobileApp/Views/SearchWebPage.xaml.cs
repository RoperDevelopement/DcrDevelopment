
using BMRMobileApp.Utilites;

namespace BMRMobileApp;

public partial class SearchWebPage : ContentPage
{
	public SearchWebPage()
	{
		InitializeComponent();
    }
    private async void OnSearchButtonClicked(object sender, EventArgs e)
	{
		Utilites.SearchWeb searchWeb = new Utilites.SearchWeb();
		string query = SearchEntry.Text;
		if (!string.IsNullOrWhiteSpace(query))
		{
			await searchWeb.SearchBingAsync(query);
        }
    }
}