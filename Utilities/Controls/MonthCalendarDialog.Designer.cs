/*
 * User: Sam Brinly
 * Date: 10/9/2014
 */
namespace EdocsUSA.Utilities.Controls
{
	partial class MonthCalendarDialog
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.MonthCalendar = new System.Windows.Forms.MonthCalendar();
			this.SuspendLayout();
			// 
			// MonthCalendar
			// 
			this.MonthCalendar.Location = new System.Drawing.Point(2, 2);
			this.MonthCalendar.MaxSelectionCount = 1;
			this.MonthCalendar.Name = "MonthCalendar";
			this.MonthCalendar.TabIndex = 0;
			this.MonthCalendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.MonthCalendar_DateChanged);
			this.MonthCalendar.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.MonthCalendar_DateSelected);
			// 
			// MonthCalendarDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(230, 167);
			this.Controls.Add(this.MonthCalendar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "MonthCalendarDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select a Date";
			this.ResumeLayout(false);
		}
		public System.Windows.Forms.MonthCalendar MonthCalendar;
	}
}
