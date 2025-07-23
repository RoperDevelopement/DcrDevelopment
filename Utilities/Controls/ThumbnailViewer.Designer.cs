/*
 * User: Sam Brinly
 * Date: 1/14/2014
 */
namespace EdocsUSA.Utilities.Controls
{
	partial class ThumbnailViewer
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
			this.VScrollBar = new EdocsUSA.Utilities.Controls.ThumbnailViewer_VScrollBar();
			this.ThumbnailPanel = new EdocsUSA.Utilities.Controls.ThumbnailViewer_ThumbnailPanel();
			this.SuspendLayout();
			// 
			// VScrollBar
			// 
			this.VScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
			this.VScrollBar.Location = new System.Drawing.Point(127, 0);
			this.VScrollBar.Name = "VScrollBar";
			this.VScrollBar.Size = new System.Drawing.Size(23, 150);
			this.VScrollBar.TabIndex = 0;
			// 
			// ThumbnailPanel
			// 
			this.ThumbnailPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ThumbnailPanel.Location = new System.Drawing.Point(0, 0);
			this.ThumbnailPanel.Name = "ThumbnailPanel";
			this.ThumbnailPanel.Size = new System.Drawing.Size(127, 150);
			this.ThumbnailPanel.TabIndex = 1;
			// 
			// ThumbnailViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ThumbnailPanel);
			this.Controls.Add(this.VScrollBar);
			this.Name = "ThumbnailViewer";
			this.ResumeLayout(false);
		}
		private EdocsUSA.Utilities.Controls.ThumbnailViewer_ThumbnailPanel ThumbnailPanel;
		private EdocsUSA.Utilities.Controls.ThumbnailViewer_VScrollBar VScrollBar;
	}
}
