﻿using System.Net;
using System.Net.Mail;

namespace login.Helpers
{
    public static class SendEmail
    {
        public static void SendEmailReq(string toEmail, string body, string subject, string fromEmail, string passFromEmail)
        {
            try
            {
                string FromEmail = fromEmail;
                string FromEmailKey = passFromEmail;
                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(FromEmail, FromEmailKey),
                    EnableSsl = true
                };

                var email = new MailMessage
                {
                    From = new MailAddress(FromEmail)
                };
                email.To.Add(toEmail);
                email.SubjectEncoding = System.Text.Encoding.UTF8;
                email.Subject = subject;
                email.Body = body;
                email.IsBodyHtml = true;
                email.Priority = MailPriority.Normal;

                smtpClient.Send(email);
                email.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
