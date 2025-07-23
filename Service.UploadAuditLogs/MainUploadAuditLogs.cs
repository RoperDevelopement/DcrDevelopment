using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.Service.UploadAuditLogs
{
    static class MainUploadAuditLogs
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new WindowsAuditLogService()
            };
            ServiceBase.Run(ServicesToRun);


            // TestWindowsService testWindowsService = new TestWindowsService();
            //  testWindowsService.GetAssemblyInfo();
            //ProcessAuditLogs processAudit = new ProcessAuditLogs();

            //processAudit.UpLoadAuditLogs(AzureBolbStorageTypes.AllBlobs).ConfigureAwait(false).GetAwaiter().GetResult();

            //processAudit.Dispose();
        }
    }
}
