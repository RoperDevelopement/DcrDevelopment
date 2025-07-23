using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EdocsUSA.Utilities;
using FreeImageAPI;
using Polenter.Serialization;
using Scanquire.Public.Extensions;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public
{
	/// <summary>A single SQImage with revision control.</summary>
	public class SQImage : IDisposable, INotifyPropertyChanged
	{
		#region Static Properties
		
		/// <summary>Subdirectory to store image files in</summary>
		public string ImageFilesDirectory = SQImageRevision.ImageFilesDirectory;
		
		#endregion Static Properties
		
		#region InstanceProperties
		
		/*
		private string _RecoveryFilePath;
		/// <summary>Path to the recovery file.</summary>
		/// <remarks>
		/// Value is automatically generated if no previous value has been set.
		/// Set intended only for use during deserialization.
		/// </remarks>
		public string RecoveryFilePath
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_RecoveryFilePath))
				{ _RecoveryFilePath = TemporaryFile.CreateEmptyRandom(ImageFilesDirectory, "sqi"); }
				return _RecoveryFilePath;
			}
			set { _RecoveryFilePath = value; }
		}
		*/
		public bool SaveQaImage
        { get; set; }
		/// <summary>Stack containing all revisions of the image with the latest on top.</summary>
		protected Stack<SQImageRevision> Revisions = new Stack<SQImageRevision>();
		
		/// <summary>The latest SAVED revision.</summary>
		/// <remarks>Does NOT return WorkingCopy.</remarks>
		public SQImageRevision LatestRevision
		{
			get
			{
				EnsureRevisionsNotEmpty();
				return Revisions.Peek();
			}
		}
		
		public int RevisionCount { get { return Revisions.Count; } }
		
		private FreeImageBitmap _WorkingCopy;
		/// <summary>Copy of the latest revision kept in memory for editing.</summary>
		/// <remarks>
		/// Use BeginEdit to load and Save or DiscardEdit to release.
		/// To conserve memory, limit the number of images with working copies loaded and always Save or Discard and edits as soon as possible.
		/// Automatically disposes any previous value.
		/// If you directly assign a value to WorkingCopy, you are passing complete control of the value (including disposal) to the SQImage.
		/// </remarks>
		public FreeImageBitmap WorkingCopy
		{
			get { return _WorkingCopy; }
			set
			{
				//Dispose any previous value
				if (_WorkingCopy != null) _WorkingCopy.Dispose();
				_WorkingCopy = value;
			}
		}
		
		/// <summary>True if WorkingCopy is loaded (available for editing), false if not.</summary>
		public bool Editing { get { return _WorkingCopy != null; } }

        /// <summary>Set of all open edit locks for this image.</summary>
        protected HashSet<SQImageEditLock> EditLocks = new HashSet<SQImageEditLock>();

        /*
		/// <summary>List of all active edit operation IDs.</summary>
		protected List<int> editIds = new List<int>();
		
		/// <summary>Counter fo assigning new Edit IDs. </summary>
		protected int editIdCounter = 0;
		*/

		private Bitmap _Thumbnail;
		/// <summary>Thumbnail image of the latest revision.</summary>
		/// <remarks>
		/// Value is cached.
		/// Automatically disposes any previous value.
		/// </remarks>
		[ExcludeFromSerialization]
		public Bitmap Thumbnail
		{
			get
			{
				if (_Thumbnail == null)
				{ 
					EnsureRevisionsNotEmpty();
					_Thumbnail = LatestRevision.GetThumbnailBitmap(); 
				}
				return _Thumbnail;
			}
			protected set 
			{
				if (_Thumbnail != null) _Thumbnail.Dispose();
				_Thumbnail = value;
			}
		}
		
		#endregion InstanceProperties
		
		#region Events
		
		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged(string propertyName)
		{ 
			if (PropertyChanged != null) 
			{ PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }

		}
		
		/// <summary>Fired when a new revision is saved, or a revision is rolled back</summary>
		/// <remarks>Does not get fired on changes to WorkingCopy.</remarks>
		public event EventHandler RevisionsChanged;
		
		protected void NotifyRevisionsChanged()
		{ 				
			if (RevisionsChanged != null)
			{ RevisionsChanged(this, null); }
			NotifyPropertyChanged("Revisions");
		}
		
		#endregion Events
		
		#region Event Handlers
		
		protected void OnRevisionsChanged(object sender, EventArgs e)
		{
			//Clear the thumbnail.
			Thumbnail = null;
			
			//Update the recovery file.
			//SaveToRecoveryFile();
		}
		
		#endregion Event Handles
		
		#region Constructors & Destructors
		
		/// <summary>Base constructor, sets up internal event handlers.</summary>
		public SQImage()
		{
			RevisionsChanged += OnRevisionsChanged;
		}

        /// <summary>Creates a new instance and leaves the image open for editing</summary>
        /// <remarks>Caller is responsible for calling DiscardEdit with the returned editLick</remarks>
		/// <remarks>Creates a clone of image, so caller can modify/dispose original image.</remarks>		
		public SQImage(FreeImageBitmap image, out SQImageEditLock editLock) : this()
		{
            ETL.TraceLoggerInstance.TraceInformation($"Creating a new  new instance for image type:{image.ImageType.ToString()}");
            FreeImageBitmap imageClone = (FreeImageBitmap)image.Clone();
            editLock = BeginEdit(imageClone);
            Save(true);
		}

        /// <remarks>Creates a clone of image, so caller can modify/dispose original image.</remarks>		
		public SQImage(FreeImageBitmap image) : this()
		{
            FreeImageBitmap imageClone = (FreeImageBitmap)image.Clone();
            using (SQImageEditLock editLock = BeginEdit(imageClone))
            { Save(true); }
		}

        ~SQImage()
        { Dispose(false); }

		/*
		/// <summary>Generate an SQImage from a recovery file.</summary>
		/// <param name="path">Path to a file generated with SaveToRecoveryFile().</param>
		/// <remarks>
		/// Passing in a path to a file not created with the TemporaryFile api is not guaranteed to work.
		/// </remarks>
		public SQImage(string recoveryFilePath) : this()
		{
			//Deserialize the recovery file.
			//SharpSerializer does not work with Stacks, so recovery file should contain an array of revisions.
			byte[] recoveryData = TemporaryFile.ReadAllBytes(recoveryFilePath);
			SQImageRevision[] revisions = Serializer.Deserialize<SQImageRevision[]>(recoveryData);
			
			//Add the revisions directly to the stack (do not need to go through normal save operations).
			foreach (SQImageRevision revision in revisions) 
			{ Revisions.Push(revision); }
			
			//Set the recoveryFilePath to the provided value.
			//Risk of corruption if recovery process fails?  (Since recovery is not critical, doesn't matter)
			this.RecoveryFilePath = recoveryFilePath;
			
			//Finally alert that the image has been updated.
			//Results in an extra save operation.
			NotifyLatestRevisionChanged();		
		}
		*/
		
		#endregion Constructors & Destructors
		
		#region Revision Management & Editing
		
		/// <summary>Raises an InvalidOperationException if WorkingCopy has not been loaded for editing.</summary>
		protected void EnsureEditing()
		{
			if (Editing == false)
			{
                ETL.TraceLoggerInstance.TraceError("The requested operation requires the image be open for editing, use BeginEdit()");
                throw new InvalidOperationException("The requested operation requires the image be open for editing, use BeginEdit()");
            }				
		}
		
		/// <summary>Throws an InvalidOperationException if the image is open for editing.</summary>
		protected void EnsureNotEditing()
		{
			if (Editing == true)
			{
                ETL.TraceLoggerInstance.TraceError("The requested operation cannot be performed while the image is open for editing, use Save or DiscardEdit");
                throw new InvalidOperationException("The requested operation cannot be performed while the image is open for editing, use Save or DiscardEdit");
            }
		}
		
		/// <summary>Throws an InvalidOperationException if there are no revisions.</summary>
		protected void EnsureRevisionsNotEmpty()
		{
            if (RevisionCount == 0)
            {
                ETL.TraceLoggerInstance.TraceError("The requested operation cannot be preformed with an empty revision list.");
                throw new InvalidOperationException("The requested operation cannot be preformed with an empty revision list.");
            }
        }		
		
		/// <summary>Sets the provided image as the WorkingCopy and initializes an Edit operation.</summary>
		/// <returns>Edit lock, caller is reposible for disposing the edit lock.</returns>
		/// <remarks>Image is cloned, so caller is responsible for disposing passed in value.</remarks>
		protected SQImageEditLock BeginEdit(FreeImageBitmap image)
		{
			WorkingCopy = (FreeImageBitmap)image.Clone();
            return BeginEdit();
		}
		
		/// <summary>Begins an edit operation.  If WorkingCopy is not loaded, the latest revision will be loaded as the WorkingCopy.</summary>
        /// <returns>Edit lock, caller is reposible for disposing the edit lock.</returns>
		public SQImageEditLock BeginEdit()
		{
			if (WorkingCopy == null) WorkingCopy = LatestRevision.GetOriginalImage();
            SQImageEditLock editLock = new SQImageEditLock(this);
            ETL.TraceLoggerInstance.TraceInformation("Edit lock created " + editLock.Id.ToString());
            EditLocks.Add(editLock);
			return editLock;
		}
		
		/// <summary>Save the current WorkingCopy as a revision.</summary>
		/// <param name="overwritePreviousRevision">True to delete the previous revision, false to retain it.</param>
        public void Save(bool overwritePreviousRevision)
        {
            EnsureEditing();

            if ((overwritePreviousRevision) && (Revisions.Count > 0))
            { _RollbackSingleRevision(overwritePreviousRevision); }

            //Add the revision to the list
            Revisions.Push(new SQImageRevision(WorkingCopy));
            NotifyRevisionsChanged();
        }
		
		/// <summary>Save the current WorkingCopy as a new revision.</summary>
		public void Save()
		{ Save(false); }
		
		/// <summary>Releases a pending edit lock.</summary>
		public void DiscardEdit(SQImageEditLock editLock)
		{
            ETL.TraceLoggerInstance.TraceWarning("Discarding edit lock " + editLock.Id.ToString());
            
			EditLocks.Remove(editLock);
			
            //If no more edit locks remain, discard the working copy.
            if (EditLocks.Count == 0)
			{
                ETL.TraceLoggerInstance.TraceWarning("Discarding working copy");
				WorkingCopy = null; //Working copy gets disposed by the property.
			}
		}
		
		/// <summary>
		/// Discards the latest saved revision.
		/// </summary>
		/// <remarks>
		/// This only affects the latest SAVED revision, it has no impact on WorkingCopy
		/// For performance, does not raise the ImageChanged event.
		/// Can result in an empty revision stack.
		/// Should only be used if an immediate edit is anticipated.
		/// </remarks>
		protected void _RollbackSingleRevision(bool deleFiles)
		{
            ETL.TraceLoggerInstance.TraceInformation("Discards the latest saved revision RollbackSingleRevision");
            //If no revisions, log and ignore.
            if (RevisionCount < 1) 
			{
                ETL.TraceLoggerInstance.TraceWarning("_RollbackSingle called with " + Revisions.Count + " revisions, ignoring");
				return;
			}
			SQImageRevision rev = Revisions.Pop();
            ETL.TraceLoggerInstance.TraceInformation("Disposing RollbackSingleRevision " + (rev == null));
            if(deleFiles)
			    rev.Dispose();
		}
		
		/// <summary> Discards the latest saved revision. </summary>
		/// <remarks>
		/// This only affects the latest SAVED revision, it has no impact on WorkingCopy
		/// Cannot be used to rollback the first revision.
		/// </remarks>
		public void RollbackSingleRevision()
		{
            ETL.TraceLoggerInstance.TraceInformation("Discards the latest saved revision RollbackSingleRevision");
            //If no revisions, log and ignore.
            if (RevisionCount <= 1) 
			{
                ETL.TraceLoggerInstance.TraceWarning("RollbackSingle called with " + Revisions.Count + " revisions, ignoring");
            }
			
			Revisions.Pop().Dispose();
			NotifyRevisionsChanged();
		}
		
		/// <summary>Discards all but the primary revision.</summary>
		public void RollbackToOriginalRevision()
		{
            ETL.TraceLoggerInstance.TraceInformation("RollbackToOriginalRevision Discards all but the primary revision");
            //If no revisions, log and ignore.
            if (Revisions.Count <= 1) 
			{
                ETL.TraceLoggerInstance.TraceWarning("RollbackToOriginalRevision called with " + RevisionCount + " revsions, ignoring");
            }
			
			//Call the private rollback op.
			while (RevisionCount > 1) _RollbackSingleRevision(true);
			
			//Finaly fire the image changed event.
			NotifyRevisionsChanged();
		}
		
		#endregion Revision Management & Editing
		
		#region Recovery
		
		private class RecoveryData
		{
			public byte[][] RevisionsRecoveryData { get; set; }
		}

		/*
		public byte[] GetRecoveryData()
		{
			RecoveryData rData = new RecoveryData();
			rData.RevisionsRecoveryData = 
				(from SQImageRevision rev in Revisions.Reverse() select rev.GetRecoveryData()).ToArray();
			return Serializer.SerializeBinary(rData);
		}
		
		public static SQImage FromRecoveryData(byte[] value)
		{
			RecoveryData rData = Serializer.DeserializeBinary<RecoveryData>(value);
			SQImage image = new SQImage();
			foreach (byte[] revisionData in rData.RevisionsRecoveryData)
			{
				image.Revisions.Push(SQImageRevision.FromRecoveryData(revisionData));
			}
			image.NotifyRevisionsChanged();
			
			return image;
		}*/
		
		/*
		/// <summary>Store the revisions list to a temporary file that can be recovered in the event of application crash.</summary>
		/// <remarks>The recovery file is not deleted until SQImage is disposed.</remarks>
		protected void SaveToRecoveryFile()
		{
			//SharpSerializer does not work with Stacks, so convert it to an array before serializing
			SQImageRevision[] revisions = Revisions.ToArray();
			
			//Stack .ToArray returns items in FILI order, so reverse the list
			Array.Reverse(revisions);
			
			byte[] data = Serializer.Serialize(revisions);
			
			TemporaryFile.WriteAllBytes(RecoveryFilePath, data);
		}
		*/
		#endregion Recovery
		
		#region IDisposable Support
		
		/// <summary>Releases resources and deletes all temporary files associated with the SQImage.</summary>
		public void Dispose() { Dispose(true); }
		
		/// <summary>Releases resources and deletes all temporary files associated with the SQImage.</summary>
		/// <param name="disposing"></param>
		/// <remarks>Once an SQImage has been disposed, recovery is no longer available.</remarks>
		public void Dispose(bool disposing)
		{
            ETL.TraceLoggerInstance.TraceInformation("Disposing all resources and deletes all temporary files associated with the SQImage.");

            if (disposing)
			{
				Thumbnail = null; // Dispose the thumbnail object
			}
			
			while (RevisionCount > 0) 
			{
                ETL.TraceLoggerInstance.TraceWarning($"Rolllling back single image revisions:{RevisionCount.ToString()}");
                _RollbackSingleRevision(false);
			}
			//TemporaryFile.Delete(RecoveryFilePath);
		}
		
		#endregion IDisposable Support	
		
		#region Static Creators
		
        /// <summary>Create an SQImage from a .Net bitmap.</summary>
        /// <param name="bitmap">The bitmap to create the SQImage from.</param>
        /// <param name="disposeBitmap">
        /// If true, disposes the bitmap after creating the SQImage.
        /// If false, caller is responsible for disposing bitmap.
        /// </param>
        /// <returns>A new SQImage from the specified bitmap.</returns>
        /// <remarks>No reference to the provided bitmap is retained, it can be freely disposed.</remarks>
		public static SQImage FromBitmap(Bitmap bitmap, bool disposeBitmap)
		{
            ETL.TraceLoggerInstance.TraceInformation("Creating an SQImage from a .Net bitmap.");
            using (FreeImageBitmap fib = new FreeImageBitmap(bitmap))
			{ 
				SQImage sqImage = new SQImage(fib); 
				if (disposeBitmap)
				{ bitmap.Dispose(); }
				return sqImage;
			}
		}
		
		/// <summary>Create a new SQImage from a Bitmap</summary>
		/// <remarks>Caller is responsible for disposing bitmap.</remarks>
		public static SQImage FromBitmap(Bitmap bitmap)
		{ return FromBitmap(bitmap, false); }

        /// <summary>Create an SQImage from a .Net Image.</summary>
        /// <param name="bitmap">The Image to create the SQImage from.</param>
        /// <param name="disposeBitmap">
        /// If true, disposes the Image after creating the SQImage.
        /// If false, caller is responsible for disposing Image.
        /// </param>
        /// <returns>A new SQImage from the specified Image.</returns>
        /// <remarks>No reference to the provided Image is retained, it can be freely disposed.</remarks>
		public static SQImage FromImage(Image image, bool disposeImage)
		{ return FromBitmap((Bitmap)image); }
		
		/// <summary>Create a new SQImage from a image</summary>
		/// <remarks>Caller is responsible for disposing image.</remarks>		
		public static SQImage FromImage(Image image)
		{ return FromImage(image, false); }
		
        /// <summary>Create a new SQImage from a byte array.</summary>
        /// <param name="data"></param>
        /// <returns></returns>
		public static SQImage FromBytes(byte[] data)
		{
            ETL.TraceLoggerInstance.TraceInformation("Creating a new SQImage from a byte array");
            FreeImageBitmap fib = null;

			using (fib = FreeImageBitmapExtensions.FromBytes(data))
			{ return new SQImage(fib); }
		}
		
		/// <summary>Create a new SQImage from a dib handle (hDib).</summary>
		public static SQImage FromHDib(IntPtr hDib, bool freeHDib)
		{ return FromBytes(ImageTools.HDIBToBytes(hDib, freeHDib)); }
		
		/// <summary>Create a new SQImage from a dib handle (hDib).</summary>
		/// <remarks>Caller is responsible for releasing hDib.</remarks>
		public static SQImage FromHDib(IntPtr hDib)
		{ return FromHDib(hDib, false); }
		
		#endregion Static Creators
	}
}
