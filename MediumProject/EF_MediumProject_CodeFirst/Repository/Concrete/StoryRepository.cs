using MediumProject.EF_MediumProject_CodeFirst.Context;
using MediumProject.EF_MediumProject_CodeFirst.Entities;
using MediumProject.EF_MediumProject_CodeFirst.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MediumProject.EF_MediumProject_CodeFirst.Repository.Concrete
{
    public class StoryRepository : IStoryRepository
    {
        public StoryRepository(MediumContext context, ITopicRepository topicRepository)
        {
            this.context = context;
            this.topicRepository = topicRepository;
        }
        MediumContext context;
        ITopicRepository topicRepository;

        public Story GetStoryById(int storyId)
        {
            return context.Stories.Find(storyId);
        }
        public bool AddNewStory(Story story, List<Topic> topics)
        {
            try
            {
                story.ReadTimeInMinutes = Convert.ToInt16(story.Description.Length / 2000);
                story.CreatedDate = DateTime.Now;
                story.ClickCount = 0;
                story.IsActive = true;
                context.Stories.Add(story);
                topicRepository.AddTopicsToStory(GetLastStoryID(), topics);
                return context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateStory(Story story, List<Topic> topics)
        {
            try
            {
                Story newStory = context.Stories.Find(story.StoryID);
                newStory.Title = story.Title;
                newStory.Description = story.Description;
                newStory.ModifiedDate = DateTime.Now;
                story.ReadTimeInMinutes = Convert.ToInt16(story.Description.Length / 2000);
                topicRepository.AddTopicsToStory(story.StoryID, topics);
                return context.SaveChanges() > 0;
        }
            catch (Exception)
            {
                return false;
            }
}
        public void RemoveStory(int storyId)
        {
            Story story = GetStoryById(storyId);
            story.IsActive = false;
            context.SaveChanges();
        }
        public List<Story> GetStoriesByUserID(int userId)
        {
            List<Story> stories = context.Stories.Where(x => x.UserID == userId && x.IsActive).OrderByDescending(x=>x.CreatedDate).ToList();
            return stories;
        }
        public List<Story> GetStoriesByTopic(int topicId)
        {
            List<int> storyTopics = context.StoryTopics.Where(x => x.TopicID == topicId).Select(x => x.StoryID).ToList();
            List<Story> stories = new List<Story>();
            foreach (int item in storyTopics)
            {
                Story story = context.Stories.Find(item);
                if (story.IsActive)
                {
                    stories.Add(story);
                }
            }

            return stories;
        }
        public List<Story> GetMostPopularStories()
        {
            List<Story> stories = context.Stories.Where(x => x.IsActive).OrderByDescending(x => x.ClickCount).Take(10).ToList();
            return stories;
        }
        public List<int> GetUserStoriesByFavouriteTopics(int userId)
        {
            List<Topic> topics = topicRepository.GetFavouriteTopicsByUserID(userId);
            List<int> stories = new List<int>();
            foreach (Topic topic in topics)
            {
                foreach (StoryTopic storyTopic in context.StoryTopics)
                {
                    if (topic.TopicID == storyTopic.TopicID)
                    {
                        stories.Add(storyTopic.StoryID);
                    }
                }
            }

            return stories;
        }
        public int GetLastStoryID()
        {
            int id = context.Stories.OrderByDescending(x => x.CreatedDate).Select(x => x.StoryID).FirstOrDefault() + 1;
            return id;
        }
        public void ReadStory(int storyId)
        {
            Story story = GetStoryById(storyId);
            story.ClickCount++;
            context.SaveChanges();
        }
    }
}
