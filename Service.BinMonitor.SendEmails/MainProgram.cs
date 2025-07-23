using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.Service.BinMonitor.SendEmails
{
    static class MainProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new BmServiceSendEmails()
            };
            ServiceBase.Run(ServicesToRun);
            //  SendEmailsBinMonitor sendEmailsBinMonitor = new SendEmailsBinMonitor();
            //  sendEmailsBinMonitor.CheckEmails().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
