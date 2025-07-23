using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinMonitor.Common.Sharepoint
{
    public static class SPHelper
    {
        public const string DateTimeStringFormat = "yyyy-MM-ddThh:mm:ssZ";

        public const string DateStringFormat = "yyyy-MM-dd";

        public static string UnspaceValue(string value)
        { return value.Replace(" ", "_x0020_"); }
    }
}
