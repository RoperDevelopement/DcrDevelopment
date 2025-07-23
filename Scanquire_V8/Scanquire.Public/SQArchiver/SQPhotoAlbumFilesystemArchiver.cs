using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using EdocsUSA.Controls;
using EdocsUSA.Utilities;
using FreeImageAPI;

namespace Scanquire.Public
{	
    /// <summary>Simple archiver for storing photographs to the local filesystem.</summary>
	public class SQPhotoAlbumFilesystemArchiver : SQArchiverBase
	{
		protected class PhotoAlbumInputDialog : TableLayoutPanelInputDialog
		{
			public ValidatingTextBox<string> AlbumName;
			public CheckBox LowQualityCopyCheckBox;
			public bool LowQualityCopy
			{ 
				get { return LowQualityCopyCheckBox.Checked; }
				set { LowQualityCopyCheckBox.Checked = value; }
			}
			
			private void InitializeComponent()
			{
				this.Size = new System.Drawing.Size(275, 300);
				
				AddControl(new CaptionLabel("Album Name"), 0, 0);
				AlbumName = new ValidatingTextBox<string>()
				{
					Width = 250,
					RequiresValue = true
				};
				AddControl(AlbumName, 1, 0);
				
				AddControl(new CaptionLabel("Low Quality Copy"), 0, 1);
				LowQualityCopyCheckBox = new CheckBox()
				{ Checked = false };
				AddControl(LowQualityCopyCheckBox, 1, 1);
			}
			
			public PhotoAlbumInputDialog() : base(2, 2)
			{
				InitializeComponent();
			}
			
			protected override void OnShown(EventArgs e)
			{
				base.OnShown(e);
				AlbumName.Focus();
			}
			
			public void Clear()
			{ AlbumName.Clear(); }
		}
		
		protected PhotoAlbumInputDialog InputDialog = new PhotoAlbumInputDialog();
		
		private string _RootPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        /// <summary>Root path to store archived photos to.</summary>
		public string RootPath
		{
			get { return _RootPath;  }
			set
			{ _RootPath = value; }
		}
		
		private FREE_IMAGE_FORMAT _HighQualityImageFormat = FREE_IMAGE_FORMAT.FIF_JPEG;
        /// <summary>Image format for storing images when high quality copies are requested.</summary>
		public FREE_IMAGE_FORMAT HighQualityImageFormat
		{
			get { return _HighQualityImageFormat; }
			set { _HighQualityImageFormat = value; }
		}
		
		private FREE_IMAGE_SAVE_FLAGS _HighQualityImageSaveFlags = FREE_IMAGE_SAVE_FLAGS.JPEG_QUALITYGOOD;
        /// <summary>Compression flags for storing images when high quality copies are requested.</summary>
		public FREE_IMAGE_SAVE_FLAGS HighQualityImageSaveFlags
		{ 
			get { return _HighQualityImageSaveFlags; }
			set { _HighQualityImageSaveFlags = value; }
		}
		
		private string _HighQualityImageExtension = "jpg";
        /// <summary>File extension to apply to images stored using the high quality settings</summary>
		public string HighQualityImageExtension
		{
			get { return _HighQualityImageExtension; }
			set { _HighQualityImageExtension = value; }
		}
		
		private FREE_IMAGE_FORMAT _LowQualityImageFormat = FREE_IMAGE_FORMAT.FIF_JPEG;
        /// <summary>Image format for storing images when low quality copies are requested.</summary>
		public FREE_IMAGE_FORMAT LowQualityImageFormat
		{
			get { return _LowQualityImageFormat; }
			set { _LowQualityImageFormat = value; }
		}
		
		private FREE_IMAGE_SAVE_FLAGS _LowQualityImageSaveFlags = FREE_IMAGE_SAVE_FLAGS.JPEG_QUALITYGOOD;
        /// <summary>Compression flags for storing images when low quality copies are requested.</summary>
		public FREE_IMAGE_SAVE_FLAGS LowQualityImageSaveFlags
		{ 
			get { return _LowQualityImageSaveFlags; }
			set { _LowQualityImageSaveFlags = value; }
		}		

		private string _LowQualityImageExtension = "jpg";
        /// <summary>File extension to apply to images stored using the low quality settings.</summary>
		public string LowQualityImageExtension
		{
			get { return _LowQualityImageExtension; }
			set { _LowQualityImageExtension = value; }
		}
		
		public SQPhotoAlbumFilesystemArchiver()
		{ }
		
        /// <summary>Intended for use with single page image files only, so all processing will occur at the send images level.</summary>
		public override void Send(SQFile file) { throw new NotImplementedException(); }
		
		public override void Send(IEnumerable<SQImage> images)
		{
            //Prompt user for settings
			InputDialog.Clear();
			if (InputDialog.ShowDialog(Global.HostWindow) != DialogResult.OK)
			{ throw new OperationCanceledException(); }
			
            //Compile the name from the root path and the user inputed album name
			string albumName = InputDialog.AlbumName.Value;
			string albumPath = Path.Combine(RootPath, albumName);
			
            //If low quality copies are requested, create seperate high and low quality folders off of the main album directory
            //If low quality copies are not requested, just store everything in the main album directory
            bool lowQualityCopy = InputDialog.LowQualityCopy;
			string highQualityAlbumPath;
			if (lowQualityCopy == true) 
			{ highQualityAlbumPath = Path.Combine(albumPath, "High Quality"); }
			else { highQualityAlbumPath = albumPath; }
			Directory.CreateDirectory(highQualityAlbumPath);
			string lowQualityAlbumPath = Path.Combine(albumPath, "Low Quality");
			if (lowQualityCopy) Directory.CreateDirectory(lowQualityAlbumPath);
			
			int counter = 0;
			foreach (SQImage image in images)
			{
				string highQualityFilePath;
				string lowQualityFilePath;
                //Generate the base file name as {album name}_{counter padded to 4 digits} (ie: VACATION_0001)
                //If low quality images are requested, add the quality to the file name (ie: VACATION_0001_high & VACATION_0001_low)
                //To account for appending to existing albums, loop through the counter until a non-used file name is encountered.
                do
				{
					if (counter >= 9999)
					{
                        throw new Exception("Albums are limited to 10,000 images, please choose a new album");
                    }
					string fileName = albumName + "_" + counter.ToString().PadLeft(4, '0');
                    string highQualityFileName;
					if (lowQualityCopy == true)
					{ highQualityFileName = Path.ChangeExtension(fileName + "_high", HighQualityImageExtension); }
					else { highQualityFileName = Path.ChangeExtension(fileName, "jpg"); }
					highQualityFilePath = Path.Combine(highQualityAlbumPath, highQualityFileName);
					string lowQualityFileName = Path.ChangeExtension(fileName + "_low", LowQualityImageExtension);
					lowQualityFilePath = Path.Combine(lowQualityAlbumPath, lowQualityFileName);
					counter++;
				} while (File.Exists(highQualityFilePath) || File.Exists(lowQualityFilePath));
				
				int editID = image.BeginEdit();
                //Save the image files to disk.
				image.WorkingCopy.Save(highQualityFilePath, HighQualityImageFormat, HighQualityImageSaveFlags);
				if (lowQualityCopy)
				{ image.WorkingCopy.Save(lowQualityFilePath, LowQualityImageFormat, LowQualityImageSaveFlags); }
				
				image.DiscardEdit(editID);
				
			}
		}
		
	}
}
