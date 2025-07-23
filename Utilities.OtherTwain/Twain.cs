/* Based off of http://www.codeproject.com/Articles/1376/NET-TWAIN-image-scanner */
/* Based off of http://www.codeproject.com/Articles/1376/NET-TWAIN-image-scanner */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Interop;

namespace EdocsUSA.Utilities
{
    public class Twain : IMessageFilter, IDisposable
    {
        #region Interop

        // ------ DSM entry point DAT_ variants:
        [DllImport("Twain_32.dll", EntryPoint = "DSM_Entry")]
        public static extern TWRC DSM_PARENT([In, Out] TW_IDENTITY origin, IntPtr zeroptr, DG dg, DAT dat, MSG msg, ref IntPtr refptr);

        [DllImport("Twain_32.dll", EntryPoint = "DSM_Entry")]
        public static extern TWRC DSM_IDENTITY([In, Out] TW_IDENTITY origin, IntPtr zeroptr, DG dg, DAT dat, MSG msg, [In, Out] TW_IDENTITY idds);

        [DllImport("Twain_32.dll", EntryPoint = "DSM_Entry")]
        public static extern TWRC DSM_STATUS([In, Out] TW_IDENTITY origin, IntPtr zeroptr, DG dg, DAT dat, MSG msg, [In, Out] TW_STATUS dsmstat);

        // ------ DSM entry point DAT_ variants to DS:		
        [DllImport("Twain_32.dll", EntryPoint = "DSM_Entry")]
        public static extern TWRC DS_CUSTOMDSDATA([In, Out] TW_IDENTITY origin, [In, Out] TW_IDENTITY dest, DG dg, DAT dat, MSG msg, [In, Out] TW_CUSTOMDSDATA customData);

        [DllImport("Twain_32.dll", EntryPoint = "DSM_Entry")]
        public static extern TWRC DS_EVENT([In, Out] TW_IDENTITY origin, [In, Out] TW_IDENTITY dest, DG dg, DAT dat, MSG msg, ref TW_EVENT evt);

        [DllImport("Twain_32.dll", EntryPoint = "DSM_Entry")]
        public static extern TWRC DS_STATUS([In, Out] TW_IDENTITY origin, [In] TW_IDENTITY dest, DG dg, DAT dat, MSG msg, [In, Out] TW_STATUS dsmstat);

        [DllImport("Twain_32.dll", EntryPoint = "DSM_Entry")]
        public static extern TWRC DS_CAP([In, Out] TW_IDENTITY origin, [In] TW_IDENTITY dest, DG dg, DAT dat, MSG msg, [In, Out] TW_CAPABILITY capa);

        [DllImport("Twain_32.dll", EntryPoint = "DSM_Entry")]
        public static extern TWRC DS_IMAGEINFO([In, Out] TW_IDENTITY origin, [In] TW_IDENTITY dest, DG dg, DAT dat, MSG msg, [In, Out] TW_IMAGEINFO imginf);

        [DllImport("Twain_32.dll", EntryPoint = "DSM_Entry")]
        public static extern TWRC DS_IMAGENATIVETRANSFER([In, Out] TW_IDENTITY origin, [In] TW_IDENTITY dest, DG dg, DAT dat, MSG msg, ref IntPtr hbitmap);

        [DllImport("Twain_32.dll", EntryPoint = "DSM_Entry")]
        public static extern TWRC DS_PENDINGTRANSFER([In, Out] TW_IDENTITY origin, [In] TW_IDENTITY dest, DG dg, DAT dat, MSG msg, [In, Out] TW_PENDINGXFERS pxfr);

        [DllImport("Twain_32.dll", EntryPoint = "DSM_Entry")]
        public static extern TWRC DS_USERINTERFACE([In, Out] TW_IDENTITY origin, [In, Out] TW_IDENTITY dest, DG dg, DAT dat, MSG msg, [In, Out] TW_USERINTERFACE guif);

        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry")]
        internal static extern TWRC DSM_DataSource([In, Out] TW_IDENTITY appIdentity, IntPtr pZero, DG dg, DAT dat, MSG msg, [In, Out] TW_IDENTITY dsIdentity);

        #endregion Interop

        #region Enums

