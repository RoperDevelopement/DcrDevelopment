/*
 * User: Sam Brinly
 * Date: 10/9/2014
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EdocsUSA.Utilities.Controls
{
	/// <summary>
	/// Description of MonthCalendarDialog.
	/// </summary>
	public partial class MonthCalendarDialog : Form
	{
		public MonthCalendarDialog()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		

		
		void MonthCalendar_DateChanged(object sender, DateRangeEventArgs e)
		{
			
		}
		
		void MonthCalendar_DateSelected(object sender, DateRangeEventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}
	}
}
