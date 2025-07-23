using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using Microsoft;
using System.Diagnostics;
using Tesseract;
using System.IO;
using System.Configuration;
using System.Reflection;
using Scanquire.Public.Extensions;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using ETL = EdocsUSA.Utilities.Logging;
using EdocsUSA.Utilities.Controls;

namespace Scanquire.Public
{
    public class AutoQaCheckBlankImages
    {
        //    const int MAX_AVG_BLANK_R = 250;
        //    const int MAX_AVG_BLANK_G = 250;
        //    const int MAX_AVG_BLANK_B = 250;
        const int MAX_AVG_BLANK_R = 253;
        const int MAX_AVG_BLANK_G = 253;
        const int MAX_AVG_BLANK_B = 253;
        string tessPath = (@"C:\Program Files (x86)\Tesseract-OCR\tessdata");

        private int TotalQaImagesDone
        { get; set; }

        public bool ErrorsRunningQutoQA
        { get; set; }


        public static AutoQaCheckBlankImages AutoQaCheckBlankImagesInstance = null;

        AutoQaCheckBlankImages()
        {

        }
        static AutoQaCheckBlankImages()
        {
            if (AutoQaCheckBlankImagesInstance == null)
            {
                AutoQaCheckBlankImagesInstance = new AutoQaCheckBlankImages();
            }
        }

        //private bool IsImageBlank1(SQImage image)
        //{
        //    double stdDev = CheckImageBlank(image);
        //    return stdDev < 100000;
        //}

        public void AutoQa(IEnumerable<SQImage> images, CancellationToken cToken, IProgress<ProgressEventArgs> progress)
        {
            DateTime st = DateTime.Now;
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation($"Starting quto QA:{st.ToString()}");
            ErrorsRunningQutoQA = false;
            TotalQaImagesDone = 0;
            images.TryCount(out int totalAutoQA);
            int totalQa = 0;
            var parallelQuery = images.AsParallel();
            //bool saveImage = parallelQuery.ForAll((image) => AutoQaCheckBlankImages.AutoQaCheckBlankImagesInstance.AutoQa(image)); //-ML
            parallelQuery.ForAll((image) => AutoQa(image, progress, totalAutoQA)); //-ML

            foreach (var n in parallelQuery)
            {
                totalQa++;

                cToken.ThrowIfCancellationRequested();
                if (n.SaveQaImage)
                {
                    progress.Report(new ProgressEventArgs(totalQa, totalAutoQA, $"Saving Image {totalQa.ToString()} of {totalAutoQA.ToString()}"));
                    n.Save(true);
                }


            }
            if (ErrorsRunningQutoQA)
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceError("Errors found running AutoQa");
             //   MessageBox.Show("Errors found running AutoQa", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            TimeSpan ts = DateTime.Now -st;
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation($"Done running AutoAQ end time:{DateTime.Now.ToString()}  total time:{string.Format("{0:00}",ts.Hours)}:{string.Format("{0:00}", ts.Minutes)}:{string.Format("{0:00}", ts.Seconds)}:{string.Format("{0:000}", ts.Milliseconds)} total images auto qa:{TotalQaImagesDone.ToString()}");
        }
        public void AutoQa(SQImage image, IProgress<ProgressEventArgs> progress, int totalQa)
        {
            TotalQaImagesDone++;
            image.SaveQaImage = false;
            progress.Report(new ProgressEventArgs(TotalQaImagesDone, totalQa, $"AutoQa Image {TotalQaImagesDone.ToString()} of {totalQa.ToString()}"));
            if(IsImageBlank(image))
           {
              ETL.TraceLogger.TraceLoggerInstance.TraceInformation($"Skipping AutoQa Image:{image.LatestRevision.OriginalImageFilePath} since image blank");
                return;
           }
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation($"AutoQa Image:{image.LatestRevision.OriginalImageFilePath}");
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation($"AutoQa Image {TotalQaImagesDone.ToString()} of {totalQa.ToString()}");
            try
            {
                using (var engine = new TesseractEngine(tessPath, "eng", EngineMode.Default))
                {
                    SQImageEditLock image_Lock = image.BeginEdit();
                  //  Bitmap autoQaImage = image.WorkingCopy.ToBitmap();
                    using (var img = PixConverter.ToPix((Bitmap)image.WorkingCopy))
              //      using (var img = PixConverter.ToPix(autoQaImage))
                    {
                        using (var page = engine.Process(img, PageSegMode.Auto)) //was (img, pageSegMode))
                        {


                            int orientation = 0;
                            float confidence = (float)0;
                            page.DetectBestOrientation(out orientation, out confidence);

                            switch (orientation)
                            {
                                case 180:
                                    image.WorkingCopy.RotateFlipEx(RotateFlipType.Rotate180FlipNone);

                                    image.SaveQaImage = true;
                                    break;
                                case 270:
                                    image.WorkingCopy.RotateFlipEx(RotateFlipType.Rotate90FlipNone);
                                    image.SaveQaImage = true;
                                    break;
                                case 90:
                                    image.WorkingCopy.RotateFlipEx(RotateFlipType.Rotate180FlipNone);
                                    image.WorkingCopy.RotateFlipEx(RotateFlipType.Rotate90FlipNone);
                                    image.SaveQaImage = true;
                                    break;

                            }

                        }



                    }


                }

            }
            catch (TesseractException te)
            {
                // throw new Exception($"Tesseract error message {te.Message} stack trace {te.StackTrace}");
                ETL.TraceLogger.TraceLoggerInstance.TraceError($"Tesseract error message {te.Message} stack trace {te.StackTrace}");
                ErrorsRunningQutoQA = true;

            }
            catch (Exception ex)
            {
                //  throw new Exception($"Tesseract error message {ex.Message} stack trace {ex.StackTrace}");
                ETL.TraceLogger.TraceLoggerInstance.TraceError($"Tesseract error message {ex.Message} stack trace {ex.StackTrace}");
                ErrorsRunningQutoQA = true;

            }

        }

