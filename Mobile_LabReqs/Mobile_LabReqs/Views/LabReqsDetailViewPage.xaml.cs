using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mobile_LabReqs.ViewsModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
namespace Mobile_LabReqs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
   // [QueryProperty(nameof(JsonStr), "JsonStr")]
    public partial class LabReqsDetailViewPage : ContentPage
    {
        string itemId = string.Empty;
        public LabReqsDetailViewPage()
        {
            
            InitializeComponent();
            
         //   BindingContext = new LabReqsDetailPageModel(); ;

            

        }
        
        //public string JsonStr
        //{
        //    get => itemId;

        //    set
        //    {
        //        itemId =  Uri.UnescapeDataString(value ?? string.Empty);
        //        OnPropertyChanged();


        //    }
        //}
    }
}