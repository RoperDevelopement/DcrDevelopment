using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdocsUSA.Utilities.EdocsWia
{
    public enum ScannerColor
    {
        Color = 1,
        GrayScanle = 2,
        BlackandWhite = 3,
    }
    public class ScannerColorProp
    {
        public int ScannerColor
        { get; set; }
        public int ScannerColorPropID
        { get; set; }
    }
    public class ScannerDPIProp
    {
        public int HorizontalDPI
        { get; set; }
        public int HorizontalDPIID
        { get; set; }
        public int VerticalDPI
        { get; set; }
        public string VerticalDPIPropID
        { get; set; }
    }
    public class ScannerStartPropProp
    {
        public int HorizontalStartPosition
        { get; set; }
        public int HorizontalStartPositionID
        { get; set; }
        public int VerticalStartPosition
        { get; set; }
        public int VerticalStartPositionID
        { get; set; }
    }
    public class ScannerBCT
    {
        public int Brightness
        { get; set; }
        public int BrightnessID
        { get; set; }
        public int Contrast
        { get; set; }
        public int ContrastID
        { get; set; }
        public string Threshold
        { get; set; }
        public int ThresholdPropID
        { get; set; }
    }
    public class ScannerPageSize
    {
        public string PaperType
        { get; set; }
        public string ScannerType
        { get; set; }
        public int PageType
        { get; set; }
        public int PageTypeID
        { get; set; }
        public int PageWidth
        { get; set; }
        public int PageHeight
        { get; set; }
        public int PageWidthID
        { get; set; }
        public int PageHeightID
        { get; set; }
    }

        public class WIAScannerSetttingPropModel
        {
            public ScannerColorProp ScannerColorProp
            { get; set; }
            public ScannerDPIProp ScannerDPIProp
            { get; set; }
            public ScannerStartPropProp ScannerStartPropProp
            { get; set; }
            public ScannerBCT ScannerBCT
            { get; set; }
            public ScannerPageSize ScannerPageSize
            { get; set; }
        }
    }
