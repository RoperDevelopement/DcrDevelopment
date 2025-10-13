
using BMRMobileApp.Models;
using BMRMobileApp.SQLiteDBCommands;
using BMRMobileApp.Utilites;
using BMRMobileApp.ViewModels;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Media;
using MimeKit.Text;
using Plugin.Maui.Audio;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace BMRMobileApp
{
    public partial class RecordViedoPage : ContentPage, INotifyPropertyChanged
    {
        ISpeechToText speechToText = SpeechToText.Default;
        private readonly SQLiteService sQLiteService = new SQLiteService();
        public readonly TextSpeakerService speechToTextService = new TextSpeakerService();
        private readonly SpeechToTextUtility speechUtility = new();

        private SQLiteService cmd = new SQLiteService();
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private readonly IAudioManager _audioManager;
        private IAudioPlayer _player;
        bool isRecording = false;
        string videoPath;
        private IconDataModel iconDataModel;
        private bool isNotSpeaking;
        private bool isSpeaking;
        private bool isNotSpeakingVFN;
        private bool isSpeakingVFN;

        // public Command StartVoiceRecordingCommand { get; }
        // public ICommand StopVoiceRecordingCommand { get; }
        private ObservableCollection<ViedoNotesModel> viedoNotes = new ObservableCollection<ViedoNotesModel>();
        //        public ICommand DeleteCommand => new Command<int>(OnDelete<int>);
        public RecordViedoPage()
        {
            InitializeComponent();
            //   _audioManager = audioManager;
            BindingContext = this;
            speechUtility.PartialResultReceived += OnPartialResult;
            speechUtility.FinalResultReceived += OnFinalResult;
            GetPermission().Wait();
            Title = "Make Viedos";
            BackgroundColor = Color.FromArgb(Utilites.Consts.DefaultBackgroundColor);
         
           // StopVoiceRecordingCommand = new Command(OnClickStopRecording);
            IsNotSpeaking = false;
            IsSpeaking = true;
            IsSpeakingVFN = false;
            IsNotSpeakingVFN = true;
            OnPropertyChanged(nameof(IsNotSpeaking));
            OnPropertyChanged(nameof(IsSpeaking));
            GetMood();
            
        }
        private bool FocuseTxtVTitle { get; set; } = true;
        private string CurrentTextFocused {  get; set; }
        private async void GetMood()
        {
            var mood = cmd.GetUserCurrentMoodNonAsync();
            if (mood != null)
            {
                Title = $"{Title} {mood.Mood} {mood.MoodTag}";
                BackgroundColor = Color.FromArgb(mood.BackgroundColor);
            }
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

        [ObservableProperty]
        public bool IsNotSpeaking
        {
            get => isNotSpeaking;
            set
            {
                isNotSpeaking = value;
                OnPropertyChanged(nameof(IsNotSpeaking));
            }

        }
        public bool IsNotSpeakingVFN
        {
            get => isNotSpeakingVFN;
            set
            {
                isNotSpeakingVFN = value;
                OnPropertyChanged(nameof(IsNotSpeakingVFN));
            }

        }
        public bool IsSpeakingVFN
        {
            get => isSpeakingVFN;
            set
            {
                isSpeakingVFN = value;
                OnPropertyChanged(nameof(IsSpeakingVFN));
            }

        }

      
        
        [ObservableProperty]
        public bool IsSpeaking
        {
            get => isSpeaking;
            set
            {
                isSpeaking = value;
                OnPropertyChanged(nameof(IsSpeaking));
            }

        }

        async Task GetPermission()
        {

            await Permissions.RequestAsync<Permissions.StorageRead>();
            await Permissions.RequestAsync<Permissions.StorageWrite>();

        }
        async void OnRecordClicked(object sender, EventArgs e)
        {
            StopRecording();
            Task.Delay(3000);
            if (string.IsNullOrWhiteSpace(ViedoTitle.Text))
            {
                await Microsoft.Maui.Controls.Application.Current.Windows[0].Page.DisplayAlert("Alert", "Please enter a title for the video before recording.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(ViedoSaveName.Text))
            {
                bool answerv = await DisplayAlert("Viedo Save Name", "Use Viedo Title as save name", "Yes", "No");
                if (answerv)
                {
                    ViedoSaveName.Text = ViedoTitle.Text.Replace(" ","").Trim();
                    if(ViedoSaveName.Text.Length >15)
                    {
                        ViedoSaveName.Text = ViedoSaveName.Text.Substring(0, 15);
                    }
                }
                else
                {
                    // await Application.Current.Windows[0].Page.DisplayAlert("Alert", "Please enter a name for the video before recording.", "OK");
                    return;
                }
            }
            
                
            
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            string localPath = string.Empty;
            if (status != PermissionStatus.Granted)
            {
                //await Application.Current.MainPage.DisplayAlert("Alert", "Camera permission denied.", "OK");
                await Microsoft.Maui.Controls.Application.Current.Windows[0].Page.DisplayAlert("Alert", "Camera permission denied.", "OK");
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

                        var id = await AddViedoRecord(localPath,ViedoTitle.Text);
                        var newViedo = new ViedoNotesModel {ViedoTitle=ViedoTitle.Text, VideoPath = localPath, Emotion = "❌🐙", ViedoTaken = $"{DateTime.Now.ToString()}", ViedoID = id, ViedoName = Path.GetFileNameWithoutExtension(localPath) };
                        //   var newViedo = new ViedoNotesModel { VideoPath = localPath, Emotion = "\U000F0B8C", Transcription = "Feeling calm today.", ViedoTaken = $"Date Viedo Take {DateTime.Now.ToString()}" };
                        VideoNotes.Add(newViedo);

                        ViedoSaveName.Text = string.Empty;
                        ViedoTitle.Text = string.Empty;
                        // await PlayAudioAsync(localPath);
                        // videoPlayer.Source = MediaSource.FromFile(localPath);
                        // You can now play or process the video
                    }
                    else
                    {
                        await Microsoft.Maui.Controls.Application.Current.Windows[0].Page.DisplayAlert("Cancled:", $"Captureing Viedo Cancled.", "OK");   // Handle permissions or capture errors
                    }
                }
                catch (Exception ex)
                {
                    await Microsoft.Maui.Controls.Application.Current.Windows[0].Page.DisplayAlert("Error", $"Captureing Viedo {ex.Message}.", "OK");   // Handle permissions or capture errors
                }
            }
            else
            {
                await Microsoft.Maui.Controls.Application.Current.Windows[0].Page.DisplayAlert("Not Supported:", "Viedo Capture is not supported", "OK");   // Handle permissions or capture errors
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
                await DisplayAlert("Error", $"Playing  Viedo {ex.Message}.", "OK");   // Handle permissions or capture errors
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
        private Task<int> AddViedoRecord(string vPath,string viedoTitle)
        {
            return Task.Run(() =>
            {
                var newViedoRec = new PSViedo {ViedoTitle= viedoTitle, VideoPath = vPath, DateViedoTaken = DateTime.Now.ToString() };


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
        protected override void OnDisappearing() { base.OnDisappearing();
            _ = speechToText.StopListenAsync();
        }
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
                    
                        var newViedo = new ViedoNotesModel { ViedoTitle = psViedo.ViedoTitle, VideoPath = psViedo.VideoPath, Emotion = "❌🐙 ", ViedoID = psViedo.ID, ViedoTaken = psViedo.DateViedoTaken, ViedoName = Path.GetFileNameWithoutExtension(psViedo.VideoPath) };

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
        public async void OnClickStartRecordingViedoTitle(object sender, EventArgs e)
        {
         //   CurrentTextFocused = txtFocused;
            IsNotSpeaking =true;
            IsSpeaking = false;
            FocuseTxtVTitle = true;
            IsSpeakingVFN = false;
            IsNotSpeakingVFN = false;
            await StartVoiceJournalAsync();

        }
     //   public async void StartVoiceRecordingCommand(string txtVoice)
     //   {
     //       Console.WriteLine(txtVoice);
     //   }
        //public async void OnClickStopRecording()
        //{
        //    FocuseTxtVTitle = true;
        //    IsNotSpeaking = true;
        //    IsSpeaking = false;
        //    await StartVoiceJournalAsync();
        //}

        private async void OnClickStartRecordingViedoFN(object sender, EventArgs e)
        {
            FocuseTxtVTitle =false;
            IsNotSpeaking = false;
            IsSpeaking = false;
            IsSpeakingVFN = true;
            IsNotSpeakingVFN = false;
            await StartVoiceJournalAsync();

        }

        private async void StopVoiceRecording(object sender, EventArgs e)
        {
            StopRecording();
        }
        private async void StopRecording()
        {
            if (FocuseTxtVTitle)
            {
                FocuseTxtVTitle = false;
                IsNotSpeaking = false;
                IsSpeaking = true;
                IsSpeakingVFN = false;
                IsNotSpeakingVFN = true;

            }
            else
            {
                IsNotSpeaking = false;
                IsSpeaking = true;
                IsSpeakingVFN = false;
                IsNotSpeakingVFN = true;
            }
            await StopVoiceJournalAsync();
        }
        public async Task StopVoiceJournalAsync()
        {
            await speechUtility.StopListeningAsync();
            
        }
        private async void StopRec()
        {
            await StopVoiceJournalAsync();
        }
        //protected override void OnParentSet()
        //{
        //    base.OnParentSet();
        //    BindingContext = Parent?.BindingContext;
        //}
        private void OnFinalResult(object? sender, string text)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (!(string.IsNullOrEmpty(text)))
                    {
                        if (FocuseTxtVTitle)
                        {
                            ViedoTitle.Text = text;
                            FocuseTxtVTitle = false;
                        }
                            
                        else
                            ViedoSaveName.Text = text;
                    }

                });   // Final transcript after stop

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Optional: Save to journal, tag emotion, trigger animation
            //  SaveTranscript(text);
        }
        private void OnPartialResult(object? sender, string text)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (string.IsNullOrEmpty(text)) return;
                    if (FocuseTxtVTitle)
                        ViedoTitle.Text = text;
                    else
                        ViedoSaveName.Text = text;
                });
                // Live updates while speaking

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task StartVoiceJournalAsync()
        {
            var ready = await speechUtility.InitializeAsync(CancellationToken.None);
            if (!ready) return;


            //  speechUtility.PartialResultReceived += (s, text) => UpdateLiveTranscript(text);
            //  speechUtility.FinalResultReceived += (s, text) => SaveFinalTranscript(text);

            await speechUtility.StartListeningAsync();
        }
       
    }
}