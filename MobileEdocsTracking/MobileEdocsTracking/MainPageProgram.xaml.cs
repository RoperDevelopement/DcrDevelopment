using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileEdocsTracking.Views;

namespace MobileEdocsTracking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageProgram : ContentPage
    {
        public MainPageProgram()
        {
            InitializeComponent();
            Navigation.PushModalAsync(new LoginPage());
          //  btnLookUpInventory.Clicked += (s, e) => Navigation.PushAsync(new LookUpInventoryPage());

        }
    }
}
