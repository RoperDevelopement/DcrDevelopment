using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using EdocsUSA.Utilities.Extensions;

namespace EdocsUSA.Controls
{
	public class TableLayoutPanelInputDialog : InputDialogBase
	{		
		public TableLayoutPanel TableLayoutPanel;
		/*
		protected TableLayoutPanel ControlPanel;
		public Button OKButton;
		public Button CancelDialogButton;
		protected Panel CaptionPanel;
		public Label CaptionLabel;
		
		public string Caption
		{
			get { return CaptionLabel.Text; }
			set { CaptionLabel.Text = value; }
		}
		*/
		
		private void InitializeComponent(int? colCount, int? rowCount)
		{			
			
			TableLayoutPanel = new TableLayoutPanel()
			{ Dock = DockStyle.Fill, };			
			if (colCount.HasValue && rowCount.HasValue)
			{
				TableLayoutPanel.ColumnCount = colCount.Value;
				TableLayoutPanel.RowCount = rowCount.Value + 1;
				foreach (int i in IntExtensions.Range(0, colCount.Value - 2))
				{ TableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); }
				//Make the last column take up the remaining space
				TableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
				
				foreach (int i in IntExtensions.Range(0, rowCount.Value - 1))
				{ TableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); }
				//Add an extra row to take up the remaining space
				TableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
			}
			ContentPanel.Controls.Add(TableLayoutPanel);
		}
		
		public void AddControl(Control control, int column, int row)
		{ TableLayoutPanel.Controls.Add(control, column, row); }
		
		public void AddControl(Control control)
		{ TableLayoutPanel.Controls.Add(control); }
		
		public TableLayoutPanelInputDialog() : base()
		{
			InitializeComponent(null, null);
		}
		
		public TableLayoutPanelInputDialog(int colCount, int rowCount) : base()
		{
			InitializeComponent(colCount, rowCount);
		}
	}
}
