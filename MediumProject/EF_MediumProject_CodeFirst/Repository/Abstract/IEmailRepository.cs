using MediumProject.EF_MediumProject_CodeFirst.Entities;
using MediumProject.Models;

namespace MediumProject.EF_MediumProject_CodeFirst.Repository.Abstract
{
    public interface IEmailRepository
    {
        void SendEmail(MailRequest mailRequest);
        void SendActivationLinkToUser(User user, string activationLink);
    }
}
