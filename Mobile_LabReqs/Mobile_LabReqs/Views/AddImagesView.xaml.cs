using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Mobile_LabReqs.ViewsModels;
using Android.Content;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace Mobile_LabReqs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddImagesView : ContentPage
    {
        private const int MaxColumns = 3;

        private double _rowHeight = 0;
        private int _currentRow = 0;
        private int _currentColumn = -1;
        private IList<string> ImageFile
        { get; set; }
        public AddImagesView()
        {
            InitializeComponent();
            Title = "Capture Images To Archive";
            ImageFile = new List<string>();
             Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
          
        Device.BeginInvokeOnMainThread(async () => await InitialiseMediaPermissions());

         //  System.IO.File folder = new System.IO.File(Environment.ggetExternalStorageDirectory(), this.folder_main);
            //var addPhotoButton = new Button()
            //{
            //    Text = "Add Photo",
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    BorderColor = Color.FromHex("#F0F0F0"),
            //    BorderWidth = 1,
            //    BackgroundColor = Color.FromHex("#F9F9F9"),
            //    TextColor = Color.Black,
            //    FontAttributes = FontAttributes.Bold
            //};
            ToolbarItem addPhotoButton = new ToolbarItem
            {
                Text = "Capture Image",
                 IconImageSource = ImageSource.FromResource("camera.png"),
                Order = ToolbarItemOrder.Secondary,
                Priority = 0,
                
            };
            ToolbarItem archiverButton = new ToolbarItem
            {
                Text = "Archive Image",
                // IconImageSource = ImageSource.FromFile("example_icon.png"),
                Order = ToolbarItemOrder.Secondary,
                Priority = 1
            };
            ToolbarItem exitButton = new ToolbarItem
            {
                Text = "Exit",
                // IconImageSource = ImageSource.FromFile("example_icon.png"),
                Order = ToolbarItemOrder.Secondary,
                Priority = 2
            };
          

            // "this" refers to a Page object
            this.ToolbarItems.Add(addPhotoButton);
            this.ToolbarItems.Add(archiverButton);
            this.ToolbarItems.Add(exitButton);
            //    ToolbarItem addPhotoButton = new ToolbarItem();
            addPhotoButton.Clicked += async (object sender, EventArgs e) => await AddPhoto();
            archiverButton.Clicked += async (object sender, EventArgs e) => await AddArchiver();
            exitButton.Clicked += async (object sender, EventArgs e) => await ExitAddImages();

            //   ImageGridContainer.Children.Add(addPhotoButton, 0, 0);

            Device.BeginInvokeOnMainThread(async () =>
            {
                // Wait for a small amount of time so the UI has a chance to update the relevant values
                // we need to complete the operation.
                await Task.Delay(10);

                // Set the row height to be the same as the column width so that the image 
                // is presented in a square grid.
                //    _rowHeight = addPhotoButton.Width;
                _rowHeight = 200;
                   ImageGridContainer.RowDefinitions[0].Height = _rowHeight;
              //  ImageGridContainer.RowDefinitions[0].Height = 1;

                await ImageGridContainer.FadeTo(1);
            });
        }
        async Task ExitAddImages()
        {
            App.Current.MainPage = new MainPage();
        }
    async Task AddArchiver()
        {
 
        UploadImages upload = new UploadImages();
            // $"{new LabReqsListViewPage()}
            // await Shell.Current.GoToAsync($"{new HomePage()}");
            // Shell.Current.SendBackButtonPressed();
            // await  Shell.Current.GoToAsync(nameof(MainPage));
           //   App.Current.MainPage =  new NavigationPage(new MainPage());
             //await Navigation.PushAsync(new HomePage());
           // await Navigation.PushModalAsync(new NavigationPage(new MainPage()));
            //this.Navigation.PopAsync();
            //  await.Navigation.PopAsync();
            //   await Navigation.PushModalAsync(new MainPage());
            // await this.Navigation.PopToRootAsync();
            foreach (string image in ImageFile)
            {
                upload.ArchiverName = "LabReqs";
                upload.ArchiverFileName = System.IO.Path.GetFileName(image);
                upload.ArchiverImage = System.IO.File.ReadAllBytes(image);
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(2);

                       client.BaseAddress = new Uri($"http://10.0.2.2:25100/weatherforecast");
                 //   client.BaseAddress = new Uri($"http://192.168.1.13:25100/weatherforecast");
                    //  client.BaseAddress = new Uri($"http://192.168.1.13:25100/weatherforecast");
                    //   client.BaseAddress = new Uri($"http://localhost:25100/weatherforecast");


                    var responseTask = client.GetAsync($"{client.BaseAddress.AbsoluteUri}");
                    //  var responseTask = client.GetAsync($"{client.BaseAddress.AbsoluteUri}");
                    responseTask.Wait();
                    
                    var results = responseTask.Result;
                    if (results.IsSuccessStatusCode)
                    {
                        var readTask = results.Content.ReadAsStringAsync();
                        readTask.Wait();
                        var r = readTask.ToString();

                    }

                    
                }
            }
        }
        async Task AddPhoto()
        {
            MediaFile file = null;

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "You need to fix the problem of camera availability", "OK");
                return;
            }

            var imageSource = await DisplayActionSheet("Image Source", "Cancel", null, new string[] { "Camera", "Photo Gallery" });
            var photoName = Guid.NewGuid().ToString() + ".png";
            StoreCameraMediaOptions storeCameraMediaOptions = new StoreCameraMediaOptions();
           
            switch (imageSource)
            {
                case "Camera":
                    file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "Images",
                        Name = photoName
                    });

                    break;

                case "Photo Gallery":
                    file = await CrossMedia.Current.PickPhotoAsync();

                    break;

                default:
                    break;
            }

            if (file == null)
                return;

            // We have the photo, now add it to the grid.
            _currentColumn++;

            if (_currentColumn > MaxColumns - 1)
            {
                _currentColumn = 0;
                _currentRow++;

                // Add a new row definition by copying the first row.
                ImageGridContainer.RowDefinitions.Add(ImageGridContainer.RowDefinitions[0]);
            }

            var newImage = new Xamarin.Forms.Image()
            {
                Source = ImageSource.FromFile(file.Path),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Aspect = Aspect.AspectFill,
                Scale = 0
            };

            //var button = new Button
            //{
            //    Text = "Exit",
            //    VerticalOptions =LayoutOptions.FillAndExpand,
            //    HorizontalOptions=LayoutOptions.FillAndExpand,
            //    Scale=0,
            //    ImageSource.FromFile(file.Path)



            //    //   IconImageSource = ImageSource.FromFile("example_icon.png")

            //};
            //var btn = new Button();
            //btn.ImageSource = ImageSource.FromStream(file.Path);
            //btn.Text = "Exit";
            //btn.VerticalOptions = LayoutOptions.FillAndExpand;
            //btn.HorizontalOptions = LayoutOptions.FillAndExpand;
            //btn.Scale = 0;

            ImageFile.Add(file.Path);
              ImageGridContainer.Children.Add(newImage, _currentColumn, _currentRow);
          //  ImageGridContainer.Children.Add(btn, _currentColumn, _currentRow);
            //         ImageGridContainer.Children.Add(button, _currentColumn, _currentRow+1);

            await Task.Delay(250);
         //  await btn.ScaleTo.ScaleTo(1, 250, Easing.SpringOut);
            await newImage.ScaleTo(1, 250, Easing.SpringOut);
        }

        async Task InitialiseMediaPermissions()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            
            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }
        }
        
    }
    }
 