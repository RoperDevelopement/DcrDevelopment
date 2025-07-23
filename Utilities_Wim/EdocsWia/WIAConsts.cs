//Consts come from
/****************************************************************************
*
*  Copyright (c) Microsoft Corporation. All rights reserved.
*
*  File: wiadef.h
*
*  Version: 4.0
*
*  Description: WIA constant definitions
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace EdocsUSA.Utilities.EdocsWia
{
    public enum PaperSize
    {
        A0,
        A1,
        A2,
        A3,
        A4,
        A5,
        A6,
        A7,
        A8,
        Legal,
        LegalExtra,
        LetterExtra

    }
    public class WIAConsts
    {
        public const string SINGLESCANNING = "Single Scanning";
        public const string FLATBEDSCANNER = "FlatBed Scanner";
        public const string DUPLEXSCANNING ="Duplex Scanning";
        public const double POINT = 2.8346456693;
        public const            double MM = 25.4;
        public const int A0_PAPER_SIZE_MMWIDTH = 841;
        public const int A0_PAPER_SIZE_MMHEIGHT = 1189;
        public const int A1_PAPER_SIZE_MMWIDTH = 594;
        public const int A1_PAPER_SIZE_MMHEIGHT = 841;
        public const int A2_PAPER_SIZE_MMWIDTH = 420;
        public const int A2_PAPER_SIZE_MMHEIGHT = 594;
        public const int A3_PAPER_SIZE_MMWIDTH = 297;
        public const int A3_PAPER_SIZE_MMHEIGHT = 420;
        public const int A4_PAPER_SIZE_MMWIDTH = 210;
        public const int A4_PAPER_SIZE_MMHEIGHT = 297;
        public const int A5_PAPER_SIZE_MMWIDTH = 148;
        public const int A5_PAPER_SIZE_MMHEIGHT = 210;
        public const int A6_PAPER_SIZE_MMWIDTH = 105;
        public const int A6_PAPER_SIZE_MMHEIGHT = 148;
        public const int A7_PAPER_SIZE_MMWIDTH = 74;
        public const int A7_PAPER_SIZE_MMHEIGHT = 105;
        public const int A8_PAPER_SIZE_MMWIDTH = 52;
        public const int A8_PAPER_SIZE_MMHEIGHT = 74;
        public const int LEGAL_PAPER_SIZE_MMWIDTH = 216;
        public const int LEGAL_PAPER_SIZE_MMHEIGHT = 356;
        public const int LEGAL_EXTRA_PAPER_SIZE_MMWIDTH = 241;
        public const int LEGAL_EXTRA_PAPER_SIZE_MMHEIGHT = 381;
        public const int LETTER_EXTRA_PAPER_SIZE_MMWIDTH = 241;
        public const int LETTER_EXTRA_PAPER_SIZE_MMHEIGHT = 305;
        public const int HORBEDSIZR = 3074;
        public const int VERBEDSIZE = 3075;
        public const int HORDPI = 3090;
        public const int VERDPI = 3091;


     
        public const string WiaScannerName = "Name";
        public const int WIAFacility = 33;
        public const int WIA_SCANNERNAME = 7;
        public const int WIA_ERROR_GENERAL_ERROR = 1;
        public const int WIA_ERROR_PAPER_JAM = 2;
        public const int WIA_ERROR_PAPER_EMPTY = 3;
        public const int WIA_ERROR_PAPER_PROBLEM = 4;
        public const int WIA_ERROR_OFFLINE = 5;
        public const int WIA_ERROR_BUSY = 6;
        public const int WIA_ERROR_WARMING_UP = 7;
        public const int WIA_ERROR_USER_INTERVENTION = 8;
        public const int WIA_ERROR_ITEM_DELETED = 9;
        public const int WIA_ERROR_DEVICE_COMMUNICATION = 10;
        public const int WIA_ERROR_INVALID_COMMAND = 11;
        public const int WIA_ERROR_INCORRECT_HARDWARE_SETTING = 12;
        public const int WIA_ERROR_DEVICE_LOCKED = 13;
        public const int WIA_ERROR_EXCEPTION_IN_DRIVER = 14;
        public const int WIA_ERROR_INVALID_DRIVER_RESPONSE = 15;
        public const int WIA_S_NO_DEVICE_AVAILABLE = 21;
        //
        // WIA_IPS_PRINTER_ENDORSER constants
        //
        public const int DEVICE_PROPERTY_DOCUMENT_HANDLING_CAPABILITIES_ID = 3086;
        public const int DEVICE_PROPERTY_DOCUMENT_HANDLING_STATUS_ID = 3087;
        public const int DEVICE_PROPERTY_DOCUMENT_HANDLING_SELECT_ID = 3088;
        public const int DEVICE_PROPERTY_PAGES_ID = 3096;
        public const int WIA_PRINTER_ENDORSER_DISABLED = 0;
        public const int WIA_PRINTER_ENDORSER_AUTO = 1;
        public const int WIA_PRINTER_ENDORSER_FLATBED = 2;
        public const int WIA_PRINTER_ENDORSER_FEEDER_FRONT = 3;
        public const int WIA_PRINTER_ENDORSER_FEEDER_BACK = 4;
        public const int WIA_PRINTER_ENDORSER_FEEDER_DUPLEX = 5;
        public const int WIA_PRINTER_ENDORSER_DIGITAL = 6;
        //
        // WIA_IPS_LAMP constants
        //DETECT_FEED

        public const int WIA_LAMP_ON = 0;
        public const int WIA_LAMP_OFF = 1;

        //
        // WIA_IPS_AUTO_DESKEW constants
        //
        public const int COLOR_BLACK_WHITE = 0;
        public const int COLOR_COLOR = 1;
        public const int COLOR_GRAY_SCALE = 2;


        public const int WIA_AUTO_DESKEW_ON = 0;
        public const int WIA_AUTO_DESKEW_OFF = 1;

        public const uint WIA_RESERVED_FOR_NEW_PROPS = 1024;
        public const uint WIA_DIP_FIRST = 2;
        public const uint WIA_DPA_FIRST = WIA_DIP_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
        public const uint WIA_DPC_FIRST = WIA_DPA_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
        public const uint FEEDER = 0x00000001;
        public const uint FLATBED = 0x00000002;
        public const uint FEED_READY = 0x00000001;

        //
        // Scanner only device properties (DPS)
        //
        public const uint WIA_DPS_FIRST = WIA_DPC_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
        public const uint WIA_DPS_DOCUMENT_HANDLING_STATUS = WIA_DPS_FIRST + 13;
        public const uint WIA_DPS_DOCUMENT_HANDLING_SELECT_MORE_PAGES = WIA_DPS_FIRST + 14;

        public const string wiaFormatBMP = "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}";
        public const string wiaFormatPNG = "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}";
        public const string wiaFormatGIF = "{B96B3CB0-0728-11D3-9D7B-0000F81EF32E}";
        public const string wiaFormatJPEG = "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}";
        public const string wiaFormatTIFF = "{B96B3CB1-0728-11D3-9D7B-0000F81EF32E}";

        public const string WIA_SCAN_COLOR_MODE = "6146";
        public const string WIA_HORIZONTAL_SCAN_RESOLUTION_DPI = "6147";
        public const string WIA_VERTICAL_SCAN_RESOLUTION_DPI = "6148";
        public const string WIA_HORIZONTAL_SCAN_START_PIXEL = "6149";
        public const string WIA_VERTICAL_SCAN_START_PIXEL = "6150";
        public const string WIA_HORIZONTAL_SCAN_SIZE_PIXELS = "6151";
        public const string WIA_VERTICAL_SCAN_SIZE_PIXELS = "6152";
        public const string WIA_SCAN_BRIGHTNESS_PERCENTS = "6154";
        public const string WIA_SCAN_CONTRAST_PERCENTS = "6155";

        //
        // WIA_IPS_MULTI_FEED constants
        //

        public const int WIA_MULTI_FEED_DETECT_DISABLED = 0;
        public const int WIA_MULTI_FEED_DETECT_STOP_ERROR = 1;
        public const int WIA_MULTI_FEED_DETECT_STOP_SUCCESS = 2;
        public const int WIA_MULTI_FEED_DETECT_CONTINUE = 3;

        //
        // WIA_IPS_MULTI_FEED_DETECT_METHOD constants
        //

        public const int WIA_MULTI_FEED_DETECT_METHOD_LENGTH = 0;
        public const int WIA_MULTI_FEED_DETECT_METHOD_OVERLAP = 1;

        //
        // WIA_IPS_AUTO_CROP constants
        //

        public const int WIA_AUTO_CROP_DISABLED = 0;
        public const int WIA_AUTO_CROP_SINGLE = 1;
        public const int WIA_AUTO_CROP_MULTI = 2;

        //
        // WIA_IPS_OVER_SCAN constants
        //

        public const int WIA_OVER_SCAN_DISABLED = 0;
        public const int WIA_OVER_SCAN_TOP_BOTTOM = 1;
        public const int WIA_OVER_SCAN_LEFT_RIGHT = 2;
        public const int WIA_OVER_SCAN_ALL = 3;


        //
        // WIA_IPS_COLOR_DROP constants
        //

        public const int WIA_COLOR_DROP_DISABLED = 0;
        public const int WIA_COLOR_DROP_RED = 1;
        public const int WIA_COLOR_DROP_GREEN = 2;
        public const int WIA_COLOR_DROP_BLUE = 3;
        public const int WIA_COLOR_DROP_RGB = 4;

        //
        // WIA_IPS_FEEDER_CONTROL constants
        //

        public const int WIA_FEEDER_CONTROL_AUTO = 0;
        public const int WIA_FEEDER_CONTROL_MANUAL = 1;
        public const int WIA_DPS_DOCUMENT_HANDLING_SELECT = 3088; // 0xc10
        public const string WIA_DPS_DOCUMENT_HANDLING_SELECT_STR = "Document Handling Select";

        public const string WIA_DPS_DOCUMENT_HANDLING_CAPACITY = "3089"; // 0xc11

        public const string WIA_DPS_DOCUMENT_HANDLING_CAPACITY_STR = "Document Handling Capacity";
        public const string WIA_DPS_PAGES = "3096"; // 0xc18
        public const string WIA_DPS_PAGES_STR = "Pages";

        public const string WIA_DPS_PAGE_SIZE = "3097"; // 0xc19
        public const string WIA_DPS_PAGE_SIZE_STR = "Page Size";

        public const string WIA_DPS_PAGE_WIDTH = "3098"; // 0xc1a
public const string WIA_DPS_PAGE_WIDTH_STR = "Page Width";

        public const string WIA_DPS_PAGE_HEIGHT = "3099"; // 0xc1b
        public const string WIA_DPS_PAGE_HEIGHT_STR = "Page Height";

        public const string WIA_DPS_TRANSPARENCY_CAPABILITIES = "3106"; // 0xc22
        public const string WIA_DPS_TRANSPARENCY_CAPABILITIES_STR = "Transparency Adapter Capabilities";

        public const string WIA_DPS_TRANSPARENCY_STATUS = "3107"; // 0xc23
        public const string WIA_DPS_TRANSPARENCY_STATUS_STR = "Transparency Adapter Status";
        public const string WIA_IPS_DESKEW_X = "6162"; // 0x1812;
public const string WIA_IPS_DESKEW_X_STR = "DeskewX";

        public const string WIA_IPS_DESKEW_Y = "6163"; // 0x1813
        public const string WIA_IPS_DESKEW_Y_STR = "DeskewY";
        public const string WIA_IPS_MAX_HORIZONTAL_SIZE = "6165"; // 0x1815
public const string WIA_IPS_MAX_HORIZONTAL_SIZE_STR = "Maximum Horizontal Scan Size";

        public const string WIA_IPS_MAX_VERTICAL_SIZE = "6166"; // 0x1816
        public const string WIA_IPS_MAX_VERTICAL_SIZE_STR = "Maximum Vertical Scan Size";

        public const string WIA_IPS_MIN_HORIZONTAL_SIZE = "6167"; // 0x1817
        public const string WIA_IPS_MIN_HORIZONTAL_SIZE_STR = "Minimum Horizontal Scan Size";

        public const string WIA_IPS_MIN_VERTICAL_SIZE = "6168"; // 0x1818
        public const string WIA_IPS_MIN_VERTICAL_SIZE_STR = "Minimum Vertical Scan Size";
        public const string WIA_IPS_PAGES = "3096"; // 0xc18
        public const string WIA_IPS_PAGES_STR = "Pages";
        public const string WIA_IPS_PAGE_SIZE = "3097"; // 0xc19
public const string WIA_IPS_PAGE_SIZE_STR = "Page Size";

        public const string WIA_IPS_PAGE_WIDTH = "3098"; // 0xc1a
public const string WIA_IPS_PAGE_WIDTH_STR = "Page Width";

        public const string WIA_IPS_PAGE_HEIGHT = "3099"; // 0xc1b
        public const string WIA_IPS_PAGE_HEIGHT_STR = "Page Height";
        public const string WIA_IPS_LAMP = "3105"; // 0xc21
        public const string WIA_IPS_LAMP_STR = "Lamp";

        public const string WIA_IPS_LAMP_AUTO_OFF = "3106"; // 0xc22
public const string WIA_IPS_LAMP_AUTO_OFF_STR = "Lamp Auto Off";

       // public const string WIA_IPS_LAMP = "3105"; // 0xc21
      //  public const string WIA_IPS_LAMP_STR = "Lamp";

       // public const string WIA_IPS_LAMP_AUTO_OFF = "3106"; // 0xc22
      //  public const string WIA_IPS_LAMP_AUTO_OFF_STR = "Lamp Auto Off";
        public const string WIA_IPS_OVER_SCAN = "4171"; // 0x104b
        public const string WIA_IPS_OVER_SCAN_STR = "Overscan";

        public const string WIA_IPS_OVER_SCAN_LEFT = "4172"; // 0x104c
        public const string WIA_IPS_OVER_SCAN_LEFT_STR = "Overscan Left";

        public const string WIA_IPS_OVER_SCAN_RIGHT = "4173"; // 0x104d
        public const string WIA_IPS_OVER_SCAN_RIGHT_STR = "Overscan Right";

        public const string WIA_IPS_OVER_SCAN_TOP = "4174"; // 0x104e
        public const string WIA_IPS_OVER_SCAN_TOP_STR = "Overscan Top";

        public const string WIA_IPS_OVER_SCAN_BOTTOM = "4175"; // 0x104f
        public const string WIA_IPS_OVER_SCAN_BOTTOM_STR = "Overscan Bottom";

        public const string WIA_IPS_COLOR_DROP = "4176"; // 0x1050
        public const string WIA_IPS_COLOR_DROP_STR = "Color Drop";

        public const string WIA_IPS_COLOR_DROP_RED = "4177"; // 0x1051
        public const string WIA_IPS_COLOR_DROP_RED_STR = "Color Drop Red";

        public const string WIA_IPS_COLOR_DROP_GREEN = "4178"; // 0x1052
        public const string WIA_IPS_COLOR_DROP_GREEN_STR = "Color Drop Green";

        public const string WIA_IPS_COLOR_DROP_BLUE = "4179"; // 0x1053
        public const string WIA_IPS_COLOR_DROP_BLUE_STR = "Color Drop Blue";

        public const string WIA_IPS_FEEDER_CONTROL = "4182"; // 0x1056;
        public const string WIA_IPS_FEEDER_CONTROL_STR = "Feeder Control";
        public const string WIA_IPS_COLOR_DROP_MULTI = "4191"; // 0x105F
        public const string WIA_IPS_COLOR_DROP_MULTI_STR = "Color Drop Multiple";

        public const string WIA_IPS_BLANK_PAGES_SENSITIVITY = "4192"; // 0x1060
        public const string WIA_IPS_BLANK_PAGES_SENSITIVITY_STR = "Blank Pages Sensitivity";

        public const string WIA_IPS_MULTI_FEED_DETECT_METHOD = "4193"; // 0x1061
        public const string WIA_IPS_MULTI_FEED_DETECT_METHOD_STR = "Multi-Feed Detection Method";

        //
        // WIA_IPS_LAMP constants
        //

       // public const int WIA_LAMP_ON = 0;
       // public const int WIA_LAMP_OFF = 1;

        //
        // WIA_IPS_AUTO_DESKEW constants
        //

//public const int WIA_AUTO_DESKEW_ON = 0;
  //      public const int WIA_AUTO_DESKEW_OFF = 1;
        
// WIA_IPS_BLANK_PAGES constants
//

public const int WIA_BLANK_PAGE_DETECTION_DISABLED = 0;
        public const int WIA_BLANK_PAGE_DISCARD = 1;
        public const int WIA_BLANK_PAGE_JOB_SEPARATOR = 2;

        //
        // WIA_IPS_MULTI_FEED constants
        //

        //public const int WIA_MULTI_FEED_DETECT_DISABLED = 0;
        //public const int WIA_MULTI_FEED_DETECT_STOP_ERROR = 1;
        //public const int WIA_MULTI_FEED_DETECT_STOP_SUCCESS = 2;
        //public const int WIA_MULTI_FEED_DETECT_CONTINUE = 3;
        //
        // WIA_IPS_AUTO_CROP constants
        //

        //public const int WIA_AUTO_CROP_DISABLED = 0;
        //public const int WIA_AUTO_CROP_SINGLE = 1;
        //public const int WIA_AUTO_CROP_MULTI = 2;
        //
        // WIA_IPS_COLOR_DROP constants
        //

        //public const int WIA_COLOR_DROP_DISABLED = 0;
        //public const int WIA_COLOR_DROP_RED = 1;
        //public const int WIA_COLOR_DROP_GREEN = 2;
        //public const int WIA_COLOR_DROP_BLUE = 3;
        //public const int WIA_COLOR_DROP_RGB = 4;

        //
        // WIA_IPS_FEEDER_CONTROL constants
        //

        //public const int WIA_FEEDER_CONTROL_AUTO = 0;
        //public const int WIA_FEEDER_CONTROL_MANUAL = 1;

//
// WIA_DPS_DOCUMENT_HANDLING_STATUS flags
//

        //public const uint FEED_READY = 0x01;
        //public const uint FLAT_READY = 0x02;
        //public const uint DUP_READY = 0x04;
        //public const uint FLAT_COVER_UP = 0x08;
        //public const uint PATH_COVER_UP = 0x10;
        //public const uint PAPER_JAM = 0x20;

//
// WIA_DPS_DOCUMENT_HANDLING_CAPABILITIES flags
//

        public const uint FEED = 0x01;
        public const uint FLAT = 0x02;
        public const uint DUP = 0x04;
        public const uint DETECT_FLAT = 0x08;
        public const uint DETECT_SCAN = 0x10;
        public const uint DETECT_FEED = 0x20;
        public const uint DETECT_DUP = 0x40;
        public const uint DETECT_FEED_AVAIL = 0x80;
        public const uint DETECT_DUP_AVAIL = 0x100;

//
// WIA_DPS_DOCUMENT_HANDLING_SELECT flags
//

       //// public const uint FEEDER = 0x001;
     //   public const uint FLATBED = 0x002;
        public const uint DUPLEX = 0x004;
        public const uint FRONT_FIRST = 0x008;
        public const uint BACK_FIRST = 0x010;
        public const uint FRONT_ONLY = 0x020;
        public const uint BACK_ONLY = 0x040;
        public const uint NEXT_PAGE = 0x080;
        public const uint PREFEED = 0x100;
        public const uint AUTO_ADVANCE = 0x200;

        public const uint ADVANCED_DUPLEX = 0x400;

        public const int ALL_PAGES = 0;

        //
        // WIA_DPS_PAGE_SIZE/WIA_IPS_PAGE_SIZE constants
        // Dimensions are defined as (WIDTH x HEIGHT) in 1/1000ths of an inch
        //

        public const int WIA_PAGE_A4 = 0; //  8267 x 11692
        public const int WIA_PAGE_LETTER = 1;//  8500 x 11000
        public const int WIA_PAGE_CUSTOM = 2; // (current extent settings)

        public const int WIA_PAGE_USLEGAL = 3; //  8500 x 14000
        public const int WIA_PAGE_USLETTER   =   WIA_PAGE_LETTER;
public const int WIA_PAGE_USLEDGER = 4; // 11000 x 17000
        public const int WIA_PAGE_USSTATEMENT = 5; //  5500 x  8500
        public const int WIA_PAGE_BUSINESSCARD = 6; //  3543 x  2165

        //
        // ISO A page size constants
        //

        public const int WIA_PAGE_ISO_A0 = 7; // 33110 x 46811
        public const int WIA_PAGE_ISO_A1 = 8; // 23385 x 33110
        public const int WIA_PAGE_ISO_A2 = 9; // 16535 x 23385
        public const int WIA_PAGE_ISO_A3 = 10; // 11692 x 16535
        public const int WIA_PAGE_ISO_A4    =   WIA_PAGE_A4;
public const int WIA_PAGE_ISO_A5 = 11; //  5826 x  8267
        public const int WIA_PAGE_ISO_A6 = 12;//  4133 x  5826
        public const int WIA_PAGE_ISO_A7 = 13; //  2913 x  4133
        public const int WIA_PAGE_ISO_A8 = 14; //  2047 x  2913
        public const int WIA_PAGE_ISO_A9 = 15;//  1456 x  2047
        public const int WIA_PAGE_ISO_A10 = 16; //  1023 x  1456

        //
        // ISO B page size constants
        //

        public const int WIA_PAGE_ISO_B0 = 17; //  39370 x 55669
        public const int WIA_PAGE_ISO_B1 = 18; //  27834 x 39370
        public const int WIA_PAGE_ISO_B2 = 19; //  19685 x 27834
        public const int WIA_PAGE_ISO_B3 = 20; //  13897 x 19685
        public const int WIA_PAGE_ISO_B4 = 21; //   9842 x 13897
        public const int WIA_PAGE_ISO_B5 = 22; //   6929 x  9842
        public const int WIA_PAGE_ISO_B6 = 23; //   4921 x  6929
        public const int WIA_PAGE_ISO_B7 = 24; //   3464 x  4921
        public const int WIA_PAGE_ISO_B8 = 25; //   2440 x  3464
        public const int WIA_PAGE_ISO_B9 = 26; //   1732 x  2440
        public const int WIA_PAGE_ISO_B10 = 27; //   1220 x  1732

        //
        // ISO C page size constants
        //

        public const int WIA_PAGE_ISO_C0 = 28; //  36102 x 51062
        public const int WIA_PAGE_ISO_C1 = 29; //  25511 x 36102
        public const int WIA_PAGE_ISO_C2 = 30; //  18031 x 25511
        public const int WIA_PAGE_ISO_C3 = 31;//  12755 x 18031
        public const int WIA_PAGE_ISO_C4 = 32; //   9015 x 12755 (unfolded)
        public const int WIA_PAGE_ISO_C5 = 33; //   6377 x  9015 (folded once)
        public const int WIA_PAGE_ISO_C6 = 34; //   4488 x  6377 (folded twice)
        public const int WIA_PAGE_ISO_C7 = 35; //   3188 x  4488
        public const int WIA_PAGE_ISO_C8 = 36; //   2244 x  3188
        public const int WIA_PAGE_ISO_C9 = 37; //   1574 x  2244
        public const int WIA_PAGE_ISO_C10 = 38; //   1102 x  1574

        //
        // JIS B page size constants
        //

        public const int WIA_PAGE_JIS_B0 = 39; //  40551 x 57322
        public const int WIA_PAGE_JIS_B1 = 40; //  28661 x 40551
        public const int WIA_PAGE_JIS_B2 = 41; //  20275 x 28661
        public const int WIA_PAGE_JIS_B3 = 42; //  14330 x 20275
        public const int WIA_PAGE_JIS_B4 = 43; //  10118 x 14330
        public const int WIA_PAGE_JIS_B5 = 44; //   7165 x 10118
        public const int WIA_PAGE_JIS_B6 = 45; //   5039 x  7165
        public const int WIA_PAGE_JIS_B7 = 46; //   3582 x  5039
        public const int WIA_PAGE_JIS_B8 = 47; //   2519 x  3582
        public const int WIA_PAGE_JIS_B9 = 48; //   1771 x  2519
        public const int WIA_PAGE_JIS_B10 = 49; //   1259 x  1771

        //
        // JIS A page size constants
        //

        public const int WIA_PAGE_JIS_2A = 50; //  46811 x 66220
        public const int WIA_PAGE_JIS_4A = 51;//  66220 x  93622

    }
}
