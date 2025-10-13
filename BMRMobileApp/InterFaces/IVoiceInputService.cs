using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.InterFaces
{
    public interface IVoiceInputService
    {
        Task<string?> ListenAsync(CancellationToken token);
    }

    public interface IAudioPlaybackService
    {
        void Play(string filePath);
        void Stop();
    }
    public interface IID
    {
        int ID { get; set; }
    }
        public interface IAudioRecordingService
        {
            Task StartRecordingAsync();
            Task<string?> StopRecordingAsync(); // Returns path to saved file
        }
        public interface IViedoServiceDB : IID
        {
            string VideoPath { get; set; }
              string DateViedoTaken
            {
                get; set;
            }
        string ViedoTitle { get; set; }
        }
        public interface ISpeechService
        {
            Task StartListeningAsync(CancellationToken token);
            Task StopListeningAsync(CancellationToken token);
            event EventHandler<string> OnPartialResult;
            event EventHandler<string> OnFinalResult;
        }
        public interface ISearchWebService
        {
             string Title { get; set; }
             string Link { get; set; }
             string Snippet { get; set; }
        }
        public interface ITxtSpeaker
        {
           // Task  SpeakAsync(string text, float pitch = 1.0f, float volume = 1.0f, string? locale = null);
            Task SpeakWithOptions(string message);
             Task SpeakText(string message);
        }
        public interface IAudioInputService
        {
            event Action<float[]> OnAudioSample;
            void Start();
            void Stop();
        }
    }
 
