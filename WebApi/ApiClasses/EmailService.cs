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
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.LabReqInterfaces;
using MimeKit;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
namespace EDocs.Nyp.LabReqs.AppServices.ApiClasses
{
    public class EmailService:IEmailService
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
        private async Task EmailSend(string eMessage,string eSubject)
        {
            var message = new MimeMessage();
            message.To.Add(new MailboxAddress(emailSettings.EmailTo));
            message.From.Add(new MailboxAddress(emailSettings.EmailFrom));
            if(!(string.IsNullOrWhiteSpace(emailSettings.EmailCC)))
            {
                message.Cc.Add(new MailboxAddress(emailSettings.EmailCC));

            }
            message.Subject = eSubject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = EmailHtmlBody(eMessage,eSubject)
            };
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(emailSettings.EmailServer,emailSettings.EmailPort, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.Authenticate(emailSettings.EmailFrom,emailSettings.EmailPassWord);
                client.Send(message);
                client.Disconnect(true);
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
        private string EmailHtmlBody(string eMessage,string subject)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<head>");
            sb.AppendLine($"</head>");
            sb.AppendLine($"<html>");
            
            sb.AppendLine($"<body style={ConstNypLabReqs.DoubleQuotes}background-color:cornflowerblue;{ConstNypLabReqs.DoubleQuotes}>");
            sb.AppendLine($"<h1 class={ConstNypLabReqs.DoubleQuotes}text-center{ConstNypLabReqs.DoubleQuotes}>{subject}</h1>");
            sb.AppendLine($"<div>");
            //sb.AppendLine($"<p class={ConstNypLabReqs.DoubleQuotes}border text-danger{ConstNypLabReqs.DoubleQuotes}>{eMessage}</p>");
            sb.AppendLine($"<p style={ConstNypLabReqs.DoubleQuotes}border;{ConstNypLabReqs.DoubleQuotes}>{eMessage}</p>");
             
            sb.AppendLine($"</div>");
            sb.AppendLine($"</body>");
            sb.AppendLine($"</html>");
            return sb.ToString();
        }
        private List<string> EmailMessageToCCFrom(string emailToFRomCC)
        {
            List<string> retEMess = new List<string>();
            foreach(string emailStr in emailToFRomCC.Split(";"))
            {
                if (string.IsNullOrWhiteSpace(emailStr))
                    retEMess.Add(emailStr);
                
            }
            return retEMess;
        }
    }
}
