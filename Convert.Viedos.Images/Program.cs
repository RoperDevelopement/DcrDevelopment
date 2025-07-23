using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Edocs.Convert.Viedos.Images.PropConst;
namespace Edocs.Convert.Viedos.Images
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Form applicationForm = null;
            try
            {


                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                applicationForm = new MainForm();
                PropConst.ConstProp.Args = args;
                
                //Add exception handling.
                Application.ThreadException += delegate (object sender, ThreadExceptionEventArgs e)
                {
                    

                    if (e.Exception is OperationCanceledException)
                    {
                        
                        MessageBox.Show("Operation Canceled Exception", "Operation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Environment.Exit(-1);
                    }
                    else
                    {
                        string errorMessage = e.Exception.Message;
                       
                        MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(-1);
                    }
                };

                AppDomain.CurrentDomain.UnhandledException += delegate (object sender, UnhandledExceptionEventArgs e)
                {

                   
                    Exception ex = (Exception)e.ExceptionObject;
                   
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(-1);
                };


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error launching application " + ex.Message);
                
                #if DEBUG
                MessageBox.Show(ex.StackTrace);
#endif

               // Environment.Exit(-1);
            }
            Application.Run(applicationForm);
            // Application.Run(new MainForm());
        }
        
        }
    }
 
