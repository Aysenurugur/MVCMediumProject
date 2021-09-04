using MediumProject.EF_MediumProject_CodeFirst.Entities;
using MediumProject.EF_MediumProject_CodeFirst.Repository.Abstract;
using MediumProject.Models;
using MediumProject.Models.Model_Repository.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MediumProject.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IStoryRepository storyRepository, IUserRepository userRepository, ITopicRepository topicRepository, IUserVMRepository userVMRepository, IStoryVMRepository storyVMRepository, ITopicVMRepository topicVMRepository)
        {
            this.storyRepository = storyRepository;
            this.userRepository = userRepository;
            this.topicRepository = topicRepository;
            this.userVMRepository = userVMRepository;
            this.storyVMRepository = storyVMRepository;
            this.topicVMRepository = topicVMRepository;
        }
        #region Interfaces
        IStoryRepository storyRepository;
        IUserRepository userRepository;
        ITopicRepository topicRepository;
        IUserVMRepository userVMRepository;
        IStoryVMRepository storyVMRepository;
        ITopicVMRepository topicVMRepository;
        #endregion

        public IActionResult Index()
        {
            List<Story> stories = new List<Story>();
            int? id = HttpContext.Session.GetInt32("Login");
            if (id == null)
            {
                stories = storyRepository.GetMostPopularStories();
            }
            else
            {
                if (topicRepository.GetFavouriteTopicsByUserID((int)id).Count == 0) //User cannot view the homepage without choosing topics first
                {
                    return RedirectToAction("SelectTopics", "Topic");
                }

                foreach (int item in storyRepository.GetUserStoriesByFavouriteTopics((int)id))
                {
                    Story story = storyRepository.GetStoryById(item);
                    if (story.IsActive)
                    {
                        stories.Add(story);
                    }
                }
            }
            List<StoryVM> storyVMs = new List<StoryVM>();
            foreach (Story item in stories)
            {
                User user = userRepository.GetUserById(item.UserID);
                StoryVM storyVM = storyVMRepository.ConvertStoryToStoryModel(item);
                storyVM.User = userVMRepository.ConvertUserToUserModel(user);
                storyVMs.Add(storyVM);
            }
            ViewBag.Topics = topicVMRepository.ConvertTopicListToTopicModelList(topicRepository.GetTopics());
            return View(storyVMs);
        }
        public IActionResult Information()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
