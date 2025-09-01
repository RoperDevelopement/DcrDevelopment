
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.Utilites
{
    public class UploadPictureViedosFiles
    {
        public async Task<string> TakePhotoAsync()
        {
            try
            {
                var status = await Permissions.RequestAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    //await Application.Current.MainPage.DisplayAlert("Alert", "Camera permission denied.", "OK");
                    await Application.Current.Windows[0].Page.DisplayAlert("Alert", "Camera permission denied.", "OK");
                    return null;
                }
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult photo = await MediaPicker.Default.CapturePhotoAsync();


                    if (photo != null)
                    {
                        // Save to local cache
                        string localPath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                        using Stream sourceStream = await photo.OpenReadAsync();
                        using FileStream localFileStream = File.OpenWrite(localPath);
                        await sourceStream.CopyToAsync(localFileStream);
                       
                        return localPath;
                        // Display the image (e.g., in an Image control)
                        //  MyImage.Source = ImageSource.FromFile(localPath);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "Camera permission denied check settings.", "OK");
                        AppInfo.ShowSettingsUI();

                    }
                }
            }

            catch (Exception ex)
            {
                // Handle exceptions (e.g., user canceled the operation)
                await Application.Current.Windows[0].Page.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");

            }
            return null;
        }

        public async Task<FileResult> PickFileAsync()
        {
            bool done = false;
            try
            {
                while (!done)
                {
                    var result = await FilePicker.Default.PickAsync(new PickOptions
                    {
                        PickerTitle = "Select a file to upload"
                    });
                    if (result != null)
                        if(await IsImageFile(result.FullPath))
                            return result;
                    else
                        {
                            bool answer = await Application.Current.Windows[0].Page.DisplayAlert("Only can Upload Image files", "Would you like to continue?", "Yes", "No");
                            if (!answer)
                               done = true;
                        }
                

                    else
                            throw new Exception("No file selected");
                }
            }
            catch (Exception ex)
            {
                // Handle errors or cancellation
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred uploading file: {ex.Message}", "OK");
                return null;
            }
            return null;
        }
        public async Task<bool> IsImageFile(string filePath)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp",".svg" };
            string? extension = Path.GetExtension(filePath)?.ToLower();
            return Array.Exists(imageExtensions, ext => ext == extension);
        }

        
        //public async Task<bool> IsImage(string filePath)
        //{
        //    string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp" };
            
        //    return Array.Exists(validExtensions, ext => ext == fileExtension);
        //}
    }

}
