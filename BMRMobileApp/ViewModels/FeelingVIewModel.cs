
using BMRMobileApp.InterFaces;
using BMRMobileApp.Models;
using BMRMobileApp.SQLiteDBCommands;
using BMRMobileApp.Utilites;
using BMRMobileApp.Utilites;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/views/popup/popup-result
namespace BMRMobileApp.ViewModels
{
    public class FeelingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ISpeechToText speechToText = SpeechToText.Default;
        private readonly SQLiteService sQLiteService = new SQLiteService();
        public readonly TextSpeakerService speechToTextService = new TextSpeakerService();
        private readonly SpeechToTextUtility speechUtility = new();
        private bool isSpeaking = false;
        private bool isNotSpeaking = false;
        private bool journalEntryGoalsVisable = false;
        private bool isSpeakingJournalGoals;
        private string journalLabel;
        public PickerModel selectedSite = new PickerModel();
        DetectMood detectMood = new DetectMood();

       // public ICommand PickerChangedCommand { get; }
        public ICommand SelectFeelingCommand { get; }
        public ICommand SaveFeelingCommand { get; }
        public ICommand ExitFeelingCommand { get; }
        public ICommand StartVoiceRecordingCommand { get; }
        public ICommand StopVoiceRecordingCommand { get; }
        public ICommand SaveJournalGoalsCommand { get; }
        public ICommand StartJournalGoalsVoiceRecordingCommand { get; }
        public ICommand StopRecJournalGoalsCommand { get; }
        public ICommand InsertEmjoiJournalEntry { get; }
      
        public ICommand PlayTextCommand { get; }
        private int selectionLength;
        private int cursorIndex;
        private int selectionLengthJG;
        private int cursorIndexJG;
        public ICommand ClearTextCommand { get; }
        private string selectedFeeling;
        private string journalEntry;
        private string journalEntryGoals;
        private string finalTranscript;
        private Color backgroundGradient;
        private bool isNotVoiceSpeakingJournalGoals;
        private bool isEmableJournal;
        private string moodTag;
        private ImageSource imgSourceSR;
        private IList<string> menuFeelings = new List<string>();
        public ICommand NavigateCommand { get; set; }
        public ICommand NavCommand { get; set; }
        public FeelingViewModel()
        {
            SelectFeelingCommand = new Command<string>(OnFeelingSelected);
            SaveFeelingCommand = new Command(OnSaveFeeling);
            ExitFeelingCommand = new Command(OnExitFeeling);
            StartVoiceRecordingCommand = new Command(OnStartRecordingClicked);
            StopVoiceRecordingCommand = new Command(OnSStopRecordingClicked);
            ClearTextCommand = new Command(OnClearTextClicked);
            SaveJournalGoalsCommand = new Command(OnClickedSaveJournalGoals);
            StopRecJournalGoalsCommand = new Command(OnClickedStopJournalGoalsVoiceRecordingCommand);
            InsertEmjoiJournalEntry = new Command<string>(OnClickInsertEmjoiJournalEntry);
            StartJournalGoalsVoiceRecordingCommand = new Command(OnClickedStartJournalGoalsVoiceRecording);
            PlayTextCommand = new Command<string>(OnClickPlayTextCommand);
            NavCommand = new Command<string>(OnClickNav);

            BackgroundGradient = Color.FromArgb("#7393B3");
            speechUtility.PartialResultReceived += OnPartialResult;
            speechUtility.FinalResultReceived += OnFinalResult;
            IsNotSpeaking = false;
            IsSpeaking = true;
            JournalEntryGoalsVisable = false;
            IsSpeakingJournalGoals = true;
            IsNotVoiceSpeakingJournalGoals = false;
            IsEmableJournal = true;
            JournalLabel = "Journal how you are doing and feeling at this moment";
            GetEmotionsTag();
            // PickerChangedCommand = new Command(OpenJournalSite);
            // Init();

            // SetImage();
        }


        //public  ICommand PickerChangedCommand => new Command(async () =>
        //{
        //    if ((SelectedSite == null) || (string.IsNullOrWhiteSpace(SelectedSite.Value))) { return; }
        //    await Task.Delay(300);

        //    if (Shell.Current != null)
        //        await Shell.Current.GoToAsync($"{SelectedSite.Value}");
        //});

