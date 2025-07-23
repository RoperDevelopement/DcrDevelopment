using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using BinMonitor.BinInterfaces;
using BinMonitorAppService.Constants;
using MimeKit;

using System.IO;
namespace BinMonitorAppService.ApiClasses
{
    public class EmailService : IEmailService
    {
        //private readonly IEmailSettings emailSettings;
        //public EmailService(IEmailSettings emailConfiguration)
        //{
        //    emailSettings = emailConfiguration;
        //}

        public EmailService()
        {

        }
        public void SendEmail(string message, string subject, IEmailSettings emailSettings)
        {
            EmailSend(message, subject, emailSettings).ConfigureAwait(true).GetAwaiter().GetResult();
            EmailSendText(message, subject, emailSettings).ConfigureAwait(true).GetAwaiter().GetResult();


        }
        public void SendHtmlEmail(string htmlMessage, string subject, IEmailSettings emailSettings)
        {
            EmailSendHtml(htmlMessage, subject, emailSettings).ConfigureAwait(true);
            EmailSendText(subject, subject, emailSettings).ConfigureAwait(true);


        }
        public void SendEmail(string message, string subject, bool sendText, IEmailSettings emailSettings)
        {
            if (sendText)
                SendEmail(message, subject, emailSettings);
            else
                EmailSend(message, subject, emailSettings).ConfigureAwait(true).GetAwaiter().GetResult();

        }
        public void SendHtmlEmail(string htmlFile, string subject, IEmailSettings emailSettings, string cwid, string userEmailAddress)
        {
            string fileHtml = File.ReadAllText(htmlFile);
            fileHtml = fileHtml.Replace(SqlConstants.RepStrCwid, cwid).Replace(SqlConstants.RepStrEmailAddress, userEmailAddress);
            EmailSendHtml(fileHtml, subject, emailSettings).ConfigureAwait(true).GetAwaiter().GetResult(); 
            EmailSendText(fileHtml, subject, emailSettings).ConfigureAwait(true).GetAwaiter().GetResult();

        }
        private async Task EmailSend(string eMessage, string eSubject, IEmailSettings emailSettings)
        {
            try
            {


                var message = new MimeMessage();
                foreach (string emailAddTo in emailSettings.EmailTo.Split(';'))
                {
                    if (!(string.IsNullOrWhiteSpace(emailAddTo)))
                        message.To.Add(new MailboxAddress(emailAddTo));
                }
                //message.To.Add(new MailboxAddress(emailSettings.EmailTo));
                message.From.Add(new MailboxAddress(emailSettings.EmailFrom));
                if (!(string.IsNullOrWhiteSpace(emailSettings.EmailCC)))
                {
                    foreach (string emailAddCC in emailSettings.EmailCC.Split(';'))
                        if (!(string.IsNullOrWhiteSpace(emailAddCC)))
                            message.Cc.Add(new MailboxAddress(emailAddCC));

                }
                message.Subject = eSubject;
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = EmailHtmlBody(eMessage, eSubject)
                };
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(emailSettings.EmailServer, emailSettings.EmailPort, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.Authenticate(emailSettings.EmailFrom, emailSettings.EmailPassWord);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task EmailSendText(string eMessage, string eSubject, IEmailSettings emailSettings)
        {
            var message = new MimeMessage();

            foreach (string emailAddTo in emailSettings.TextTo.Split(';'))
            {
                if (!(string.IsNullOrWhiteSpace(emailAddTo)))
                    message.To.Add(new MailboxAddress(emailAddTo));
            }
            message.From.Add(new MailboxAddress(emailSettings.EmailFrom));
            if (!(string.IsNullOrWhiteSpace(emailSettings.TextCC)))
            {
                foreach (string emailAddCC in emailSettings.TextCC.Split(';'))
                    if (!(string.IsNullOrWhiteSpace(emailAddCC)))
                        message.Cc.Add(new MailboxAddress(emailAddCC));

            }
            message.Subject = "";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = eMessage
            };
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(emailSettings.EmailServer, emailSettings.EmailPort, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.Authenticate(emailSettings.EmailFrom, emailSettings.EmailPassWord);
                client.Send(message);
                client.Disconnect(true);
            }

        }
        private async Task EmailSendHtml(string eMessage, string eSubject, IEmailSettings emailSettings)
        {
            try
            {


                var message = new MimeMessage();
                foreach (string emailAddTo in emailSettings.EmailTo.Split(';'))
                {
                    if (!(string.IsNullOrWhiteSpace(emailAddTo)))
                        message.To.Add(new MailboxAddress(emailAddTo));
                }

                message.From.Add(new MailboxAddress(emailSettings.EmailFrom));
                if (!(string.IsNullOrWhiteSpace(emailSettings.EmailCC)))
                {
                    foreach (string emailAddCC in emailSettings.EmailCC.Split(';'))
                        if (!(string.IsNullOrWhiteSpace(emailAddCC)))
                            message.Cc.Add(new MailboxAddress(emailAddCC));

                }
                message.Subject = eSubject;
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = eMessage
                };
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(emailSettings.EmailServer, emailSettings.EmailPort, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.Authenticate(emailSettings.EmailFrom, emailSettings.EmailPassWord);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private string EmailHtmlBody(string eMessage, string subject)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<head>");
            sb.AppendLine($"</head>");
            sb.AppendLine($"<html>");

            sb.AppendLine($"<body style={SqlConstants.DoubleQuotes}background-color:cornflowerblue;{SqlConstants.DoubleQuotes}>");
            sb.AppendLine($"<h1 class={SqlConstants.DoubleQuotes}text-center{SqlConstants.DoubleQuotes}>{subject}</h1>");
            sb.AppendLine($"<div>");
            //sb.AppendLine($"<p class={ConstNypLabReqs.DoubleQuotes}border text-danger{ConstNypLabReqs.DoubleQuotes}>{eMessage}</p>");
            sb.AppendLine($"<p style={SqlConstants.DoubleQuotes}border;{SqlConstants.DoubleQuotes}>{eMessage}</p>");

            sb.AppendLine($"</div>");
            sb.AppendLine($"</body>");
            sb.AppendLine($"</html>");
            return sb.ToString();
        }
        private List<string> EmailMessageToCCFrom(string emailToFRomCC)
        {
            List<string> retEMess = new List<string>();
            foreach (string emailStr in emailToFRomCC.Split(";"))
            {
                if (string.IsNullOrWhiteSpace(emailStr))
                    retEMess.Add(emailStr);

            }
            return retEMess;
        }
    }
}
