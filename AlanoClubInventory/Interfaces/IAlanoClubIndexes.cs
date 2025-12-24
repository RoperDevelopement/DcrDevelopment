using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Interfaces
{
    public interface IAlanoClubIndexes
    {
         string Schema { get; set; }
         string  Table { get; set; }
        string Index { get; set; }
        
    int AvgFragmentationInPercent { get; set; }
        int IndexPageCount { get; set; }
    }
    public interface IErrorLogEntry
    {
        DateTime LogDate { get; set; }
        string ProcessInfo { get; set; }
        string Text { get; set; }

    }
    public interface IStoredProcedures
    {
        string StoredProcedureName { get; set; }
        string StoredProcedureDefinition {  get; set; }
    }
}
