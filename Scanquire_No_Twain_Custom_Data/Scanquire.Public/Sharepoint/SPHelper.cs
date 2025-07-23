using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scanquire.Public.Sharepoint
{
    static class SPHelper
    {
        public const string DateTimeStringFormat = "yyyy-MM-ddThhmmssZ";

        public const string DateStringFormat = "yyyy-MM-dd";

        public static string UnspaceValue(string value)
		{ return value.Replace(" ", "_x0020_"); }
    }
}
