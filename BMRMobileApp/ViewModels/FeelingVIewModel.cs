 
using BMRMobileApp.Models;
using BMRMobileApp.SQLiteDBCommands;
using BMRMobileApp.Utilites;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Media;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace BMRMobileApp.ViewModels
{
    public class FeelingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ISpeechToText speechToText = SpeechToText.Default;
        private readonly SpeechToTextUtility speechUtility = new();
        private bool isSpeaking = false;
        private bool isNotSpeaking = false;
   
        public ICommand SelectFeelingCommand { get; }
        public ICommand SaveFeelingCommand { get; }
        public ICommand ExitFeelingCommand { get; }
        public ICommand StartVoiceRecordingCommand { get; }
        public ICommand StopVoiceRecordingCommand { get; }

        private string selectedFeeling;
        private string journalEntry;
        private Color backgroundGradient;
        private ImageSource imgSourceSR;
        public FeelingViewModel()
        {
            SelectFeelingCommand = new Command<string>(OnFeelingSelected);
            SaveFeelingCommand = new Command(OnSaveFeeling);
            ExitFeelingCommand = new Command(OnExitFeeling);
            StartVoiceRecordingCommand = new Command(OnStartRecordingClicked);
            StopVoiceRecordingCommand = new Command(OnSStopRecordingClicked);
            BackgroundGradient = Colors.Gray;
            speechUtility.PartialResultReceived += OnPartialResult;
            speechUtility.FinalResultReceived += OnFinalResult;
            IsNotSpeaking = false;
            isSpeaking = true;
           // SetImage();
        }
        private void SetImage()
        {
            //ConvertHexToImage convertHexToImage = new ConvertHexToImage();
            //convertHexToImage.ImageColor = Colors.Black;
           // imgSourceSR = convertHexToImage.HexValueToImge(Utilites.FluentUI.record_48_regular, string.Empty, 10);
            var img = new FontImageSource
            {
                Glyph = "\uf737", // Example: FontAwesome icon glyph
                FontFamily =FluentUI.text_font_size_24_regular, // Or your custom font
                Size = 240,
                Color = Colors.White// Your hex color here
            };
            ImgSourceSR = img;
            OnPropertyChanged(nameof(ImgSourceSR));
        }
       public  ImageSource ImgSourceSR
        {

            get => imgSourceSR;

            set
            {
                imgSourceSR = value;
                OnPropertyChanged(nameof(ImgSourceSR));
            }
        }
        public bool IsSpeaking
        {
            get => isSpeaking;
            set
            {
                isSpeaking = value;
                OnPropertyChanged(nameof(IsSpeaking));
            }
        }
        public bool IsNotSpeaking
        {
            get => isNotSpeaking;
            set
            {
                isNotSpeaking = value;
                OnPropertyChanged(nameof(IsNotSpeaking));
            }
        }
        public string JournalEntry
        {
            get => journalEntry;
            set
            {
                journalEntry = value;
                OnPropertyChanged(nameof(JournalEntry));
            }
        }

        public Color BackgroundGradient
        {
            get => backgroundGradient;
            set
            {
                backgroundGradient = value;
                OnPropertyChanged(nameof(BackgroundGradient));
            }
        }

        public Command Command { get; private set; }

      

        private void OnFeelingSelected(string feeling)
        {
            selectedFeeling = feeling;
           
            BackgroundGradient = feeling switch
            {
                "Happy" => Color.FromArgb("#FFD700"),
                "Sad" => Color.FromArgb("#1E90FF"),
                "Angry" => Color.FromArgb("#FF4500"),
                "Calm" => Color.FromArgb("#98FB98"),
                "Neutral" => Color.FromArgb("#D3D3D3"),
                _ => Colors.Gray
            };
        }

        private async void OnSaveFeeling()
        {
            // Save logic here (e.g., SQLite, Azure, etc.)
            await Application.Current.MainPage.DisplayAlert("Saved", $"Feeling '{selectedFeeling}' notes {journalEntry} saved!", "OK");
            JournalEntry = string.Empty; // Clear the journal entry after saving
        }
        private async Task AddFeelings()
        {
            SQLiteService sQLiteService = new SQLiteService();
            var feelinfs = new PSFeelingNotes   
            {
                Feeling = await Utilites.PSUtilites.ConvertStringToBye(selectedFeeling),
                PSUserID = PsUserLoginModel.instance.Value.ID,
                FeelingNotes = JournalEntry,
                DateAdded = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")


            };

            await sQLiteService.AddPSFeelings(feelinfs);
        }
        private async void OnExitFeeling()
        {
            // Save logic here (e.g., SQLite, Azure, etc.)
            //   await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new MainPage()));

            //     Application.Current.MainPage = new AppShell();

            // Navigate to the main page
            //   await Shell.Current.GoToAsync("//mainpage");

            //  await INavigation.PushModalAsync(new FeelingPage());
            //NavigationService.Navigate(new Uri("Page1.xaml", UriKind.Relative));
            //plication.Current.MainPage = new AppShell();
            //  await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new AppShell()));
            //  await Application.Current.MainPage.Navigation.PushAsync(new NavigationPage(new MainPage()));
        //    Application.Current.MainPage = new AppShell();
            if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0] is Window window)
            {
             window.Page = new AppShell();
            }

            //  await Application.Current.MainPage.Navigation.PushAsync(new AppShell());
            //new Window(new AppShell());
            // await Application.Current.MainPage.Navigation.PushModalAsync(new MainPage());
            // await Shell.Current.GoToAsync("//mainpage");
            // await Shell.Current.GoToAsync("///mainpage");
            // Command = new Command(async () =>
            // {
            // Simulate login logic
            //     await Application.Current.MainPage.Navigation.PushModalAsync(new MainPage());
            // });

        }
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


        private async void OnStartRecordingClicked()
        {
            //ButtonStartListing.IsVisible = false;
            //ButtonStopListing.IsVisible = true;
            //  await ListenAndFillTextBox(null);
            IsSpeaking = false;
            IsNotSpeaking = true;
            await StartVoiceJournalAsync();
            //  await SpeakText(JournalEditor.Text);
        }
        private async void OnSStopRecordingClicked()
        {

            //ButtonStartListing.IsVisible = true;
            //ButtonStopListing.IsVisible = false;
            //  await ListenAndFillTextBox(null);
            IsSpeaking = true;
            IsNotSpeaking = false;
            await StopVoiceJournalAsync();
            //  await SpeakText(JournalEditor.Text);
        }
        public async Task StartVoiceJournalAsync()
        {
            var ready = await speechUtility.InitializeAsync(CancellationToken.None);
            if (!ready) return;

            //  speechUtility.PartialResultReceived += (s, text) => UpdateLiveTranscript(text);
            //  speechUtility.FinalResultReceived += (s, text) => SaveFinalTranscript(text);

            await speechUtility.StartListeningAsync();
        }
        private void OnPartialResult(object? sender, string text)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    JournalEntry = text;
                });
                // Live updates while speaking

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnFinalResult(object? sender, string text)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    JournalEntry = text;

                });   // Final transcript after stop

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Optional: Save to journal, tag emotion, trigger animation
            //  SaveTranscript(text);
        }
        public async Task StopVoiceJournalAsync()
        {
            await speechUtility.StopListeningAsync();
        }

    }
}
