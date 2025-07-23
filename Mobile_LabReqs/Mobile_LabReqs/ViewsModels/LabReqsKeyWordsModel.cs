using Mobile_LabReqs.Services;
using Mobile_LabReqs.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;


namespace Mobile_LabReqs.ViewsModels
{
  public  class LabReqsKeyWordsModel:BaseViewModel
    {
        public Command SearchCommand { get; }
        public LabReqsKeyWordsModel()
        {
            Title = "Search By KeyWords";
            SearchCommand = new Command(GetLabReqsKeyWords);

        }
        private async void GetLabReqsKeyWords(object obj)
        {
            // OnPropertyChanged(nameof(SearchStartDate));
            //   await Shell.Current.GoToAsync($"{nameof(LabReqsListViewPage)}?SearchStartDate={SearchStartDate.ToString().Replace("/","-")}&SearchEndDate={SearchEndDate.ToString().Replace("/", "-")}");
            Const.SearchByDateofServiceScanDate = Const.SearchByScanDate;
            if (SearchDateofService)
                Const.SearchByDateofServiceScanDate = Const.SearchByLogDate;
            Services.Const.SearchStartDate = SearchStartDate;
            Services.Const.SearchEndDate = SearchEndDate;
            Const.SearchStr = SearchStr;
            //  Const.SearchStr = "158410874";
            Const.SearchPart = SearchPartial;
            Const.StoreProcuder = Const.KeyWordsSP;
            //  await Shell.Current.GoToAsync($"{new LabReqsListViewPage()}?SearchStartDate={SearchStartDate.ToString().Replace("/", "-")}");
            //     await Shell.Current.GoToAsync($"{new LabReqsListViewPage()}");
            await Shell.Current.GoToAsync(nameof(LabReqsListViewPage));
        }
    }
}
