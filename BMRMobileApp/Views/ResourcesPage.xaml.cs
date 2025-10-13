using BMRMobileApp.ViewModels;

namespace BMRMobileApp;

public partial class ResourcesPage : ContentPage
{
	public ResourceViewModel  resourceViewModel = new ResourceViewModel();
    private bool load = false;
	public ResourcesPage()
	{
		InitializeComponent();
		BindingContext=resourceViewModel;
     //   var browser = new WebView();
      //  browser.Source = "https://edocsusa.com";
		//BindingContext = browser;

    }

    private void CheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value) return; // Only respond when a box is checked

        var selected = sender as CheckBox;
        if ((load))
        {

            foreach (var child in CheckBoxGroup.Children)
            {
                if (child is CheckBox cb && cb != selected)
                {
                    cb.IsChecked = false;
                }
            }
        }
        load = true;
    }
    //(BindingContext as MainPageViewModel)?.ExpanderChangedCommand?.Execute(e.IsExpanded);
    private void ChangedCheckedOpenBrowser(object sender, CheckedChangedEventArgs e)
    {
        if ((load))
        {

            if (!resourceViewModel.CheckedOpenBrowser)
            {
                
                //resourceViewModel.CheckedOpenBrowser = true;
                resourceViewModel.CheckedOpenInternal = true;


            }
            if (!resourceViewModel.CheckedOpenInternal)
            {
                resourceViewModel.CheckedOpenBrowser = false;
                //resourceViewModel.CheckedOpenBrowser = true;
                //resourceViewModel.CheckedOpenInternal = true;


            }

        }
        load = true;
    }
    protected override void OnAppearing()
    {
 
    }

}