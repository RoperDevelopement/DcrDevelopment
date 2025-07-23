using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edocs.Clients.NYP.L8SpecimenBatchUploader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                MessageBox.Show("Only one instance can run at a time");
            }
            else
            { 
                try
                { 
                    Application.EnableVisualStyles();
                    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                    Application.ThreadException += Application_ThreadException;
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmBatchUploader());
                }
                catch (Exception ex)
                { MessageBox.Show("Error launching " + ex.Message); }
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show("Application failed " + e.Exception.Message);
        }
    }
}
