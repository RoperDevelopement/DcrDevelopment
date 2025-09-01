using BMRMobileApp.Models;
using BMRMobileApp.ViewModels;
namespace BMRMobileApp
{
	public partial class ChatViewPage : ContentPage
	{
        private readonly ChatViewModel viewModel;
        
        public ChatViewPage()
		{
			InitializeComponent();
			  viewModel = new ChatViewModel();
			BindingContext = viewModel;
			
        }
		 

    protected override void OnAppearing()
        {
            base.OnAppearing();
            if(!(viewModel.noChatHub))
                viewModel.EnsureHubConnectedAsync().Wait();
        }
    }
}