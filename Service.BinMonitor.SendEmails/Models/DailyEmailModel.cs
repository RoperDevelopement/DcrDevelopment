using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.Service.BinMonitor.SendEmails.InterFaces;
namespace Edocs.Service.BinMonitor.SendEmails.Models
{
  public  class DailyEmailModel:IEmailDaily
    {
       public string EmailTo
        { get; set; }
       public string EmailCC
        { get; set; }
       public string EmailFrequency
        { get; set; }
       public DateTime LastTimeEmailSent
        { get; set; }
    }
}
