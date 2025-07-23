/*
 * User: Sam Brinly
 * Date: 10/9/2014
 */
namespace EdocsUSA.Utilities.Controls
{
	partial class DatePicker
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
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
			this.txtValue = new System.Windows.Forms.TextBox();
			this.btnShowDialog = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtValue
			// 
			this.txtValue.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtValue.Location = new System.Drawing.Point(0, 0);
			this.txtValue.Margin = new System.Windows.Forms.Padding(0);
			this.txtValue.Name = "txtValue";
			this.txtValue.Size = new System.Drawing.Size(69, 20);
			this.txtValue.TabIndex = 0;
			// 
			// btnShowDialog
			// 
			this.btnShowDialog.AutoSize = true;
			this.btnShowDialog.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnShowDialog.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnShowDialog.Location = new System.Drawing.Point(69, 0);
			this.btnShowDialog.Margin = new System.Windows.Forms.Padding(0);
			this.btnShowDialog.Name = "btnShowDialog";
			this.btnShowDialog.Size = new System.Drawing.Size(26, 20);
			this.btnShowDialog.TabIndex = 1;
			this.btnShowDialog.Text = "...";
			this.btnShowDialog.UseVisualStyleBackColor = true;
			this.btnShowDialog.Click += new System.EventHandler(this.BtnShowDialog_Click);
			// 
			// DatePicker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtValue);
			this.Controls.Add(this.btnShowDialog);
			this.Name = "DatePicker";
			this.Size = new System.Drawing.Size(95, 20);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button btnShowDialog;
		private System.Windows.Forms.TextBox txtValue;
	}
}
