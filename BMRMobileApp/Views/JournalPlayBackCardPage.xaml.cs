using BMRMobileApp.ViewModels;
using System.Threading.Tasks;

namespace BMRMobileApp;
public partial class JournalPlayBackCardPage : ContentPage
{
	public readonly JournalPlayBackCardView journalPlayBackCardPage = new JournalPlayBackCardView();
	public JournalPlayBackCardPage()
	{
		InitializeComponent();
		BindingContext=journalPlayBackCardPage;
       

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        //if (BindingContext is JournalPlayBackCardView vm)
        //{
        //    vm.Init();
        //}
    }
    protected override void OnDisappearing()
    {

        base.OnDisappearing();
        if (BindingContext is JournalPlayBackCardView vm)
        {
            
            vm.StopPlayback();
        }

        Task.Delay(3000);
    }
}