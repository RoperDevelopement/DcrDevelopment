using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScanQuireSqlCmds
{
    public class ScanQuireUsers
    {
        public string UserFName
            {get;set;}
        public string UserLName
        { get; set; }
        public string UserEmailAddress
        { get; set; }
        public string IsAdmin
        { get; set; }


        public string LoginId
        { get; set; }

        public string Pasword
        { get; set; }

        public string LastLogin
        { get; set; }

        public string PWLastChanged
        { get; set; }

    }
}
