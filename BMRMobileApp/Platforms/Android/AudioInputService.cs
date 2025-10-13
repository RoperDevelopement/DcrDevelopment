using Android.Media;
using BMRMobileApp.InterFaces;
using BMRMobileApp.Platforms.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[assembly: Dependency(typeof(AudioInputService))]
namespace BMRMobileApp.Platforms.Android
{
    public class AudioInputService : IAudioInputService
    {
        
            public event Action<float[]> OnAudioSample;

            private AudioRecord _recorder;
            private bool _isRecording;

            public void Start()
            {
                // Initialize AudioRecord and start capturing
                // Convert raw audio to amplitude samples
                // Call OnAudioSample?.Invoke(samples);
            }

            public void Stop()
            {
                _isRecording = false;
                _recorder?.Stop();
            }
        }
    
}