        public enum TWCY : short
        {
            AFGHANISTAN = 1001,
            ALGERIA = 213,
            AMERICANSAMOA = 684,
            ANDORRA = 27,
            ANGOLA = 1002,
            ANGUILLA = 8090,
            ANTIGUA = 8091,
            ARGENTINA = 54,
            ARUBA = 297,
            ASCENSIONI = 247,
            AUSTRALIA = 61,
            AUSTRIA = 43,
            BAHAMAS = 8092,
            BAHRAIN = 973,
            BANGLADESH = 880,
            BARBADOS = 8093,
            BELGIUM = 32,
            BELIZE = 501,
            BENIN = 229,
            BERMUDA = 8094,
            BHUTAN = 1003,
            BOLIVIA = 591,
            BOTSWANA = 267,
            BRITAIN = 6,
            BRITVIRGINIS = 8095,
            BRAZIL = 55,
            BRUNEI = 673,
            BULGARIA = 359,
            BURKINAFASO = 1004,
            BURMA = 1005,
            BURUNDI = 1006,
            CAMAROON = 237,
            CANADA = 2,
            CAPEVERDEIS = 238,
            CAYMANIS = 8096,
            CENTRALAFREP = 1007,
            CHAD = 1008,
            CHILE = 56,
            CHINA = 86,
            CHRISTMASIS = 1009,
            COCOSIS = 1009,
            COLOMBIA = 57,
            COMOROS = 1010,
            CONGO = 1011,
            COOKIS = 1012,
            COSTARICA = 506,
            CUBA = 5,
            CYPRUS = 357,
            CZECHOSLOVAKIA = 42,
            DENMARK = 45,
            DJIBOUTI = 1013,
            DOMINICA = 8097,
            DOMINCANREP = 8098,
            EASTERIS = 1014,
            ECUADOR = 593,
            EGYPT = 20,
            ELSALVADOR = 503,
            EQGUINEA = 1015,
            ETHIOPIA = 251,
            FALKLANDIS = 1016,
            FAEROEIS = 298,
            FIJIISLANDS = 679,
            FINLAND = 358,
            FRANCE = 33,
            FRANTILLES = 596,
            FRGUIANA = 594,
            FRPOLYNEISA = 689,
            FUTANAIS = 1043,
            GABON = 241,
            GAMBIA = 220,
            GERMANY = 49,
            GHANA = 233,
            GIBRALTER = 350,
            GREECE = 30,
            GREENLAND = 299,
            GRENADA = 8099,
            GRENEDINES = 8015,
            GUADELOUPE = 590,
            GUAM = 671,
            GUANTANAMOBAY = 5399,
            GUATEMALA = 502,
            GUINEA = 224,
            GUINEABISSAU = 1017,
            GUYANA = 592,
            HAITI = 509,
            HONDURAS = 504,
            HONGKONG = 852,
            HUNGARY = 36,
            ICELAND = 354,
            INDIA = 91,
            INDONESIA = 62,
            IRAN = 98,
            IRAQ = 964,
            IRELAND = 353,
            ISRAEL = 972,
            ITALY = 39,
            IVORYCOAST = 225,
            JAMAICA = 8010,
            JAPAN = 81,
            JORDAN = 962,
            KENYA = 254,
            KIRIBATI = 1018,
            KOREA = 82,
            KUWAIT = 965,
            LAOS = 1019,
            LEBANON = 1020,
            LIBERIA = 231,
            LIBYA = 218,
            LIECHTENSTEIN = 41,
            LUXENBOURG = 352,
            MACAO = 853,
            MADAGASCAR = 1021,
            MALAWI = 265,
            MALAYSIA = 60,
            MALDIVES = 960,
            MALI = 1022,
            MALTA = 356,
            MARSHALLIS = 692,
            MAURITANIA = 1023,
            MAURITIUS = 230,
            MEXICO = 3,
            MICRONESIA = 691,
            MIQUELON = 508,
            MONACO = 33,
            MONGOLIA = 1024,
            MONTSERRAT = 8011,
            MOROCCO = 212,
            MOZAMBIQUE = 1025,
            NAMIBIA = 264,
            NAURU = 1026,
            NEPAL = 977,
            NETHERLANDS = 31,
            NETHANTILLES = 599,
            NEVIS = 8012,
            NEWCALEDONIA = 687,
            NEWZEALAND = 64,
            NICARAGUA = 505,
            NIGER = 227,
            NIGERIA = 234,
            NIUE = 1027,
            NORFOLKI = 1028,
            NORWAY = 47,
            OMAN = 968,
            PAKISTAN = 92,
            PALAU = 1029,
            PANAMA = 507,
            PARAGUAY = 595,
            PERU = 51,
            PHILLIPPINES = 63,
            PITCAIRNIS = 1030,
            PNEWGUINEA = 675,
            POLAND = 48,
            PORTUGAL = 351,
            QATAR = 974,
            REUNIONI = 1031,
            ROMANIA = 40,
            RWANDA = 250,
            SAIPAN = 670,
            SANMARINO = 39,
            SAOTOME = 1033,
            SAUDIARABIA = 966,
            SENEGAL = 221,
            SEYCHELLESIS = 1034,
            SIERRALEONE = 1035,
            SINGAPORE = 65,
            SOLOMONIS = 1036,
            SOMALI = 1037,
            SOUTHAFRICA = 27,
            SPAIN = 34,
            SRILANKA = 94,
            STHELENA = 1032,
            STKITTS = 8013,
            STLUCIA = 8014,
            STPIERRE = 508,
            STVINCENT = 8015,
            SUDAN = 1038,
            SURINAME = 597,
            SWAZILAND = 268,
            SWEDEN = 46,
            SWITZERLAND = 41,
            SYRIA = 1039,
            TAIWAN = 886,
            TANZANIA = 255,
            THAILAND = 66,
            TOBAGO = 8016,
            TOGO = 228,
            TONGAIS = 676,
            TRINIDAD = 8016,
            TUNISIA = 216,
            TURKEY = 90,
            TURKSCAICOS = 8017,
            TUVALU = 1040,
            UGANDA = 256,
            USSR = 7,
            UAEMIRATES = 971,
            UNITEDKINGDOM = 44,
            USA = 1,
            URUGUAY = 598,
            VANUATU = 1041,
            VATICANCITY = 39,
            VENEZUELA = 58,
            WAKE = 1042,
            WALLISIS = 1043,
            WESTERNSAHARA = 1044,
            WESTERNSAMOA = 1045,
            YEMEN = 1046,
            YUGOSLAVIA = 38,
            ZAIRE = 243,
            ZAMBIA = 260,
            ZIMBABWE = 263,
            ALBANIA = 355,
            ARMENIA = 374,
            AZERBAIJAN = 994,
            BELARUS = 375,
            BOSNIAHERZGO = 387,
            CAMBODIA = 855,
            CROATIA = 385,
            CZECHREPUBLIC = 420,
            DIEGOGARCIA = 246,
            ERITREA = 291,
            ESTONIA = 372,
            GEORGIA = 995,
            LATVIA = 371,
            LESOTHO = 266,
            LITHUANIA = 370,
            MACEDONIA = 389,
            MAYOTTEIS = 269,
            MOLDOVA = 373,
            MYANMAR = 95,
            NORTHKOREA = 850,
            PUERTORICO = 787,
            RUSSIA = 7,
            SERBIA = 381,
            SLOVAKIA = 421,
            SLOVENIA = 386,
            SOUTHKOREA = 82,
            UKRAINE = 380,
            USVIRGINIS = 340,
            VIETNAM = 84
        }

        public enum TWLG : short
        {
            USERLOCALE = (-1),
            DAN = 0,
            DUT = 1,
            ENG = 2,
            FCF = 3,
            FIN = 4,
            FRN = 5,
            GER = 6,
            ICE = 7,
            ITN = 8,
            NOR = 9,
            POR = 10,
            SPA = 11,
            SWE = 12,
            USA = 13,
            AFRIKAANS = 14,
            ALBANIA = 15,
            ARABIC = 16,
            ARABIC_ALGERIA = 17,
            ARABIC_BAHRAIN = 18,
            ARABIC_EGYPT = 19,
            ARABIC_IRAQ = 20,
            ARABIC_JORDAN = 21,
            ARABIC_KUWAIT = 22,
            ARABIC_LEBANON = 23,
            ARABIC_LIBYA = 24,
            ARABIC_MOROCCO = 25,
            ARABIC_OMAN = 26,
            ARABIC_QATAR = 27,
            ARABIC_SAUDIARABIA = 28,
            ARABIC_SYRIA = 29,
            ARABIC_TUNISIA = 30,
            ARABIC_UAE = 31,
            ARABIC_YEMEN = 32,
            BASQUE = 33,
            BYELORUSSIAN = 34,
            BULGARIAN = 35,
            CATALAN = 36,
            CHINESE = 37,
            CHINESE_HONGKONG = 38,
            CHINESE_PRC = 39,
            CHINESE_SINGAPORE = 40,
            CHINESE_SIMPLIFIED = 41,
            CHINESE_TAIWAN = 42,
            CHINESE_TRADITIONAL = 43,
            CROATIA = 44,
            CZECH = 45,
            DANISH = DAN,
            DUTCH = DUT,
            DUTCH_BELGIAN = 46,
            ENGLISH = ENG,
            ENGLISH_AUSTRALIAN = 47,
            ENGLISH_CANADIAN = 48,
            ENGLISH_IRELAND = 49,
            ENGLISH_NEWZEALAND = 50,
            ENGLISH_SOUTHAFRICA = 51,
            ENGLISH_UK = 52,
            ENGLISH_USA = USA,
            ESTONIAN = 53,
            FAEROESE = 54,
            FARSI = 55,
            FINNISH = FIN,
            FRENCH = FRN,
            FRENCH_BELGIAN = 56,
            FRENCH_CANADIAN = FCF,
            FRENCH_LUXEMBOURG = 57,
            FRENCH_SWISS = 58,
            GERMAN = GER,
            GERMAN_AUSTRIAN = 59,
            GERMAN_LUXEMBOURG = 60,
            GERMAN_LIECHTENSTEIN = 61,
            GERMAN_SWISS = 62,
            GREEK = 63,
            HEBREW = 64,
            HUNGARIAN = 65,
            ICELANDIC = ICE,
            INDONESIAN = 66,
            ITALIAN = ITN,
            ITALIAN_SWISS = 67,
            JAPANESE = 68,
            KOREAN = 69,
            KOREAN_JOHAB = 70,
            LATVIAN = 71,
            LITHUANIAN = 72,
            NORWEGIAN = NOR,
            NORWEGIAN_BOKMAL = 73,
            NORWEGIAN_NYNORSK = 74,
            POLISH = 75,
            PORTUGUESE = POR,
            PORTUGUESE_BRAZIL = 76,
            ROMANIAN = 77,
            RUSSIAN = 78,
            SERBIAN_LATIN = 79,
            SLOVAK = 80,
            SLOVENIAN = 81,
            SPANISH = SPA,
            SPANISH_MEXICAN = 82,
            SPANISH_MODERN = 83,
            SWEDISH = SWE,
            THAI = 84,
            TURKISH = 85,
            UKRANIAN = 86,
            ASSAMESE = 87,
            BENGALI = 88,
            BIHARI = 89,
            BODO = 90,
            DOGRI = 91,
            GUJARATI = 92,
            HARYANVI = 93,
            HINDI = 94,
            KANNADA = 95,
            KASHMIRI = 96,
            MALAYALAM = 97,
            MARATHI = 98,
            MARWARI = 99,
            MEGHALAYAN = 100,
            MIZO = 101,
            NAGA = 102,
            ORISSI = 103,
            PUNJABI = 104,
            PUSHTU = 105,
            SERBIAN_CYRILLIC = 106,
            SIKKIMI = 107,
            SWEDISH_FINLAND = 108,
            TAMIL = 109,
            TELUGU = 110,
            TRIPURI = 111,
            URDU = 112,
            VIETNAMESE = 113
        }

