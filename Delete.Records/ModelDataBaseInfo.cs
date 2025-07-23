using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.Delete.Records.Interfaces;
namespace Edocs.Delete.Records
{
   public class ModelDataBaseInfo: IDBInfo
    {
       public string DataBaseName
        { get; set; }
        public string TableName
        { get; set; }
        public int NumberYrsKeep
        { get; set; }
        public int NumberDaysKeep
        { get; set; }
        public int NumberMonthsKeep
        { get; set; }
        public DateTime DateToDelete
        { get; set; }
       public string DBUserName
        { get; set; }
       public string DBPassWord
        { get; set; }

       public string SqlServer
        { get; set; }
        public string DeleteStoredProcedure
        { get; set; }
        public string GetLabRecsStoredProcedure
        { get; set; }

        public int SqlServerTimeOut
        { get; set; }
    }
}
