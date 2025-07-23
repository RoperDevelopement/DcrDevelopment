using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.Check.HL7.Running
{
    static class MainHL7Check
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new CheckHL7()
            };
            ServiceBase.Run(ServicesToRun);
            // CHKH7RevecingFiles cHKH7RevecingFiles = new CHKH7RevecingFiles();
            //  cHKH7RevecingFiles.HL7RecevingFiles();
        }
    }
}
