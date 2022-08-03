using System.Net;
using System.Net.Mail;

namespace login.Helpers
{
    public static class SendEmail
    {
        private static readonly IConfiguration configuration;
        public static void SendEmailReq(string toEmail, string body, string subject)
        {
            string FromEmail = configuration.GetSection("ApplicationSettings:EmailConfig:email").Value.ToString();
            string FromEmailKey = configuration.GetSection("ApplicationSettings:EmailConfig:password").Value.ToString();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential(FromEmail, FromEmailKey);
            smtpClient.EnableSsl = true;

            var email = new MailMessage();
            email.From = new MailAddress(FromEmail);
            email.To.Add(toEmail);
            email.SubjectEncoding = System.Text.Encoding.UTF8;
            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;

            smtpClient.Send(email);
            email.Dispose();
        }
    }
}
