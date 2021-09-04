using MediumProject.EF_MediumProject_CodeFirst.Entities;
using MediumProject.EF_MediumProject_CodeFirst.Repository.Abstract;
using MediumProject.Models;
using MediumProject.Models.Model_Repository.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace MediumProject.Controllers
{
    public class UserController : Controller
    {
        public UserController(IUserRepository userRepository, IStoryRepository storyRepository, IEmailRepository emailRepository,
            IUserVMRepository userVMRepository, IWebHostEnvironment Environment, ITopicRepository topicRepository,
            IStoryVMRepository storyVMRepository)
        {
            this.userRepository = userRepository;
            this.storyRepository = storyRepository;
            this.emailRepository = emailRepository;
            this.userVMRepository = userVMRepository;
            this.Environment = Environment;
            this.topicRepository = topicRepository;
            this.storyVMRepository = storyVMRepository;
        }
        #region Interfaces
        IUserRepository userRepository;
        IStoryRepository storyRepository;
        IEmailRepository emailRepository;
        IUserVMRepository userVMRepository;
        IWebHostEnvironment Environment;
        ITopicRepository topicRepository;
        IStoryVMRepository storyVMRepository;
        #endregion

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVM user)
        {
            User newUser = userRepository.GetUserByEmail(user.EmailAddress);
            if (newUser != null && newUser.IsActive)
            {
                string token = userRepository.Login(user.EmailAddress);
                if (token != null)
                {
                    var confirmationLink = Url.Action("ActivateUser", "User", new { token = token }, Request.Scheme);
                    emailRepository.SendActivationLinkToUser(newUser, confirmationLink);
                    TempData["Message"] = "You can log in to your account by using the activation link in your inbox.";
                }
                else
                {
                    TempData["Error"] = "There has been an error, please try again.";
                }
                return RedirectToAction(nameof(Index), "Home");
            }
            ViewBag.Error = "Please check your e-mail, if you don't have an account Sign Up!";
            return View();
        }
        public IActionResult ActivateUser(string token)
        {
            User user = userRepository.ValidateUser(token, true);
            if (user != null)
            {
                HttpContext.Session.SetInt32("Login", user.UserID);
                HttpContext.Session.SetString("Url", user.Url);
                HttpContext.Session.SetString("UserName", user.UserName);
                if (topicRepository.GetFavouriteTopicsByUserID(user.UserID).Count == 0)
                {
                    return RedirectToAction("SelectTopics", "Topic");
                }
            }
            else
                TempData["Error"] = "Your token is expired, please try logging in again.";
            return RedirectToAction(nameof(Index), "Home");
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("Login");
            HttpContext.Session.Remove("userName");
            return RedirectToAction(nameof(Index), "Home");
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(UserVM user)
        {
            if (userRepository.GetUserByEmail(user.EmailAddress) == null)
            {
                User newUser = userVMRepository.ConvertUserModelToUser(user);
                string token = userRepository.SignUp(newUser);

                if (token != null)
                {
                    var confirmationLink = Url.Action("RegisterUser", "User", new { token = token }, Request.Scheme);
                    emailRepository.SendActivationLinkToUser(newUser, confirmationLink);
                    TempData["Message"] = "Your activation link has been sent. Please check your inbox.";
                }
                else
                {
                    TempData["Error"] = "This email address has been taken by another user.";
                }
                return RedirectToAction(nameof(Index), "Home");
            }
            ViewBag.Error = "This email address has been taken by another user.";
            return View();
        }
        public IActionResult RegisterUser(string token)
        {
            User user = userRepository.ValidateUser(token, false);
            if (user != null)
            {
                return RedirectToAction("Login");
            }
            else
                ViewBag.Error = "Your token is expired, please try logging in again.";
            return RedirectToAction(nameof(Index), "Home");
        }
        [Route("{url}")]
        public IActionResult Account(string url)
        {
            User user = userRepository.GetUserByUrl(url);
            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }
            List<Story> stories = storyRepository.GetStoriesByUserID(user.UserID);
            UserWithStories userWithStories = new UserWithStories()
            {
                User = userVMRepository.ConvertUserToUserModel(user)
            };
            List<StoryVM> storyVMs = new List<StoryVM>();
            foreach (Story item in stories)
            {
                StoryVM storyVM = storyVMRepository.ConvertStoryToStoryModel(item);
                storyVM.User = userWithStories.User;
                storyVMs.Add(storyVM);
            }
            userWithStories.Stories = storyVMs;
            return View(userWithStories);
        }
        public IActionResult AccountSettings()
        {
            int id = (int)HttpContext.Session.GetInt32("Login");
            User user = userRepository.GetUserById(id);
            UserVM userVM = userVMRepository.ConvertUserToUserModel(user);
            return View(userVM);
        }
        [HttpPost]
        public IActionResult AccountSettings(UserVM user)
        {
            User newUser = userVMRepository.ConvertUserModelToUser(user);
            int id = (int)HttpContext.Session.GetInt32("Login");
            newUser.UserID = id;
            bool check = userRepository.UpdateUserInfo(newUser);
            if (check)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "There has been an error while updating your information, please try again later.";
            }
            return View(user);
        }
        public IActionResult MyStories()
        {
            int id = (int)HttpContext.Session.GetInt32("Login");
            User user = userRepository.GetUserById(id);
            List<Story> stories = storyRepository.GetStoriesByUserID(id);
            List<StoryVM> storyVMs = new List<StoryVM>();
            foreach (Story item in stories)
            {
                StoryVM storyVM = storyVMRepository.ConvertStoryToStoryModel(item);
                storyVM.User = userVMRepository.ConvertUserToUserModel(user);
                storyVMs.Add(storyVM);
            }
            return View(storyVMs);
        }
        public IActionResult Deactivate()
        {
            int id = (int)HttpContext.Session.GetInt32("Login");
            userRepository.DeactivateUser(id);
            HttpContext.Session.Remove("Login");
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Index", "Home");
        }
    }
}
