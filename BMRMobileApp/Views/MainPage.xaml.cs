using BMRMobileApp.Models;
using BMRMobileApp.ViewModels;
using CommunityToolkit.Maui.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BMRMobileApp;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private HubConnection _hubConnection;
 //   public readonly MainPageViewModel mainPageViewModel = new MainPageViewModel();
    //private readonly UserMoodModle userMoods;
    private readonly SQLiteDBCommands.SQLiteService sqLCmd;
    
    public MainPageViewModel mainPageViewModel = new MainPageViewModel();
    public ICommand NavigateCommand { get; set; }
    //  Image profImg = new Image();
    //public MainPage(UserMoodModle scoresModel, SQLiteDBCommands.SQLiteService sQLiteService)
    public MainPage(SQLiteDBCommands.SQLiteService sQLiteService)
    {
        InitializeComponent();


        // await Shell.Current.GoToAsync("loginpage");


        //NavigateCommand = new Command<Type>(
        //    async (Type pageType) =>
        //    {
        //        Page page = (Page)Activator.CreateInstance(pageType);
        //        await Navigation.PushAsync(page);
        //        // If you want to use a NavigationPage, uncomment the next line
        //        //  await Navigation.PushAsync(page);
        //        //  await navigationPage.Navigation.PushAsync(page);
        //        // await Navigation.PushModalAsync(new NavigationPage(new Type(page)))
        //    });
      //  userMoods = scoresModel;
        sqLCmd = sQLiteService;
        BindingContext = mainPageViewModel;
        //private readonly UserMoodModle userMoods;;



    }

    private async void ConnectToHub()
    {
        try
        {
            await _hubConnection.StartAsync();
        }
        catch (Exception ex)
        {
            // Handle connection errors
        }
    }
    // [Bindable(true)]
    //public Image ImgProfPic
    //{
    //     get => profImg;
    //    //get {
    //    //    if (System.IO.File.Exists(PsUserLoginModel.instance.Value.PSUserProfilePicture))
    //    //    {
    //    //        profImg.Source = ImageSource.FromFile(PsUserLoginModel.instance.Value.PSUserProfilePicture);
    //    //    }
    //    //    else
    //    //    {

    //    //          ImgProfPic.Source = ImageSource.FromFile("psloadimg.png");
    //    //    }

    //    //       return profImg;
    //    //}
    //    set
    //    {
    //         profImg = value;
    //        OnPropertyChanged(nameof(ImgProfPic));
    //      //  set => SetProperty(ref profImg, value);
    //    }   
    //}
    private  async void GetMoodSettings()
    {
        try
        {
            Title = $"Dashboard Hello {PsUserLoginModel.instance.Value.PSUserDisplayName}";
            Mood.IsVisible = false;
            BackgroundColor = Color.FromArgb(Utilites.Consts.DefaultBackgroundColor);
            //if (string.IsNullOrEmpty(userMoods.Mood))
            //{
            //    // var tExists = sqLCmd.TableExists("PSUserMood");

            //    var moods = await sqLCmd.GetPSUserMood();
            //    if (moods == null) return;
            //    userMoods.Mood = moods.Mood;
            //    userMoods.SentimentScore = moods.SentimentScore;
            //    userMoods.BackgroundColor = moods.BackgroundColor;
            //    BackgroundColor = Color.FromArgb(userMoods.BackgroundColor);
            //    userMoods.ModIcon = moods.Mood switch
            //    {
            //        "Happy" => Utilites.EmojiTags.Happy,
            //        "Calm" => Utilites.EmojiTags.Calm,
            //        "Confused" => Utilites.EmojiTags.Confused,
            //        "Sad" => Utilites.EmojiTags.Sad,
            //        "Angry" => Utilites.EmojiTags.Frustrated,
            //        _ => Utilites.EmojiTags.Calm,
            //    };
            //}
            //

            //Mood.Text = $"Today my mood is  {userMoods.ModIcon} {userMoods.Mood}";
            //BackgroundColor = Color.FromArgb(userMoods.BackgroundColor);
            ////  BackgroundColor = Color.FromArgb(userMoods.BackgroundColor);
            //if (userMoods.SentimentScore > 0)
            //{
            //    Mood.Text = $"{Mood.Text} My Sentiment Score is {userMoods.SentimentScore} % Positive";
            //}
            ////else
            //// {
            ////   Mood.Text = $"{Mood.Text} My Sentiment Score is {userMoods.SentimentScore} % Negative";
            ////}
            var usermood = await sqLCmd.GetUserCurrentMood();
            if (usermood != null)
            {
                Mood.IsVisible = true;
                Mood.Text = $"Today my mood is {usermood.Mood} {usermood.MoodTag}";
                BackgroundColor = Color.FromArgb(usermood.BackgroundColor);
                Title = $"{Title} {usermood.Mood} {usermood.MoodTag}";
            }

        }
        catch (Exception ex)
        {
            Mood.IsVisible = false;
            // Handle exceptions
        }

        // var moodSettings = await confidenceScores.GetMoodAsync("I am very happy today");
        // Use the moodSettings as needed
    }
    private void Init()
    {
        //  ProfileImage.Source = await Utilites.PSUtilites.ConvertBlodToImageSource(PsUserLoginModel.instance.Value.PSUserProfilePicture);
        //return Task.Run(async () =>
        //{
        if (System.IO.File.Exists(PsUserLoginModel.instance.Value.PSUserProfilePicture))
        {
            //   ImageSource FileImage = ImageSource.FromFile(PsUserLoginModel.instance.Value.PSUserProfilePicture);
            // ProfileImage.Source = FileImage;// PsUserLoginModel.instance.Value.PSUserProfilePicture;
            //  ImgProfPic.Source = ImageSource.FromFile(PsUserLoginModel.instance.Value.PSUserProfilePicture);
            ProfImg.Source = ImageSource.FromFile(PsUserLoginModel.instance.Value.PSUserProfilePicture);
            //  using var stream = File.OpenRead(PsUserLoginModel.instance.Value.PSUserProfilePicture);
            // ImgProfPic.Source = ImageSource.FromStream(() => stream);
        }

        else
            ProfImg.Source = ImageSource.FromFile("psloadimg.png");
        //  ProfileImage.Source = PsUserLoginModel.instance.Value.PSUserProfilePicture;

       
        //BackgroundColor = Color.FromArgb(f7");
        //if (File.Exists(PsUserLoginModel.instance.Value.PSUserProfilePicture))
        // {
        //   ProfileImage.Source = ImageSource.FromFile(PsUserLoginModel.instance.Value.PSUserProfilePicture);
        // ProfileImage.Source = await  Utilites.PSUtilites.ConvertBlodToImageSource(PsUserLoginModel.instance.Value.PSUserProfilePicture);
        //}
        //else
        // {
        //ProfileImage.Source = "peersupportgroupimage.png";
        // }
        // });
        LabelEntry.Text = $"Hey {PsUserLoginModel.instance.Value.PSUserDisplayName} you’re not alone. Let’s check in together.";
        GetMoodSettings();

        //        using var stream = await FileSystem.OpenAppPackageFileAsync("Images/image.png");
        //      image.Source = ImageSource.FromStream(() => stream);

    }

    private void Login()
    {         // Navigate to the login page
        Shell.Current.GoToAsync("loginpage");
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Fire and forget the async call, handle exceptions as needed
        Init();

        //    OnPropertyChanged(nameof(ImgProfPic));
      //  Shell.SetTabBarIsVisible(this, true);
         MainThread.BeginInvokeOnMainThread(() =>
          {
            Shell.SetTabBarIsVisible(this, true);
         });
        //0 viedoNotes = new ObservableCollection<ViedoNotesModel>();
    }
    protected override void  OnDisappearing()
    {
        
        base.OnDisappearing();
        if (BindingContext is MainPageViewModel vm)
        {
            vm.AutoScroll = false;
            vm.ExitMain = true;
        }
        
        Task.Delay(3000);
    }
         
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private void CarouselView_PositionChanged(object sender, PositionChangedEventArgs e)
    {
        Console.WriteLine();
    }

    private void Expander_ExpandedChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
    {
        var expander = sender as Expander;
        (BindingContext as MainPageViewModel)?.ExpanderChangedCommand?.Execute(e.IsExpanded);
    }

}
 