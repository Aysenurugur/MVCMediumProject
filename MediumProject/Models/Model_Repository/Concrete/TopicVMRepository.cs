using MediumProject.EF_MediumProject_CodeFirst.Entities;
using MediumProject.EF_MediumProject_CodeFirst.Repository.Abstract;
using MediumProject.Models.Model_Repository.Abstract;
using System.Collections.Generic;

namespace MediumProject.Models.Model_Repository.Concrete
{
    public class TopicVMRepository : ITopicVMRepository
    {
        public TopicVMRepository(ITopicRepository topicRepository)
        {
            this.topicRepository = topicRepository;
        }
        ITopicRepository topicRepository;

        public List<TopicVM> ConvertTopicListToTopicModelList(List<Topic> topics)
        {
            List<TopicVM> topicVMs = new List<TopicVM>();
            foreach (Topic item in topics)
            {
                topicVMs.Add(ConvertTopicToTopicModel(item));
            }
            return topicVMs;
        }
        public TopicVM ConvertTopicToTopicModel(Topic topic)
        {
            TopicVM topicVM = new TopicVM()
            {
                TopicID = topic.TopicID,
                Title = topic.Title,
                Description = topic.Description
            };
            return topicVM;
        }
        public List<Topic> ConvertTopicModelListToTopicList(List<int> topicVMs)
        {
            List<Topic> topics = new List<Topic>();
            foreach (int item in topicVMs)
            {
                Topic topic = topicRepository.GetTopicById(item);
                if (!topics.Contains(topic))
                {
                    topics.Add(topicRepository.GetTopicById(item));
                }
            }
            return topics;
        }

        public List<Topic> ConvertTopicModelListToTopicList(List<TopicVM> topicVMs)
        {
            List<Topic> topics = new List<Topic>();
            foreach (TopicVM item in topicVMs)
            {
                topics.Add(topicRepository.GetTopicById(item.TopicID));
            }
            return topics;
        }
    }
}
