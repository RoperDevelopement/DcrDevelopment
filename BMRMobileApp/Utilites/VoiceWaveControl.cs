using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.InterFaces;
namespace BMRMobileApp.Utilites
{
    public class VoiceWaveControl:ContentView
    {
        private readonly GraphicsView _graphicsView;
        private readonly WaveformDrawable _drawable;

        public static readonly BindableProperty WaveColorProperty =
            BindableProperty.Create(nameof(WaveColor), typeof(Color), typeof(VoiceWaveControl), Colors.DeepSkyBlue,
                propertyChanged: OnWaveColorChanged);

        public Color WaveColor
        {
            get => (Color)GetValue(WaveColorProperty);
            set => SetValue(WaveColorProperty, value);
        }

        private static void OnWaveColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is VoiceWaveControl control && newValue is Color color)
            {
            //    control._drawable.WaveColor = color;
                control._graphicsView.Invalidate();
            }
        }

        public VoiceWaveControl()
        {
         //   _drawable = new WaveformDrawable();
            _graphicsView = new GraphicsView { Drawable = _drawable };
            Content = _graphicsView;

            //var audioService = DependencyService.Get<IAudioInputService>();
            //audioService.OnAudioSample += samples => {
            //    _drawable.Samples = samples;
            //    _graphicsView.Invalidate();
            //};
        }

         public void Start() => DependencyService.Get<IAudioInputService>().Start();
       public void Stop() => DependencyService.Get<IAudioInputService>().Stop();

    }
}
