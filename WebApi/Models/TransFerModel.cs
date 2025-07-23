using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinMonitor.BinInterfaces;
namespace BinMonitorAppService.Models
{
    public enum TransferType
    {
        TB,
        TC,
        TP,
        TL
    }
    public class TransFerModel : ITransFer
    {
        public Guid BatchId
        { get; set; }
        public string BinID
        { get; set; }
        public string CategoryName
        { get; set; }
        public string LabReqNumber
        { get; set; }

        public string Comments
        { get; set; }


        public string OldBinId
        { get; set; }
        public string Processing
        { get; set; }
        public string TransFerType
        { get; set; }
     public   string TransferBy
        { get; set; }
    }
}
