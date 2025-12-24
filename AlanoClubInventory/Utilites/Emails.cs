using AlanoClubInventory.Models;
using AlanoClubInventory.SqlServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlanoClubInventory.Utilites
{
    public class Emails
    {
        private static Emails emailInstance;

        private static readonly object _lock = new object();
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private Emails() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public static Emails EmailsInstance
        {
            get
            {
                // Double-checked locking for thread safety
                if (emailInstance == null)
                {
                    lock (_lock)
                    {
                        if (emailInstance == null)
                        {
                            emailInstance = new Emails();
                        }
                    }
                }
                return emailInstance;
            }
        }
        private SendEmailModel ModelEmail { get; set; }
        private async Task GetEmailInformation()
        {
            ReadJsonFile readJson = new ReadJsonFile();

            ModelEmail = readJson.GetJsonData<SendEmailModel>("SendEmailModel").Result;
        }

        public async void SendEmail(string toEmail, string cc, string subject, string body, bool ishtml = false, string? fileName = null)
        {
            try
            {
                await GetEmailInformation();

                var fromAddress = new MailAddress(ModelEmail.EmailFrom, "Butte ALano CLub");
                var toAddress = new MailAddress(toEmail);



                var smtp = new SmtpClient
                {
                    Host = ModelEmail.SmtpServer,
                    Port = ModelEmail.Port,
                    EnableSsl = ModelEmail.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, ModelEmail.Password)
                };

                var message = new MailMessage(fromAddress, toAddress);


                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = ishtml;





                if (!(string.IsNullOrWhiteSpace(fileName)))
                {
                    Attachment attachment = new Attachment(fileName);
                    message.Attachments.Add(attachment);
                }



                //  var message = new MailMessage(fromAddress, toAddress);



                smtp.Send(message);

            }
            catch (Exception ex)
            {
                throw new Exception("Error sending email: " + ex.Message);
            }



        }
        //public async void SendHtmlEmail(string toEmail, string cc, string subject, string body, string htmlBody)
        //{
        //    try
        //    {
        //        await GetEmailInformation();

        //        var fromAddress = new MailAddress(ModelEmail.EmailFrom, "Butte ALano CLub");
        //        var toAddress = new MailAddress("mtcharles@hotmail.com");

        //        AlternateView avHtml = AlternateView.CreateAlternateViewFromString(
        //                                htmlBody, null, MediaTypeNames.Text.Html);
        //        //       AlternateView avHtml = AlternateView.CreateAlternateViewFromString(
        //        //        htmlBody, null, MediaTypeNames.Text.Html);

        //        var smtp = new SmtpClient
        //        {
        //            Host = ModelEmail.SmtpServer,
        //            Port = ModelEmail.Port,
        //            EnableSsl = ModelEmail.EnableSsl,
        //            DeliveryMethod = SmtpDeliveryMethod.Network,
        //            UseDefaultCredentials = false,
        //            Credentials = new NetworkCredential(fromAddress.Address, ModelEmail.Password)
        //        };

        //        using (var message = new MailMessage(fromAddress, toAddress)
        //        {
        //            Subject = subject,
        //            Body = body

        //        })

        //        {
        //            smtp.Send(message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception("Error sending email: " + ex.Message);
        //    }



        //}
        public async Task<bool> IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return mailAddress.Address == email; // Ensures the parsed address matches the input
            }
            catch
            {
                return false;
            }
        }

        public async void SendEmail(string toEmail, string cc, string subject, string body, bool ishtml = false, IList<string>? fileName = null)
        {
            try
            {
                await GetEmailInformation();

                var fromAddress = new MailAddress(ModelEmail.EmailFrom, "Butte ALano CLub");
                var toAddress = new MailAddress(toEmail);



                var smtp = new SmtpClient
                {
                    Host = ModelEmail.SmtpServer,
                    Port = ModelEmail.Port,
                    EnableSsl = ModelEmail.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, ModelEmail.Password)
                };

                var message = new MailMessage(fromAddress, toAddress);


                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = ishtml;




                if(fileName != null && fileName.Count > 0)
                {
                    foreach(var file in fileName)
                    {
                        if (string.Compare(file, Utilites.AlanoCLubConstProp.NA, StringComparison.OrdinalIgnoreCase) != 0)
                        {
                            Attachment attachment = new Attachment(file);
                            message.Attachments.Add(attachment);
                        }
                    }
                }
                



                //  var message = new MailMessage(fromAddress, toAddress);



                smtp.Send(message);

            }
            catch (Exception ex)
            {
                throw new Exception("Error sending email: " + ex.Message);
                
            }



        }
    }
}
