using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
namespace CompressTiff
{
    class Program
    {
        static void Main(string[] args)
        {
            CompressImage();
           // VaryQualityLevel();
            //Bitmap myBitmap;
            //ImageCodecInfo myImageCodecInfo;
            //Encoder myEncoder;
            //EncoderParameter myEncoderParameter;
            //EncoderParameters myEncoderParameters;

            //// Create a Bitmap object based on a BMP file.
            //myBitmap = new Bitmap("Shapes.bmp");

            //// Get an ImageCodecInfo object that represents the JPEG codec.
            //myImageCodecInfo = GetEncoderInfo("image/jpeg");

            //// Create an Encoder object based on the GUID

            //// for the Quality parameter category.
            //myEncoder = Encoder.Quality;

            //// Create an EncoderParameters object.

            //// An EncoderParameters object has an array of EncoderParameter

            //// objects. In this case, there is only one

            //// EncoderParameter object in the array.
            //myEncoderParameters = new EncoderParameters(1);

            //// Save the bitmap as a JPEG file with quality level 25.
            //myEncoderParameter = new EncoderParameter(myEncoder, 25L);
            //myEncoderParameters.Param[0] = myEncoderParameter;
            //myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters);

            //// Save the bitmap as a JPEG file with quality level 50.
            //myEncoderParameter = new EncoderParameter(myEncoder, 50L);
            //myEncoderParameters.Param[0] = myEncoderParameter;
            //myBitmap.Save("Shapes050.jpg", myImageCodecInfo, myEncoderParameters);

            //// Save the bitmap as a JPEG file with quality level 75.
            //myEncoderParameter = new EncoderParameter(myEncoder, 75L);
            //myEncoderParameters.Param[0] = myEncoderParameter;
            //myBitmap.Save("Shapes075.jpg", myImageCodecInfo, myEncoderParameters);
        }
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        private static void VaryQualityLevel()
        {
            // Get a bitmap. The using statement ensures objects  
            // are automatically disposed from memory after use.  
            using (Bitmap bmp1 = new Bitmap(@"D:\Archives\NYP\fb21ddbe-36b9-44f3-bc0e-7e1a7928cb38\2b4cef25-42d2-4eef-a433-9e1fd2221572.TIF"))
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Tiff);

                // Create an Encoder object based on the GUID  
                // for the Quality parameter category.  
                System.Drawing.Imaging.Encoder myEncoder =
                    System.Drawing.Imaging.Encoder.Quality;

                // Create an EncoderParameters object.  
                // An EncoderParameters object has an array of EncoderParameter  
                // objects. In this case, there is only one  
                // EncoderParameter object in the array.  
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp1.Save(@"D:\Archives\TestPhotoQualityFifty.TIF", jpgEncoder, myEncoderParameters);
                 
                myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp1.Save(@"D:\Archives\TestPhotoQualityHundred.TIF", jpgEncoder, myEncoderParameters);

                // Save the bitmap as a JPG file with zero quality level compression.  
                myEncoderParameter = new EncoderParameter(myEncoder, 0L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp1.Save(@"D:\Archives\TestPhotoQualityZero.TIF", jpgEncoder, myEncoderParameters);
            }
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        private   static void CompressImage()
        {
            Bitmap myBitmap;
            ImageCodecInfo myImageCodecInfo;
       System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            // Create a Bitmap object based on a BMP file.
            myBitmap = new Bitmap(@"D:\Archives\NYP\LabReqs\9a2c933d-af7b-4b1d-81ef-da888aa94244\ccc83ca6-dcc0-4cd7-9666-8b722f19ae60.tif");

            // Get an ImageCodecInfo object that represents the TIFF codec.
           // myImageCodecInfo = GetEncoderInfo("image/tiff");
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Tiff);

            // Create an Encoder object based on the GUID
            // for the Compression parameter category.
            myEncoder = System.Drawing.Imaging.Encoder.Compression;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
            myEncoderParameters = new EncoderParameters(1);

            // Save the bitmap as a TIFF file with LZW compression.
            myEncoderParameter = new EncoderParameter(
                myEncoder,
                (long)EncoderValue.CompressionCCITT3);
            myEncoderParameters.Param[0] = myEncoderParameter;
            myBitmap.Save("D:\\Archives\\ShapesLZW.tif", jpgEncoder, myEncoderParameters);
        }
    }
}
