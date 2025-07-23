/*
 * User: Sam
 * Date: 10/10/2011
 * Time: 10:26 AM
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using EdocsUSA.Controls;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Controls;
using EdocsUSA.Utilities.Extensions;

namespace Testing
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			SettingsManager.PortableMode = true;
			Trace.TraceInformation(SettingsManager.TempDirectoryPath);
			
			return;
			Rectangle orig = new Rectangle(0, 0, 2496,3507);
			Debug.WriteLine(orig.GetScaledInstance(1440, 920).ToString());
			orig = new Rectangle(0, 0, 3507, 2496);
			Debug.WriteLine(orig.GetScaledInstance(920, 1440).ToString());
			return;
			InputDlg d = new Program.InputDlg();
			List<string> items = new List<string>(){"sam", "tressa", "amber"};
			d.ListBox.DataSource = items;
			d.ShowDialog();
			//Trace.TraceInformation((string)d.SelectedValue);
			return;
			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
		class InputDlg : ListBoxInputDialog
		{
			public override bool RequiresValue
			{
				get { return false; }
			} 
		}
	}
}
