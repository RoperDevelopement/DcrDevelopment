using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;
using Edocs.Service.BinMonitor.SendEmails.Models;
using Edocs.Service.BinMonitor.SendEmails;
using Edocs.Service.BinMonitor.SendEmails.EmailSqlCommands;
namespace Edocs.Service.BinMonitor.SendEmails.SendEmail
{
    public class SendEmailBM
    {
        private static SendEmailBM instance = null;
        public static SendEmailBM SendEmailInstance
        {
            get
            {
                if (instance == null)
                    instance = new SendEmailBM();
                return instance;
            }
        }
        private SendEmailBM() { }

        public async Task SendEmailMessage(string xmlSettingsFile, string textMessage, string emailBody, string emailTO, string emailCC, bool htmlBody, string emailSubject)
        {
            SendEmailModel emailModel = ServiceHelpers.Instance.GetEmailInfo(xmlSettingsFile).ConfigureAwait(false).GetAwaiter().GetResult();
            MailMessage emailMessage = new MailMessage();
            emailMessage.Sender = new MailAddress(emailModel.EmailFrom, "e-Docs Usa");
            emailMessage.From = new MailAddress(emailModel.EmailFrom, "e-Docs Usa");
            if (string.IsNullOrWhiteSpace(emailTO))
                emailTO = emailModel.EmailTo;
            if (string.Compare(emailCC, "noemailcc", true) == 0)
                emailCC = string.Empty;
            else
            {
                if (string.IsNullOrWhiteSpace(emailCC))
                {
                    emailCC = emailModel.EmailCC;
                }
            }

            foreach (string mTo in emailTO.Split(';'))
            {
                if (!(string.IsNullOrEmpty(mTo)))
                    emailMessage.To.Add(mTo);
            }
            foreach (string mCC in emailCC.Split(';'))
            {
                if (!(string.IsNullOrEmpty(mCC)))
                    emailMessage.CC.Add(mCC);
            }



            if (ServiceHelpers.Instance.CheckFileExists(emailBody))
            {
                emailMessage.IsBodyHtml = true;
                emailBody = ServiceHelpers.Instance.ReadData(emailBody).ConfigureAwait(false).GetAwaiter().GetResult();
                emailBody = ServiceHelpers.Instance.StrReplace(emailBody, BinMonitorConst.ReplaceStrDateTime, DateTime.Now.ToString());
            }

            emailMessage.IsBodyHtml = htmlBody;
            emailMessage.Body = emailBody;
            emailSubject = $"{ServiceHelpers.Instance.StrReplace(emailModel.EmailSubject, BinMonitorConst.RepStrEmailSubject, emailSubject)}";
            emailMessage.Subject = $"{emailSubject} email sent at {DateTime.Now.ToString()}";
            EmailSend(emailMessage, emailModel).ConfigureAwait(false).GetAwaiter().GetResult();
            if (!(string.IsNullOrWhiteSpace(textMessage)))
                SendTextMessage(emailModel, textMessage).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private async Task SendTextMessage(SendEmailModel emailModel, string textMessage)
        {

            MailMessage emailMessage = new MailMessage();
            emailMessage.Sender = new MailAddress(emailModel.EmailFrom, "e-Docs Usa");
            emailMessage.From = new MailAddress(emailModel.EmailFrom, "e-Docs Usa");


            foreach (string tTo in emailModel.TextTO.Split(';'))
            {
                if (!(string.IsNullOrEmpty(tTo)))
                    emailMessage.To.Add(tTo);
            }
            foreach (string tCC in emailModel.TextCC.Split(';'))
            {
                if (!(string.IsNullOrEmpty(tCC)))
                    emailMessage.CC.Add(tCC);
            }
            emailMessage.IsBodyHtml = false;
            emailMessage.Body = textMessage;
            EmailSend(emailMessage, emailModel).ConfigureAwait(false).GetAwaiter().GetResult();


        }

        private async Task EmailSend(MailMessage mail, SendEmailModel emailModel)
        {
            SmtpClient mailClient = new SmtpClient(emailModel.EmailServer);
            mailClient.Port = emailModel.EmailPort;
            mailClient.UseDefaultCredentials = false;
            mailClient.EnableSsl = true;
            NetworkCredential nc = new NetworkCredential(emailModel.EmailFrom, emailModel.EmailPw);
            mailClient.Credentials = nc;
            mailClient.Send(mail);
        }
    }
}
