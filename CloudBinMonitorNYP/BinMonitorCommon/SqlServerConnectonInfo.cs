using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{
  public  class SqlServerConnectonInfo
    {
        public string CloudServerName
        { get; set; }
        public string CloudDbName
        { get; set; }
        public string CloudDbUserName
        { get; set; }
        public string CloudUserPw
        { get; set; }
    }
}
