using MediumProject.EF_MediumProject_CodeFirst.Entities;
using MediumProject.EF_MediumProject_CodeFirst.Repository.Abstract;
using MediumProject.Models;
using MediumProject.Models.Model_Repository.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MediumProject.Controllers
{
    public class StoryController : Controller
    {
        public StoryController(IStoryRepository storyRepository, IUserRepository userRepository, ITopicRepository topicRepository,
            ITopicVMRepository topicVMRepository, IStoryVMRepository storyVMRepository)
        {
            this.storyRepository = storyRepository;
            this.userRepository = userRepository;
            this.topicRepository = topicRepository;
            this.topicVMRepository = topicVMRepository;
            this.storyVMRepository = storyVMRepository;
        }
        #region Interfaces
        IStoryRepository storyRepository;
        IUserRepository userRepository;
        ITopicRepository topicRepository;
        ITopicVMRepository topicVMRepository;
        IStoryVMRepository storyVMRepository;
        #endregion

        public IActionResult ReadStory(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Error", "Home");
            }
            Story story = storyRepository.GetStoryById(id);
            if (story.UserID != HttpContext.Session.GetInt32("Login")) //kullanıcı kendi hikayesine click count ekleyemesin diye
            {
                storyRepository.ReadStory(id);
            }
            StoryVM storyVM = storyVMRepository.ConvertStoryToStoryModel(story);
            List<Topic> topics = topicRepository.GetTopicsByStoryId(id);
            ViewBag.StoryTopics = topicVMRepository.ConvertTopicListToTopicModelList(topics);
            return View(storyVM);
        }
        public IActionResult WriteOrUpdate(int? id = null)
        {
            if (HttpContext.Session.GetInt32("Login") == null)  //User cannot view this page withour logging in       
                return RedirectToAction("Index", "Home");
            ViewBag.Topics = topicVMRepository.ConvertTopicListToTopicModelList(topicRepository.GetTopics());
            if (id != null)
            {
                Story story = storyRepository.GetStoryById((int)id);
                ViewBag.Story = storyVMRepository.ConvertStoryToStoryModel(story);
            }
            return View();
        }
        public IActionResult GetStory(StoryWithTopics storyWithTopics)
        {
            User user = userRepository.GetUserById((int)HttpContext.Session.GetInt32("Login"));
            storyWithTopics.Story.UserID = user.UserID;
            List<Topic> topics = new List<Topic>();

            if (storyWithTopics.Topics != null)
            {
                topics = topicVMRepository.ConvertTopicModelListToTopicList(storyWithTopics.Topics.ToList());
            }

            if (storyWithTopics.Story.StoryID == 0)
            {
                Story story = storyVMRepository.ConvertStoryModelToStory(storyWithTopics.Story);
                storyRepository.AddNewStory(story, topics);
            }
            else
            {
                Story story = storyVMRepository.ConvertStoryModelToStory(storyWithTopics.Story);
                storyRepository.UpdateStory(story, topics);
            }
            return RedirectToAction("MyStories", "User");
        }
        public IActionResult Delete(int id)
        {
            storyRepository.RemoveStory(id);
            return RedirectToAction("MyStories", "User");
        }

    }
}
