/*
 * User: Sam Brinly
 * Date: 2/21/2013
 */
using System;
using System.Windows.Forms;

using EdocsUSA.Utilities.Extensions;

namespace EdocsUSA.Controls
{
	public class ListBoxInputDialog : InputDialogBase
	{
		public ListBox ListBox;
		
		bool _RequiresValue = true;
		public virtual bool RequiresValue
		{
			get { return _RequiresValue; }
			set { _RequiresValue = value; }
		}
		
		private void InitializeComponent()
		{
			ListBox = new ListBox()
			{ Dock = DockStyle.Fill };
			ContentPanel.Controls.Add(ListBox);
			
			ListBox.DoubleClick += delegate(object sender, EventArgs e) 
			{ this.DialogResult = DialogResult.OK; };
		}
		
		public ListBoxInputDialog() : base()
		{ InitializeComponent(); }
		
		protected override void OnValidating(System.ComponentModel.CancelEventArgs e)
		{
			if (RequiresValue && (ListBox.HasSelection() == false)) e.Cancel = true;
			base.OnValidating(e);
		}
		
		public void Clear() 
		{ ListBox.ClearSelected(); }
	}
}
