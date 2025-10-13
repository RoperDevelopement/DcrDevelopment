using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Media;
using BMRMobileApp.InterFaces;
namespace BMRMobileApp.Utilites
{
    public class TextSpeakerService
    {
        public async Task SpeakText(string message)
        {
            await TextToSpeech.Default.SpeakAsync(message);
        }
        public async Task SpeakWithOptions(string message)
        {
            var locales = await TextToSpeech.Default.GetLocalesAsync();

            var options = new SpeechOptions
            {
                Pitch = 1.5f, // Range: 0.0 to 2.0
                Volume = 0.75f, // Range: 0.0 to 1.0
                Locale = locales.FirstOrDefault() // Choose a specific language/region
            };

            await TextToSpeech.Default.SpeakAsync(message, options);
            Task.WaitAll();
        }
        public async Task SpeakAsync(string text, float pitch = 1.0f, float volume = 1.0f, string? locale = null)
        {
            var options = new SpeechOptions
            {
                Pitch = pitch,
                Volume = volume
            };

            if (!string.IsNullOrEmpty(locale))
            {
                var locales = await TextToSpeech.Default.GetLocalesAsync();
                options.Locale = locales.FirstOrDefault(l => l.Language == locale);
            }

            await TextToSpeech.Default.SpeakAsync(text, options);
            Task.WaitAll();
        }
    }
}
