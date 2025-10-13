using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

//https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/views/popup/popup-result
namespace BMRMobileApp;

public partial class InsertEmjoiPage : Popup<string>, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private int _currentPage = 0;
    private   const int PageSize = 25;
    private int lastPage = 0;
    private ObservableCollection<string> eItems = new ObservableCollection<string>();
    //  ObservableCollection<string> Emojis;
    //ObservableCollection<string> EmojisItems;
    public InsertEmjoiPage()
    {
        InitializeComponent();
        BindingContext = this;
        // EmojisItems = new ObservableCollection<string>();
        BackgroundColor = Color.FromArgb(Utilites.Consts.DefaultBackgroundColor);
        LoadItemsAsync();
        
        //  ObservableCollection<string> Emojis = new ObservableCollection<string>();
        //this.Opacity = 0;
    }
    [ObservableProperty]
    public ObservableCollection<string> EmojisItems
    {
        get => eItems;
        set
        {
            eItems = value;
            OnPropertyChanged(nameof(EmojisItems));
        }

    }

    public ObservableCollection<string> Emojis { get; set; } = new ObservableCollection<string>();
    // public ObservableCollection<string> EmojisItems { get; set; } = new ObservableCollection<string>();


    private async Task LoadItemsAsync()
    {
        Loading.IsVisible = false;
        MyCollectionView.IsVisible = false;
        SlButtons.IsVisible = false;
        await  MyCollectionView.FadeTo(0, 500, Easing.BounceOut);
        await MyCollectionView.ScaleTo(1.05, 200, Easing.BounceOut);
        if ((EmojisItems == null) || (EmojisItems.Count == 0))
        {
            EmojisItems = await Utilites.LoadEmojis.GetObservableCollectionEmjo();
            await LoadInitialItems();
        }
        else
        {

            Task.Delay(500).ContinueWith(_ =>

                  {
                      _currentPage = 0;
                      for (int i = Emojis.Count()+1; i < EmojisItems.Count(); i++)
                      {

                          Emojis.Add(EmojisItems[i]);
                          _currentPage++;
                          if (_currentPage > PageSize)
                              break;
                      }

                  }, TaskScheduler.FromCurrentSynchronizationContext());
           

      
        }
        await Task.WhenAll(
    MyCollectionView.FadeTo(1, 500, Easing.BounceIn),
     MyCollectionView.ScaleTo(1, 500)
);
        MyCollectionView.IsVisible=true;
        SlButtons.IsVisible = true;
        Loading.IsVisible = false;
        //await  MyCollectionView.FadeTo(1, 500, Easing.CubicIn);
    }
    //private async Task LoadItemsAsync()
    //{
    //    try
    //    {


    //        this.Opacity = 50;
    //        //   await this.FadeTo(1, 1000);
    //        // await Task.Delay(1000);
    //        if ((EmojisItems == null) || (EmojisItems.Count == 0))
    //        {
    //            EmojisItems = await Utilites.LoadEmojis.GetObservableCollectionEmjo();
    //            await LoadInitialItems();
    //            //     await LoadInit();
    //        }
    //        else
    //        { 
    //            // Simulate data fetching
    //            Task.Delay(1000).ContinueWith(_ =>

    //            {
    //                if ((_currentPage + 1) >= EmojisItems.Count())
    //                {
    //                    return;
    //                }
    //                    for (int i = 1; i <= 10; i++)
    //                {
    //                    if ((_currentPage+i) >= EmojisItems.Count())
    //                    {
    //                        //Emojis.Add(EmojisItems[EmojisItems.Count()]);
    //                        break;
    //                    }
    //                    Emojis.Add(EmojisItems[_currentPage + i]);
    //                }
    //                _currentPage += PageSize;

    //            }, TaskScheduler.FromCurrentSynchronizationContext());
    //    }

    //        //for (int i = 0; i < PageSize; i++)
    //        //{
    //        //    Emojis.Add(EmojisItems[_currentPage * PageSize + i]);
    //        //}

    //        //_currentPage++;

    //    }
    //    catch (Exception ex)
    //    { 
    //        Console.WriteLine(ex.Message);
    //    }  

    //    this.Opacity = 100;

    //   // Let UI catch up
    //}
    private async Task LoadInitialItems()
    {
        for (int i = 1; i <= PageSize; i++)
        {

            Emojis.Add(EmojisItems[i]);

        }
           
    }

    async void OnEmojiTapped(object sender, TappedEventArgs e)
    {
        var label = sender as Label;
        var emoji = label?.Text;
        if (emoji != null)
        {
            // EmojiSelected?.Invoke(this, emoji);
            await CloseAsync(emoji.ToString());
        }
    }
     
    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await CloseAsync(string.Empty);

    }

    //private async void CollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
    //{

    //       await LoadItemsAsync();
    //}

    //private async void CollectionViewScrolled(object sender, ItemsViewScrolledEventArgs e)
    //{
    //    try
    //    {
    //        //   int pageCount = Application.Current.MainPage.Navigation.NavigationStack.Count;
    //        //  MyCollectionView.ScrollTo(Emojis.Last(), position: ScrollToPosition.End, animate: false);
    //        // await Task.Delay(1000);
    //        // PageSize = e.LastVisibleItemIndex + 1;
    //        await LoadItemsAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine(ex.Message);
    //    }
    //    //   if (e.LastVisibleItemIndex >= Emojis.Count - 5)
    //    // {
    //    //   await LoadItemsAsync();
    //    //  MyCollectionView.ScrollTo(Emojis.Last(), position: ScrollToPosition.End, animate: false);
    //    // }


    //}
    protected void OnPropertyChanged(string name) =>
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private async void AddIcons_Clicked(object sender, EventArgs e)
    {

        await Scroll();
      

    }

     

    private async void MyCollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        //await LoadItemsAsync();
        //MyCollectionView.ScrollTo(Emojis.Last(), position: ScrollToPosition.End, animate: false);
        //await Task.Delay(1000);

        //if (Emojis.Count() - 1 == EmojisItems.Count() - 1)
        //    AddEmjo.IsEnabled = false;
        await Scroll();

    }

    private async void MyCollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        await Scroll();
    }
    private async Task Scroll()
    {
        // MyCollectionView.Opacity = 0.6;
        await LoadItemsAsync();
        //MyCollectionView.Opacity = 50;


        //this.FadeTo(10, 2000);
        MyCollectionView.ScrollTo(Emojis.Last(), position: ScrollToPosition.End, animate: false);
        await Task.Delay(1000);

        if (Emojis.Count() - 1 == EmojisItems.Count() - 1)
            AddEmjo.IsVisible = false;

        // await MyCollectionView.ScaleTo(1, 500);
    }
}
