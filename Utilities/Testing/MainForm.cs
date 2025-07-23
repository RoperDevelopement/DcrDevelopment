/*
 * User: Sam
 * Date: 10/10/2011
 * Time: 10:26 AM
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using EdocsUSA.Controls;
using EdocsUSA.Utilities;

namespace Testing
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		
		
		public MainForm()
		{
			InitializeComponent();
			
			List<Tuple<string, string>> barcodes = new List<Tuple<string, string>>();
			barcodes.Add(new Tuple<string, string>("TESTING TESTING", "TESTING TESTING"));
			barcodes.Add(new Tuple<string, string>("TESTING TESTING", "TESTING TESTING"));
			barcodes.Add(new Tuple<string, string>("TESTING TESTING", "TESTING TESTING"));
			barcodes.Add(new Tuple<string, string>("TESTING TESTING", "TESTING TESTING"));
			barcodes.Add(new Tuple<string, string>("TESTING TESTING", "TESTING TESTING"));
			barcodes.Add(new Tuple<string, string>("TESTING TESTING", "TESTING TESTING"));
			

			
			return;
			
			INIFile ini = new INIFile(@"C:\Code\Scanquire\Build\Scanquire.ini");
			//MessageBox.Show(ini.ReadStructKey<bool>("Settings", "LocalPDFArchiverEnabled", null).ToString());
			//MessageBox.Show(ini.ReadStructKey<bool>("Settings", "LocalTIFArchiverEnabled", null).ToString());
			//MessageBox.Show(ini.ReadStructKey<bool>("Settings", "LocalIMGArchiverEnabled", null).ToString());
			
			BoolInput.ValidatingType = typeof(bool);
			BoolInput.RequiresValue = true;
			BoolInput.HintText = "True|False";
			BoolNInput.ValidatingType = typeof(bool?);
			BoolNInput.RequiresValue = true;
			IntInput.ValidatingType = typeof(int);
			IntNInput.ValidatingType = typeof(int?);
			DateInput.ValidatingType = typeof(DateTime);
			StringInput.ValidatingType = typeof(string);
			StringInput.ValueTextBox.Multiline = true;
			comboBoxInputControl1.RequiresValue = true;
			comboBoxInputControl1.Items = new List<string>() { "TEST1", "TEST2", "TEST3", "TEST0" };
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			MessageLabel.Text = this.ValidateChildren().ToString();
			MessageBox.Show(comboBoxInputControl1.ValueText);
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			InputDialog dlg = new InputDialog();
			dlg.MessageLabel.Text = "Enter the following information " + Environment.NewLine + "Then pres [OK]";
			InputControl boolInput = new InputControl();
			boolInput.Caption = "bool";
			boolInput.ValidatingType = typeof(bool?);
			boolInput.HintText = "True|False";
			boolInput.RequiresValue = false;
			InputControl intInput = new InputControl();
			intInput.Caption = "int";
			intInput.ValidatingType = typeof(int?);
			intInput.RequiresValue = false;
			dlg.LayoutPanel.Controls.Add(boolInput,0,0);
			dlg.LayoutPanel.Controls.Add(intInput, 1, 0);
			
			InputControl firstName = new InputControl();
			firstName.ValidatingType = typeof(string);
			firstName.Caption = "Name";
			firstName.RequiresValue = false;
			InputControl lastName = new InputControl();
			lastName.Caption="";
			lastName.RequiresValue = false;
			lastName.ValidatingType = typeof(string);
			dlg.LayoutPanel.Controls.Add(firstName,0,1);
			dlg.LayoutPanel.Controls.Add(lastName, 1,1);
			
			InputControl ssn = new InputControl();
			ssn.Caption = "SSN";
			ssn.ValidatingType = typeof(string);
			ssn.RequiresValue = false;
			ssn.RegEx = @"^\d{3}-\d{2}-\d{4}$";
			ssn.HintText = "111-11-1111";
			dlg.LayoutPanel.Controls.Add(ssn, 1, 2);
			
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				MessageLabel.Text= boolInput.GetValue<bool?>().ToString();
			}
		}
	}
}
