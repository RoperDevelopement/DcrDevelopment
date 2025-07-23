//1 inch = 25.4 mm
using EdocsUSA.Utilities.Micosoft.WIA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using WIA;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using EdocsUSA.Controls;
using static EdocsUSA.Controls.ImageViewer;

namespace EdocsUSA.Utilities.EdocsWia
{
    public partial class WIA_Settings_Dialog : Form
    {
        public byte[] WIACustomaData
        { get; set; }
        private WIAScannerSettingsModelDeivce WIAScannerSettings
        { get; set; }
        private Device ScannerDevice
        { get; set; }
        private IItem ScannerProp
        { get; set; }
        private bool SettingsSaved
        { get; set; }
        private bool HasFeeder
        { get; set; }
        private bool LoadSettingsFile
        { get; set; }
        int MaxDPI
        { get; set; }
        int MinDPI
        { get; set; }
        int HorBedSize
        { get; set; }
        int VerBedSize
        { get; set; }
        IList<string> ListPaperSizes
        { get; set; }
        int X
        { get; set; }
        int Y
        { get; set; }
        public string ActiveDataSource
        { get; set; }
        public WIA_Settings_Dialog()
        {
            InitializeComponent();
            //  string s = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.BytesToStr(WIACustomaData).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void WIA_Settings_Dialog_Load(object sender, EventArgs e)
        {
            LoadSettingsFile = false;
            try
            { 
            // if(WIACustomaData != null)
            //string s = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.BytesToStr(WIACustomaData).ConfigureAwait(false).GetAwaiter().GetResult();
            //System.IO.File.WriteAllText(@"L:\AutoScan\twainsetting.txt", s);
            GetScanner().ConfigureAwait(false).GetAwaiter().GetResult();
            if (ScannerDevice != null)
            {
                MinDPI = 0;
                ScannerProp = ScannerDevice.Items[1];
                GetMaxDpiBedSize(ScannerDevice);
                LoadPaperSizes().ConfigureAwait(false).GetAwaiter().GetResult();

                //   ImageBox img = new ImageBox();
                // set your image to imagebox

                GetScannerSettings().ConfigureAwait(false).GetAwaiter().GetResult();


                //imageViewer1.Dock = DockStyle.Fill;
                //  imageViewer1.Image = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.ResizeImage(imageViewer1.Image,new Size(panel1.Width, panel1.Height));
                //  imageViewer1.ScaleToFit();
                SettingsSaved = false;
                this.Text = $"Editing WIA Scanner Setting for Scanner {WIAScannerSettings.ScannerName}";
                button6.Visible = false;
                button4.Visible = false;
                //hScrollBar1.Width = pbImage.Width;
                //hScrollBar1.Left = pbImage.Left;
                //hScrollBar1.Top = pbImage.Bottom;
                //hScrollBar1.Maximum = pbImage.Image.Width - pbImage.Width;
                //vScrollBar1.Height = pbImage.Height;
                //vScrollBar1.Left = pbImage.Left + pbImage.Width;
                //vScrollBar1.Top = pbImage.Top;
                //vScrollBar1.Maximum = pbImage.Image.Height - pbImage.Height;
                //  WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SaveDeviceStrProperty(ScannerDevice, $@"L:\AutoScan\WIAPROP\{WIAScannerSettings.ScannerName}_deviceProp.txt");
                // WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SaveStrProperty(ScannerProp, $@"L:\AutoScan\WIAPROP\{WIAScannerSettings.ScannerName}_itemprop.txt");

            }
            else
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error Setting Up Scanner {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
           }
        }
        async Task LoadPaperSizes()
        {

            ListPaperSizes = new List<string>();
            //   GetMaxDpiBedSize(ScannerDevice);
            if (HasFeeder)
            {
                int dpi = 150;
                GetWidthHight("A4 Letter 210 x 297 mm(8.5 x 11 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("A4 Extra 235 x 322 mm(9.3 x 12.7 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("A4 Rotated 297 x 210 mm(11.7 x 8.3 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("A4 210 x 297 mm(8.3 x 11.7 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                //  GetWidthHight("A3 297 x 420 mm(11.7 x 16.5 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("A3 297 x 420 mm(11.7 x 16.5 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();

                GetWidthHight("A5 148 x 210 mm(9.84 x 8.27)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("A6 105 x 148 mm(4.13 x 5.83)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("A7 74 x 105 mm(2.91 x 4.13 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("A8 52 x 74 mm(2.05 x 2.91 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();

                GetWidthHight("B4 250 x 353 mm(9.84 x 13.90)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();


                GetWidthHight("B5 176 x 250 mm(6.93 x 9.84)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("B6 125 x 1763 mm(4.13 x 5.83)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("B7 88 x 12 mm(3.46 x 4.92)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("B8 62 x 88 mm(2.44 x 3.46)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("B9 44 x 62 mm(1.73 x 2.44)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("B10 31 x 44 mm(1.22 x 1.73)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("Envelope C4 229 x 324 mm(9 x 12.8 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("C5 229 x 162 mm(9.02 x 6.38 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("C6 162 x 114 mm(6.38 x 4.49 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("10 x 14 254 x 356 mm(10 x 14)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();

                GetWidthHight("Legal 216 x 356 mm(8.5 x 14 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("LegalExtra 241 x 381 mm(9.5 x 15 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("LetterExtra 241 x 305 mm(9.5 x 12 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("Ledger 279 x 431 mm(11 x 17 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("Letter 216 x 279 mm(8.5 x 11 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("Letter Plus 216 x 332 mm(8.2 x 12.7 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("Letter Rotated 297 x 216 mm(11 x 8.5 inch)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("Envelope B5 176 x 250 mm(6.9 x 9.8)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("Envelope #12 127 x 279 mm(4.8 x 11)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("Envelope #14 127 x 292 mm(14.5 x 11.5)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("Envelope C65 114 x 299 mm(4.5 x 9)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
                GetWidthHight("Envelope #110 110 x 230 mm(4.3 x 9.1)", dpi).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else
            {
                ListPaperSizes.Add("A4 Letter 210 x 297 mm(8.5 x 11 inch)");
                ListPaperSizes.Add("A5 148 x 210 mm(5.83 x 8.27)");
                ListPaperSizes.Add("A6 105 x 148  mm(4 x 6)");

            }

            if (ListPaperSizes.Count > 0)
            {
                ListPaperSizes.Add("Custom");
                cBoxPaperSize.BeginUpdate();
                cBoxPaperSize.Items.Clear();
                cBoxPaperSize.DataSource = ListPaperSizes;
                cBoxPaperSize.SelectedIndex = 0;
                cBoxPaperSize.EndUpdate();


            }
            else
                cBoxPaperSize.Enabled = false;


        }
        async Task GetWidthHight(string sizePaper, int dpi)
        {
            int indexPar = sizePaper.IndexOf('(');
            if (indexPar > 0)
            {

                double width = 0.0;
                double height = 0.0;
                int w = 0; ;
                int h = 0;
                string tempStr = sizePaper.Substring(++indexPar).Trim();
                tempStr = tempStr.Substring(0, tempStr.Length - 1);
                string[] values = tempStr.Split(' ');
                width = double.Parse(values[0]);
                height = 0;
                for (int i = 1; i <= values.Length; i++)
                    if (double.TryParse(values[i], out double results))
                    {
                        height = results;
                        break;
                    }
                w = (int)width * dpi;
                h = (int)height * dpi;
                try
                {
                    WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(ScannerProp.Properties, WIAConsts.WIA_HORIZONTAL_SCAN_SIZE_PIXELS, w);

                    WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(ScannerProp.Properties, WIAConsts.WIA_VERTICAL_SCAN_SIZE_PIXELS, h);
                }
                catch (Exception ex)
                {
                    return;
                }
                ListPaperSizes.Add(sizePaper);
            }
        }
        private async void GetMaxDpiBedSize(Device device)
        {
            //  Device scanDevice = device;
            string size = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetDeviceStrProperty(ref device, WIAConsts.HORDPI);
            MaxDPI = int.Parse(size);
            size = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetDeviceStrProperty(ref device, WIAConsts.VERDPI);
            MinDPI = int.Parse(size);
            size = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetDeviceStrProperty(ref device, WIAConsts.HORBEDSIZR);
            HorBedSize = int.Parse(size);
            size = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetDeviceStrProperty(ref device, WIAConsts.VERBEDSIZE);
            VerBedSize = int.Parse(size);
            numCustomDPI.Maximum = MaxDPI;
            HasFeeder = false;
            int hasFeeder = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetDeviceIntProperty(ref device, WIAConsts.DEVICE_PROPERTY_DOCUMENT_HANDLING_CAPABILITIES_ID);
            if ((Convert.ToUInt32(hasFeeder) & WIAConsts.FEEDER) != 0)
                HasFeeder = true;

        }
        private async Task GetScannerSettings()
        {
            if (string.IsNullOrEmpty(WIAScannerSettings.ScannerName))
            {
                GetDefaultScannerSettings().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else
                SetDialogValues();


        }
        private async void SetDialogValues()
        {
            LoadSettingsFile = true;


            txtBoxLeft.Text = WIAScannerSettings.ScannerStartPropProp.HorizontalStartPosition.ToString();
            txtBoxTop.Text = WIAScannerSettings.ScannerStartPropProp.VerticalStartPosition.ToString();
            TBarBrightness.Value = WIAScannerSettings.ScannerBCT.Brightness;
            tBarContrast.Value = WIAScannerSettings.ScannerBCT.Contrast;
            txtBoxBrightness.Text = TBarBrightness.Value.ToString();
            tBoxContrast.Text = tBarContrast.Value.ToString();
            // WIAScannerSettings.ScannerPageSize.ScannerType
            txtBoxTop.Text = WIAScannerSettings.ScannerStartPropProp.VerticalStartPosition.ToString();
            txtBoxLeft.Text = WIAScannerSettings.ScannerStartPropProp.HorizontalStartPosition.ToString();
            txtBoxWidth.Text = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAIntProperty(ScannerProp.Properties, WIAConsts.WIA_HORIZONTAL_SCAN_SIZE_PIXELS).ToString();
            txtBoxLength.Text = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAIntProperty(ScannerProp.Properties, WIAConsts.WIA_VERTICAL_SCAN_SIZE_PIXELS).ToString();
            //   GetMaxDpiBedSize(deivce);
            if ((MaxDPI > 600) && (HasFeeder))
                cBoxDPI.Items.Insert(0, MaxDPI);
            if (HasFeeder)
            {


                cBoxScannerType.BeginUpdate();
                cBoxScannerType.Items.Clear();
                cBoxScannerType.Items.Add(WIAConsts.SINGLESCANNING);
                cBoxScannerType.Items.Add(WIAConsts.DUPLEXSCANNING);
                cBoxScannerType.Text = WIAScannerSettings.ScannerTypeDevice.ScannerTypeValue;
                cBoxScannerType.Enabled = true;
                cBoxScannerType.EndUpdate();


            }
            if (!(HasFeeder))
            {

                //   txtBoxWidth.Enabled = false;
                //   txtBoxLength.Enabled = false;
                //   cBoxPaperSize.BeginUpdate();
                //cBoxPaperSize.Items.Clear();
                //cBoxPaperSize.Enabled = false;
                //cBoxPaperSize.EndUpdate();
                cBoxScannerType.BeginUpdate();
                cBoxScannerType.Items.Clear();
                cBoxScannerType.Items.Add("FlatBed Scanner");

                cBoxScannerType.Text = "FlatBed Scanner";
                cBoxScannerType.Enabled = false;
                cBoxScannerType.EndUpdate();

            }
            cBoxDPI.Text = WIAScannerSettings.ScannerDPIProp.HorizontalDPI.ToString();
            cBoxColor.SelectedIndex = WIAScannerSettings.ScannerColorProp.ScannerColor;
            GetHeightWidth();
            GetPaperSizes(WIAScannerSettings.ScannerPageSize.PaperType).ConfigureAwait(false).GetAwaiter().GetResult();
            // string
        }
        //private async void GetADFPaperSixes()
        //{
        //    cBoxPaperSize.BeginUpdate();
        //    cBoxPaperSize.Items.Clear();
        //    cBoxPaperSize.DataSource = ListPaperSizes;
        //    cBoxPaperSize.EndUpdate();











        //}
        //private async void GetFlatBedPaperSixes()
        //{
        //    cBoxPaperSize.BeginUpdate();
        //    cBoxPaperSize.Items.Clear();
        //    //  cBoxPaperSize.Items.Add("A3 297 x 420 mm(11.7 x 16.5 inch)");
        //    cBoxPaperSize.Items.Add("A4 210 x 297 mm(8.5 x 11 inch)");
        //    cBoxPaperSize.Items.Add("A6 105 x 148  mm(4.0 x 6.0 inch)");
        //    cBoxPaperSize.Items.Add("A5 148 x 210 mm(5.83 x 8.27)");
        //    cBoxPaperSize.Enabled = false;
        //    cBoxPaperSize.EndUpdate();











        //}
        private async Task GetDefaultScannerSettings()
        {

            Device deivce = ScannerDevice;
          
            
            WIAScannerSettings.ScannerName = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetDeviceStrProperty(ref deivce, WIAConsts.WIA_SCANNERNAME);
            WIAScannerSettings.ScannerPropID = WIAConsts.WIA_SCANNERNAME;
            // int hasFeeder = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetDeviceIntProperty(ref deivce, WIAConsts.DEVICE_PROPERTY_DOCUMENT_HANDLING_CAPABILITIES_ID);
           // ScannerProp.Properties["3097"].set_Value(0);

            txtBoxLeft.Text = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAIntProperty(ScannerProp.Properties, WIAConsts.WIA_HORIZONTAL_SCAN_START_PIXEL).ToString();
            txtBoxTop.Text = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAIntProperty(ScannerProp.Properties, WIAConsts.WIA_VERTICAL_SCAN_START_PIXEL).ToString();
            txtBoxWidth.Text = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAIntProperty(ScannerProp.Properties, WIAConsts.WIA_HORIZONTAL_SCAN_SIZE_PIXELS).ToString();
            txtBoxLength.Text = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAIntProperty(ScannerProp.Properties, WIAConsts.WIA_VERTICAL_SCAN_SIZE_PIXELS).ToString();
            // int maxVer = ScannerProp.Properties[WIAConsts.WIA_IPS_MAX_HORIZONTAL_SIZE_STR].get_Value();
            // object 0=
            //  string k = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetDeviceStrProperty(ref deivce, WIAConsts.VERDPI);
            //  string horBedSize = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAProperty(ScannerProp.Properties,(object)WIAConsts.HORDPI);

            //int maxHer = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAIntProperty(ScannerProp.Properties, WIAConsts.WIA_IPS_MAX_HORIZONTAL_SIZE);
            numCustomDPI.Value = 300;
            txtBoxLeft.Text = "0";
            txtBoxTop.Text = "0";


            //WIAScannerSettings.ScannerDPIDevice.HorizontalDPIValue = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAProperty(ScannerProp.Properties, WIAConsts.WIA_HORIZONTAL_SCAN_RESOLUTION_DPI);
            //WIAScannerSettings.ScannerDPIDevice.HorizontalDPIID = WIAConsts.WIA_HORIZONTAL_SCAN_RESOLUTION_DPI;
            //WIAScannerSettings.ScannerDPIDevice.VerticalDPIValue = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAProperty(ScannerProp.Properties, WIAConsts.WIA_VERTICAL_SCAN_RESOLUTION_DPI);
            //WIAScannerSettings.ScannerDPIDevice.VerticalDPIPropID = WIAConsts.WIA_VERTICAL_SCAN_RESOLUTION_DPI;
            //  GetMaxDpiBedSize(deivce);
            cBoxDPI.BeginUpdate();
            //if (int.Parse(WIAScannerSettings.ScannerDPIDevice.HorizontalDPIValue) > 600)
            //{
            //    cBoxDPI.Text = "Custom";
            //    numCustomDPI.Enabled = false;
            //}
            //else
            //   cBoxDPI.SelectedText = "300";
            if ((MaxDPI > 600) && (HasFeeder))
                cBoxDPI.Items.Insert(0, MaxDPI);
            if (HasFeeder)
                cBoxDPI.Items.Insert(cBoxDPI.Items.Count - 1, 150);
            //cBoxDPI.SelectedIndex = 0;
            cBoxDPI.Text = "300";
            cBoxDPI.EndUpdate();


            int bright = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAIntProperty(ScannerProp.Properties, WIAConsts.WIA_SCAN_BRIGHTNESS_PERCENTS);
            int cont = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAIntProperty(ScannerProp.Properties, WIAConsts.WIA_SCAN_CONTRAST_PERCENTS);
            //WIAScannerSettings.ScannerDPIDevice.VerticalDPIPropID = int.Parse(WIAConsts.WIA_VERTICAL_SCAN_RESOLUTION_DPI);
            //WIAScannerSettings.ScannerBCT.Brightness
            TBarBrightness.Value = bright;
            tBarContrast.Value = cont;
            cBoxColor.SelectedIndex = 1;

            txtBoxBrightness.Text = bright.ToString();
            tBoxContrast.Text = cont.ToString();
            WIAScannerSettings.ScannerBCT.Brightness = bright;
            WIAScannerSettings.ScannerBCT.BrightnessID = int.Parse(WIAConsts.WIA_SCAN_BRIGHTNESS_PERCENTS);
            WIAScannerSettings.ScannerBCT.Contrast = cont;
            WIAScannerSettings.ScannerBCT.ContrastID = int.Parse(WIAConsts.WIA_SCAN_CONTRAST_PERCENTS);
            //  GetPaperSizes().ConfigureAwait(false).GetAwaiter().ToString();
            numCustomDPI.Value = int.Parse(cBoxDPI.Text);

            if (HasFeeder)
            {
                // GetADFPaperSixes();
                //HasFeeder = true;
                cBoxScannerType.BeginUpdate();
                cBoxScannerType.Items.Clear();
                cBoxScannerType.Items.Add(WIAConsts.SINGLESCANNING);
                cBoxScannerType.Items.Add(WIAConsts.DUPLEXSCANNING);
                cBoxScannerType.SelectedIndex = 1;
                cBoxScannerType.EndUpdate();
                WIAScannerSettings.DocumentHandlingDevice.DocumentHandlingSelectValue = WIAConsts.DUPLEXSCANNING;
                WIAScannerSettings.DocumentHandlingDevice.DocumentHandlingSelectPropID = WIAConsts.DEVICE_PROPERTY_DOCUMENT_HANDLING_CAPABILITIES_ID;
                WIAScannerSettings.ScannerPageSize.ScannerType = PaperSize.A4.ToString();
                WIAScannerSettings.ScannerPageSize.PageWidth = WIAConsts.A4_PAPER_SIZE_MMWIDTH;
                WIAScannerSettings.ScannerPageSize.PageHeight = WIAConsts.A4_PAPER_SIZE_MMHEIGHT;
                WIAScannerSettings.ScannerPageSize.PageWidthID = int.Parse(WIAConsts.WIA_HORIZONTAL_SCAN_SIZE_PIXELS);
                WIAScannerSettings.ScannerPageSize.PageHeightID = int.Parse(WIAConsts.WIA_VERTICAL_SCAN_SIZE_PIXELS);
                //cBoxPaperSize.SelectedIndex = 1;
                //   GetPaperSizes("A3 297 x 420 mm(11.7 x 16.5 inch)").ConfigureAwait(false).GetAwaiter().GetResult();
                //  ScannerPageSize size = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.PaperSizeInches(PaperSize.A4);
                //  txtBoxWidth.Text = size.PageWidth.ToString();
                //  txtBoxLength.Text = size.PageHeight.ToString();
                //   GetPaperSizes(WIAScannerSettings.ScannerPageSize.ScannerType);




            }
            else
            {
                // txtBoxWidth.Enabled = false;
                // txtBoxLength.Enabled = false;
                // cBoxPaperSize.Enabled = false;
                //GetFlatBedPaperSixes();
                //   cBoxPaperSize.Text = "A4 210 x 297 mm(8.5 x 11 inch)";
                cBoxScannerType.BeginUpdate();
                cBoxScannerType.Items.Clear();
                cBoxScannerType.Items.Add(WIAConsts.FLATBEDSCANNER);
                cBoxScannerType.SelectedIndex = 0;
                cBoxScannerType.Enabled = false;
                cBoxScannerType.EndUpdate();
                WIAScannerSettings.DocumentHandlingDevice.DocumentHandlingSelectValue = "Single Scanning";
                WIAScannerSettings.DocumentHandlingDevice.DocumentHandlingSelectPropID = WIAConsts.DEVICE_PROPERTY_DOCUMENT_HANDLING_CAPABILITIES_ID;

            }

            GetHeightWidth();

        }
        private async Task GetPaperSizes(string paperType)
        {
            cBoxPaperSize.BeginUpdate();
            int iCount = 0;
            foreach (var item in cBoxPaperSize.Items)
            {

                if (item.ToString().ToLower().StartsWith(paperType.ToLower()))
                {
                    cBoxPaperSize.SelectedIndex = iCount;
                    break;
                }
                ++iCount;
            }
            //   cBoxPaperSize.Items.Clear();
            // IItem item = ScannerProp;
            //  string s =    WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetStrProperty(ref item, 6151);
            // string s = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetStrProperty(ref item, int.Parse(WIAConsts.WIA_HORIZONTAL_SCAN_SIZE_PIXELS));


            cBoxPaperSize.EndUpdate();
        }
        private async Task GetScanner()
        {

            WIAScannerSettings = new WIAScannerSettingsModelDeivce();
            try
            {
                ScannerDevice = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.FindDevice(ActiveDataSource).ConfigureAwait(false).GetAwaiter().GetResult();

                if (WIACustomaData != null)
                {
                    try
                    {


                        string scannerSetting = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.BytesToStr(WIACustomaData).ConfigureAwait(false).GetAwaiter().GetResult();
                        WIAScannerSettings = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.DeserializeObject<WIAScannerSettingsModelDeivce>(scannerSetting).ConfigureAwait(false).GetAwaiter().GetResult();
                        //ScannerDevice = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.ConnectToWimDecive(WIAScannerSettings.ScannerName).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (ScannerDevice == null)
                        {
                            
                            if (!(string.IsNullOrEmpty(WIAScannerSettings.ScannerName)))
                            {

                                ScannerDevice = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.ConnectToWimDecive(WIAScannerSettings.ScannerName).ConfigureAwait(false).GetAwaiter().GetResult();
                            }
                        }
                    }
                    catch { }

                }
                if (ScannerDevice == null)
                    ScannerDevice = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SelectDevice(0).ConfigureAwait(false).GetAwaiter().GetResult();

                if (ScannerDevice == null)
                {
                    MessageBox.Show("Scanner error");
                    SettingsSaved = true;
                    WIACustomaData = null;
                    button3.PerformClick();
                }

            }
            catch (Exception ex)
            {
                this.Close();
            }
        }

        //private WIAScannerSettingsModelDeivce InitProp()
        //{
        //    WIAScannerSettingsModelDeivce wIAScannerSettingsModelDeivce = new WIAScannerSettingsModelDeivce();
        //    wIAScannerSettingsModelDeivce.ScannerTypeDevice = new ScannerTypeDevice();
        //    wIAScannerSettingsModelDeivce.DocumentHandlingDevice = new DocumentHandlingDevice();
        //    wIAScannerSettingsModelDeivce.ScannerBCT = new ScannerBCT();
        //    wIAScannerSettingsModelDeivce.ScannerBedSize = new ScannerBedSizeDevice();
        //    wIAScannerSettingsModelDeivce.ScannerColorProp = new ScannerColorProp();
        //    wIAScannerSettingsModelDeivce.ScannerDPIDevice = new ScannerDPIDevice();
        //    wIAScannerSettingsModelDeivce.ScannerDPIProp = new ScannerDPIProp();
        //    wIAScannerSettingsModelDeivce.ScannerStartPropProp = new ScannerStartPropProp();
        //    wIAScannerSettingsModelDeivce.ScannerTimeOutDevice = new ScannerTimeOutDevice();
        //    wIAScannerSettingsModelDeivce.ScannerTypeDevice = new ScannerTypeDevice();
        //    return wIAScannerSettingsModelDeivce;
        //}
        private void button1_Click(object sender, EventArgs e)
        {
            WIAScannerSettingsModelDeivce wIAScannerSettingsModelDeivce = new WIAScannerSettingsModelDeivce();
            wIAScannerSettingsModelDeivce.ScannerName = "lll";
            wIAScannerSettingsModelDeivce.ScannerPropID = 10;



            // pairs["ScannerName"] = wIAScannerSettingsModelDeivce;
            string s = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SerialzeClass(wIAScannerSettingsModelDeivce).ConfigureAwait(false).GetAwaiter().GetResult();
            WIAScannerSettingsModelDeivce settingsModelDeivce = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.DeserializeObject<WIAScannerSettingsModelDeivce>(s).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private void TBarBrightness_Scroll(object sender, EventArgs e)
        {
            txtBoxBrightness.Text = TBarBrightness.Value.ToString();
            // pbImage.Image= AdjustBrightness(pbImage.Image, TBarBrightness.Value);
        }

        private void cBoxDPI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBoxDPI.Text.ToLower().StartsWith("custom"))
                numCustomDPI.Enabled = true;
            else
            {
                numCustomDPI.Enabled = false;
                if (!(string.IsNullOrEmpty(cBoxDPI.Text)))
                    numCustomDPI.Value = int.Parse(cBoxDPI.Text.Trim());
                GetHeightWidth();
            }

        }

        private void tBarContrast_Scroll(object sender, EventArgs e)
        {
            tBoxContrast.Text = tBarContrast.Value.ToString();
        }

        private async void GetHeightWidth()
        {

            //  if (HasFeeder)
            // {

            if (!(string.IsNullOrWhiteSpace(cBoxDPI.Text)))
            {
                int dpi = 0;
                double width = 0.0;
                double height = 0.0;
                int w = 0; ;
                int h = 0;
                if (cBoxDPI.Text.ToLower().StartsWith("custom"))
                    dpi = (int)numCustomDPI.Value;
                else
                {
                    dpi = int.Parse(cBoxDPI.Text); ;
                }

                //  if (string.Compare(cBoxPaperSize.Text, "Custom", true) == 0)
                //  {
                //      width = double.Parse(txtBoxWidth.Text);
                //       height = double.Parse(txtBoxLength.Text);
                //       w = (int)width * dpi;
                //      h = (int)height * dpi;
                //      txtBoxWidth.Text = w.ToString();
                //      txtBoxLength.Text = h.ToString();
                //  }
                //else
                //  { 
                int indexPar = cBoxPaperSize.Text.IndexOf('(');
                if (indexPar > 0)
                {
                    string tempStr = cBoxPaperSize.Text.Substring(++indexPar).Trim();
                    tempStr = tempStr.Substring(0, tempStr.Length - 1);
                    string[] values = tempStr.Split(' ');
                    width = double.Parse(values[0]);
                    height = 0;
                    for (int i = 1; i <= values.Length; i++)
                        if (double.TryParse(values[i], out double results))
                        {
                            height = results;
                            break;
                        }
                    w = (int)(width * dpi);
                    h = (int)(height * dpi);
                    txtBoxWidth.Text = w.ToString();
                    txtBoxLength.Text = h.ToString();

                }
            }
            //   }

        }

        private void cBoxPaperSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indexBlank = cBoxPaperSize.Text.IndexOf(" ");
            if (indexBlank > 0)
            {
                GetHeightWidth();
                // string pType = cBoxPaperSize.Text.Substring(0, indexBlank).Trim();
                //   PaperSize paper = (PaperSize)Enum.Parse(typeof(PaperSize), pType);
                //  ScannerPageSize size = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.PaperSizeInches(paper);
                //  txtBoxWidth.Text = size.PageWidth.ToString();
                //  txtBoxLength.Text = size.PageHeight.ToString();

            }
        }



        private bool SetScnnerSettings()
        {
            System.Windows.Forms.Control control = TBarBrightness;
            try
            {

                //object o = 619;
                //WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(ScannerProp.Properties, o, 2500);
                //o = 876;
                //   WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(ScannerProp.Properties, o, 3300);
                int width = int.Parse(txtBoxWidth.Text);
                int height = int.Parse(txtBoxLength.Text);

                control = TBarBrightness;
                WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(ScannerProp.Properties, WIAConsts.WIA_SCAN_BRIGHTNESS_PERCENTS, TBarBrightness.Value);
                control = tBarContrast;
                WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(ScannerProp.Properties, WIAConsts.WIA_SCAN_CONTRAST_PERCENTS, tBarContrast.Value);
                control = cBoxDPI;
                WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(ScannerProp.Properties, WIAConsts.WIA_HORIZONTAL_SCAN_RESOLUTION_DPI, cBoxDPI.Text);

                WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(ScannerProp.Properties, WIAConsts.WIA_VERTICAL_SCAN_RESOLUTION_DPI, cBoxDPI.Text);
                control = cBoxColor;
                if (cBoxColor.SelectedIndex == 0)
                    WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(ScannerProp.Properties, WIAConsts.WIA_SCAN_COLOR_MODE, 2);

                else
                {

                    WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(ScannerProp.Properties, WIAConsts.WIA_SCAN_COLOR_MODE, cBoxColor.SelectedIndex);
                }

                control = txtBoxLeft;
                WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(ScannerProp.Properties, WIAConsts.WIA_HORIZONTAL_SCAN_START_PIXEL, txtBoxLeft.Text);
                control = txtBoxTop;
                WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(ScannerProp.Properties, WIAConsts.WIA_VERTICAL_SCAN_START_PIXEL, txtBoxTop.Text);
                //if(string.Compare(cBoxPaperSize.Text, "Entire Area",true)== 0)
                //{
                //    width = HorBedSize;
                //    height = VerBedSize;
                //    txtBoxWidth.Text = width.ToString();
                //    txtBoxLength.Text = height.ToString();

                //}
                //else
                //{ 
                //  if ((string.Compare(cBoxScannerType.Text, WIAConsts.FLATBEDSCANNER, true) != 0))
                // {
                if (width > HorBedSize)
                {

                    width = HorBedSize;
                    //errorProvider.SetError(txtBoxWidth, $"Setting Width TO Max Horizal Bed Size {HorBedSize}");
                    //errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                    txtBoxWidth.Text = width.ToString();
                    // txtBoxWidth.Focus();


                }
                if (height > VerBedSize)
                {
                    //errorProvider.SetError(txtBoxWidth, $"Setting Height to Max Vertical Bed Size {VerBedSize}");
                    //errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
                    txtBoxLength.Text = height.ToString();
                    height = VerBedSize;
                }

                control = txtBoxWidth;
                WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(ScannerProp.Properties, WIAConsts.WIA_HORIZONTAL_SCAN_SIZE_PIXELS, width);
                control = txtBoxLength;
                WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(ScannerProp.Properties, WIAConsts.WIA_VERTICAL_SCAN_SIZE_PIXELS, height);

            }
            catch (Exception ex)
            {
                errorProvider.SetError(control, ex.Message);

                MessageBox.Show($"Error Settings Settings {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                control.Focus();
                return false;
            }
            return true;
            //}
        }
        private async void d()
        {
            try
            {


                WIA.ICommonDialog wiaCommonDialog = new WIA.CommonDialog();
                Item scannnerItem = (Item)ScannerProp;
                object scanResult = wiaCommonDialog.ShowTransfer(scannnerItem, WIAConsts.wiaFormatJPEG, false);
                if (scanResult != null)
                {
                    ImageFile imageFile = (ImageFile)scanResult;
                    byte[] data = (byte[])imageFile.FileData.get_BinaryData();
                    MemoryStream stream = new MemoryStream(data);

                    // using (System.Drawing.Image image = System.Drawing.Image.FromStream(stream))
                    // {
                    //result = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

                    //  pbImage.Image = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.ResizeImage(System.Drawing.Image.FromStream(stream),new Size(300,300));
                    System.Drawing.Image image = System.Drawing.Image.FromStream(stream);


                    stream.FlushAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    stream.Close();
                    stream.Dispose();

                    //   panel1.AutoScrollMinSize = new Size(Convert.ToInt32(pbImage.Image.Width-20), Convert.ToInt32(pbImage.Image.Height-20));
                    // panel1.SetAutoScrollMargin(pbImage.Image.Width, pbImage.Image.Height);
                    // pbImage.Refresh();
                    //imageViewer1.//Image.Size = new Size(panel1.Width, panel1.Height);
                    // panel1.AutoScroll = true;
                    imageViewer1.Dock = DockStyle.Fill;
                    imageViewer1.Image = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.ResizeImage(image, new Size(panel1.Width, panel1.Height));
                    imageViewer1.ScaleToFit();



                }

            }
            catch (COMException cx)
            {
                int comErrorCode = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAErrorCode(cx);
                if (comErrorCode > 0)
                {
                    MessageBox.Show(WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetErrorCodeDescription(comErrorCode), "Error");


                }
                else
                    MessageBox.Show("UnKnown Error...", "Error");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error {ex.Message}", "Error");

            }

        }
        private Bitmap AdjustBrightness(System.Drawing.Image image, float brightness)
        {
            // Make the ColorMatrix.
            float b = brightness;
            ColorMatrix cm = new ColorMatrix(new float[][]
                {
                    new float[] {b, 0, 0, 0, 0},
                    new float[] {0, b, 0, 0, 0},
                    new float[] {0, 0, b, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1},
                });
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(cm);

            // Draw the image onto the new bitmap while applying the new ColorMatrix.
            Point[] points =
            {
                new Point(0, 0),
                new Point(image.Width, 0),
                new Point(0, image.Height),
            };
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            // Make the result bitmap.
            Bitmap bm = new Bitmap(image.Width, image.Height);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.DrawImage(image, points, rect, GraphicsUnit.Pixel, attributes);
            }

            // Return the result.
            return bm;
        }








        private void tstDropDownImageScale_Click(object sender, EventArgs e)
        {

        }

        private void txtBoxBrightness_TextChanged(object sender, EventArgs e)
        {
            if (!(string.IsNullOrWhiteSpace(txtBoxBrightness.Text)))
            {
                if (int.TryParse(txtBoxBrightness.Text, out int results))
                {
                    if (results > TBarBrightness.Maximum)
                        TBarBrightness.Value = TBarBrightness.Maximum;
                    else if (results < TBarBrightness.Minimum)
                        TBarBrightness.Value = TBarBrightness.Minimum;
                    else
                        TBarBrightness.Value = results;
                    txtBoxBrightness.Text = TBarBrightness.Value.ToString();
                }
                else
                    txtBoxBrightness.Text = TBarBrightness.Value.ToString();

            }
            else
                txtBoxBrightness.Text = TBarBrightness.Value.ToString();

        }

        private void tBoxContrast_TextChanged(object sender, EventArgs e)
        {
            if (!(string.IsNullOrWhiteSpace(tBoxContrast.Text)))
            {
                if (int.TryParse(tBoxContrast.Text, out int results))
                {
                    if (results > tBarContrast.Maximum)
                        tBarContrast.Value = TBarBrightness.Maximum;
                    else if (results < tBarContrast.Minimum)
                        tBarContrast.Value = tBarContrast.Minimum;
                    else
                        tBarContrast.Value = results;
                    tBoxContrast.Text = tBarContrast.Value.ToString();
                }
                else
                    tBoxContrast.Text = tBarContrast.Value.ToString();

            }
            else
                txtBoxBrightness.Text = tBarContrast.Value.ToString();
        }

        private async void PreViewImage()
        {
            try
            {

                WIA.ICommonDialog wiaCommonDialog = new WIA.CommonDialog();
                Item scannnerItem = (Item)ScannerProp;
                object scanResult = wiaCommonDialog.ShowTransfer(scannnerItem, WIAConsts.wiaFormatTIFF, false);
                if (scanResult != null)
                {
                    ImageFile imageFile = (ImageFile)scanResult;
                    byte[] data = (byte[])imageFile.FileData.get_BinaryData();
                    MemoryStream stream = new MemoryStream(data);

                    // using (System.Drawing.Image image = System.Drawing.Image.FromStream(stream))
                    // {
                    //result = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

                    //  pbImage.Image = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.ResizeImage(System.Drawing.Image.FromStream(stream),new Size(300,300));
                    System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                    stream.FlushAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    stream.Close();
                    stream.Dispose();
                    imageViewer1.Image = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.ResizeImage(image, new Size(panel1.Width, panel1.Height));
                    imageViewer1.ScaleToFit();
                    button6.Visible = true;
                    button4.Visible = true;
                    //rBtnZoom.Visible = false;
                    //rBtnStretchImage.Visible = false;

                    //rBtnZoom.Visible = true;
                    //rbBtnNormal.Visible = true;
                    //rbBtnAutoSize.Visible = true;
                    //rbBtnCenterImage.Visible = true;
                    //  panel1.AutoScroll = false;
                    //   panel1.AutoScrollMinSize = new Size(Convert.ToInt32(pbImage.Image.Width-20), Convert.ToInt32(pbImage.Image.Height-20));
                    // panel1.SetAutoScrollMargin(panel1.Width, panel1.Height);
                    // pbImage.Refresh();
                    //  panel1.AutoScroll = true;
                    // rBtnStretchImage.PerformClick();

                }

            }
            catch (COMException cx)
            {
                int comErrorCode = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAErrorCode(cx);
                if (comErrorCode > 0)
                {
                    MessageBox.Show(WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetErrorCodeDescription(comErrorCode), "Error");


                }
                else
                    MessageBox.Show("UnKnown Error...", "Error");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error {ex.Message}", "Error");

            }

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            errorProvider.Clear();
            try
            {
                if (SetScnnerSettings())
                    PreViewImage();
            }
            catch { }


        }

        private async void SaveSettings()
        {
            try
            {


                SetScnnerSettings();
                UpDateSettings();
                SettingsSaved = true;
            }
            catch (COMException cx)
            {
                int comErrorCode = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAErrorCode(cx);
                if (comErrorCode > 0)
                {
                    MessageBox.Show(WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetErrorCodeDescription(comErrorCode), "Error");


                }
                else
                    MessageBox.Show("UnKnown Error...", "Error");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error {ex.Message}", "Error");

            }
        }

        private async void UpDateSettings()
        {
            int width = int.Parse(txtBoxWidth.Text);
            int height = int.Parse(txtBoxLength.Text);
            if (width > HorBedSize)
            {

                width = HorBedSize;



            }
            if (height > VerBedSize)
            {
                height = VerBedSize;
            }
            WIAScannerSettings.ScannerTypeDevice.ScannerTypeValue = cBoxScannerType.Text;
            WIAScannerSettings.ScannerBCT.Brightness = TBarBrightness.Value;
            WIAScannerSettings.ScannerBCT.BrightnessID = int.Parse(WIAConsts.WIA_SCAN_BRIGHTNESS_PERCENTS);
            WIAScannerSettings.ScannerBCT.Contrast = tBarContrast.Value;
            WIAScannerSettings.ScannerBCT.ContrastID = int.Parse(WIAConsts.WIA_SCAN_CONTRAST_PERCENTS);
            WIAScannerSettings.ScannerDPIProp.HorizontalDPI = int.Parse(cBoxDPI.Text);
            WIAScannerSettings.ScannerDPIProp.HorizontalDPIID = int.Parse(WIAConsts.WIA_HORIZONTAL_SCAN_RESOLUTION_DPI);
            WIAScannerSettings.ScannerDPIProp.VerticalDPI = int.Parse(cBoxDPI.Text);
            WIAScannerSettings.ScannerDPIProp.VerticalDPIPropID = WIAConsts.WIA_VERTICAL_SCAN_RESOLUTION_DPI;
            WIAScannerSettings.ScannerColorProp.ScannerColor = cBoxColor.SelectedIndex;
            WIAScannerSettings.ScannerColorProp.ScannerColorPropID = int.Parse(WIAConsts.WIA_SCAN_COLOR_MODE);
            if (!(HasFeeder))
            {

                if (cBoxColor.SelectedIndex == 0)
                    WIAScannerSettings.ScannerColorProp.ScannerColor = 2;


            }

            WIAScannerSettings.ScannerStartPropProp.HorizontalStartPosition = int.Parse(txtBoxLeft.Text);
            WIAScannerSettings.ScannerStartPropProp.HorizontalStartPositionID = int.Parse(WIAConsts.WIA_HORIZONTAL_SCAN_START_PIXEL);
            WIAScannerSettings.ScannerStartPropProp.VerticalStartPosition = int.Parse(txtBoxTop.Text);
            WIAScannerSettings.ScannerStartPropProp.VerticalStartPositionID = int.Parse(WIAConsts.WIA_VERTICAL_SCAN_START_PIXEL);
            WIAScannerSettings.ScannerPageSize.PaperType = cBoxPaperSize.Text;
            if ((string.Compare(cBoxScannerType.Text, WIAConsts.FLATBEDSCANNER, true) != 0))
            {

                WIAScannerSettings.ScannerTypeDevice.ScannerValue = WIAConsts.WIA_PRINTER_ENDORSER_FEEDER_DUPLEX;
                WIAScannerSettings.ScannerTypeDevice.ScannerTypePropID = WIAConsts.WIA_DPS_DOCUMENT_HANDLING_SELECT;
                WIAScannerSettings.ScannerPageSize.PageType = 1;
                WIAScannerSettings.ScannerPageSize.PageTypeID = int.Parse(WIAConsts.WIA_DPS_PAGES);
                cBoxScannerType.Enabled = true;
            }
            WIAScannerSettings.ScannerPageSize.PageWidth = width;
            WIAScannerSettings.ScannerPageSize.PageWidthID = int.Parse(WIAConsts.WIA_HORIZONTAL_SCAN_SIZE_PIXELS);
            WIAScannerSettings.ScannerPageSize.PageHeight = height;
            WIAScannerSettings.ScannerPageSize.PageHeightID = int.Parse(WIAConsts.WIA_VERTICAL_SCAN_SIZE_PIXELS);


            string convertSettings = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SerialzeClass(WIAScannerSettings).ConfigureAwait(false).GetAwaiter().GetResult();
            WIACustomaData = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.StrToBytes(convertSettings).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!(SettingsSaved))
            {
                if (MessageBox.Show("Save Settings", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    button2.PerformClick();
                }
                else
                {
                    WIACustomaData = null;
                    SettingsSaved = true;
                }
            }
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            imageViewer1.SuspendLayout();
            try
            {

                imageViewer1.ScalingMode = ImageScalingMode.Custom;
                imageViewer1.ZoomLevel *= (1 + imageViewer1.ZoomMultiplier);

            }
            finally
            {
                imageViewer1.ResumeLayout();
            }
            imageViewer1.AutoScroll = true;
            //WIA.ICommonDialog wiaCommonDialog = new WIA.CommonDialog();
            // wiaCommonDialog.ShowAcquisitionWizard(ScannerDevice);
        }

        private void numCustomDPI_ValueChanged(object sender, EventArgs e)
        {
            GetHeightWidth();
        }

        private void WIA_Settings_Dialog_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!(SettingsSaved))
            {

                if (MessageBox.Show("Save Settings", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SaveSettings();
                    //  base.OnFormClosing(e);
                    //this.Close();
                }
                else
                {
                    WIACustomaData = null;


                }
            }

            if (ScannerDevice != null)

                Marshal.ReleaseComObject(ScannerDevice);
            imageViewer1.Dispose();
            // base.OnFormClosing(e);
            // this.Close();


        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void rBtnStretchImage_Click(object sender, EventArgs e)
        {
            //rBtnStretchImage.Checked = true;
            //rBtnZoom.Checked = false;
            //rbBtnNormal.Checked = false;
            //rbBtnCenterImage.Checked = false;
            //rbBtnAutoSize.Checked = false;
            //pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
            imageViewer1.ZoomLevel = (float)0.5;
        }




        private void txtBoxWidth_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxWidth.Text))
            {

                errorProvider.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;
                errorProvider.SetError(txtBoxWidth, "Length cannot be blank");
            }
            //else
            //    errorProvider.Clear();

            //GetHeightWidth();

        }

        private void txtBoxLength_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxLength.Text))
            {
                GetHeightWidth();
                errorProvider.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;
                errorProvider.SetError(txtBoxLength, "Length cannot be blank");
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (LoadSettingsFile)
                SetDialogValues();
            else
                GetDefaultScannerSettings().ConfigureAwait(false).GetAwaiter().GetResult();
        }



        private void button6_Click(object sender, EventArgs e)
        {

            imageViewer1.SuspendLayout();
            try
            {
                imageViewer1.ScalingMode = ImageViewer.ImageScalingMode.Custom;
                imageViewer1.ZoomLevel *= (1 - imageViewer1.ZoomMultiplier);
            }
            finally
            {
                imageViewer1.ResumeLayout();
                imageViewer1.AutoScroll = false;

            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            WIA.ICommonDialog wiaCommonDialog = new WIA.CommonDialog();
            wiaCommonDialog.ShowAcquisitionWizard(ScannerDevice);
        }

        //     private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        //     {
        //         X = (sender as HScrollBar).Value;
        //         pbImage.Refresh();
        //     }

        //     private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        //     {
        //         Y = (sender as VScrollBar).Value;
        //         pbImage.Refresh();
        //     }

        //     private void pbImage_Paint(object sender, PaintEventArgs e)
        //     {
        //         pbImage = sender as PictureBox;
        //e.Graphics.DrawImage(pbImage.Image, e.ClipRectangle, X, Y, e.ClipRectangle.Width, 
        //  e.ClipRectangle.Height, GraphicsUnit.Pixel);
        //     }
        //public void System.Drawing.Bitmap AdjustContrast(Bitmap Image, float Value)
        //{

        //    Value = (100.0f + Value) / 100.0f;
        //    Value *= Value;
        //    Bitmap NewBitmap = (Bitmap)Image.Clone();
        //    BitmapData data = NewBitmap.LockBits(
        //        new Rectangle(0, 0, NewBitmap.Width, NewBitmap.Height),
        //        ImageLockMode.ReadWrite,
        //        NewBitmap.PixelFormat);
        //    int Height = NewBitmap.Height;
        //    int Width = NewBitmap.Width;

        //    //unsafe
        //    //  {
        //    for (int y = 0; y < Height; ++y)
        //    {
        //        byte* row = (byte*)data.Scan0 + (y * data.Stride);
        //        int columnOffset = 0;
        //        for (int x = 0; x < Width; ++x)
        //        {
        //            byte B = row[columnOffset];
        //            byte G = row[columnOffset + 1];
        //            byte R = row[columnOffset + 2];

        //            float Red = R / 255.0f;
        //            float Green = G / 255.0f;
        //            float Blue = B / 255.0f;
        //            Red = (((Red - 0.5f) * Value) + 0.5f) * 255.0f;
        //            Green = (((Green - 0.5f) * Value) + 0.5f) * 255.0f;
        //            Blue = (((Blue - 0.5f) * Value) + 0.5f) * 255.0f;

        //            int iR = (int)Red;
        //            iR = iR > 255 ? 255 : iR;
        //            iR = iR < 0 ? 0 : iR;
        //            int iG = (int)Green;
        //            iG = iG > 255 ? 255 : iG;
        //            iG = iG < 0 ? 0 : iG;
        //            int iB = (int)Blue;
        //            iB = iB > 255 ? 255 : iB;
        //            iB = iB < 0 ? 0 : iB;

        //            row[columnOffset] = (byte)iB;
        //            row[columnOffset + 1] = (byte)iG;
        //            row[columnOffset + 2] = (byte)iR;

        //            columnOffset += 4;
        //        }
        //    }


        //    NewBitmap.UnlockBits(data);

        //    return NewBitmap;
        //}
        //private void SetContrast(Bitmap bmp, int threshold)
        //{
        //    var data  = (Bitmap)bmp.Clone();
        //    BitmapData lockedBitmap = data.LockBits(
        //            new Rectangle(0, 0, data.Width, data.Height),
        //            ImageLockMode.ReadWrite,
        //            data.PixelFormat);
        //    //lockedBitmap.LockBits();

        //    var contrast = Math.Pow((100.0 + threshold) / 100.0, 2);

        //    for (int y = 0; y < lockedBitmap.Height; y++)
        //    {
        //        for (int x = 0; x < lockedBitmap.Width; x++)
        //        {
        //            var oldColor = lockedBitmap..GetPixel(x, y);
        //            var red = ((((oldColor.R / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
        //            var green = ((((oldColor.G / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
        //            var blue = ((((oldColor.B / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
        //            if (red > 255)
        //                red = 255;
        //            if (red < 0)
        //                red = 0;
        //            if (green > 255)
        //                green = 255;
        //            if (green < 0)
        //                green = 0;
        //            if (blue > 255)
        //                blue = 255;
        //            if (blue < 0)
        //                blue = 0;

        //            var newColor = Color.FromArgb(oldColor.A, (int)red, (int)green, (int)blue);
        //            lockedBitmap.SetPixel(x, y, newColor);
        //        }
        //    }
        //    lockedBitmap.UnlockBits();
        //}
    }
}
