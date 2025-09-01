using BMRMobileApp.Utilites;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using System.Globalization;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
namespace BMRMobileApp
{

    public partial class RecordVoicePage : ContentPage
    {
        ISpeechToText speechToText = SpeechToText.Default;
        private readonly SpeechToTextUtility speechUtility = new();
       
        public RecordVoicePage()
        {
            InitializeComponent();
            BindingContext = this;
            speechUtility.PartialResultReceived += OnPartialResult;
            speechUtility.FinalResultReceived += OnFinalResult;
        }
        public async Task ListenAndFillTextBox(Entry entryControl)
        {
            var cancellationToken = new CancellationToken();


            var isGranted = await SpeechToText.Default.RequestPermissions(cancellationToken);
            if (!isGranted)
            {
                await Toast.Make("Microphone permission not granted").Show(cancellationToken);
                return;
            }
            speechToText.RecognitionResultUpdated += OnPartialResult;
            speechToText.RecognitionResultCompleted += OnFinalResult;
            await speechToText.StartListenAsync(new SpeechToTextOptions
            {
                Culture = CultureInfo.CurrentCulture,
                ShouldReportPartialResults = true
                
            }, CancellationToken.None);
        }
        private void OnPartialResult(object sender, SpeechToTextRecognitionResultUpdatedEventArgs e)
        {
            JournalEditor.Text = e.RecognitionResult;
        }

        private void OnFinalResult(object sender, SpeechToTextRecognitionResultCompletedEventArgs e)
        {
            JournalEditor.Text = e.RecognitionResult.Text;
            //ButtonStartListing.IsEnabled = true;
            speechToText.RecognitionResultUpdated -= OnPartialResult;
            speechToText.RecognitionResultCompleted -= OnFinalResult;
        }
        private async void OnMicClicked(object sender, EventArgs e)
        {
              ButtonStartListing.IsVisible = false;
            ButtonStopListing.IsVisible = true;
            //  await ListenAndFillTextBox(null);
            await StartVoiceJournalAsync();
        //  await SpeakText(JournalEditor.Text);
        }

        private async void OnMicStopClicked(object sender, EventArgs e)
        {
            ButtonStartListing.IsVisible = true;
            ButtonStopListing.IsVisible = false;
            //  await ListenAndFillTextBox(null);
            await StopVoiceJournalAsync();
            //  await SpeakText(JournalEditor.Text);
        }

        
        
        public async Task SpeakText(string text)
        {
            await TextToSpeech.Default.SpeakAsync(text);
        }
        public async Task StartVoiceJournalAsync()
        {
            var ready = await speechUtility.InitializeAsync(CancellationToken.None);
            if (!ready) return;

          //  speechUtility.PartialResultReceived += (s, text) => UpdateLiveTranscript(text);
          //  speechUtility.FinalResultReceived += (s, text) => SaveFinalTranscript(text);

            await speechUtility.StartListeningAsync();
        }

        public async Task StopVoiceJournalAsync()
        {
            await speechUtility.StopListeningAsync();
        }
        private void OnPartialResult(object? sender, string text)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    JournalEditor.Text = text;
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
                    JournalEditor.Text = text;

                });   // Final transcript after stop
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Optional: Save to journal, tag emotion, trigger animation
          //  SaveTranscript(text);
        }
        protected override void OnDisappearing()
        {
           base.OnDisappearing();
           speechUtility?.StopListeningAsync();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            JournalEditor.Text = string.Empty;

        }

    }
}