        [Flags]
        public enum DG : short
        {
            Control = 0x0001,
            Image = 0x0002,
            Audio = 0x0004
        }

        public enum DAT : short
        {
            Null = 0x0000,
            Capability = 0x0001,
            Event = 0x0002,
            Identity = 0x0003,
            Parent = 0x0004,
            PendingXfers = 0x0005,
            SetupMemXfer = 0x0006,
            SetupFileXfer = 0x0007,
            Status = 0x0008,
            UserInterface = 0x0009,
            XferGroup = 0x000a,
            TwunkIdentity = 0x000b,
            CustomDSData = 0x000c,
            DeviceEvent = 0x000d,
            FileSystem = 0x000e,
            PassThru = 0x000f,

            ImageInfo = 0x0101,
            ImageLayout = 0x0102,
            ImageMemXfer = 0x0103,
            ImageNativeXfer = 0x0104,
            ImageFileXfer = 0x0105,
            CieColor = 0x0106,
            GrayResponse = 0x0107,
            RGBResponse = 0x0108,
            JpegCompression = 0x0109,
            Palette8 = 0x010a,
            ExtImageInfo = 0x010b,

            SetupFileXfer2 = 0x0301
        }

        public enum MSG : short
        {
            Null = 0x0000,
            Get = 0x0001,
            GetCurrent = 0x0002,
            GetDefault = 0x0003,
            GetFirst = 0x0004,
            GetNext = 0x0005,
            Set = 0x0006,
            Reset = 0x0007,
            QuerySupport = 0x0008,

            XFerReady = 0x0101,
            CloseDSReq = 0x0102,
            CloseDSOK = 0x0103,
            DeviceEvent = 0x0104,

            CheckStatus = 0x0201,

            OpenDSM = 0x0301,
            CloseDSM = 0x0302,

            OpenDS = 0x0401,
            CloseDS = 0x0402,
            UserSelect = 0x0403,

            DisableDS = 0x0501,
            EnableDS = 0x0502,
            EnableDSUIOnly = 0x0503,

            ProcessEvent = 0x0601,

            EndXfer = 0x0701,
            StopFeeder = 0x0702,

            ChangeDirectory = 0x0801,
            CreateDirectory = 0x0802,
            Delete = 0x0803,
            FormatMedia = 0x0804,
            GetClose = 0x0805,
            GetFirstFile = 0x0806,
            GetInfo = 0x0807,
            GetNextFile = 0x0808,
            Rename = 0x0809,
            Copy = 0x080A,
            AutoCaptureDir = 0x080B,

            PassThru = 0x0901
        }

        public enum TWRC : short
        {
            Success = 0x0000,
            Failure = 0x0001,
            CheckStatus = 0x0002,
            Cancel = 0x0003,
            DSEvent = 0x0004,
            NotDSEvent = 0x0005,
            XferDone = 0x0006,
            EndOfList = 0x0007,
            InfoNotSupported = 0x0008,
            DataNotAvailable = 0x0009
        }

        public enum TWCC : short
        {
            Success = 0x0000,
            Bummer = 0x0001,
            LowMemory = 0x0002,
            NoDS = 0x0003,
            MaxConnections = 0x0004,
            OperationError = 0x0005,
            BadCap = 0x0006,
            BadProtocol = 0x0009,
            BadValue = 0x000a,
            SeqError = 0x000b,
            BadDest = 0x000c,
            CapUnsupported = 0x000d,
            CapBadOperation = 0x000e,
            CapSeqError = 0x000f,
            Denied = 0x0010,
            FileExists = 0x0011,
            FileNotFound = 0x0012,
            NotEmpty = 0x0013,
            PaperJam = 0x0014,
            PaperDoubleFeed = 0x0015,
            FileWriteError = 0x0016,
            CheckDeviceOnline = 0x0017
        }

        public enum TWON : short
        {
            Array = 0x0003,
            Enum = 0x0004,
            One = 0x0005,
            Range = 0x0006,
            DontCare = -1
        }

        public enum TWTY : short
        {
            Int8 = 0x0000,
            Int16 = 0x0001,
            Int32 = 0x0002,
            UInt8 = 0x0003,
            UInt16 = 0x0004,
            UInt32 = 0x0005,
            Bool = 0x0006,
            Fix32 = 0x0007,
            Frame = 0x0008,
            Str32 = 0x0009,
            Str64 = 0x000a,
            Str128 = 0x000b,
            Str255 = 0x000c,
            Str1024 = 0x000d,
            Str512 = 0x000e
        }

        public enum CAP : short
        {
            XferCount = 0x0001,         // CAP_XFERCOUNT
            ICompression = 0x0100,          // ICAP_...
            IPixelType = 0x0101,
            IUnits = 0x0102,
            IXferMech = 0x0103
        }

        #endregion Enums

