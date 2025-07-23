using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Edocs.Check.HL7.Running.ConstProperties;
using SE = Edocs.Check.HL7.Running.SEmail;
namespace Edocs.Check.HL7.Running
{
    public partial class CheckHL7 : ServiceBase
    {
        private bool isStopped;
        const int DEFAULTSLEEP = 3600;
        private readonly int MillSeconds = 1000;
        Thread monitorSystem = null;
        public CheckHL7()
        {
      isStopped = false;
            InitializeComponent();
            //  this.AutoLog = true;
            this.AutoLog = true;
            this.CanShutdown = true;
            this.CanStop = true;
        }

        protected override void OnStart(string[] args)
        {
            EmailSend("Check HL7 Service Running starting",false).ConfigureAwait(false).GetAwaiter().GetResult();
            monitorSystem = new Thread(new ThreadStart(this.CheckHL7RecevingFiles));
            monitorSystem.Start();
        }

        protected override void OnStop()
        {
            EmailSend("Check HL7 Service is stopping",true).ConfigureAwait(false).GetAwaiter().GetResult();
            //  EmailSend("Service Edocs.Service.UploadAuditLogs.exe is stopping");
            isStopped = true;
        }
        protected override void OnPause()
        {
            isStopped = true;
            EmailSend("Check HL7 Service is paused",true).ConfigureAwait(false).GetAwaiter().GetResult(); ;
            base.OnPause();
        }

        /// <summary>
        /// is called when the service is continued
        /// </summary>
        protected override void OnContinue()
        {
            EmailSend("Check HL7 Service  is continue",false).ConfigureAwait(false).GetAwaiter().GetResult(); ;
            base.OnContinue();
             isStopped = false;
            Thread monitorSystem = new Thread(new ThreadStart(this.CheckHL7RecevingFiles));
            monitorSystem.Start();
        }
        /// <summary>
        /// Called when the service is stopped
        /// </summary>
        protected override void OnShutdown()
        {
           isStopped = true;
               EmailSend("Check HL7 Service is shutting down",true).ConfigureAwait(false).GetAwaiter().GetResult(); 
            base.OnShutdown();
        }
        private void CheckHL7RecevingFiles()
        {
            while (!(isStopped))
            {
                try
                {
                    CHKH7RevecingFiles cHKH7RevecingFiles = new CHKH7RevecingFiles();
                    cHKH7RevecingFiles.HL7RecevingFiles();
                    
                    cHKH7RevecingFiles.Dispose();
                    Thread.Sleep(PropertiesConst.HL7Instance.HL7ThreadSleepSecs);
                }
                catch(Exception ex)
                {
                    EmailSend($"Check HL7 Service method  CheckHL7RecevingFiles() threw exception so shutting down {ex.Message}", true).ConfigureAwait(false).GetAwaiter().GetResult();
                    isStopped = true;
                    
                }
            }
        }

        private async Task EmailSend(string message,bool error)
        {
            try
            {
                SE.SEmail.SendEMailsInstance.EmailSend($"{message} on machine {Environment.MachineName}", error).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch { }
        }


    }
}
