using MediumProject.EF_MediumProject_CodeFirst.Entities;
using System.Collections.Generic;

namespace MediumProject.EF_MediumProject_CodeFirst.Repository.Abstract
{
    public interface IStoryRepository
    {
        Story GetStoryById(int storyId);
        bool AddNewStory(Story story, List<Topic> topics);
        bool UpdateStory(Story story, List<Topic> topics);
        void RemoveStory(int storyId);
        List<Story> GetStoriesByUserID(int userId);
        List<Story> GetStoriesByTopic(int topicId);
        List<Story> GetMostPopularStories();
        List<int> GetUserStoriesByFavouriteTopics(int userId);
        int GetLastStoryID();
        void ReadStory(int storyId);
    }
}
