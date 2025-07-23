/*
 * User: Sam
 * Date: 10/10/2011
 * Time: 12:43 PM
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace EdocsUSA.Controls
{
	/// <summary>
	/// Description of InputDialog.
	/// </summary>
	public partial class InputDialog : Form
	{
		public string Message 
		{
			get { return MessageLabel.Text; }
			set { MessageLabel.Text = value; }
		}
		
		public InputDialog() 
		{ 
			InitializeComponent(); 	
		}
		
		public InputDialog(int columnCount, int rowCount)
		{
			InitializeComponent();
			
			LayoutPanel.ColumnCount = columnCount;
			LayoutPanel.RowCount = rowCount;
			
			LayoutPanel.ColumnStyles.Clear();
			for (int i = 0; i < columnCount; i++)
			{ LayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); }
			
			LayoutPanel.RowStyles.Clear();
			for (int i = 0; i < rowCount; i++)
			{ LayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); }	
		}
		
		void OkButtonClick(object sender, EventArgs e)
		{ if (this.ValidateChildren()) this.DialogResult = DialogResult.OK; }
		
		public Control FindControl(string name) { return LayoutPanel.Controls.Find(name, true)[0]; }
		
		public void AddControl(Control control, int column, int row)
		{ LayoutPanel.Controls.Add(control, column, row); }
		
	}
}
