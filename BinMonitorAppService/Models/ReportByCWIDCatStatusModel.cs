using BinMonitor.BinInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinMonitorAppService.Models
{
    public class ReportByCWIDCatStatusModel 
    {
        
        public string BinID
        { get; set; }
        
        public string RegAssignedTo
        { get; set; }
       
          
        public string RegDuration
        { get; set; }
        
        public string ProcessAssignedTo
        { get; set; }
       
     
        
        public string ProcessDuration
        { get; set; }

        public string BinClosedBy
        { get; set; }
        
        public string CompleteDuration
        { get; set; }

         
        public string CategoryName
        { get; set; }
        public string BinStatusReg
        { get; set; }
        public string BinStatusProcessing
        { get; set; }

        public string BinStatusClosed
        { get; set; }




        public int TotalVol
        { get; set; }
    }
}
