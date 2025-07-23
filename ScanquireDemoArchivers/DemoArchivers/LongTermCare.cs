using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EdocsUSA.Controls;
using Scanquire.Public;
using Scanquire.Public.UserControls;

using EdocsUSA.Utilities.Extensions;
using System.Threading.Tasks;

namespace DemoArchivers
{
	public class LongTermCareArchiver : SQFilesystemArchiver
	{	
		protected class LongTermCareInputDialog : TableLayoutPanelInputDialog
		{
			public ValidatingTextBox<string> ClientID;
			public ValidatingTextBox<string> FirstName;
			public ValidatingTextBox<string> LastName;
			public ValidatingTextBox<DateTime> IntakeDate;
			public ValidatingTextBox<DateTime> DischargeDate;
            public ValidatingTextBox<string> DocumentType;
            public ValidatingTextBox<DateTime> DocumentDate;
			
			private void InitializeCompoenet()
			{
				this.Size = new System.Drawing.Size(550, 250);
				
				AddControl(new CaptionLabel("Client ID"));
				ClientID = new ValidatingTextBox<string>() {
					RequiresValue = true,
					CharacterCasing = CharacterCasing.Upper
				};
				AddControl(ClientID);
				
				AddControl(new CaptionLabel("Last Name"));
				LastName = new ValidatingTextBox<string>() {
					RequiresValue = true,
					CharacterCasing = CharacterCasing.Upper
				};
				AddControl(LastName);
				
				AddControl(new CaptionLabel("First Name"));
				FirstName = new ValidatingTextBox<string>() {
					RequiresValue = true,
					CharacterCasing = CharacterCasing.Upper
				};
				AddControl(FirstName);
				
				AddControl(new CaptionLabel("Intake Date"));
                IntakeDate = new ValidatingTextBox<DateTime>()
                {
					RequiresValue = true,
				};
                AddControl(IntakeDate);
				
				AddControl(new CaptionLabel("Discharge Date"));
				DischargeDate = new ValidatingTextBox<DateTime>(){
					RequiresValue = true
				};
				AddControl(DischargeDate);

                AddControl(new CaptionLabel("Document Type"));
                DocumentType = new ValidatingTextBox<string>() { RequiresValue = true, CharacterCasing = CharacterCasing.Upper };
                AddControl(DocumentType);

                AddControl(new CaptionLabel("Document Date"));
                DocumentDate = new ValidatingTextBox<DateTime>() { RequiresValue = true };
			}
			
			public LongTermCareInputDialog() : base (7, 2)
			{ InitializeCompoenet(); }
			
			protected override void OnShown(EventArgs e)
			{
				if (ClientID.HasValue)
				{
                    ClientID.SelectAll();
                    ClientID.Focus();
				}
				else
				{
                    ClientID.SelectAll();
                    ClientID.Focus();
				}
			}
			
			public void Clear()
			{
                IntakeDate.Clear();
				DischargeDate.Clear();
                DocumentType.Clear();
                DocumentDate.Clear();
			}
		}

		protected LongTermCareInputDialog InputDialog = new LongTermCareInputDialog();

		
		public LongTermCareArchiver()
		{
		}

        public override IEnumerable<System.Threading.Tasks.Task<SQImage>> AcquireForNew(SQAcquireSource source, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cToken)
        {
            if (InputDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { return base.AcquireForNew(source, progress, cToken); }
            else
            { throw new OperationCanceledException(); }
        }

        public override async System.Threading.Tasks.Task Send(SQFile file, int fileNumber, IProgress<EdocsUSA.Utilities.ProgressEventArgs> progress, System.Threading.CancellationToken cancelToken)
		{
            this.InputDialog.TryShowDialog(DialogResult.OK);
            
			string relativeDirectory = InputDialog.ClientID.Value 
				+ "_" + InputDialog.LastName.Value 
				+ "_" + InputDialog.FirstName.Value;
            string fileName = InputDialog.ClientID.Value
                + "_" + InputDialog.LastName.Value
                + "_" + InputDialog.FirstName.Value
                + "_" + InputDialog.IntakeDate.Value.ToString("yyyyMMdd")
                + "_" + (InputDialog.DischargeDate.HasValue ? InputDialog.DischargeDate.Value.ToString("yyyyMMdd") : string.Empty)
                + "_" + InputDialog.DocumentType.Value
                + "_" + InputDialog.DocumentDate.Value.ToString("yyyyMMdd");
			fileName = Path.ChangeExtension(fileName, file.FileExtension);
			string relativeFilePath = Path.Combine(relativeDirectory, fileName);

            await FilesystemConnector.SaveFile(relativeFilePath, file.Data);

            Dictionary<string, string> logEntry = new Dictionary<string, string>();
            logEntry["Patient ID"] = InputDialog.ClientID.Value;
            logEntry["Last Name"] = InputDialog.LastName.Value;
            logEntry["First Name"] = InputDialog.FirstName.Value;
            logEntry["Admit Date"] = InputDialog.IntakeDate.Value.ToShortDateString();
            logEntry["Discharge Date"] = (InputDialog.DischargeDate.HasValue ? InputDialog.DischargeDate.Value.ToShortDateString() : string.Empty);
            logEntry["Document Type"] = InputDialog.DocumentType.Value;
            logEntry["Document Date"] = InputDialog.DocumentDate.Value.ToShortDateString();

            logEntry["Page Count"] = file.PageCount.ToString();
            logEntry["Checksum"] = file.Checksum;
            logEntry["User Name"] = Environment.UserName;
            logEntry["Timestamp"] = DateTime.Now.ToUniversalTime().ToLongTimeString();
            logEntry["File Path"] = relativeFilePath;
            Log.Append(logEntry);

            Dictionary<string, string> spEntry = new Dictionary<string, string>();
            spEntry["Client_x0020_ID"] = InputDialog.ClientID.Value;
            spEntry["Last_x0020_Name"] = InputDialog.LastName.Value;
            spEntry["First_x0020_Name"] = InputDialog.FirstName.Value;
            spEntry["Intake_x0020_Date"] = InputDialog.IntakeDate.Value.Date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            spEntry["Discharge_x0020_Date"] = InputDialog.DischargeDate.Value.Date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            spEntry["Document_x0020_Type"] = InputDialog.DocumentType.Value;
            spEntry["Document_x0020_Date"] = InputDialog.DocumentDate.Value.Date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");


		}
	}
}