        public PickerModel SelectedSite
        {
            get => selectedSite;
            set
            {
              //  if (selectedSite != value)
                //{
                    selectedSite = value;
                    OnPropertyChanged(nameof(SelectedSite));
                
               
                    //Debug.WriteLine($"Selected mood value: {_selectedMood?.Value}");
               // }
            }
        }
        public ObservableCollection<PickerModel> PM { get; set; } = new ObservableCollection<PickerModel>();
        private IList<PSEmotionsTag>  EmotionsTags { get; set; } = new List<PSEmotionsTag>();
            public async void Init()
        {
            PM.Clear();
            PM.Add(new PickerModel { Label = "Journal PlayBack", Value = "JournalPlayBackCardPage" });
            PM.Add(new PickerModel { Label = "Settings", Value = "SettingsPage" });
           
            
           // SelectedSite.Value = string.Empty;
        }
        private async void GetEmotionsTag()
        {
            EmotionsTags = await sQLiteService.GetTAsync<PSEmotionsTag>();
        }
        private async Task<int> GetEmotionsTagID(string tag)
        {
            var eID = EmotionsTags.Where(p=>p.Emotion == tag).FirstOrDefault();
            if(eID == null)
            {
                return 0;
            }
         
            return eID.ID;
            await Task.CompletedTask;
        }
        public int CursorIndex
        {
            get => cursorIndex;
            set
            {
                cursorIndex = value;
                OnPropertyChanged(nameof(CursorIndex));
         
            }
        }
        public int CursorIndexJG
        {
            get => cursorIndexJG;
            set
            {
                cursorIndex = value;
                OnPropertyChanged(nameof(CursorIndexJG));

            }
        }
        
        
        private void IntTask()
        {

        }
        public async void OpenJournalSite()
        {
            //Command OpenWebCommand = new Command(async () =>
            ///await Browser.OpenAsync(SelectedSite.Value, BrowserLaunchMode.SystemPreferred));
            ///
            if ((SelectedSite == null) || (string.IsNullOrWhiteSpace(SelectedSite.Value))) { return; }
            await Task.Delay(300);

            if (Shell.Current != null)
                    await Shell.Current.GoToAsync($"{SelectedSite.Value}");
            SelectedSite.Value=string.Empty;

        }


        public int SelectionLength
        {
            get => selectionLength;
            set
            {
                selectionLength = value;
                OnPropertyChanged(nameof(SelectionLength));
            }
        }

        public int SelectionLengthJG
        {
            get => selectionLengthJG;
            set
            {
                selectionLength = value;
                OnPropertyChanged(nameof(SelectionLengthJG));
            }
        }
        private void SetImage()
        {
            //ConvertHexToImage convertHexToImage = new ConvertHexToImage();
            //convertHexToImage.ImageColor = Colors.Black;
            // imgSourceSR = convertHexToImage.HexValueToImge(Utilites.FluentUI.record_48_regular, string.Empty, 10);
            var img = new FontImageSource
            {
                Glyph = "\uf737", // Example: FontAwesome icon glyph
                FontFamily = FluentUI.text_font_size_24_regular, // Or your custom font
                Size = 240,
                Color = Colors.White// Your hex color here
            };
            ImgSourceSR = img;
            OnPropertyChanged(nameof(ImgSourceSR));
        }
        public ImageSource ImgSourceSR
        {

            get => imgSourceSR;

            set
            {
                imgSourceSR = value;
                OnPropertyChanged(nameof(ImgSourceSR));
            }
        }


        public bool IsEmableJournal

        {
            get => isEmableJournal
;
            set
            {
                isEmableJournal = value;
                OnPropertyChanged(nameof(IsEmableJournal));
            }
        }
        public bool IsNotVoiceSpeakingJournalGoals

        {
            get => isNotVoiceSpeakingJournalGoals
;
            set
            {
                isNotVoiceSpeakingJournalGoals = value;
                OnPropertyChanged(nameof(IsNotVoiceSpeakingJournalGoals));
            }
        }
        public bool JournalEntryGoalsVisable

        {
            get => journalEntryGoalsVisable
;
            set
            {
                journalEntryGoalsVisable = value;
                OnPropertyChanged(nameof(JournalEntryGoalsVisable));
            }
        }
        public bool IsSpeakingJournalGoals

