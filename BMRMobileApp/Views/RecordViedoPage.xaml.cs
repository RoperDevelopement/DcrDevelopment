
using BMRMobileApp.Models;
using BMRMobileApp.SQLiteDBCommands;
using BMRMobileApp.ViewModels;

using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Media;

using Plugin.Maui.Audio;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using BMRMobileApp.Utilites;

namespace BMRMobileApp
{
    public partial class RecordViedoPage : ContentPage, INotifyPropertyChanged
    {

        private SQLiteService cmd = new SQLiteService();
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private readonly IAudioManager _audioManager;
        private IAudioPlayer _player;
        bool isRecording = false;
        string videoPath;
        private IconDataModel iconDataModel;

        private ObservableCollection<ViedoNotesModel> viedoNotes = new ObservableCollection<ViedoNotesModel>();
        //        public ICommand DeleteCommand => new Command<int>(OnDelete<int>);
        public RecordViedoPage()
        {
            InitializeComponent();
            //   _audioManager = audioManager;
            BindingContext = this;
            GetPermission().Wait();

        }
        //  public ObservableCollection<ViedoNotesModel> VideoNotes { get; set; }
        [ObservableProperty]
        public ObservableCollection<ViedoNotesModel> VideoNotes
        {
            get => viedoNotes;
            set
            {
                viedoNotes = value;
                OnPropertyChanged(nameof(VideoNotes));
            }

        }

        async Task GetPermission()
        {

            await Permissions.RequestAsync<Permissions.StorageRead>();
            await Permissions.RequestAsync<Permissions.StorageWrite>();

        }
        async void OnRecordClicked(object sender, EventArgs e)
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            string localPath = string.Empty;
            if (status != PermissionStatus.Granted)
            {
                //await Application.Current.MainPage.DisplayAlert("Alert", "Camera permission denied.", "OK");
                await Application.Current.Windows[0].Page.DisplayAlert("Alert", "Camera permission denied.", "OK");
            }
            if (MediaPicker.Default.IsCaptureSupported)
            {
                try
                {
                    FileResult video = await MediaPicker.Default.CaptureVideoAsync();

                    if (video != null)
                    {
                        video.FileName = $"{ViedoSaveName.Text}{Path.GetExtension(video.FileName)}";
                        localPath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, video.FileName);
                        //    byte[] buffer = File.ReadAllBytes(video.FullPath);

                        //#if ANDROID
                        //                 var andir =  Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMovies);
                        //                  localPath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, video.FileName);
                        //                  await DisplayAlert("Confrim", $"{localPath}","OK");

                        //#endif

                        //    await File.WriteAllBytesAsync(localPath, buffer);







                        //  videoPath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, $"profilepictures.png");
                        using Stream sourceStream = await video.OpenReadAsync();
                        using FileStream localFileStream = File.OpenWrite(localPath);
                        await sourceStream.CopyToAsync(localFileStream);
                        sourceStream.Flush();
                        sourceStream.Close();
                        localFileStream.Flush();
                        localFileStream.Close();

                        var id = await AddViedoRecord(localPath);
                        var newViedo = new ViedoNotesModel { VideoPath = localPath, Emotion = "❌🐙", ViedoTaken = $"{DateTime.Now.ToString()}", ViedoID = id, ViedoName = Path.GetFileNameWithoutExtension(localPath) };
                        //   var newViedo = new ViedoNotesModel { VideoPath = localPath, Emotion = "\U000F0B8C", Transcription = "Feeling calm today.", ViedoTaken = $"Date Viedo Take {DateTime.Now.ToString()}" };
                        VideoNotes.Add(newViedo);

                        ViedoSaveName.Text = string.Empty;

                        // await PlayAudioAsync(localPath);
                        // videoPlayer.Source = MediaSource.FromFile(localPath);
                        // You can now play or process the video
                    }
                    else
                    {
                        await Application.Current.Windows[0].Page.DisplayAlert("Cancled:", $"Captureing Viedo Cancled.", "OK");   // Handle permissions or capture errors
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.Windows[0].Page.DisplayAlert("Error", $"Captureing Viedo {ex.Message}.", "OK");   // Handle permissions or capture errors
                }
            }
            else
            {
                await Application.Current.Windows[0].Page.DisplayAlert("Not Supported:", "Viedo Capture is not supported", "OK");   // Handle permissions or capture errors
            }
        }


