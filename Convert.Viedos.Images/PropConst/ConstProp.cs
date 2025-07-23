using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Edocs.HelperUtilities;
namespace Edocs.Convert.Viedos.Images.PropConst
{
    public static class ConstProp
    {
        readonly static string AppConfigKeyImageFolder = "ImageFolder";
        readonly static string AppConfigKeyFfmpegExe = "FfmpegExe";
        readonly static string AppConfigKeyImageSizeViedoWidth = "ImageSizeViedoWidth";
        readonly static string AppConfigKeyImageSizeViedoHeight = "ImageSizeViedoHeight";
        readonly static string AppConfigKeyViedoCreateFolder = "ViedoCreateFolder";
        
        readonly static string AppConfigKeyFfmpegExeParamViedoToImages = "FfmpegExeParamViedoToImages";
        readonly static string AppConfigKeyFfmpegExeParamIagesToViedo = "FfmpegExeParamIagesToViedo";
       public readonly static string RepStrViedoName = "{ViedoName}";
        readonly static string AppConfigKeyImageWFFolder = "ImageWFFolder";
        public readonly static string AppConfigKeyViewFolder = "ViewFolder";
        readonly static string RepStrArchx64x86 = "{Archx64x86}";
        public readonly static string RepStrImageFolder = "{ImageFolder}";
        public readonly static string RepStrImageExten = "{ImageExten}";
        public readonly static string RepStrImageName = "{ImageName}";
        public static readonly string ArgsViedoImage = "-iv";
        public static readonly string ArgsParmaImage = "images";
        public static readonly string ArgsParmaViedo = "viedo";


        public static string[] Args
        { get; set; }
        public static string  ViedoCreateFolder
        {
            get { return HelperUtilities.Utilities.CheckFolderPath(Utilities.GetAppConfigSetting(AppConfigKeyViedoCreateFolder)); }
        }
        public static string ImageWFFolder
        {
            get { return HelperUtilities.Utilities.CheckFolderPath(Utilities.GetAppConfigSetting(AppConfigKeyImageWFFolder)); }
        }
        private static string IsX64X86
        {
            get
            {
                if (Environment.Is64BitProcess)
                    return "x64";
                else return "x86";
            }
        }

       
        public static int ImageSizeViedoWidth
        {

            get
            {

                return Utilities.ParseInt(Utilities.GetAppConfigSetting(AppConfigKeyImageSizeViedoWidth));
            }

        }
        public static int ImageSizeViedoHeight
        {

            get
            {

                return Utilities.ParseInt(Utilities.GetAppConfigSetting(AppConfigKeyImageSizeViedoHeight));
            }

        }
        public static string FfmpegExeParamViedoToImages
        {

            get
            {

                return Utilities.GetAppConfigSetting(AppConfigKeyFfmpegExeParamViedoToImages);
            }

        }
        
            public static string ViewFolder
        {
            get { return HelperUtilities.Utilities.CheckFolderPath(Utilities.GetAppConfigSetting(AppConfigKeyViewFolder)); }
        }

        public static string ImageFolder
        {
            get { return HelperUtilities.Utilities.CheckFolderPath(Utilities.GetAppConfigSetting(AppConfigKeyImageFolder)); }
        }

        public static string FFmpegExe
        {
            get
            {
                //   string exeFold = $"{Utilities.CheckFolderPath(Utilities.GetApplicationDir())}{Utilities.GetAppConfigSetting(AppConfigKeyFfmpegExe)}";
                //exeFold = Path.Combine(exeFold, Utilities.GetAppConfigSetting(AppConfigKeyFfmpegExe));
                  string exeFold = $"{Utilities.CheckFolderPath(Utilities.GetApplicationDir())}";
                exeFold = Path.Combine(exeFold, Utilities.GetAppConfigSetting(AppConfigKeyFfmpegExe));
                return Utilities.ReplaceString(exeFold, RepStrArchx64x86, IsX64X86);
            }
        }
        public static bool CompareBitmapsFast(Bitmap bmp1, Bitmap bmp2)
        {
            if (bmp1 == null || bmp2 == null)
                return false;
            if (object.Equals(bmp1, bmp2))
                return true;
            if (!bmp1.Size.Equals(bmp2.Size) || !bmp1.PixelFormat.Equals(bmp2.PixelFormat))
                return false;

            int bytes = bmp1.Width * bmp1.Height * (Image.GetPixelFormatSize(bmp1.PixelFormat) / 8);

            bool result = true;
            byte[] b1bytes = new byte[bytes];
            byte[] b2bytes = new byte[bytes];

            BitmapData bitmapData1 = bmp1.LockBits(new Rectangle(0, 0, bmp1.Width, bmp1.Height), ImageLockMode.ReadOnly, bmp1.PixelFormat);
            BitmapData bitmapData2 = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, bmp2.PixelFormat);

            Marshal.Copy(bitmapData1.Scan0, b1bytes, 0, bytes);
            Marshal.Copy(bitmapData2.Scan0, b2bytes, 0, bytes);

            for (int n = 0; n <= bytes - 1; n++)
            {
                if (b1bytes[n] != b2bytes[n])
                {
                    result = false;
                    break;
                }
            }

            bmp1.UnlockBits(bitmapData1);
            bmp2.UnlockBits(bitmapData2);

            return result;
        }
        public static bool CompareBitmapsLazy(Bitmap bmp1, Bitmap bmp2)
        {
            if (bmp1 == null || bmp2 == null)
                return false;
            if (object.Equals(bmp1, bmp2))
                return true;
            if (!bmp1.Size.Equals(bmp2.Size) || !bmp1.PixelFormat.Equals(bmp2.PixelFormat))
                return false;

            //Compare bitmaps using GetPixel method
            for (int column = 0; column < bmp1.Width; column++)
            {
                for (int row = 0; row < bmp1.Height; row++)
                {
                    if (!bmp1.GetPixel(column, row).Equals(bmp2.GetPixel(column, row)))
                        return false;
                }
            }

            return true;
        }
       public static async Task<string> GetImageFolder(int index, IDictionary<int, string> imgLocDic)
        {
            if (imgLocDic.TryGetValue(index, out string value))
                return value;
            return string.Empty;
        }
       public static async Task RunFFmpeg(string args)
        {
            Utilities.RunTask(ConstProp.FFmpegExe, args, true, true, false, System.Diagnostics.ProcessWindowStyle.Minimized, Path.GetDirectoryName(ConstProp.FFmpegExe), Path.GetDirectoryName(ConstProp.FFmpegExe));
        }
        public static System.Drawing.Image ResizeImage(Image imgToResize, Size size)
        {
            return (System.Drawing.Image)(new Bitmap(imgToResize, size));
        }
      public  static async Task<string> GetInputArgs(string[] args)
        {
            if(args.Length > 0)
            {
                if (args.Length > 2)
                    throw new Exception("Invalid args passed");

                if (args[0].StartsWith(ArgsViedoImage, StringComparison.InvariantCultureIgnoreCase))
                {
                    int i = 0;
                    string retArgs = args[1];
                   
                    if(string.Compare(retArgs, ArgsParmaImage,true) == 0)
                        return retArgs;
                    else if (string.Compare(retArgs, ArgsParmaViedo, true) == 0)
                                return retArgs;
                    else
                        throw new Exception("Invalid args passed");
                }
                else
                    throw new Exception("Invalid args passed");
            }
            
            return string.Empty;
        }

    }
}
