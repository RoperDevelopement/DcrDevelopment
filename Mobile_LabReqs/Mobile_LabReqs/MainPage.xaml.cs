using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Mobile_LabReqs.Views;
using Mobile_LabReqs.Services;
namespace Mobile_LabReqs
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            
InitializeComponent();
           //   Services.RestApi.Test("https://edocsnypwebapi.azurewebsites.net/api/Values").ConfigureAwait(false).GetAwaiter().GetResult();
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(LabReqsCSNIndexNumPage), typeof(LabReqsCSNIndexNumPage));
            Routing.RegisterRoute(nameof(LabReqsPatientIDMRNPage), typeof(LabReqsPatientIDMRNPage));
            Routing.RegisterRoute(nameof(LabReqsKeyWordsPage), typeof(LabReqsKeyWordsPage));
         Routing.RegisterRoute(nameof(LabReqsDetailViewPage), typeof(LabReqsDetailViewPage));
            Routing.RegisterRoute(nameof(LabReqsListViewPage), typeof(LabReqsListViewPage));
            Routing.RegisterRoute(nameof(WebPage), typeof(WebPage));
            Routing.RegisterRoute(nameof(MediaPage), typeof(MediaPage));
            Routing.RegisterRoute(nameof(AddImagesView), typeof(AddImagesView));
            




        }
    }
}

