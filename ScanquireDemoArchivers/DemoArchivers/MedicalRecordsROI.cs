using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EdocsUSA.Controls;
using EdocsUSA.Utilities;
using Scanquire.Public;
using Scanquire.Public.SQImageSource;
using Scanquire.Public.UserControls;

namespace DemoArchivers
{
	public class MedicalRecordsROIArchiver : SQFilesystemArchiver
	{
		protected class MedicalRecordsROIInputDialog : TableLayoutPanelInputDialog
		{			
			protected ErrorProvider ErrorProvider = new ErrorProvider();
			
			public ValidatingTextBox<string> PatientID;
			public ValidatingTextBox<string> Requestor;
			public ValidatingTextBox<DateTime> RequestDate;
			
			private void InitializeComponent()
			{
				this.Size = new System.Drawing.Size(300, 300);
				
				AddControl(new CaptionLabel("Patient ID"));
				PatientID = new ValidatingTextBox<string>(){
					RequiresValue = true,
					CharacterCasing = CharacterCasing.Upper
				};
				AddControl(PatientID);
				
				AddControl(new CaptionLabel("Requestor"));
				Requestor = new ValidatingTextBox<string>() {
					RequiresValue = true,
					CharacterCasing = CharacterCasing.Upper
				};
				AddControl(Requestor);
				
				AddControl(new CaptionLabel("Request Date"));
				RequestDate = new ValidatingTextBox<DateTime>() {
					RequiresValue = true,
				};
				AddControl(RequestDate);	
			}
			
			public MedicalRecordsROIInputDialog() : base(2, 4)
			{ InitializeComponent(); }
			
			protected override void OnShown(EventArgs e)
			{
				base.OnShown(e);
				PatientID.SelectAll();
				PatientID.Focus();
			}
			
			public void Clear()
			{
				
			}
		}
		
		private string _LogHeader = "Patient ID,Requestor,Request Date,Page Count,Checksum,User Name,Timestamp";
		public override string LogHeader
		{
			get { return _LogHeader; }
			set { _LogHeader = value; }
		}
		
		protected MedicalRecordsROIInputDialog InputDialog = new MedicalRecordsROIInputDialog();
		
		private string _EncryptedFileWriterName = null;
		public string EncryptedFileWriterName
		{
			get 
			{
				if (_EncryptedFileWriterName == null)
				{ _EncryptedFileWriterName = "PDF ENCRYPTED"; }
				return _EncryptedFileWriterName;
			}
			set 
			{ _EncryptedFileWriterName = value; }
		}
		
		protected SQFileWriter EncryptedFileWriter
		{
			get { return SQFileWriters.Instance [_EncryptedFileWriterName]; }
		}
		
		public override DoWorkEventHandler AcquireFromFile()
		{
			return ((MedicalRecordsArchiver)(SQArchivers.Instance["MEDICAL RECORDS"])).AcquireFromFile();
		}
		
		public override DoWorkEventHandler NewDocument(SQAcquireSource source)
		{
			InputDialog.Clear();
			if (InputDialog.ShowDialog(Global.HostWindow) == DialogResult.OK)
			{ return base.NewDocument(source); }
			else throw new OperationCanceledException();
		}
		
		public override void Send(IEnumerable<SQImage> images)
		{			
			if (InputDialog.ShowDialog(Global.HostWindow) == DialogResult.OK)
			{	
				string relativeFolderPath = Path.Combine(RootPath, InputDialog.PatientID.Value);
				Directory.CreateDirectory(relativeFolderPath);
				
				string password = StringTools.GenerateRandomString(8, 8);
				SQDocument document = DocumentBuilder.Build(images).First();
				if (document.OwnerPassword.IsEmpty())
				{ document.OwnerPassword = password; }
				SQFile recordFile = EncryptedFileWriter.Write(document);
				
				string recordFileName = InputDialog.PatientID.Value
					+ "_" + InputDialog.RequestDate.Value.ToString("yyyyMMdd")
					+ "_" + InputDialog.Requestor.Value;					
				
				string letterText = "Requestor: " + InputDialog.Requestor.Value
					+ Environment.NewLine
					+ "Date: " + InputDialog.RequestDate.Value.ToShortDateString()
					+ Environment.NewLine
	          	+ Environment.NewLine
		         + "CONFIDENTIALITY NOTICE."
					+ Environment.NewLine
	          	+ Environment.NewLine
					+ "The information contained in this transmission may contain confidential information that is legally privileged. This information is intended only for the use of the individual or entity named above. The authorized recipient of this information is prohibited from disclosing this information to any other party unless required to do so by law or regulation and is required to destroy the information after its need has been filled."
					+ Environment.NewLine
	          	+ Environment.NewLine
					+ "If you are not the intended recipient, you are hereby notified that any disclosure, copying, distribution, or action taken in reliance on the contents of these documents is strictly prohibited. If you have received this information in error, please notify the sender immediately and arrange for the return or destruction of these documents."
					+ Environment.NewLine
	          	+ Environment.NewLine
		         + "For any questions or concerns, please contact:"
		         + Environment.NewLine
		         + "Demo"
		         + Environment.NewLine
					+ "e-Docs USA."
					+ Environment.NewLine
					+ "107 E. Granite"
					+ Environment.NewLine
					+ "Butte Mt 59701"
					+ Environment.NewLine
					+ "(406) 723-8721"
					+ Environment.NewLine
					+ "This document contains requested records from Patient ID: " + InputDialog.PatientID.Value
					+ Environment.NewLine
					+ "This document has been encrypted and password protected"
					+ Environment.NewLine
					+ "The required password is: " + password;
				SQTextPage letterPage = new SQTextPage();
				letterPage.PageTextCommands.Add(new SQPageTextCommand(letterText));
				
				SQDocument letterDocument = new SQDocument(new List<SQPage>() {letterPage});
				SQFile letterFile = FileWriter.Write(letterDocument);					
				
				string letterFileName = recordFileName + "_letter";
				
				recordFileName = Path.ChangeExtension(recordFileName, recordFile.FileExtension);
				letterFileName = Path.ChangeExtension(letterFileName, letterFile.FileExtension);
				
				string recordFilePath = Path.Combine(relativeFolderPath, recordFileName);
				string letterFilePath = Path.Combine(relativeFolderPath, letterFileName);
				
				File.WriteAllBytes(recordFilePath, recordFile.Data);
				File.WriteAllBytes(letterFilePath, letterFile.Data);
				
				string logEntry = InputDialog.PatientID.Value
					+ "," + InputDialog.Requestor.Value
					+ "," + InputDialog.RequestDate.Value.ToShortDateString()
					+ "," + recordFile.PageCount
					+ "," + recordFile.Checksum
					+ "," + Environment.UserName
					+ "," + DateTime.Now.ToString();
				WriteLogEntry(logEntry);
				
			}
			else { throw new OperationCanceledException(); }
			
		}
		
		public override void Send(SQFile file)
		{ throw new NotImplementedException(); }
		
		public MedicalRecordsROIArchiver()
		{
		}
	}
}
