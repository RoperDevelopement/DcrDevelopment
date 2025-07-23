using Flowapp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Flowapp.Views
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