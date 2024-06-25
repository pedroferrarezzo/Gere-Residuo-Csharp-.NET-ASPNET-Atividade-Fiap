using System.Net.Mail;
using System.Net;

namespace Br.Com.Fiap.Gere.Residuo.Utils.Email
{
    public class SmtpUtils : ISmtpUtils
    {
        private readonly string _mailTrapUser;
        private readonly string _mailTrapPassword;
        private readonly string _mailTrapHost;
        private readonly string _mailTrapSender;
        private readonly int _mailTrapPort;

        public SmtpUtils (string mailTrapUser, string mailTrapPassword, string mailTrapHost, int mailTrapPort, string mailTrapSender)
        {
            _mailTrapUser = mailTrapUser;
            _mailTrapPassword = mailTrapPassword;
            _mailTrapHost = mailTrapHost;
            _mailTrapPort = mailTrapPort;
            _mailTrapSender = mailTrapSender;
        }

        public Task SendEmailAsync(string recipient, string subject, string message)
        {
            var client = new SmtpClient(_mailTrapHost, _mailTrapPort)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_mailTrapUser, _mailTrapPassword)
            };

            return client.SendMailAsync(
                new MailMessage(from: _mailTrapSender,
                                to: recipient,
                                subject,
                                message
                                ));
        }
    }
}
