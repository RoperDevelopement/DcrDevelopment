using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using FreeImageAPI;
using Polenter.Serialization;
using Scanquire.Public.Extensions;
using System.Threading.Tasks;
using System.Threading;
using Microsoft;
using EdocsUSA.Utilities.Logging;

namespace Scanquire.Public
{
    //Base implemetation of ISQCommandImageBuilder.
	public abstract class SQCommandImageBuilderBase : ISQCommandImageBuilder
	{

		//TODO: Test 200 DPI (was not working with PDF)
		private int _ImageDPI = 300;
		public int ImageDPI
		{
			get { return _ImageDPI; }
			set { _ImageDPI = value; }
		}

        private float _ImageWidth = ImageTools.InchesToPoints(8.5F);
        /// <summary>Width of the produced image (in points)</summary>
        public float ImageWidth
        { get { return _ImageWidth; } }

        private float _MinImageHeight = ImageTools.InchesToPoints(11F);
        /// <summary>The minimum height for a produced image (in points)</summary>
        public float MinImageHeight
        {
            get { return _MinImageHeight; }
            set { _MinImageHeight = value; }
        }

        private Padding _ImageMargin = new Padding(ImageTools.POINTS_PER_HALF_INCH);
        /// <summary>Margin (in points) for the whole image.</summary>
        public Padding ImageMargin
		{
            get { return _ImageMargin; }
            set { _ImageMargin = value; }
		}

        private int _ElementSpacing = ImageTools.POINTS_PER_QUARTER_INCH;
        /// <summary>Vertical spacing (in points) between individual content elements.</summary>
        public int ElementSpacing
        { 
            get { return _ElementSpacing; }
            set { _ElementSpacing = value; }
        }


        /// <summary>Maximum width (in points) for a content element.</summary>
        private float ElementMaxWidth
        { get { return ImageWidth - ImageMargin.Horizontal; } }

        private string _BarcodeRendererName = "DEFAULT";
        /// <summary>ID of the barcode renderer to use for drawing any barcodes</summary>
        public string BarcodeRendererName
        {
            get { return _BarcodeRendererName; }
            set { _BarcodeRendererName = value; }
        }

        private Code128BarcodeRenderer _BarcodeRenderer = null;
        /// <summary>Code128BarcodeRenderer specified by BarcodeRendererName.</summary>
        [ExcludeFromSerialization]
		public Code128BarcodeRenderer BarcodeRenderer
		{
			get 
			{
                if (_BarcodeRenderer == null)
				{ _BarcodeRenderer = BarcodeRenderers.Instance[BarcodeRendererName]; }
				return _BarcodeRenderer; 
			}
			set { _BarcodeRenderer = value; }
		}
		
		public SQCommandImageBuilderBase()
		{
			
		}

