using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;

namespace EdocsUSA.Controls
{
	public class ExistingFileActionDialog : Form
	{
		protected Label CaptionLabel;
		
		protected TableLayoutPanel ButtonLayoutPanel;
		protected Panel ButtonPanel;
		protected Button OverwriteButton;
		protected Button NewRevisionButton;
		protected Button RetryButton;
		protected Button CancelSaveButton;
		protected ToolTip ToolTips;
		
		public ExistingFileAction ExistingFileAction { get; private set; }
		
		private void InitializeComponent()
		{
			this.MinimumSize = new Size(400, 175);
			this.AutoSize = true;
			
			this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			
			this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			this.Text = "File Exists";
			
			CaptionLabel = new Label()
			{ 
				Dock = DockStyle.Fill,
				Text = "The specified file already exists, please select from the following options",
				TextAlign = ContentAlignment.MiddleCenter,
				Font = SystemFonts.DialogFont
			};
			Controls.Add(CaptionLabel);
			
			ButtonPanel = new Panel()
			{
				Size = new Size(300, 75),
				Padding = new Padding(0),
				Margin = new Padding(0)
			};
			
			ButtonLayoutPanel = new TableLayoutPanel()
			{
				Dock = DockStyle.Bottom,
				ColumnCount = 3,
				RowCount = 1
			};
			ButtonLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
			ButtonLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			ButtonLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, ButtonPanel.Width));
			ButtonLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			
			Controls.Add(ButtonLayoutPanel);
			ButtonLayoutPanel.Controls.Add(ButtonPanel, 1, 0);
			
			NewRevisionButton = new Button()
			{
				Text = "New Version",
				Size = new Size(75, 75),
				Location = new Point(0, 0)
			};
			NewRevisionButton.Click += delegate(object sender, EventArgs e) 
			{  
				ExistingFileAction = ExistingFileAction.NewVersion;
				this.DialogResult = DialogResult.OK;
			};
			ButtonPanel.Controls.Add(NewRevisionButton);
			
			OverwriteButton = new Button()
			{
				Text = "Overwrite Latest",
				Size = new Size(75, 75),
				Location = new Point(75, 0)
			};
			OverwriteButton.Click += delegate(object sender, EventArgs e) 
			{
				ExistingFileAction = ExistingFileAction.OverwriteLatest;
				this.DialogResult = DialogResult.OK;
			};
			ButtonPanel.Controls.Add(OverwriteButton);
			
			RetryButton = new Button()
			{
				Text = "Retry",
				Size = new Size(75, 75),
				Location = new Point(150, 0)
			};
			RetryButton.Click += delegate(object sender, EventArgs e) 
			{
				ExistingFileAction = ExistingFileAction.RequestRetry;
				this.DialogResult = DialogResult.OK;
			};
			ButtonPanel.Controls.Add(RetryButton);
			
			this.CancelButton = new Button()
			{
				Text = "Cancel",
				Size = new Size(75, 75),
				Location = new Point(225, 0)
			};
			/*
			CancelButton.Click += delegate(object sender, EventArgs e)
			{
				this.DialogResult = DialogResult.Cancel;
			};
			*/
			ButtonPanel.Controls.Add((Button)CancelButton);
			
			//Controls.Add(new Panel() { Dock = DockStyle.Top, Height = 50 });
			
			ToolTips = new ToolTip();
			ToolTips.SetToolTip(OverwriteButton, "Overwrite the latest version of the file.\nThe existing file will be lost.");
			ToolTips.SetToolTip(NewRevisionButton, "Create a new version of the file.\nThe existing file will be retained.");
			ToolTips.SetToolTip(RetryButton, "You will be prompted to provide a new file name.");
			ToolTips.SetToolTip((Button)CancelButton, "Abort the save operation.\nYour data will not be written to a file.");
			
		}
		public ExistingFileActionDialog()
		{
			InitializeComponent();
		}
		
		public static ExistingFileAction TryAskUser(IWin32Window owner)
		{
			ExistingFileActionDialog dlg = new ExistingFileActionDialog();
			dlg.TryShowDialog(owner, DialogResult.OK);
			return dlg.ExistingFileAction;
		}
		
		public static ExistingFileAction TryAskUser()
		{ return TryAskUser(default(IWin32Window)); }
		
		public static ExistingFileAction TryAskUser(SynchronizationContext context)
		{
			return TryAskUser(default(IWin32Window), context);
		}
		
		public static ExistingFileAction TryAskUser(IWin32Window owner, SynchronizationContext context)
		{
			ExistingFileActionDialog dlg = new ExistingFileActionDialog();
			dlg.TryShowDialog(owner, context, DialogResult.OK);
			return dlg.ExistingFileAction;
		}
	}
}
