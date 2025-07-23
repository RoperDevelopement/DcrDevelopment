using BinMonitor.Common;
using BinMonitor.Common.Sharepoint;
/*
 * User: Sam Brinly
 * Date: 11/19/2014
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace BinMonitor
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{

        //Unigue named mutex to prevent multiple application instances from running.
        static Mutex singleAppInstanceMutex = new Mutex(true, "6a0f9453-e8c7-43c8-8166-a3b8c4fb4d94");

        [STAThread]
		private static void Main(string[] args)
		{
            //Ensure only one instance of the application is running.
            if (singleAppInstanceMutex.WaitOne(TimeSpan.Zero, true) == false)
            {
                string message = string.Format("Another instance of {0} is already running.", Process.GetCurrentProcess().ProcessName);
                MessageBox.Show(message);
                return;
            }

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new BinMonitorForm());
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                MessageBox.Show(ex.Message); 
            }
            finally
            {
                singleAppInstanceMutex.ReleaseMutex();
            }            
		}
		
	}
}
