/*
 * User: Sam
 * Date: 10/10/2011
 * Time: 12:43 PM
 */
namespace EdocsUSA.Controls
{
	partial class InputDialog
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
			this.CommandPanel = new System.Windows.Forms.Panel();
			this.CancelDialogButton = new System.Windows.Forms.Button();
			this.OkButton = new System.Windows.Forms.Button();
			this.MessagePanel = new System.Windows.Forms.Panel();
			this.MessageLabel = new System.Windows.Forms.Label();
			this.LayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.CommandPanel.SuspendLayout();
			this.MessagePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// CommandPanel
			// 
			this.CommandPanel.Controls.Add(this.CancelDialogButton);
			this.CommandPanel.Controls.Add(this.OkButton);
			this.CommandPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.CommandPanel.Location = new System.Drawing.Point(0, 74);
			this.CommandPanel.Name = "CommandPanel";
			this.CommandPanel.Size = new System.Drawing.Size(304, 85);
			this.CommandPanel.TabIndex = 1;
			// 
			// CancelDialogButton
			// 
			this.CancelDialogButton.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.CancelDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelDialogButton.Location = new System.Drawing.Point(151, 5);
			this.CancelDialogButton.Name = "CancelDialogButton";
			this.CancelDialogButton.Size = new System.Drawing.Size(75, 75);
			this.CancelDialogButton.TabIndex = 1;
			this.CancelDialogButton.Text = "&Cancel";
			this.CancelDialogButton.UseVisualStyleBackColor = true;
			// 
			// OkButton
			// 
			this.OkButton.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.OkButton.Location = new System.Drawing.Point(70, 5);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(75, 75);
			this.OkButton.TabIndex = 0;
			this.OkButton.Text = "&OK";
			this.OkButton.UseVisualStyleBackColor = true;
			this.OkButton.Click += new System.EventHandler(this.OkButtonClick);
			// 
			// MessagePanel
			// 
			this.MessagePanel.AutoSize = true;
			this.MessagePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MessagePanel.Controls.Add(this.MessageLabel);
			this.MessagePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.MessagePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MessagePanel.Location = new System.Drawing.Point(0, 0);
			this.MessagePanel.Margin = new System.Windows.Forms.Padding(0);
			this.MessagePanel.Name = "MessagePanel";
			this.MessagePanel.Padding = new System.Windows.Forms.Padding(15);
			this.MessagePanel.Size = new System.Drawing.Size(304, 43);
			this.MessagePanel.TabIndex = 2;
			// 
			// MessageLabel
			// 
			this.MessageLabel.AutoSize = true;
			this.MessageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MessageLabel.Location = new System.Drawing.Point(15, 15);
			this.MessageLabel.Name = "MessageLabel";
			this.MessageLabel.Size = new System.Drawing.Size(0, 13);
			this.MessageLabel.TabIndex = 0;
			// 
			// LayoutPanel
			// 
			this.LayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.LayoutPanel.AutoSize = true;
			this.LayoutPanel.ColumnCount = 1;
			this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.LayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
			this.LayoutPanel.Location = new System.Drawing.Point(0, 46);
			this.LayoutPanel.MinimumSize = new System.Drawing.Size(300, 25);
			this.LayoutPanel.Name = "LayoutPanel";
			this.LayoutPanel.RowCount = 1;
			this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.LayoutPanel.Size = new System.Drawing.Size(300, 25);
			this.LayoutPanel.TabIndex = 3;
			// 
			// InputDialog
			// 
			this.AcceptButton = this.OkButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			this.ClientSize = new System.Drawing.Size(304, 159);
			this.Controls.Add(this.LayoutPanel);
			this.Controls.Add(this.MessagePanel);
			this.Controls.Add(this.CommandPanel);
			this.MinimumSize = new System.Drawing.Size(300, 38);
			this.Name = "InputDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.CommandPanel.ResumeLayout(false);
			this.MessagePanel.ResumeLayout(false);
			this.MessagePanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		public System.Windows.Forms.TableLayoutPanel LayoutPanel;
		public System.Windows.Forms.Label MessageLabel;
		protected System.Windows.Forms.Panel MessagePanel;
		protected System.Windows.Forms.Button OkButton;
		protected System.Windows.Forms.Button CancelDialogButton;
		private System.Windows.Forms.Panel CommandPanel;
	}
}
