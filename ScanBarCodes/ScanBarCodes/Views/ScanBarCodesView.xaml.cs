using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;
using static Xamarin.Essentials.AppleSignInAuthenticator;

namespace ScanBarCodes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanBarCodesView : ContentPage
    {
        public ScanBarCodesView()
        {
            InitializeComponent();
        }

        private void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            // setup options https://devblogs.microsoft.com/xamarin/barcode-scanning-made-easy-with-zxing-net-for-xamarin-forms/ https://www.lindseybroos.be/2020/02/scanning-qr-codes-with-xamarin-forms/
            //    var options = new MobileBarcodeScanningOptions
            // {
            //     AutoRotate = false,
            //     UseFrontCameraIfAvailable = true,
            //     TryHarder = true,

            //};
            //            var scanPage = new ZXingScannerPage();
            //            // Navigate to our scanner page
            //            // await Navigation.PushAsync(scanPage);
            //            //add options and customize page
            //            scanPage = new ZXingScannerPage(options)
            //            {
            //                DefaultOverlayTopText = "Align the barcode within the frame",
            //                DefaultOverlayBottomText = string.Empty,
            //                DefaultOverlayShowFlashButton = true
            //            };

            //   scanResultsText.Text = string.Empty;
            
          Device.BeginInvokeOnMainThread(() =>
            { 
                scanResultsText.Text = $"{result.Text} type: {result.BarcodeFormat.ToString()}";
               
            });
        }
    }
}