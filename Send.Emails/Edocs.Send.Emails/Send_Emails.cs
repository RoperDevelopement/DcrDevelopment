using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using Edocs.Send.Emails.Properties;
using Edocs.Encrypt.Decrypt;
namespace Edocs.Send.Emails
{
    public class Send_Emails
    {
        public string EmailFrom
        { get { return Settings.Default.EmailFrom; } }
        public string EmailTo
        { get { return Settings.Default.EmailTo; } }

        public string EmailPasswordKey
        { get { return Settings.Default.EmailPasswordKey; } }

        public string EmailPassword
        { get { return Settings.Default.EmailPassword; } }

        public string EmailCC
        { get { return Settings.Default.EmailCC; } }

        public string EmailServer
        { get { return Settings.Default.EmailServer; } }

        public string EmailSubject
        { get { return Settings.Default.EmailSubject; } }
        public string EmailBody
        { get; set; }
        private int EmailPort
        { get { return Settings.Default.EmailPort; } }

        private string TextTo
        { get { return Settings.Default.TextTo; } }

        public string TextCc
        { get { return Settings.Default.TextCc; } }
        private string TxtSubject
        { get { return Settings.Default.TxtSubject; } }

        public static Send_Emails EmailInstance = null;
        protected Send_Emails()
        { }
        static Send_Emails()
        {

            if (EmailInstance == null)
            {
                EmailInstance = new Send_Emails();
            }

        }
        public void SendTxtMessage(string message, bool err)
        {
            return;
            MailMessage textMessage = NewMailMessage(TextTo);
            if (err)
                textMessage.Subject = $"Errors {DateTime.Now.ToString()}";
            else
                textMessage.Subject = $"Success {DateTime.Now.ToString()}";
            
            textMessage.Body = message;
            textMessage.IsBodyHtml = false;
            foreach (string eText in TextTo.Split(';'))
            {
                if (!(string.IsNullOrWhiteSpace(eText)))
                    textMessage.CC.Add(eText);
            }
            foreach (string eCC in TextCc.Split(';'))
            {
                if(!(string.IsNullOrWhiteSpace(eCC)))
                textMessage.CC.Add(eCC);
            }
            
            SendMessage(textMessage);
        }
        public void UpDateEmailPw(string newPw)
        {
             
            byte[] pwBytes = AESSaltEncDec.GetPasswordBytes(EmailPasswordKey);
            Settings.Default.EmailPassword = AESSaltEncDec.Encrypt(newPw, pwBytes);

            
             Settings.Default.Save();


        }
        public void SendEmail(string emailTo, string emailMessage, string emailSubject,string emailAttachment,bool emailBodyHtml,string textMessage)
        {
            string emailCC = string.Empty;
            if (string.IsNullOrWhiteSpace(emailTo))
                emailTo = EmailTo;
            else
            {
                emailCC = EmailTo;
            }

           
            MailMessage messageEmail = NewMailMessage(emailTo);
            messageEmail.IsBodyHtml = false;
            if (emailBodyHtml)
            {
                messageEmail.IsBodyHtml = true;
                try
                {
                    if (Path.HasExtension(emailMessage))
                    {
                        emailMessage = File.ReadAllText(emailMessage);
                    }
                }
                catch { }
            }
            if (!(string.IsNullOrEmpty(emailAttachment)))
            {
                if (File.Exists(emailAttachment))
                    messageEmail.Attachments.Add(new Attachment(emailAttachment));
                else
                    throw new Exception($"Email attachement {emailAttachment} not found");
            }
            string[] toEmail = emailTo.Split(';');

            for (int i= 0;i < toEmail.Length;i++)
            {
                //if(!(messageEmail.To.Contains(new MailMessage((eTo))))
                        messageEmail.To.Add(toEmail[i]);
            }

            if (!(string.IsNullOrWhiteSpace(EmailCC)))
            {
                emailCC += EmailCC;
            }

          
            foreach (string eCC in emailCC.Split(';'))
            {
                if(!(string.IsNullOrWhiteSpace(eCC)))
                messageEmail.CC.Add(eCC);
            }
            if (string.IsNullOrWhiteSpace(emailSubject))
            {
                emailSubject = EmailSubject;
            }
            emailSubject = $"{emailSubject} email sent on {DateTime.Now.ToString()}";
            messageEmail.Body = emailMessage;
            messageEmail.Subject = emailSubject;
            SendMessage(messageEmail);
            if(!(string.IsNullOrWhiteSpace(textMessage)))
            SendTxtMessage(textMessage,true);
        }

