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
namespace Edocs.Service.BinMonitor.SendEmails
{
    public partial class BmServiceSendEmails : ServiceBase
    {
        private bool isStopped;
        const int DEFAULTSLEEP = 3600;
        private readonly int MillSeconds = 1000;
        public BmServiceSendEmails()
        {
            isStopped = false;
            InitializeComponent();
            this.AutoLog = true;
            this.CanShutdown = true;
            this.CanStop = true;
             

        }
        protected override void OnPause()
        {
            isStopped = true;
            base.OnPause();
        }


        protected override void OnStart(string[] args)
        {
            isStopped = false;
            Thread bmSEmails = new Thread(new ThreadStart(this.SendBMEmails));
            bmSEmails.Start();
        }
        protected override void OnContinue()
        {
            
            base.OnContinue();
            isStopped = false;
            Thread monitorSystem = new Thread(new ThreadStart(this.SendBMEmails));
            monitorSystem.Start();
        }
        protected override void OnStop()
        {
            isStopped = true;
        }
        protected override void OnShutdown()
        {
            isStopped = true;
            base.OnShutdown();
        }

        private int BMThreadSleep
        { get; set; }
        private void SendBMEmails()
        {
            while(!isStopped)
            {
                try
                {
                    SendEmailsBinMonitor sendEmailsBinMonitor = new SendEmailsBinMonitor();
                    sendEmailsBinMonitor.CheckEmails().ConfigureAwait(false).GetAwaiter().GetResult();
                    BMThreadSleep = sendEmailsBinMonitor.ThreadSleep;
                    sendEmailsBinMonitor.Dispose();
                    Thread.Sleep(BMThreadSleep);

                }
                catch { }
            }
        }
    }
}
