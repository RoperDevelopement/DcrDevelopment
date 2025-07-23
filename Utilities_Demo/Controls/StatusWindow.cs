/*
 * User: Sam
 * Date: 10/24/2011
 * Time: 10:43 AM
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EdocsUSA.Utilities
{
	/// <summary>
	/// Description of StatusWindow.
	/// </summary>
	public partial class StatusWindow : Form
	{
		public string Status
		{
			get { return StatusLabel.Text; }
			set 
			{ 
				StatusLabel.Text = value;
				StatusLabel.Refresh();
			}
		}
		
		public StatusWindow()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
	}
}
