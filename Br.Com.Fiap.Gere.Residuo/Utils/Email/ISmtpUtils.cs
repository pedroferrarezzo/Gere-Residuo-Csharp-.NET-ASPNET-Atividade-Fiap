namespace Br.Com.Fiap.Gere.Residuo.Utils.Email
{
    public interface ISmtpUtils
    {
        public Task SendEmailAsync(string recipient, string subject, string message);
    }
}