        public void SendEmail(string emailTo,string emailCC, string emailMessage, string emailSubject, string emailAttachment, bool emailBodyHtml, string textMessage)
        {
            
            if (string.IsNullOrWhiteSpace(emailTo))
                emailTo = EmailTo;
            


            MailMessage messageEmail = NewMailMessage(emailTo);
            messageEmail.IsBodyHtml = false;
            if (emailBodyHtml)
            {
                messageEmail.IsBodyHtml = true;
                try
                {
                    if (Path.HasExtension(emailMessage))
                    {
                        emailMessage = File.ReadAllText(emailMessage);
                    }
                }
                catch { }
            }
            if (!(string.IsNullOrEmpty(emailAttachment)))
            {
                if (File.Exists(emailAttachment))
                    messageEmail.Attachments.Add(new Attachment(emailAttachment));
                else
                    throw new Exception($"Email attachement {emailAttachment} not found");
            }
            string[] toEmail = emailTo.Split(';');

            for (int i = 0; i < toEmail.Length; i++)
            {
                //if(!(messageEmail.To.Contains(new MailMessage((eTo))))
                messageEmail.To.Add(toEmail[i]);
            }

            if (!(string.IsNullOrWhiteSpace(EmailCC)))
            {
                emailCC += EmailCC;
            }


            foreach (string eCC in emailCC.Split(';'))
            {
                if (!(string.IsNullOrWhiteSpace(eCC)))
                    messageEmail.CC.Add(eCC);
            }
            if (string.IsNullOrWhiteSpace(emailSubject))
            {
                emailSubject = EmailSubject;
            }
            emailSubject = $"{emailSubject} email sent on {DateTime.Now.ToString()}";
            messageEmail.Body = emailMessage;
            messageEmail.Subject = emailSubject;
            SendMessage(messageEmail);
            if (!(string.IsNullOrWhiteSpace(textMessage)))
                SendTxtMessage(textMessage, true);
        }

        private void SendMessage(MailMessage messageEmail)
        {
            try
            { 
            messageEmail.Sender = new MailAddress(EmailFrom);
            messageEmail.From = new MailAddress(EmailFrom);
            SmtpClient mailClient = new SmtpClient(EmailServer);
            mailClient.Port = EmailPort;
            mailClient.UseDefaultCredentials = false;
            mailClient.EnableSsl = true;
            string emailPassord = GetEmailPassword();
            NetworkCredential nc = new NetworkCredential(EmailFrom, emailPassord);
            mailClient.Credentials = nc;
            mailClient.Send(messageEmail);
            }
            catch(Exception ex)
            {
               // throw new Exception(ex.Message);
            }
        }
        public MailMessage NewMailMessage(string emailTo)
        {
            try
            {
                MailMessage emailMessage = new MailMessage();
                foreach (string emailAdd in emailTo.Split(';'))
                    emailMessage.To.Add(emailAdd);

                return emailMessage;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private string GetEmailPassword()
        {
            byte[] pwBytes = AESSaltEncDec.GetPasswordBytes(EmailPasswordKey+
                "fixme");
            string retStr =  AESSaltEncDec.Decrypt(EmailPassword, pwBytes);
              return retStr;
      
        }
    }
}
