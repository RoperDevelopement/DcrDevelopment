using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;
namespace Edocs.HelperUtilities
{
   public class EdocsSendEmails
    {
        public string EmailServer
        {
            get; set;
        }
        public string EmailFrom
        { get; set; }
        public string EmailTo
        { get; set; }
        public string EmailCC
        { get; set; }

        public string EmailSubject
        { get; set; }
        public string EmailBody
        { get; set; }

        public string EmailAttachment
        { get; set; }
        public string EmailPassord
        {
            get; set;
        }
        public int EmailPort
        {
            get;set;
        }

        public void SendEmail(bool emailHtml)
        {
            try
            { 
            MailMessage emailMessage = new MailMessage();
            emailMessage.Sender = new MailAddress(EmailFrom);
            emailMessage.From = new MailAddress(EmailFrom);
            string[] strEmailTo = EmailTo.Split(';');
            foreach(string s in strEmailTo)
            emailMessage.To.Add(s);
            if(!(string.IsNullOrWhiteSpace(EmailCC)))
            {
                string[] strEmailCC = EmailCC.Split(';');
                foreach (string scc in strEmailCC)
                    emailMessage.CC.Add(scc);
            }
            if(emailHtml)
            {
                emailMessage.IsBodyHtml = true;
                if (Path.HasExtension(EmailBody))
                    EmailBody = HelperUtilities.Utilities.ReadFile(EmailBody);
            }
            
            if(string.IsNullOrEmpty(EmailAttachment))
            {
                if (HelperUtilities.Utilities.CheckFileExists(EmailAttachment))
                    emailMessage.Attachments.Add(new Attachment(EmailAttachment));
            }
            emailMessage.Body = EmailBody;
            emailMessage.Body = EmailBody;
            SmtpClient mailClient = new SmtpClient(EmailServer);
            mailClient.Port = EmailPort;
            mailClient.UseDefaultCredentials = false;
            mailClient.EnableSsl = true;
            NetworkCredential nc = new NetworkCredential(EmailFrom, EmailPassord);
            mailClient.Credentials = nc;
            mailClient.Send(emailMessage);
            }
            catch(Exception ex)
            {
                throw new Exception($"Sending email:{ex.Message}");
            }

        }
    }
}
