using MediumProject.EF_MediumProject_CodeFirst.Context;
using MediumProject.EF_MediumProject_CodeFirst.Entities;
using MediumProject.EF_MediumProject_CodeFirst.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MediumProject.EF_MediumProject_CodeFirst.Repository.Concrete
{
    public class TopicRepository : ITopicRepository
    {
        public TopicRepository(MediumContext context)
        {
            this.context = context;
        }
        MediumContext context;

        public Topic GetTopicById(int topicId)
        {
            return context.Topics.Find(topicId);
        }
        public List<Topic> GetTopics()
        {
            return context.Topics.OrderBy(x => x.Title).ToList();
        }
        public bool AddFavouriteTopicsToUsers(int userId, List<Topic> topics)
        {
            try
            {
                foreach (Topic item in topics)
                {
                    UserTopic userTopic = context.UserTopics.Where(x => x.UserID == userId && x.TopicID == item.TopicID).FirstOrDefault();
                    if (userTopic == null)
                    {
                        context.UserTopics.Add(new UserTopic()
                        {
                            TopicID = item.TopicID,
                            UserID = userId
                        });
                    }
                }
                return context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void AddTopicsToStory(int storyId, List<Topic> topics)
        {
            foreach (Topic item in topics)
            {
                StoryTopic storyTopic = context.StoryTopics.Where(x => x.StoryID == storyId && x.TopicID == item.TopicID).FirstOrDefault();
                if (storyTopic == null)
                {
                    context.StoryTopics.Add(new StoryTopic()
                    {
                        TopicID = item.TopicID,
                        StoryID = storyId
                    });
                }
            }
        }
        public List<Topic> GetFavouriteTopicsByUserID(int userId)
        {
            List<UserTopic> userTopics = context.UserTopics.Where(x => x.UserID == userId).ToList();
            List<Topic> topics = new List<Topic>();
            foreach (UserTopic item in userTopics)
            {
                Topic topic = context.Topics.Find(item.TopicID);
                if (!topics.Contains(topic))
                {
                    topics.Add(topic);
                }
            }
            return topics;
        }
        public List<Topic> GetTopicsByStoryId(int storyId)
        {
            List<StoryTopic> storyTopics = context.StoryTopics.Where(x => x.StoryID == storyId).ToList();
            List<Topic> topics = new List<Topic>();
            foreach (StoryTopic item in storyTopics)
            {
                topics.Add(GetTopicById(item.TopicID));
            }
            return topics;
        }
    }
}
