using BMRMobileApp.Models;
using BMRMobileApp.Utilites;
using BMRMobileApp.ViewModels;
using CommunityToolkit.Maui.Extensions;
using System.Threading.Tasks;
using System.Windows.Input;
namespace BMRMobileApp;
//https://www.nuget.org/packages/AlohaKit.UI
public partial class FeelingPage : ContentPage
{
    FeelingViewModel feelingPage = new FeelingViewModel();
   
    private VoiceWaveControl _voiceWave;
    public FeelingPage(bool pickerVisable=true)
	{
		InitializeComponent();
        PickerVisable.IsVisible = pickerVisable;
        // Shell.SetNavBarIsVisible(this, true);
        BindingContext = feelingPage;
      
        //_voiceWave = new VoiceWaveControl
        //{
        //    HeightRequest = 160,
        //    WaveColor = Color.FromArgb("#90A4AE"), // Sad tone
        //    Background = new LinearGradientBrush(
        //       new GradientStopCollection {
        //            new GradientStop(Color.FromArgb("#37474F"), 0.0f),
        //            new GradientStop(Color.FromArgb("#90A4AE"), 1.0f)
        //       },
        //       new Point(0, 0),
        //       new Point(1, 1)
        //   ),
        //    HorizontalOptions = LayoutOptions.Fill,
        //    VerticalOptions = LayoutOptions.Center,
        //    IsVisible = false
        //};

        //MainPage.Children.Insert(2,_voiceWave);
    }
    private async void OnCloseClicked(object sender, EventArgs e)
    {
        if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0] is Window window)
        {
            window.Page = new AppShell();
        }

    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
          feelingPage?.StopVoiceJournalAsync();
       

    }
    void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        var editor = sender as Editor;
        var newText = e.NewTextValue;
       // var replacedText = ReplaceEmojiTags(newText);

       // if (newText != replacedText)
       // {
         //   editor.TextChanged -= OnEditorTextChanged; // prevent recursion
          //  editor.Text = replacedText;
          //  editor.CursorPosition = replacedText.Length; // optional: keep cursor at end
          //  editor.TextChanged += OnEditorTextChanged;
       // }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
      //  _ = feelingPage.speechToTextService.SpeakText($"Hello {PsUserLoginModel.instance.Value.PSUserDisplayName} hopefully your doing good today.");
        //Task.Delay(5000).Wait();
        Thread.Sleep( 1000 );
        feelingPage.Init();
        PickerVisable.SelectedIndex = -1;


        // Fire and forget the async call, handle exceptions as needed


        //    OnPropertyChanged(nameof(ImgProfPic));
        //  Shell.SetTabBarIsVisible(this, true);
        //    MainThread.BeginInvokeOnMainThread(() =>
        //  {
        //   Shell.SetTabBarIsVisible(this, true);
        // });
        //0 viedoNotes = new ObservableCollection<ViedoNotesModel>();

    }

    private void Editor_TextChanged(object sender, TextChangedEventArgs e)
    {
     //  Editor editor1 = sender as Editor;
       // var focusedEditor = EdJournal.FirstOrDefault(e => e.IsFocused);

        var editor = sender as Editor;
        //int cursorIndex = editor.CursorPosition;
        //int selectionLength = editor.SelectionLength;

        if (BindingContext is FeelingViewModel vm)
        {

            if (string.Compare(editor?.StyleId.ToString(), "EdJournal", true) == 0)
            {
                vm.CursorIndex = editor.CursorPosition;
                vm.SelectionLength = editor.SelectionLength;


            }
            else
            {
                vm.CursorIndexJG = editor.CursorPosition;
                vm.SelectionLengthJG = editor.SelectionLength;
            }
        }

    }

    private async void PickerVisable_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if ((picker != null) && picker.SelectedIndex != -1)
        {




             PickerModel pickerModel = feelingPage.PM[picker.SelectedIndex];
            feelingPage.SelectedSite.Value = pickerModel.Value;
                    feelingPage.OpenJournalSite();
           
            
        }
        
        
    }

    //{

    //    var popup =  LoadEmojis.EmojiPopup(emoji =>
    //    {
    //        var cursor = .CursorPosition;
    //        var text = emojiEditor.Text ?? "";
    //        emojiEditor.Text = text.Insert(cursor, emoji);
    //        emojiEditor.CursorPosition = cursor + emoji.Length;
    //        this.HidePopup(); // if using a popup manager
    //    });

    //    this.ShowPopup(popup); // requires CommunityToolkit.Maui
    //}
}