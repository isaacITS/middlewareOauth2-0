using System.Net;
using System.Net.Mail;

namespace ITS_Middleware.Helpers
{
    public static class SendEmail
    {
        public static void SendEmailReq(string toEmail, string body, string subject)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("noreply.its.portalconfig@gmail.com", "dvqxxkdwmuynsboa");
            smtpClient.EnableSsl = true;

            var email = new MailMessage();
            email.From = new MailAddress("noreply.its.portalconfig@gmail.com");
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