        public SQImage AutoQa(SQImage image, IProgress<ProgressEventArgs> progress)
        {
            if (IsImageBlank(image))
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceInformation($"Skipping AutoQa Image:{image.LatestRevision.OriginalImageFilePath} since image blank");
                return image;
            }
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation($"AutoQa Image:{image.LatestRevision.OriginalImageFilePath}");
            progress.Report(new ProgressEventArgs(1, 1, "Running AutoQa"));
            try
            {
                using (var engine = new TesseractEngine(tessPath, "eng", EngineMode.Default))
                {
                    SQImageEditLock image_Lock = image.BeginEdit();
                  //  Bitmap autoQaImage = image.WorkingCopy.ToBitmap();
                    using (var img = PixConverter.ToPix((Bitmap)image.WorkingCopy))
                    //using (var img = PixConverter.ToPix(autoQaImage))
                    {
                        using (var page = engine.Process(img, PageSegMode.Auto)) //was (img, pageSegMode))
                        {


                            int orientation = 0;
                            float confidence = (float)0;
                            page.DetectBestOrientation(out orientation, out confidence);

                            switch (orientation)
                            {
                                case 180:
                                    image.WorkingCopy.RotateFlipEx(RotateFlipType.Rotate180FlipNone);

                                    image.Save();
                                    break;
                                case 270:
                                    image.WorkingCopy.RotateFlipEx(RotateFlipType.Rotate90FlipNone);
                                    image.Save();
                                    break;
                                case 90:
                                    image.WorkingCopy.RotateFlipEx(RotateFlipType.Rotate180FlipNone);
                                    image.WorkingCopy.RotateFlipEx(RotateFlipType.Rotate90FlipNone);
                                    image.Save();
                                    break;

                            }

                        }



                    }


                }

            }
            catch (TesseractException te)
            {
                ETL.TraceLogger.TraceLoggerInstance.TraceError($"Tesseract error message {te.Message} stack trace {te.StackTrace}");
                // throw new Exception($"Tesseract error message {te.Message} stack trace {te.StackTrace}");
                ErrorsRunningQutoQA = true;

            }
            catch (Exception ex)
            {
                //  throw new Exception($"Tesseract error message {ex.Message} stack trace {ex.StackTrace}");
                ETL.TraceLogger.TraceLoggerInstance.TraceError($"Tesseract error message {ex.Message} stack trace {ex.StackTrace}");
                ErrorsRunningQutoQA = true;

            }
            return image;

        }

