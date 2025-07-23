using EdocsUSA.Utilities.EdocsWia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdocsUSA.Utilities.Micosoft.WIA
{
    public class ScannerTypeDevice
    {
        public int ScannerValue
        { get; set; }
        public string ScannerTypeValue
        { get; set; }
        public int ScannerTypePropID
        { get; set; }
    }
    public class ScannerBedSizeDevice
    {
        public string HorizontalSizeValue
        { get; set; }
        public int HorizontalSizePropID
        { get; set; }
        public string VerticalSizeValue
        { get; set; }
        public int VerticalSizePropID
        { get; set; }
    }
    public class ScannerDPIDevice
    {
        public string HorizontalDPIValue
        { get; set; }
        public string HorizontalDPIID
        { get; set; }
        public string VerticalDPIValue
        { get; set; }
        public string VerticalDPIPropID
        { get; set; }
    }
    public class ScannerTimeOutDevice
    {
        public string TimeOutValue
        { get; set; }
        public int TimeOutPropID
        { get; set; }
       
    }
    public class DocumentHandlingDevice
    {
        public string DocumentHandlingCapabilitiesValue
        { get; set; }
        public int DocumentHandlingCapabilitiesID
        { get; set; }
        public string DocumentHandlingStatusValue
        { get; set; }
        public int DocumentHandlingStatusID
        { get; set; }
        public string DocumentHandlingSelectValue
        { get; set; }
        public int DocumentHandlingSelectPropID
        { get; set; }
        public string DocumentHandlingOptionValue
        { get; set; }
        public int DocumentHandlingOptionPropId
        { get; set; }
        public string DocumentHandlingPagesValue
        { get; set; }
        public int DocumentHandlingPagesPropID
        { get; set; }
    }
    public class WIAScannerSettingsModelDeivce : EdocsUSA.Utilities.EdocsWia.WIAScannerSetttingPropModel
    {
        public WIAScannerSettingsModelDeivce()
        {
            
            ScannerTypeDevice = new ScannerTypeDevice();
            DocumentHandlingDevice = new DocumentHandlingDevice();
            ScannerBCT = new ScannerBCT();
            ScannerBedSize = new ScannerBedSizeDevice();
            ScannerColorProp = new ScannerColorProp();
            ScannerDPIDevice = new ScannerDPIDevice();
            ScannerDPIProp = new ScannerDPIProp();
            ScannerStartPropProp = new ScannerStartPropProp();
            ScannerTimeOutDevice = new ScannerTimeOutDevice();
            ScannerTypeDevice = new ScannerTypeDevice();
            ScannerPageSize = new ScannerPageSize();
        }
        
      public  string ScannerName
        { get; set; }
        public int ScannerPropID
        { get; set; }
        public ScannerTypeDevice ScannerTypeDevice
        { get; set; }
        public ScannerBedSizeDevice ScannerBedSize
        {
            get;set;
        }
        public ScannerDPIDevice ScannerDPIDevice
        { get; set; }
        public DocumentHandlingDevice DocumentHandlingDevice
        { get; set; }
        public ScannerTimeOutDevice ScannerTimeOutDevice
        { get; set; }
    }
}
