using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using EdocsUSA.Controls;
using Scanquire.Public;
using Scanquire.Public.SQImageSource;

namespace DemoArchivers
{
	public class HumanResourcesArchiver : SQFilesystemArchiver
	{
		protected class HumanResourcesInputDialog : TableLayoutPanelInputDialog
		{
			public ValidatingTextBox<string> EmployeeID;
			public ComboBoxInputControl Category;
			
			private void InitializeComponent()
			{
				AddControl(new CaptionLabel("Employee ID"), 0, 0);
				EmployeeID = new ValidatingTextBox<string>()
				{
					RequiresValue = true,
					CharacterCasing = CharacterCasing.Upper
				};
				AddControl(EmployeeID, 1, 0);
				
				AddControl(new CaptionLabel("Category"), 0, 1);
				Category = new ComboBoxInputControl()
				{
					RequiresValue = true,
					Caption = string.Empty,
					CaptionWidth = 0
				};
				Category.ValueComboBox.Sorted = true;
				Category.ValueComboBox.DataSource = new List<string>() 
				                        {
				                        	"HIRING",
				                        	"MISC",
				                        	"PICTURES",
				                        	"IRS",
				                        	"TRAINING",
				                        	"CORRESPONDENCE",
				                        	"TERMINATION",
				                        	"FULL"
				                        };
				AddControl(Category, 1, 1);
			}
			
			public HumanResourcesInputDialog() : base(2, 2) {InitializeComponent(); }
			
			public void Clear()
			{
				Category.Clear();
			}
		}
		
		protected HumanResourcesInputDialog InputDialog = new HumanResourcesInputDialog();
		
		private string _LogHeader = "Employee ID,Category,Page Count,Checksum,User Name,Timestamp";
		public override string LogHeader
		{
			get { return _LogHeader; }
			set { _LogHeader = value; }
		}
		
		public HumanResourcesArchiver()
		{
		}
		
		public override DoWorkEventHandler NewFromScanner()
		{
			InputDialog.Clear();
			if (InputDialog.ShowDialog(Global.HostWindow) == DialogResult.OK)
			{ 
				Application.DoEvents();
				return base.NewFromScanner(); 
			}
			else throw new OperationCanceledException();	
		}
		
		public override void Send(SQFile file)
		{
			if (InputDialog.ShowDialog(Global.HostWindow) == DialogResult.OK)
			{
				string relativeDirectory = InputDialog.EmployeeID.Value;
				string fileName = InputDialog.EmployeeID.Value
					+ "_" + InputDialog.Category.ValueText;
				fileName = Path.ChangeExtension(fileName, file.FileExtension);
				string filePath = Path.Combine(RootPath, relativeDirectory, fileName);
				
				string logEntry = InputDialog.EmployeeID.Value
					+ "," + InputDialog.Category.ValueText
					+ "," + file.PageCount
					+ "," + file.Checksum
					+ "," + Environment.UserName
					+ "," + DateTime.Now.ToString();
				
				Send(filePath, file.Data);
				WriteLogEntry(logEntry);
			}
			else throw new OperationCanceledException();				
		}
	}
}
