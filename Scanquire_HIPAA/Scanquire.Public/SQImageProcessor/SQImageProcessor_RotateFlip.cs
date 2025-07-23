using System;
using System.Drawing;
using Scanquire.Public.Extensions;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public
{
    /// <summary>Specialized ISQImageProcessor to Rotate & Flip an SQImage</summary>
	public class SQImageProcessor_RotateFlip : ISQImageProcessor
	{ 
		private RotateFlipType _RotateFlipType = RotateFlipType.RotateNoneFlipNone;
        /// <summary>RotateFlip operation to perform.</summary>
		public RotateFlipType RotateFlipType 
		{
			get { return _RotateFlipType; }
			set { _RotateFlipType = value; }
		}
		
		public SQImageProcessor_RotateFlip() {}

        public SQImageProcessor_RotateFlip(RotateFlipType rotateFlipType)
		{ RotateFlipType = rotateFlipType; }
		
        /// <summary>Perform the RotateFlip operation specified by RotateFlipType on the specified image.</summary>
        /// <param name="image"></param>
		public void ProcessImage(ref SQImage image)
		{
            ETL.TraceLoggerInstance.TraceInformation($"Applying  RotateFlip operation:{RotateFlipType.ToString()} to image:{image.LatestRevision.OriginalImageFilePath}");
            using (SQImageEditLock editLock = image.BeginEdit())
            {
                image.WorkingCopy.RotateFlipEx(RotateFlipType);
                image.Save(true);
            }
		}
	}
}
