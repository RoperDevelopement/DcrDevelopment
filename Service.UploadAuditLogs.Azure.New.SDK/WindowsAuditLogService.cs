using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
 
namespace Edocs.Service.UploadAuditLogs
{
    public partial class WindowsAuditLogService : ServiceBase
    {
        private bool isStopped;
        const int DEFAULTSLEEP = 3600;
        private readonly int MillSeconds = 1000;


         
        public WindowsAuditLogService()
        {
            isStopped = false;
            InitializeComponent();
            this.AutoLog = true;
            this.CanShutdown = true;
            this.CanStop = true;

        }

        protected override void OnStart(string[] args)
        {

            isStopped = false;
           
            Thread monitorSystem = new Thread(new ThreadStart(this.UploadAuditLogsSP));
            monitorSystem.Start();
        }

        protected override void OnStop()
        {
            EmailSend("Service Edocs.Service.UploadAuditLogs.exe is stopping");
            isStopped = true;
        }
        protected override void OnPause()
        {
            isStopped = true;
            EmailSend("Service Edocs.Service.UploadAuditLogs.exe is paused");
            base.OnPause();
        }

        /// <summary>
        /// is called when the service is continued
        /// </summary>
        protected override void OnContinue()
        {
            EmailSend("Service Edocs.Service.UploadAuditLogs.exe is continue");
            base.OnContinue();
            isStopped = false;
            Thread monitorSystem = new Thread(new ThreadStart(this.UploadAuditLogsSP));
            monitorSystem.Start();
        }
        /// <summary>
        /// Called when the service is stopped
        /// </summary>
        protected override void OnShutdown()
        {
            isStopped = true;
            EmailSend("Service Edocs.Service.UploadAuditLogs.exe is shutting down");
            base.OnShutdown();
        }

        private int GetServiceSleep()
        {
           int retSystemServiceSleep = DEFAULTSLEEP;
            try
            {
                if (Properties.Settings.Default.ThreadSleepSeconds >= 0)
                    retSystemServiceSleep = Properties.Settings.Default.ThreadSleepSeconds;
            }
            catch { }
            return (retSystemServiceSleep * MillSeconds);
        }
       private void EmailSend(string message)
        {
            try
            {
                SEmail.SendEMailsInstance.EmailSend($"{message} on machine {Environment.MachineName}", false);
            }
            catch { }
        }

        private void UploadAuditLogsSP()
        {

            EmailSend("Service Edocs.Service.UploadAuditLogs.exe is starting");


            while (!(isStopped))
            {
                try
                {
                    ProcessAuditLogs processAuditLogs = new ProcessAuditLogs();
                    processAuditLogs.UpLoadAuditLogs(AzureBolbStorageTypes.AllBlobs).ConfigureAwait(false).GetAwaiter().GetResult();
                    processAuditLogs.Dispose();
                    GC.Collect();
                   int systemServiceSleep = GetServiceSleep();
                    Thread.Sleep(systemServiceSleep);

                }
                catch
                { }
            }

        }
    }
}
