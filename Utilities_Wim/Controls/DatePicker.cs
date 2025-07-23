/*
 * User: Sam Brinly
 * Date: 10/9/2014
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EdocsUSA.Utilities.Controls
{
	/// <summary>
	/// Description of DatePicker.
	/// </summary>
	public partial class DatePicker : UserControl
	{
	
		protected MonthCalendarDialog MonthCalendarDialog = new MonthCalendarDialog();
		
		public override string Text
		{
			get { return txtValue.Text; }
			set 
			{ 
				if (string.IsNullOrWhiteSpace(value))
				{this.Clear();}
				else
				{
					DateTime date;
					if (DateTime.TryParse(value, out date))
					{ this.Value = date; }
					else
					{ throw new ArgumentException("Value must be empty or a valid DateTime"); }
				}
			}
		}
		
		private string _DisplayFormat = "MM/dd/yyyy";
		public string DisplayFormat
		{
			get { return _DisplayFormat; }
			set { _DisplayFormat= value; }
		}
		
		[Browsable(false)]
		public DateTime? Value
		{
			get 
			{ 
				DateTime value;
				if (TryGetValue(out value) == false)
				{return (DateTime?)null;}
				else return value;
			}
			set 
			{ 
				if (value.HasValue == false)
				{ Clear(); }
				else
				{ txtValue.Text = value.Value.ToString(this.DisplayFormat); }
			}
		}
			
		public bool HasValue
		{ get { return string.IsNullOrWhiteSpace(txtValue.Text) == false; } }
		
	
		public DatePicker()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public void Clear()
		{ txtValue.Clear();}
		
		public bool TryGetValue(out DateTime value)
		{ return DateTime.TryParse(txtValue.Text, out value); }
		
		void BtnShowDialog_Click(object sender, EventArgs e)
		{
			this.MonthCalendarDialog.StartPosition = FormStartPosition.Manual;
			this.MonthCalendarDialog.Location = 
				this.PointToScreen(new Point(txtValue.Left, txtValue.Bottom));
			if (this.MonthCalendarDialog.ShowDialog(this) == DialogResult.OK)
			{this.Value = this.MonthCalendarDialog.MonthCalendar.SelectionStart; }
		}
	}
}