        #region Data Types

        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
        public class TW_IDENTITY
        {
            public IntPtr Id;
            public TW_VERSION Version;
            public short ProtocolMajor;
            public short ProtocolMinor;
            public int SupportedGroups;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)]
            public string Manufacturer;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)]
            public string ProductFamily;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)]
            public string ProductName;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
        public class TW_VERSION
        {
            public short MajorNum;
            public short MinorNum;
            public short Language;
            public short Country;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)]
            public string Info;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public class TW_USERINTERFACE
        {
            public short ShowUI;                // bool is strictly 32 bit, so use short
            public short ModalUI;
            public IntPtr ParentHand;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public class TW_STATUS
        {
            public short ConditionCode;     // TwCC
            public short Reserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_EVENT
        {
            public IntPtr EventPtr;
            public short Message;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public class TW_IMAGEINFO
        {
            public int XResolution;
            public int YResolution;
            public int ImageWidth;
            public int ImageLength;
            public short SamplesPerPixel;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public short[] BitsPerSample;
            public short BitsPerPixel;
            public short Planar;
            public short PixelType;
            public short Compression;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public class TW_PENDINGXFERS
        {
            public short Count;
            public int EOJ;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TW_FIX32
        {                                               // TW_FIX32
            public short Whole;
            public ushort Frac;

            public float ToFloat()
            {
                return (float)Whole + ((float)Frac / 65536.0f);
            }

            public void FromFloat(float f)
            {
                int i = (int)((f * 65536.0f) + 0.5f);
                Whole = (short)(i >> 16);
                Frac = (ushort)(i & 0x0000ffff);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public class TW_CAPABILITY
        {
            public short Cap;
            public short ConType;
            public IntPtr Handle;

            public TW_CAPABILITY(CAP cap)
            {
                Cap = (short)cap;
                ConType = -1;
            }

            public TW_CAPABILITY(CAP cap, short sval)
            {
                Cap = (short)cap;
                ConType = (short)TWON.One;
                Handle = Kernel32.GlobalAlloc(0x42, 6);
                IntPtr pv = Kernel32.GlobalLock(Handle);
                Marshal.WriteInt16(pv, 0, (short)TWTY.Int16);
                Marshal.WriteInt32(pv, 2, (int)sval);
                Kernel32.GlobalUnlock(Handle);
            }

            ~TW_CAPABILITY()
            {
                if (Handle != IntPtr.Zero)
                    Kernel32.GlobalFree(Handle);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public class TW_CUSTOMDSDATA
        {
            public UInt32 InfoLength;
            public IntPtr hData;
        }

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

        [StructLayout(LayoutKind.Explicit)]
        struct LowHighInt
        {
            [FieldOffset(0)]
            public int Number;

            [FieldOffset(0)]
            public short Low;

            [FieldOffset(2)]
            public short High;
        }

        public class TwainException : Exception
        {
            public string Description { get; set; }

            public TWRC? ReturnCode { get; set; }

            public TWCC? ConditionCode { get; set; }

            public override string Message
            {
                get
                {
                    Logging.TraceLogger.TraceLoggerInstance.TraceError("A twain error has occurred:" + "Description: " + Description + "Condition Code: " + (ConditionCode.HasValue ? ConditionCode.Value.ToString() : "null") + "Return Code: " + (ReturnCode.HasValue ? ReturnCode.Value.ToString() : "null"));
                    return "A twain error has occurred:"
                        + Environment.NewLine + "Description: " + Description
                        + Environment.NewLine + "Condition Code: " + (ConditionCode.HasValue ? ConditionCode.Value.ToString() : "null")
                        + Environment.NewLine + "Return Code: " + (ReturnCode.HasValue ? ReturnCode.Value.ToString() : "null");
                }
            }

            public TwainException(string description, TWRC? returnCode, TWCC? conditionCode)
            {

                Description = description;
                ConditionCode = conditionCode;
                ReturnCode = returnCode;
                Logging.TraceLogger.TraceLoggerInstance.TraceError($"Twain exception Description:{Description.ToString()} ConditionCode:{ConditionCode.ToString()} ReturnCode:{ReturnCode.ToString()}");

            }

            public TwainException(string description) : this(description, null, null) { }
        }

        #endregion Data Types

        #region Properties

        public const short TWON_PROTOCOLMAJOR = 1;
        public const short TWON_PROTOCOLMINOR = 9;

        protected IntPtr AppHwnd;

        protected TW_IDENTITY AppId;

        protected Dictionary<string, TW_IDENTITY> DataSources = new Dictionary<string, TW_IDENTITY>();

        public string[] DataSourcesNames
        {
            get
            {
                if (DataSources == null)
                {
                    Logging.TraceLogger.TraceLoggerInstance.TraceError("Sources has not been initialized");
                    throw new InvalidOperationException("Sources has not been initialized");
                }
                return DataSources.Keys.ToArray();
            }
        }

        protected TW_IDENTITY ActiveDataSource;

        public string ActiveDataSourceName
        { get { return (ActiveDataSource == null ? null : ActiveDataSource.ProductName); } }

        protected EventWaiter XferReadyWaiter = new EventWaiter();
        protected EventWaiter DeviceUICanceledWaiter = new EventWaiter();
        protected EventWaiter DeviceUIClosedWaiter = new EventWaiter();

        #endregion Properties

        #region Events

        public event EventHandler DataSourcesChanged;

        protected void NotifyDataSourcesChanged()
        { if (DataSourcesChanged != null) DataSourcesChanged(this, null); }

        public event EventHandler ActiveDataSourceChanged;

        protected void NotifyActiveDataSourceChanged()
        { if (ActiveDataSourceChanged != null) ActiveDataSourceChanged(this, null); }

        #endregion Events

        #region Constructors, Initializers, Destructors

        public Twain(IntPtr appHwnd, string manufacturer, string productFamily, string productName)
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Init twain manufacturer:{manufacturer} productFamily:{productFamily} productName:{productName}");
            AppHwnd = appHwnd;

            Application.AddMessageFilter(this);

            AppId = new TW_IDENTITY()
            {
                Id = IntPtr.Zero,
                Manufacturer = manufacturer,
                ProductFamily = productFamily,
                ProductName = productName,
                ProtocolMajor = TWON_PROTOCOLMAJOR,
                ProtocolMinor = TWON_PROTOCOLMINOR,
                SupportedGroups = (int)(DG.Image | DG.Control),
                Version = new TW_VERSION()
                {
                    Country = (short)TWCY.USA,
                    Language = (short)TWLG.USERLOCALE,
                    Info = string.Empty
                }
            };

            Init();
        }
        public Twain()
        { }
        //TODO:Rename?
        protected void Init()
        {
            OpenDataSourceManager();
            RefreshDataSources();
        }

        //TODO:Rename?
        protected void Close()
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Closing Twain");
            CloseActiveDataSource(true);
            CloseDataSourceManager(true);
        }

        protected void Reset()
        {
            Close();
            Init();
        }

        ~Twain() { Dispose(false); }

        #endregion Constructors, Initializers, Destructors

        #region Opening/Closing Data Source Manager

        protected void OpenDataSourceManager()
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Opening data source manager");
            TWRC r = DSM_PARENT(AppId, IntPtr.Zero, DG.Control, DAT.Parent, MSG.OpenDSM, ref AppHwnd);
            if (r == TWRC.Success)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Data source manager is open");
            }
            else
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Error opening data source manager {r} {GetConditionCode().ToString()}");
                throw new TwainException("Error opening data source manager", r, GetConditionCode());
            }
        }

        protected void CloseDataSourceManager(bool suppressError)
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Closing data source manager");
            TWRC r = DSM_PARENT(AppId, IntPtr.Zero, DG.Control, DAT.Parent, MSG.CloseDSM, ref AppHwnd);
            if (r == TWRC.Success)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Data source manager is closed");
            }
            else
            {
                TwainException ex = new TwainException("Error closing data source manager", r, GetConditionCode());
                Logging.TraceLogger.TraceLoggerInstance.TraceError($"Closing data souce:{ex.Message}");
                if (suppressError)
                {
                    Logging.TraceLogger.TraceLoggerInstance.TraceError(ex.Message);
                }
                else
                {
                    Logging.TraceLogger.TraceLoggerInstance.TraceError(ex.Message);
                    throw ex;
                }
            }
        }

        #endregion Opening/Closing Data Source Manager

        #region Condition Code

        /// <summary>Get the current twain condition code of the device (if available)</summary>
        protected TWCC? GetConditionCode()
        {
            TW_STATUS status = new TW_STATUS();
            TWRC r = DSM_STATUS(AppId, IntPtr.Zero, DG.Control, DAT.Status, MSG.Get, status);
            if (r == TWRC.Success)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Twain condition code:{status.ConditionCode.ToString()}");
                return (TWCC?)(status.ConditionCode);
            }
            else
            {
                TwainException ex = new TwainException("Error getting condition code", r, null);
                Logging.TraceLogger.TraceLoggerInstance.TraceError(ex.Message);
                return null;
            }
        }

        #endregion Condition Code

        #region Data Source

        ///<summary>Retrieve the default data source</summary>
        protected TW_IDENTITY GetDefaultDataSource()
        {
            TW_IDENTITY defaultDataSource = new TW_IDENTITY();
            TWRC r = DSM_IDENTITY(AppId, IntPtr.Zero, DG.Control, DAT.Identity, MSG.GetDefault, defaultDataSource);
            if (r == TWRC.Success) return defaultDataSource;
            else
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceError($"Error getting default data source {r.ToString()} conditiocode:{GetConditionCode().ToString()}");
                throw new TwainException("Error getting default data source", r, GetConditionCode());

            }

        }

        /// <summary>Refresh the list of data sources.</summary>
        /// <remarks>Attempts to restore the previous active data source.</remarks>
        protected void RefreshDataSources()
        {
            string previousActiveDataSourceName = (ActiveDataSource == null ? null : ActiveDataSource.ProductName);

            DataSources.Clear();
            bool firstSource = true;
            TWRC r;
            do
            {
                MSG msg = (firstSource ? MSG.GetFirst : MSG.GetNext);
                TW_IDENTITY currentDataSource = new TW_IDENTITY();
                r = DSM_IDENTITY(AppId, IntPtr.Zero, DG.Control, DAT.Identity, msg, currentDataSource);
                if (r == TWRC.Success)
                {
                    Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Adding twain device " + currentDataSource.ProductName);
                    DataSources[currentDataSource.ProductName] = currentDataSource;
                    firstSource = false;
                }
            } while (r != TWRC.EndOfList);

            //If there was a previous active source and it still exists, reset it,
            //Otherwise, use the system default.
            if (previousActiveDataSourceName.IsNotEmpty() && DataSources.ContainsKey(previousActiveDataSourceName))
            { SetActiveDataSource(previousActiveDataSourceName); }
            else SetActiveDataSource(GetDefaultDataSource().ProductName);
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Twain initalized, active data source is " + ActiveDataSource.ProductName);

            NotifyDataSourcesChanged();
        }

        public bool DataSourceExists(string dataSourceName)
        { return DataSourcesNames.Contains(dataSourceName); }

        public void EnsureDataSourceExists(string dataSourceName)
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Checking twain datasorce:{dataSourceName} exists");

            if (DataSourceExists(dataSourceName))
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceError("The specified data source " + dataSourceName + " does not exist");
                throw new InvalidOperationException("The specified data source " + dataSourceName + " does not exist");
            }
        }

        public void SetActiveDataSource()
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Prompting user to select active data source");
            TW_IDENTITY dataSource = new TW_IDENTITY();
            TWRC r = DSM_IDENTITY(AppId, IntPtr.Zero, DG.Control, DAT.Identity, MSG.UserSelect, dataSource);
            if (r == TWRC.Success)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Source selected " + dataSource.ProductName);
                SetActiveDataSource(dataSource.ProductName);
            }

            else if (r == TWRC.Cancel)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceWarning("User canceled selection active twain datasource");
                throw new OperationCanceledException();
            }
            else
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceError($"Error setting active data source {r.ToString()} condition code:{GetConditionCode().ToString()}");
                throw new TwainException("Error setting active data source", r, GetConditionCode());
            }

        }

        public void SetActiveDataSource(string name)
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Setting active data source to " + name);
            if (ActiveDataSource != null)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Closing active data source:{ActiveDataSource.ProductName}");
                CloseActiveDataSource(true);
            }
            if (DataSources.ContainsKey(name) == false)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceError("Data source " + name + " could not be found.  Make sure the device is connected and turned on");
                throw new TwainException("Data source " + name + " could not be found.  Make sure the device is connected and turned on");
            }
            ActiveDataSource = DataSources[name];
            //OpenActiveDataSource();
            NotifyActiveDataSourceChanged();
        }
        /*
		public void OpenActiveDataSource()
		{
			Debug.WriteLine("Opening active data source");
			TWRC r = DSM_IDENTITY(AppId, IntPtr.Zero, DG.Control, DAT.Identity, MSG.OpenDS, ActiveDataSource);
			if (r != TWRC.Success) throw new TwainException("Error opening active data source", r, GetConditionCode());
		}
		
		public void CloseActiveDataSource(bool suppressError)
		{
			Debug.WriteLine("Closing active data source");
			TWRC r = DSM_IDENTITY(AppId, IntPtr.Zero, DG.Control, DAT.Identity, MSG.CloseDS, ActiveDataSource);
			if (r == TWRC.Success) 
			{ Debug.WriteLine("Active data source closed"); }
			else
			{
				TwainException ex = new TwainException("Error closing source", r, GetConditionCode());
				if (suppressError) Trace.TraceError(ex.Message);
				else throw ex;
			}
		}
		*/

        bool ActiveDataSourceOpen = false;

        public void OpenActiveDataSource()
        {

            if (ActiveDataSourceOpen == false)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Opening active data source");
                TWRC r = DSM_IDENTITY(AppId, IntPtr.Zero, DG.Control, DAT.Identity, MSG.OpenDS, ActiveDataSource);
                if (r != TWRC.Success)
                {
                    Logging.TraceLogger.TraceLoggerInstance.TraceError($"Error opening active data source {r.ToString()} condition code:{GetConditionCode().ToString()}");
                    throw new TwainException("Error opening active data source", r, GetConditionCode());
                }
                ActiveDataSourceOpen = true;
            }
            else
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceWarning($"Active data source:{ActiveDataSource.ProductName} was already open");

            }

        }

        public void CloseActiveDataSource(bool suppressError)
        {
            if (ActiveDataSourceOpen == false)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceWarning($"Data source already opened:");
                return;
            }

            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Closing active data source");
            TWRC r = DSM_IDENTITY(AppId, IntPtr.Zero, DG.Control, DAT.Identity, MSG.CloseDS, ActiveDataSource);
            if (r == TWRC.Success)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceWarning("Active data source closed");
                ActiveDataSourceOpen = false;
            }
            else
            {
                TwainException ex = new TwainException("Error closing source", r, GetConditionCode());
                Logging.TraceLogger.TraceLoggerInstance.TraceError(ex.Message);
                if (suppressError) Logging.TraceLogger.TraceLoggerInstance.TraceError(ex.Message);
                else throw ex;
            }
        }


        public void EnableActiveDataSource(bool showUI, bool forAcquire)
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Enabling active data source showui:{showUI.ToString()} forAcwuire:{forAcquire.ToString()}");
            TW_USERINTERFACE userInterface = new TW_USERINTERFACE()
            {
                ShowUI = (short)((showUI ? 1 : 0)),
                ModalUI = 1, //0 will cause access violation
                ParentHand = AppHwnd
            };
            MSG msg = (forAcquire ? MSG.EnableDS : MSG.EnableDSUIOnly);
            TWRC r = DS_USERINTERFACE(AppId, ActiveDataSource, DG.Control, DAT.UserInterface, msg, userInterface);
            // r = DSM_DataSource(AppId, IntPtr.Zero, DG.Control, DAT.UserInterface, msg, AppId);

            if (r == TWRC.Success)
                Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Active data source enabled");
            else
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceError($"Error enabling active data source {r.ToString()} condition code:{GetConditionCode().ToString()}");
                throw new TwainException("Error enabling active data source", r, GetConditionCode());
            }
        }

        public void DisableActiveDataSource(bool suppressError)
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Disabling active data source");
            TW_USERINTERFACE userInterface = new TW_USERINTERFACE();
            TWRC r = DS_USERINTERFACE(AppId, ActiveDataSource, DG.Control, DAT.UserInterface, MSG.DisableDS, userInterface);
            if (r == TWRC.Success) Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Active data source disabled:{ActiveDataSource.ProductName}");
            else
            {
                Twain.TW_PENDINGXFERS pxfer = new Twain.TW_PENDINGXFERS();
                pxfer.Count = 0;
                pxfer.EOJ = 0;
                EndCurrentTransfer(pxfer);
                TwainException ex = new TwainException("Error disabling active data source", r, null);
                Logging.TraceLogger.TraceLoggerInstance.TraceError(ex.Message);
                if (suppressError) Logging.TraceLogger.TraceLoggerInstance.TraceError(ex.Message);
                else throw ex;
            }
        }

        #endregion Data Source

        #region Capabilities

        public byte[] GetCustomDSData(byte[] customDSData)
        {
            OpenActiveDataSource();
            try
            {
                if ((customDSData != null) && (customDSData.Length > 0))
                { SetCustomDSData(customDSData); }

                EventWaiter.Reset(DeviceUIClosedWaiter, DeviceUICanceledWaiter);
                EnableActiveDataSource(true, false);
                try
                {
                    EventWaiter waiter = EventWaiter.Wait(DeviceUIClosedWaiter, DeviceUICanceledWaiter);
                    if (waiter == DeviceUICanceledWaiter)
                    {
                        Logging.TraceLogger.TraceLoggerInstance.TraceError("Device cancled waiter");
                        throw new OperationCanceledException();
                    }
                }
                finally { DisableActiveDataSource(false); }

                TW_CUSTOMDSDATA data = new TW_CUSTOMDSDATA();
                TWRC r = DS_CUSTOMDSDATA(AppId, ActiveDataSource, DG.Control, DAT.CustomDSData, MSG.Get, data);
                if (r == TWRC.Success)
                {
                    IntPtr pData = Kernel32.GlobalLock(data.hData);

                    byte[] dataBytes = new byte[data.InfoLength];
                    for (int pos = 0; pos < data.InfoLength; pos++)
                    { dataBytes[pos] = Marshal.ReadByte(pData, pos); }
                    Kernel32.GlobalFree(data.hData);

                    return dataBytes;

                }
                else
                {
                    Logging.TraceLogger.TraceLoggerInstance.TraceError($"Error getting custom DS data {r.ToString()} condition code:{GetConditionCode().ToString()}");
                    throw new TwainException("Error getting custom DS data", r, GetConditionCode());
                }
            }
            finally { CloseActiveDataSource(false); }
        }

        public void GetCustomDSData()
        {
            OpenActiveDataSource();
            try
            {

                EventWaiter.Reset(DeviceUIClosedWaiter, DeviceUICanceledWaiter);
                EnableActiveDataSource(true, false);
                try
                {
                    EventWaiter waiter = EventWaiter.Wait(DeviceUIClosedWaiter, DeviceUICanceledWaiter);
                    if (waiter == DeviceUICanceledWaiter)
                    {
                        Logging.TraceLogger.TraceLoggerInstance.TraceError("Device cancled waiter");
                        throw new OperationCanceledException();
                    }
                }
                finally { DisableActiveDataSource(false); }


            }
            finally { CloseActiveDataSource(false); }
        }
        protected void SetCustomDSDataXmlFile(byte[] dataBytes)
        {
            Edocs_Utilities.EdocsUtilitiesInstance.CloseRunning(SettingsManager.CloseProcess);
            Edocs_Utilities.EdocsUtilitiesInstance.DeleteFiles(SettingsManager.TwainSettingsCacheFolder, "*.*");
            if (!(SettingsManager.UpDatedCustomDataFile))
            {
                string settingsFiles = Encoding.Default.GetString(dataBytes);
                foreach (var files in settingsFiles.Split(';'))
                {
                    if (!(string.IsNullOrWhiteSpace(files)))
                    {
                        string[] sourceDestFile = files.Split(',');
                        if (sourceDestFile.Count() == 2)
                        {
                            Edocs_Utilities.EdocsUtilitiesInstance.CopyFile(sourceDestFile[0], sourceDestFile[1], true, false);
                        }
                    }
                }

            }
            SettingsManager.UpDatedCustomDataFile = true;

        }
        protected void SetCustomDSData(byte[] dataBytes)
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Setting custom DS Data");
            //  SetCustomDSDataXmlFile(dataBytes);
            IntPtr pData = Marshal.AllocHGlobal(dataBytes.Length);
            for (int pos = 0; pos < dataBytes.Length; pos++)
            { Marshal.WriteByte(pData, pos, dataBytes[pos]); }

            TW_CUSTOMDSDATA customDSData = new TW_CUSTOMDSDATA()
            {
                InfoLength = (uint)dataBytes.Length,
                hData = pData
            };
            TWRC r = DS_CUSTOMDSDATA(AppId, ActiveDataSource, DG.Control, DAT.CustomDSData, MSG.Set, customDSData);

            Marshal.FreeHGlobal(pData);

            if (r == TWRC.Success) Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Custom DS Data set");
            else
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceError($"Error setting custom DS data {r.ToString()} condition code:{GetConditionCode().ToString()}");
                SetCustomDSDataXmlFile(dataBytes);
                //   throw new TwainException("Error setting custom DS data", r, GetConditionCode());
            }
        }

        protected void SetCapOneValue(CAP cap, short value)
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Setting capability:{cap.ToString()} to vlaue:{value.ToString()}");

            TW_CAPABILITY capOneValue = new TW_CAPABILITY(cap, value);

            IntPtr pValue = Kernel32.GlobalLock(capOneValue.Handle);
            Marshal.WriteInt16(pValue, 0, ((Int16)TWTY.Int16));
            Marshal.WriteInt32(pValue, 2, ((Int32)value));
            Kernel32.GlobalUnlock(capOneValue.Handle);

            TWRC r = DS_CAP(AppId, ActiveDataSource, DG.Control, DAT.Capability, MSG.Set, capOneValue);
            if (r == TWRC.Success) Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Cap set to vlaue:{value.ToString()}");
            else
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceError($"Error setting capability {r.ToString()} condition code:{GetConditionCode().ToString()}");
                throw new TwainException("Error setting capability", r, GetConditionCode());
            }
        }

        #endregion Capabilities

        #region Transferring

        public void EndCurrentTransfer(TW_PENDINGXFERS pxfer)
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Ending current transfer");
            TWRC r = DS_PENDINGTRANSFER(AppId, ActiveDataSource, DG.Control, DAT.PendingXfers, MSG.EndXfer, pxfer);
            if (r == TWRC.Success)
                Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Transfer canceled");
            else
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceError($"Error ending current transfer {r.ToString()} condition code:{GetConditionCode().ToString()}");
                throw new TwainException("Error ending current transfer", r, GetConditionCode());
            }
        }

        public void ResetPendingTransfers(TW_PENDINGXFERS pxfer)
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Reseting pending transfers");
            TWRC r = DS_PENDINGTRANSFER(AppId, ActiveDataSource, DG.Control, DAT.PendingXfers, MSG.Reset, pxfer);
            if (r == TWRC.Success) Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Pending transfers reset");
            else
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceError($"Error resetting pending transfers {r.ToString()} condition code:{GetConditionCode().ToString()}");
                throw new TwainException("Error resetting pending transfers", r, GetConditionCode());
            }
        }

        /// <summary>Transfer a single image from the source as a handle to a DIB image (hDib)</summary>
        /// <remarks>Consumer is responsible for releasing the handle</remarks>
        public IntPtr TransferSingleHDib()
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Transferring single image (native mode) using datasource:{ActiveDataSource.ProductName}");
            IntPtr hDib = new IntPtr(0);
            TWRC r = DS_IMAGENATIVETRANSFER(AppId, ActiveDataSource, DG.Image, DAT.ImageNativeXfer, MSG.Get, ref hDib);
            if (r == TWRC.XferDone)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Image retrieved using datasource:{ActiveDataSource.ProductName}");
                return hDib;
            }
            else if (r == TWRC.Cancel)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceWarning($"Operation Canceled Image retrieved using datasource:{ActiveDataSource.ProductName}");

                //TW_PENDINGXFERS pxfer = new TW_PENDINGXFERS();
                //pxfer.Count = 0;
                //pxfer.EOJ = 0;
                //EndCurrentTransfer(pxfer);
                throw new OperationCanceledException();
            }
            else
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceError($"Error retrieving image {r.ToString()} condition code:{GetConditionCode().ToString()} using datasource:{ActiveDataSource.ProductName}");
                TW_PENDINGXFERS pxfer = new TW_PENDINGXFERS();
                pxfer.Count = 0;
                pxfer.EOJ = 0;
                EndCurrentTransfer(pxfer);
                throw new TwainException("Error retrieving image", r, GetConditionCode());
            }
        }

        public IntPtr TransferSingleHDib(out bool more)
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Transfer Single hdbid");
            TW_PENDINGXFERS pxfer = new TW_PENDINGXFERS();
            pxfer.Count = 0;
            IntPtr hDib = TransferSingleHDib();
            EndCurrentTransfer(pxfer);
            more = pxfer.Count > 0;
            return hDib;
            //TODO: ResetPendingXfers()
        }

        protected byte[] TransferSingleImageData()
        { return ImageTools.HDIBToBytes(TransferSingleHDib(), true); }

        /// <summary>Transfer all available images to dib handles.</summary>
        public IEnumerable<IntPtr> TransferAllHDib()
        {
            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Transfering all HDibs");
            TW_PENDINGXFERS pxfer = new TW_PENDINGXFERS();
            try
            {
                do
                {
                    pxfer.Count = 0;
                    IntPtr hDib = TransferSingleHDib();
                    EndCurrentTransfer(pxfer);
                    yield return hDib;

                    Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Remaining images to transfer " + pxfer.Count);
                } while (pxfer.Count != 0);
            }
            finally
            {
                if (pxfer.Count > 0)
                { ResetPendingTransfers(pxfer); }
            }
        }

        /// <summary>Transfer all available images as bitmap encoded byte array.</summary>
        public IEnumerable<byte[]> TransferAllImageData()
        {
            foreach (IntPtr hDib in TransferAllHDib())
            { yield return ImageTools.HDIBToBytes(hDib, true); }
        }


        AutoResetEvent XferReady = new AutoResetEvent(false);
        AutoResetEvent DeviceUiCanceled = new AutoResetEvent(false);
        /*
		/// <summary>Initialize an acquire operation and return when the first image is ready.</summary>
		/// <remarks>Images are transfered using one of the TransferAll functions.</remarks>
		/// <remarks>Caller must call EndAcquire after transferring the images.</remarks>
		public void BeginAcquire(string scannerName, byte[] customDSData, bool showUI)
		{
			
			if ((string.IsNullOrWhiteSpace(scannerName) == false)
			    && (ActiveDataSourceName != scannerName))
			{ SetActiveDataSource(scannerName); }
			
			OpenActiveDataSource();
			
			Debug.WriteLine("Acquiring from " + ActiveDataSourceName);
			
			if ((customDSData != null) && (customDSData.Length > 0))
			{ SetCustomDSData(customDSData); }
			
			if (XferReady.Reset() == false)
			{ throw new Exception("Error resetting XferReady"); }
			if (DeviceUiCanceled.Reset() == false)
			{ throw new Exception("Error resetting DeviceUiCanceled"); }
			AutoResetEvent[] acquireResetEvents = new AutoResetEvent[]
			{ XferReady, DeviceUiCanceled };
			
			SetCapOneValue(CAP.XferCount, -1);
			EnableActiveDataSource(showUI, true);
			
			int autoResetEventIndex = AutoResetEvent.WaitAny(acquireResetEvents, Timeout.Infinite);
			if (acquireResetEvents[autoResetEventIndex] == XferReady)
			{ Trace.TraceInformation("Begin acquire complete, ready to transfer images"); }
			else
			{ throw new OperationCanceledException(); }
			
			
		}			
		*/

        /// <summary>Initialize an acquire operation and return when the first image is ready.</summary>
        /// <remarks>Images are transfered using one of the TransferAll functions.</remarks>
        /// <remarks>Caller must call EndAcquire after transferring the images.</remarks>
        public void BeginAcquire(string scannerName, byte[] customDSData, bool showUI)
        {

            if ((string.IsNullOrWhiteSpace(scannerName) == false)
                && (ActiveDataSourceName != scannerName))
            { SetActiveDataSource(scannerName); }

            OpenActiveDataSource();

            Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Acquiring from " + ActiveDataSourceName);

            if ((customDSData != null) && (customDSData.Length > 0))
            { SetCustomDSData(customDSData); }

            EventWaiter.Reset(XferReadyWaiter, DeviceUICanceledWaiter);

            SetCapOneValue(CAP.XferCount, -1);
            EnableActiveDataSource(showUI, true);

            //Wait for either cancel or xfer ready
            EventWaiter waiter = EventWaiter.Wait(XferReadyWaiter, DeviceUICanceledWaiter);
            if (waiter == DeviceUICanceledWaiter)
            {
                Logging.TraceLogger.TraceLoggerInstance.TraceWarning($"Operation Canceled for activedatasource {ActiveDataSourceName}");
                throw new OperationCanceledException();
            }

            Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Begin acquire complete, ready to transfer images for active datasource {ActiveDataSourceName}");
        }


        /// <summary>Begin an acquire operation on the active data source.</summary>
        /// <remarks>Images are transfered using one of the TransferAll functions.</remarks>
        /// <remarks>Caller must call EndAcquire after transferring the images.</remarks>
        public void BeginAcquire(byte[] customDSData, bool showUI)
        { BeginAcquire(null, customDSData, showUI); }

        /// <remarks>Images are transfered using one of the TransferAll functions.</remarks>
        /// <remarks>Caller must call EndAcquire after transferring the images.</remarks>
        public void BeginAcquire(bool showUI)
        { BeginAcquire(null, null, showUI); }

        /// <remarks>Images are transfered using one of the TransferAll functions.</remarks>
        /// <remarks>Caller must call EndAcquire after transferring the images.</remarks>
        public void BeginAcquire()
        { BeginAcquire(null, null, true); }

        /// <summary>Finalize an acquire operation.</summary>
        public void EndAcquire()
        {
            DisableActiveDataSource(true);
            CloseActiveDataSource(true);
        }

        public IEnumerable<Task<IntPtr>> AcquireHdib(string scannerName, byte[] customDsData, bool showUi, CancellationToken cToken)
        {
            BeginAcquire(scannerName, customDsData, showUi);
            TW_PENDINGXFERS pxfer = new TW_PENDINGXFERS();
            try
            {
                do
                {
                    pxfer.Count = 0;
                    yield return Task.Factory.StartNew<IntPtr>(() =>
                                                               { return TransferSingleHDib(); });
                    EndCurrentTransfer(pxfer);

                    Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Remaining images " + pxfer.Count);
                } while (pxfer.Count != 0);
            }
            finally
            {
                if (pxfer.Count > 0)
                { ResetPendingTransfers(pxfer); }
                Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Scanning complete, calling EndAcquire for scannername:{scannerName}");
                EndAcquire();
            }
        }

        /// <summary>Begin an acquire operation, return all available images, and terminate the acquire operation.</summary>
        public IEnumerable<IntPtr> AcquireHDib(string scannerName, byte[] customDSData, bool showUI)
        {
            try
            {
                BeginAcquire(scannerName, customDSData, showUI);
                //Loop and return each image.
                //NOTE: Force itteration to prevent EndAcquire from being called too early
                foreach (IntPtr hDib in TransferAllHDib())
                { yield return hDib; }
            }
            finally
            {
                EndAcquire();
            }
        }

        /// <summary>Begin an acquire operation, return all available images, and terminate the acquire operation.</summary>		
        public IEnumerable<IntPtr> AcquireHDib(byte[] customDSData, bool showUI)
        { return AcquireHDib(null, customDSData, showUI); }

        /// <summary>Begin an acquire operation, return all available images, and terminate the acquire operation.</summary>
        public IEnumerable<IntPtr> AcquireHDib(bool showUI)
        { return AcquireHDib(null, null, showUI); }

        /// <summary>Begin an acquire operation, return all available images, and terminate the acquire operation.</summary>
        public IEnumerable<IntPtr> AcquireHDib()
        { return AcquireHDib(null, null, true); }

        /// <summary>Begin an acquire operation, return all available images, and terminate the acquire operation.</summary>
        public IEnumerable<byte[]> AcquireImageData(string scannerName, byte[] customDSData, bool showUI)
        {
            foreach (IntPtr hDib in AcquireHDib(scannerName, customDSData, showUI))
            { yield return ImageTools.HDIBToBytes(hDib, true); }
        }

        /// <summary>Begin an acquire operation, return all available images, and terminate the acquire operation.</summary>
        public IEnumerable<byte[]> AcquireImageData(byte[] customDSData, bool showUI)
        { return AcquireImageData(null, customDSData, showUI); }

        /// <summary>Begin an acquire operation, return all available images, and terminate the acquire operation.</summary>
        public IEnumerable<byte[]> AcquireImageData(bool showUI)
        { return AcquireImageData(null, null, showUI); }

        /// <summary>Begin an acquire operation, return all available images, and terminate the acquire operation.</summary>
        public IEnumerable<byte[]> AcquireImageData()
        { return AcquireImageData(null, null, true); }

        [Obsolete("Use AcquireImageData or AcquireHDib")]
        public IEnumerable<byte[]> Acquire(byte[] customDSData, bool showUI)
        {
            OpenActiveDataSource();
            try
            {
                //TODO: Implement scanner selection
                Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Acquiring from " + ActiveDataSourceName);

                if ((customDSData != null) && (customDSData.Length > 0))
                { SetCustomDSData(customDSData); }

                //If showUI is false, only execute once, otherwise loop until canceled
                while (true)
                {
                    SetCapOneValue(CAP.XferCount, -1);
                    EventWaiter.Reset(XferReadyWaiter, DeviceUICanceledWaiter);
                    EnableActiveDataSource(showUI, true);
                    EventWaiter waiter = EventWaiter.Wait(XferReadyWaiter, DeviceUICanceledWaiter);
                    Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Done waiting");

                    if (waiter == DeviceUICanceledWaiter) throw new OperationCanceledException();

                    TW_PENDINGXFERS pxfer = new TW_PENDINGXFERS();
                    try
                    {
                        do
                        {
                            pxfer.Count = 0;
                            try
                            { yield return TransferSingleImageData(); }
                            finally { EndCurrentTransfer(pxfer); }

                        } while (pxfer.Count != 0);
                    }
                    finally
                    {
                        DisableActiveDataSource(false);
                        if (pxfer.Count > 0) ResetPendingTransfers(pxfer);
                    }
                    Logging.TraceLogger.TraceLoggerInstance.TraceInformation("Done transferring images");
                    if (showUI == false) break;
                }
            }
            finally { CloseActiveDataSource(false); }
        }

        #endregion Transferring

        #region Message Processing

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
                if (ActiveDataSource == null) return false;

                //Debug.WriteLine("Not Keyboard or mouse");

                //Convert the message to a windows mesage structure
                LowHighInt pos = new LowHighInt() { Number = (int)User32.GetMessagePos() };
                WIN_MSG winmsg = new WIN_MSG()
                {
                    hwnd = m.HWnd,
                    message = m.Msg,
                    wParam = m.WParam,
                    lParam = m.LParam,
                    time = User32.GetMessageTime(),
                    x = pos.Low,
                    y = pos.High
                };

                TW_EVENT twevt = new TW_EVENT()
                {
                    //TODO: Free pEvent
                    EventPtr = Marshal.AllocHGlobal(Marshal.SizeOf(winmsg)),
                    Message = (short)MSG.Null
                };
                Marshal.StructureToPtr(winmsg, twevt.EventPtr, false);

                //Ask the device to process the event
                TWRC r = DS_EVENT(AppId, ActiveDataSource, DG.Control, DAT.Event, MSG.ProcessEvent, ref twevt);
                if (r == TWRC.NotDSEvent)
                {
                    Logging.TraceLogger.TraceLoggerInstance.TraceInformation($"Source said not ds event for datasource:{ActiveDataSource.ProductName}");
                    return false;
                }
                //Debug.WriteLine("Source DS event");
                switch (twevt.Message)
                {
                    case (short)MSG.XFerReady:
                        Logging.TraceLogger.TraceLoggerInstance.TraceInformation("XFERREADY");
                        if (XferReady.Set() == false)
                        {
                            Logging.TraceLogger.TraceLoggerInstance.TraceError("Error setting XferReady");
                            throw new Exception("Error setting XferReady");
                        }
                        XferReadyWaiter.Set();
                        return true;
                    case (short)MSG.CloseDSReq:
                        Logging.TraceLogger.TraceLoggerInstance.TraceInformation("CLOSEDSREQ");
                        if (DeviceUiCanceled.Set() == false)
                        {
                            Logging.TraceLogger.TraceLoggerInstance.TraceError("Error setting DeviceUiCanceled");
                            throw new Exception("Error setting DeviceUiCanceled");
                        }
                        DeviceUICanceledWaiter.Set();
                        return true;
                    case (short)MSG.CloseDSOK:
                        Logging.TraceLogger.TraceLoggerInstance.TraceInformation("CLOSEDSOK");
                        DeviceUIClosedWaiter.Set();
                        return true;
                    case (short)MSG.DeviceEvent:
                        Logging.TraceLogger.TraceLoggerInstance.TraceInformation("DEVICEEVENT");
                        //TODO:Implement
                        return true;
                    default: return false;
                }
            }
            catch (OverflowException of)
            {
                Console.WriteLine(of.Message);
                return false;
            }

        }

        #endregion Message Processing

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            Application.RemoveMessageFilter(this);
            Close();
        }

        #endregion IDisposable

    }
}
