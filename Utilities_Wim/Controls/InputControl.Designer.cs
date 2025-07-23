/*
 * User: Sam
 * Date: 10/10/2011
 * Time: 9:41 AM
 */
 
namespace EdocsUSA.Controls
{
	partial class InputControl
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
			this.components = new System.ComponentModel.Container();
			this.CaptionLabel = new System.Windows.Forms.Label();
			this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.ValueTextBox = new System.Windows.Forms.TextBox();
			this.ErrorPanel = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// CaptionLabel
			// 
			this.CaptionLabel.Dock = System.Windows.Forms.DockStyle.Left;
			this.CaptionLabel.Location = new System.Drawing.Point(0, 0);
			this.CaptionLabel.Name = "CaptionLabel";
			this.CaptionLabel.Size = new System.Drawing.Size(43, 20);
			this.CaptionLabel.TabIndex = 0;
			this.CaptionLabel.Text = "Caption";
			this.CaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ErrorProvider
			// 
			this.ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
			this.ErrorProvider.ContainerControl = this;
			// 
			// ValueTextBox
			// 
			this.ValueTextBox.CausesValidation = false;
			this.ValueTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ValueTextBox.Location = new System.Drawing.Point(43, 0);
			this.ValueTextBox.Name = "ValueTextBox";
			this.ValueTextBox.Size = new System.Drawing.Size(82, 20);
			this.ValueTextBox.TabIndex = 1;
			// 
			// ErrorPanel
			// 
			this.ErrorPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.ErrorPanel.Location = new System.Drawing.Point(125, 0);
			this.ErrorPanel.Name = "ErrorPanel";
			this.ErrorPanel.Size = new System.Drawing.Size(25, 20);
			this.ErrorPanel.TabIndex = 2;
			// 
			// InputControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			this.Controls.Add(this.ValueTextBox);
			this.Controls.Add(this.ErrorPanel);
			this.Controls.Add(this.CaptionLabel);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.MinimumSize = new System.Drawing.Size(2, 20);
			this.Name = "InputControl";
			this.Size = new System.Drawing.Size(150, 20);
			this.Validating += new System.ComponentModel.CancelEventHandler(this.InputControlValidating);
			((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Panel ErrorPanel;
		public System.Windows.Forms.TextBox ValueTextBox;
		private System.Windows.Forms.ErrorProvider ErrorProvider;
		public System.Windows.Forms.Label CaptionLabel;
	}
}
