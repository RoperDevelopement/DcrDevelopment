using BMRMobileApp.Models;
using BMRMobileApp.SQLiteDBCommands;
using BMRMobileApp.Utilites;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BMRMobileApp.ViewModels
{

    public class ResourceViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        string cPText;
        Color cPBC;
        private readonly SQLiteService sqLCmd = new SQLiteService();
        private ObservableCollection<PickerModel> recourceSites = new ObservableCollection<PickerModel>();
        public event PropertyChangedEventHandler PropertyChanged;
        private bool checkedOpenInternal;
        private bool checkedOpenBrowser;
        private string newSiteName;
        private string newSiteUrlText;
       private ObservableCollection<PSRecourceSites> sitesDelete = new ObservableCollection<PSRecourceSites>();
        public PickerModel selectedSite = new PickerModel();
        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
        public ICommand DeleteSite { get; }
        public ICommand AddSite { get; }
        
       // public Command CheckChange { get; }

        // public event PropertyChangingEventHandler PropertyChanging;
        public ResourceViewModel()
        {
            GetMood();
            GetResourceSites();
            CheckedOpenInternal = true;
            CheckedOpenBrowser = false;
            DeleteSite = new Command<PSRecourceSites>(OnClickDeleteSite);
            AddSite = new Command(OnClickAddSite);
            
           
         //   CheckChange = new Command(OnClickCheckChange);
        }

        //public ObservableCollection<SelectWebSite> RecourceSites
        //{ 
        //    get => recourceSites;
        //    set
        //    {
        //        recourceSites = value;
        //        OnPropertyChanged(nameof(RecourceSites));
        //    }
        //}
        private async void OpenBrowser()
        {
            //Command OpenWebCommand = new Command(async () =>
            ///await Browser.OpenAsync(SelectedSite.Value, BrowserLaunchMode.SystemPreferred));
            ///
            if((SelectedSite == null) ||(string.IsNullOrWhiteSpace(SelectedSite.Value))) { return; }
            if(CheckedOpenInternal)
            {
                if(Shell.Current != null)
                                    await Shell.Current.GoToAsync($"OpenWebPagePhonePage?webSitePage={SelectedSite.Value}");
                    //await Shell.Current.GoToAsync($"openwebpagecontentpage");
                //await Shell.Current.GoToAsync($"resourcespage");
            }
            else
            await Browser.OpenAsync(SelectedSite.Value, BrowserLaunchMode.SystemPreferred);


        }
        public ObservableCollection<PickerModel> RecourceSites { get; set; } = new ObservableCollection<PickerModel> { new PickerModel() };
        public ObservableCollection<PSRecourceSites> SitesDelete
        { 
          get => sitesDelete;
            set
            {
                
                    sitesDelete = value;
                    OnPropertyChanged(nameof(SitesDelete));
                

                //Debug.WriteLine($"Selected mood value: {_selectedMood?.Value}");
            }
}
        public string NewSiteName
        {
            get => newSiteName;
            set
            {
                newSiteName = value;

            }
        }
        public string NewSiteUrlText
        {
            get => newSiteUrlText;
            set
            {
                newSiteUrlText = value;
                OnPropertyChanged(nameof(NewSiteUrlText));
            }
        }
public PickerModel SelectedSite
        {
            get => selectedSite;
            set
            {
                if (selectedSite != value)
                {
                    selectedSite = value;
                    OnPropertyChanged();
                    OpenBrowser();

                    //Debug.WriteLine($"Selected mood value: {_selectedMood?.Value}");
                }
            }
        }

        public bool CheckedOpenBrowser
        {
            get => checkedOpenBrowser;
            set
            {

                checkedOpenBrowser = value;

                OnPropertyChanged();

                //Debug.WriteLine($"Selected mood value: {_selectedMood?.Value}");

            }
        }
        
        public bool CheckedOpenInternal
        {
            get => checkedOpenInternal;
            set
            {
                
                checkedOpenInternal = value;
                OnPropertyChanged();

                //Debug.WriteLine($"Selected mood value: {_selectedMood?.Value}");

            }
        }
        private async void GetMood()
        {
            CPText = "Resources ";
            CPBC = Color.FromArgb(Utilites.Consts.DefaultBackgroundColor);
            var mood = sqLCmd.GetUserCurrentMoodNonAsync();
            if (mood != null)
            {
                CPText = $"{CPText} {mood.Mood} {mood.MoodTag}";
                CPBC = Color.FromArgb(mood.BackgroundColor);
            }
        }
        public string CPText
        {
            get => cPText;
            set
            {
                cPText = value;
                OnPropertyChanged(nameof(CPText));
            }
        }
        public Color CPBC
        {
            get => cPBC;
            set
            {
                cPBC = value;
                OnPropertyChanged(nameof(CPBC));
            }
        }
        private async void GetResourceSites()
        {
            var rs = sqLCmd.GetResSites();
            if ((rs != null) && (rs.Count > 0))
            {
                if (RecourceSites.Count > 0)
                {
                    RecourceSites.Clear();
                    SitesDelete.Clear();
                }
                //RecourceSites = await Utilites.EnumerableExtensions.ToObservableCollection<PSRecourceSites>(rs);
                foreach (var site in rs)
                {
                    
                        SitesDelete.Add(new PSRecourceSites { ID = site.ID,SupportedSiteName=site.SupportedSiteName,SupportWebSites=site.SupportWebSites,DeleteResource=site.DeleteResource });
                    RecourceSites.Add(new PickerModel { Label = site.SupportedSiteName, Value = site.SupportWebSites });
                }

            }
        }
        protected void OnPropertyChanged(string name = null) =>
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        protected virtual void OnPropertyChanging(string propertyName)
        {
            PropertyChanging?.Invoke(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
        }
        public async void OnClickDeleteSite(PSRecourceSites pSRecource)
        {
            try
            {
                if(pSRecource.DeleteResource == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Error:", $"Site Name {pSRecource.SupportedSiteName} cannot be deleted", "OK");
                    return;
                }

                await sqLCmd.DelDataAsyncByID<PSRecourceSites>(pSRecource, pSRecource.ID);
                GetResourceSites();
            }
            catch (Exception ex)
            {
              await  Application.Current.MainPage.DisplayAlert("Error:", $"Could not delete site name {pSRecource.SupportedSiteName}", "OK");
            }
        }
        public async void OnClickAddSite()
        {
            if(string.IsNullOrWhiteSpace(NewSiteName))
            {
                await Application.Current.MainPage.DisplayAlert("Error:", $"Need Site Name", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(NewSiteUrlText))
            {
                await Application.Current.MainPage.DisplayAlert("Error:", $"Need Site URL", "OK");
                return;
            }
            if (await Utilites.PSUtilites.IsWebsiteReachableAsync(NewSiteUrlText))
            {
                var newRec = new PSRecourceSites{ SupportedSiteName = NewSiteName, SupportWebSites = NewSiteUrlText, DeleteResource = 1 };
               await sqLCmd.InserDataAsync(newRec);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error:", $"Invalid Site URL {NewSiteUrlText}", "OK");
                return ;
            }
            NewSiteUrlText=string.Empty;
            newSiteName=string.Empty;
            
            GetResourceSites();
        }
     
    }

}



