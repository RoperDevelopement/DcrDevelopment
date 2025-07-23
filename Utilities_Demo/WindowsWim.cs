//https://docs.microsoft.com/en-us/windows/win32/wia/-wia-device-properties
//https://docs.microsoft.com/en-us/windows/win32/wia/-wia-reading-and-setting-wia-properties
using EdocsUSA.Utilities.Interop;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WIA;
using EdocsUSA.Utilities.EdocsWia;
using EdocsUSA.Utilities.Micosoft.WIA;

namespace EdocsUSA.Utilities
{
    public class WIAImages
    {
        public byte[] FirstWIAImage
        { get; set; }
        public byte[] SecondWIAImage
        { get; set; }
    }
    public class WindowsWim : IMessageFilter, IDisposable
    {

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        internal struct WIN_MSG
        {
            public IntPtr hwnd;
            public int message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int time;
            public int x;
            public int y;
        }
        protected EventWaiter XferReadyWaiter = new EventWaiter();
        protected EventWaiter DeviceUIClosedWaiter = new EventWaiter();
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public WindowsWim()
        {

        }

        protected void Close()
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Closing Twain");
            //  CloseActiveDataSource(true);
            //  CloseDataSourceManager(true);
        }
        private async Task<List<string>> GetDevices()
        {
            List<string> devices = new List<string>();
            WIA.DeviceManager manager = new WIA.DeviceManager();
            foreach (WIA.DeviceInfo info in manager.DeviceInfos)
            {
                devices.Add(info.DeviceID);
            }
            return devices;
        }
        private async Task<Device> ConnectToWimDecive(string deviceName)
        {
            List<string> devices = GetDevices().ConfigureAwait(false).GetAwaiter().GetResult();
            foreach (string device in devices)
            {
                Device wiaDevice = ConnectToWimDecive(deviceName, device).ConfigureAwait(false).GetAwaiter().GetResult();
                if (wiaDevice != null)
                    return wiaDevice;
            }
            return null;
        }
        private async Task<bool> AdjustScannerSettings(IItem scannnerItem, WIAScannerSettingsModelDeivce scannerSettingsModelDeivce, Device scannerDevice)
        {
            bool freeder = false;
            try
            {
                if ((scannerSettingsModelDeivce != null) && (!string.IsNullOrWhiteSpace(scannerSettingsModelDeivce.ScannerName)))
                {
                    WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(scannnerItem.Properties, scannerSettingsModelDeivce.ScannerBCT.BrightnessID.ToString(), scannerSettingsModelDeivce.ScannerBCT.Brightness);
                    WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(scannnerItem.Properties, scannerSettingsModelDeivce.ScannerBCT.ContrastID.ToString(), scannerSettingsModelDeivce.ScannerBCT.Contrast);
                    //WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(scannnerItem.Properties, , scannerSettingsModelDeivce.ScannerDPIProp.HorizontalDPI);
                    WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(scannnerItem.Properties, WIAConsts.WIA_HORIZONTAL_SCAN_RESOLUTION_DPI, scannerSettingsModelDeivce.ScannerDPIProp.HorizontalDPI);

                    WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(scannnerItem.Properties, scannerSettingsModelDeivce.ScannerDPIProp.VerticalDPIPropID, scannerSettingsModelDeivce.ScannerDPIProp.VerticalDPI);
                    WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(scannnerItem.Properties, scannerSettingsModelDeivce.ScannerColorProp.ScannerColorPropID.ToString(), scannerSettingsModelDeivce.ScannerColorProp.ScannerColor);
                    WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(scannnerItem.Properties, scannerSettingsModelDeivce.ScannerStartPropProp.HorizontalStartPositionID.ToString(), scannerSettingsModelDeivce.ScannerStartPropProp.HorizontalStartPosition);
                    WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(scannnerItem.Properties, scannerSettingsModelDeivce.ScannerStartPropProp.VerticalStartPositionID.ToString(), scannerSettingsModelDeivce.ScannerStartPropProp.VerticalStartPosition);
                    WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(scannnerItem.Properties, scannerSettingsModelDeivce.ScannerPageSize.PageWidthID.ToString(), scannerSettingsModelDeivce.ScannerPageSize.PageWidth);
                    WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(scannnerItem.Properties, scannerSettingsModelDeivce.ScannerPageSize.PageHeightID.ToString(), scannerSettingsModelDeivce.ScannerPageSize.PageHeight);
                    if ((string.Compare(scannerSettingsModelDeivce.ScannerTypeDevice.ScannerTypeValue, WIAConsts.FLATBEDSCANNER, true) != 0))
                    {

                        // WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(scannnerItem.Properties, scannerSettingsModelDeivce.ScannerPageSize.PageWidthID.ToString(), scannerSettingsModelDeivce.ScannerPageSize.PageWidth);
                        //WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(scannnerItem.Properties, scannerSettingsModelDeivce.ScannerPageSize.PageHeightID.ToString(), scannerSettingsModelDeivce.ScannerPageSize.PageHeightID);


                        //    WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(scannnerItem.Properties, scannerSettingsModelDeivce.ScannerTypeDevice.ScannerTypePropID.ToString(), //scannerSettingsModelDeivce.ScannerTypeDevice.ScannerTypeValue);
                        //   WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SetWIAProperty(scannnerItem.Properties, scannerSettingsModelDeivce.ScannerPageSize.PageTypeID.ToString(), //scannerSettingsModelDeivce.ScannerPageSize.PageType);
                        if ((string.Compare(scannerSettingsModelDeivce.ScannerTypeDevice.ScannerTypeValue, WIAConsts.DUPLEXSCANNING, true) == 0))
                        {
                            scannerDevice.Properties["Document Handling Select"].set_Value(5);
                            //  scannerDevice.Properties["Pages"].set_Value(1);
                            scannerDevice.Properties["Pages"].set_Value(1);
                            freeder = true;
                        }


                    }
                }
            }
            catch (COMException cx)
            {
                int comErrorCode = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAErrorCode(cx);
                if (comErrorCode > 0)
                {
                    MessageBox.Show(WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetErrorCodeDescription(comErrorCode));
                    throw new Exception(WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetErrorCodeDescription(comErrorCode));


                }
                else
                    throw new Exception("Error Setting scanner settings Unkown error");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Setting scanner settings {ex.Message}");
            }
            return freeder;
        }
        private void AdjustScannerSettings(IItem scannnerItem, int scanResolutionDPI, int scanStartLeftPixel, int scanStartTopPixel,
                   int scanWidthPixels, int scanHeightPixels, int brightnessPercents, int contrastPercents, int colorMode)
        {
            //   object DUPLEX = "0x004";
            string ALL_PAGES = "0";
            //   Property prop = scannnerItem.Properties.get_Item(ref DUPLEX);
            //  object i = prop.get_Value();
            SetWIAProperty(scannnerItem.Properties, WIAConsts.WIA_HORIZONTAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
            SetWIAProperty(scannnerItem.Properties, WIAConsts.WIA_VERTICAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
            SetWIAProperty(scannnerItem.Properties, WIAConsts.WIA_HORIZONTAL_SCAN_START_PIXEL, scanStartLeftPixel);
            SetWIAProperty(scannnerItem.Properties, WIAConsts.WIA_VERTICAL_SCAN_START_PIXEL, scanStartTopPixel);
            SetWIAProperty(scannnerItem.Properties, WIAConsts.WIA_HORIZONTAL_SCAN_SIZE_PIXELS, scanWidthPixels);
            SetWIAProperty(scannnerItem.Properties, WIAConsts.WIA_VERTICAL_SCAN_SIZE_PIXELS, scanHeightPixels);
            SetWIAProperty(scannnerItem.Properties, WIAConsts.WIA_SCAN_BRIGHTNESS_PERCENTS, brightnessPercents);
            SetWIAProperty(scannnerItem.Properties, WIAConsts.WIA_SCAN_CONTRAST_PERCENTS, contrastPercents);
            SetWIAProperty(scannnerItem.Properties, WIAConsts.WIA_SCAN_COLOR_MODE, colorMode);
            //  SetWIAProperty(scannnerItem.Properties, ALL_PAGES, "1");



        }
        private void SetWIAProperty(IProperties properties, object propName, object propValue)
        {
            Property prop = properties.get_Item(ref propName);
            object i = prop.get_Value();
            prop.set_Value(ref propValue);

        }



        private async Task<Device> ConnectToWimDecive(string deviceName, string scannerId)
        {
            Application.AddMessageFilter(this);
            WIA.DeviceManager manager = new WIA.DeviceManager();
            WIA.Device device = null;
            try
            {
                foreach (WIA.DeviceInfo info in manager.DeviceInfos)
                {
                    if (info.DeviceID == scannerId)
                    {
                        // if (string.Compare(info.DeviceID,deviceName,true) == 0)
                        // {
                        // connect to scanner
                        device = info.Connect();
                        if (!(string.IsNullOrWhiteSpace(await GetItemValueProperty(device, WIAConsts.WiaScannerName, deviceName))))
                            return device;
                        else
                            return null;

                    }

                }
            }
            catch (Exception ex)
            { }
            return device;

        }
        private async Task<System.Drawing.Image> ByteToImage(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (Image image = Image.FromStream(stream))
                {
                    //result = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
                    image.Save(@"l:\d.tiff");
                    return image;


                    //        yield return image;
                }
            }
        }
        int GetWIAErrorCode(COMException cx)
        {
            int origErrorMsg = cx.ErrorCode;

            int errorCode = origErrorMsg & 0xFFFF;

            int errorFacility = ((origErrorMsg) >> 16) & 0x1fff;

            if (errorFacility == WIAConsts.WIAFacility)
                return errorCode;

            return -1;
        }
        public async Task AcquireWIA(byte[] CustomDSData)
        {
            Device scannerDevice = null;
            try
            {
                WIAScannerSettingsModelDeivce wIAScanner = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetScannerSettings(CustomDSData).ConfigureAwait(false).GetAwaiter().GetResult();
                if ((wIAScanner != null) && (!string.IsNullOrWhiteSpace(wIAScanner.ScannerName)))
                {

                    scannerDevice = ConnectToWimDecive(wIAScanner.ScannerName).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                Application.DoEvents();
                if (scannerDevice == null)
                {

                    scannerDevice = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SelectDevice(2000).ConfigureAwait(false).GetAwaiter().GetResult();
                    Application.DoEvents();
                    if (scannerDevice == null)
                    { throw new Exception("Cound not connect to scanner"); }

                }
                WIA.ICommonDialog wiaCommonDialog = new WIA.CommonDialog();
                ImageFile o = wiaCommonDialog.ShowAcquireImage(WiaDeviceType.ScannerDeviceType, WiaImageIntent.UnspecifiedIntent, WiaImageBias.MaximizeQuality, WIAConsts.wiaFormatJPEG, false, true, false);
            }
            catch (COMException cx)
            {
                int comErrorCode = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAErrorCode(cx);
                if (comErrorCode > 0)
                {
                    MessageBox.Show(WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetErrorCodeDescription(comErrorCode), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"UnKnown Error :{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (scannerDevice != null)
                Marshal.ReleaseComObject(scannerDevice);
        }
        // public IEnumerable<System.Drawing.Image> AcquireWIA(string scannerName, byte[] CustomDSData, bool showUi)
        public IEnumerable<System.Drawing.Image> AcquireWIA(string scannerName, byte[] CustomDSData, bool showUi)
        {


            Device scannerDevice = null;
            WIAScannerSettingsModelDeivce wIAScanner = null;
            
                wIAScanner= WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetScannerSettings(CustomDSData).ConfigureAwait(false).GetAwaiter().GetResult();
            if ((wIAScanner != null) && (!string.IsNullOrWhiteSpace(wIAScanner.ScannerName)))
            {

                scannerDevice = ConnectToWimDecive(wIAScanner.ScannerName).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            Application.DoEvents();
            if (scannerDevice == null)
            {

                Application.DoEvents();
                scannerDevice = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.SelectDevice(2000).ConfigureAwait(false).GetAwaiter().GetResult();
                if (scannerDevice == null)
                { throw new Exception("Cound not connect to scanner"); }

            }
            
            WIA.ICommonDialog wiaCommonDialog = new WIA.CommonDialog();
            //if(showUi)
            //{
            //    wiaCommonDialog.ShowAcquisitionWizard(scannerDevice);

            //}
            //Vector files;

            //files = new Vector();
            //files.Add(@"l:\434da9cb-94ea-4c37-9c26-8d484b9946ec.tiff");
            //files.Add(@"l:\imagestovideo_screenshot1.png");
            //wiaCommonDialog.ShowPhotoPrintingWizard(files);
            //  scannerDevice.Properties["Document Handling Select"].set_Value(5);
            //  scannerDevice.Properties["Pages"].set_Value(1);
            // WIA.CommonDialog commonDialogClass = new WIA.CommonDialog();
            //   GetItemValueProperty(scannerDevice, DUPLEX, "").ConfigureAwait(false).GetAwaiter().GetResult();
            Item scannnerItem = scannerDevice.Items[1];
            bool hasMorePages = true;
            //     wiaCommonDialog.ShowAcquisitionWizard(scannerDevice);


            bool duplex = AdjustScannerSettings(scannnerItem, wIAScanner, scannerDevice).ConfigureAwait(false).GetAwaiter().GetResult();



            while (hasMorePages)
            {
                object scanResult = null;

                //   object scanResult1 = wiaCommonDialog.ShowAcquisitionWizard(scannerDevice);
                try
                {
                    if (showUi)
                    {
                        duplex = false;
                        scanResult = wiaCommonDialog.ShowAcquireImage(WiaDeviceType.ScannerDeviceType, WiaImageIntent.UnspecifiedIntent, WiaImageBias.MaximizeQuality, WIAConsts.wiaFormatJPEG, false, false, false);
                    }
                    else
                    {
                        scanResult = wiaCommonDialog.ShowTransfer(scannnerItem, WIAConsts.wiaFormatJPEG, false);
                    }
                }
                catch (COMException cx)
                {
                    int comErrorCode = WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetWIAErrorCode(cx);
                    if (comErrorCode > 0)
                    {
                        if (MessageBox.Show(WIAErrorCodesProperties.WIAErrorCodesWizardInstance.GetErrorCodeDescription(comErrorCode), "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                            continue;

                    }
                    break;
                }
                catch
                {
                    if (MessageBox.Show("Add More Pages", "Pages", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        continue;
                    break;
                }
                if (scanResult != null)
                {

                    ImageFile imageFile = (ImageFile)scanResult;


                    byte[] data = (byte[])imageFile.FileData.get_BinaryData();

                    using (MemoryStream stream = new MemoryStream(data))
                    {
                        using (System.Drawing.Image image = System.Drawing.Image.FromStream(stream))
                        {
                            //result = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

                            yield return image;


                            //        yield return image;
                        }
                    }
                    //Image retInt= ByteToImage(data).ConfigureAwait(false).GetAwaiter().GetResult();



                    if (duplex)
                    {

                        ImageFile imgduplex = null;
                        imgduplex = wiaCommonDialog.ShowTransfer(scannnerItem, WIAConsts.wiaFormatJPEG);
                        byte[] data2 = (byte[])imgduplex.FileData.get_BinaryData();
                        using (MemoryStream stream = new MemoryStream(data2))
                        {
                            using (System.Drawing.Image image2 = System.Drawing.Image.FromStream(stream))
                            {
                                //result = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

                                yield return image2;
                                System.Threading.Thread.Sleep(1000);

                                //        yield return image;
                            }
                        }
                    }

                    // Guid guid = System.Guid.NewGuid();


                    //   Console.WriteLine();
                    //string sImageName = $"l:\\{guid.ToString()}.tiff";
                    // System.IO.File.Delete(@"l:\test.tiff");
                    //imageFile.SaveFile(sImageName);
                    //  System.Drawing.Image image = System.Drawing.Image.FromFile(sImageName);
                    //System.Drawing.Image image = (System.Drawing.Image)scanResult;
                    //using (MemoryStream stream = new MemoryStream(data))
                    //{
                    //    using (Image image = Image.FromStream(stream))
                    //    {
                    //        //result = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);



                    //        yield return image;
                    //    }
                    //}
                }
                //    System.Threading.Thread.Sleep(1000);
                // assume there are no more pages
                hasMorePages = false;
                if (duplex)
                {
                    WIA.Property documentHandlingSelect = null;
                    WIA.Property documentHandlingStatus = null;
                    // System.Text.StringBuilder sb = new StringBuilder();
                    // sb.Append("ID,Name,Value");
                    foreach (WIA.Property prop in scannerDevice.Properties)
                    {
                        // object v = prop.get_Value();
                        //   sb.AppendLine($"{prop.PropertyID.ToString()},{prop.Name},{v.ToString()}");
                        if (prop.PropertyID == WIAConsts.WIA_DPS_DOCUMENT_HANDLING_SELECT)
                            documentHandlingSelect = prop;
                        if (prop.PropertyID == WIAConsts.WIA_DPS_DOCUMENT_HANDLING_STATUS)
                            documentHandlingStatus = prop;
                    }
                    //  File.WriteAllText(@"l:\prop.txt", sb.ToString());

                    // may not exist on flatbed scanner but required for feeder
                    if (documentHandlingSelect != null)
                    {
                        // check for document feeder
                        if ((Convert.ToUInt32(documentHandlingSelect.get_Value()) & WIAConsts.FEEDER) != 0)
                        {
                            hasMorePages = ((Convert.ToUInt32(documentHandlingStatus.get_Value()) & WIAConsts.FEED_READY) != 0);
                        }
                    }
                }
            }
            if (scannerDevice != null)
                Marshal.ReleaseComObject(scannerDevice);
        }
        //   foreach (System.Drawing.Image image in wiaCommonDialog.ShowTransfer(scannnerItem, wiaFormatTIFF, false))
        //    {

        //}



        private async Task<string> GetItemValueProperty(Device scannerDivce, object propertyName, string propValue)
        {


            Property prop = scannerDivce.Properties.get_Item(ref propertyName);
            string valueProp = (string)prop.get_Value();
            if (valueProp.ToLower().StartsWith(propValue.ToLower()))
                return valueProp;



            return string.Empty; ;
        }

        public bool PreFilterMessage(ref Message m)
        {
            //Debug.WriteLine("PreFilterMessage");
            try
            {
                //Trying to process keyboard or mouse events can cause problems with the UI, so ignore them
                if ((m.Msg >= WindowsMessageIDs.WM_MOUSEMOVE) && (m.Msg <= WindowsMessageIDs.WM_XBUTTONDBLCLK))
                {
                    //Debug.WriteLine("Mouse Event");
                    return false;
                }
                if ((m.Msg >= WindowsMessageIDs.WM_KEYDOWN) && (m.Msg <= WindowsMessageIDs.WM_KEYUP))
                {
                    //Debug.WriteLine("Keyboard Event");
                    return false;
                }

                //If the source hasn't been initialized, break
                //  if (ActiveDataSource == null) return false;

                //Debug.WriteLine("Not Keyboard or mouse");

                //Convert the message to a windows mesage structure
                // LowHighInt pos = new LowHighInt() { Number = (int)User32.GetMessagePos() };
                //WIN_MSG winmsg = new WIN_MSG()
                //{
                //    hwnd = m.HWnd,
                //    message = m.Msg,
                //    wParam = m.WParam,
                //    lParam = m.LParam,
                //    time = User32.GetMessageTime(),
                ////     x = pos.Low,
                ////     y = pos.High
                //};

                //TW_EVENT twevt = new TW_EVENT()
                //{
                //    //TODO: Free pEvent
                //    EventPtr = Marshal.AllocHGlobal(Marshal.SizeOf(winmsg)),
                //    Message = (short)MSG.Null
                //};
                //Marshal.StructureToPtr(winmsg, twevt.EventPtr, false);

                //Ask the device to process the event
                //   TWRC r = DS_EVENT(AppId, ActiveDataSource, DG.Control, DAT.Event, MSG.ProcessEvent, ref twevt);
                //   if (r == TWRC.NotDSEvent)
                //  {
                ////      Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Source said not ds event for datasource:{ActiveDataSource.ProductName}");
                //     return false;
                // }
                //Debug.WriteLine("Source DS event");
                //    switch (twevt.Message)
                //    {
                //        case (short)MSG.XFerReady:
                //            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("XFERREADY");
                //           // if (XferReady.Set() == false)
                //          //  {
                //           //     Logging.TraceLogger.TraceLoggerInstance.TraceError("Error setting XferReady");
                //          //      throw new Exception("Error setting XferReady");
                //          //  }
                //           // XferReadyWaiter.Set();
                //            return true;
                //        case (short)MSG.CloseDSReq:
                //            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("CLOSEDSREQ");
                //          //  if (DeviceUiCanceled.Set() == false)
                //           // {
                //          //     Logging.TraceLogger.TraceLoggerInstance.TraceError("Error setting DeviceUiCanceled");
                //         //       throw new Exception("Error setting DeviceUiCanceled");
                //         //  }
                //           // DeviceUICanceledWaiter.Set();
                //            return true;
                //        case (short)MSG.CloseDSOK:
                //            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("CLOSEDSOK");
                //        //    DeviceUIClosedWaiter.Set();
                //            return true;
                //        case (short)MSG.DeviceEvent:
                //            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("DEVICEEVENT");
                //            //TODO:Implement
                //            return true;
                //        default: return false;
                //    }
            }
            catch (OverflowException of)
            {
                Console.WriteLine(of.Message);
                return false;
            }
            return false;
        }
        protected void Dispose(bool disposing)
        {
            Application.RemoveMessageFilter(this);

            Close();
        }


    }
}
