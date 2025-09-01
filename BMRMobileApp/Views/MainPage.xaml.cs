using BMRMobileApp.Models;
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
    public ICommand NavigateCommand { get; set; }
    Image profImg = new Image();
    public MainPage()
    {
        InitializeComponent();
        
       
        // await Shell.Current.GoToAsync("loginpage");


        NavigateCommand = new Command<Type>(
            async (Type pageType) =>
            {
                Page page = (Page)Activator.CreateInstance(pageType);
                await Navigation.PushAsync(page);
                // If you want to use a NavigationPage, uncomment the next line
                //  await Navigation.PushAsync(page);
                //  await navigationPage.Navigation.PushAsync(page);
                // await Navigation.PushModalAsync(new NavigationPage(new Type(page)))
            });

        BindingContext = this;

       

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
     [Bindable(true)]
    public Image ImgProfPic
    {
         get => profImg;
        //get {
        //    if (System.IO.File.Exists(PsUserLoginModel.instance.Value.PSUserProfilePicture))
        //    {
        //        profImg.Source = ImageSource.FromFile(PsUserLoginModel.instance.Value.PSUserProfilePicture);
        //    }
        //    else
        //    {
                 
        //          ImgProfPic.Source = ImageSource.FromFile("psloadimg.png");
        //    }

        //       return profImg;
        //}
        set
        {
             profImg = value;
            OnPropertyChanged(nameof(ImgProfPic));
          //  set => SetProperty(ref profImg, value);
        }
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
        //    ImgProfPic.Source = ImageSource.FromFile(PsUserLoginModel.instance.Value.PSUserProfilePicture);
            using var stream = File.OpenRead(PsUserLoginModel.instance.Value.PSUserProfilePicture);
            ImgProfPic.Source = ImageSource.FromStream(() => stream);
         }
            
        else
            ImgProfPic.Source = ImageSource.FromFile("psloadimg.png");
         //  ProfileImage.Source = PsUserLoginModel.instance.Value.PSUserProfilePicture;

        Title = $"Dashboard Hello {PsUserLoginModel.instance.Value.PSUserDisplayName}";
        
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
         OnPropertyChanged(nameof(ImgProfPic));
        //0 viedoNotes = new ObservableCollection<ViedoNotesModel>();
    }
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

}