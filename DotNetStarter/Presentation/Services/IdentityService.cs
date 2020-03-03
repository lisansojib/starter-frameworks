using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Presentation.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailService : IIdentityMessageService
    {
        /// <summary>
        /// 
        /// </summary>
        public static string host = "smtp-mail.outlook.com";

        /// <summary>
        /// 
        /// </summary>
        public static int port = 587;

        /// <summary>
        /// 
        /// </summary>
        public static string username = "ozrtcnu@outlook.com";

        /// <summary>
        /// 
        /// </summary>
        public static string password = "Merc21!@#";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendAsync(IdentityMessage message)
        {
            MailMessage mail = new MailMessage();
            try
            {
                mail.To.Add(message.Destination);
                mail.From = new MailAddress(username, "WebRTC to OZRTC.com");
                mail.Subject = message.Subject;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;
                mail.Body = message.Body;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                mail.CC.Add(message.Destination);

                var credentials = new NetworkCredential
                {
                    UserName = username,
                    Password = password,
                    Domain = "ozrtc.com"
                };

                SmtpClient client = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = credentials,
                    Host = host,
                    Port = port,
                    EnableSsl = true
                };
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception)
            {
                mail.Dispose();
                throw;
            }
            return Task.FromResult(0);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SmsService : IIdentityMessageService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}