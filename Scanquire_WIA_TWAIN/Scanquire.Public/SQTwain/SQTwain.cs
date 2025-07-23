using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EdocsUSA.Controls;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using FreeImageAPI;
using Scanquire.Public.Extensions;
using Microsoft;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
using System.Text;

namespace Scanquire.Public
{
    public class SQTwain
    {
        /// <summary>The handle of the window making Twain requests.</summary>
        public static IntPtr HostWindowHandle
        { get { return Process.GetCurrentProcess().MainWindowHandle; } }
        public bool UseTwain
        { get; set; }
        /// <summary>The window making Twain requests.</summary>
        public static IWin32Window HostWindow
        { get { return new IWin32WindowWrapper(HostWindowHandle); } }

        /// <summary>Name of the last used data source, to be used when no new data source is provided.</summary>
		protected string PreviousDataSourceName
        {
            get { return Properties.SQTwain.Default.PreviousDataSourceName; }
            set
            {
                Properties.SQTwain.Default.PreviousDataSourceName = value;
                Properties.SQTwain.Default.Save();
            }
        }

        private Twain _Twain = new Twain(HostWindowHandle, "eDocs USA", "Scanning", "Scanquire");
        private WindowsWim _Wia = new WindowsWim();
        /// <summary>Twain interface.</summary>
        public Twain Twain
        { get { return _Twain; } }

        /// <summary>Twain interface.</summary>
        public WindowsWim WIA
        {
            get { return _Wia; }
        }
        protected SQTwainSettingsDialog _SQTwainSettingsDialog = null;
        /// <summary>Dialog to display and allow user selection of twain settings.</summary>
		protected SQTwainSettingsDialog SQTwainSettingsDialog
        {
            get
            {
                if (_SQTwainSettingsDialog == null)
                { _SQTwainSettingsDialog = new SQTwainSettingsDialog(Twain); }
                return _SQTwainSettingsDialog;
            }
        }

        private SQTwain()
        {
        }

        /// <summary>Retrieve a series of images from the Twain device.</summary>
        /// <param name="progress">Used to report progress back to the caller.</param>
        /// <param name="cToken">Cancelation token that can be set by the caller to stop acquiring.</param>
        /// <returns>An SQImage task for each image acquired from the Twain device.</returns>
        public IEnumerable<Task<SQImage>> Acquire(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            ETL.TraceLoggerInstance.TraceInformation("Scanning images from scanner");
            while (true)
            {
                //Ask the user what scanner and settings to continue with.
                SQTwainSettingsDialog.TryShowDialog(DialogResult.OK);

                string scannerName = SQTwainSettingsDialog.SelectedScanner;
                string settingName = SQTwainSettingsDialog.SelectedSetting;
                ETL.TraceLoggerInstance.TraceInformation($"Scanning images from scanner name:{scannerName} for scanner setting:{settingName}");
                //If the user specified a setting, apply it, otherwise set to use the previous settings.
                SQTwainSetting setting = null;
                if ((string.IsNullOrWhiteSpace(settingName) == false)
                    || (SQTwainSettingsDialog.UsePreviousSettings))
                { setting = SQTwainSettings.Instance[scannerName][settingName]; }

                bool showUi = SQTwainSettingsDialog.ShowUI;
                ETL.TraceLoggerInstance.TraceInformation($"Scanning images from scanner name:{scannerName} for scanner setting:{settingName} show scanner ui:{showUi.ToString()}");
                foreach (Task<SQImage> imageTask in Acquire(scannerName, settingName, showUi, false, progress, cToken))
                { yield return imageTask; }
            }

            //return Acquire(scannerName, settingName, showUi, progress, cToken);
        }

