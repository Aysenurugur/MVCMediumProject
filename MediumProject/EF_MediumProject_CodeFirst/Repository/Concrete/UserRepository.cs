using MediumProject.EF_MediumProject_CodeFirst.Context;
using MediumProject.EF_MediumProject_CodeFirst.Entities;
using MediumProject.EF_MediumProject_CodeFirst.Repository.Abstract;
using MediumProject.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace MediumProject.EF_MediumProject_CodeFirst.Repository.Concrete
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(MediumContext context, IOptions<AppSettings> appSettings)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
        }
        MediumContext context;
        private readonly AppSettings appSettings;

        public string Login(string email)
        {
            foreach (User item in GetUsers())
            {
                if (item.EmailAddress == email && item.IsActive)
                {
                    return GenerateJwtToken(item);
                }
            }
            return null;
        }
        private List<User> GetUsers()
        {
            return context.Users.ToList();
        }
        public string SignUp(User user)
        {
            try
            {
                Regex rgx = new Regex("[^a-zA-Z -]");
                user.CreatedDate = DateTime.Now;
                user.UserName = user.EmailAddress.Split('@')[0];
                user.FullName = user.EmailAddress.Split('@')[0];
                user.Url = rgx.Replace(user.EmailAddress.Split('@')[0], "");
                user.IsActive = false;
                context.Users.Add(user);
                context.SaveChanges();

                return GenerateJwtToken(user);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public User GetUserById(int id)
        {
            return context.Users.Find(id);
        }
        public void DeactivateUser(int userId)
        {
            User user = GetUserById(userId);
            user.IsActive = false;
            context.SaveChanges();
        }
        public bool UpdateUserInfo(User user)
        {
            try
            {
                Regex rgx = new Regex("[^a-zA-Z -]");
                User newUser = context.Users.Find(user.UserID);
                newUser.UserName = user.UserName;
                newUser.Url = rgx.Replace(user.Url, "");
                newUser.EmailAddress = user.EmailAddress;
                newUser.FullName = user.FullName;
                newUser.Description = user.Description;
                newUser.ModifiedDate = DateTime.Now;
                if (user.PhotoPath != @"Uploads\user-icon.jpg") //if user didn't choose a new photo, this helps not to use default icon
                    newUser.PhotoPath = user.PhotoPath;
                return context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public User GetUserByEmail(string email)
        {
            return context.Users.Where(x => x.EmailAddress == email).FirstOrDefault();
        }
        public User GetUserByUrl(string url)
        {
            return context.Users.Where(x => x.Url == url).FirstOrDefault();
        }
        private void ActivateUser(int id)
        {
            User user = GetUserById(id);
            user.IsActive = true;
            context.SaveChanges();
        }
        public User ValidateUser(string token, bool isLogin)
        {
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            string userId = string.Empty;
            SecurityToken securityToken = null;
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters { ValidateLifetime = true, IssuerSigningKey = new SymmetricSecurityKey(key), ValidateAudience = false, ValidateIssuer = false }, out securityToken);
                if (securityToken != null)
                {
                    userId = ((JwtSecurityToken)securityToken).Claims.Where(x => x.Type == "id").First().Value;
                    if (!isLogin)
                        ActivateUser(Convert.ToInt32(userId));
                }
                return GetUserById(Convert.ToInt32(userId));
            }
            catch (Exception)
            {
                return null;
            }
        }
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserID.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
