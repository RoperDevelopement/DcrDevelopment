using System.ComponentModel;
using BMRMobileApp.Models;
using BMRMobileApp.SQLiteDBCommands;
using CommunityToolkit.Mvvm.ComponentModel;
namespace BMRMobileApp;

public partial class SettingsPage : ContentPage, INotifyPropertyChanged
{
    private bool isSwitchOn;
    private bool isAutoScroll;
    public event PropertyChangedEventHandler PropertyChanged;
    private bool loading=true;
    private readonly SQLiteService cmd = new SQLiteService();
    private   Color bc;
    public SettingsPage()
    {

        InitializeComponent();
        BindingContext = this;
        Title = "Change Settings";
        BackgroundColor = Color.FromArgb(Utilites.Consts.DefaultBackgroundColor);
        BC = this.BackgroundColor;
        GetMood();
    }
   
    private async void AddAutoScrollTimes()
    {

        AutoScrollDelay.Items.Clear();
        for (int i = 1; i < 61; i++)
        {
            AutoScrollDelay.Items.Add($"{i} Sec");

        }
        for (int i = 1; i < 5; i++)
        {
            AutoScrollDelay.Items.Add($"{i} Hour");

        }


    }
    public Color BC
    {
        get => bc;
        set
        {
            
            
                bc = value;
                OnPropertyChanged(nameof(BC));
            
        }
    }
    private async void GetMood()
    {
        var mood = cmd.GetUserCurrentMoodNonAsync();
        if (mood != null)
        {
            Title = $"{Title} {mood.Mood} {mood.MoodTag}";
            BackgroundColor = Color.FromArgb(mood.BackgroundColor);
        }
        BC = this.BackgroundColor;
        

    }

    private async Task GetPSSetting()
    {

        IsAutoScroll = false;
        IsSwitchOn = false;
        var psSetting = await cmd.GetPsSettings();
        
        if (psSetting != null)
        {
            if (psSetting.ShowFeelingsPage == 1)
                            IsSwitchOn = true;

                

            if (psSetting.AutoScroll == 1)
                         IsAutoScroll = true;
         
        }
        
        OnPropertyChanged(nameof(IsSwitchOn));
        OnPropertyChanged(nameof(IsAutoScroll));
        AutoScrollDelay.SelectedItem = psSetting.ScrollWaits;
        loading = false;

    }
    private async Task UPDatePsSetting(bool psSetting)
    {
        
        if (psSetting)
        {
            await cmd.AddSettings("ShowFeelingsPage", "1");
        }
        else
        {
             await cmd.AddSettings("ShowFeelingsPage", "0");
        }

    }
    private async Task UPDateToggleSettons()
    {

        if (IsSwitchOn)
        {
            await cmd.AddSettings("ShowFeelingsPage", "1");
        }
        else
        {
            await cmd.AddSettings("ShowFeelingsPage", "0");
        }
        if (IsAutoScroll)
        {
            await cmd.AddSettings("AutoScroll", "1");
        }
        else
        {
            await cmd.AddSettings("AutoScroll", "0");
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
    public bool IsAutoScroll
    {
        get => isAutoScroll;
        set
        {
            if (isAutoScroll != value)
            {
                isAutoScroll = value;
                OnPropertyChanged(nameof(IsAutoScroll));
            }
        }
    }
    private async void OnSwitchToggled(object sender, ToggledEventArgs e)
    {
        
        bool isOn = e.Value;
      //  if (!loading)
        //await UPDatePsSetting(isOn);


    }
    private async void OnEditProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddEditUsersPage));
    }

    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    protected override void OnAppearing()
    {
        base.OnAppearing();
        AddAutoScrollTimes();
        _ = GetPSSetting();

    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        UPDateToggleSettons();
    }

    private async void AutoSccroll_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        var delayAcroll = sender as Picker;
        var dTime = delayAcroll.SelectedItem as string;
        if (dTime != null)
        {
            if(!loading)
            await cmd.AddSettings("ScrollWaits", $"'{dTime}'");
        }
         

    }

    private async void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        
            var isOn = e.Value;
        //if (isOn)
        //{
        //    if (!loading)
        //        await cmd.AddSettings("AutoScroll", "1");
        //}
        //else
        //{
        //    await cmd.AddSettings("AutoScroll", "0");
        //}

    }
}