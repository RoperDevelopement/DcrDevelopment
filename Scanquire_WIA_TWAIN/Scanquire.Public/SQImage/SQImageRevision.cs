using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using EdocsUSA.Utilities.Extensions;
using FreeImageAPI;
using Scanquire.Public.Extensions;
using EdocsUSA.Utilities;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public
{
	/// <summary>Represents a single revision of an image.</summary>
	/// <remarks>
	/// Saves images to the filesystem.
	/// Caller must manually call Dispose to delete these image files.
	/// Dispose is NOT called by the object finalizer (by design to allow recovery on application crash).
	/// </remarks>
	public class SQImageRevision : IDisposable
	{
        /*
		/// <summary>Object used for storing and rebuilding an SQImageRevision between application sessions.</summary>
		private class RecoveryData
		{			
			public string ThumbnailImageFilePath { get; set; }
			
			public string PreviewImageFilePath { get; set; }
			
			public string OriginalImageFilePath { get; set; }
		}
		*/
		#region Static Properties
		
		private static Properties.SQImageRevision DefaultSettings = Properties.SQImageRevision.Default;
		
		/// <summary>Maximum size (in pixels) of the thumbnail image.</summary>
		/// <remarks>
		/// Value retrieved from ApplicationSetting Settings.SQImageRevision.MaxThumbnailImageSize.
		/// </remarks>
		private static Size ThumbnailImageMaxSize = DefaultSettings.MaxThumbnailImageSize;
				
		/// <summary>Maximum size (in pixels) of the preview image.</summary>
		/// <remarks>
		/// Reducing this will increase viewer performance, but reduce visible image quality.
		/// Value retrieved from ApplicationSetting Settings.SQImageRevision.MaxPreviewImageSize.
		/// </remarks>
		private static Size PreviewImageMaxSize = DefaultSettings.MaxPreviewImageSize;

        /// <summary>Directory to store image files.</summary>
        /// <remarks>If provided a relative path, it will be saved to the applications temporary directory.</remarks>
        //public static string ImageFilesDirectory = Path.Combine(SettingsManager.TempDirectoryPath, "Image Files");
        public static string ImageFilesDirectory = Path.Combine(SettingsManager.TempDirectoryPath, "Image_Files");

        /// <summary>True to generate a reduced size preview image.</summary>
        /// <remarks>
        /// If false, all requests for preview image will redirect to the original image.
        /// Value retrieved from/stores to UserSetting Settings.SQImageRevision.GeneratePreviewImage.
        /// Forces save to Settings.SQImageRevision.
        /// </remarks>
        public static bool GeneratePreviewImage
		{
			get { return DefaultSettings.GeneratePreviewImage; }
			set
			{
				DefaultSettings.GeneratePreviewImage = value; 
				DefaultSettings.Save();
			}
		}
		
		#endregion Static Properties
		
		#region Instance Properties		
		
		/// <summary>Path to the thumbnail image file.</summary>
		public string ThumbnailImageFilePath { get; set; }
		
		/// <summary>Path to the preview image file.</summary>
		/// <remarks>When GeneratePreviewImage is false, this will be assigned the same value as OriginalFilePath</remarks>
		private string _PreviewImageFilePath = null;
		public string PreviewImageFilePath
		{
			get 
			{ 
				if (string.IsNullOrEmpty(_PreviewImageFilePath))
				{ return OriginalImageFilePath; }
				else
				{ return _PreviewImageFilePath; }
			}
			set { _PreviewImageFilePath = value; }
		}
		
		/// <summary>Path to the original (full size) image file.</summary>
		public string OriginalImageFilePath { get; set; }
		
		#endregion InstanceProperties
		
		#region Constructors
				
		/// <summary>Serialization constructor.  Not intended for normal use.</summary>
		/// <remarks>
		/// Refer to the class documentation for warnings on temporary files and disposing.
		/// Caller MUST immediately populate Thumbnail/Preview/Original ImageFilePath properties.
		/// </remarks>
		public SQImageRevision() { }
		
		/// <remarks>
		/// Refer to the class documentation for warnings on temporary files and disposing
		/// SQImageRevision does not retain any reference or dependency to fib after creation.
		/// fib can be immediately disposed by caller after creation.
		/// </remarks>
		public SQImageRevision(FreeImageBitmap fib)
		{
			// Performance Testing Results (BW was 300DPI 8.5"x11", Color was 24Bpp 300 DPI 8.5"x14")
			//	Task		PreviewOn		PixelType		Time(ms)
			//	N			Y					Color				826		
			//	Y			Y					Color				492
			//	Y			N					Color				477
			//	N			Y					BW					374
			//	Y			Y					BW					306			
			
			//Initialize task to save the original image
			Task originalImageTask = new Task(() =>
			{
                OriginalImageFilePath = WriteImageToTemporaryFile(fib);
                Edocs_Utilities.EdocsUtilitiesInstance.CopyBackUpOrgImage(OriginalImageFilePath);

            });

			//Initialize task to save the preview image
			Task previewImageTask = new Task(() =>
         {
         	//If requested, generate a preview image file, otherwise, set the path to the original image file path
				if (GeneratePreviewImage)
				{ PreviewImageFilePath = WriteScaledImageToTemporaryFile(fib, PreviewImageMaxSize, true); }			                                 	
			});
			
			//Inialize task to save the thumbnail image
			Task thumbnailImageTask = new Task(() =>
			{ ThumbnailImageFilePath = WriteScaledImageToTemporaryFile(fib, ThumbnailImageMaxSize, false); });
			
			//Execute the tasks.
			//WriteImageToTemporaryFile does not modify the original image, 
			//Running the tasks should not cause any conflicts, more testing required to confirm
			//TODO: Test for conflict
			originalImageTask.Start();
			previewImageTask.Start();
			thumbnailImageTask.Start();
			//Wait for all of the image files be written.
			//Timeout not needed (if the operations fail, exception will be thrown).
			Task.WaitAll(originalImageTask, previewImageTask, thumbnailImageTask);
		}
		
		#endregion Constructors
		
		#region Image Access
		
		/// <summary>Get a copy of the original image.</summary>
		/// <remarks>
		/// Caller is responsible for disposing image.
		/// Does not create a lock on associated image file.
		/// </remarks>
		public FreeImageBitmap GetOriginalImage()
		{ return GetFreeImageBitmap(OriginalImageFilePath); }

		/// <summary>Get a copy of the original image.</summary>
		/// <remarks>
		/// Caller is responsible for disposing image.
		/// Does not create a lock on associated image file.
		/// </remarks>
		public Bitmap GetOriginalImageBitmap()
		{ return GetBitmap(OriginalImageFilePath); }
		
		/// <summary>Get a copy of the preview image.</summary>
		/// <remarks>
		/// Caller is responsible for disposing image.
		/// Does not create a lock on associated image file.
		/// If GeneratePreviewImage is false, will return a copy of the original image.
		/// </remarks>
		public FreeImageBitmap GetPreviewImage()
		{ return GeneratePreviewImage ? GetFreeImageBitmap(PreviewImageFilePath) : GetFreeImageBitmap(OriginalImageFilePath); }

		/// <summary>Get a copy of the preview image.</summary>
		/// <remarks>
		/// Caller is responsible for disposing image.
		/// Does not create a lock on associated image file.
		/// If GeneratePreviewImage is false, will return a copy of the original image.
		/// </remarks>		
		public Bitmap GetPreviewImageBitmap()
		{ return GeneratePreviewImage ? GetBitmap(PreviewImageFilePath) : GetBitmap(OriginalImageFilePath); }
		
		/// <summary>Get a copy of the thumbnail image.</summary>
		/// <remarks>
		/// Caller is responsible for disposing image.
		/// Does not create a lock on associated image file.
		/// </remarks>
		public FreeImageBitmap GetThumbnailImage()
		{ return GetFreeImageBitmap(ThumbnailImageFilePath); }

		/// <summary>Get a copy of the thumbnail image.</summary>
		/// <remarks>
		/// Caller is responsible for disposing image.
		/// Does not create a lock on associated image file.
		/// </remarks>
		public Bitmap GetThumbnailBitmap()
		{ return GetBitmap(ThumbnailImageFilePath); }
		
		/// <summary>Read an image from the specified file path and return a Bitmap.</summary>
		/// <remarks>Does not generate a file lock on filePath.</remarks>
		protected Bitmap GetBitmap(string filePath)
		{ 
			//Loading a FreeImageBitmap and converting to a Bitmap is actually significantly faster
			//than just loading the bitmap from the file.
			using (FreeImageBitmap fib = GetFreeImageBitmap(filePath))
			{ return fib.ToBitmap(); }
		}
		
		/// <summary>Read an image from the specified file path and return a FreeImageBitmap.</summary>
		/// <remarks>Does not generate a file lock on filePath.</remarks>
		protected FreeImageBitmap GetFreeImageBitmap(string filePath)
		{
			//return new FreeImageBitmap(filePath);
			byte[] data = ReadFromTemporaryFile(filePath);
			return FreeImageBitmapExtensions.FromBytes(data);
		}
		#endregion Image Access
		
		#region File Access
		
		/// <summary>Read the binary contents of a temporary file.</summary>
		protected byte[] ReadFromTemporaryFile(string path)
		{ return TemporaryFile.ReadAllBytes(path); }
		
		/// <summary>Compresses and writes the image data to a random temporary file.</summary>
		/// <returns>Path to the temporary file.</returns>
		protected string WriteImageToTemporaryFile(FreeImageBitmap fib)
		{							
			//Determine the compression parameters.
			//TODO: Configurable format and flags?
			//TODO: Consider lossy compression for full color images.
			FREE_IMAGE_FORMAT format;
			FREE_IMAGE_SAVE_FLAGS flags;
			string fileExtension;
			if (fib.PixelFormat == PixelFormat.Format1bppIndexed) 
			{ 
				format = FREE_IMAGE_FORMAT.FIF_TIFF;
				flags = FREE_IMAGE_SAVE_FLAGS.TIFF_CCITTFAX4; 
				fileExtension = "tif";
			}
			else 
			{ 
				format = FREE_IMAGE_FORMAT.FIF_TIFF;
				flags = FREE_IMAGE_SAVE_FLAGS.TIFF_LZW;
				fileExtension = "tif";
			}
			
			//Compress the image.
			byte[] encodedImageData = fib.Save(format, flags);
		
			//Write the compressed image data to a random temporary file and return the file's path.
			string path = TemporaryFile.WriteAllBytesToRandom(encodedImageData, ImageFilesDirectory, fileExtension);
            ETL.TraceLoggerInstance.TraceInformation($"Saving compressed image data to path:{path}");
			return path;
		}
		
		/// <summary>Rescales the image , compresses it and writes it to a random temporary file.</summary>
		/// <param name="maxSize">Maximum size for the rescaled image.  Image proportion will be maintained.</param>
		/// <param name="scaleResolution">True to scale the image's resolution to maintain the original image's physical size.</param>
		/// <returns>Path to the temporary file.</returns>
		protected string WriteScaledImageToTemporaryFile(FreeImageBitmap fib, Size maxSize, bool scaleResolution)
		{
			using (FreeImageBitmap scaledFib = fib.GetScaledInstanceEx(maxSize, FREE_IMAGE_FILTER.FILTER_BOX, scaleResolution))
			{ return WriteImageToTemporaryFile(scaledFib); }
		}
		
		#endregion File Access
		
        /*
		#region Recovery
		
		public byte[] GetRecoveryData()
		{
			RecoveryData rData = new RecoveryData()
			{
				OriginalImageFilePath = this.OriginalImageFilePath,
				PreviewImageFilePath = this.PreviewImageFilePath,
				ThumbnailImageFilePath = this.ThumbnailImageFilePath
			};
			return Serializer.SerializeBinary(rData);
		}
		
		public static SQImageRevision FromRecoveryData(byte[] data)
		{
			RecoveryData rData = Serializer.Deserialize<RecoveryData>(data);
			SQImageRevision rev = new SQImageRevision()
			{
				OriginalImageFilePath = rData.OriginalImageFilePath,
				PreviewImageFilePath = rData.PreviewImageFilePath,
				ThumbnailImageFilePath = rData.ThumbnailImageFilePath
			};
			return rev;
		}
		
		#endregion Recovery
		*/
		#region IDisposable support

		/// <summary>Delete the temporary files from the file system.</summary>
		/// <remarks>Once disposed, recovery will no longer be available.</remarks>		
		public void Dispose() { Dispose(true); }
		
		/// <summary>Delete the temporary files from the file system.</summary>
		/// <remarks>Once disposed, recovery will no longer be available.</remarks>
		public void Dispose(bool disposing)
		{
            try
            {
                //Delete the original image.
                ETL.TraceLoggerInstance.TraceInformation($"Disposing SQImageRevision deleteing file:{OriginalImageFilePath}");
                TemporaryFile.Delete(OriginalImageFilePath);

                //Delete the preview image if it exists.
                if (PreviewImageFilePath.Equals(OriginalImageFilePath) == false)
                {
                    ETL.TraceLoggerInstance.TraceInformation($"Disposing SQImageRevision deleteing file:{PreviewImageFilePath}");
                    TemporaryFile.Delete(PreviewImageFilePath);
                }

                //Delete the thumbnail image
                ETL.TraceLoggerInstance.TraceInformation($"Disposing SQImageRevision deleteing file:{ThumbnailImageFilePath}");
                TemporaryFile.Delete(ThumbnailImageFilePath);
            }
            catch (Exception ex)
            { ETL.TraceLoggerInstance.TraceWarning("Error disposing SQImageRevision " + ex.Message); }
		}
		
		#endregion IDisposable support
	}
}
