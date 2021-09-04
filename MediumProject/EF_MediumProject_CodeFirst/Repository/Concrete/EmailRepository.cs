using MediumProject.EF_MediumProject_CodeFirst.Entities;
using MediumProject.EF_MediumProject_CodeFirst.Repository.Abstract;
using MediumProject.Models;
using MediumProject.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using System.Net.Mail;

namespace MediumProject.EF_MediumProject_CodeFirst.Repository.Concrete
{
    public class EmailRepository : IEmailRepository
    {
        private readonly MailSettings mailSettings;
        public EmailRepository(IOptions<MailSettings> mailSettings)
        {
            this.mailSettings = mailSettings.Value;
        }
        public void SendEmail(MailRequest mailRequest)
        {
            var email = new MailMessage();
            email.Sender = new MailAddress(mailSettings.Mail);
            email.From = new MailAddress(mailSettings.Mail);
            email.To.Add((MailAddress)MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;

            BodyBuilder builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.HtmlBody;            

            NetworkCredential networkCredential = new NetworkCredential(mailSettings.Mail, mailSettings.Password);

            var smtp = new SmtpClient();
            smtp.Host = mailSettings.Host;
            smtp.Port = mailSettings.Port;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = networkCredential;
            smtp.Send(email);
            smtp.Dispose();
        }
        public void SendActivationLinkToUser(User user, string activationLink)
        {
            MailRequest mailRequest = new MailRequest();
            mailRequest.ToEmail = user.EmailAddress;
            string subject = "Click here to activate your account: ";
            if (user.IsActive)
            {
                mailRequest.Subject = "Login to Your Account";
                subject = "Click here to login: ";
            }
            else
            {
                mailRequest.Subject = "Acount Activation";
            }
            mailRequest.Body = subject + activationLink;

            SendEmail(mailRequest);
        }
    }
}
