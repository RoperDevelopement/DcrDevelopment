/*
 * Created by SharpDevelop.
 * User: Sam Brinly
 * Date: 4/5/2012
 * Time: 9:01 AM
 */
namespace EdocsUSA.Controls
{
	partial class ComboBoxInputControl
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
			this.ErrorPanel = new System.Windows.Forms.Panel();
			this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.ValueComboBox = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// CaptionLabel
			// 
			this.CaptionLabel.Dock = System.Windows.Forms.DockStyle.Left;
			this.CaptionLabel.Location = new System.Drawing.Point(0, 0);
			this.CaptionLabel.Name = "CaptionLabel";
			this.CaptionLabel.Size = new System.Drawing.Size(43, 20);
			this.CaptionLabel.TabIndex = 3;
			this.CaptionLabel.Text = "Caption";
			this.CaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ErrorPanel
			// 
			this.ErrorPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.ErrorPanel.Location = new System.Drawing.Point(125, 0);
			this.ErrorPanel.Name = "ErrorPanel";
			this.ErrorPanel.Size = new System.Drawing.Size(25, 20);
			this.ErrorPanel.TabIndex = 5;
			// 
			// ErrorProvider
			// 
			this.ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
			this.ErrorProvider.ContainerControl = this;
			// 
			// ValueComboBox
			// 
			this.ValueComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ValueComboBox.FormattingEnabled = true;
			this.ValueComboBox.Location = new System.Drawing.Point(43, 0);
			this.ValueComboBox.Margin = new System.Windows.Forms.Padding(0);
			this.ValueComboBox.Name = "ValueComboBox";
			this.ValueComboBox.Size = new System.Drawing.Size(82, 21);
			this.ValueComboBox.Sorted = true;
			this.ValueComboBox.TabIndex = 6;
			// 
			// ComboBoxInputControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ValueComboBox);
			this.Controls.Add(this.CaptionLabel);
			this.Controls.Add(this.ErrorPanel);
			this.Name = "ComboBoxInputControl";
			this.Size = new System.Drawing.Size(150, 20);
			this.Validating += new System.ComponentModel.CancelEventHandler(this.ComboBoxInputControlValidating);
			((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
			this.ResumeLayout(false);
		}
		public System.Windows.Forms.ComboBox ValueComboBox;
		private System.Windows.Forms.ErrorProvider ErrorProvider;
		private System.Windows.Forms.Panel ErrorPanel;
		public System.Windows.Forms.Label CaptionLabel;
	}
}
