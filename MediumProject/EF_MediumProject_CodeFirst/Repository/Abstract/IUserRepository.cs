using MediumProject.EF_MediumProject_CodeFirst.Entities;

namespace MediumProject.EF_MediumProject_CodeFirst.Repository.Abstract
{
    public interface IUserRepository
    {
        string SignUp(User user);
        public User GetUserById(int id);
        void DeactivateUser(int userId);
        bool UpdateUserInfo(User user);
        User GetUserByEmail(string email);
        User GetUserByUrl(string url);
        string Login(string email);
        User ValidateUser(string token, bool isLogin);
    }
}
