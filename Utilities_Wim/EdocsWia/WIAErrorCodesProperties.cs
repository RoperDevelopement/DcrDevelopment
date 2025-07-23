using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using WIA;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using EdocsUSA.Utilities.Micosoft.WIA;
using System.Drawing;
using System.Text.RegularExpressions;

namespace EdocsUSA.Utilities.EdocsWia
{
    public class WIAErrorCodesProperties
    {


        private static readonly object lockCheck = new object();
        private static WIAErrorCodesProperties instance = null;
        WIAErrorCodesProperties() { }
        public static WIAErrorCodesProperties WIAErrorCodesWizardInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockCheck)
                    {
                        if (instance == null)
                        {
                            instance = new WIAErrorCodesProperties();
                        }
                    }
                }
                return instance;
            }
        }
        public async Task<List<string>> GetDevices()
        {

            List<string> devices = new List<string>();
            WIA.DeviceManager manager = new WIA.DeviceManager();
            foreach (WIA.DeviceInfo info in manager.DeviceInfos)
            {
                devices.Add(info.DeviceID);
            }
            return devices;
        }
        public async Task<Device> SelectDevice(int threadSleep)
        {
            //  EventWaiter eventWaiter = new EventWaiter();

             //   Application.DoEvents();
            WIA.ICommonDialog wiaCommonDialog = new WIA.CommonDialog();
            System.Threading.Thread.Sleep(threadSleep);
            Device scannerDevice = wiaCommonDialog.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, false, false);
           // Application.DoEvents();
            // wiaCommonDialog.ShowAcquisitionWizard(scannerDevice);
            // Application.DoEvents();
            if (scannerDevice != null)
                return scannerDevice;
            return null;
        }
        public async Task<Device> FindDevice(string deviceName)
        {
            List<string> devices = GetDevices().ConfigureAwait(false).GetAwaiter().GetResult();
            foreach (string device in devices)
            {
              //  Console.WriteLine();
                  Device wiaDevice = SearchWimDeciveByName(deviceName, device).ConfigureAwait(false).GetAwaiter().GetResult();
                if (wiaDevice != null)
                    return wiaDevice;
            }
            return null;
        }
        public async Task<bool> FindScannerDecive(string deviceName)
        {
            List<string> devices = GetDevices().ConfigureAwait(false).GetAwaiter().GetResult();
            foreach (string device in devices)
            {
                Console.WriteLine();
                //  Device wiaDevice = ConnectToWimDecive(deviceName, device).ConfigureAwait(false).GetAwaiter().GetResult();
                // if (wiaDevice != null)
                //  return wiaDevice;
            }
            return false;
        }
        public async Task<Device> ConnectToWimDecive(string deviceName)
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
        public async Task<Device> SearchWimDeciveByName(string deviceName, string scannerId)
        {
            //Application.AddMessageFilter(this);
            WIA.DeviceManager manager = new WIA.DeviceManager();
            WIA.Device device = null;
            try
            {
                foreach (WIA.DeviceInfo info in manager.DeviceInfos)
                {

                    if (info.DeviceID == scannerId)
                    {
                        //    object dName = info.Properties[7].get_Value();

                        /// foreach(Property p in info.Properties)
                        //  {
                        //    Console.WriteLine(p.Name);
                        // }
                        // if (string.Compare(info.DeviceID,deviceName,true) == 0)
                        // {
                        // connect to scanner
                        device = info.Connect();
                       // object o = device.Properties[7].get_Value(); 
                        string nameDevice= GetDeviceStrProperty(ref device, WIAConsts.WIA_SCANNERNAME);
                        var rx = new Regex($"{nameDevice}?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                     Match match=   rx.Match(deviceName);
                        if(match.Success)
                        //if (nameDevice.ToLower().IndexOfAny(deviceName.ToLower()) > 0)
                              return device;
                        break;
                        //string nameDevice = await GetItemValueProperty(device, WIAConsts.WiaScannerName, deviceName);


                    }

                }
            }
            catch (Exception ex)
            { }
            return null;

        }
        public async Task<Device> ConnectToWimDecive(string deviceName, string scannerId)
        {
            //Application.AddMessageFilter(this);
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
        private async Task<string> GetItemValueProperty(Device scannerDivce, object propertyName, string propValue)
        {


            Property prop = scannerDivce.Properties.get_Item(ref propertyName);
            string valueProp = (string)prop.get_Value();
            if (valueProp.ToLower().StartsWith(propValue.ToLower()))
                return valueProp;



            return string.Empty; ;
        }
        public async Task<string> BytesToStr(byte[] wiaCustomData)
        {
            //return Encoding.Default.GetString(wiaCustomData);
            using (MemoryStream Stream = new MemoryStream(wiaCustomData))
            {
                using (StreamReader streamReader = new StreamReader(Stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
            return string.Empty;
        }
        public async Task<byte[]> StrToBytes(string wiaCustomData)
        {
            return Encoding.ASCII.GetBytes(wiaCustomData); ;
        }
        //public   void SetDeviceProperty(Device device, int propertyId,
        //                                object value)
        //{
        //    Property property =  FindProperty(device.Properties, propertyId);
        //    if (property != null)
        //        property.set_Value(value);
        //}

        //public   object GetDeviceProperty(Device device, int propertyId)
        //{
        //    Property property = FindProperty(device.Properties, propertyId);
        //    return property != null ? property.get_Value() : null;
        //}

        //public   void SelectDeviceDocumentHandling(Device device,
        //                           Properties.DeviceDocumentHandling handling)
        //{
        //    int requested = (int)handling;
        //    int supported = (int)GetDeviceProperty(device,
        //             DEVICE_PROPERTY_DOCUMENT_HANDLING_CAPABILITIES_ID);
        //    if ((requested & supported) != 0)
        //    {
        //        if ((requested & (int)DeviceDocumentHandling.Feeder) != 0)
        //            SetDeviceProperty(device, DEVICE_PROPERTY_PAGES_ID, 1);
        //        SetDeviceProperty(device,
        //               DEVICE_PROPERTY_DOCUMENT_HANDLING_SELECT_ID, requested);
        //    }
        //}
        public string GetErrorCodeDescription(int errorCode)
        {
            string desc = null;

            switch (errorCode)
            {
                case (WIAConsts.WIA_ERROR_GENERAL_ERROR):
                    {
                        desc = "A general error occurred";
                        break;
                    }
                case (WIAConsts.WIA_ERROR_PAPER_JAM):
                    {
                        desc = "There is a paper jam";
                        break;
                    }
                case (WIAConsts.WIA_ERROR_PAPER_EMPTY):
                    {
                        desc = "The feeder tray is empty";
                        break;
                    }
                case (WIAConsts.WIA_ERROR_PAPER_PROBLEM):
                    {
                        desc = "There is a problem with the paper";
                        break;
                    }
                case (WIAConsts.WIA_ERROR_OFFLINE):
                    {
                        desc = "The scanner is offline";
                        break;
                    }
                case (WIAConsts.WIA_ERROR_BUSY):
                    {
                        desc = "The scanner is busy";
                        break;
                    }
                case (WIAConsts.WIA_ERROR_WARMING_UP):
                    {
                        desc = "The scanner is warming up";
                        break;
                    }
                case (WIAConsts.WIA_ERROR_USER_INTERVENTION):
                    {
                        desc = "The scanner requires user intervention";
                        break;
                    }
                case (WIAConsts.WIA_ERROR_ITEM_DELETED):
                    {
                        desc = "An unknown error occurred";
                        break;
                    }
                case (WIAConsts.WIA_ERROR_DEVICE_COMMUNICATION):
                    {
                        desc = "An error occurred attempting to communicate with the scanner";
                        break;
                    }
                case (WIAConsts.WIA_ERROR_INVALID_COMMAND):
                    {
                        desc = "The scanner does not understand this command";
                        break;
                    }
                case (WIAConsts.WIA_ERROR_INCORRECT_HARDWARE_SETTING):
                    {
                        desc = "The scanner has an incorrect hardware setting";
                        break;
                    }
                case (WIAConsts.WIA_ERROR_DEVICE_LOCKED):
                    {
                        desc = "The scanner device is in use by another application";
                        break;
                    }
                case (WIAConsts.WIA_ERROR_EXCEPTION_IN_DRIVER):
                    {
                        desc = "The scanner driver reported an error";
                        break;
                    }
                case (WIAConsts.WIA_ERROR_INVALID_DRIVER_RESPONSE):
                    {
                        desc = "The scanner driver gave an invalid response";
                        break;
                    }
                default:
                    {
                        desc = "An unknown error occurred";
                        break;
                    }
            }

            return desc;
        }
        public int GetWIAErrorCode(COMException cx)
        {
            int origErrorMsg = cx.ErrorCode;

            int errorCode = origErrorMsg & 0xFFFF;

            int errorFacility = ((origErrorMsg) >> 16) & 0x1fff;

            if (errorFacility == WIAConsts.WIAFacility)
                return errorCode;

            return -1;
        }

        public async Task<string> SerialzeClass(object dicWIASet)
        {
            var j = JsonConvert.SerializeObject(dicWIASet);
            return j.ToString();
        }
        public async Task<T> DeserializeObject<T>(string classObj) where T : new()
        {

            return JsonConvert.DeserializeObject<T>(classObj);


        }
        public void SetWIAProperty(IProperties properties, object propName, object propValue)
        {
            Property prop = properties.get_Item(ref propName);
            prop.set_Value(ref propValue);

        }
        public string GetWIAProperty(IProperties properties, object propName)
        {
            string propValue = string.Empty;
            Property prop = properties.get_Item(ref propName);
            object value = prop.get_Value();
            propValue = value.ToString();
            return propValue;
        }

        public int GetWIAIntProperty(IProperties properties, object propName)
        {
            int propValue = -1;
            Property prop = properties.get_Item(ref propName);
            propValue = prop.get_Value();

            return propValue;
        }

        public int GetDeviceIntProperty(ref Device device, int propertyID)
        {
            int ret = -1;

            foreach (Property p in device.Properties)
            {

                if (p.PropertyID == propertyID)
                {
                    ret = (int)p.get_Value();
                    break;
                }
            }

            return ret;
        }
        public string GetDeviceStrProperty(ref Device device, int propertyID)
        {
            string retStr = string.Empty;




            foreach (Property p in device.Properties)
            {

                if (p.PropertyID == propertyID)
                {
                    object devValue = p.get_Value();

                    retStr = devValue.ToString();
                    break;
                }
            }

            return retStr;
        }
        public string GetStrProperty(ref IItem item, int propertyID)
        {
            string retStr = string.Empty;
            object devValue = null;



            foreach (Property p in item.Properties)
            {

                //if (p.Name.Equals("Page Width"))
                //{

                //    //devValue = p.get_Value();
                //    // object tempNewProperty = 11692;
                //    //   p.set_Value(ref tempNewProperty);
                //    devValue = p.get_Value();
                //    retStr = devValue.ToString();
                //    break;

                //}
                if (p.PropertyID == propertyID)
                {
                    devValue = p.get_Value();

                    retStr = devValue.ToString();
                    break;

                }
                //if (p.Name.Equals("Page Size"))
                //{

                //    //devValue = p.get_Value();
                //    // object tempNewProperty = 11692;
                //    //   p.set_Value(ref tempNewProperty);
                //    devValue = p.get_Value();
                //    retStr = devValue.ToString();

                //}
                //if (p.Name.Equals("Horizontal Extent"))
                //{
                //    Console.WriteLine(p.PropertyID.ToString());
                //    devValue = p.get_Value();
                //   //object tempNewProperty = 8276;
                //   // p.set_Value(ref tempNewProperty);
                //    devValue = p.get_Value();
                //    retStr = devValue.ToString();

                //}
                //if (p.Name.Equals("Vertical Extent"))
                //{

                //    //devValue = p.get_Value();
                //  // object tempNewProperty = 11692;
                // //   p.set_Value(ref tempNewProperty);
                //    devValue = p.get_Value();
                //    retStr = devValue.ToString();


                //}
                //if (p.Name.Equals("Page Width"))
                //{

                //    //devValue = p.get_Value();
                //    // object tempNewProperty = 11692;
                //    //   p.set_Value(ref tempNewProperty);
                //    devValue = p.get_Value();
                //    retStr = devValue.ToString();

                //}


            }

            return retStr;
        }
        private int ConverMMToInches(int paperSize)
        {

            double dInch = paperSize / WIAConsts.MM;
            return (int)Math.Ceiling(dInch);

        }
        private int ConverMMToPoint(int paperSize)
        {

            double dInch = paperSize * WIAConsts.POINT;
            return (int)Math.Ceiling(dInch);

        }
        public ScannerPageSize PaperSizeInches(PaperSize pSize)
        {
            ScannerPageSize pageSize = new ScannerPageSize();
            switch (pSize)
            {

                case PaperSize.A0:
                    {
                        pageSize.PageWidth = ConverMMToPoint(WIAConsts.A0_PAPER_SIZE_MMWIDTH);
                        pageSize.PageHeight = ConverMMToPoint(WIAConsts.A0_PAPER_SIZE_MMHEIGHT);

                        break;
                    }
                case PaperSize.A1:
                    {
                        pageSize.PageWidth = ConverMMToPoint(WIAConsts.A1_PAPER_SIZE_MMWIDTH);
                        pageSize.PageHeight = ConverMMToPoint(WIAConsts.A1_PAPER_SIZE_MMHEIGHT);

                        break;
                    }
                case PaperSize.A2:
                    {
                        pageSize.PageWidth = ConverMMToPoint(WIAConsts.A2_PAPER_SIZE_MMWIDTH);
                        pageSize.PageHeight = ConverMMToPoint(WIAConsts.A2_PAPER_SIZE_MMHEIGHT);

                        break;
                    }
                case PaperSize.A3:
                    {
                        pageSize.PageWidth = ConverMMToPoint(WIAConsts.A3_PAPER_SIZE_MMWIDTH);
                        pageSize.PageHeight = ConverMMToPoint(WIAConsts.A3_PAPER_SIZE_MMHEIGHT);

                        break;
                    }
                case PaperSize.A4:
                    {
                        pageSize.PageWidth = ConverMMToPoint(WIAConsts.A4_PAPER_SIZE_MMWIDTH);
                        pageSize.PageHeight = ConverMMToPoint(WIAConsts.A4_PAPER_SIZE_MMHEIGHT);

                        break;
                    }
                case PaperSize.A5:
                    {
                        pageSize.PageWidth = ConverMMToPoint(WIAConsts.A5_PAPER_SIZE_MMWIDTH);
                        pageSize.PageHeight = ConverMMToPoint(WIAConsts.A5_PAPER_SIZE_MMHEIGHT);

                        break;
                    }
                case PaperSize.A6:
                    {
                        pageSize.PageWidth = ConverMMToInches(WIAConsts.A6_PAPER_SIZE_MMWIDTH);
                        pageSize.PageHeight = ConverMMToInches(WIAConsts.A6_PAPER_SIZE_MMHEIGHT);

                        break;
                    }
                case PaperSize.A7:
                    {
                        pageSize.PageWidth = ConverMMToInches(WIAConsts.A7_PAPER_SIZE_MMWIDTH);
                        pageSize.PageHeight = ConverMMToInches(WIAConsts.A7_PAPER_SIZE_MMHEIGHT);

                        break;
                    }
                case PaperSize.A8:
                    {
                        pageSize.PageWidth = ConverMMToInches(WIAConsts.A8_PAPER_SIZE_MMWIDTH);
                        pageSize.PageHeight = ConverMMToInches(WIAConsts.A8_PAPER_SIZE_MMHEIGHT);

                        break;
                    }
                case PaperSize.Legal:
                    {
                        pageSize.PageWidth = ConverMMToInches(WIAConsts.LEGAL_PAPER_SIZE_MMWIDTH);
                        pageSize.PageHeight = ConverMMToInches(WIAConsts.LEGAL_PAPER_SIZE_MMHEIGHT);

                        break;
                    }
                case PaperSize.LegalExtra:
                    {
                        pageSize.PageWidth = ConverMMToInches(WIAConsts.LEGAL_EXTRA_PAPER_SIZE_MMWIDTH);
                        pageSize.PageHeight = ConverMMToInches(WIAConsts.LEGAL_EXTRA_PAPER_SIZE_MMHEIGHT);

                        break;
                    }
                case PaperSize.LetterExtra:
                    {
                        pageSize.PageWidth = ConverMMToInches(WIAConsts.LETTER_EXTRA_PAPER_SIZE_MMWIDTH);
                        pageSize.PageHeight = ConverMMToInches(WIAConsts.LETTER_EXTRA_PAPER_SIZE_MMHEIGHT);

                        break;
                    }
                default:
                    return null;
            }
            return pageSize;

        }

        public System.Drawing.Image ResizeImage(Image imgToResize, Size size)
        {
            return (System.Drawing.Image)(new Bitmap(imgToResize, size));
        }
        public async Task<WIAScannerSettingsModelDeivce> GetScannerSettings(byte[] scannerSettings)
        {
            WIAScannerSettingsModelDeivce wIAScanner = new WIAScannerSettingsModelDeivce();
            try
            {
                string strData = BytesToStr(scannerSettings).ConfigureAwait(false).GetAwaiter().GetResult();
                wIAScanner = DeserializeObject<WIAScannerSettingsModelDeivce>(strData).ConfigureAwait(false).GetAwaiter().GetResult();
                if (string.IsNullOrWhiteSpace(wIAScanner.ScannerName))
                    return null;
            }
            catch { }
            return wIAScanner;
        }
        public void SaveDeviceStrProperty(  Device device, string fileName)
        {
            if(Directory.Exists(Path.GetDirectoryName(fileName)))
            { 

            System.Text.StringBuilder sb = new StringBuilder();
            sb.Append("Prop Name,Prop Id,Prop Value,ReadOnly,min,max");
            int subMin = 0;
            int subMax = 0;
            foreach (Property p in device.Properties)
            {
                 subMin = 0;
                 subMax = 0;
                try
                {
                    subMax = p.SubTypeMax;
                }
                catch { }
                try
                { subMin = p.SubTypeMin; }
                catch { }
                    object devValue = p.get_Value();

                string    retStr = devValue.ToString();
                sb.AppendLine($"{p.Name}, {p.PropertyID.ToString()},{retStr},{p.IsReadOnly},{subMin},{subMax}");
                    
               
            }

            File.WriteAllText(fileName, sb.ToString());
            }
        }
        public void SaveStrProperty(IItem item, string fileName)
        {
            if (Directory.Exists(Path.GetDirectoryName(fileName)))
            {

                string retStr = string.Empty;
                object devValue = null;

                System.Text.StringBuilder sb = new StringBuilder();
                sb.Append("Prop Name,Prop Id,Prop Value,ReadOnly,min,max");
                int subMin = 0;
                int subMax = 0;

                foreach (Property p in item.Properties)
                {

                    try
                    {
                        subMax = p.SubTypeMax;
                    }
                    catch { }
                    try
                    { subMin = p.SubTypeMin; }
                    catch { }

                    devValue = p.get_Value();

                    retStr = devValue.ToString();
                    sb.AppendLine($"{p.Name}, {p.PropertyID.ToString()},{retStr},{p.IsReadOnly},{subMin},{subMax}");





                }

                File.WriteAllText(fileName, sb.ToString());
            }
        }
    }
}
