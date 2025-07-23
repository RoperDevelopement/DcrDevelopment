using ScanBarCodes.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace ScanBarCodes.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}