        //private void CheckImage(SQImage image)
        //{
        //    SQImageEditLock image_Lock = image.BeginEdit();





        //    //int editID = image.BeginEdit();

        //    //Save the image files to disk.
        //    //image.WorkingCopy.Save(highQualityFilePath, HighQualityImageFormat, HighQualityImageSaveFlags);

        //    Bitmap bm1 = (Bitmap)image.WorkingCopy;
        //    Bitmap bm = new Bitmap(bm1);
        //    image.DiscardEdit(image_Lock);
        //    int height = bm.Height;
        //    int width = bm.Width;
        //    //     BitmapData srcData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

        //    for (int i = 0; i < height; i++)
        //    {
        //        for (int j = 0; j < width; j++)
        //        {
        //            Color bmColor = bm.GetPixel(j, i);
        //            Console.WriteLine($"J={j.ToString()} i={i.ToString()}");
        //        }
        //    }

        //}

        //public async Task<bool> IsImageBlank(SQImage image)
        public bool IsImageBlank(SQImage image)
        {
            DateTime st = DateTime.Now;
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation($"Checing image is blank:{image.LatestRevision.OriginalImageFilePath} start time:{st.ToString()}");

            // CheckImage(image);

            bool bad_page_result = false;

            SQImageEditLock image_Lock = image.BeginEdit();





            //int editID = image.BeginEdit();

            //Save the image files to disk.
            //image.WorkingCopy.Save(highQualityFilePath, HighQualityImageFormat, HighQualityImageSaveFlags);

            Bitmap bm = (Bitmap)image.WorkingCopy;

            image.DiscardEdit(image_Lock);

            BitmapData srcData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            int stride = srcData.Stride;

            IntPtr Scan0 = srcData.Scan0;

            long[] totals = new long[] { 0, 0, 0 };

            int width = bm.Width;
            int height = bm.Height;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int color = 0; color < 3; color++)
                        {
                            int idx = (y * stride) + x * 4 + color;

                            totals[color] += p[idx];
                        }
                    }
                }
            }

            long avgB = totals[0] / (width * height);
            long avgG = totals[1] / (width * height);
            long avgR = totals[2] / (width * height);

            // Console.WriteLine("avgB = " + avgB);
            // Console.WriteLine("avgG = " + avgG);
            //  Console.WriteLine("avgR = " + avgR);

            if (avgB > MAX_AVG_BLANK_B && avgG > MAX_AVG_BLANK_G && avgR > MAX_AVG_BLANK_R)
            {
                //    Console.WriteLine("Blank Page Detected");
                bad_page_result = true;
            }
            else
            {
                //    Console.WriteLine("Good Scan");
                bad_page_result = false;
            }

            //TODO: add the functionality to detect large errors
            //If there are large chunks of an image missing we want to call th

            //clear the data
            //

            bm.UnlockBits(srcData);
            //bm.Dispose();

            //TODO: check if the page is right or wrong
            // return bad_page_result;
            TimeSpan ts = DateTime.Now - st;
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation($"Done checking image is blank:{image.LatestRevision.OriginalImageFilePath} end time:{DateTime.Now.ToString()} total time:{string.Format("{0:00}", ts.Hours)}:{string.Format("{0:00}", ts.Minutes)}:{string.Format("{0:00}", ts.Seconds)}:{string.Format("{0:000}", ts.Milliseconds)}");
            return bad_page_result;
        }

        public   Task<bool> IsImageBlankAynsc(SQImage image)
        {
            DateTime st = DateTime.Now;
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation($"Checing image is blank:{image.LatestRevision.OriginalImageFilePath} start time:{st.ToString()}");
            bool bad_page_result = false;
            // CheckImage(image);
            return Task.Factory.StartNew(() =>
            { 


             SQImageEditLock image_Lock = image.BeginEdit();





            //int editID = image.BeginEdit();

            //Save the image files to disk.
            //image.WorkingCopy.Save(highQualityFilePath, HighQualityImageFormat, HighQualityImageSaveFlags);

            Bitmap bm = (Bitmap)image.WorkingCopy;

            image.DiscardEdit(image_Lock);

            BitmapData srcData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            int stride = srcData.Stride;

            IntPtr Scan0 = srcData.Scan0;

            long[] totals = new long[] { 0, 0, 0 };

            int width = bm.Width;
            int height = bm.Height;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int color = 0; color < 3; color++)
                        {
                            int idx = (y * stride) + x * 4 + color;

                            totals[color] += p[idx];
                        }
                    }
                }
            }

            long avgB = totals[0] / (width * height);
            long avgG = totals[1] / (width * height);
            long avgR = totals[2] / (width * height);

            // Console.WriteLine("avgB = " + avgB);
            // Console.WriteLine("avgG = " + avgG);
            //  Console.WriteLine("avgR = " + avgR);

            if (avgB > MAX_AVG_BLANK_B && avgG > MAX_AVG_BLANK_G && avgR > MAX_AVG_BLANK_R)
            {
                //    Console.WriteLine("Blank Page Detected");
                bad_page_result = true;
            }
            else
            {
                //    Console.WriteLine("Good Scan");
                bad_page_result = false;
            }

            //TODO: add the functionality to detect large errors
            //If there are large chunks of an image missing we want to call th

            //clear the data
            //

            bm.UnlockBits(srcData);
            //bm.Dispose();

            //TODO: check if the page is right or wrong
            // return bad_page_result;
            TimeSpan ts = DateTime.Now - st;
            ETL.TraceLogger.TraceLoggerInstance.TraceInformation($"Done checking image is blank:{image.LatestRevision.OriginalImageFilePath} end time:{DateTime.Now.ToString()} total time:{string.Format("{0:00}", ts.Hours)}:{string.Format("{0:00}", ts.Minutes)}:{string.Format("{0:00}", ts.Seconds)}:{string.Format("{0:000}", ts.Milliseconds)}");
            return bad_page_result;
            });

        }

        /// <summary>
        /// Get the standard deviation of pixel values.
        /// </summary>
        /// <param name="image">SqlImage image.</param>
        /// <returns>Standard deviation.</returns>
        //protected double CheckImageBlank(SQImage image)
        //{
        //    double total = 0, totalVariance = 0;
        //    int count = 0;
        //    double stdDev = 0;
        //    SQImageEditLock image_Lock = image.BeginEdit();
        //    Bitmap bm = (Bitmap)image.WorkingCopy;

        //    image.DiscardEdit(image_Lock);
        //    using (Bitmap b = new Bitmap(bm))
        //    {
        //        BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        //        int stride = bmData.Stride;
        //        IntPtr Scan0 = bmData.Scan0;
        //        unsafe
        //        {
        //            byte* p = (byte*)(void*)Scan0;
        //            int nOffset = stride - b.Width * 3;
        //            for (int y = 0; y < b.Height; ++y)
        //            {
        //                for (int x = 0; x < b.Width; ++x)
        //                {
        //                    count++;

        //                    byte blue = p[0];
        //                    byte green = p[1];
        //                    byte red = p[2];

        //                    int pixelValue = Color.FromArgb(0, red, green, blue).ToArgb();
        //                    total += pixelValue;
        //                    double avg = total / count;
        //                    totalVariance += Math.Pow(pixelValue - avg, 2);
        //                    stdDev = Math.Sqrt(totalVariance / count);

        //                    p += 3;
        //                }
        //                p += nOffset;
        //            }
        //        }

        //        b.UnlockBits(bmData);
        //    }

        //    return stdDev;
        //}

        public string GetAppConfigSqlSetting(string key, string dllName, string defaultValue)
        {
            string retConfig = defaultValue;
            ExeConfigurationFileMap exeConfigurationFileMap = new ExeConfigurationFileMap();
            exeConfigurationFileMap.ExeConfigFilename = string.Format("{0}\\{1}", GetApplicationDir(), dllName);

            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);
            KeyValueConfigurationCollection keyValueConfigurationCollection = configuration.AppSettings.Settings;// figurationManager.AppSettings.Get(key).ToString();
            foreach (KeyValueConfigurationElement configurationElement in keyValueConfigurationCollection)
            {
                if (configurationElement.Key.ToLower() == key.ToLower())
                {
                    retConfig = configurationElement.Value;

                    break;
                }
            }
            if (string.IsNullOrWhiteSpace(retConfig))
                retConfig = defaultValue;
            return retConfig;
        }

        internal string GetApplicationDir()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }


    }

}

