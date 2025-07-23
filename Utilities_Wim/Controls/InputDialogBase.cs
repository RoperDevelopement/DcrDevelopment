using System;
using System.Drawing;
using System.Windows.Forms;

namespace EdocsUSA.Controls
{
	public abstract class InputDialogBase : Form
	{
		private static Icon _DefaultIcon = null;
		public static Icon DefaultIcon
		{
			get 
			{ 
				if (_DefaultIcon == null) 
				{ 
					using (Form f = new Form())
					{ _DefaultIcon = f.Icon; }
				}
				return _DefaultIcon;
			}
			set { _DefaultIcon = value; }
		}
		
		protected Panel ContentPanel;
		protected TableLayoutPanel ControlPanel;
		protected Button OKButton;
		protected Button CancelDialogButton;
		protected Button ClearInputButton;
		protected Panel CaptionPanel;
		protected Label CaptionLabel;
		protected StatusBar StatusBar;
		
		public string Caption
		{
			get { return CaptionLabel.Text; }
			set { CaptionLabel.Text = value; }
		}
		
		private void InitializeComponent()
		{	
			base.Icon = DefaultIcon;
			
			base.MinimumSize = new Size(300, 150);
			
			AutoValidate = AutoValidate.EnableAllowFocusChange;
			
			ContentPanel = new Panel() { Dock = DockStyle.Fill };
			Controls.Add(ContentPanel);
			
			CaptionPanel = new Panel()
			{
				Dock = DockStyle.Top,
				AutoSize = true
			};
			Controls.Add(CaptionPanel);
			
			CaptionLabel = new Label()
			{ 
				AutoSize = true,
				MaximumSize = new System.Drawing.Size(this.Width, 0),
			};
			CaptionPanel.Controls.Add(CaptionLabel);
			
			Panel ControlButtonPanel = new Panel()
			{ 
				Width = 225,
				Height = 80
			};
			
			AcceptButton = new Button() 
			{ 
				Text = "&OK", 
				Size = new System.Drawing.Size(75, 75),
				Location = new Point(0,2)
			};
			AcceptButton.DialogResult = DialogResult.OK;
			ControlButtonPanel.Controls.Add((Button)AcceptButton);
			
			CancelButton = new Button() 
			{ 
				Text = "&Cancel",
				Size = new System.Drawing.Size(75, 75),
				Location = new Point(75,2)
			};
			CancelButton.DialogResult = DialogResult.Cancel;
			ControlButtonPanel.Controls.Add((Button)CancelButton);
			
			ClearInputButton = new Button() 
			{ 
				Text="&Clear",
				Size=new System.Drawing.Size(75,75),
				Location = new Point(150, 2),
				TabStop = false
			};
			ClearInputButton.Click += delegate(object sender, EventArgs e) 
			{ ClearInput(); };
			ControlButtonPanel.Controls.Add(ClearInputButton);
			
			ControlPanel = new TableLayoutPanel()
			{
				Dock = DockStyle.Bottom,
				Height = 80,
				RowCount = 1,
				ColumnCount = 4
			};
			ControlPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			ControlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			ControlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, ControlButtonPanel.Width));
			ControlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			Controls.Add(ControlPanel);
			
			ControlPanel.Controls.Add(ControlButtonPanel, 1, 0);
			
			
			StatusBar = new StatusBar() 
			{ 
				Dock = DockStyle.Bottom,
				Text = "..."
			};
			
			Controls.Add(StatusBar);
		}
		
		public InputDialogBase() : base()
		{
			InitializeComponent();
		}
		
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			CaptionLabel.Width = this.Width;
		}
		
		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			if ((this.DialogResult == DialogResult.OK) && (ValidateChildren() == false))
			{
				StatusBar.Text = "Content is invalid";
				e.Cancel = true;
			}
			else 
			{ 
				StatusBar.Text = string.Empty;
				base.OnClosing(e);
			}
		}
		
		public override bool ValidateChildren(ValidationConstraints validationConstraints)
		{
			bool ret = base.ValidateChildren(validationConstraints);
			StatusBar.Text = ret == true ? string.Empty : "Content is invalid";
			return ret;
		}
		
		public override bool ValidateChildren()
		{
			bool ret = base.ValidateChildren();
			StatusBar.Text = ret == true ? string.Empty : "Countent is invalid";
			return ret;
		}
		
		public virtual void ClearInput()
		{
			MessageBox.Show(this, "This dialog does not support clearing");
		}
	}
}
