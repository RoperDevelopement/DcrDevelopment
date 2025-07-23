using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinMonitorAppService.Logging
{
   public interface Ilog
    {
        void LogWarning(string message);
        void LogInformation(string message);
        void LogError(string message);

    }
}
