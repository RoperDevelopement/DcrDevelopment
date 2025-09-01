using BMRMobileApp.InterFaces;
//using CommunityToolkit.Maui.MediaElement;
using BMRMobileApp.Models;
using BMRMobileApp.SQLiteDBCommands;
using BMRMobileApp.Utilites;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Plugin.Maui.Audio;
namespace BMRMobileApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                 .AddAudio(
                playbackOptions =>
                {
#if IOS || MACCATALYST
					playbackOptions.Category = AVFoundation.AVAudioSessionCategory.Playback;
#endif
#if ANDROID
                    playbackOptions.AudioContentType = Android.Media.AudioContentType.Music;
                    playbackOptions.AudioUsageKind = Android.Media.AudioUsageKind.Media;
#endif
                },
                recordingOptions =>
                {
#if IOS || MACCATALYST
					recordingOptions.Category = AVFoundation.AVAudioSessionCategory.Record;
					recordingOptions.Mode = AVFoundation.AVAudioSessionMode.Default;
					recordingOptions.CategoryOptions = AVFoundation.AVAudioSessionCategoryOptions.MixWithOthers;
#endif
                },
                streamerOptions =>
                {
#if IOS || MACCATALYST
					streamerOptions.Category = AVFoundation.AVAudioSessionCategory.Record;
					streamerOptions.Mode = AVFoundation.AVAudioSessionMode.Default;
					streamerOptions.CategoryOptions = AVFoundation.AVAudioSessionCategoryOptions.MixWithOthers;
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIcons.ttf", "MaterialIcons");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
                });


#if DEBUG
    		builder.Logging.AddDebug();

#endif 
           // builder.Services.AddSingleton<PsUserLoginModel>();
            builder.Services.AddSingleton<SQLiteService>(s =>
            {
                return new SQLiteService();
           });
            builder.UseMauiCommunityToolkit();
            builder.UseMauiCommunityToolkitMediaElement();
            builder.AddAudio();
            // builder.UseMauiCommunityToolkitCamera();
            ConfigurationManager.Initialize();
            return builder.Build();
        }
    }
}
