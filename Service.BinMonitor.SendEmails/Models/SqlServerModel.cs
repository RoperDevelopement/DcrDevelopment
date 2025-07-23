using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.Service.BinMonitor.SendEmails.InterFaces;
namespace Edocs.Service.BinMonitor.SendEmails.Models
{
   public class SqlServerModel:InterfaceSqlInfo
    {
      public  string SqlDBName
        { get; set; }
        public string SqlServerName
        { get; set; }
        public string SqlDBUserName
        { get; set; }
        public string SqlDBPassWord
        { get; set; }
    }
}
