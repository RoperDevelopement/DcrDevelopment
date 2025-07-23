using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mobile_LabReqs.ViewsModels;
using Mobile_LabReqs.Views;
using System.Web;
using Mobile_LabReqs.Services;
using System.Collections.Generic;

namespace Mobile_LabReqs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //[QueryProperty(nameof(SearchStartDate), "SearchStartDate")]
    //[QueryProperty(nameof(SearchEndDate),"SearchEndDate")]
    public partial class LabReqsListViewPage : ContentPage
    {
        // public ObservableCollection<LabReqsModel> Items { get; set; }
        public IList<LabReqsModel> Items { get; set; }
       

        public DateTime SearchStartDate
        {
            get { return Const.SearchStartDate; }
            //set
            //{
            //    stdate = Uri.UnescapeDataString(value ?? string.Empty);
            //    OnPropertyChanged();
            //}
        }
        public LabReqsListViewPage()
        {
            InitializeComponent();
            
            Routing.RegisterRoute(nameof(LabReqsDetailPageModel), typeof(LabReqsDetailPageModel));

            //   BindingContext = new LabReqsModel();
            //Items = new ObservableCollection<string>
            //{
            //    "Item 1",
            //    "Item 2",
            //    "Item 3",
            //    "Item 4",
            //    "Item 5"
            //};



        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if(string.Compare(Const.StoreProcuder,Const.KeyWordsSP,true) == 0)
            await GetLabReqsKeyWords();
            else
                await GetLabReqs();
            Title = $"Vewing LabReq {SearchStr}";
            
            Device.BeginInvokeOnMainThread(() => { MyListView.ItemsSource = Items; });
            

        }
        public async Task GetLabReqs()
        {
            LabReqsModel labReqs = new LabReqsModel();
            labReqs.FinancialNumber = SearchStr;
            labReqs.IndexNumber = SearchStr;
            labReqs.ReceiptDate = SearchStartDate;
            labReqs.ScanDate = SearchEndDate;
            labReqs.DateOfService = SearchEndDate;
            labReqs.ScanMachine = Const.SearchByDateofServiceScanDate;
            labReqs.SearchPartial = SearchPart;
            labReqs.PatientID = SearchStr;
            labReqs.MRN = SearchStr;

            // labReqs.IndexNumber = labReqs.FinancialNumber= "157287792";
            Items = await Services.RestApi.GetLabReqs($"{Const.WebUrl}{Const.ApiNypLabReqsController}", Const.StoreProcuder, labReqs);
        }

        public async Task GetLabReqsKeyWords()
        {
             

            // labReqs.IndexNumber = labReqs.FinancialNumber= "157287792";
            Items = await Services.RestApi.GetLabReqsByKeyWords(Const.WebUrl,Const.ApiNypLabReqsKeyWords,SearchStartDate,SearchEndDate,Const.SearchByDateofServiceScanDate,SearchStr);
        }
        public string SearchStr
        { get { return Const.SearchStr; } }
        public bool SearchPart
        { get { return Const.SearchPart; } }
        public DateTime SearchEndDate
        {
            get { return Const.SearchEndDate; }
            //get => stdate;
            //set
            //{
            //    enddate = Uri.UnescapeDataString(value ?? string.Empty);
            //    OnPropertyChanged();
            //}
        }
        
        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
           
          //  await DisplayAlert("Item Tapped", "An item was tapped.", "OK");
            //   await Shell.Current.GoToAsync($"{nameof(LabReqsListViewPage)}?{nameof(l.ItemId)}={(ListView)sender).SelectedItem}");
            //Deselect Item
            object o = ((ListView)sender).SelectedItem;
            LabReqsModel ItemId = (LabReqsModel)o;
            var jsonStr = JsonConvert.SerializeObject(ItemId);
        
             await Shell.Current.GoToAsync($"{nameof(LabReqsDetailViewPage)}?JsonStr={jsonStr}");
          //  await Shell.Current.GoToAsync($"{nameof(LabReqsDetailViewPage)}");
            ((ListView)sender).SelectedItem = null;
        }
    }
}
