using BMRMobileApp.Models;
using BMRMobileApp.ViewModels;
using System.Threading.Tasks;

namespace BMRMobileApp;

public partial class FeelingPage : ContentPage
{
    FeelingViewModel feelingPage = new FeelingViewModel();
	public FeelingPage()
	{
		InitializeComponent();
		BindingContext = feelingPage;
         
    }
    private async void OnCloseClicked(object sender, EventArgs e)
    {
        if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0] is Window window)
        {
            window.Page = new AppShell();
        }

    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
          feelingPage?.StopVoiceJournalAsync();

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
       

    }
}