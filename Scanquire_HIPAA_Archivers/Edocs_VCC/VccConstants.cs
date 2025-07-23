using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Edocs.Dillion.VCC.Archiver
{
 public   class VccConstants
    {
        public const string InvoiceDateFormatMMDDYY = @"\b(0[1-9]|1[0-2])/(0[1-9]|[12][0-9]|3[01])/\d{4}\b";
        public const string InvoiceDateFormatMDDYY = @"\b[1-9]|1[0-2]\s[0-3]?[0-9]\s\d{4}\b";
        public const string InvoiceNameDesc = @"name|description";
        public const string InvoiceNumber = @"\d{4,}";

     public   static async Task<MatchCollection > GetRegxMatch(string inputStr,string regxPattern)
        {
            Regex regex = new Regex(regxPattern, RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(inputStr);
            return matches;
        }

    }
}
