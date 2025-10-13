namespace BMRMobileApp;

[QueryProperty(nameof(WebSitePage), "webSitePage")]

public partial class OpenWebPagePhonePage : ContentPage
{
    string webSitePage;

    public OpenWebPagePhonePage()
	{
		InitializeComponent();
        BindingContext = this;

    }
    public string WebSitePage
    {
        get => webSitePage;
        set
        {
            webSitePage = value;
            WebResource.Source = webSitePage; // Pass to ContentView
        }
    }

}