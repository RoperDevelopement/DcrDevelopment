using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using EdocsUSA.Controls;
using Scanquire.Public;
using Scanquire.Public.Sharepoint;
using Scanquire.Public.SQImageSource;

namespace DemoArchivers
{
	public class DischargeSummaryArchiver : SQArchiverBase
	{		
		protected class DischargeSummaryInputDialog : TableLayoutPanelInputDialog
		{
			public ValidatingTextBox<string> PatientID;
			public ValidatingTextBox<string> FirstName;
			public ValidatingTextBox<string> LastName;
			public ValidatingTextBox<DateTime> AdmitDate;
			public ValidatingTextBox<DateTime> DischargeDate;
			
			private void InitializeCompoenet()
			{
				this.Size = new System.Drawing.Size(550, 250);
				
				AddControl(new CaptionLabel("Patient ID"));
				PatientID = new ValidatingTextBox<string>() {
					RequiresValue = true,
					CharacterCasing = CharacterCasing.Upper
				};
				AddControl(PatientID);
				
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
				
				AddControl(new CaptionLabel("Admit Date"));
				AdmitDate = new ValidatingTextBox<DateTime>(){
					RequiresValue = true,
				};
				AddControl(AdmitDate);
				
				AddControl(new CaptionLabel("Discharge Date"));
				DischargeDate = new ValidatingTextBox<DateTime>(){
					RequiresValue = true
				};
				AddControl(DischargeDate);
			}
			
			public DischargeSummaryInputDialog() : base (6, 2)
			{ InitializeCompoenet(); }
			
			protected override void OnShown(EventArgs e)
			{
				if (PatientID.HasValue)
				{
					AdmitDate.SelectAll();
					AdmitDate.Focus();
				}
				else
				{
					PatientID.SelectAll();
					PatientID.Focus();
				}
			}
			
			public void Clear()
			{
				AdmitDate.Clear();
				DischargeDate.Clear();
			}
		}
		
		DischargeSummaryInputDialog InputDialog = new DischargeSummaryInputDialog();
		
		SharepointConnector SharepointConnector;
		
		public DischargeSummaryArchiver()
		{
			SharepointConnector = SharepointConnectors.GetDemoConnector();
			SharepointConnector.APPHwnd = Global.HostWindow;
			SharepointConnector.LibraryName = "Discharge Summaries";
		}
		
		public override System.Collections.Generic.DoWorkEventHandler NewFromScanner()
		{
			if (InputDialog.ShowDialog(Global.HostWindow) != DialogResult.OK)
			{ throw new OperationCanceledException(); }
			return base.NewFromScanner();
		}
		
		public override void Send(SQFile file)
		{
			if (InputDialog.ShowDialog(Global.HostWindow) != DialogResult.OK)
			{ throw new OperationCanceledException(); }
			Application.DoEvents();
			
			Dictionary<string, object> fieldValues = new Dictionary<string, object>();
			fieldValues["Patient ID"] = InputDialog.PatientID.Value;
			fieldValues["Patient First Name"] = InputDialog.FirstName.Value;
			fieldValues["Patient Last Name"] = InputDialog.LastName.Value;
			fieldValues["Admit Date"] = InputDialog.AdmitDate.Value.ToShortDateString();
			fieldValues["Discharge Date"] = InputDialog.DischargeDate.Value.ToShortDateString();
			
			string fileName = InputDialog.PatientID.Value
				+ "_" + InputDialog.LastName.Value
				+ "_" + InputDialog.FirstName.Value
				+ "_" + InputDialog.AdmitDate.Value.ToString("yyyyMMdd")
				+ "_" + InputDialog.DischargeDate.Value.ToString("yyyyMMdd");
			fileName = Path.ChangeExtension(fileName, file.FileExtension);
			
			SharepointConnector.SendFile(file.Data, fileName, fieldValues);
			
		}
	}
}
