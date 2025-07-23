using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.Send.Emails;
namespace Edocs.Service.UploadAuditLogs
{
   public class SEmail
    {
        static readonly SEmail Instance = new SEmail();
        public static SEmail SendEMailsInstance
        { get { return Instance; } }

        private string GetEmailBody(string message, bool errorMessage)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html>");
            sb.AppendLine(@"<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml""> ");
            sb.AppendLine("<head>");
            sb.AppendLine(@"<meta charset=""utf-8""/>");
            sb.AppendLine("<title>Uplaod Audit files to Azure Cloud</title>");
            sb.AppendLine("</head>");
            if (errorMessage)
                sb.AppendLine(@"<body style=""background-color:#fff957"">");
            else
                sb.AppendLine(@"<body style=""background-color:#00ffff"">");

            if (errorMessage)
                sb.AppendLine($"<h1> Errors found running Upload Audit Logs to Azure Cloud Run time:{DateTime.Now.ToString()}</h1>");
            else
                sb.AppendLine($"<h1>No Errors found running Upload Audit Logs to Azure CLoud Run time:{DateTime.Now.ToString()}</h1>");
            sb.AppendLine("</br></br>");
            sb.AppendLine($"<p>Message</p></br></br> {message}");
            sb.AppendLine("</br></br>");
            sb.AppendLine($"<p> Ran on computer {Environment.MachineName}</p>");
            sb.AppendLine($"<p> Ran for user{Environment.UserName}</p>");
            sb.AppendLine($"<p> Assembly name {ServiceHelpers.Instance.GetAssemblyName()}</p>");
            sb.AppendLine($"<p> Assembly Version {ServiceHelpers.Instance.GetAssemblyVersion()}</p>");
            sb.AppendLine($"<p> Assembly File Version {ServiceHelpers.Instance.GetAssemblyFileVersion()}</p>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();

        }

        //public void EmailSend(string message)
         

        public void EmailSend(string message, bool errorMessage)
        {
            try
            {
                string compDomain = $"{Environment.UserDomainName}\\{Environment.UserName}";
                string emailSubject = $"{Send_Emails.EmailInstance.EmailSubject.Replace("{ProcessName}", ServiceHelpers.Instance.GetAssemblyName())}";
                emailSubject = $"{emailSubject.Replace("{DateTime}", DateTime.Now.ToString())}";
                emailSubject = $"{emailSubject.Replace("{ComputerName}", compDomain)}";
                if (errorMessage)
                {
                    emailSubject = $"{emailSubject.Replace("{PassFail}", "Failed")}";
                }
                else
                    emailSubject = $"{emailSubject.Replace("{PassFail}", "Passed")}";
              //  Send_Emails.EmailInstance.EmailSubject = emailSubject;
               string emailBody = GetEmailBody(message, errorMessage);
                if(errorMessage)
                Send_Emails.EmailInstance.SendEmail(string.Empty, emailBody, emailSubject, string.Empty, true,message); 
                else
                    Send_Emails.EmailInstance.SendEmail(string.Empty, emailBody, emailSubject, string.Empty, true,string.Empty);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
