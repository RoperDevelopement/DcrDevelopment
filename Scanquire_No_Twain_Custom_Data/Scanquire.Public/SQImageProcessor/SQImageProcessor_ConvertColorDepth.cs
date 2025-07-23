using System;
using FreeImageAPI;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public
{
    /// <summary>Specialized ISQImageProcessor to convert the color depth of an image.</summary>
	public class SQImageProcessor_ConvertColorDepth : ISQImageProcessor
	{
        /// <summary>The color depth to apply to the image.</summary>
		public FREE_IMAGE_COLOR_DEPTH ColorDepth { get; set; }

        public SQImageProcessor_ConvertColorDepth() { }
		
		public SQImageProcessor_ConvertColorDepth(FREE_IMAGE_COLOR_DEPTH colorDepth)
		{ ColorDepth = colorDepth; }
		
        /// <summary>Apply the color depth manipulation to the specified image.</summary>
        /// <param name="image">The image to apply the color depth to.</param>
		public void ProcessImage(ref SQImage image)
		{
            ETL.TraceLoggerInstance.TraceInformation($"Applying color depth:{ColorDepth.ToString()} to image:{image.LatestRevision.OriginalImageFilePath}");
            using (SQImageEditLock editLock = image.BeginEdit())
            { 
                image.WorkingCopy.ConvertColorDepth(ColorDepth);
                image.Save(true);
            }			
		}
	}
}
