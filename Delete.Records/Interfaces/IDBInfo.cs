using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.Delete.Records.Interfaces
{
    interface IDBInfo
    {
        string DataBaseName
        { get; set; }
        string TableName
        { get; set; }
        int NumberYrsKeep
        { get; set; }
        int NumberDaysKeep
        { get; set; }
        int NumberMonthsKeep
        { get; set; }
        DateTime DateToDelete
        { get; set; }
        string DBUserName
        { get; set; }
        string DBPassWord
        { get; set; }
        string SqlServer
        { get; set; }
        string DeleteStoredProcedure
        { get; set; }
        string GetLabRecsStoredProcedure
        { get; set; }
        int SqlServerTimeOut
        { get; set; }
    }
    interface ILabReqs
    {
        int ID
        { get; set; }
        string FileUrl
        { get; set; }

        Guid ScanBatch
        { get; set; }
        DateTime ScanDate
        { get; set; }
    }
}
