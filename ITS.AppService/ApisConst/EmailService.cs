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
using MimeKit;
using Edocs.ITS.AppService.Interfaces;
using Edocs.ITS.AppService.ApisConst;

namespace Edocs.ITS.AppService.ApisConst
{ 
    public class EmailService : IEmailService
    {
        private readonly IEmailSettings emailSettings;
        public EmailService(IEmailSettings emailConfiguration)
        {
            emailSettings = emailConfiguration;

        }
        public void SendEmail(string message, string subject)
        {
            EmailSend(message, subject).ConfigureAwait(true);
            EmailSendText(message, subject).ConfigureAwait(true);


        }
        public void SendEmail(string message, string subject, string emailTo, string emailCC)
        {
            EmailSend(message, subject, emailTo, emailCC).ConfigureAwait(true);



        }
        public void SendText(string message, string subject, string textTo, string textCC)
        {

            EmailSendText(message, subject, textTo, textCC).ConfigureAwait(true);


        }
        private async Task EmailSend(string eMessage, string eSubject)
        {
            var message = new MimeMessage();
            message.To.Add(new MailboxAddress(emailSettings.EmailTo));
            message.From.Add(new MailboxAddress(emailSettings.EmailFrom));
            if (!(string.IsNullOrWhiteSpace(emailSettings.EmailCC)))
            {
                message.Cc.Add(new MailboxAddress(emailSettings.EmailCC));

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
        private async Task EmailSend(string eMessage, string eSubject, string emailTo, string emailCC)
        {
            try
            {


                var message = new MimeMessage();
                message.To.Add(new MailboxAddress(emailTo.Trim()));
                message.From.Add(new MailboxAddress(emailSettings.EmailFrom));
                if (!(string.IsNullOrWhiteSpace(emailCC)))
                {
                    message.Cc.Add(new MailboxAddress(emailCC));

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
                Console.WriteLine(ex.Message);
            }

        }
        private async Task EmailSendText(string eMessage, string eSubject)
        {
            var message = new MimeMessage();
            message.To.Add(new MailboxAddress(emailSettings.TextTo));
            message.From.Add(new MailboxAddress(emailSettings.EmailFrom));
            if (!(string.IsNullOrWhiteSpace(emailSettings.TextCC)))
            {
                message.Cc.Add(new MailboxAddress(emailSettings.TextCC));

            }
            message.Subject = "";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Text)
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
        private async Task EmailSendText(string eMessage, string eSubject, string textTo, string textCC)
        {
            try
            {

                textTo = $"{EdocsITSHelpers.RepStr(textTo, "-", string.Empty)}@vtext.com";
                //textTo = PSEHelpers.RepStr(textTo, "-", string.Empty);
                var message = new MimeMessage();
                message.To.Add(new MailboxAddress(textTo));
                message.From.Add(new MailboxAddress(emailSettings.EmailFrom));

                if (!(string.IsNullOrWhiteSpace(textCC)))
                {
                    message.Cc.Add(new MailboxAddress(textCC));

                }
                message.Subject = eSubject;
                message.Body = new TextPart(MimeKit.Text.TextFormat.Text)
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
                Console.WriteLine(ex.Message);
            }

        }
        private string EmailHtmlBody(string eMessage, string subject)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<head>");
            sb.AppendLine($"</head>");
            sb.AppendLine($"<html>");

            sb.AppendLine($"<body style={EdocsITSHelpers.QUOTE}background-color:cornflowerblue;{EdocsITSHelpers.QUOTE}>");
            sb.AppendLine($"<h1 class={EdocsITSHelpers.QUOTE}text-center{EdocsITSHelpers.QUOTE}>{subject}</h1>");
            sb.AppendLine($"<div>");
            //sb.AppendLine($"<p class={ConstNypLabReqs.DoubleQuotes}border text-danger{ConstNypLabReqs.DoubleQuotes}>{eMessage}</p>");
            sb.AppendLine($"<p style={EdocsITSHelpers.QUOTE}border;{EdocsITSHelpers.QUOTE}>{eMessage}</p>");

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
