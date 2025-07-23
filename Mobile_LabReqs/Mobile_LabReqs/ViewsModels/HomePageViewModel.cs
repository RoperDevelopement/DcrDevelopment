using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Mobile_LabReqs.ViewsModels
{
   public class HomePageViewModel:BaseViewModel
    {
        public ICommand OpenWebCommand { get; }
        public HomePageViewModel()
        {
            Title = "Home Page";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://edocsusa.com"));
        }
    }
}
