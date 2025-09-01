using CommunityToolkit.Maui.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.Utilites
{

    public class SpeechToTextUtility
    {
        private readonly ISpeechToText speechToText = SpeechToText.Default;
        private CancellationToken _cancellationToken;

        public event EventHandler<string>? PartialResultReceived;
        public event EventHandler<string>? FinalResultReceived;

        public async Task<bool> InitializeAsync(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            var permissionGranted = await speechToText.RequestPermissions(cancellationToken);
            return permissionGranted;
        }

        public async Task StartListeningAsync()
        {

            speechToText.RecognitionResultUpdated += HandlePartialResult;
            speechToText.RecognitionResultCompleted += HandleFinalResult;

            await speechToText.StartListenAsync(new SpeechToTextOptions
            {
                Culture = CultureInfo.CurrentCulture,
                ShouldReportPartialResults = true
            }, _cancellationToken);
        }

        public async Task StopListeningAsync()
        {
            await speechToText.StopListenAsync(_cancellationToken);

            speechToText.RecognitionResultUpdated -= HandlePartialResult;
            speechToText.RecognitionResultCompleted -= HandleFinalResult;
        }

        private void HandlePartialResult(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs e)
        {
            PartialResultReceived?.Invoke(this, e.RecognitionResult);
        }

        private void HandleFinalResult(object? sender, SpeechToTextRecognitionResultCompletedEventArgs e)
        {
            FinalResultReceived?.Invoke(this, e.RecognitionResult.Text);
        }

        public async Task SpeakText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;
            var speechOptions = new SpeechOptions
            {
                Volume = 1.0f,
                Pitch = 1.0f,
                

            };
            await TextToSpeech.Default.SpeakAsync(text, speechOptions);
        }
    }
    }
 
