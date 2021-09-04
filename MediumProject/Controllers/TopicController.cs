using MediumProject.EF_MediumProject_CodeFirst.Entities;
using MediumProject.EF_MediumProject_CodeFirst.Repository.Abstract;
using MediumProject.Models;
using MediumProject.Models.Model_Repository.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MediumProject.Controllers
{
    public class TopicController : Controller
    {
        public TopicController(IUserRepository userRepository, ITopicRepository topicRepository, ITopicVMRepository topicVMRepository, IStoryRepository storyRepository, IStoryVMRepository storyVMRepository)
        {
            this.userRepository = userRepository;
            this.topicRepository = topicRepository;
            this.topicVMRepository = topicVMRepository;
            this.storyRepository = storyRepository;
            this.storyVMRepository = storyVMRepository;
        }
        #region Interfaces
        IUserRepository userRepository;
        ITopicRepository topicRepository;
        ITopicVMRepository topicVMRepository;
        IStoryRepository storyRepository;
        IStoryVMRepository storyVMRepository;
        #endregion

        public IActionResult SelectTopics()
        {
            if (HttpContext.Session.GetInt32("Login") == null)  //User cannot view this page withour logging in       
                return RedirectToAction("Index", "Home");

            List<TopicVM> topics = topicVMRepository.ConvertTopicListToTopicModelList(topicRepository.GetTopics());
            return View(topics);
        }
        public IActionResult AddTopics(int[] ids)
        {
            List<Topic> newTopics = topicVMRepository.ConvertTopicModelListToTopicList(ids.ToList());
            int id = (int)HttpContext.Session.GetInt32("Login");
            bool check = topicRepository.AddFavouriteTopicsToUsers(id, newTopics);
            if (check)
            {
                return RedirectToAction(nameof(Index), "Home");
            }
            return View();
        }
        public IActionResult Stories(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Error", "Home");
            }
            Topic topic = topicRepository.GetTopicById(id);
            ViewBag.TopicName = topic.Title;
            List<Story> stories = storyRepository.GetStoriesByTopic(id);
            List<StoryVM> storyVMs = new List<StoryVM>();
            foreach (Story item in stories)
            {
                storyVMs.Add(storyVMRepository.ConvertStoryToStoryModel(item));
            }
            return View(storyVMs);
        }
    }
}
