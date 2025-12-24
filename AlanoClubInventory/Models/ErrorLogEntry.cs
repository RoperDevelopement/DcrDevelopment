using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlanoClubInventory.Interfaces;
namespace AlanoClubInventory.Models
{
    public class ErrorLogEntry : IErrorLogEntry
    {
        
        
        public    DateTime LogDate { get; set; }
          public  string ProcessInfo { get; set; }
           public string Text { get; set; }
        
    }
}
