using Ecommerce.Application.Services.MailNotifyService.Utils;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.MailNotifyService
{
    public class MailNotifyService : IMailNotifyService
    {
        private readonly MailSettings _mailSettings;
        public MailNotifyService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        private async Task SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = MailUtils.GetSubjectMail(mailContent.EventCode);


            var builder = new BodyBuilder();
            builder.HtmlBody = MailUtils.GetTemplateMail(mailContent.EventCode, mailContent.Data);
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                return;
            }

            smtp.Disconnect(true);
        }
        public async Task SendMailAsync(string email, object data, string eventCode)
        {
            await SendMail(new MailContent()
            {
                To = email,
                Data = data,
                EventCode = eventCode
            });
        }
    }
}
