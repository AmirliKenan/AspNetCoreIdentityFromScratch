using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MimeKit;

namespace CoreIdentity.Helper
{
    public static class PasswordReset
    {

        public static void PasswordResetSendEmail(string link,string email) 
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("amirlikenan@gmail.com");
                mail.To.Add(email);
                mail.Subject= "Reset Password";

                mail.Body = $"<a href='{link}'>Reset Password Link</a>";
                mail.IsBodyHtml = true;
              

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("amirlikenan@gmail.com", "hmplwrhshyinwabu");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }








            }
    }
}