        private async Task PlayAudioAsync(string filePath)
        {
            try
            {
                if (_player != null && _player.IsPlaying)
                {
                    _player.Stop();
                }

                var stream = File.OpenRead(filePath);
                _player = _audioManager.CreatePlayer(stream);
                _player.Play();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Playing  Viedo {ex.Message}.", "OK");   // Handle permissions or capture errors
            }
        }
        protected void OnPropertyChanged(string name) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        // _player?.Pause(); // Temporarily halt playback
        //  _player?.Stop();  // Fully stop and reset

        private async void OnDeleteClicked(object sender, EventArgs e)
        {

            bool answerv = await DisplayAlert("Confrim", "Delete Viedo", "Yes", "No");
            if (answerv)
            {
                // GetConfirm().Wait();
                if (sender is Button button && button.CommandParameter is int parameter)
                {
                    await DeleteViedo(parameter);
                }
            }


            // Use itemId as needed
        }

        private async Task DeleteViedo(int id)
        {

            try
            {

                cmd.DelViedo(id).GetAwaiter().GetResult();
                var itemToRemove = VideoNotes.FirstOrDefault(item => item.ViedoID == id);
                
                    
                if (itemToRemove != null)
                {
                    if (File.Exists(itemToRemove.VideoPath))
                        File.Delete(itemToRemove.VideoPath);   
                    VideoNotes.Remove(itemToRemove); OnPropertyChanged(nameof(VideoNotes));

                }
              ;
                // GetPSUserViedo().GetAwaiter().GetResult();
                //   if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0] is Window window)
                //  {
                //await Shell.Current.GoToAsync(nameof(RecordViedoPage));
                //await Shell.Current.GoToAsync("///recordviedopage") ;

                //  }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }



        }
        private Task<int> AddViedoRecord(string vPath)
        {
            return Task.Run(() =>
            {
                var newViedoRec = new PSViedo { VideoPath = vPath, DateViedoTaken = DateTime.Now.ToString() };


                return cmd.AddViedo(newViedoRec);


            });
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Fire and forget the async call, handle exceptions as needed
            _ = GetPSUserViedo();
            //0 viedoNotes = new ObservableCollection<ViedoNotesModel>();
        }
        protected override void OnDisappearing() { base.OnDisappearing(); }
        private async Task GetPSUserViedo()
        {
            IList<PSViedo> psVided = await cmd.GetPsUsersViedo() as IList<PSViedo>;
            if (psVided != null && psVided.Count > 0)
            {
                if (VideoNotes != null && VideoNotes.Count > 0)
                {
                    VideoNotes.Clear();
                }
                foreach (PSViedo psViedo in psVided)
                {
                    if ((File.Exists(psViedo.VideoPath)))
                    {
                        var newViedo = new ViedoNotesModel { VideoPath = psViedo.VideoPath, Emotion = "❌🐙 ", ViedoID = psViedo.ID, ViedoTaken = psViedo.DateViedoTaken, ViedoName = Path.GetFileNameWithoutExtension(psViedo.VideoPath) };

                        VideoNotes.Add(newViedo);
                    }

                }
            }
        }
        //private void OnPlayClicked(object sender, EventArgs e)
        //{


        //   PlayViedo.Source = VideoNotes[0].VideoPath;
        //    PlayViedo.Play(); 
        //}

        //private void OnPauseClicked(object sender, EventArgs e)
        //{
        //   // playViedo.Pause();
        //}

        //private void OnStopClicked(object sender, EventArgs e)
        //{
        //   // playViedo.Stop();
        //}
        public void Remove()
        {

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, VideoNotes));

        }
    }
}