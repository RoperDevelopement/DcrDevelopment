using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Edocs.DOH.DL.Doc.Utilities;
namespace Edocs.DOH.DL.Doc
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Form MForm = null;
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                MForm = new DOHMainForm();
                Application.ThreadException += delegate (object sender, ThreadExceptionEventArgs e)
                {
                    if (e.Exception is OperationCanceledException)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        MessageBox.Show(e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                AppDomain.CurrentDomain.UnhandledException += delegate (object sender, UnhandledExceptionEventArgs e)
                {
                    Exception ex = (Exception)e.ExceptionObject;
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                Send_Emails.EmailInstance.SendEmail(ex.Message);

            }
                Application.Run(new DOHMainForm());

        }
    }
}
 
