using HRIS.Domain;
using HRIS.Domain.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task<bool> SendMail(MailData mailData)
        {
            var emailMessage = CreateEmailMessage(mailData);
            var result = Send(emailMessage);
            return result;

        }

        private MimeMessage CreateEmailMessage(MailData mailData)
        {

            MimeMessage emailMessage = new MimeMessage();
            MailboxAddress emailFrom = new MailboxAddress(_mailSettings.Name, _mailSettings.EmailId);
            emailMessage.From.Add(emailFrom);
            MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
            emailMessage.To.Add(emailTo);
            emailMessage.Subject = mailData.EmailSubject;
            BodyBuilder emailBodyBuilder = new BodyBuilder();

            //plain text body
            emailBodyBuilder.TextBody = mailData.EmailBody;

            //html body
            emailBodyBuilder.HtmlBody = "<h1>Hello </h1>" + mailData.EmailBody;

            emailMessage.Body = emailBodyBuilder.ToMessageBody();

            return emailMessage;

        }


        private bool Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {

                    client.Connect(_mailSettings.Host, _mailSettings.Port, _mailSettings.UseSSL);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                    client.Send(mailMessage);

                    return true;

                }

                catch (Exception ex)
                {
                    return false;
                }

                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }

            }
        }
    }
}
