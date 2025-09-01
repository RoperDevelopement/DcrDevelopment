using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
 

namespace BMRMobileApp.Utilites
{
    public class EmailService
    {
        
      
       
        public async Task<bool> SendEmailAsync(string recipientEmail, string subject, string body)
        {
            try
            {

                  await MicrosoftAzureAccessTokem.AccessTokenInstance.Value.SendEmailTask(); 

                //string token=   await MicrosoftAzureAccessTokem.AccessTokenInstance.Value.GetAccessTokenAsync(ConfigurationManager.SettingsApp.AzureEmailClientID, ConfigurationManager.SettingsApp.AzureEmaiClientSecert, ConfigurationManager.SettingsApp.AzureEmaiTenantId, ConfigurationManager.SettingsApp.AzureScope);
                if (Microsoft.Maui.ApplicationModel.Communication.Email.Default.IsComposeSupported)
                {
                    var emailMessage = new MimeMessage();
                    emailMessage.From.Add(new MailboxAddress("BMR App", ConfigurationManager.SettingsApp.EmailFrom));
                    emailMessage.To.Add(new MailboxAddress("", recipientEmail));
                    emailMessage.Subject = $"{ConfigurationManager.SettingsApp.EmailSubject} {subject}";
                    emailMessage.Body = new TextPart("plain")
                    {
                        Text = body

                    };
                    using var smtpClient = new SmtpClient();
                    smtpClient.Timeout = 20000; // 20 seconds timeout
                //    smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    smtpClient.ConnectAsync(ConfigurationManager.SettingsApp.SmtpMailOutlookCom, int.Parse(ConfigurationManager.SettingsApp.SmtpMailPortOutlookCom), MailKit.Security.SecureSocketOptions.StartTls).GetAwaiter().GetResult();
                      smtpClient.AuthenticateAsync(ConfigurationManager.SettingsApp.EmailFrom, ConfigurationManager.SettingsApp.PassWordOutlookCom).GetAwaiter().GetResult(); ;
                    
                    
                     smtpClient.SendAsync(emailMessage).GetAwaiter().GetResult(); ;
                     smtpClient.DisconnectAsync(true).GetAwaiter().GetResult(); ;


                    //    using var message = new MailMessage(ConfigurationManager.SettingsApp.EmailFrom, recipientEmail)
                    //    {
                    //        Subject = $"{ConfigurationManager.SettingsApp.EmailSubject} {subject}",
                    //        Body = body,
                    //        IsBodyHtml = false // Flip to true if you want HTML support
                    //    };

                    //    using var smtpClient = new SmtpClient(ConfigurationManager.SettingsApp.SmtpMailOutlookCom,ConfigurationManager.SettingsApp.SmtpMailPortOutlookCom)
                    //    {
                    //        Credentials = new NetworkCredential(ConfigurationManager.SettingsApp.EmailFrom,ConfigurationManager.SettingsApp.PassWordOutlookCom),
                    //        EnableSsl = true,
                    //        //DeliveryMethod = SmtpDeliveryMethod.NetworkNetwork,
                    //        Timeout = 20000 // 20 seconds timeout



                    //    };

                    //    await smtpClient.SendMailAsync(message);
                    return true;
                }
                else
                {
                    // 🔥 Hook for SmartReply nudging or emotion-aware feedback
                  //  Console.WriteLine("Email composition is not supported on this device.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // 🔥 Hook for SmartReply nudging or emotion-aware feedback
              //  Console.WriteLine($"Email send failed: {ex.Message}");
                return false;
            }
        }
    }
}
 