        /// <summary>Retrieve a series of images from the specified Twain device.</summary>
        /// <param name="scannerName">The name of the twain device to aquire from.  Name must be as it appears in the source list.</param>
        /// <param name="settingName">The name of the stored configuration to apply to the acquire job.</param>
        /// <param name="showUi">
        /// If true, display the device's native UI.
        /// If false, do not display the device's native UI.
        /// </param>
        /// <param name="promptForContinue">
        /// If true, ask the user to continue when the device finishes returning images.
        /// If false, stop when the device finishes returning images.
        /// </param>
        /// <param name="progress">Used to report progress back to the caller.</param>
        /// <param name="cToken">Cancelation token that can be set by the caller to stop acquiring.</param>
        /// <returns>An SQImage task for each image acquired from the Twain device.</returns>
        public IEnumerable<Task<SQImage>> Acquire(string scannerName, string settingName, bool showUi, bool promptForContinue, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {

            if (string.IsNullOrWhiteSpace(scannerName))
            {
                ETL.TraceLoggerInstance.TraceError("scannerName is required");
                throw new ArgumentNullException("scannerName is required");
            }
            ETL.TraceLoggerInstance.TraceInformation($"Acquire images from scanner:{scannerName} for setting:{settingName} show ui:{showUi.ToString()} promp for continue:{promptForContinue.ToString()}");
            SQTwainSetting setting = null;

            //If no setting was applied, create an empty one.
            if (string.IsNullOrWhiteSpace(scannerName))
            { setting = new SQTwainSetting(); }
            //If a setting was requested, set it.
            //If the requested setting does not exist, fail.
            else
            {
                setting = SQTwainSettings.Instance[scannerName][settingName];
                if (setting == null)
                {
                    ETL.TraceLoggerInstance.TraceError("Setting " + settingName + " was not found for scanner " + scannerName);
                    throw new KeyNotFoundException("Setting " + settingName + " was not found for scanner " + scannerName);
                }
            }


            int progressCurrent = 0;
            //ProgressTotal to -1 to indicate unknown number of total images.
            int progressTotal = -1;
            //Loop until the user cancels
            bool keepScanning = true;
            // UseTwain = false;
            do
            {
                //   string s = Encoding.Default.GetString(setting.CustomDSData);
                //Request the images from the Twain device.
                if (UseTwain)
                {
                    foreach (IntPtr hDib in Twain.AcquireHDib(scannerName, setting.CustomDSData, showUi))
                    {
                        progressCurrent++;
                        //Yield Return a task to convert the retrieved HDib to an SQImage and apply any requested filters.
                        yield return Task.Factory.StartNew<SQImage>(() =>
                        {
                            SQImage image = SQImage.FromHDib(hDib, true);
                            ApplyImageFilters(image, setting.ImageProcessors);
                            return image;
                        });
                        ETL.TraceLoggerInstance.TraceInformation($"{progressCurrent.ToString()} {progressTotal.ToString()} Scanning");
                        progress.Report(new ProgressEventArgs(progressCurrent, progressTotal, "Scanning"));
                    }
                }
                else
                {
                   // if(showUi)
                 //   {
                 //       WIA.AcquireWIA(setting.CustomDSData).ConfigureAwait(false).GetAwaiter().GetResult();
                 //   }
                   // else
                   // { 
                    foreach (Image wiaImage in WIA.AcquireWIA(scannerName, setting.CustomDSData, showUi))
                    {
                        progressCurrent++;
                        //Yield Return a task to convert the retrieved HDib to an SQImage and apply any requested filters.
                        yield return Task.Factory.StartNew<SQImage>(() =>
                        {

                            SQImage image = SQImage.FromImage(wiaImage);
                            ApplyImageFilters(image, setting.ImageProcessors);
                            return image;
                            //if(wiaImage.SecondWIAImage != null)
                            //{ 
                            //SQImage image2 = SQImage.FromBytes(wiaImage.SecondWIAImage);
                            //ApplyImageFilters(image2, setting.ImageProcessors);
                            //return image2;
                            //}
                        });
                        ETL.TraceLoggerInstance.TraceInformation($"{progressCurrent.ToString()} {progressTotal.ToString()} Scanning");
                        progress.Report(new ProgressEventArgs(progressCurrent, progressTotal, "Scanning"));
                    //}
                    }
                }
                //Once the device runs out of images, prompt to continue if needed.
                if (promptForContinue == false)
                { keepScanning = false; }
                else
                {
                    DialogResult r = MessageBox.Show("Add more pages and click 'Ok' to append", "Scan Complete", MessageBoxButtons.OKCancel);
                    if (r != DialogResult.OK)
                    {
                        ETL.TraceLoggerInstance.TraceInformation("Stop Scanning");
                        keepScanning = false;
                    }
                }
            } while (keepScanning);
        }
        /*
        public IEnumerable<Task<SQImage>> Acquire(string scannerName, string settingName, bool showUi, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            //If no scanner was specified, set it to the current device
            if (string.IsNullOrWhiteSpace(scannerName))
            { scannerName = Twain.ActiveDataSourceName; }
            //Else if the specified scanner does not exist, fail
            else if (Twain.DataSourcesNames.Contains(scannerName) == false)
            { throw new ArgumentException("scannerName", "The specified scanner (" + scannerName + ") could not be found"); }

            //If a setting was provided, but does not exist, fail
            if ((String.IsNullOrWhiteSpace(settingName) == false)
                && (SQTwainSettings.Instance[scannerName].ContainsKey(settingName) == false))
            { throw new ArgumentException("settingName", "The specified setting name (" + settingName + ") could not be found for the specified scanner (" + scannerName + ")"); }

            //If a setting was not specified, create an empty one, otherwise set it.
            SQTwainSetting setting;
            if (string.IsNullOrWhiteSpace(settingName))
            { setting = new SQTwainSetting(); }
            else
            { setting = SQTwainSettings.Instance[scannerName][settingName]; }

            BlockingCollection<IntPtr> hDibs = new BlockingCollection<IntPtr>();

            //Twain can take a while to initialize, so raise an empty progress to inform the
            // user that the scanning has started.
            progress.Report(new ProgressEventArgs(0, -1, "Waiting for twain"));

            //Start a task to acquire the images from the scanner
            Task hDibTask = Task.Factory.StartNew(() =>
            {
                try
                {
                    foreach (IntPtr hDib in Twain.AcquireHDib(setting.CustomDSData, showUi))
                    {
                        cancelToken.ThrowIfCancellationRequested();
                        hDibs.Add(hDib);
                    }
                }
                finally
                { hDibs.CompleteAdding(); }
            });

            //Start a task to convert the hDibs to images
            //Doing this on separate Task to allow Twain to run uninterrupted
            Task sqImageTask = Task.Factory.StartNew(() =>
            {
                int progressCurrent = 0;
                int progressTotal = -1;
                foreach (IntPtr hDib in hDibs.GetConsumingEnumerable())
                {
                    progressCurrent++;
                    SQImage image = SQImage.FromHDib(hDib, true);
                    ApplyImageFilters(image, setting.ImageProcessors);
                    progress.Report(new SQAcquireProgressEventArgs(progressCurrent, progressTotal, image));
                }
            });
            
            //Wait for both tasks to complete
            await hDibTask;
            await sqImageTask;
        }
		*/

        /// <summary>Apply a series of imageProcessors to the specified image.</summary>
        /// <param name="image">The image to process.</param>
        /// <param name="imageProcessors">The imageProcessors to apply to the image.</param>
		protected void ApplyImageFilters(SQImage image, IEnumerable<ISQImageProcessor> imageProcessors)
        {
            ETL.TraceLoggerInstance.TraceInformation($"Appling image filters to image:{image.LatestRevision.OriginalImageFilePath}  imageProcessors to apply {imageProcessors.GetType().ToString()}");
            if (imageProcessors.Count() > 0)
            {
                using (SQImageEditLock editLock = image.BeginEdit())
                {
                    foreach (ISQImageProcessor processor in imageProcessors)
                    {
                        ETL.TraceLoggerInstance.TraceInformation("Processing with " + processor.GetType().ToString());
                        processor.ProcessImage(ref image);
                    }
                    image.Save(true);
                }
            }
        }

        static readonly SQTwain _Instance = new SQTwain();
        public static SQTwain Instance
        { get { return _Instance; } }
    }
}
