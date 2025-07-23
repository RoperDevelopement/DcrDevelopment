using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Net;
using ScanQuire_SendEmails.Properties;
using EdocsUSA.Utilities;
namespace ScanQuire_SendEmails
{
    public class Send_Emails
    {
        private string EmailFrom
        { get { return Settings.Default.EmailFrom; } }
        private string EmailTo
        { get { return Settings.Default.EmailTo; } }

        private string EmailPasswordKey
        { get { return Settings.Default.EmailPasswordKey; } }

        private string EmailPassword
        { get { return Settings.Default.EmailPassword; } }

        private string EmailCC
        { get { return Settings.Default.EmailCC; } }

        private string EmailServer
        { get { return Settings.Default.EmailServer; } }

        private string EmailSubject
        { get { return Settings.Default.EmailSubject; } }

        private int EmailPort
        { get { return Settings.Default.EmailPort; } }

        private string TextTo
        { get { return Settings.Default.TextTo; } }

        private string TextCc
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
            MailMessage textMessage = NewMailMessage(TextTo);
            if (err)
                textMessage.Subject = TxtSubject.Replace("{ErrorSuccess}", "Errors").Replace("{runDate}", DateTime.Now.ToString());
            else
                textMessage.Subject = TxtSubject.Replace("{ErrorSuccess}", "Successfully").Replace("{runDate}", DateTime.Now.ToString());
            textMessage.Body = message;
            textMessage.IsBodyHtml = false;
            foreach (string eCC in TextCc.Split(';'))
            {
                if (!(string.IsNullOrWhiteSpace(eCC)))
                    textMessage.CC.Add(eCC);
            }
            SendMessage(textMessage);
        }
        public void UpDateEmailPw(string newPw)
        {
            Edocs_Utilities.EdocsUtilitiesInstance.PasswordKey = EmailPasswordKey;
            //  Settings.Default.EmailPassword = Edocs_Utilities.EdocsUtilitiesInstance.DecryptToString(newPw, DataProtectionScope.CurrentUser);
            Settings.Default.EmailPassword = Edocs_Utilities.EdocsUtilitiesInstance.EncryptCipher(newPw, EmailPasswordKey);
            Settings.Default.Save();


        }
        public void SendEmail(string emailTo, string emailMessage, string emailSubject)
        {
            string emailCC = string.Empty;
            if (string.IsNullOrWhiteSpace(emailTo))
                emailTo = EmailTo;
            else
            {
                emailCC = EmailTo;
            }

            MailMessage messageEmail = NewMailMessage(emailTo, EmailFrom,string.Empty);

            if (!(string.IsNullOrWhiteSpace(EmailCC)))
            {
                emailCC += EmailCC;
            }

            //foreach (string eTo in emailTo.Split(';'))
            //{
            //    messageEmail.To.Add(eTo);
            //}
            foreach (string eCC in emailCC.Split(';'))
            {
                if (!(string.IsNullOrWhiteSpace(eCC)))
                    messageEmail.CC.Add(eCC);
            }
            if (string.IsNullOrWhiteSpace(emailSubject))
            {
                emailSubject = EmailSubject;
            }
            emailSubject = $"{emailSubject} email sent on{DateTime.Now.ToString()}";
            messageEmail.Body = emailMessage;
            messageEmail.IsBodyHtml = false;
            messageEmail.Subject = emailSubject;
            SendMessage(messageEmail);
        }

        public void SendEmail(string emailFrom, string emailTo, string emailCC, string emailMessage, string emailSubject,List<string> emailAttachment)
        {


            MailMessage messageEmail = NewMailMessage(emailTo, emailFrom, $"Email from {emailFrom}");

            //foreach (string eTo in emailTo.Split(';'))
            //{
            //    messageEmail.To.Add(eTo);
            //}
            foreach (string eCC in emailCC.Split(';'))
            {
                if (!(string.IsNullOrWhiteSpace(eCC)))
                    messageEmail.CC.Add(eCC);
            }
            if (string.IsNullOrWhiteSpace(emailSubject))
            {
                emailSubject = EmailSubject;
            }

            if ((emailAttachment != null) && (emailAttachment.Count > 0))
            {
             


                    foreach (string att in emailAttachment)
                    {
                        Edocs_Utilities.EdocsUtilitiesInstance.CHeckFileExists(att, false);
                        messageEmail.Attachments.Add(new Attachment(att));
                    }
               
            }
            emailSubject = $"{emailSubject} email sent on{DateTime.Now.ToString()}";
            messageEmail.Body = emailMessage;
            messageEmail.IsBodyHtml = false;
            messageEmail.Subject = emailSubject;
            SendMessage(messageEmail);
        }

        private void SendMessage(MailMessage messageEmail)
        {
            //if(true)
            //{ 
            messageEmail.Sender = new MailAddress(EmailFrom);
            messageEmail.From = new MailAddress(EmailFrom);
            //}
            SmtpClient mailClient = new SmtpClient(EmailServer);
            mailClient.Port = EmailPort;
            mailClient.UseDefaultCredentials = false;
            mailClient.EnableSsl = true;
       //  string emailPassord = GetEmailPassword();
        string emailPassord = "6746edocs";
            NetworkCredential nc = new NetworkCredential(EmailFrom, emailPassord);
            mailClient.Credentials = nc;
            mailClient.Send(messageEmail);
            mailClient.Dispose();
            messageEmail.Dispose();
        }
        //private void SendMessage(MailMessage messageEmail)
        //{

        //    //messageEmail.Sender = new MailAddress(emailFrom);
        //    //messageEmail.From = new MailAddress(emailFrom);
        //    SmtpClient mailClient = new SmtpClient(EmailServer);
        //    mailClient.Port = EmailPort;
        //    mailClient.UseDefaultCredentials = true;
        //    mailClient.EnableSsl = true;

        //    //string emailPassord = GetEmailPassword();
        //    string emailPassord = "6746edocs";
        //    NetworkCredential nc = new NetworkCredential(EmailFrom, emailPassord);
        //    mailClient.Credentials = nc;

        //    mailClient.Send(messageEmail);
        //}
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
        public MailMessage NewMailMessage(string emailTo, string emailFrom, string displayName)
        {
            try
            {
                MailMessage emailMessage = new MailMessage();
                if (!(string.IsNullOrWhiteSpace(displayName)))
                {
                    emailMessage.Sender = new MailAddress(emailFrom,displayName);
                    emailMessage.From = new MailAddress(emailFrom,displayName);
                    emailMessage.ReplyToList.Add(emailFrom);
                }
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
            Edocs_Utilities.EdocsUtilitiesInstance.PasswordKey = EmailPasswordKey;
             string retStr = Edocs_Utilities.EdocsUtilitiesInstance.DecryptToString(EmailPassword, DataProtectionScope.LocalMachine);
         //   string retStr = Edocs_Utilities.EdocsUtilitiesInstance.DecryptCipher(EmailPassword, EmailPasswordKey);
            return retStr;

        }
    }
}