        /// <summary>Generate a sub-image containing the specified image.</summary>
        protected virtual Bitmap GenerateImageImage(SQCommand_Image_DrawImage command)
        {
            TraceLogger.TraceLoggerInstance.TraceInformation("Generate a sub-image containing the specified image.");
            using (SQImageEditLock editLock = command.Image.BeginEdit())
            {
                //Calculate the dimensions (pixel) of the new image.
                float originalWidth = command.Image.WorkingCopy.Width;
                float originalHeight = command.Image.WorkingCopy.Height;

                //Limit the width to ContentMaxWidth
                float maxWidth = ImageTools.PointsToPixels(ElementMaxWidth, ImageDPI);
                int scaledWidth = (int)(Math.Max(originalWidth, maxWidth));
                
                //Calculate the zoom factor for the scaled width and apply it to the height.
                float zoomFactor = scaledWidth / originalWidth;
                int scaledHeight = (int)(originalHeight * zoomFactor);

                //Create the empty bitmap using the scaled dimensions.
                Bitmap bitmap = new Bitmap((int)scaledWidth, (int)scaledHeight);
                bitmap.SetResolution(ImageDPI, ImageDPI);
                //Paint the image to the empty bitmap.
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.PageUnit = GraphicsUnit.Pixel;
                    g.Clear(Color.White);
                    using (Bitmap pasteBitmap = command.Image.WorkingCopy.ToBitmap())
                    { g.DrawImage(pasteBitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height)); }
                    g.Save();
                }
                //Set the resolution again (don't trust Graphics not to loose it) and return.
                bitmap.SetResolution(ImageDPI, ImageDPI);
                return bitmap;
            }
        }

        /// <summary>Generate a sub-image containing the specified text.</summary>
        protected virtual Bitmap GenerateTextImage(SQCommand_Image_DrawText command)
        {
            TraceLogger.TraceLoggerInstance.TraceInformation("Generate a sub-image containing the specified text.");
            float width = ImageTools.PointsToPixels(ElementMaxWidth, ImageDPI);
            SizeF imageSize;

            //Create a 1x1 bitmap to create a graphics object against to use MeasureString to determine the text height.
            using (Bitmap bmp = new Bitmap(1, 1))
            {
                bmp.SetResolution(ImageDPI, ImageDPI);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.PageUnit = GraphicsUnit.Pixel;
                    imageSize = g.MeasureString(command.Text, command.Font, new SizeF((int)width, 0));
                }
            }

            //Create an empty bitmap with the calculated dimensions.
            //Bitmap bitmap = new Bitmap((int)ImageTools.PointsToPixels(imageSize.Width, ImageDPI), (int)ImageTools.PointsToPixels(imageSize.Height, ImageDPI));
            Bitmap bitmap = new Bitmap((int)imageSize.Width, (int)imageSize.Height);
            bitmap.SetResolution(ImageDPI, ImageDPI);
            //Draw the text
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                g.PageUnit = GraphicsUnit.Pixel;
                g.Clear(Color.White);
                g.DrawString(command.Text, command.Font, command.Brush, new Point(0, 0));
                g.Save();
            }
            //Set the resolution again (don't trust Graphics not to loose it) and return.
            bitmap.SetResolution(ImageDPI, ImageDPI);
            return bitmap;
        }

        /// <summary>Generate a sub-image containing a barcode.</summary>
        protected virtual Bitmap GenerateBarcodeImage(SQCommand_Image_DrawBarcode command)
        {
            TraceLogger.TraceLoggerInstance.TraceInformation("Generate a sub-image containing a barcode.");
            Code128Barcode barcode = new Code128Barcode(command.Value, command.Caption);
            return BarcodeRenderer.Generate(barcode, ImageDPI);
        }

        /// <summary>Compile the provided commands into a single image.</summary>
        public virtual Task<SQImage> Build(IEnumerable<ISQCommand> commands, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            TraceLogger.TraceLoggerInstance.TraceInformation("Compile the provided commands into a single image.");
            return Task.Factory.StartNew<SQImage>(() =>
            {
                //Generate all of the element images
                List<Bitmap> images = new List<Bitmap>();
                try
                {
                    int progressCurrent = 0;
                    int progressTotal;
                    commands.TryCount(out progressTotal);
                    foreach (ISQCommand command in commands)
                    {
                        progressCurrent++;
                        //Delegate based on command type.
                        if (command is SQCommand_Image_DrawText)
                        {
                            SQCommand_Image_DrawText drawTextCommand = (SQCommand_Image_DrawText)command;
                            images.Add(GenerateTextImage(drawTextCommand));
                        }
                        else if (command is SQCommand_Image_DrawImage)
                        {
                            SQCommand_Image_DrawImage drawImageCommand = (SQCommand_Image_DrawImage)command;
                            images.Add(GenerateImageImage(drawImageCommand));
                        }
                        else if (command is SQCommand_Image_DrawBarcode)
                        {
                            SQCommand_Image_DrawBarcode drawBarcodeCommand = (SQCommand_Image_DrawBarcode)command;
                            images.Add(GenerateBarcodeImage(drawBarcodeCommand));
                        }
                        else
                        {
                            Trace.TraceWarning("Unexpected image command " + command.GetType().ToString() + " ... Skipping");
                        }
                        progress.Report(new ProgressEventArgs(progressCurrent, progressTotal, "Generating elements"));
                    }

                    //Calculate the combined size (pixels) of all elements in pixels
                    SizeF imageSize = new SizeF()
                    {
                        Width = ImageTools.PointsToPixels(ImageWidth, ImageDPI),
                        Height = ImageTools.PointsToPixels(ImageMargin.Vertical, ImageDPI)
                    };

                    //Loop through the images and increment height by image height and element spacing.
                    foreach (Bitmap bitmap in images)
                    { imageSize.Height += bitmap.Height + ImageTools.PointsToPixels(ElementSpacing, ImageDPI); }

                    //Calculate the height (pixels) of the final image (max of MinImageHeight & calculated imageSize.Height)
                    float minHeight = ImageTools.PointsToPixels(MinImageHeight, ImageDPI);
                    imageSize.Height = (float)Math.Ceiling(Math.Max(imageSize.Height, minHeight));

                    //Create the final image                
                    using (FreeImageBitmap fib = new FreeImageBitmap((int)imageSize.Width, (int)imageSize.Height, PixelFormat.Format1bppIndexed))
                    {
                        fib.SetResolution(ImageDPI, ImageDPI);
                        //TODO: Support color?
                        fib.Palette[0] = new RGBQUAD(Color.White);
                        fib.Palette[1] = new RGBQUAD(Color.Black);

                        
                        float startX = ImageTools.PointsToPixels(ImageMargin.Left, ImageDPI);
                        float startY = ImageTools.PointsToPixels(ImageMargin.Top, ImageDPI);
                        Point currentLocation = new Point((int)startX, (int)startY);
                        
                        progressCurrent = 0;
                        images.TryCount(out progressTotal);
                        foreach (Bitmap image in images)
                        {
                            Debug.WriteLine("Painting " + image.Height + " to " + currentLocation.ToString());
                            progressCurrent++;
                            using (FreeImageBitmap fibCurrent = new FreeImageBitmap(image))
                            {
                                fibCurrent.ConvertColorDepth(FREE_IMAGE_COLOR_DEPTH.FICD_01_BPP);
                                fibCurrent.Normalize1BPPToMinIsWhite();
                                if (fib.Paste(fibCurrent, currentLocation, 255) == false)
                                {
                                    TraceLogger.TraceLoggerInstance.TraceError("Error pasting bitmap");
                                    throw new Exception("Error pasting bitmap");
                                }
                                currentLocation.Y += fibCurrent.Height + (int)ImageTools.PointsToPixels(ElementSpacing, ImageDPI);
                            }
                        }

                        return new SQImage(fib);
                    }
                }
                finally //Dispose all temporary images
                {
                    foreach (Bitmap image in images)
                    { image.Dispose(); }
                }               
            });
        }

        /// <summary>Generate a series of SQImages based on a series of command lists.</summary>
        /// <returns>A compiled image for each command list provided.</returns>
        public virtual IEnumerable<Task<SQImage>> Build(IEnumerable<IEnumerable<ISQCommand_Image>> commandLists, IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            int progressCurrent = 0;
            int progressTotal;
            commandLists.TryCount(out progressTotal);
            TraceLogger.TraceLoggerInstance.TraceInformation("Generate a series of SQImages based on a series of command lists.");
            foreach (IEnumerable<ISQCommand_Image> commandList in commandLists)
            {
                cToken.ThrowIfCancellationRequested();
                progressCurrent++;
                string currentImageProgressCaption = "Image " + progressCurrent + " of " + progressTotal;
                TraceLogger.TraceLoggerInstance.TraceInformation($"Current image process caption {currentImageProgressCaption}");
                Action <ProgressEventArgs> currentImageProgressAction = new Action<ProgressEventArgs>(p =>
                { progress.Report(new ProgressEventArgs(p.Current, p.Total, currentImageProgressCaption)); });
                Progress<ProgressEventArgs> currentImageProgress = new Progress<ProgressEventArgs>(currentImageProgressAction);
                yield return Build(commandList, currentImageProgress, cToken); 
            }
        }

        public abstract IEnumerable<Task<SQImage>> Build(IProgress<ProgressEventArgs> progress, CancellationToken cToken);
	}
}
