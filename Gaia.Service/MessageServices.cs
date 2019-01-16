using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace Gaia.Service
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            SendEmail(email, subject, message);
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }


        public const string qqemail = "3031518365@qq.com";
        public const string qqemail_user = "3031518365";
        public const string qqemail_pwd = "zyuktngebiwedhff";


        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">要发送的邮箱</param>
        /// <param name="mailSubject">邮箱主题</param>
        /// <param name="mailContent">邮箱内容</param>
        /// <returns>返回发送邮箱的结果</returns>
        public static bool SendEmail(string mailTo, string mailSubject, string mailContent)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Gaia Project", qqemail));
            //message.To.Add(new MailboxAddress("Mrs. Chanandler Bong", "84320362@qq.com"));
            message.To.Add(new MailboxAddress(mailTo, mailTo));

            //            message.Subject = "星期天去哪里玩？";
            //            message.Body = new TextPart("plain") { Text = "我想去故宫玩，如何" };
            message.Subject = mailSubject;
            message.Body = new TextPart("plain") { Text = mailContent };
            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                //client.Connect("smtp.friends.com", 587, false);smtp.qq.com
                client.Connect("smtp.qq.com", 25, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(qqemail_user, qqemail_pwd);

                client.Send(message);
                client.Disconnect(true);
            }
            return true;
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
