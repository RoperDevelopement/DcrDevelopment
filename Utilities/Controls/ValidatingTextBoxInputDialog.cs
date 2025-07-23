using System;
using System.Windows.Forms;

namespace EdocsUSA.Controls
{
	public class ValidatingTextBoxInputDialog<T> : InputDialogBase
	{
		public ValidatingTextBox<T> ValidatingTextBox;
		
		private void InitializeComponent()
		{
			ValidatingTextBox = new ValidatingTextBox<T>()
			{ Dock = DockStyle.Fill };
			ContentPanel.Controls.Add(ValidatingTextBox);
		}
		
		public ValidatingTextBoxInputDialog() : base()
		{
			InitializeComponent();
		}
		
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			ValidatingTextBox.SelectAll();
			ValidatingTextBox.Focus();
		}

	}
}
