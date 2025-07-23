using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
  using Edocs.Service.BinMonitor.SendEmails.InterFaces;
namespace Edocs.Service.BinMonitor.SendEmails.Models
{
   public class SendEmailModel:IEmail
    {
       public string EmailServer
        { get; set; }
        public string EmailFrom
        { get; set; }
        public string EmailTo
        { get; set; }
        public string EmailCC
        { get; set; }
        public int EmailPort
        { get; set; }

        public string EmailPw
        { get; set; }
        public string EmailSubject
        { get; set; }
        public string TextTO
        { get; set; }
        public string TextCC
        { get; set; }
    }
}
