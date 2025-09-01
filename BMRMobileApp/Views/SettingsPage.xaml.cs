using System.ComponentModel;
using BMRMobileApp.Models;
using BMRMobileApp.SQLiteDBCommands;
namespace BMRMobileApp ;

public partial class SettingsPage : ContentPage,INotifyPropertyChanged
{
    private bool isSwitchOn;
    public event PropertyChangedEventHandler PropertyChanged;
    public SettingsPage()
	{

		InitializeComponent();
        BindingContext = this;
       


    }
   
    private async Task GetPSSetting()
    {
        SQLiteService cmd = new SQLiteService();
        var psSetting = await cmd.GetPsSettings();
        if (psSetting != null)
        {
            if(psSetting.ShowFeelingsPage == 1)
                IsSwitchOn = true;
            else
                IsSwitchOn = false;
        }
    }
    private async Task UPDatePsSetting(bool psSetting)
    {
        SQLiteService cmd = new SQLiteService();
        if (psSetting)
        {
           await cmd.AddSettings(1);
        }
        else
        {
            await cmd.AddSettings(0);
        }

    }
    public bool IsSwitchOn
    {
        get => isSwitchOn;
        set
        {
            if (isSwitchOn != value)
            {
                isSwitchOn = value;
                OnPropertyChanged(nameof(IsSwitchOn));
            }
        }
    }
    private async void OnSwitchToggled(object sender, ToggledEventArgs e)
    {
        bool isOn = e.Value;
       await UPDatePsSetting(isOn);


    }
    private async void OnEditProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("addedituserspage");
    }
    
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = GetPSSetting();

    }
}