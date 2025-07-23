using Plugin.Media.Abstractions;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
namespace Mobile_LabReqs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaPage : ContentPage
    {
        public MediaPage()
        {
            string PhotoPath = @"l:\";
              
            InitializeComponent();
            takePhoto.Clicked += async (sender, args) =>
             {
                 await TakePhotoAsync();
               
             };

            async Task TakePhotoAsync()
            {
                try
                {
                    var photo = await MediaPicker.CapturePhotoAsync();
                    await LoadPhotoAsync(photo);
                    Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    // Feature is not supported on the device
                }
                catch (PermissionException pEx)
                {
                    // Permissions not granted
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
                }
            }

            async Task LoadPhotoAsync(FileResult photo)
            {
                // canceled
                if (photo == null)
                {
                    PhotoPath = null;
                    return;
                }
                // save the file into local storage
                var newFile =System.IO.Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = System.IO.File.OpenWrite(newFile))
                    await stream.CopyToAsync(newStream);

                PhotoPath = newFile;
            }

            //    takePhoto.Clicked += async (sender, args) =>
            //    {

            //        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            //        {
            //            DisplayAlert("No Camera", ":( No camera available.", "OK");
            //            return;
            //        }

            //        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            //        {
            //            Directory = @"L:\EdocsGItHub\Mobile_LabReqs\Mobile_LabReqs\bin\Debug\Picture",
            //            SaveToAlbum = true,
            //            CompressionQuality = 75,
            //            CustomPhotoSize = 50,
            //            PhotoSize = PhotoSize.MaxWidthHeight,
            //            MaxWidthHeight = 2000,
            //            DefaultCamera = CameraDevice.Front
            //        });

            //        if (file == null)
            //            return;

            //        DisplayAlert("File Location", file.Path, "OK");

            //        image.Source = ImageSource.FromStream(() =>
            //        {
            //            var stream = file.GetStream();
            //            file.Dispose();
            //            return stream;
            //        });
            //    };

            //    pickPhoto.Clicked += async (sender, args) =>
            //    {
            //        if (!CrossMedia.Current.IsPickPhotoSupported)
            //        {
            //            DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
            //            return;
            //        }
            //        var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            //        {
            //            PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

            //        });


            //        if (file == null)
            //            return;

            //        image.Source = ImageSource.FromStream(() =>
            //        {
            //            var stream = file.GetStream();
            //            file.Dispose();
            //            return stream;
            //        });
            //    };

            //    takeVideo.Clicked += async (sender, args) =>
            //    {
            //        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
            //        {
            //            DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
            //            return;
            //        }

            //        var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
            //        {
            //            Name = "video.mp4",
            //            Directory = "DefaultVideos",
            //        });

            //        if (file == null)
            //            return;

            //        DisplayAlert("Video Recorded", "Location: " + file.Path, "OK");

            //        file.Dispose();
            //    };

            //    pickVideo.Clicked += async (sender, args) =>
            //    {
            //        if (!CrossMedia.Current.IsPickVideoSupported)
            //        {
            //            DisplayAlert("Videos Not Supported", ":( Permission not granted to videos.", "OK");
            //            return;
            //        }
            //        var file = await CrossMedia.Current.PickVideoAsync();

            //        if (file == null)
            //            return;

            //        DisplayAlert("Video Selected", "Location: " + file.Path, "OK");
            //        file.Dispose();
            //    };
            //}
        }
    }
}
