
using BMRMobileApp.Utilites;

namespace BMRMobileApp;

public partial class SearchWebPage : ContentPage
{
	public SearchWebPage()
	{
		InitializeComponent();
		Title = "Search Web";
		BackgroundColor = Color.FromArgb(Utilites.Consts.DefaultBackgroundColor);
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

    private async void GetMood()
    {
        SQLiteDBCommands.SQLiteService sQLiteService = new SQLiteDBCommands.SQLiteService();
        var mood = sQLiteService.GetUserCurrentMoodNonAsync();
        if (mood != null)
        {
            Title = $"{Title} {mood.Mood} {mood.MoodTag}";
            BackgroundColor = Color.FromArgb(mood.BackgroundColor);
        }
    }

}