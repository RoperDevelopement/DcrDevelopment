using Mobile_LabReqs.Services;
using Mobile_LabReqs.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile_LabReqs.ViewsModels
{
   public class LabReqsPatientIDMRNModel:BaseViewModel
    {
        public ICommand SearchCommand { get; }
        public LabReqsPatientIDMRNModel()
        {
            Title = "Search By Index/CSN Number";
            SearchCommand = new Command(GetLabReqsMRNPatientID);
        }
        private async void GetLabReqsMRNPatientID(object obj)
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
            Const.StoreProcuder = Const.MRNPatientIdSP;
            //  await Shell.Current.GoToAsync($"{new LabReqsListViewPage()}?SearchStartDate={SearchStartDate.ToString().Replace("/", "-")}");
            //     await Shell.Current.GoToAsync($"{new LabReqsListViewPage()}");
            await Shell.Current.GoToAsync(nameof(LabReqsListViewPage));
        }
    }
}
