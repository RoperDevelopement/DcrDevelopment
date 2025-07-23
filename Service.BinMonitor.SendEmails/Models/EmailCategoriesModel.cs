using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.Service.BinMonitor.SendEmails.InterFaces;
namespace Edocs.Service.BinMonitor.SendEmails.Models
{
   public class EmailCategoriesModel:IEmailCategories
    {
     public   int CategoryId
        { get; set; }
        public string EmailDur
        { get; set; }
        public string EmailsTo
        { get; set; }
        public DateTime LastTimeEmailSent
        { get; set; }
    }
}
