using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Mobile_LabReqs.Views;
using Mobile_LabReqs.Services;
namespace Mobile_LabReqs.ViewsModels
{
   public class LabReqCSNIndexNumberModel:BaseViewModel
    {
        public Command SearchCommand { get; }
        public LabReqCSNIndexNumberModel()
        {
            Title = "Search By Index/CSN Number";
            SearchStr = string.Empty;
            SearchCommand = new Command(GetLabReqsIndexCSN);
             
    }

      

        private async void GetLabReqsIndexCSN(object obj)
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
            Const.StoreProcuder = Const.LabReqsSP;
            //  await Shell.Current.GoToAsync($"{new LabReqsListViewPage()}?SearchStartDate={SearchStartDate.ToString().Replace("/", "-")}");
            //     await Shell.Current.GoToAsync($"{new LabReqsListViewPage()}");
            await Shell.Current.GoToAsync(nameof(LabReqsListViewPage));
        }
     
    }
}
