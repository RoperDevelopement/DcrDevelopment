using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Mobile_LabReqs.Views
{
    public class WebPage : ContentPage
    {
        public WebPage()
        {
            var browser = new WebView();
          browser.Source = "https://edocsnyplabreqs.azurewebsites.net/";
        //    browser.Source = "https://edocsnypbinmonitor.azurewebsites.net/";
          //  browser.Source = "https://edocsusa.com";
            Content = browser;
           
        }
    }
}