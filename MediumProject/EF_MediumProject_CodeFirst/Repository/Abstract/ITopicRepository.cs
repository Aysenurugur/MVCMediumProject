using MediumProject.EF_MediumProject_CodeFirst.Entities;
using System.Collections.Generic;

namespace MediumProject.EF_MediumProject_CodeFirst.Repository.Abstract
{
    public interface ITopicRepository
    {
        Topic GetTopicById(int topicId);
        List<Topic> GetTopics();
        bool AddFavouriteTopicsToUsers(int userId, List<Topic> topics);
        void AddTopicsToStory(int storyId, List<Topic> topics);
        List<Topic> GetFavouriteTopicsByUserID(int userId);
        List<Topic> GetTopicsByStoryId(int storyId);
    }
}
