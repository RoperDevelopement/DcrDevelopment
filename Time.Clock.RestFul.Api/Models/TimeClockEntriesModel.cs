using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edocs.Employees.Time.Clock.App.Service.InterFaces;
namespace Edocs.Employees.Time.Clock.App.Service.Models
{
    public class TimeClockEntriesModel: IEmpClockInOut, IEmpID, IRecordID,ITimeClockDurHours
    {
        public int ID { get; set; }
        public  string EmpID
        { get; set; }
     
     public   DateTime ClockInTime { get; set; }
      public  DateTime ClockOutTime { get; set; }
        public string TimeSpanDur { get; set; }
    }
}
