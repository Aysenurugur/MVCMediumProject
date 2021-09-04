using MediumProject.EF_MediumProject_CodeFirst.Entities;
using MediumProject.Models.Model_Repository.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MediumProject.Models.Model_Repository.Concrete
{
    public class UserVMRepository : IUserVMRepository
    {
        public UserVMRepository(IWebHostEnvironment Environment)
        {
            this.Environment = Environment;
        }
        IWebHostEnvironment Environment;
        public User ConvertUserModelToUser(UserVM user)
        {
            User newUser = new User()
            {
                Description = user.Description,
                EmailAddress = user.EmailAddress,
                FullName = user.FullName,
                ModifiedDate = DateTime.Now,
                UserName = user.UserName,
                PhotoPath = GetPhotoFile(user.PhotoFile)
            };
            if (user.Url == null)
            {
                newUser.Url = user.EmailAddress.Split('@')[0];
                GetUrlFormat(newUser.Url);
            }
            else { newUser.Url = GetUrlFormat(user.Url); }
            return newUser;
        }
        public UserVM ConvertUserToUserModel(User user)
        {
            UserVM newUser = new UserVM()
            {
                UserID = user.UserID,
                Description = user.Description,
                EmailAddress = user.EmailAddress,
                FullName = user.FullName,
                Url = user.Url,
                UserName = user.UserName,
                PhotoPath = user.PhotoPath
            };
            return newUser;
        }
        private string GetPhotoFile(IFormFile postedFile)
        {
            if (postedFile == null)
            {
                return @"Uploads\user-icon.jpg";
            }
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Path.GetFileName(postedFile.FileName);
            FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            postedFile.CopyTo(stream);
            stream.Close();

            return GetPhotoPathFormat(stream.Name);
        }
        private string GetPhotoPathFormat(string path)
        {
            string[] str = path.Split(@"\");
            string newPath = @"\" + str[str.Length - 2] + @"\" + str[str.Length - 1];
            return newPath;
        }
        private string GetUrlFormat(string url)
        {
            var rgx = new Regex("[^a-zA-Z -]");
            return rgx.Replace(url, "");
        }
    }
}