        {
            get => isSpeakingJournalGoals;


            set
            {
                isSpeakingJournalGoals = value;
                OnPropertyChanged(nameof(IsSpeakingJournalGoals));
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
        private bool SpeakingJournal { get; set; } = true;
        public bool IsNotSpeaking
        {
            get => isNotSpeaking;
            set
            {
                isNotSpeaking = value;
                OnPropertyChanged(nameof(IsNotSpeaking));
            }
        }

        
                    public string JournalLabel
        {
            get => journalLabel;
            set
            {
                journalLabel = value;
                OnPropertyChanged(nameof(JournalLabel));
            }
        }

        public string JournalEntryGoals
        {
            get => finalTranscript;
            set
            {
                finalTranscript = value;
                OnPropertyChanged(nameof(JournalEntryGoals));
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



        private async void OnFeelingSelected(string feeling)
        {
            selectedFeeling = feeling;
            var mood = await Utilites.EmojiTags.GetMoodTagColorLabel(feeling);

            BackgroundGradient = mood.Item1;
            

            JournalLabel = mood.Item2;
            moodTag = mood.Item3;
           await speechToTextService.SpeakAsync(JournalLabel);

        }

        private async void OnClickedStopJournalGoalsVoiceRecordingCommand()
        {
            IsSpeakingJournalGoals = true;
            IsNotVoiceSpeakingJournalGoals = false;
            SpeakingJournal = false;
            IsEmableJournal = true;
            await StopVoiceJournalAsync();

        }
        private async void OnClickedSaveJournalGoals()
        {
            await AddGoals();
        }
        private async void OnClickedStartJournalGoalsVoiceRecording()
        {
            IsSpeakingJournalGoals = false;
            IsNotVoiceSpeakingJournalGoals = true;
            IsEmableJournal = false;
           // StopRecording();
            SpeakingJournal = false;
            await StartVoiceJournalAsync();
        }
        private void ClearText()
        {

        }
        private async void OnSaveFeeling()
        {
            // Save logic here (e.g., SQLite, Azure, etc.)

            //   await Application.Current.MainPage.DisplayAlert("Saved", $"Feeling '{selectedFeeling}' notes {journalEntry} saved!", "OK");

            if (string.IsNullOrWhiteSpace(JournalEntry))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please add a journal entry to be saved.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(selectedFeeling))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select a feeling before saving.", "OK");
                return;
            }
            StopRecording();
            await AddFeelings();
            await UserMoods();
            JournalEntryGoalsVisable = true;
            OnPropertyChanged(nameof(JournalEntryGoalsVisable));
            // JournalEntry = string.Empty; // Clear the journal entry after saving

        }
        private async Task UserMoods()
        {
            var mood = await detectMood.GetMoodAsync(JournalEntry, selectedFeeling);
            if (mood != null)
            {
                PSUserMood pSUserMood = new PSUserMood { Mood = $"{selectedFeeling}", SentimentScore = mood.SentimentScore, BackgroundColor = mood.BackGroudColor.ToString(),MoodTag=moodTag, TimeMood = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };

                if (mood.SentimentScore == 0)
                {

                    var sen = mood.MoodName switch
                    {
                        "Happy" => 80,
                        "Calm" => 60,
                        "Confused" => 40,
                        "Sad" => 20,
                        "Angry" => 0,
                        _ => 50,
                    };
                    pSUserMood.SentimentScore = sen;
                }
                await sQLiteService.UpDateInsertPSUserMood(pSUserMood);
            }


        }
        private async Task AddFeelings()
        {
            var feelindID = await GetEmotionsTagID(selectedFeeling);
            var feelinfs = new PSJJournalEntry
            {
                EmotionTagID=feelindID,
                PSUserID = PsUserLoginModel.instance.Value.ID,
                JournalEntry = JournalEntry,
                DateAdded = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                JournalGoalsID = 0

            };

            await sQLiteService.AddPSFeelings(feelinfs);
        }
        private async Task AddGoals()
        {
            StopRecording();

           // var maxID = await sQLiteService.GetMaxId("PSJJournalEntry");

            var feelinfs = new PSJounalEntryGoals
            {
                JournalEntry = JournalEntryGoals,
                PSUserID = PsUserLoginModel.instance.Value.ID,
               
                DateAdded = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")


            };

            await sQLiteService.AddUpdateJournalEntryGoals(feelinfs);
         // var  maxID = await sQLiteService.GetMaxId("PSJounalEntryGoals");
            await sQLiteService.UpDateTableByID("JournalGoalsID", string.Empty, "PSJJournalEntry", 0, true);
            OnClearTextClicked();
            JournalEntryGoalsVisable = false;
        }
        private void OnClearTextClicked()
        {
            JournalEntry = string.Empty;
            JournalEntryGoals = string.Empty;
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
            SpeakingJournal = true;
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
            StopRecording();
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
                    if (string.IsNullOrEmpty(text)) return;
                    if (SpeakingJournal)
                        JournalEntry = text;
                    else
                        journalEntryGoals = text;
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
                    if (!(string.IsNullOrEmpty(text)))
                    {
                        if (SpeakingJournal)
                            JournalEntry = text;
                        else
                            journalEntryGoals = text;
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
        public async Task StopVoiceJournalAsync()
        {
            await speechUtility.StopListeningAsync();
        }
       public async void StopRecording()
        {
            IsSpeaking = true;
           IsNotSpeaking = false;
            IsSpeakingJournalGoals = true;
           IsNotVoiceSpeakingJournalGoals = false;
             
            await StopVoiceJournalAsync();
        }
        async void OnClickPlayTextCommand(string editorName)
        {
            if (string.Compare(editorName, "JournalGoals", true) == 0)
            {
                if (!string.IsNullOrEmpty(JournalEntryGoals))
                {
                    await speechToTextService.SpeakText(JournalEntryGoals);
                }

            }
            else
            {

                if (!string.IsNullOrEmpty(JournalEntry))
                {
                    await speechToTextService.SpeakText(JournalEntry);
                }
            }


        }
        
        async void OnClickInsertEmjoiJournalEntry(string editorText)
        {
            var popup = new InsertEmjoiPage();
            Editor editor = new Editor();
            
            //var mainPage = Application.Current?.MainPage?;
            // The type parameter must match the type returned from the popup.
            //  OnPropertyChanged(nameof(CursorIndex));
            IPopupResult<string> popupResult = (IPopupResult<string>)await Application.Current.MainPage?.ShowPopupAsync<string>(popup, new PopupOptions{CanBeDismissedByTappingOutsideOfPopup = false}, CancellationToken.None);
            if (!(string.IsNullOrWhiteSpace(popupResult.Result)))
            {
                editor.Text = JournalEntry;
                editor.CursorPosition = CursorIndex;
                  if(string.Compare(editorText, "JournalGoals",true) == 0)
                {
                    editor.Text = JournalEntryGoals;
                    editor.CursorPosition = CursorIndexJG;

                }
                  
                 Utilites.LoadEmojis.InsertEmoji(editor, popupResult.Result);
                // JournalEntry = $"{JournalEntry}{popupResult.Result}";
                
                if (string.Compare(editorText, "JournalGoals", true) == 0)
                {
                 JournalEntryGoals = editor.Text;

                }
                else
                { 
                    JournalEntry = editor.Text;
                }
                
            }

        }
      
        //public ICommand NavigateCommand => new AsyncRelayCommand(async () =>
        //{
        //    // Perform asynchronous operations here if needed
        //    await Task.Delay(100); // Example of an async operation
        //    await Shell.Current.GoToAsync(nameof(GoalsPage));
        //});
        private async void OnClickNav(string navigation)
        {
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync($"{navigation}");
               // await Shell.Current.GoToAsync(nameof(navigation));
            }
             
              //  await Shell.Current.GoToAsync(nameof(navigation));
            //    GoalsPage navPage = new GoalsPage();
            //  NavigateCommand = new Command(async () =>
            //    {
            //   await Shell.Current.GoToAsync(nameof(navigation));
             // });
            // NavigateCommand = new Command<Type>(
            //async (Type pageType) =>
            //{
            //    Page page = (Page)Activator.CreateInstance(pageType);
            //    await Navigation.PushAsync(page);
            //    // If you want to use a NavigationPage, uncomment the next line
            //    //  await Navigation.PushAsync(page);
            //    //  await navigationPage.Navigation.PushAsync(page);
            //    // await Navigation.PushModalAsync(new NavigationPage(new Type(page)))
            //});
            //  await Shell.Current.GoToAsync(nameof(navigation));

        }
        private async void OnClickNavCmd(string navigation)
        {
            if (Shell.Current != null)
            {
                //     await Shell.Current.GoToAsync($"{navigation}");
                // await Shell.Current.GoToAsync(nameof(navigation));
            }

            //  await Shell.Current.GoToAsync(nameof(navigation));
            //    GoalsPage navPage = new GoalsPage();
            NavigateCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync(nameof(navigation));
            });
            // NavigateCommand = new Command<Type>(
            //async (Type pageType) =>
            //{
            //    Page page = (Page)Activator.CreateInstance(pageType);
            //    await Navigation.PushAsync(page);
            //    // If you want to use a NavigationPage, uncomment the next line
            //    //  await Navigation.PushAsync(page);
            //    //  await navigationPage.Navigation.PushAsync(page);
            //    // await Navigation.PushModalAsync(new NavigationPage(new Type(page)))
            //});
            //  await Shell.Current.GoToAsync(nameof(navigation));

        }

    }
    }

